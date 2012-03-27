using System;
using System.Collections.Generic;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Interpretation;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels.Enums;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Coding
{
    /// <summary>Represents an abstract base MVE coder for video pixel data</summary>
    public abstract class VideoCodingBase
    {
        #region Fields
        /// <summary>The width of the video file, in pixels</summary>
        protected Int32 Width { get; set; }

        /// <summary>The height of the video file, in pixels</summary>
        protected Int32 Height { get; set; }

        /// <summary>Represents the most recent video frame decoded</summary>
        protected PixelData RecentFrame { get; set; }

        /// <summary>Represents the stream of video opcodes</summary>
        public VideoOpcodeStream VideoStream { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the width, in pixel-coded blocks, of the video file</summary>
        protected virtual Int32 BlockWidth
        {
            get { return (this.Width / 8) + (this.Width % 8 == 0 ? 0 : 1); }
        }

        /// <summary>Exposes the height, in pixel-coded blocks, of the video file</summary>
        protected virtual Int32 BlockHeight
        {
            get { return (this.Height / 8) + (this.Height % 8 == 0 ? 0 : 1); }
        }

        /// <summary>Exposes the size of the PixelData buffer needed</summary>
        protected abstract Int32 BufferSize { get; }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="width">Width of the video data, in pixels</param>
        /// <param name="height">Height of the video data, in pixels</param>
        /// <param name="videoStream">MveVideoFrame collection of frames</param>
        public VideoCodingBase(Int32 width, Int32 height, VideoOpcodeStream videoStream)
        {
            this.Width = width;
            this.Height = height;
            this.VideoStream = videoStream;

            this.RecentFrame = null;
        }
        #endregion


        #region Frame Exposure
        /// <summary>Gets the next frame from the video stream</summary>
        /// <param name="mveFrame">MveVideoFrame to decode</param>
        /// <returns>A MediaBase Frame</returns>
        public virtual Frame GetNextFrame(MveVideoFrame mveFrame)
        {
            Frame frame = null;

            if (mveFrame != null)
            {
                Byte[] previous = (this.RecentFrame != null) ? this.RecentFrame.NativeBinaryData : new Byte[this.BufferSize];
                PixelData pd = this.GetNextImage(previous, mveFrame.DecodingMap.BlockEncoding, mveFrame.Data.Data, this.VideoStream.Palette, mveFrame.Data.DeltaFrame);
                
                //remember the current pixel data for the next frame; otherwise the movie gets very blocky (but interrestingly you can see the delta regions)
                this.RecentFrame = pd;

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
        protected virtual PixelData GetNextImage(Byte[] previousFrame, IList<BlockEncodingMethod> blockEncodings, Byte[] data, Palette palette, Boolean isDeltaFrame)
        {
            Byte[] newData = this.GetNextImageBuffer(isDeltaFrame);
            Int32 blockIndex = 0;
            Int64 dataPosition = 0L;

            for (Int32 y = 0; y < this.BlockHeight; ++ y)
                for (Int32 x = 0; x < this.BlockWidth; ++x)
                {
                    this.DecodeBlock(x, y, blockEncodings[blockIndex], data, ref dataPosition, previousFrame, newData);
                    ++blockIndex;
                }

            return new PixelData(newData, ScanLineOrder.TopDown, PixelFormat.RGB_B8G8R8, palette, this.Height, this.Width, 0, 0, 8);
        }

        /// <summary>Gets a buffer for the next image being prepared</summary>
        /// <param name="isDeltaFrame">Flag indicating whether to reuse the pivot buffer or to create a new one</param>
        /// <returns>A Byte Array buffer</returns>
        protected virtual Byte[] GetNextImageBuffer(Boolean isDeltaFrame)
        {
            return new Byte[this.BufferSize];
        }
        #endregion


        #region Decoding methods
        /// <summary>Decodes a block from the pixel data stream, and writes it to the outpu array at the specified X,Y cordinates</summary>
        /// <param name="blockX">X coordinate -in blocks- of the decoded block</param>
        /// <param name="blockY">Y coordinate -in blocks- of the decoded block</param>
        /// <param name="encodingMethod">Method the block is encoded with</param>
        /// <param name="data">Data stream to read from</param>
        /// <param name="dataPosition">Position within the data stream to read from</param>
        /// <param name="previousFrame">Frevious rame data to back-reference</param>
        /// <param name="outputData">output data to write to</param>
        protected virtual void DecodeBlock(Int32 blockX, Int32 blockY, BlockEncodingMethod encodingMethod, Byte[] data, ref Int64 dataPosition, Byte[] previousFrame, Byte[] outputData)
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
                    this.DecodeCopyNewFrameBottom(blockX, blockY, data, ref dataPosition, outputData);
                    break;
                case BlockEncodingMethod.CopyTop:
                    this.DecodeCopyNewFrameTop(blockX, blockY, data, ref dataPosition, outputData);
                    break;
                case BlockEncodingMethod.CopyPrevious:
                    this.DecodeCopyPreviousFrameClose(blockX, blockY, data, ref dataPosition, previousFrame, outputData);
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
        protected abstract void DecodeCopyBlock(Int32 blockX, Int32 blockY, Byte[] previousFrame, Byte[] outputData);

        ///<remarks>Read source is the new frame</remarks>
        protected abstract void DecodeCopyNewFrameBottom(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData);

        ///<remarks>Read source is the new frame</remarks>
        protected abstract void DecodeCopyNewFrameTop(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData);

        protected abstract void DecodeCopyPreviousFrameClose(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] previousFrame, Byte[] outputData);

        protected abstract void DecodeCopyPreviousFrameDistant(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] previousFrame, Byte[] outputData);

        protected abstract void DecodeTwoToneIndecesRowOrSquare(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData);

        protected abstract void DecodeTwoToneIndecesQuadrantsOrHalves(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData);

        protected abstract void DecodeFourColorIndecesRowsOrRectangles(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData);

        protected abstract void DecodeFourColorIndecesQuadrantsOrHalves(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData);

        protected abstract void DecodeRawIndeces(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData);

        protected abstract void DecodeRawIndeces2PixelSquares(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData);

        protected abstract void DecodeRawIndeces4PixelSquares(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData);

        protected abstract void DecodeRawIndecesSolidColor(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData);

        protected abstract void DecodeDither2Colors(Int32 blockX, Int32 blockY, Byte[] data, ref Int64 dataPosition, Byte[] outputData);
        #endregion
        #endregion


        #region Coder Commands
        /// <summary>Resets the video stream to the beginning</summary>
        public virtual void ResetVideo()
        {
            this.VideoStream.ResetStream();
            this.RecentFrame = null;
        }
        #endregion
    }
}