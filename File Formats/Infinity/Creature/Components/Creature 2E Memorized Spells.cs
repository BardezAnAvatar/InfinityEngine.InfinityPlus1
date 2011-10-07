using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Components
{
    /// <summary>Structure representing the overlay memorization area</summary>
    public class Creature2EMemorizedSpells : IInfinityFormat
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 12;

        #region Members
        /// <summary>Resource to the overlay prepared/memorized</summary>
        protected ResourceReference spell;

        /// <summary>
        ///     Boolean indicating whether the overlay is available
        ///     to be cast or if it has been used already.
        /// </summary>
        protected UInt32 memorized;
        #endregion

        #region Properties
        /// <summary>Resource to the overlay prepared/memorized</summary>
        public ResourceReference Spell
        {
            get { return this.spell; }
            set { this.spell = value; }
        }

        /// <summary>
        ///     Value indicating whether the overlay is available
        ///     to be cast or if it has been used already.
        /// </summary>
        public UInt32 Memorized
        {
            get { return this.memorized; }
            set { this.memorized = value; }
        }

        /// <summary>
        ///     Boolean indicating whether the overlay is available
        ///     to be cast or if it has been used already.
        /// </summary>
        public Boolean Available
        {
            get { return Convert.ToBoolean(this.memorized); }
            set { this.memorized = Convert.ToUInt32(value); }
        }
        #endregion

        #region Constructor(s)
        /// <summary>Default constructor</summary>
        public Creature2EMemorizedSpells()
        {
            this.spell = null;
        }

        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.spell = new ResourceReference();
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

            this.spell.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 0, Constants.CultureCodeEnglish);
            this.memorized = ReusableIO.ReadUInt32FromArray(remainingBody, 8);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.spell.ResRef, output, Constants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream(this.memorized, output);
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
            return String.Format("Memorized Spells Entry # {0}:", entryIndex) + this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\n\tSpell:                                          '");
            builder.Append(this.spell.ResRef);
            builder.Append("'");
            builder.Append("\n\tMemorized:                                      ");
            builder.Append(this.memorized);
            builder.Append("\n\tAvailable to cast:                              ");
            builder.Append(this.Available);
            builder.Append("\n\n");

            return builder.ToString();
        }
        #endregion
    }
}