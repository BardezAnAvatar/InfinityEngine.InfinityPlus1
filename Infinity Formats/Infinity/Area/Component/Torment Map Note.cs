using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Component
{
    /// <summary>Represents an area's map note in PS:T</summary>
    public class TormentMapNote
    {
        #region Constants
        /// <summary>Represents the size of the structure on disk</summary>
        public const Int32 StructSize = 52;
        #endregion


        #region Fields
        /// <summary>Location of this map note</summary>
        public Point Location { get; set; }

        /// <summary>Text of this map note</summary>
        /// <value>500-byte ASCII text data</value>
        public ZString Text { get; set; }

        /// <summary>Color of the map note</summary>
        public TormentMapNoteColor Color { get; set; }

        /// <summary> 20 Bytes of padding at offset 0x200</summary>
        public Byte[] Padding_0x0200 { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Location = new Point();
            this.Text = new ZString();
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

            Byte[] buffer = ReusableIO.BinaryRead(input, 512);

            this.Location.X = ReusableIO.ReadUInt16FromArray(buffer, 0);    //Really 4 Bytes
            this.Location.Y = ReusableIO.ReadUInt16FromArray(buffer, 4);    //Really 4 Bytes
            this.Text.Value = ReusableIO.ReadStringFromByteArray(buffer, 8, CultureConstants.CultureCodeEnglish, 500);
            this.Color = (TormentMapNoteColor)ReusableIO.ReadUInt32FromArray(buffer, 508);

            this.Padding_0x0200 = ReusableIO.BinaryRead(input, 20);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteUInt32ToStream(this.Location.X, output);    //Really 4 Bytes
            ReusableIO.WriteUInt32ToStream(this.Location.Y, output);    //Really 4 Bytes
            ReusableIO.WriteStringToStream(this.Text.Value, output, CultureConstants.CultureCodeEnglish, false, 500);
            ReusableIO.WriteUInt32ToStream((UInt16)this.Color, output);
            output.Write(this.Padding_0x0200, 0, 20);
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
            return StringFormat.ReturnAndIndent(String.Format("Map note # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "Map note:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Map note location"));
            builder.Append(this.Location.ToString());
            builder.Append(StringFormat.ToStringAlignment("Map note text"));
            builder.Append(String.Format("'{0}'", this.Text.Value));
            builder.Append(StringFormat.ToStringAlignment("Color (value)"));
            builder.Append((UInt16)this.Color);
            builder.Append(StringFormat.ToStringAlignment("Color (description)"));
            builder.Append(this.Color.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Padding @ offset 0x200"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding_0x0200));

            return builder.ToString();
        }
        #endregion
    }
}