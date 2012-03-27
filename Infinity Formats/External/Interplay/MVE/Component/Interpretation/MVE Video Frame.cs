using System;
using System.Collections.Generic;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Interpretation
{
    /// <summary>Represents a container for the data needed to process a single video frame (i.e.: the MVE SetDecodingMap and VideoData opcodes)</summary>
    /// <remarks>Does not contain a back-reference to the previous frame</remarks>
    public class MveVideoFrame : MveInterpretableChunk
    {
        #region Fields
        /// <summary>Represents a frame's decoding approaches for each 8x8 block of the frame</summary>
        public SetDecodingMap DecodingMap { get; set; }

        /// <summary>Represents the binary encoded data of the frame</summary>
        public VideoData Data { get; set; }

        /// <summary>Buffer to hold a decoded frame from the buffer</summary>
        public Frame FrameBuffer { get; set; }

        public Int32 FrameNumber;
        #endregion


        #region Construction
        /// <summary>Partial definition constructor</summary>
        /// <param name="decodingMap">SetDecodingMap opcode</param>
        /// <param name="data">VideoData Opcode</param>
        public MveVideoFrame(SetDecodingMap decodingMap, VideoData data)
        {
            this.DecodingMap = decodingMap;
            this.Data = data;
        }

        /// <summary>Partial definition constructor</summary>
        /// <param name="decodingMap">SetDecodingMap opcode</param>
        /// <param name="data">VideoData Opcode</param>
        public MveVideoFrame(SetDecodingMap decodingMap, VideoData data, Int32 number)
        {
            this.DecodingMap = decodingMap;
            this.Data = data;
            this.FrameNumber = number;
        }
        #endregion
    }
}