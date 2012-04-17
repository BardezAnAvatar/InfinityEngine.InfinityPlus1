using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Component
{
    /// <summary>Represents an area's map note in BG, BG2, ID and IWD2</summary>
    public class CommonMapNote : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of the structure on disk</summary>
        public const Int32 StructSize = 52;
        #endregion


        #region Fields
        /// <summary>Location of this map note</summary>
        public Point Location { get; set; }

        /// <summary>Strref to string representing the note</summary>
        public StringReference Text { get; set; }

        /// <summary>Flag indicating that this string resides in dialog.tlk</summary>
        public Boolean InDialogTlk { get; set; }

        /// <summary>Color of the map note</summary>
        public MapNoteColor Color { get; set; }

        /// <summary> 40 Bytes of padding at offset 0x0C</summary>
        public Byte[] Padding_0x000C { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Location = new Point();
            this.Text = new StringReference();
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

            Byte[] buffer = ReusableIO.BinaryRead(input, 12);

            this.Location.X = ReusableIO.ReadUInt16FromArray(buffer, 0);
            this.Location.Y = ReusableIO.ReadUInt16FromArray(buffer, 2);
            this.Text.StringReferenceIndex = ReusableIO.ReadInt32FromArray(buffer, 4);
            this.InDialogTlk = Convert.ToBoolean(ReusableIO.ReadUInt16FromArray(buffer, 8));
            this.Color = (MapNoteColor)ReusableIO.ReadUInt16FromArray(buffer, 10);
            this.Padding_0x000C = ReusableIO.BinaryRead(input, 40);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteUInt16ToStream(this.Location.X, output);
            ReusableIO.WriteUInt16ToStream(this.Location.Y, output);
            ReusableIO.WriteInt32ToStream(this.Text.StringReferenceIndex, output);
            ReusableIO.WriteUInt16ToStream(Convert.ToUInt16(this.InDialogTlk), output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.Color, output);
            output.Write(this.Padding_0x000C, 0, 40);
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
            builder.Append(StringFormat.ToStringAlignment("Map note text (strref)"));
            builder.Append(this.Text.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Is strref in Dialog.tlk"));
            builder.Append(this.InDialogTlk);
            builder.Append(StringFormat.ToStringAlignment("Color (value)"));
            builder.Append((UInt16)this.Color);
            builder.Append(StringFormat.ToStringAlignment("Color (description)"));
            builder.Append(this.Color.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Padding @ offset 0x0C"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding_0x000C));

            return builder.ToString();
        }
        #endregion
    }
}