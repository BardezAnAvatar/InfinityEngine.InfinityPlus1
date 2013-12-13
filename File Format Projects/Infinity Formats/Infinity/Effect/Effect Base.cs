using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect
{
    public abstract class EffectBase : IInfinityFormat
    {
        #region Members
        /// <summary>First parameter to the effect</summary>
        protected Int32 parameter1;

        /// <summary>Second parameter to the effect</summary>
        protected Int32 parameter2;

        /// <summary>Duration (rounds?) of the effect</summary>
        protected UInt32 duration;

        /// <summary>Resource referenced by the effect</summary>
        protected ResourceReference resource1;

        /// <summary>Count of any dice thrown</summary>
        protected UInt32 diceCount;

        /// <summary>Sides of any dice thrown</summary>
        protected UInt32 diceSides;

        /// <summary>Saving throw type</summary>
        protected EffectSavingThrow savingThrowType;

        /// <summary>Bonus or penalty to saving throw</summary>
        protected Int32 savingThrowModifier;

        /// <summary>First unknown 4 bytes</summary>
        protected UInt32 unknown1;
        #endregion

        #region Properties
        /// <summary>First parameter to the effect</summary>
        protected Int32 Parameter1
        {
            get { return this.parameter1; }
            set { this.parameter1 = value; }
        }

        /// <summary>second parameter to the effect</summary>
        protected Int32 Parameter2
        {
            get { return this.parameter2; }
            set { this.parameter2 = value; }
        }

        /// <summary>Duration (rounds?) of the effect</summary>
        protected UInt32 Duration
        {
            get { return this.duration; }
            set { this.duration = value; }
        }

        /// <summary>Resource referenced by the effect</summary>
        protected ResourceReference Resource1
        {
            get { return this.resource1; }
            set { this.resource1 = value; }
        }

        /// <summary>Count of any dice thrown</summary>
        protected UInt32 DiceCount
        {
            get { return this.diceCount; }
            set { this.diceCount = value; }
        }

        /// <summary>Sides of any dice thrown</summary>
        protected UInt32 DiceSides
        {
            get { return this.diceSides; }
            set { this.diceSides = value; }
        }

        /// <summary>Saving throw type</summary>
        protected EffectSavingThrow SavingThrowType
        {
            get { return this.savingThrowType; }
            set { this.savingThrowType = value; }
        }

        /// <summary>Bonus or penalty to saving throw</summary>
        protected Int32 SavingThrowModifier
        {
            get { return this.savingThrowModifier; }
            set { this.savingThrowModifier = value; }
        }

        /// <summary>First unknown 4 bytes</summary>
        protected UInt32 Unknown1
        {
            get { return this.unknown1; }
            set { this.unknown1 = value; }
        }
        #endregion

        /// <summary>Instantiates reference types</summary>
        public abstract void Initialize();

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