using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Components
{
    /// <summary>Structure representing the overlay memorization area</summary>
    public class CreatureD20SpellMemorization : IInfinityFormat
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 8;

        #region Members
        /// <summary>Total number of overlay slots</summary>
        protected UInt32 slotsTotal;

        /// <summary>Remaining overlay slots memorizable</summary>
        protected UInt32 slotsRemaining;
        #endregion

        #region Properties
        /// <summary>Total number of overlay slots</summary>
        protected UInt32 SlotsTotal
        {
            get { return this.slotsTotal; }
            set { this.slotsTotal = value; }
        }

        /// <summary>Remaining overlay slots memorizable</summary>
        protected UInt32 SlotsRemaining
        {
            get { return this.slotsRemaining; }
            set { this.slotsRemaining = value; }
        }
        #endregion

        /// <summary>Instantiates reference types</summary>
        public void Initialize() { }

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
            Byte[] remainingBody = ReusableIO.BinaryRead(input, 8);

            this.slotsTotal = ReusableIO.ReadUInt32FromArray(remainingBody, 0);
            this.slotsRemaining = ReusableIO.ReadUInt32FromArray(remainingBody, 4);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        public void Write(Stream output)
        {
            ReusableIO.WriteUInt32ToStream(this.slotsTotal, output);
            ReusableIO.WriteUInt32ToStream(this.slotsRemaining, output);
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
            String header = "Creature D20 Spell Preparation:";

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
            return StringFormat.ReturnAndIndent(String.Format("Spell level memorization Entry # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Total memorization slots"));
            builder.Append(this.slotsTotal);
            builder.Append(StringFormat.ToStringAlignment("Remaining memorization slots"));
            builder.Append(this.slotsRemaining);

            return builder.ToString();
        }
        #endregion
    }
}