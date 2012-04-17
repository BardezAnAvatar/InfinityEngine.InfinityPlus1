using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Character.Header
{
    /// <summary>Character header version 1</summary>
    public class Character1Header : CharacterHeaderBase
    {
        #region Constants
        /// <summary>Binary size of the struct on disk</summary>
        public Int32 StructSize
        {
            get { return 0x64; }
        }

        /// <summary>Length of binary data to read for minor fields</summary>
        private const Int32 ReadLength = 40;
        #endregion


        #region Fields
        /// <summary>Represents the collection of quickslots for this character</summary>
        public CharacterSlots_v1 QuickSlots { get; set; }
        #endregion


        #region Constructor(s)
        /// <summary>Default constructor</summary>
        public Character1Header()
        {
            this.QuickSlots = null;
        }

        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            base.Initialize();
            this.QuickSlots = new CharacterSlots_v1();
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //read remiander
            Byte[] header = ReusableIO.BinaryRead(input, Character1Header.ReadLength);

            this.Name.Value = ReusableIO.ReadStringFromByteArray(header, 0, CultureConstants.CultureCodeEnglish, 32);
            this.OffsetCreature = ReusableIO.ReadUInt32FromArray(header, 32);
            this.LengthCreature = ReusableIO.ReadUInt32FromArray(header, 36);
            this.QuickSlots.ReadBody(input);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.Name.Value, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt32ToStream(this.OffsetCreature, output);
            ReusableIO.WriteUInt32ToStream(this.LengthCreature, output);
            this.QuickSlots.Write(output);
        }
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
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(this.signature);
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(this.version);
            builder.Append(StringFormat.ToStringAlignment("Name"));
            builder.Append(this.Name.Value);
            builder.Append(StringFormat.ToStringAlignment("Creature offset"));
            builder.Append(this.OffsetCreature);
            builder.Append(StringFormat.ToStringAlignment("Creature length"));
            builder.Append(this.LengthCreature);

            builder.Append(this.QuickSlots.ToString());

            return builder.ToString();
        }
        #endregion
    }
}