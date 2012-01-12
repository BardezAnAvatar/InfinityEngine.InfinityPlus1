using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Character.Biography
{
    /// <summary>Character biography, used in Icewind Dales with a *.res extension</summary>
    public class Biography2 : IInfinityFormat
    {
        #region Members
        /// <summary>Character biography</summary>
        /// <remarks>Z-String</remarks>
        protected ZString biography;
        #endregion

        #region Properties
        /// <summary>Character biography</summary>
        public ZString Biography
        {
            get { return this.biography; }
            set { this.biography = value; }
        }
        #endregion

        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.biography = new ZString();
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
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] bioBuffer = ReusableIO.BinaryRead(input, input.Length);
            this.biography.Source = ReusableIO.ReadStringFromByteArray(bioBuffer, 0, CultureConstants.CultureCodeEnglish, bioBuffer.Length);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.biography.Source, output, CultureConstants.CultureCodeEnglish, true);
        }
        #endregion

        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Character Biography version 2:");
            builder.Append(this.biography.Value.Replace("\n", "\r\n"));

            return builder.ToString();
        }
        #endregion
    }
}