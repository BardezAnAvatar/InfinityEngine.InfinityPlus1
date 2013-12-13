using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Mosaic
{
    /// <summary>Represents the 24-byte MOS file header</summary>
    /// <remarks>It is of some significance that this is so similar to JPEG. I am not sure how much, though.</remarks>
    public class MosHeader : InfinityFormat
    {
        #region Constants
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 24;
        #endregion


        #region Fields
        /// <summary>Width, in pixels, of the MOS file</summary>
        public UInt16 Width { get; set; }

        /// <summary>Height, in pixels, of the MOS file</summary>
        public UInt16 Height { get; set; }

        /// <summary>Count of vertical block columns</summary>
        public UInt16 Columns { get; set; }

        /// <summary>Count of horizontal block rows</summary>
        public UInt16 Rows { get; set; }

        /// <summary>The size of blocks</summary>
        public UInt32 BlockSize { get; set; }

        /// <summary>Offset from start of file to palette entries</summary>
        public UInt32 PaletteOffset { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the total count of 8x8 blocks in this MOS file</summary>
        public Int32 BlockCount
        {
            get { return this.Columns * this.Rows; }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize() { }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] header = ReusableIO.BinaryRead(input, MosHeader.StructSize - 8);
            this.Width = ReusableIO.ReadUInt16FromArray(header, 0);
            this.Height = ReusableIO.ReadUInt16FromArray(header, 2);
            this.Columns = ReusableIO.ReadUInt16FromArray(header, 4);
            this.Rows = ReusableIO.ReadUInt16FromArray(header, 6);
            this.BlockSize = ReusableIO.ReadUInt32FromArray(header, 8);
            this.PaletteOffset = ReusableIO.ReadUInt32FromArray(header, 12);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteUInt16ToStream(this.Width, output);
            ReusableIO.WriteUInt16ToStream(this.Height, output);
            ReusableIO.WriteUInt16ToStream(this.Columns, output);
            ReusableIO.WriteUInt16ToStream(this.Rows, output);
        }
        #endregion


        #region ToString() methods
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

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "MOS header:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(this.signature);
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(this.version);
            builder.Append(StringFormat.ToStringAlignment("Width"));
            builder.Append(this.Width);
            builder.Append(StringFormat.ToStringAlignment("Height"));
            builder.Append(this.Height);
            builder.Append(StringFormat.ToStringAlignment("Columns"));
            builder.Append(Columns);
            builder.Append(StringFormat.ToStringAlignment("Rows"));
            builder.Append(this.Rows);
            builder.Append(StringFormat.ToStringAlignment("Block Count"));
            builder.Append(this.BlockCount);
            builder.Append(StringFormat.ToStringAlignment("Block Size"));
            builder.Append(this.BlockSize);
            builder.Append(StringFormat.ToStringAlignment("Palette Offset"));
            builder.Append(this.PaletteOffset);

            return builder.ToString();
        }
        #endregion
    }
}