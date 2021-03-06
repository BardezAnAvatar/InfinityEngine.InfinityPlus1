using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Components
{
    /// <summary>Structure representing the spell memorization area</summary>
    public class Creature2EMemorizedSpells : IInfinityFormat
    {
        #region Constants
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 12;
        #endregion


        #region Fields
        /// <summary>Resource to the spell prepared/memorized</summary>
        public ResourceReference Spell { get; set; }

        /// <summary>Flags indicating whether the spell is memorized and/or disabled.</summary>
        public MemorizationFlags Memorization { get; set; }
        #endregion


        #region Properties

        /// <summary>
        ///     Boolean indicating whether the spell is available
        ///     to be cast or if it has been used already.
        /// </summary>
        public Boolean Available
        {
            get { return this.Memorization == MemorizationFlags.Memorized; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public Creature2EMemorizedSpells()
        {
            this.Spell = null;
        }

        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.Spell = new ResourceReference();
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

            this.Spell.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 0, CultureConstants.CultureCodeEnglish);
            this.Memorization =  (MemorizationFlags)(ReusableIO.ReadUInt32FromArray(remainingBody, 8));
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.Spell.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream((UInt32)(this.Memorization), output);
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
            String header = "Creature 2E Memorized Spells:";

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
            return StringFormat.ReturnAndIndent(String.Format("Memorized Spells Entry # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Spell"));
            builder.Append(String.Format("'{0}'", this.Spell.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Memorization (value)"));
            builder.Append((UInt32)(this.Memorization));
            builder.Append(StringFormat.ToStringAlignment("Memorization (enumeration)"));
            builder.Append(this.GetMemorizationFlagsString());
            builder.Append(StringFormat.ToStringAlignment("Available to cast"));
            builder.Append(this.Available);

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which Memorization flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetMemorizationFlagsString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.Memorization & MemorizationFlags.Memorized) == MemorizationFlags.Memorized, MemorizationFlags.Memorized.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Memorization & MemorizationFlags.Disabled) == MemorizationFlags.Disabled, MemorizationFlags.Disabled.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}