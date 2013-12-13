using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Character.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Character.Header
{
    /// <summary>Character header version 2.2</summary>
    public class Character2_2Header : CharacterHeaderBase
    {
        #region Constants
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 0x224;
        #endregion


        #region Fields
        /// <summary>Represents the collection of quickslots for this character</summary>
        public CharacterSlots_v2_2 QuickSlots { get; set; }

        /// <summary>Unknown 2 Bytes after configurable slots</summary>
        public UInt16 Unknown2 { get; set; }

        /// <summary>Level of the associated spell slot</summary>
        public Byte LevelSpell1 { get; set; }

        /// <summary>Level of the associated spell slot</summary>
        public Byte LevelSpell2 { get; set; }

        /// <summary>Level of the associated spell slot</summary>
        public Byte LevelSpell3 { get; set; }

        /// <summary>Level of the associated spell slot</summary>
        public Byte LevelSpell4 { get; set; }

        /// <summary>Level of the associated spell slot</summary>
        public Byte LevelSpell5 { get; set; }

        /// <summary>Level of the associated spell slot</summary>
        public Byte LevelSpell6 { get; set; }

        /// <summary>Level of the associated spell slot</summary>
        public Byte LevelSpell7 { get; set; }

        /// <summary>Level of the associated spell slot</summary>
        public Byte LevelSpell8 { get; set; }

        /// <summary>Level of the associated spell slot</summary>
        public Byte LevelSpell9 { get; set; }

        /// <summary>Third unknown byte after spell slot levels</summary>
        /// <remarks>Probably unused</remarks>
        public Byte Unknown3 { get; set; }

        /// <summary>14 unknown bytes</summary>
        public Byte[] Unknown4 { get; set; }

        /// <summary>Soundset resource reference</summary>
        public ResourceReference SoundSet { get; set; }

        /// <summary>Biff archive in which the soundset is stored. 20 Bytes.</summary>
        public ZString SoundSetBiff { get; set; }

        /// <summary>128 reserved bytes. Probably unused.</summary>
        public Byte[] Reserved { get; set; }
        #endregion

        
        #region Constructor(s)
        /// <summary>Default constructor</summary>
        public Character2_2Header()
        {
            this.QuickSlots = null;
            this.Unknown4 = null;
            this.SoundSet = null;
            this.Reserved = null;
        }

        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            base.Initialize();

            this.QuickSlots = new CharacterSlots_v2_2();

            this.Unknown4 = new Byte[14];
            this.SoundSet = new ResourceReference();
            this.SoundSetBiff = new ZString();
            this.Reserved = new Byte[128];
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //read Name & offsets
            Byte[] header = ReusableIO.BinaryRead(input, 40);

            this.Name.Source = ReusableIO.ReadStringFromByteArray(header, 0, CultureConstants.CultureCodeEnglish, 32);
            this.OffsetCreature = ReusableIO.ReadUInt32FromArray(header, 32);
            this.LengthCreature = ReusableIO.ReadUInt32FromArray(header, 36);

            //Read quick slots
            this.QuickSlots.ReadBody(input);

            //read header remainder
            header = ReusableIO.BinaryRead(input, 194);

            this.Unknown2 = ReusableIO.ReadUInt16FromArray(header, 0); //346
            this.LevelSpell1 = header[2];
            this.LevelSpell2 = header[3];
            this.LevelSpell3 = header[4];
            this.LevelSpell4 = header[5];
            this.LevelSpell5 = header[6];
            this.LevelSpell6 = header[7];
            this.LevelSpell7 = header[8];
            this.LevelSpell8 = header[9];
            this.LevelSpell9 = header[10];
            this.Unknown3 = header[11];
            Array.Copy(header, 12, this.Unknown4, 0, 14);
            this.SoundSet.ResRef = ReusableIO.ReadStringFromByteArray(header, 26, CultureConstants.CultureCodeEnglish);
            this.SoundSetBiff.Source = ReusableIO.ReadStringFromByteArray(header, 34, CultureConstants.CultureCodeEnglish, 32);
            Array.Copy(header, 66, this.Reserved, 0, 128);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.Name.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt32ToStream(this.OffsetCreature, output);
            ReusableIO.WriteUInt32ToStream(this.LengthCreature, output);
            this.QuickSlots.Write(output);
            ReusableIO.WriteUInt16ToStream(this.Unknown2, output);
            output.WriteByte(this.LevelSpell1);
            output.WriteByte(this.LevelSpell2);
            output.WriteByte(this.LevelSpell3);
            output.WriteByte(this.LevelSpell4);
            output.WriteByte(this.LevelSpell5);
            output.WriteByte(this.LevelSpell6);
            output.WriteByte(this.LevelSpell7);
            output.WriteByte(this.LevelSpell8);
            output.WriteByte(this.LevelSpell9);
            output.WriteByte(this.Unknown3);
            output.Write(this.Unknown4, 0, 14);
            ReusableIO.WriteStringToStream(this.SoundSet.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.SoundSetBiff.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            output.Write(this.Reserved, 0, 128);
        }
        #endregion


        #region ToString() helpers
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

            builder.Append(this.QuickSlots.ToString());

            builder.Append(StringFormat.ToStringAlignment("Unknown #2 (Padding?)"));
            builder.Append(this.Unknown2);
            builder.Append(StringFormat.ToStringAlignment("Level of quick spell 1"));
            builder.Append(this.LevelSpell1);
            builder.Append(StringFormat.ToStringAlignment("Level of quick spell 2"));
            builder.Append(this.LevelSpell2);
            builder.Append(StringFormat.ToStringAlignment("Level of quick spell 3"));
            builder.Append(this.LevelSpell3);
            builder.Append(StringFormat.ToStringAlignment("Level of quick spell 4"));
            builder.Append(this.LevelSpell4);
            builder.Append(StringFormat.ToStringAlignment("Level of quick spell 5"));
            builder.Append(this.LevelSpell5);
            builder.Append(StringFormat.ToStringAlignment("Level of quick spell 6"));
            builder.Append(this.LevelSpell6);
            builder.Append(StringFormat.ToStringAlignment("Level of quick spell 7"));
            builder.Append(this.LevelSpell7);
            builder.Append(StringFormat.ToStringAlignment("Level of quick spell 8"));
            builder.Append(this.LevelSpell8);
            builder.Append(StringFormat.ToStringAlignment("Level of quick spell 9"));
            builder.Append(this.LevelSpell9);
            builder.Append(StringFormat.ToStringAlignment("Unknown #3 (Padding?)"));
            builder.Append(this.Unknown3);
            builder.Append(StringFormat.ToStringAlignment("Unknown #4"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Unknown4));
            builder.Append(StringFormat.ToStringAlignment("Sound set prefix"));
            builder.Append("'");
            builder.Append(this.SoundSet.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Sound set archive"));
            builder.Append("'");
            builder.Append(this.SoundSetBiff.Value);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Trailing reservd bytes"));
            builder.AppendLine(StringFormat.ByteArrayToHexString(this.Reserved));

            return builder.ToString();
        }
        #endregion
    }
}