using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.PixelLocationTable.Component
{
    /// <summary>Represents a PLT file's header</summary>
    public class Header : InfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 0x18;
        #endregion


        #region Fields
        /// <summary>Represents the count of materials in this file</summary>
        /// <value>Always 8, and coincidentially there are always 8 colors.</value>
        public Int32 CountColors { get; set; }

        /// <summary>Unknown 4 Bytes at offset 0x0C</summary>
        public Int32 Unknown_0x000C { get; set; }

        /// <summary>Width of this image</summary>
        public Int32 Width { get; set; }

        /// <summary>Height of this image</summary>
        public Int32 Height { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize() { }
        #endregion


        #region IInfinityFormat IO Methods
        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        public override void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] buffer = ReusableIO.BinaryRead(input, 16);

            this.CountColors = ReusableIO.ReadInt32FromArray(buffer, 0);
            this.Unknown_0x000C = ReusableIO.ReadInt32FromArray(buffer, 4);
            this.Width = ReusableIO.ReadInt32FromArray(buffer, 8);
            this.Height = ReusableIO.ReadInt32FromArray(buffer, 12);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public override void Write(Stream output)
        {
            base.Write(output);

            ReusableIO.WriteInt32ToStream(this.CountColors, output);
            ReusableIO.WriteInt32ToStream(this.Unknown_0x000C, output);
            ReusableIO.WriteInt32ToStream(this.Width, output);
            ReusableIO.WriteInt32ToStream(this.Height, output);
        }
        #endregion


        #region ToString methods
        /// <summary>Generates a human-readable String representation of the formations</summary>
        /// <returns>A human-readable String representation of the formations</returns>
        public override String ToString()
        {
            return this.GetStringRepresentation();
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
            return "PLT Header:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected virtual String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Count of materials/colors"));
            builder.Append(this.CountColors);
            builder.Append(StringFormat.ToStringAlignment("Unknown @ ofset 0x0C"));
            builder.Append(this.Unknown_0x000C);
            builder.Append(StringFormat.ToStringAlignment("Width"));
            builder.Append(this.Width);
            builder.Append(StringFormat.ToStringAlignment("Height"));
            builder.Append(this.Height);

            return builder.ToString();
        }
        #endregion
    }
}