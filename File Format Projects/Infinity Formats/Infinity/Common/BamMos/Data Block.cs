using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.BamMos
{
    /// <summary>Represents a MOS v2 data block</summary>
    public class DataBlock : IInfinityFormat
    {
        #region Constants
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 28;
        #endregion


        #region Fields
        /// <summary>The targeted PVRZ file whose name is in the format of MOSXXXX.pvrz</summary>
        public UInt32 PageNumber { get; set; }

        /// <summary>The start point's X co-ordinate within the targeted file</summary>
        public UInt32 OriginX { get; set; }

        /// <summary>The start point's Y co-ordinate within the targeted file</summary>
        public UInt32 OriginY { get; set; }

        /// <summary>The width of the sub-image</summary>
        public UInt32 Width { get; set; }

        /// <summary>The height of the sub-image</summary>
        public UInt32 Height { get; set; }

        /// <summary>The rendering point's X co-ordinate for the composited image</summary>
        public UInt32 RenderX { get; set; }

        /// <summary>The rendering point's Y co-ordinate within the composited image</summary>
        public UInt32 RenderY { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the related filename, excluding the file extension</summary>
        public String RelatedFileName
        {
            get { return String.Format("MOS{0}", this.PageNumber); }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize() { }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public virtual void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);
        }

        /// <summary>This public method reads file format from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] header = ReusableIO.BinaryRead(input, DataBlock.StructSize);
            this.PageNumber = ReusableIO.ReadUInt32FromArray(header, 0);
            this.OriginX = ReusableIO.ReadUInt32FromArray(header, 4);
            this.OriginY = ReusableIO.ReadUInt32FromArray(header, 8);
            this.Width = ReusableIO.ReadUInt16FromArray(header, 12);
            this.Height = ReusableIO.ReadUInt16FromArray(header, 16);
            this.RenderX = ReusableIO.ReadUInt16FromArray(header, 20);
            this.RenderY = ReusableIO.ReadUInt16FromArray(header, 24);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteUInt32ToStream(this.PageNumber, output);
            ReusableIO.WriteUInt32ToStream(this.OriginX, output);
            ReusableIO.WriteUInt32ToStream(this.OriginY, output);
            ReusableIO.WriteUInt32ToStream(this.Width, output);
            ReusableIO.WriteUInt32ToStream(this.Height, output);
            ReusableIO.WriteUInt32ToStream(this.RenderX, output);
            ReusableIO.WriteUInt32ToStream(this.RenderY, output);
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
            return "MOS version 2 Data block:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Page Number"));
            builder.Append(this.PageNumber);
            builder.Append(StringFormat.ToStringAlignment("Related File (no extension)"));
            builder.Append(this.RelatedFileName);
            builder.Append(StringFormat.ToStringAlignment("Source file's origin X coordinate"));
            builder.Append(this.OriginX);
            builder.Append(StringFormat.ToStringAlignment("Source file's origin X coordinate"));
            builder.Append(this.OriginY);
            builder.Append(StringFormat.ToStringAlignment("Width"));
            builder.Append(this.Width);
            builder.Append(StringFormat.ToStringAlignment("Height"));
            builder.Append(this.Height);
            builder.Append(StringFormat.ToStringAlignment("Columns"));
            builder.Append(this.RenderX);
            builder.Append(StringFormat.ToStringAlignment("Rows"));
            builder.Append(this.RenderY);

            return builder.ToString();
        }
        #endregion
    }
}