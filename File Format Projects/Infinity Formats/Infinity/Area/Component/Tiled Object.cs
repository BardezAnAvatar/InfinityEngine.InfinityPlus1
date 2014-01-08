using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Component
{
    /// <summary>Tiled objects</summary>
    public class TiledObject : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 108;
        #endregion


        #region Fields
        /// <summary>Script name of the tiled object</summary>
        public ZString Name { get; set; }

        /// <summary>Tile referenced by this object</summary>
        public ResourceReference Tile { get; set; }

        /// <summary>Flags associated with the tiled object</summary>
        public TiledObjectFlags Flags { get; set; }

        /// <summary>Index into point collection of first open search square point</summary>
        public Int32 OpenSearchSquareFirstIndex { get; set; }

        /// <summary>Count of open search square points</summary>
        public Int16 OpenSearchSquareCount { get; set; }

        /// <summary>Count of closed search square points</summary>
        public Int16 ClosedSearchSquareCount { get; set; }

        /// <summary>Index into point collection of first closed search square point</summary>
        public Int32 ClosedSearchSquareFirstIndex { get; set; }

        /// <summary>Unused 48 bytes at offset 0x003C</summary>
        public Byte[] Reserved_003C { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.Name = new ZString();
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] buffer = ReusableIO.BinaryRead(input, 0x3C);

            this.Name.Source = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish, 32);
            this.Tile.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 32, CultureConstants.CultureCodeEnglish);
            this.Flags = (TiledObjectFlags)ReusableIO.ReadUInt32FromArray(buffer, 40);
            this.OpenSearchSquareFirstIndex = ReusableIO.ReadInt32FromArray(buffer, 44);
            this.OpenSearchSquareCount = ReusableIO.ReadInt16FromArray(buffer, 46);
            this.ClosedSearchSquareCount = ReusableIO.ReadInt16FromArray(buffer, 48);
            this.ClosedSearchSquareFirstIndex = ReusableIO.ReadInt32FromArray(buffer, 50);
            this.Reserved_003C = ReusableIO.BinaryRead(input, 48);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.Name.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteStringToStream(this.Tile.ResRef, output, CultureConstants.CultureCodeEnglish, false, 8);
            ReusableIO.WriteUInt32ToStream((UInt32)this.Flags, output);
            ReusableIO.WriteInt32ToStream(this.OpenSearchSquareFirstIndex, output);
            ReusableIO.WriteInt16ToStream(this.OpenSearchSquareCount, output);
            ReusableIO.WriteInt16ToStream(this.ClosedSearchSquareCount, output);
            ReusableIO.WriteInt32ToStream(this.ClosedSearchSquareFirstIndex, output);
            output.Write(this.Reserved_003C, 0, 48);
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
            String header = this.GetVersionString();

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
            return StringFormat.ReturnAndIndent(String.Format("Entrance # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "Tiled Object:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Name"));
            builder.Append(String.Format("'{0}'", this.Name.Value));
            builder.Append(StringFormat.ToStringAlignment("Tile ID"));
            builder.Append(String.Format("'{0}'", this.Tile.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Flags (value)"));
            builder.Append((UInt32)this.Flags);
            builder.Append(StringFormat.ToStringAlignment("Flags (enumeration)"));
            builder.Append(this.GetTiledObjectFlagsEnumerationString());
            builder.Append(StringFormat.ToStringAlignment("Open Search Square first point index"));
            builder.Append(this.OpenSearchSquareFirstIndex);
            builder.Append(StringFormat.ToStringAlignment("Open Search Square point count"));
            builder.Append(this.OpenSearchSquareCount);
            builder.Append(StringFormat.ToStringAlignment("Closed Search Square first point index"));
            builder.Append(this.ClosedSearchSquareFirstIndex);
            builder.Append(StringFormat.ToStringAlignment("Closed Search Square point count"));
            builder.Append(this.ClosedSearchSquareCount);
            builder.Append(StringFormat.ToStringAlignment("Padding at offset 0x3C"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Reserved_003C));

            return builder.ToString();
        }

        /// <summary>Gets a human-readable enumeration String of set Flags enumeration values</summary>
        /// <returns>A human-readable enumeration String of set Flags enumeration values</returns>
        protected String GetTiledObjectFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.Flags & TiledObjectFlags.SecondaryState) == TiledObjectFlags.SecondaryState, TiledObjectFlags.SecondaryState.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & TiledObjectFlags.Transparent) == TiledObjectFlags.Transparent, TiledObjectFlags.Transparent.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}