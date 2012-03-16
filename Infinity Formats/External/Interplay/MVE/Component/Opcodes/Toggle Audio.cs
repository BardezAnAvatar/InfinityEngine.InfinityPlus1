using System;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes
{
    /// <summary>Represents an end-of-stream opcode to cease all decoding</summary>
    public class ToggleAudio : OpcodeData
    {
        #region Properties
        /// <summary>Exposes the opcode enum for this opcode</summary>
        public override OpcodeTypes Opcode
        {
            get { return OpcodeTypes.ToggleAudio; }
        }
        #endregion
    }
}