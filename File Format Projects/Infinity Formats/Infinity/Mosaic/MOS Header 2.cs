using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Mosaic
{
    /// <summary>The header for a Mosaic V2 file</summary>
    public class MosaicHeader_v2 : InfinityFormat
    {
        #region Constants
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 16;
        #endregion


        #region Fields
        /// <summary>Width of the image</summary>
        public UInt16 Width { get; set; }

        /// <summary>Height of the image</summary>
        public UInt16 Height { get; set; }

        /// <summary>Count of blocks</summary>
        public Int16 BlockCount { get; set; }

        /// <summary>Offset within the data file to where blocks are located</summary>
        public UInt16 BlockOffset { get; set; }
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

            Byte[] header = ReusableIO.BinaryRead(input, MosaicHeader_v2.StructSize - 8);
            this.Width = ReusableIO.ReadUInt16FromArray(header, 0);
            this.Height = ReusableIO.ReadUInt16FromArray(header, 2);
            this.BlockCount = ReusableIO.ReadInt16FromArray(header, 4);
            this.BlockOffset = ReusableIO.ReadUInt16FromArray(header, 6);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteUInt16ToStream(this.Width, output);
            ReusableIO.WriteUInt16ToStream(this.Height, output);
            ReusableIO.WriteInt16ToStream(this.BlockCount, output);
            ReusableIO.WriteUInt16ToStream(this.BlockOffset, output);
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
            return "MOS version 2 header:";
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
            builder.Append(StringFormat.ToStringAlignment("Block Count"));
            builder.Append(this.BlockCount);
            builder.Append(StringFormat.ToStringAlignment("Block Offset"));
            builder.Append(this.BlockOffset);

            return builder.ToString();
        }
        #endregion
    }
}