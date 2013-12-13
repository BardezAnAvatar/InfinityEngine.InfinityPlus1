using System;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes
{
    /// <summary>Represents an end-of-chunk opcode to cease reading the chunk</summary>
    public class EndOfChunk : OpcodeData
    {
        #region Properties
        /// <summary>Exposes the opcode enum for this opcode</summary>
        public override OpcodeTypes Opcode
        {
            get { return OpcodeTypes.EndOfChunk; }
        }
        #endregion
    }
}