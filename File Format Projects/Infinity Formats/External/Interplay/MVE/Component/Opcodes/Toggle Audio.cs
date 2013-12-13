using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes
{
    /// <summary>Represents an opcode to cease toggle audio playback</summary>
    public class ToggleAudio : OpcodeData
    {
        #region IO method implemetations
        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
        }
        #endregion
    }
}