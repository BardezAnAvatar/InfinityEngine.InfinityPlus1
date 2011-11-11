using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Base
{
    public abstract class InfinityFormat : IInfinityFormat
    {
        #region Members
        /// <summary>This member contains the signature of the file.</summary>
        /// <remarks>The smallest would probably be an encrypted 2DA: 0x00, 0x00</remarks>
        protected String signature;

        /// <summary>This member contains the version of the file format.</summary>
        protected String version;
        #endregion

        #region Public Properties
        /// <summary>This public property gets or sets the file Signature</summary>
        public String Signature
        {
            get { return this.signature; }
            set { this.signature = value; }
        }

        /// <summary>This public property gets or sets the string representation of the file version</summary>
        public String Version
        {
            get { return this.version; }
            set { this.version = value; }
        }
        #endregion
        
        /// <summary>Instantiates reference types</summary>
        public abstract void Initialize();

        #region Abstract IO methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            //read signature
            Byte[] buffer = ReusableIO.BinaryRead(input, 8);   //header buffer

            this.signature = ReusableIO.ReadStringFromByteArray(buffer, 0, Constants.CultureCodeEnglish, 4);
            this.version = ReusableIO.ReadStringFromByteArray(buffer, 4, Constants.CultureCodeEnglish, 4);

            this.ReadBody(input);
        }

        /// <summary>This public method reads file format from the output stream.</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public virtual void Read(Stream input, Boolean fullRead)
        {
            if (fullRead)
                this.Read(input);
            else
                this.ReadBody(input);
        }

        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public abstract void ReadBody(Stream input);

        /// <summary>This public method writes the file format data structure to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public abstract void Write(Stream output);
        #endregion
    }
}
