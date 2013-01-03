using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Interpretation;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels.Enums;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Coding
{
    /// <summary>Represents an MVE coder for video pixel data that is palette-based</summary>
    public class VideoCoding8Bit : VideoCodingBase
    {
        #region Fields
        /// <summary>Palette for data reference</summary>
        protected Palette PalatteData { get; set; }

        /// <summary>Represents the video frame prior to the most recent decoded frame, pivoted into the next frame</summary>
        protected PixelData PivotFrame { get; set; }

        /// <summary>Represents the current frame number</summary>
        public Int32 FrameNumber { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the size of the PixelData buffer needed</summary>
        protected override Int32 BufferSize
        {
            get { return this.Height * this.Width; }
        }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="width">Width of the video data, in pixels</param>
        /// <param name="height">Height of the video data, in pixels</param>
        /// <param name="videoStream">MveVideoFrame collection of frames</param>
        public VideoCoding8Bit(Int32 width, Int32 height, VideoOpcodeStream videoStream) : base(width, height, videoStream)
        {
            this.FrameNumber = 0;
        }
        #endregion


        #region Frame Exposure
        /// <summary>Gets the next frame from the video stream</summary>
        /// <param name="mveFrame">MveVideoFrame to decode</param>
        /// <returns>A MediaBase Frame</returns>
        public override IMultimediaImageFrame GetNextFrame(MveVideoFrame mveFrame)
        {
            IMultimediaImageFrame frame = null;

            if (mveFrame != null)
            {
                MemoryStream previousData = (this.RecentFrame != null) ? this.RecentFrame.NativeBinaryData : null;
                Byte[] previousBinary = (previousData != null) ? previousData.ToArray() : new Byte[this.BufferSize];
                PixelData pd = this.GetNextImage(previousBinary, mveFrame.DecodingMap.BlockEncoding, mveFrame.Data.Data, this.VideoStream.Palette, mveFrame.Data.DeltaFrame);

                //remember the current pixel data for the next frame; otherwise the movie gets very blocky (but interrestingly you can see the delta regions)
                if (mveFrame.Data.DeltaFrame)
                    this.PivotFrame = this.RecentFrame.Clone();

                this.RecentFrame = pd;

                //DEBUG
                //using (System.IO.FileStream fs = new System.IO.FileStream("\\Test Data\\Infinity Engine\\Problem\\MVE\\iep1\\iep1_" + this.FrameNumber.ToString() + ".raw", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
                //    fs.Write(pd.NativeBinaryData, 0, pd.NativeBinaryData.Length);
                
                ++this.FrameNumber;

                frame = new BasicImageFrame(pd);
            }

            return frame;
        }

        /// <summary>Gets a buffer for the next image being prepared</summary>
        /// <param name="isDeltaFrame">Flag indicating whether to reuse the pivot buffer or to create a new one</param>
        /// <returns>A Byte Array buffer</returns>
        protected override Byte[] GetNextImageBuffer(Boolean isDeltaFrame)
        {
            Byte[] buffer = null;
            if (this.PivotFrame == null || !isDeltaFrame)
                buffer = new Byte[this.BufferSize];
            else //PivotFrame != null && isDeltaFrame
                buffer = this.PivotFrame.NativeBinaryData.ToArray();

            return buffer;
        }
        #endregion


        #region Coder Commands
        /// <summary>Resets the video stream to the beginning</summary>
        public override void ResetVideo()
        {
            base.ResetVideo();
            this.PivotFrame = null;
            this.FrameNumber = 0;
        }
        #endregion


        #region Video Decode operations
        /// <summary>Copies one block's data from the previous frame</summary>
        /// <param name="blockX">Block X index to copy</param>
        /// <param name="blockY">Block Y index to copy</param>
        /// <param name="previousFrame">Previous frame's binary data</param>
        /// <param name="outputData">New frame's binary data</param>
        /// <remarks>Opcode 0</remarks>
        protected override void DecodeCopyBlock(Int32 blockX, Int32 blockY, Byte[] previousFrame, Byte[] outputData)
        {
            Int32 srcY = blockY * 8;
            Int32 srcX = blockX * 8;

            for (Int32 y = srcY; y < (srcY + 8); ++y)
                Array.Copy(previousFrame, ((y * this.Width) + srcX), outputData, ((y * this.Width) + srcX), 8);
        }

        ///<remarks>Read source is the new frame</remarks>
        /// <remarks>Opcode 2</remarks>
        protected override void DecodeCopyNewFrameBottom(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            this.HelperDecpdeCopyNewFrameRelative(blockX, blockY, data, ref dataPosition, outputData, 1);
        }

        ///<remarks>Read source is the new frame</remarks>
        /// <remarks>Opcode 3</remarks>
        protected override void DecodeCopyNewFrameTop(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            this.HelperDecpdeCopyNewFrameRelative(blockX, blockY, data, ref dataPosition, outputData, -1);
        }

        /// <remarks>Opcode 4</remarks>
        protected override void DecodeCopyPreviousFrameClose(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] previousFrame, Byte[] outputData)
        {
            Byte offsets = data[dataPosition];
            ++dataPosition;

            Int32 relOffsetX = (offsets & 0x0F) - 8;
            Int32 relOffsetY = (offsets >> 4) - 8;

            this.HelperDecodeComputeCopyPreviousFrameRelative(previousFrame, outputData, blockX, blockY, relOffsetX, relOffsetY);
        }

        /// <remarks>Opcode 5</remarks>
        protected override void DecodeCopyPreviousFrameDistant(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] previousFrame, Byte[] outputData)
        {
            SByte relOffsetX = (SByte)data[dataPosition];
            SByte relOffsetY = (SByte)data[dataPosition + 1];
            dataPosition += 2;
            this.HelperDecodeComputeCopyPreviousFrameRelative(previousFrame, outputData, blockX, blockY, relOffsetX, relOffsetY);
        }

        /// <remarks>Opcode 7</remarks>
        protected override void DecodeTwoToneIndecesRowOrSquare(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            Byte paletteIndex0 = data[dataPosition];
            Byte paletteIndex1 = data[dataPosition + 1];
            dataPosition += 2;

            if (paletteIndex0 > paletteIndex1)
                this.HelperDecodeTwoToneIndecesBySquare(data, ref dataPosition, blockX, blockY, outputData, paletteIndex0, paletteIndex1);
            else
                this.HelperDecodeTwoToneIndecesByRow(data, ref dataPosition, blockX, blockY, outputData, paletteIndex0, paletteIndex1);
        }

        /// <remarks>Opcode 8</remarks>
        protected override void DecodeTwoToneIndecesQuadrantsOrHalves(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            Byte paletteIndex0 = data[dataPosition];
            Byte paletteIndex1 = data[dataPosition + 1];
            dataPosition += 2;

            if (paletteIndex0 > paletteIndex1)  //halves
            {
                //peek (do not consume) to determine which plane to split halves on
                Byte paletteIndex2 = data[dataPosition + 4];
                Byte paletteIndex3 = data[dataPosition + 5];

                if (paletteIndex2 > paletteIndex3)  //top, bottom
                    this.HelperDecodeTwoToneIndecesVerticalHalves(data,ref dataPosition, blockX, blockY, outputData, paletteIndex0, paletteIndex1);
                else    //left, right
                    this.HelperDecodeTwoToneIndecesHorizontalHalves(data, ref dataPosition, blockX, blockY, outputData, paletteIndex0, paletteIndex1);
            }
            else    //quadrants
                this.HelperDecodeTwoToneIndecesQuadrants(data, ref dataPosition, blockX, blockY, outputData, paletteIndex0, paletteIndex1);
        }

        /// <remarks>Opcode 9</remarks>
        protected override void DecodeFourColorIndecesRowsOrRectangles(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            Byte[] paletteIndeces = new Byte[4];
            Array.Copy(data, dataPosition, paletteIndeces, 0, 4);
            dataPosition += 4;

            if (paletteIndeces[0] > paletteIndeces[1])
            {
                if (paletteIndeces[2] > paletteIndeces[3])  //1x2 rows
                    this.HelperDecodeFourColorIndecesVerticalRectangles(data, ref dataPosition, blockX, blockY, outputData, paletteIndeces);
                else    //2x1 rows
                    this.HelperDecodeFourColorIndecesHorizontalRectangles(data, ref dataPosition, blockX, blockY, outputData, paletteIndeces);
            }
            else
            {
                if (paletteIndeces[2] > paletteIndeces[3])  //2x2 rows
                    this.HelperDecodeFourColorIndecesSquares(data, ref dataPosition, blockX, blockY, outputData, paletteIndeces);
                else    //1x1 rows
                    this.HelperDecodeFourColorIndecesEightRows(data, ref dataPosition, blockX, blockY, outputData, paletteIndeces);
            }
        }

        /// <remarks>Opcode 10</remarks>
        protected override void DecodeFourColorIndecesQuadrantsOrHalves(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            Byte[] paletteIndeces = new Byte[4];
            Array.Copy(data, dataPosition, paletteIndeces, 0, 4);
            dataPosition += 4;

            if (paletteIndeces[0] > paletteIndeces[1])  //halves
            {
                //peek (do not consume) to determine which plane to split halves on
                Byte paletteIndex4 = data[dataPosition + 8];
                Byte paletteIndex5 = data[dataPosition + 9];
                
                if (paletteIndex4 > paletteIndex5)  //top, bottom
                    this.HelperDecodeFourColorIndecesVerticalHalves(data, ref dataPosition, blockX, blockY, outputData, paletteIndeces);
                else    //left, right
                    this.HelperDecodeFourColorIndecesHorizontalHalves(data, ref dataPosition, blockX, blockY, outputData, paletteIndeces);
            }
            else    //quadrants
                this.HelperDecodeFourColorIndecesQuadrants(data, ref dataPosition, blockX, blockY, outputData, paletteIndeces);
        }

        /// <remarks>Opcode 11</remarks>
        protected override void DecodeRawIndeces(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;

            for (Int32 y = 0; y < 8; ++y)
            {
                Int32 offset = ((positionY + y) * this.Width) + positionX;
                Array.Copy(data, dataPosition, outputData, offset, 8);
                dataPosition += 8;
            }
        }

        /// <remarks>Opcode 12</remarks>
        protected override void DecodeRawIndeces2PixelSquares(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;

            for (Int32 y = 0; y < 8; y += 2)
            {
                Int32 offset1 = ((positionY + y) * this.Width) + positionX;
                Int32 offset2 = ((positionY + y + 1) * this.Width) + positionX;

                for (Int32 x = 0; x < 8; x += 2)
                {
                    Byte index = data[dataPosition];
                    ++dataPosition;

                    outputData[offset1 + x] = index;
                    outputData[offset1 + x + 1] = index;
                    outputData[offset2 + x] = index;
                    outputData[offset2 + x + 1] = index;
                }
            }
        }

        /// <remarks>Opcode 13</remarks>
        protected override void DecodeRawIndeces4PixelSquares(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;

            for (Int32 y = 0; y < 8; y += 4)
            {
                Int32 offset1 = ((positionY + y) * this.Width) + positionX;
                Int32 offset2 = ((positionY + y + 1) * this.Width) + positionX;
                Int32 offset3 = ((positionY + y + 2) * this.Width) + positionX;
                Int32 offset4 = ((positionY + y + 3) * this.Width) + positionX;

                for (Int32 x = 0; x < 8; x += 4)
                {
                    Byte index = data[dataPosition];
                    ++dataPosition;

                    outputData[offset1 + x] = index;
                    outputData[offset1 + x + 1] = index;
                    outputData[offset1 + x + 2] = index;
                    outputData[offset1 + x + 3] = index;
                    outputData[offset2 + x] = index;
                    outputData[offset2 + x + 1] = index;
                    outputData[offset2 + x + 2] = index;
                    outputData[offset2 + x + 3] = index;
                    outputData[offset3 + x] = index;
                    outputData[offset3 + x + 1] = index;
                    outputData[offset3 + x + 2] = index;
                    outputData[offset3 + x + 3] = index;
                    outputData[offset4 + x] = index;
                    outputData[offset4 + x + 1] = index;
                    outputData[offset4 + x + 2] = index;
                    outputData[offset4 + x + 3] = index;
                }
            }
        }

        /// <remarks>Opcode 14</remarks>
        protected override void DecodeRawIndecesSolidColor(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;

            Byte index = data[dataPosition];
            ++dataPosition;

            for (Int32 y = 0; y < 8; ++y)
            {
                Int32 offset = ((positionY + y) * this.Width) + positionX;

                for (Int32 x = 0; x < 8; ++x)
                    outputData[offset + x] = index;
            }
        }

        /// <remarks>Opcode 15</remarks>
        protected override void DecodeDither2Colors(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;

            Byte paleetteIndex0 = data[dataPosition];
            Byte paleetteIndex1 = data[dataPosition + 1];
            dataPosition += 2;

            this.HelperDecodeRawIndecesDitherColorsRow(positionX, positionY, outputData, paleetteIndex0, paleetteIndex1);
            this.HelperDecodeRawIndecesDitherColorsRow(positionX, positionY + 1, outputData, paleetteIndex1, paleetteIndex0);
            this.HelperDecodeRawIndecesDitherColorsRow(positionX, positionY + 2, outputData, paleetteIndex0, paleetteIndex1);
            this.HelperDecodeRawIndecesDitherColorsRow(positionX, positionY + 3, outputData, paleetteIndex1, paleetteIndex0);
            this.HelperDecodeRawIndecesDitherColorsRow(positionX, positionY + 4, outputData, paleetteIndex0, paleetteIndex1);
            this.HelperDecodeRawIndecesDitherColorsRow(positionX, positionY + 5, outputData, paleetteIndex1, paleetteIndex0);
            this.HelperDecodeRawIndecesDitherColorsRow(positionX, positionY + 6, outputData, paleetteIndex0, paleetteIndex1);
            this.HelperDecodeRawIndecesDitherColorsRow(positionX, positionY + 7, outputData, paleetteIndex1, paleetteIndex0);
        }
        #endregion


        #region Helper Methods
        /// <summary>Operation 2, 3 helper. Copies the current frame's data from the buffer of older frame data.</summary>
        /// <param name="sign">Must be either -1 or 1</param>
        protected virtual void HelperDecpdeCopyNewFrameRelative(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData, Int32 sign)
        {
            //error detection
            switch (sign)
            {
                case 1:
                case -1:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("sign", "Parameter \"sign\" must be either -1 or 1.");
            }

            Int32 x, y;
            this.HelperDecodeComputeCopyNewFrameBottomTopOffsets(data, ref dataPosition, out x, out y);

            Int32 destX = (blockX * 8);
            Int32 destY = (blockY * 8);
            Int32 srcX = (blockX * 8) + (x * sign);
            Int32 srcY = (blockY * 8) + (y * sign);

            //copy
            for (Int32 yPos = srcY; yPos < (srcY + 8); ++yPos)
            {
                Array.Copy(outputData, ((yPos * this.Width) + srcX), outputData, ((destY * this.Width) + destX), 8);
                ++destY;
            }
        }

        protected virtual void HelperDecodeComputeCopyNewFrameBottomTopOffsets(Byte[] data, ref Int64 dataPosition, out Int32 offsetX, out Int32 offsetY)
        {
            Byte offsets = data[dataPosition];
            ++dataPosition;

            if (offsets < 56)
            {
                offsetX = 8 + (offsets % 7);
                offsetY = offsets / 7;
            }
            else
            {
                offsetX = -14 + ((offsets - 56) % 29);
                offsetY = 8 + ((offsets - 56) / 29);
            }
        }

        protected virtual void HelperDecodeComputeCopyPreviousFrameRelative(Byte[] origin, Byte[] destination, Int32 blockX, Int32 blockY, Int32 relX, Int32 relY)
        {
            Int32 blockYPos = blockY * 8;
            Int32 blockXPos = blockX * 8;

            Int32 srcX = blockXPos + relX;
            Int32 srcY = blockYPos + relY;

            //copy
            for (Int32 yPos = srcY; yPos < (srcY + 8); ++yPos)
            {
                Array.Copy(origin, ((yPos * this.Width) + srcX), destination, ((blockYPos * this.Width) + blockXPos), 8);
                ++blockYPos;
            }
        }

        #region Two-Tone helpers
        protected virtual void HelperDecodeTwoToneIndecesByRow(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, Byte paletteIndex0, Byte paletteIndex1)
        {
            Int32 blockYPos = blockY * 8;
            Int32 blockXPos = blockX * 8;

            for (Int32 y = 0; y < 8; ++y)
                this.HelperDecodeTwoToneIndecesBlockRow(data, ref dataPosition, destination, paletteIndex0, paletteIndex1, blockXPos, blockYPos + y);
        }

        protected virtual void HelperDecodeTwoToneIndecesBlockRow(Byte[] data, ref Int64 dataPosition, Byte[] destination, Byte paletteIndex0, Byte paletteIndex1, Int32 destX, Int32 destY)
        {
            Byte row = data[dataPosition];
            ++dataPosition;

            //do all 8 setting operations
            for (Int32 x = 0; x < 8; ++x)
            {
                Int32 offset = (destY * this.Width) + (destX + x);

                if ((row & 0x01) == 0)
                    destination[offset] = paletteIndex0;
                else
                    destination[offset] = paletteIndex1;

                row >>= 1;
            }
        }

        /// <summary>Decodes block as 2-square pixels, two bytes needed for the stream</summary>
        /// <remarks>
        ///     4 bits needed for eeach pair of rows; 0xF0:
        ///     11 11 11 11
        ///     11 11 11 11
        ///     00 00 00 00
        ///     00 00 00 00
        /// </remarks>
        protected virtual void HelperDecodeTwoToneIndecesBySquare(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, Byte paletteIndex0, Byte paletteIndex1)
        {
            Int32 blockYPos = blockY * 8;
            Int32 blockXPos = blockX * 8;

            for (Int32 byteCount = 0; byteCount < 2; ++byteCount)
            {
                Byte halfBlock = data[dataPosition];
                ++dataPosition;

                Int32 rows1 = (halfBlock & 0x0F);
                Int32 rows2 = (halfBlock >> 4);

                this.HelperDecodeTwoToneIndecesBySquareSubRows(blockXPos, blockYPos, destination, paletteIndex0, paletteIndex1, rows1);
                blockYPos += 2;
                this.HelperDecodeTwoToneIndecesBySquareSubRows(blockXPos, blockYPos, destination, paletteIndex0, paletteIndex1, rows2);
                blockYPos += 2;
            }
        }

        protected virtual void HelperDecodeTwoToneIndecesBySquareSubRows(Int32 destX, Int32 destY, Byte[] destination, Byte paletteIndex0, Byte paletteIndex1, Int32 subRowMap)
        {
            //do this 4 times
            for (Int32 bit = 0; bit < 4; ++bit)
            {
                Byte pixel = ((subRowMap & 0x01) == 0) ? paletteIndex0 : paletteIndex1;

                //apply this to the appropriate 4 pixels
                Int32 square = (bit * 2);   //0,2,4,6

                Int32 offset1 = (destY * this.Width)            + (destX + square);
                Int32 offset2 = ((destY + 1) * this.Width)      + (destX + square);

                destination[offset1] = pixel;
                destination[offset1 + 1] = pixel;
                destination[offset2] = pixel;
                destination[offset2 + 1] = pixel;

                subRowMap >>= 1;
            }
        }

        /// <remarks>Sets quadrants of an 8x8 block, top left, top right, bottom left, bottom right with two bytes per quadrant, one bit per pixel</remarks>
        protected virtual void HelperDecodeTwoToneIndecesQuadrants(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, Byte paletteIndex0, Byte paletteIndex1)
        {
            Int32 blockXPos = blockX * 8;
            Int32 blockYPos = blockY * 8;

            //top left
            Byte datum0 = data[dataPosition];
            Byte datum1 = data[dataPosition + 1];
            dataPosition += 2;
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrant(blockXPos, blockYPos, destination, paletteIndex0, paletteIndex1, datum0, datum1);
            
            //bottom left
            paletteIndex0 = data[dataPosition];
            paletteIndex1 = data[dataPosition + 1];
            datum0 = data[dataPosition + 2];
            datum1 = data[dataPosition + 3];
            dataPosition += 4;
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrant(blockXPos, blockYPos + 4, destination, paletteIndex0, paletteIndex1, datum0, datum1);
            
            //top right
            paletteIndex0 = data[dataPosition];
            paletteIndex1 = data[dataPosition + 1];
            datum0 = data[dataPosition + 2];
            datum1 = data[dataPosition + 3];
            dataPosition += 4;
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrant(blockXPos + 4, blockYPos, destination, paletteIndex0, paletteIndex1, datum0, datum1);

            //bottom right
            paletteIndex0 = data[dataPosition];
            paletteIndex1 = data[dataPosition + 1];
            datum0 = data[dataPosition + 2];
            datum1 = data[dataPosition + 3];
            dataPosition += 4;
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrant(blockXPos + 4, blockYPos + 4, destination, paletteIndex0, paletteIndex1, datum0, datum1);
        }

        /// <remarks>Sets quadrants of an 8x8 block, top left, top right, bottom left, bottom right with two bytes per quadrant, one bit per pixel</remarks>
        protected virtual void HelperDecodeTwoToneIndecesQuadrantsSingleQuadrant(Int32 destX, Int32 destY, Byte[] destination, Byte paletteIndex0, Byte paletteIndex1, Int32 quadrantDatum0, Int32 quadrantDatum1)
        {
            Int32 row1 = (quadrantDatum0 & 0x0F);
            Int32 row2 = (quadrantDatum0 >> 4);
            Int32 row3 = (quadrantDatum1 & 0x0F);
            Int32 row4 = (quadrantDatum1 >> 4);

            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(destX, destY, destination, paletteIndex0, paletteIndex1, row1);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(destX, destY + 1, destination, paletteIndex0, paletteIndex1, row2);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(destX, destY + 2, destination, paletteIndex0, paletteIndex1, row3);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(destX, destY + 3, destination, paletteIndex0, paletteIndex1, row4);
        }

        /// <remarks>Sets quadrants of an 8x8 block, top left, top right, bottom left, bottom right with two bytes per quadrant, one bit per pixel</remarks>
        protected virtual void HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(Int32 destX, Int32 destY, Byte[] destination, Byte paletteIndex0, Byte paletteIndex1, Int32 subRow)
        {
            for (Int32 x = 0; x < 4; ++x)
            {
                Int32 offset = (destY * this.Width) + (destX + x);

                if ((subRow & 0x01) == 0)
                    destination[offset] = paletteIndex0;
                else
                    destination[offset] = paletteIndex1;

                subRow >>= 1;
            }
        }

        protected virtual void HelperDecodeTwoToneIndecesHorizontalHalves(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, Byte paletteIndex1, Byte paletteIndex2)
        {
            Int32 blockXPos = blockX * 8;
            Int32 blockYPos = blockY * 8;

            Int32 row1 = (data[dataPosition] & 0x0F);
            Int32 row2 = (data[dataPosition] >> 4);
            Int32 row3 = (data[dataPosition + 1] & 0x0F);
            Int32 row4 = (data[dataPosition + 1] >> 4);
            Int32 row5 = (data[dataPosition + 2] & 0x0F);
            Int32 row6 = (data[dataPosition + 2] >> 4);
            Int32 row7 = (data[dataPosition + 3] & 0x0F);
            Int32 row8 = (data[dataPosition + 3] >> 4);
            dataPosition += 4;

            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos, blockYPos, destination, paletteIndex1, paletteIndex2, row1);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos, blockYPos + 1, destination, paletteIndex1, paletteIndex2, row2);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos, blockYPos + 2, destination, paletteIndex1, paletteIndex2, row3);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos, blockYPos + 3, destination, paletteIndex1, paletteIndex2, row4);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos, blockYPos + 4, destination, paletteIndex1, paletteIndex2, row5);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos, blockYPos + 5, destination, paletteIndex1, paletteIndex2, row6);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos, blockYPos + 6, destination, paletteIndex1, paletteIndex2, row7);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos, blockYPos + 7, destination, paletteIndex1, paletteIndex2, row8);


            paletteIndex1 = data[dataPosition];
            paletteIndex2 = data[dataPosition + 1];
            dataPosition += 2;
            row1 = (data[dataPosition] & 0x0F);
            row2 = (data[dataPosition] >> 4);
            row3 = (data[dataPosition + 1] & 0x0F);
            row4 = (data[dataPosition + 1] >> 4);
            row5 = (data[dataPosition + 2] & 0x0F);
            row6 = (data[dataPosition + 2] >> 4);
            row7 = (data[dataPosition + 3] & 0x0F);
            row8 = (data[dataPosition + 3] >> 4);
            dataPosition += 4;

            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos + 4, blockYPos, destination, paletteIndex1, paletteIndex2, row1);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos + 4, blockYPos + 1, destination, paletteIndex1, paletteIndex2, row2);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos + 4, blockYPos + 2, destination, paletteIndex1, paletteIndex2, row3);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos + 4, blockYPos + 3, destination, paletteIndex1, paletteIndex2, row4);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos + 4, blockYPos + 4, destination, paletteIndex1, paletteIndex2, row5);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos + 4, blockYPos + 5, destination, paletteIndex1, paletteIndex2, row6);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos + 4, blockYPos + 6, destination, paletteIndex1, paletteIndex2, row7);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos + 4, blockYPos + 7, destination, paletteIndex1, paletteIndex2, row8);
        }

        protected virtual void HelperDecodeTwoToneIndecesVerticalHalves(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, Byte paletteIndex0, Byte paletteIndex1)
        {
            Int32 blockXPos = blockX * 8;
            Int32 blockYPos = blockY * 8;

            this.HelperDecodeTwoToneIndecesBlockRow(data, ref dataPosition, destination, paletteIndex0, paletteIndex1, blockXPos, blockYPos);
            this.HelperDecodeTwoToneIndecesBlockRow(data, ref dataPosition, destination, paletteIndex0, paletteIndex1, blockXPos, blockYPos + 1);
            this.HelperDecodeTwoToneIndecesBlockRow(data, ref dataPosition, destination, paletteIndex0, paletteIndex1, blockXPos, blockYPos + 2);
            this.HelperDecodeTwoToneIndecesBlockRow(data, ref dataPosition, destination, paletteIndex0, paletteIndex1, blockXPos, blockYPos + 3);

            paletteIndex0 = data[dataPosition];
            paletteIndex1 = data[dataPosition + 1];
            dataPosition += 2;
            this.HelperDecodeTwoToneIndecesBlockRow(data, ref dataPosition, destination, paletteIndex0, paletteIndex1, blockXPos, blockYPos + 4);
            this.HelperDecodeTwoToneIndecesBlockRow(data, ref dataPosition, destination, paletteIndex0, paletteIndex1, blockXPos, blockYPos + 5);
            this.HelperDecodeTwoToneIndecesBlockRow(data, ref dataPosition, destination, paletteIndex0, paletteIndex1, blockXPos, blockYPos + 6);
            this.HelperDecodeTwoToneIndecesBlockRow(data, ref dataPosition, destination, paletteIndex0, paletteIndex1, blockXPos, blockYPos + 7);
        }
        #endregion


        #region Four Color helpers
        protected virtual void HelperDecodeFourColorIndecesRow(Byte[] data, ref Int64 dataPosition, Int32 destX, Int32 destY, Byte[] destination, Byte[] paletteIndeces)
        {
            Byte row = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesSubRow(destX, destY, destination, paletteIndeces, row);

            row = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesSubRow(destX + 4, destY, destination, paletteIndeces, row);
        }

        protected virtual void HelperDecodeFourColorIndecesSubRow(Int32 destX, Int32 destY, Byte[] destination, Byte[] paletteIndeces, Byte rowDatum)
        {
            //do 4 setting operations
            for (Int32 x = 0; x < 4; ++x)
            {
                Int32 offset = (destY * this.Width) + (destX + x);
                destination[offset] = paletteIndeces[(rowDatum & 0x03)];
                rowDatum >>= 2;
            }
        }
        
        protected virtual void HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(Int32 destX, Int32 destY, Byte[] destination, Byte[] paletteIndeces, Byte rowDatum)
        {
            //do this 4 times
            for (Int32 bit = 0; bit < 4; ++bit)
            {
                Byte pixel = paletteIndeces[(rowDatum & 0x03)];

                //apply this to the appropriate 4 pixels
                Int32 square = (bit * 2);   //0,2,4,6

                Int32 offset = (destY * this.Width) + (destX + square);

                destination[offset] = pixel;
                destination[offset + 1] = pixel;

                rowDatum >>= 2;
            }
        }
        
        protected virtual void HelperDecodeFourColorIndecesEightRows(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, Byte[] paletteIndeces)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;

            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY, destination, paletteIndeces);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 1, destination, paletteIndeces);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 2, destination, paletteIndeces);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 3, destination, paletteIndeces);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 4, destination, paletteIndeces);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 5, destination, paletteIndeces);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 6, destination, paletteIndeces);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 7, destination, paletteIndeces);
        }

        protected virtual void HelperDecodeFourColorIndecesSquares(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, Byte[] paletteIndeces)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;

            //square row 1
            Byte rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY, destination, paletteIndeces, rowDatum);
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 1, destination, paletteIndeces, rowDatum);

            //square row 2
            rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 2, destination, paletteIndeces, rowDatum);
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 3, destination, paletteIndeces, rowDatum);

            //square row 3
            rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 4, destination, paletteIndeces, rowDatum);
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 5, destination, paletteIndeces, rowDatum);

            //square row 4
            rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 6, destination, paletteIndeces, rowDatum);
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 7, destination, paletteIndeces, rowDatum);
        }

        protected virtual void HelperDecodeFourColorIndecesVerticalRectangles(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, Byte[] paletteIndeces)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;

            //rectangle row 1
            Byte rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesSubRow(positionX, positionY, destination, paletteIndeces, rowDatum);
            this.HelperDecodeFourColorIndecesSubRow(positionX, positionY + 1, destination, paletteIndeces, rowDatum);

            rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesSubRow(positionX + 4, positionY, destination, paletteIndeces, rowDatum);
            this.HelperDecodeFourColorIndecesSubRow(positionX + 4, positionY + 1, destination, paletteIndeces, rowDatum);


            //rectangle row 2
            rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesSubRow(positionX, positionY + 2, destination, paletteIndeces, rowDatum);
            this.HelperDecodeFourColorIndecesSubRow(positionX, positionY + 3, destination, paletteIndeces, rowDatum);

            rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesSubRow(positionX + 4, positionY + 2, destination, paletteIndeces, rowDatum);
            this.HelperDecodeFourColorIndecesSubRow(positionX + 4, positionY + 3, destination, paletteIndeces, rowDatum);


            //rectangle row 3
            rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesSubRow(positionX, positionY + 4, destination, paletteIndeces, rowDatum);
            this.HelperDecodeFourColorIndecesSubRow(positionX, positionY + 5, destination, paletteIndeces, rowDatum);

            rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesSubRow(positionX + 4, positionY + 4, destination, paletteIndeces, rowDatum);
            this.HelperDecodeFourColorIndecesSubRow(positionX + 4, positionY + 5, destination, paletteIndeces, rowDatum);


            //rectangle row 4
            rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesSubRow(positionX, positionY + 6, destination, paletteIndeces, rowDatum);
            this.HelperDecodeFourColorIndecesSubRow(positionX, positionY + 7, destination, paletteIndeces, rowDatum);

            rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesSubRow(positionX + 4, positionY + 6, destination, paletteIndeces, rowDatum);
            this.HelperDecodeFourColorIndecesSubRow(positionX + 4, positionY + 7, destination, paletteIndeces, rowDatum);
        }

        protected virtual void HelperDecodeFourColorIndecesHorizontalRectangles(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, Byte[] paletteIndeces)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;

            //rectangle row 1
            Byte rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY, destination, paletteIndeces, rowDatum);

            //rectangle row 2
            rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 1, destination, paletteIndeces, rowDatum);

            //rectangle row 3
            rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 2, destination, paletteIndeces, rowDatum);

            //rectangle row 4
            rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 3, destination, paletteIndeces, rowDatum);

            //rectangle row 5
            rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 4, destination, paletteIndeces, rowDatum);

            //rectangle row 6
            rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 5, destination, paletteIndeces, rowDatum);

            //rectangle row 7
            rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 6, destination, paletteIndeces, rowDatum);

            //rectangle row 8
            rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 7, destination, paletteIndeces, rowDatum);
        }

        protected virtual void HelperDecodeFourColorIndecesQuadrants(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, Byte[] paletteIndeces)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;

            //quadrant 1
            this.HelperDecodeFourColorIndecesSingleQuadrant(data, ref dataPosition, positionX, positionY, destination, paletteIndeces);


            //quadrant 2
            Array.Copy(data, dataPosition, paletteIndeces, 0, 4);
            dataPosition += 4;
            this.HelperDecodeFourColorIndecesSingleQuadrant(data, ref dataPosition, positionX, positionY + 4, destination, paletteIndeces);


            //quadrant 3
            Array.Copy(data, dataPosition, paletteIndeces, 0, 4);
            dataPosition += 4;
            this.HelperDecodeFourColorIndecesSingleQuadrant(data, ref dataPosition, positionX + 4, positionY, destination, paletteIndeces);


            //quadrant 4
            Array.Copy(data, dataPosition, paletteIndeces, 0, 4);
            dataPosition += 4;
            this.HelperDecodeFourColorIndecesSingleQuadrant(data, ref dataPosition, positionX + 4, positionY + 4, destination, paletteIndeces);
        }

        protected virtual void HelperDecodeFourColorIndecesSingleQuadrant(Byte[] data, ref Int64 dataPosition, Int32 destX, Int32 destY, Byte[] destination, Byte[] paletteIndeces)
        {
            //Sub-row 1
            Byte rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesSubRow(destX, destY, destination, paletteIndeces, rowDatum);

            //Sub-row 2
            rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesSubRow(destX, destY + 1, destination, paletteIndeces, rowDatum);

            //Sub-row 3
            rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesSubRow(destX, destY + 2, destination, paletteIndeces, rowDatum);

            //Sub-row 4
            rowDatum = data[dataPosition];
            ++dataPosition;
            this.HelperDecodeFourColorIndecesSubRow(destX, destY + 3, destination, paletteIndeces, rowDatum);
        }

        protected virtual void HelperDecodeFourColorIndecesHorizontalHalves(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, Byte[] paletteIndeces)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;

            //first half
            this.HelperDecodeFourColorIndecesSingleQuadrant(data, ref dataPosition, positionX, positionY, destination, paletteIndeces);
            this.HelperDecodeFourColorIndecesSingleQuadrant(data, ref dataPosition, positionX, positionY + 4, destination, paletteIndeces);

            //second half
            Array.Copy(data, dataPosition, paletteIndeces, 0, 4);
            dataPosition += 4;
            this.HelperDecodeFourColorIndecesSingleQuadrant(data, ref dataPosition, positionX + 4, positionY, destination, paletteIndeces);
            this.HelperDecodeFourColorIndecesSingleQuadrant(data, ref dataPosition, positionX + 4, positionY + 4, destination, paletteIndeces);
        }

        protected virtual void HelperDecodeFourColorIndecesVerticalHalves(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, Byte[] paletteIndeces)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;

            //first vertical half
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY, destination, paletteIndeces);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 1, destination, paletteIndeces);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 2, destination, paletteIndeces);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 3, destination, paletteIndeces);

            //second vertical half
            Array.Copy(data, dataPosition, paletteIndeces, 0, 4);
            dataPosition += 4;
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 4, destination, paletteIndeces);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 5, destination, paletteIndeces);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 6, destination, paletteIndeces);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 7, destination, paletteIndeces);
        }
        #endregion

        protected virtual void HelperDecodeRawIndecesDitherColorsRow(Int32 destX, Int32 destY, Byte[] destination, Byte paletteIndex0, Byte paletteIndex1)
        {
            Int32 rowOffset = (destY * this.Width) + destX;

            destination[rowOffset] = paletteIndex0;
            destination[rowOffset + 1] = paletteIndex1;
            destination[rowOffset + 2] = paletteIndex0;
            destination[rowOffset + 3] = paletteIndex1;
            destination[rowOffset + 4] = paletteIndex0;
            destination[rowOffset + 5] = paletteIndex1;
            destination[rowOffset + 6] = paletteIndex0;
            destination[rowOffset + 7] = paletteIndex1;
        }
        #endregion
    }
}