using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.PackedLayeredTexture.Component;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.PackedLayeredTexture.Version
{
    /// <summary>Represents a PLT file</summary>
    public class PixelTable : IInfinityFormat
    {
        #region Fields
        /// <summary>Header to the save game format</summary>
        public Header Header { get; set; }

        /// <summary>Collection of pixels</summary>
        public List<Pixel> Pixels { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Header = new Header();
            this.Pixels = new List<Pixel>();
        }
        #endregion


        #region I/O Methods
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
            if (fullRead)
                this.Read(input);
            else
            {
                this.Header = new Header();
                this.Header.Read(input, false);
            }
        }

        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //header
            this.Header.Read(input);

            Int32 pixelCount = this.Header.Height * this.Header.Width;

            for (Int32 index = 0; index < pixelCount; ++index)
            {
                Pixel pixel = new Pixel();
                pixel.Read(input);
                this.Pixels.Add(pixel);
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public virtual void Write(Stream output)
        {
            //header
            this.Header.Write(output);

            foreach (Pixel pixel in this.Pixels)
                pixel.Write(output);
        }
        #endregion


        #region ToString methods
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("PST file:");
            builder.AppendLine(this.Header.ToString());
            builder.AppendLine(this.GeneratePixelString());

            return builder.ToString();
        }

        /// <summary>Generates a human-readable console output describing the party</summary>
        /// <returns>A multi-line String</returns>
        protected String GeneratePixelString()
        {
            StringBuilder sb = new StringBuilder();

            Int32 pixelCount = this.Header.Height * this.Header.Width;

            for (Int32 i = 0; i < pixelCount; ++i)
                sb.Append(this.Pixels[i].ToString(i + 1));

            return sb.ToString();
        }
        #endregion
    }
}