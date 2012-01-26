using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common
{
    /// <summary>Represents an image (fragment) palette entry of Infinity format images</summary>
    /// <remarks>BAM, MOS and TIS all have 256-color palettes, regardless of how many are used.</remarks>
    public class PaletteEntry : IInfinityFormat
    {
        #region Constants
        /// <summary>Count of color entries in the palette</summary>
        public const Int32 PaletteSize = 256;
        #endregion


        #region Fields
        /// <summary>Array of color entries</summary>
        public ColorEntry[] Colors { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.Colors = new ColorEntry[PaletteEntry.PaletteSize];
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

            for (Int32 index = 0; index < PaletteEntry.PaletteSize; ++index)
            {
                ColorEntry ce = new ColorEntry();
                ce.Read(input);
                this.Colors[index] = ce;
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            for (Int32 index = 0; index < PaletteEntry.PaletteSize; ++index)
                this.Colors[index].Write(output);
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
            String header = PaletteEntry.GetVersionString();

            if (showType)
                header += this.GetStringRepresentation();
            else
                header = this.GetStringRepresentation();

            return header;
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected static String GetVersionString()
        {
            return "Palette Entry:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            for (Int32 index = 0; index < PaletteEntry.PaletteSize; ++index)
            {
                builder.Append(String.Format("Index {0}:", index));
                builder.Append(this.Colors[index].ToString(true));
            }

            return builder.ToString();
        }
        #endregion
    }
}