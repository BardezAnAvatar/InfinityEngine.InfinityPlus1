using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Creature2_2
{
	public class SpellLevelsGroup : IInfinityFormat
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 36;

        #region Members
        /// <summary>First-level spells</summary>
        protected UInt32 level1;

        /// <summary>Second-level spells</summary>
        protected UInt32 level2;

        /// <summary>Third-level spells</summary>
        protected UInt32 level3;

        /// <summary>Fourth-level spells</summary>
        protected UInt32 level4;

        /// <summary>Fifth-level spells</summary>
        protected UInt32 level5;

        /// <summary>Sixth-level spells</summary>
        protected UInt32 level6;

        /// <summary>Seventh-level spells</summary>
        protected UInt32 level7;

        /// <summary>Eighth-level spells</summary>
        protected UInt32 level8;

        /// <summary>Ninth-level spells</summary>
        protected UInt32 level9;
        #endregion

        #region Properties
        /// <summary>First-level spells</summary>
        public UInt32 Level1
        {
            get { return this.level1; }
            set { this.level1 = value; }
        }

        /// <summary>Second-level spells</summary>
        public UInt32 Level2
        {
            get { return this.level2; }
            set { this.level2 = value; }
        }

        /// <summary>Third-level spells</summary>
        public UInt32 Level3
        {
            get { return this.level3; }
            set { this.level3 = value; }
        }

        /// <summary>Fourth-level spells</summary>
        public UInt32 Level4
        {
            get { return this.level4; }
            set { this.level4 = value; }
        }

        /// <summary>Fifth-level spells</summary>
        public UInt32 Level5
        {
            get { return this.level5; }
            set { this.level5 = value; }
        }

        /// <summary>Sixth-level spells</summary>
        public UInt32 Level6
        {
            get { return this.level6; }
            set { this.level6 = value; }
        }

        /// <summary>Seventh-level spells</summary>
        public UInt32 Level7
        {
            get { return this.level7; }
            set { this.level7 = value; }
        }

        /// <summary>Eighth-level spells</summary>
        public UInt32 Level8
        {
            get { return this.level8; }
            set { this.level8 = value; }
        }

        /// <summary>Ninth-level spells</summary>
        public UInt32 Level9
        {
            get { return this.level9; }
            set { this.level9 = value; }
        }
        #endregion

        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
        }
        #endregion

        #region IO method implemetations
        /// <summary>This public method reads file format from the output stream. Reads the whole structure.</summary>
        /// <remarks>Calls readbody directly, as there exists no signature or version for this structure.</remarks>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        /// <remarks>Calls ReadBody directly, as there exists no signature or version for this structure.</remarks>
        public void Read(Stream input, Boolean fullRead)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from.</param>        
        public void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //read data
            this.ReadBody(ReusableIO.BinaryRead(input, 36), 0);
        }

        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="source">Byte Array to read from</param>
        /// <param name="offset">Offset of the byte array to start reading from</param>
        public void ReadBody(Byte[] source, Int32 offset)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            this.level1 = ReusableIO.ReadUInt32FromArray(source, offset);
            this.level2 = ReusableIO.ReadUInt32FromArray(source, offset + 4);
            this.level3 = ReusableIO.ReadUInt32FromArray(source, offset + 8);
            this.level4 = ReusableIO.ReadUInt32FromArray(source, offset + 12);
            this.level5 = ReusableIO.ReadUInt32FromArray(source, offset + 16);
            this.level6 = ReusableIO.ReadUInt32FromArray(source, offset + 20);
            this.level7 = ReusableIO.ReadUInt32FromArray(source, offset + 24);
            this.level8 = ReusableIO.ReadUInt32FromArray(source, offset + 28);
            this.level9 = ReusableIO.ReadUInt32FromArray(source, offset + 32);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteUInt32ToStream(this.level1, output);
            ReusableIO.WriteUInt32ToStream(this.level2, output);
            ReusableIO.WriteUInt32ToStream(this.level3, output);
            ReusableIO.WriteUInt32ToStream(this.level4, output);
            ReusableIO.WriteUInt32ToStream(this.level5, output);
            ReusableIO.WriteUInt32ToStream(this.level6, output);
            ReusableIO.WriteUInt32ToStream(this.level7, output);
            ReusableIO.WriteUInt32ToStream(this.level8, output);
            ReusableIO.WriteUInt32ToStream(this.level9, output);
        }
        #endregion
        
        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            return this.ToString(false, String.Empty, String.Empty);
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="showType">Boolean indicating whether or not to display the leading description line.</param>
        /// <param name="typeOfSpellGroup">String indicating which SpellGroup to print. Expected "Offset" or "Count"</param>
        /// <param name="spellClass">String indicating which class hte spell belongs to</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Boolean showType, String typeOfSpellGroup, String spellClass)
        {
            String header = "Creature spell " + typeOfSpellGroup + ":";

            if (showType)
                header += this.GetStringRepresentation(spellClass);
            else
                header = this.GetStringRepresentation(spellClass);

            return header;
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="entryIndex">Known spells entry #</param>
        /// <param name="typeOfSpellGroup">String indicating which SpellGroup to print. Expected "Offset" or "Count"</param>
        /// <param name="spellClass">String indicating which class hte spell belongs to</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Int32 entryIndex, String typeOfSpellGroup, String spellClass)
        {
            return String.Format("Creature " + typeOfSpellGroup + " spell group # {0}:", entryIndex) + this.GetStringRepresentation(spellClass);
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation(String spellClass)
        {
            StringBuilder builder = new StringBuilder();
            
            builder.Append(StringFormat.ToStringAlignment(spellClass + " Spell offset Level 1"));
            builder.Append(this.level1);
            builder.Append(StringFormat.ToStringAlignment(spellClass + " Spell offset Level 2"));
            builder.Append(this.level2);
            builder.Append(StringFormat.ToStringAlignment(spellClass + " Spell offset Level 3"));
            builder.Append(this.level3);
            builder.Append(StringFormat.ToStringAlignment(spellClass + " Spell offset Level 4"));
            builder.Append(this.level4);
            builder.Append(StringFormat.ToStringAlignment(spellClass + " Spell offset Level 5"));
            builder.Append(this.level5);
            builder.Append(StringFormat.ToStringAlignment(spellClass + " Spell offset Level 6"));
            builder.Append(this.level6);
            builder.Append(StringFormat.ToStringAlignment(spellClass + " Spell offset Level 7"));
            builder.Append(this.level7);
            builder.Append(StringFormat.ToStringAlignment(spellClass + " Spell offset Level 8"));
            builder.Append(this.level8);
            builder.Append(StringFormat.ToStringAlignment(spellClass + " Spell offset Level 9"));
            builder.Append(this.level9);

            return builder.ToString();
        }
        #endregion
    }
}