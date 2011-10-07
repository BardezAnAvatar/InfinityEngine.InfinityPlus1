using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Components
{
    /// <summary>Known overlay entry</summary>
    public class CreatureD20KnownSpell : IInfinityFormat
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 16;

        #region Members
        /// <summary>Index into the appropraite 2DA file indicating which overlay this is</summary>
        protected Int32 index2da;

        /// <summary>Amount of overlay prepared</summary>
        protected UInt32 amountPrepared;

        /// <summary>Amount of overlay remaining</summary>
        protected UInt32 amountRemaining;

        /// <summary>Unknown trailing 4 bytes</summary>
        protected UInt32 unknown;
	    #endregion

        #region Properties
        /// <summary>Index into the appropraite 2DA file indicating which overlay this is</summary>
        public Int32 Index2da
        {
            get { return this.index2da; }
            set { this.index2da = value; }
        }

        /// <summary>Amount of overlay prepared</summary>
        public UInt32 AmountPrepared
        {
            get { return this.amountPrepared; }
            set { this.amountPrepared = value; }
        }

        /// <summary>Amount of overlay remaining</summary>
        public UInt32 AmountRemaining
        {
            get { return this.amountRemaining; }
            set { this.amountRemaining = value; }
        }

        /// <summary>Unknown trailing 4 bytes</summary>
        public UInt32 Unknown
        {
            get { return this.unknown; }
            set { this.unknown = value; }
        }
        #endregion

        #region Constructor(s)
        /// <summary>Default constructor</summary>
        public CreatureD20KnownSpell()
        {
        }

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
        /// <remarks>Calls readbody directly, as there exists no signature or version for this structure.</remarks>
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

            //read effect
            Byte[] remainingBody = ReusableIO.BinaryRead(input, 16);
            
            this.index2da = ReusableIO.ReadInt32FromArray(remainingBody, 0);
            this.amountPrepared = ReusableIO.ReadUInt32FromArray(remainingBody, 4);
            this.amountRemaining = ReusableIO.ReadUInt32FromArray(remainingBody, 8);
            this.unknown = ReusableIO.ReadUInt32FromArray(remainingBody, 12);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteInt32ToStream(this.index2da, output);
            ReusableIO.WriteUInt32ToStream(this.amountPrepared, output);
            ReusableIO.WriteUInt32ToStream(this.amountRemaining, output);
            ReusableIO.WriteUInt32ToStream(this.unknown, output);
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
            String header = "Creature D20 Known spells:";

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
            return String.Format("Known Spells Entry # {0}:", entryIndex) + this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("\n\tSpell 2DA entry index:                          ");
            builder.Append(this.index2da);
            builder.Append("\n\tNumber prepared:                                ");
            builder.Append(this.amountPrepared);
            builder.Append("\n\tNumber uncast:                                  ");
            builder.Append(this.amountRemaining);
            builder.Append("\n\tUnknown:                                        ");
            builder.Append(this.unknown);
            builder.Append("\n\n");

            return builder.ToString();
        }
        #endregion
    }
}