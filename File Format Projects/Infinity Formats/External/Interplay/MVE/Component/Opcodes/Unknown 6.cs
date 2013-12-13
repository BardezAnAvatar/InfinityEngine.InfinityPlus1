using System;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes
{
    /// <summary>Represents unknown opcode #6</summary>
    public class RotateVideoBuffers : OpcodeData
    {
        #region Properties
        /// <summary>Exposes the opcode enum for this opcode</summary>
        public override OpcodeTypes Opcode
        {
            get { return OpcodeTypes.RotateVideoBuffers; }
        }
        #endregion
    }
}