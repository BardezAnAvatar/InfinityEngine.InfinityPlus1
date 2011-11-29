using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Components
{
    /// <summary>Known overlay entry</summary>
    public class Creature2EKnownSpell : IInfinityFormat
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 12;

        #region Members
        /// <summary>Resource to the overlay that is known</summary>
        protected ResourceReference spell;

        /// <summary>Level of the overlay.</summary>
        /// <remarks>0-indexed, so 0=1, 1=2, 2=3, etc.</remarks>
        protected UInt16 spellLevel;

        /// <summary>Type of overlay known</summary>
        protected Creature2EKnownSpellType type;
	    #endregion

        #region Properties
        /// <summary>Resource to the overlay that is known</summary>
        public ResourceReference Spell
        {
            get { return this.spell; }
            set { this.spell = value; }
        }

        /// <summary>Level of the overlay.</summary>
        /// <remarks>0-indexed, so 0=1, 1=2, 2=3, etc.</remarks>
        public UInt16 SpellLevel
        {
            get { return this.spellLevel; }
            set { this.spellLevel = value; }
        }

        /// <summary>Level of the overlay.</summary>
        public UInt16 SpellLevelActual
        {
            get { return Convert.ToUInt16(this.spellLevel + 1); }
            set { this.spellLevel = Convert.ToUInt16(value - 1); }
        }

        /// <summary>Type of overlay known</summary>
        public Creature2EKnownSpellType Type
        {
            get { return this.type; }
            set { this.type = value; }
        } 
        #endregion

        /// <summary>Default constructor</summary>
        public Creature2EKnownSpell()
        {
            this.spell = null;
        }

        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.spell = new ResourceReference();
        }

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
        /// <remarks>Calls readbody directly, as there exists no signature or version for this structure.</remarks>
        public void Read(Stream input, Boolean fullRead)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        public void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //read effect
            Byte[] remainingBody = ReusableIO.BinaryRead(input, 12);

            this.spell.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 0, Constants.CultureCodeEnglish);
            this.spellLevel = ReusableIO.ReadUInt16FromArray(remainingBody, 8);
            this.type = (Creature2EKnownSpellType)ReusableIO.ReadUInt16FromArray(remainingBody, 10);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.spell.ResRef, output, Constants.CultureCodeEnglish);
            ReusableIO.WriteUInt16ToStream(this.spellLevel, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.type, output);
        }
        #endregion

        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            return this.ToString(true);
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="showType">Boolean indicating whether or not to display the leading description line.</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Boolean showType)
        {
            String header = "Creature 2E Known spells:";

            if (showType)
                header += this.GetStringRepresentation();
            else
                header = this.GetStringRepresentation();

            return header;
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="entryIndex">Known spells entry #</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Int32 entryIndex)
        {
            return StringFormat.ReturnAndIndent(String.Format("Known Spells Entry # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Spell resource"));
            builder.Append(String.Format("'{0}'", this.spell.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Spell level"));
            builder.Append(this.spellLevel.ToString());
            builder.Append(StringFormat.ToStringAlignment("Spell level (actual)"));
            builder.Append(this.SpellLevelActual.ToString());
            builder.Append(StringFormat.ToStringAlignment("Spell Type"));
            builder.Append((UInt16)this.type);
            builder.Append(StringFormat.ToStringAlignment("Spell Type (description)"));
            builder.Append(this.type.GetDescription());

            return builder.ToString();
        }
        #endregion
    }
}