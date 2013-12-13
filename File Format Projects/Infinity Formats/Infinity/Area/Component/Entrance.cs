using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Component
{
    /// <summary>Represents an entrance for an area</summary>
    public class Entrance : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of the structure on disk</summary>
        public const Int32 StructSize = 104;
        #endregion


        #region Fields
        /// <summary>Entrance name</summary>
        public ZString Name { get; set; }

        /// <summary>Location of the entrance</summary>
        public Point Location { get; set; }

        /// <summary>Represents the orientation of the entrance</summary>
        public Direction Orientation { get; set; }

        /// <summary>66 padding Bytes at offset 0x26</summary>
        public Byte[] Padding_0x0026 { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Name = new ZString();
            this.Location = new Point();
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

            Byte[] buffer = ReusableIO.BinaryRead(input, 38);

            this.Name.Source = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish, 32);
            this.Location.X = ReusableIO.ReadUInt16FromArray(buffer, 32);
            this.Location.Y = ReusableIO.ReadUInt16FromArray(buffer, 34);
            this.Orientation = (Direction)ReusableIO.ReadUInt16FromArray(buffer, 36);

            this.Padding_0x0026 = ReusableIO.BinaryRead(input, 66);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.Name.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt16ToStream(this.Location.X, output);
            ReusableIO.WriteUInt16ToStream(this.Location.Y, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.Orientation, output);

            output.Write(this.Padding_0x0026, 0, 66);
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
            return "Entrance:";
        }
            
        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Name"));
            builder.Append(String.Format("'{0}'", this.Name.Value));
            builder.Append(StringFormat.ToStringAlignment("Location"));
            builder.Append(this.Location.ToString());
            builder.Append(StringFormat.ToStringAlignment("Orientation (value)"));
            builder.Append((UInt32)this.Orientation);
            builder.Append(StringFormat.ToStringAlignment("Orientation (description)"));
            builder.Append(this.Orientation.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Padding at offset 0x26"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding_0x0026));

            return builder.ToString();
        }
        #endregion
    }
}