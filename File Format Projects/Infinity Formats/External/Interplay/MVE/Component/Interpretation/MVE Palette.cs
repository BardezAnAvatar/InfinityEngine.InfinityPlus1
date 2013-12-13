using System;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Interpretation
{
    /// <summary>Non-frame chunk containing a message containing a video palette</summary>
    public class MvePalette : MveInterpretableChunk
    {
        #region Fields
        /// <summary>Palette opcode to replace (non-)existing palette</summary>
        public SetPalette Palette { get; set; }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="palette">Palette for this opcode chunk</param>
        public MvePalette(SetPalette palette)
        {
            this.Palette = palette;
        }
        #endregion
    }
}