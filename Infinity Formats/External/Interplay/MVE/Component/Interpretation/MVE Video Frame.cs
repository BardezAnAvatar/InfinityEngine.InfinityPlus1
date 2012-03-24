using System;
using System.Collections.Generic;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management
{
    /// <summary>Represents a container for the data needed to process a single video frame (i.e.: the MVE SetDecodingMap and VideoData opcodes)</summary>
    /// <remarks>Does not contain a back-reference to the previous frame</remarks>
    public class MveVideoFrame
    {
        #region Fields
        /// <summary>Represents a frame's decoding approaches for each 8x8 block of the frame</summary>
        public SetDecodingMap DecodingMap { get; set; }

        /// <summary>Represents the binary encoded data of the frame</summary>
        public VideoData Data { get; set; }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="decodingMap">SetDecodingMap opcode</param>
        /// <param name="data">VideoData Opcode</param>
        public MveVideoFrame(SetDecodingMap decodingMap, VideoData data)
        {
            this.DecodingMap = decodingMap;
            this.Data = data;
        }
        #endregion
    }
}