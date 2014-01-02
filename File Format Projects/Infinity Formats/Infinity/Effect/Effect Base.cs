using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect
{
    /// <summary>Base class for effects</summary>
    public abstract class EffectBase : IInfinityFormat
    {
        #region Members
        /// <summary>First parameter to the effect</summary>
        public Int32 Parameter1 { get; set; }

        /// <summary>Second parameter to the effect</summary>
        public Int32 Parameter2 { get; set; }

        /// <summary>Duration (rounds?) of the effect</summary>
        public UInt32 Duration { get; set; }

        /// <summary>Resource referenced by the effect</summary>
        public ResourceReference Resource1 { get; set; }

        /// <summary>Count of any dice thrown</summary>
        public UInt32 DiceCount { get; set; }

        /// <summary>Sides of any dice thrown</summary>
        public UInt32 DiceSides { get; set; }

        /// <summary>Saving throw type</summary>
        public EffectSavingThrow SavingThrowType { get; set; }

        /// <summary>Bonus or penalty to saving throw</summary>
        public Int32 SavingThrowModifier { get; set; }

        /// <summary>First unknown 4 bytes</summary>
        /// <remarks>This is marked "special" in the BG2 source code</remarks>
        public UInt32 Special { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public abstract void Initialize();
        #endregion


        #region Abstract IO methods
        /// <summary>This public method reads file format data structure from the output stream. Reads the whole data structure.</summary>
        public abstract void Read(Stream input);

        /// <summary>This public method reads file format data structure from the output stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public abstract void Read(Stream input, Boolean fullRead);

        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        public abstract void ReadBody(Stream input);

        /// <summary>This public method writes the file format data structure to the output stream.</summary>
        public abstract void Write(Stream output);
        #endregion
    }
}