using System;
using System.Collections.Generic;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels.Enums;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Coding
{
    /// <summary>Represents an MVE coder for video pixel data that is palette-based</summary>
    /// <remarks>Uses a fresh delta rather than a rotation; B frames, rather than P</remarks>
    public class VideoCoding8BitNoDelta : VideoCoding8Bit
    {
        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="width">Width of the video data, in pixels</param>
        /// <param name="height">Height of the video data, in pixels</param>
        /// <param name="videoStream">MveVideoFrame collection of frames</param>
        public VideoCoding8BitNoDelta(Int32 width, Int32 height, VideoOpcodeStream videoStream) : base(width, height, videoStream) { }
        #endregion


        #region Frame Exposure
        /// <summary>Gets the next decoded frame, in 32-bit color</summary>
        /// <param name="previousFrame">Previous frame, for delta-reference</param>
        /// <param name="blockEncodings">List of encodings to decode with for each 8x8 block of pixels</param>
        /// <param name="data">Source data to use for decoding</param>
        /// <param name="palette">Palette used for indexed data</param>
        /// <param name="isDeltaFrame">Flag indicating whether to reuse the pivot buffer or to create a new one</param>
        protected override PixelData GetNextImage(Byte[] previousFrame, IList<BlockEncodingMethod> blockEncodings, Byte[] data, Palette palette, Boolean isDeltaFrame)
        {
            Byte[] newData = new Byte[this.BufferSize];
            Int32 blockIndex = 0;
            Int64 dataPosition = 0L;

            for (Int32 y = 0; y < this.BlockHeight; ++y)
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
        protected override Byte[] GetNextImageBuffer(Boolean isDeltaFrame)
        {
            return new Byte[this.BufferSize];
        }
        #endregion
    }
}