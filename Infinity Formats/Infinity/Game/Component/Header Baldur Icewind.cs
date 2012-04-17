using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Component
{
    /// <summary>Represents a Baldur's Gate or Icewind Dale header</summary>
    public class HeaderBaldurIcewind : HeaderBase
    {
        #region Constants
        /// <summary>Represents the length of the trailing padding in Bytes</summary>
        private const Int32 PaddingLength = 52;

        /// <summary>Represents the length of the extended header data in Bytes</summary>
        private const Int32 ExtendedHeaderLength = 44;

        /// <summary>Size of the structure on disk</summary>
        public const Int32 StructSize = 180;
        #endregion


        #region Fields
        /// <summary>Indicates the game version/expansion number of the game file</summary>
        public Expansion ExpansionSet { get; set; }

        /// <summary>Offset within the stream to familiar options</summary>
        public Int32 FamiliarsOffset { get; set; }

        /// <summary>Offset within the stream to stored locations</summary>
        public Int32 StoredLocatonsOffset { get; set; }

        /// <summary>Count of stored locations</summary>
        public UInt32 StoredLocatonsCount { get; set; }

        /// <summary>Count of seconds playing the game</summary>
        public UInt32 RealTimeInGame { get; set; }

        /// <summary>Offset within the stream to pocket plane stored locations</summary>
        public Int32 PocketPlaneStoredLocatonsOffset { get; set; }

        /// <summary>Count of pocket plane stored locations</summary>
        public UInt32 PocketPlaneStoredLocatonsCount { get; set; }
        #endregion


        #region IInfinityFormat IO Methods
        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        public override void ReadBody(Stream input)
        {
            this.Initialize();

            this.ReadBaseHeader(input);

            Byte[] buffer = ReusableIO.BinaryRead(input, HeaderBaldurIcewind.ExtendedHeaderLength);

            this.Reputation = ReusableIO.ReadUInt32FromArray(buffer, 0);
            this.CurrentAreaCode.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 4, CultureConstants.CultureCodeEnglish);
            this.UserInterfaceFlags = (GuiFlags)ReusableIO.ReadUInt32FromArray(buffer, 12);
            this.ExpansionSet = (Expansion)ReusableIO.ReadUInt32FromArray(buffer, 16);
            this.FamiliarsOffset = ReusableIO.ReadInt32FromArray(buffer, 20);
            this.StoredLocatonsOffset = ReusableIO.ReadInt32FromArray(buffer, 24);
            this.StoredLocatonsCount = ReusableIO.ReadUInt32FromArray(buffer, 28);
            this.RealTimeInGame = ReusableIO.ReadUInt32FromArray(buffer, 32);
            this.PocketPlaneStoredLocatonsOffset = ReusableIO.ReadInt32FromArray(buffer, 36);
            this.PocketPlaneStoredLocatonsCount = ReusableIO.ReadUInt32FromArray(buffer, 40);
            this.Padding = ReusableIO.BinaryRead(input, HeaderBaldurIcewind.PaddingLength);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public override void Write(Stream output)
        {
            base.Write(output);
            this.WriteBaseHeader(output);
            ReusableIO.WriteUInt32ToStream(this.Reputation, output);
            ReusableIO.WriteStringToStream(CurrentAreaCode.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream((UInt32)this.UserInterfaceFlags, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.ExpansionSet, output);
            ReusableIO.WriteInt32ToStream(this.FamiliarsOffset, output);
            ReusableIO.WriteInt32ToStream(this.StoredLocatonsOffset, output);
            ReusableIO.WriteUInt32ToStream(this.StoredLocatonsCount, output);
            ReusableIO.WriteUInt32ToStream(this.RealTimeInGame, output);
            ReusableIO.WriteInt32ToStream(this.PocketPlaneStoredLocatonsOffset, output);
            ReusableIO.WriteUInt32ToStream(this.PocketPlaneStoredLocatonsCount, output);
            output.Write(this.Padding, 0, this.Padding.Length);
        }
        #endregion


        #region ToString methods
        /// <summary>Generates a human-readable String representation of the header</summary>
        /// <returns>A human-readable String representation of the header</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(base.ToString());
            builder.Append(StringFormat.ToStringAlignment("Reputation"));
            builder.Append(this.Reputation);
            builder.Append(StringFormat.ToStringAlignment("Current Area"));
            builder.Append(this.CurrentAreaCode.ZResRef);
            builder.Append(StringFormat.ToStringAlignment("User interface flags (value)"));
            builder.Append((UInt32)this.UserInterfaceFlags);
            builder.Append(StringFormat.ToStringAlignment("User interface flags (enumerated)"));
            builder.Append(this.GenerateGuiFlagsString());
            builder.Append(StringFormat.ToStringAlignment("Expansion (value)"));
            builder.Append((UInt32)this.ExpansionSet);
            builder.Append(StringFormat.ToStringAlignment("Expansion (enumerated)"));
            builder.Append(this.GenerateExpansionString());
            builder.Append(StringFormat.ToStringAlignment("Offset to Familiars"));
            builder.Append(this.FamiliarsOffset);
            builder.Append(StringFormat.ToStringAlignment("Offset to stored locations"));
            builder.Append(this.StoredLocatonsOffset);
            builder.Append(StringFormat.ToStringAlignment("Count of stored locations"));
            builder.Append(this.StoredLocatonsCount);
            builder.Append(StringFormat.ToStringAlignment("Game real time"));
            builder.Append(this.RealTimeInGame);
            builder.Append(StringFormat.ToStringAlignment("Offset to pocket plane stored locations"));
            builder.Append(this.PocketPlaneStoredLocatonsOffset);
            builder.Append(StringFormat.ToStringAlignment("Count of pocket plane stored locations"));
            builder.Append(this.PocketPlaneStoredLocatonsCount);
            builder.Append(StringFormat.ToStringAlignment("Padding"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding));

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which Weather flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GenerateExpansionString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.ExpansionSet & Expansion.None) == Expansion.None, Expansion.None.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ExpansionSet & Expansion.TotSC) == Expansion.TotSC, Expansion.TotSC.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ExpansionSet & Expansion.SoA) == Expansion.SoA, Expansion.SoA.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ExpansionSet & Expansion.IWD) == Expansion.IWD, Expansion.IWD.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ExpansionSet & Expansion.SoAMain) == Expansion.SoAMain, Expansion.SoAMain.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ExpansionSet & Expansion.ToB) == Expansion.ToB, Expansion.ToB.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}