using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Effect.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Effect.Effect2
{
    /// <summary>
    ///     Inner wrapper of the Effect version 2 core.
    ///     Secondary signatures make it necessary for the wrapping given reusable code base, and the first version's lack of stand-alone format with headers.
    /// </summary>
    public class Effect2 : InfinityFormat
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 272;
        
        #region Members
        /// <summary>Inner signature-wrapped effect version 2</summary>
        protected Effect2Inner inner; 
        #endregion

        #region Properties
		/// <summary>Inner signature-wrapped effect version 2</summary>
        public Effect2Inner Inner
        {
            get { return this.inner; }
            set { this.inner = value; }
        } 
	    #endregion

        #region Constructor(s)
        /// <summary>Default constructor</summary>
        public Effect2()
        {
            this.inner = null;
        }
        #endregion

        /// <summary>Instantiates the members</summary>
        public override void Initialize()
        {
            this.inner = new Effect2Inner();
        }

        #region IO method implemetations
        /// <summary>This public method reads file format from the output stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void Read(Stream input)
        {
            //read signature
            Byte[] buffer = ReusableIO.BinaryRead(input, 8);   //header buffer

            this.signature = ReusableIO.ReadStringFromByteArray(buffer, 0, Constants.CultureCodeEnglish, 4);
            this.version = ReusableIO.ReadStringFromByteArray(buffer, 4, Constants.CultureCodeEnglish, 4);

            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            this.inner.Read(input);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, Constants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, Constants.CultureCodeEnglish, false, 4);
            this.inner.Write(output);
        }
        #endregion

        #region ToString() helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Effect Version 2:");
            builder.Append("\n\tSignature:                                      ");
            builder.Append(this.signature);
            builder.Append("\n\tVersion:                                        ");
            builder.Append(this.version);
            builder.Append(this.inner.ToString());

            return builder.ToString();
        }
        #endregion
    }
}
