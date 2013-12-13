using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Character.Header
{
    /// <summary>This class represents the slot availability for CHR files</summary>
    /// <remarks>Also embedded within GAM files, so I am breaking out for reusability</remarks>
    public abstract class CharacterSlotsBase : IInfinityFormat
    {
        #region Quasi-constants
        /// <summary>Exposes the count of Quick Weapon slots</summary>
        protected virtual Int32 QuickWeaponSlotCount
        {
            get { return 4; }
        }

        /// <summary>Exposes the count of Quick Spell slots</summary>
        protected virtual Int32 QuickSpellSlotCount
        {
            get { return 3; }
        }

        /// <summary>Exposes the count of Quick Item slots</summary>
        protected virtual Int32 QuickItemSlotCount
        {
            get { return 3; }
        }
        #endregion


        #region Fields
        /// <summary>Represents an array of indeces into SLOTS.IDS for Quick Weapons</summary>
        /// <value>(0xFFFF = none)</value>
        public Int16[] QuickWeaponSlotIndeces { get; set; }

        /// <summary>Represents an array of ResourceReferences for Quick Spells</summary>
        public ResourceReference[] QuickSpellSlotIndeces { get; set; }

        /// <summary>Represents an array of indeces into SLOTS.IDS for Quick Items</summary>
        /// <value>(0xFFFF = none)</value>
        public Int16[] QuickItemSlotIndeces { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.QuickWeaponSlotIndeces = new Int16[this.QuickWeaponSlotCount];
            this.QuickSpellSlotIndeces = new ResourceReference[this.QuickSpellSlotCount];
            this.QuickItemSlotIndeces = new Int16[this.QuickItemSlotCount];

            for (Int32 i = 0; i < this.QuickSpellSlotCount; ++i)
                this.QuickSpellSlotIndeces[i] = new ResourceReference();
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public virtual void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public abstract void ReadBody(Stream input);

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public abstract void Write(Stream output);
        #endregion


        #region ToString() helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            return this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected abstract String GetStringRepresentation();
        #endregion
    }
}