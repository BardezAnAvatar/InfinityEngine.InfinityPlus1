using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect.Effect2
{
    /// <summary>
    ///     Inner wrapper of the Effect version 2 core.
    ///     Secondary signatures make it necessary for the wrapping given reusable code base, and the first version's lack of stand-alone format with headers.
    /// </summary>
    public class Effect2 : InfinityFormat
    {
        #region Constants
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 272;
        #endregion


        #region Fields
        /// <summary>Inner signature-wrapped effect version 2</summary>
        public Effect2Inner Inner { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public Effect2()
        {
            this.Inner = null;
        }

        /// <summary>Instantiates the members</summary>
        public override void Initialize()
        {
            this.Inner = new Effect2Inner();
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            this.Inner.Read(input);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);
            this.Inner.Write(output);
        }
        #endregion


        #region ToString() helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Effect Version 2:");
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(this.signature);
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(this.version);
            builder.Append(this.Inner.ToString());

            return builder.ToString();
        }
        #endregion
    }
}