using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common
{
    /// <summary>Represents a single color entry for an Infinity Engine palette</summary>
    /// <remarks>
    ///     This is a 24-bit palette, stored as 32 bits;
    ///     it is *not* RGBA; it is simply stored byte-aligned to 4 bytes.
    ///     Stored in B G R 0 order
    /// </remarks>
    public class ColorEntry : IInfinityFormat
    {
        #region Constants
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 4;
        #endregion


        #region Fields
        /// <summary>Represents the red channel of the color</summary>
        public Byte Red { get; set; }

        /// <summary>Represents the green channel of the color</summary>
        public Byte Green { get; set; }

        /// <summary>Represents the blue channel of the color</summary>
        public Byte Blue { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize() { }
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

            Byte[] color = ReusableIO.BinaryRead(input, ColorEntry.StructSize);

            this.Blue = color[0];
            this.Green = color[1];
            this.Red = color[2];
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            output.WriteByte(this.Blue);
            output.WriteByte(this.Green);
            output.WriteByte(this.Red);
            output.WriteByte(Byte.MinValue);    //Byte padding
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
        /// <param name="fast">Boolean indicating whether or not to display the hexidecimal representation of the color.</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public virtual String ToString(Boolean fast)
        {
            String output;

            if (fast)
                output = this.GetHexStringRepresentation();
            else
                output = this.GetStringRepresentation();

            return output;
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected virtual String GetVersionString()
        {
            return "Color:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected virtual String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Red"));
            builder.Append(this.Red);
            builder.Append(StringFormat.ToStringAlignment("Green"));
            builder.Append(this.Green);
            builder.Append(StringFormat.ToStringAlignment("Blue"));
            builder.Append(this.Blue);
            builder.Append(StringFormat.ToStringAlignment("Hex"));
            builder.Append(this.GetHexStringRepresentation());

            return builder.ToString();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected virtual String GetHexStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Hex"));
            builder.Append(String.Format("#{0:X2}{1:X2}{2:X2}", this.Red, this.Green, this.Blue));

            return builder.ToString();
        }
        #endregion
    }
}