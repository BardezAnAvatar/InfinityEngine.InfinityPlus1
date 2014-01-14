using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TileSet.Tis1
{
    /// <summary>Represents an enhanced edition's tileset block</summary>
    public class TileSetBlock : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 12;
        #endregion


        #region Fields
        /// <summary>The targeted PVRZ file whose name is in the format of -----XX.pvrz</summary>
        public UInt32 PageNumber { get; set; }

        /// <summary>The start point's X co-ordinate within the targeted file</summary>
        public UInt32 OriginX { get; set; }

        /// <summary>The start point's Y co-ordinate within the targeted file</summary>
        public UInt32 OriginY { get; set; }

        /// <summary>The related (area? tileset?) code key</summary>
        /// <remarks>Typically A-, O- or T- XXXX (such as: AR2500 => 2500; AR0700N => 0700N; TU0015 => 0015; OH3100N => 3100N)</remarks>
        public String TileSetKey { get; set; }

        /// <summary>The first character of the tileset's name</summary>
        public Char TileSetPrefix { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the related filename, excluding the file extension</summary>
        public String RelatedFileName
        {
            get { return String.Format("{0}{1}{2:2}", this.TileSetPrefix, this.TileSetKey, this.PageNumber); }
        }
        #endregion


        #region Construction
        /// <summary>Partial definition constructor</summary>
        /// <param name="tilesetPrefix">First character of the tileset's prefix (OH => O, AR => A, TU => T)</param>
        /// <param name="tilesetCode">4- or 5-character string representing the tileset's area code (AR4500 => 4500; AR0700 => 0700, OH9360 => 9360; AR0700N => 0700N; etc.)</param>
        public TileSetBlock(Char tilesetPrefix, String tilesetCode)
        {
            this.TileSetPrefix = tilesetPrefix;
            this.TileSetKey = tilesetCode;
        }

        /// <summary>Instantiates reference types</summary>
        public void Initialize() { }
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

            Byte[] header = ReusableIO.BinaryRead(input, TileSetBlock.StructSize);
            this.PageNumber = ReusableIO.ReadUInt32FromArray(header, 0);
            this.OriginX = ReusableIO.ReadUInt32FromArray(header, 4);
            this.OriginY = ReusableIO.ReadUInt32FromArray(header, 8);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteUInt32ToStream(this.PageNumber, output);
            ReusableIO.WriteUInt32ToStream(this.OriginX, output);
            ReusableIO.WriteUInt32ToStream(this.OriginY, output);
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
            return "Tile Set (PVRZ) tile block:";
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

            return builder.ToString();
        }
        #endregion
    }
}