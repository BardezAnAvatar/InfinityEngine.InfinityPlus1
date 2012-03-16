using System;
using System.Collections.Generic;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Coding
{
    /// <summary>Represents an MVE coder for video pixel data that is 16-bit 'true' color</summary>
    public class VideoCoding16Bit : VideoCodingBase
    {
        #region Constants
        /// <summary>Represents the width of pixels for a single row</summary>
        /// <value>16 = 2 bytes per pixel * 8 pixels per row per block</value>
        protected const Int32 PixelRowSize = 16;
        #endregion


        #region Fields
        /// <summary>Represents the video frame prior to the most recent decoded frame, pivoted into the next frame</summary>
        protected PixelData PivotFrame { get; set; }

        /// <summary>Represents the current frame number</summary>
        public Int32 FrameNumber { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the size of the PixelData buffer needed</summary>
        protected override Int32 BufferSize
        {
            get { return this.Height * this.Width * 2; }
        }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="width">Width of the video data, in pixels</param>
        /// <param name="height">Height of the video data, in pixels</param>
        /// <param name="videoStream">MveVideoFrame collection of frames</param>
        public VideoCoding16Bit(Int32 width, Int32 height, VideoOpcodeStream videoStream)
            : base(width, height, videoStream)
        {
            this.FrameNumber = 0;
        }
        #endregion


        #region Frame Exposure
        /// <summary>Gets the next frame from the video stream</summary>
        /// <returns>A MediaBase Frame</returns>
        public override Frame GetNextFrame()
        {
            Frame frame = null;
            MveVideoFrame videoFrame = this.VideoStream.GetNextFrame();

            if (videoFrame != null)
            {
                Byte[] previous = (this.RecentFrame != null) ? this.RecentFrame.NativeBinaryData : new Byte[this.BufferSize];
                PixelData pd = this.GetNextImage(previous, videoFrame.DecodingMap.BlockEncoding, videoFrame.Data.Data, null, videoFrame.Data.DeltaFrame);

                //remember the current pixel data for the next frame; otherwise the movie gets very blocky (but interrestingly you can see the delta regions)
                if (videoFrame.Data.DeltaFrame)
                    this.PivotFrame = this.RecentFrame;

                this.RecentFrame = pd;

                //DEBUG
                //using (System.IO.FileStream fs = new System.IO.FileStream("\\Test Data\\Infinity Engine\\Problem\\MVE\\iep1\\iep1_" + frameNumber.ToString() + ".raw", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
                //    fs.Write(pd.NativeBinaryData, 0, pd.NativeBinaryData.Length);

                ++this.FrameNumber;

                frame = new Frame(pd);
            }

            return frame;
        }

        /// <summary>Gets the next decoded frame, in 32-bit color</summary>
        /// <param name="previousFrame">Previous frame, for delta-reference</param>
        /// <param name="blockEncodings">List of encodings to decode with for each 8x8 block of pixels</param>
        /// <param name="data">Source data to use for decoding</param>
        /// <param name="palette">Palette used for indexed data</param>
        /// <param name="isDeltaFrame">Flag indicating whether to reuse the pivot buffer or to create a new one</param>
        protected override PixelData GetNextImage(Byte[] previousFrame, IList<BlockEncodingMethod> blockEncodings, Byte[] data, Palette palette, Boolean isDeltaFrame)
        {
            Byte[] newData = this.GetNextImageBuffer(isDeltaFrame);
            Int32 blockIndex = 0;
            Int64 dataPosition = 2L;    //ther is an odd, unused offset in true color video chunks, that seems to say 'I have x left, including me', but is ultimately unused...
            Int64 dataPosition2 = ReusableIO.ReadUInt16FromArray(data, 0);

            for (Int32 y = 0; y < this.BlockHeight; ++y)
                for (Int32 x = 0; x < this.BlockWidth; ++x)
                {
                    this.DecodeBlock(x, y, blockEncodings[blockIndex], data, ref dataPosition, ref dataPosition2, previousFrame, newData);
                    ++blockIndex;
                }

            return new PixelData(newData, ScanLineOrder.TopDown, PixelFormat.RGB_B5G5R5X1, null, this.Height, this.Width, 0, 0, 16);
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
                buffer = this.PivotFrame.NativeBinaryData;

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



        /// <summary>Decodes a block from the pixel data stream, and writes it to the outpu array at the specified X,Y cordinates</summary>
        /// <param name="blockX">X coordinate -in blocks- of the decoded block</param>
        /// <param name="blockY">Y coordinate -in blocks- of the decoded block</param>
        /// <param name="encodingMethod">Method the block is encoded with</param>
        /// <param name="data">Data stream to read from</param>
        /// <param name="dataPosition">Position within the data stream to read from</param>
        /// <param name="dataPosition">Secondary position within the data stream to read from</param>
        /// <param name="previousFrame">Frevious rame data to back-reference</param>
        /// <param name="outputData">output data to write to</param>
        protected virtual void DecodeBlock(Int32 blockX, Int32 blockY, BlockEncodingMethod encodingMethod, Byte[] data, ref Int64 dataPosition, ref Int64 dataPosition2, Byte[] previousFrame, Byte[] outputData)
        {
            switch (encodingMethod)
            {
                case BlockEncodingMethod.Copy:
                    this.DecodeCopyBlock(blockX, blockY, previousFrame, outputData);
                    break;
                case BlockEncodingMethod.Unchanged:
                    //?
                    break;
                case BlockEncodingMethod.CopyBottom:
                    this.DecodeCopyNewFrameBottom(blockX, blockY, data, ref dataPosition2, outputData);
                    break;
                case BlockEncodingMethod.CopyTop:
                    this.DecodeCopyNewFrameTop(blockX, blockY, data, ref dataPosition2, outputData);
                    break;
                case BlockEncodingMethod.CopyPrevious:
                    this.DecodeCopyPreviousFrameClose(blockX, blockY, data, ref dataPosition2, previousFrame, outputData);
                    break;
                case BlockEncodingMethod.CopyPreviousLarge:
                    this.DecodeCopyPreviousFrameDistant(blockX, blockY, data, ref dataPosition, previousFrame, outputData);
                    break;
                case BlockEncodingMethod.BitField2PixelRowOr2Square:
                    this.DecodeTwoToneIndecesRowOrSquare(blockX, blockY, data, ref dataPosition, outputData);
                    break;
                case BlockEncodingMethod.BitFieldQuadrants4or8Pixel:
                    this.DecodeTwoToneIndecesQuadrantsOrHalves(blockX, blockY, data, ref dataPosition, outputData);
                    break;
                case BlockEncodingMethod.BitField2BitsPerPixel:
                    this.DecodeFourColorIndecesRowsOrRectangles(blockX, blockY, data, ref dataPosition, outputData);
                    break;
                case BlockEncodingMethod.BitFieldQuadrantsOrHalves2BitsPerPixel:
                    this.DecodeFourColorIndecesQuadrantsOrHalves(blockX, blockY, data, ref dataPosition, outputData);
                    break;
                case BlockEncodingMethod.Raw:
                    this.DecodeRawIndeces(blockX, blockY, data, ref dataPosition, outputData);
                    break;
                case BlockEncodingMethod.Raw2Square:
                    this.DecodeRawIndeces2PixelSquares(blockX, blockY, data, ref dataPosition, outputData);
                    break;
                case BlockEncodingMethod.Raw4Square:
                    this.DecodeRawIndeces4PixelSquares(blockX, blockY, data, ref dataPosition, outputData);
                    break;
                case BlockEncodingMethod.Raw8Square:
                    this.DecodeRawIndecesSolidColor(blockX, blockY, data, ref dataPosition, outputData);
                    break;
                case BlockEncodingMethod.Dithered:
                    this.DecodeDither2Colors(blockX, blockY, data, ref dataPosition, outputData);
                    break;
                default:
                    break;
            }
        }


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
            Int32 byteWidth = this.Width * 2;

            for (Int32 y = srcY; y < (srcY + 8); ++y)
                Array.Copy(previousFrame, ((y * byteWidth) + (srcX * 2)), outputData, ((y * byteWidth) + (srcX * 2)), VideoCoding16Bit.PixelRowSize);
        }

        /// <remarks>Read source is the new frame</remarks>
        /// <remarks>Opcode 2</remarks>
        protected override void DecodeCopyNewFrameBottom(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            this.HelperDecodeCopyNewFrameRelative(blockX, blockY, data, ref dataPosition, outputData, 1);
        }

        /// <remarks>Read source is the new frame</remarks>
        /// <remarks>Opcode 3</remarks>
        protected override void DecodeCopyNewFrameTop(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            this.HelperDecodeCopyNewFrameRelative(blockX, blockY, data, ref dataPosition, outputData, -1);
        }

        /// <remarks>Opcode 4</remarks>
        protected override void DecodeCopyPreviousFrameClose(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] previousFrame, Byte[] outputData)
        {
            Byte offsets = this.ReadDataRowByte(data, ref dataPosition);

            Int32 relOffsetX = (offsets & 0x0F) - 8;
            Int32 relOffsetY = (offsets >> 4) - 8;

            this.HelperDecodeComputeCopyPreviousFrameRelative(previousFrame, outputData, blockX, blockY, relOffsetX, relOffsetY);
        }

        /// <remarks>Opcode 5</remarks>
        protected override void DecodeCopyPreviousFrameDistant(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] previousFrame, Byte[] outputData)
        {
            SByte relOffsetX = (SByte)this.ReadDataRowByte(data, ref dataPosition);
            SByte relOffsetY = (SByte)this.ReadDataRowByte(data, ref dataPosition);

            this.HelperDecodeComputeCopyPreviousFrameRelative(previousFrame, outputData, blockX, blockY, relOffsetX, relOffsetY);
        }

        /// <remarks>Opcode 7</remarks>
        protected override void DecodeTwoToneIndecesRowOrSquare(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            UInt16 color0 = this.ReadDataPixelUInt16(data, ref dataPosition);
            UInt16 color1 = this.ReadDataPixelUInt16(data, ref dataPosition);

            if ((color0 & 0x8000) == 0x8000)    //15th bit set?
            {
                color0 &= 0x7FFF;   //unset the 15th bit
                this.HelperDecodeTwoToneIndecesBySquare(data, ref dataPosition, blockX, blockY, outputData, color0, color1);
            }
            else
                this.HelperDecodeTwoToneIndecesByRow(data, ref dataPosition, blockX, blockY, outputData, color0, color1);
        }

        /// <remarks>Opcode 8</remarks>
        protected override void DecodeTwoToneIndecesQuadrantsOrHalves(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            UInt16 color0 = this.ReadDataPixelUInt16(data, ref dataPosition);
            UInt16 color1 = this.ReadDataPixelUInt16(data, ref dataPosition);

            if ((color0 & 0x8000) == 0x8000)  //halves; is the first bit set?
            {
                //unset bit #15
                color0 &= 0x7FFF;

                //peek (do not consume) to determine which plane to split halves on
                UInt16 color2 = ReusableIO.ReadUInt16FromArray(data, dataPosition + 4);

                if ((color2 & 0x8000) == 0x8000)  //top, bottom; is the first bit set?
                    this.HelperDecodeTwoToneIndecesVerticalHalves(data, ref dataPosition, blockX, blockY, outputData, color0, color1);
                else    //left, right
                    this.HelperDecodeTwoToneIndecesHorizontalHalves(data, ref dataPosition, blockX, blockY, outputData, color0, color1);
            }
            else    //quadrants
                this.HelperDecodeTwoToneIndecesQuadrants(data, ref dataPosition, blockX, blockY, outputData, color0, color1);
        }

        /// <remarks>Opcode 9</remarks>
        protected override void DecodeFourColorIndecesRowsOrRectangles(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            UInt16[] colors = new UInt16[4];
            this.ReadPixelsIntoArrayOf4(data, ref dataPosition, colors);

            if ((colors[0] & 0x8000) == 0x8000)  //first bit set?
            {
                colors[0] &= 0x7FFF;    //unset bit #15

                if ((colors[2] & 0x8000) == 0x8000)  //1x2 rows; first bit set?
                {
                    colors[2] &= 0x7FFF;    //unset bit #15
                    this.HelperDecodeFourColorIndecesVerticalRectangles(data, ref dataPosition, blockX, blockY, outputData, colors);
                }
                else    //2x1 rows
                    this.HelperDecodeFourColorIndecesHorizontalRectangles(data, ref dataPosition, blockX, blockY, outputData, colors);
            }
            else
            {
                if ((colors[2] & 0x8000) == 0x8000)  //2x2 rows; first bit set?
                {
                    colors[2] &= 0x7FFF;    //unset bit #15
                    this.HelperDecodeFourColorIndecesSquares(data, ref dataPosition, blockX, blockY, outputData, colors);
                }
                else    //1x1 rows
                    this.HelperDecodeFourColorIndecesEightRows(data, ref dataPosition, blockX, blockY, outputData, colors);
            }
        }

        /// <remarks>Opcode 10</remarks>
        protected override void DecodeFourColorIndecesQuadrantsOrHalves(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            UInt16[] colors = new UInt16[4];
            this.ReadPixelsIntoArrayOf4(data, ref dataPosition, colors);
            
            if ((colors[0] & 0x8000) == 0x8000)  //halves; first bit set?
            {
                colors[0] &= 0x7FFF;    //unset bit #15
             
                //peek (do not consume) to determine which plane to split halves on
                UInt16 color4 = ReusableIO.ReadUInt16FromArray(data, dataPosition + 8);

                if ((color4 & 0x8000) == 0x8000)  //top, bottom; first bit set?
                    this.HelperDecodeFourColorIndecesVerticalHalves(data, ref dataPosition, blockX, blockY, outputData, colors);
                else    //left, right
                    this.HelperDecodeFourColorIndecesHorizontalHalves(data, ref dataPosition, blockX, blockY, outputData, colors);
            }
            else    //quadrants
                this.HelperDecodeFourColorIndecesQuadrants(data, ref dataPosition, blockX, blockY, outputData, colors);
        }

        /// <remarks>Opcode 11</remarks>
        protected override void DecodeRawIndeces(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;
            Int32 byteWidth = this.Width * 2;

            for (Int32 y = 0; y < 8; ++y)
            {
                Int32 offset = ((positionY + y) * byteWidth) + (positionX * 2);
                Array.Copy(data, dataPosition, outputData, offset, VideoCoding16Bit.PixelRowSize);
                dataPosition += VideoCoding16Bit.PixelRowSize;
            }
        }

        /// <remarks>Opcode 12</remarks>
        protected override void DecodeRawIndeces2PixelSquares(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;
            Int32 byteWidth = this.Width * 2;

            for (Int32 y = 0; y < 8; y += 2)
            {
                Int32 offset1 = ((positionY + y) * byteWidth) + (positionX * 2);
                Int32 offset2 = ((positionY + y + 1) * byteWidth) + (positionX * 2);

                for (Int32 x = 0; x < 8; x += 2)
                {
                    UInt16 color = this.ReadDataPixelUInt16(data, ref dataPosition);

                    ReusableIO.WriteUInt16ToArray(color, outputData, offset1 + (x * 2));
                    ReusableIO.WriteUInt16ToArray(color, outputData, offset1 + (x * 2) + 2);
                    ReusableIO.WriteUInt16ToArray(color, outputData, offset2 + (x * 2));
                    ReusableIO.WriteUInt16ToArray(color, outputData, offset2 + (x * 2) + 2);
                }
            }
        }

        /// <remarks>Opcode 13</remarks>
        protected override void DecodeRawIndeces4PixelSquares(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;
            Int32 byteWidth = this.Width * 2;

            for (Int32 y = 0; y < 8; y += 4)
            {
                Int32 offset1 = ((positionY + y) * byteWidth) + (positionX * 2);
                Int32 offset2 = ((positionY + y + 1) * byteWidth) + (positionX * 2);
                Int32 offset3 = ((positionY + y + 2) * byteWidth) + (positionX * 2);
                Int32 offset4 = ((positionY + y + 3) * byteWidth) + (positionX * 2);

                for (Int32 x = 0; x < 8; x += 4)
                {
                    UInt16 color = this.ReadDataPixelUInt16(data, ref dataPosition);

                    ReusableIO.WriteUInt16ToArray(color, outputData, offset1 + (x * 2));
                    ReusableIO.WriteUInt16ToArray(color, outputData, offset1 + (x * 2) + 2);
                    ReusableIO.WriteUInt16ToArray(color, outputData, offset1 + (x * 2) + 4);
                    ReusableIO.WriteUInt16ToArray(color, outputData, offset1 + (x * 2) + 6);
                    ReusableIO.WriteUInt16ToArray(color, outputData, offset2 + (x * 2));
                    ReusableIO.WriteUInt16ToArray(color, outputData, offset2 + (x * 2) + 2);
                    ReusableIO.WriteUInt16ToArray(color, outputData, offset2 + (x * 2) + 4);
                    ReusableIO.WriteUInt16ToArray(color, outputData, offset2 + (x * 2) + 6);
                    ReusableIO.WriteUInt16ToArray(color, outputData, offset3 + (x * 2));
                    ReusableIO.WriteUInt16ToArray(color, outputData, offset3 + (x * 2) + 2);
                    ReusableIO.WriteUInt16ToArray(color, outputData, offset3 + (x * 2) + 4);
                    ReusableIO.WriteUInt16ToArray(color, outputData, offset3 + (x * 2) + 6);
                    ReusableIO.WriteUInt16ToArray(color, outputData, offset4 + (x * 2));
                    ReusableIO.WriteUInt16ToArray(color, outputData, offset4 + (x * 2) + 2);
                    ReusableIO.WriteUInt16ToArray(color, outputData, offset4 + (x * 2) + 4);
                    ReusableIO.WriteUInt16ToArray(color, outputData, offset4 + (x * 2) + 6);
                }
            }
        }

        /// <remarks>Opcode 14</remarks>
        protected override void DecodeRawIndecesSolidColor(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;
            Int32 byteWidth = this.Width * 2;

            UInt16 color = this.ReadDataPixelUInt16(data, ref dataPosition);

            for (Int32 y = 0; y < 8; ++y)
            {
                Int32 offset = ((positionY + y) * byteWidth) + (positionX * 2);

                for (Int32 x = 0; x < 8; ++x)
                    ReusableIO.WriteUInt16ToArray(color, outputData, offset + (x * 2));
            }
        }

        /// <remarks>Opcode 15</remarks>
        protected override void DecodeDither2Colors(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;

            UInt16 color0 = this.ReadDataPixelUInt16(data, ref dataPosition);
            UInt16 color1 = this.ReadDataPixelUInt16(data, ref dataPosition);

            this.HelperDecodeRawIndecesDitherColorsRow(positionX, positionY, outputData, color0, color1);
            this.HelperDecodeRawIndecesDitherColorsRow(positionX, positionY + 1, outputData, color1, color0);
            this.HelperDecodeRawIndecesDitherColorsRow(positionX, positionY + 2, outputData, color0, color1);
            this.HelperDecodeRawIndecesDitherColorsRow(positionX, positionY + 3, outputData, color1, color0);
            this.HelperDecodeRawIndecesDitherColorsRow(positionX, positionY + 4, outputData, color0, color1);
            this.HelperDecodeRawIndecesDitherColorsRow(positionX, positionY + 5, outputData, color1, color0);
            this.HelperDecodeRawIndecesDitherColorsRow(positionX, positionY + 6, outputData, color0, color1);
            this.HelperDecodeRawIndecesDitherColorsRow(positionX, positionY + 7, outputData, color1, color0);
        }
        #endregion


        #region Decode Helper Methods
        /// <summary>Operation 2, 3 helper. Copies the current frame's data from the buffer of older frame data.</summary>
        /// <param name="sign">Must be either -1 or 1</param>
        protected virtual void HelperDecodeCopyNewFrameRelative(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData, Int32 sign)
        {
            //error detection
            switch (sign)
            {
                //valid cases
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

            Int32 byteWidth = this.Width * 2;

            //copy
            for (Int32 yPos = srcY; yPos < (srcY + 8); ++yPos)
            {
                Array.Copy(outputData, ((yPos * byteWidth) + (srcX * 2)), outputData, ((destY * byteWidth) + (destX * 2)), VideoCoding16Bit.PixelRowSize);
                ++destY;
            }
        }

        protected virtual void HelperDecodeComputeCopyNewFrameBottomTopOffsets(Byte[] data, ref Int64 dataPosition, out Int32 offsetX, out Int32 offsetY)
        {
            Byte offsets = this.ReadDataRowByte(data, ref dataPosition);

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

            Int32 byteWidth = this.Width * 2;

            //copy
            for (Int32 yPos = srcY; yPos < (srcY + 8); ++yPos)
            {
                Array.Copy(origin, ((yPos * byteWidth) + (srcX * 2)), destination, ((blockYPos * byteWidth) + (blockXPos * 2)), VideoCoding16Bit.PixelRowSize);
                ++blockYPos;
            }
        }

        #region Two-Tone helpers
        protected virtual void HelperDecodeTwoToneIndecesByRow(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, UInt16 color0, UInt16 color1)
        {
            Int32 blockYPos = blockY * 8;
            Int32 blockXPos = blockX * 8;

            for (Int32 y = 0; y < 8; ++y)
                this.HelperDecodeTwoToneIndecesBlockRow(data, ref dataPosition, destination, color0, color1, blockXPos, blockYPos + y);
        }

        protected virtual void HelperDecodeTwoToneIndecesBlockRow(Byte[] data, ref Int64 dataPosition, Byte[] destination, UInt16 color0, UInt16 color1, Int32 destX, Int32 destY)
        {
            Byte row = this.ReadDataRowByte(data, ref dataPosition);
            Int32 byteWidth = this.Width * 2;

            //do all 8 setting operations
            for (Int32 x = 0; x < 8; ++x)
            {
                Int32 offset = (destY * byteWidth) + ((destX + x) * 2);
                ReusableIO.WriteUInt16ToArray((((row & 0x01) == 0) ? color0 : color1), destination, offset);

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
        protected virtual void HelperDecodeTwoToneIndecesBySquare(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, UInt16 color0, UInt16 color1)
        {
            Int32 blockYPos = blockY * 8;
            Int32 blockXPos = blockX * 8;

            for (Int32 byteCount = 0; byteCount < 2; ++byteCount)
            {
                Byte halfBlock = this.ReadDataRowByte(data, ref dataPosition);

                Int32 rows1 = (halfBlock & 0x0F);
                Int32 rows2 = (halfBlock >> 4);

                this.HelperDecodeTwoToneIndecesBySquareSubRows(blockXPos, blockYPos, destination, color0, color1, rows1);
                blockYPos += 2;
                this.HelperDecodeTwoToneIndecesBySquareSubRows(blockXPos, blockYPos, destination, color0, color1, rows2);
                blockYPos += 2;
            }
        }

        protected virtual void HelperDecodeTwoToneIndecesBySquareSubRows(Int32 destX, Int32 destY, Byte[] destination, UInt16 color0, UInt16 color1, Int32 subRowMap)
        {
            Int32 byteWidth = this.Width * 2;

            //do this 4 times
            for (Int32 bit = 0; bit < 4; ++bit)
            {
                UInt16 pixel = ((subRowMap & 0x01) == 0) ? color0 : color1;

                //apply this to the appropriate 4 pixels
                Int32 square = (bit * 2);   //0,2,4,6

                Int32 offset1 = (destY * byteWidth) + ((destX + square) * 2);
                Int32 offset2 = ((destY + 1) * byteWidth) + ((destX + square) * 2);

                ReusableIO.WriteUInt16ToArray(pixel, destination, offset1);
                ReusableIO.WriteUInt16ToArray(pixel, destination, offset1 + 2);
                ReusableIO.WriteUInt16ToArray(pixel, destination, offset2);
                ReusableIO.WriteUInt16ToArray(pixel, destination, offset2 + 2);

                subRowMap >>= 1;
            }
        }

        /// <remarks>Sets quadrants of an 8x8 block, top left, top right, bottom left, bottom right with two bytes per quadrant, one bit per pixel</remarks>
        protected virtual void HelperDecodeTwoToneIndecesQuadrants(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, UInt16 color0, UInt16 color1)
        {
            Int32 blockXPos = blockX * 8;
            Int32 blockYPos = blockY * 8;

            //top left
            Byte datum0 = this.ReadDataRowByte(data, ref dataPosition);
            Byte datum1 = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrant(blockXPos, blockYPos, destination, color0, color1, datum0, datum1);

            //bottom left
            this.ReadPixelsIntoSetOf2(data, ref dataPosition, ref color0, ref color1);
            datum0 = this.ReadDataRowByte(data, ref dataPosition);
            datum1 = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrant(blockXPos, blockYPos + 4, destination, color0, color1, datum0, datum1);

            //top right
            this.ReadPixelsIntoSetOf2(data, ref dataPosition, ref color0, ref color1);
            datum0 = this.ReadDataRowByte(data, ref dataPosition);
            datum1 = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrant(blockXPos + 4, blockYPos, destination, color0, color1, datum0, datum1);

            //bottom right
            this.ReadPixelsIntoSetOf2(data, ref dataPosition, ref color0, ref color1);
            datum0 = this.ReadDataRowByte(data, ref dataPosition);
            datum1 = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrant(blockXPos + 4, blockYPos + 4, destination, color0, color1, datum0, datum1);
        }

        /// <remarks>Sets quadrants of an 8x8 block, top left, top right, bottom left, bottom right with two bytes per quadrant, one bit per pixel</remarks>
        protected virtual void HelperDecodeTwoToneIndecesQuadrantsSingleQuadrant(Int32 destX, Int32 destY, Byte[] destination, UInt16 color0, UInt16 color1, Int32 quadrantDatum0, Int32 quadrantDatum1)
        {
            Int32 row1 = (quadrantDatum0 & 0x0F);
            Int32 row2 = (quadrantDatum0 >> 4);
            Int32 row3 = (quadrantDatum1 & 0x0F);
            Int32 row4 = (quadrantDatum1 >> 4);

            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(destX, destY, destination, color0, color1, row1);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(destX, destY + 1, destination, color0, color1, row2);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(destX, destY + 2, destination, color0, color1, row3);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(destX, destY + 3, destination, color0, color1, row4);
        }

        /// <remarks>Sets quadrants of an 8x8 block, top left, top right, bottom left, bottom right with two bytes per quadrant, one bit per pixel</remarks>
        protected virtual void HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(Int32 destX, Int32 destY, Byte[] destination, UInt16 color0, UInt16 color1, Int32 subRow)
        {
            Int32 byteWidth = this.Width * 2;

            for (Int32 x = 0; x < 4; ++x)
            {
                Int32 offset = (destY * byteWidth) + ((destX + x) * 2);
                ReusableIO.WriteUInt16ToArray( (((subRow & 0x01) == 0) ? color0 : color1) , destination, offset);

                subRow >>= 1;
            }
        }

        protected virtual void HelperDecodeTwoToneIndecesHorizontalHalves(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, UInt16 color0, UInt16 color1)
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

            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos, blockYPos, destination, color0, color1, row1);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos, blockYPos + 1, destination, color0, color1, row2);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos, blockYPos + 2, destination, color0, color1, row3);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos, blockYPos + 3, destination, color0, color1, row4);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos, blockYPos + 4, destination, color0, color1, row5);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos, blockYPos + 5, destination, color0, color1, row6);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos, blockYPos + 6, destination, color0, color1, row7);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos, blockYPos + 7, destination, color0, color1, row8);


            this.ReadPixelsIntoSetOf2(data, ref dataPosition, ref color0, ref color1);
            row1 = (data[dataPosition] & 0x0F);
            row2 = (data[dataPosition] >> 4);
            row3 = (data[dataPosition + 1] & 0x0F);
            row4 = (data[dataPosition + 1] >> 4);
            row5 = (data[dataPosition + 2] & 0x0F);
            row6 = (data[dataPosition + 2] >> 4);
            row7 = (data[dataPosition + 3] & 0x0F);
            row8 = (data[dataPosition + 3] >> 4);
            dataPosition += 4;

            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos + 4, blockYPos, destination, color0, color1, row1);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos + 4, blockYPos + 1, destination, color0, color1, row2);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos + 4, blockYPos + 2, destination, color0, color1, row3);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos + 4, blockYPos + 3, destination, color0, color1, row4);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos + 4, blockYPos + 4, destination, color0, color1, row5);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos + 4, blockYPos + 5, destination, color0, color1, row6);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos + 4, blockYPos + 6, destination, color0, color1, row7);
            this.HelperDecodeTwoToneIndecesQuadrantsSingleQuadrantRow(blockXPos + 4, blockYPos + 7, destination, color0, color1, row8);
        }

        protected virtual void HelperDecodeTwoToneIndecesVerticalHalves(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, UInt16 color0, UInt16 color1)
        {
            Int32 blockXPos = blockX * 8;
            Int32 blockYPos = blockY * 8;

            this.HelperDecodeTwoToneIndecesBlockRow(data, ref dataPosition, destination, color0, color1, blockXPos, blockYPos);
            this.HelperDecodeTwoToneIndecesBlockRow(data, ref dataPosition, destination, color0, color1, blockXPos, blockYPos + 1);
            this.HelperDecodeTwoToneIndecesBlockRow(data, ref dataPosition, destination, color0, color1, blockXPos, blockYPos + 2);
            this.HelperDecodeTwoToneIndecesBlockRow(data, ref dataPosition, destination, color0, color1, blockXPos, blockYPos + 3);

            this.ReadPixelsIntoSetOf2(data, ref dataPosition, ref color0, ref color1);
            //warning; color0 now has a high bit, #15 set. unset it.
            color0 &= 0x7FFF;

            this.HelperDecodeTwoToneIndecesBlockRow(data, ref dataPosition, destination, color0, color1, blockXPos, blockYPos + 4);
            this.HelperDecodeTwoToneIndecesBlockRow(data, ref dataPosition, destination, color0, color1, blockXPos, blockYPos + 5);
            this.HelperDecodeTwoToneIndecesBlockRow(data, ref dataPosition, destination, color0, color1, blockXPos, blockYPos + 6);
            this.HelperDecodeTwoToneIndecesBlockRow(data, ref dataPosition, destination, color0, color1, blockXPos, blockYPos + 7);
        }
        #endregion


        #region Four Color helpers
        protected virtual void HelperDecodeFourColorIndecesRow(Byte[] data, ref Int64 dataPosition, Int32 destX, Int32 destY, Byte[] destination, UInt16[] colors)
        {
            Byte row = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesSubRow(destX, destY, destination, colors, row);

            row = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesSubRow(destX + 4, destY, destination, colors, row);
        }

        protected virtual void HelperDecodeFourColorIndecesSubRow(Int32 destX, Int32 destY, Byte[] destination, UInt16[] colors, Byte rowDatum)
        {
            Int32 byteWidth = this.Width * 2;

            //do 4 setting operations
            for (Int32 x = 0; x < 4; ++x)
            {
                Int32 offset = (destY * byteWidth) + ((destX + x) * 2);
                ReusableIO.WriteUInt16ToArray(colors[(rowDatum & 0x03)], destination, offset);
                rowDatum >>= 2;
            }
        }

        protected virtual void HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(Int32 destX, Int32 destY, Byte[] destination, UInt16[] colors, Byte rowDatum)
        {
            Int32 byteWidth = this.Width * 2;

            //do this 4 times
            for (Int32 bit = 0; bit < 4; ++bit)
            {
                UInt16 pixel = colors[(rowDatum & 0x03)];

                //apply this to the appropriate 4 pixels
                Int32 square = (bit * 2);   //0,2,4,6

                Int32 offset = (destY * byteWidth) + ((destX + square) * 2);

                ReusableIO.WriteUInt16ToArray(pixel, destination, offset);
                ReusableIO.WriteUInt16ToArray(pixel, destination, offset + 2);

                rowDatum >>= 2;
            }
        }

        protected virtual void HelperDecodeFourColorIndecesEightRows(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, UInt16[] colors)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;

            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY, destination, colors);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 1, destination, colors);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 2, destination, colors);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 3, destination, colors);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 4, destination, colors);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 5, destination, colors);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 6, destination, colors);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 7, destination, colors);
        }

        protected virtual void HelperDecodeFourColorIndecesSquares(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, UInt16[] colors)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;

            //square row 1
            Byte rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY, destination, colors, rowDatum);
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 1, destination, colors, rowDatum);

            //square row 2
            rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 2, destination, colors, rowDatum);
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 3, destination, colors, rowDatum);

            //square row 3
            rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 4, destination, colors, rowDatum);
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 5, destination, colors, rowDatum);

            //square row 4
            rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 6, destination, colors, rowDatum);
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 7, destination, colors, rowDatum);
        }

        protected virtual void HelperDecodeFourColorIndecesVerticalRectangles(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, UInt16[] colors)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;

            //rectangle row 1
            Byte rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesSubRow(positionX, positionY, destination, colors, rowDatum);
            this.HelperDecodeFourColorIndecesSubRow(positionX, positionY + 1, destination, colors, rowDatum);

            rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesSubRow(positionX + 4, positionY, destination, colors, rowDatum);
            this.HelperDecodeFourColorIndecesSubRow(positionX + 4, positionY + 1, destination, colors, rowDatum);


            //rectangle row 2
            rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesSubRow(positionX, positionY + 2, destination, colors, rowDatum);
            this.HelperDecodeFourColorIndecesSubRow(positionX, positionY + 3, destination, colors, rowDatum);

            rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesSubRow(positionX + 4, positionY + 2, destination, colors, rowDatum);
            this.HelperDecodeFourColorIndecesSubRow(positionX + 4, positionY + 3, destination, colors, rowDatum);


            //rectangle row 3
            rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesSubRow(positionX, positionY + 4, destination, colors, rowDatum);
            this.HelperDecodeFourColorIndecesSubRow(positionX, positionY + 5, destination, colors, rowDatum);

            rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesSubRow(positionX + 4, positionY + 4, destination, colors, rowDatum);
            this.HelperDecodeFourColorIndecesSubRow(positionX + 4, positionY + 5, destination, colors, rowDatum);


            //rectangle row 4
            rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesSubRow(positionX, positionY + 6, destination, colors, rowDatum);
            this.HelperDecodeFourColorIndecesSubRow(positionX, positionY + 7, destination, colors, rowDatum);

            rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesSubRow(positionX + 4, positionY + 6, destination, colors, rowDatum);
            this.HelperDecodeFourColorIndecesSubRow(positionX + 4, positionY + 7, destination, colors, rowDatum);
        }

        protected virtual void HelperDecodeFourColorIndecesHorizontalRectangles(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, UInt16[] colors)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;

            //rectangle row 1
            Byte rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY, destination, colors, rowDatum);

            //rectangle row 2
            rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 1, destination, colors, rowDatum);

            //rectangle row 3
            rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 2, destination, colors, rowDatum);

            //rectangle row 4
            rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 3, destination, colors, rowDatum);

            //rectangle row 5
            rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 4, destination, colors, rowDatum);

            //rectangle row 6
            rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 5, destination, colors, rowDatum);

            //rectangle row 7
            rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 6, destination, colors, rowDatum);

            //rectangle row 8
            rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesDoubleWidthPixelSubRow(positionX, positionY + 7, destination, colors, rowDatum);
        }

        protected virtual void HelperDecodeFourColorIndecesQuadrants(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, UInt16[] colors)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;

            //quadrant 1
            this.HelperDecodeFourColorIndecesSingleQuadrant(data, ref dataPosition, positionX, positionY, destination, colors);


            //quadrant 2
            this.ReadPixelsIntoArrayOf4(data, ref dataPosition, colors);
            this.HelperDecodeFourColorIndecesSingleQuadrant(data, ref dataPosition, positionX, positionY + 4, destination, colors);


            //quadrant 3
            this.ReadPixelsIntoArrayOf4(data, ref dataPosition, colors);
            this.HelperDecodeFourColorIndecesSingleQuadrant(data, ref dataPosition, positionX + 4, positionY, destination, colors);


            //quadrant 4
            this.ReadPixelsIntoArrayOf4(data, ref dataPosition, colors);
            this.HelperDecodeFourColorIndecesSingleQuadrant(data, ref dataPosition, positionX + 4, positionY + 4, destination, colors);
        }

        protected virtual void HelperDecodeFourColorIndecesSingleQuadrant(Byte[] data, ref Int64 dataPosition, Int32 destX, Int32 destY, Byte[] destination, UInt16[] colors)
        {
            //Sub-row 1
            Byte rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesSubRow(destX, destY, destination, colors, rowDatum);

            //Sub-row 2
            rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesSubRow(destX, destY + 1, destination, colors, rowDatum);

            //Sub-row 3
            rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesSubRow(destX, destY + 2, destination, colors, rowDatum);

            //Sub-row 4
            rowDatum = this.ReadDataRowByte(data, ref dataPosition);
            this.HelperDecodeFourColorIndecesSubRow(destX, destY + 3, destination, colors, rowDatum);
        }

        protected virtual void HelperDecodeFourColorIndecesHorizontalHalves(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, UInt16[] colors)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;

            //first half
            this.HelperDecodeFourColorIndecesSingleQuadrant(data, ref dataPosition, positionX, positionY, destination, colors);
            this.HelperDecodeFourColorIndecesSingleQuadrant(data, ref dataPosition, positionX, positionY + 4, destination, colors);

            //second half
            this.ReadPixelsIntoArrayOf4(data, ref dataPosition, colors);
            this.HelperDecodeFourColorIndecesSingleQuadrant(data, ref dataPosition, positionX + 4, positionY, destination, colors);
            this.HelperDecodeFourColorIndecesSingleQuadrant(data, ref dataPosition, positionX + 4, positionY + 4, destination, colors);
        }

        protected virtual void HelperDecodeFourColorIndecesVerticalHalves(Byte[] data, ref Int64 dataPosition, Int32 blockX, Int32 blockY, Byte[] destination, UInt16[] colors)
        {
            Int32 positionX = blockX * 8;
            Int32 positionY = blockY * 8;

            //first vertical half
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY, destination, colors);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 1, destination, colors);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 2, destination, colors);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 3, destination, colors);

            //second vertical half
            this.ReadPixelsIntoArrayOf4(data, ref dataPosition, colors);
            //warning; color0 now has a high bit, #15 set. unset it.
            colors[0] &= 0x7FFF;

            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 4, destination, colors);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 5, destination, colors);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 6, destination, colors);
            this.HelperDecodeFourColorIndecesRow(data, ref dataPosition, positionX, positionY + 7, destination, colors);
        }
        #endregion

        /// <summary>Dithers between two colors, alternating between them in a given block row</summary>
        /// <param name="destX">pixel x co-ordinate</param>
        /// <param name="destY">pxiel y coordinate</param>
        /// <param name="destination">destination array to write to</param>
        /// <param name="color0">First color to write</param>
        /// <param name="color1">Second color to write</param>
        protected virtual void HelperDecodeRawIndecesDitherColorsRow(Int32 destX, Int32 destY, Byte[] destination, UInt16 color0, UInt16 color1)
        {
            Int32 byteWidth = this.Width * 2;
            Int32 rowOffset = (destY * byteWidth) + (destX * 2);

            ReusableIO.WriteUInt16ToArray(color0, destination, rowOffset);
            ReusableIO.WriteUInt16ToArray(color1, destination, rowOffset + 2);
            ReusableIO.WriteUInt16ToArray(color0, destination, rowOffset + 4);
            ReusableIO.WriteUInt16ToArray(color1, destination, rowOffset + 6);
            ReusableIO.WriteUInt16ToArray(color0, destination, rowOffset + 8);
            ReusableIO.WriteUInt16ToArray(color1, destination, rowOffset + 10);
            ReusableIO.WriteUInt16ToArray(color0, destination, rowOffset + 12);
            ReusableIO.WriteUInt16ToArray(color1, destination, rowOffset + 14);
        }
        #endregion


        #region Helper reading methods
        /// <summary>Consumes two bytes representing a pixel from a data array</summary>
        /// <param name="data">Byte array to consume from</param>
        /// <param name="dataPosition">position index for the memory</param>
        /// <returns>The pixel as an unsigned two-byte integer</returns>
        protected virtual UInt16 ReadDataPixelUInt16(Byte[] data, ref Int64 dataPosition)
        {
            UInt16 retVal = ReusableIO.ReadUInt16FromArray(data, dataPosition);
            dataPosition += 2;

            return retVal;
        }

        /// <summary>Consumes one byte from a data array, ususally for pixel assignment in a row</summary>
        /// <param name="data">Byte array to consume from</param>
        /// <param name="dataPosition">position index for the memory</param>
        /// <returns>The byte read from the data array</returns>
        protected virtual Byte ReadDataRowByte(Byte[] data, ref Int64 dataPosition)
        {
            return data[dataPosition++]; //postfix ++ should increment after the value was evaluated
        }

        /// <summary>Reads four colors (8 bytes) from the source data into the colors array</summary>
        /// <param name="data">Data array to read from</param>
        /// <param name="dataPosition">Position of data array to reaad from</param>
        /// <param name="colors">Colors array to write to</param>
        protected virtual void ReadPixelsIntoArrayOf4(Byte[] data, ref Int64 dataPosition, UInt16[] colors)
        {
            colors[0] = this.ReadDataPixelUInt16(data, ref dataPosition);
            colors[1] = this.ReadDataPixelUInt16(data, ref dataPosition);
            colors[2] = this.ReadDataPixelUInt16(data, ref dataPosition);
            colors[3] = this.ReadDataPixelUInt16(data, ref dataPosition);
        }

        /// <summary>Reads two colors (4 bytes) from the source data into the colors set</summary>
        /// <param name="data">Data array to read from</param>
        /// <param name="dataPosition">Position of data array to reaad from</param>
        /// <param name="color0">First color to assign</param>
        /// <param name="color1">Second color to assign</param>
        protected virtual void ReadPixelsIntoSetOf2(Byte[] data, ref Int64 dataPosition, ref UInt16 color0, ref UInt16 color1)
        {
            color0 = this.ReadDataPixelUInt16(data, ref dataPosition);
            color1 = this.ReadDataPixelUInt16(data, ref dataPosition);
        }
        #endregion
    }
}