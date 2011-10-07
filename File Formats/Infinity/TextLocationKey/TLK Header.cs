using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.TextLocationKey
{
    /// <summary>This struct is the header to a dialog.tlk file</summary>
    /// <remarks>
    ///     Taken from the Infinity Engine (Infinity Engine Structures Description Project):
    ///     
    ///     Offset      Size (data type)  	Description
    ///     0x0000 	    4 (char array) 	    Signature ('TLK ')
    ///     0x0004 	    4 (char array) 	    Version ('V1 ')
    ///     0x0008 	    2 (word) 	        Language ID
    ///     0x000a 	    4 (dword) 	        Number of strref entries in this file
    ///     0x000e 	    4 (dword) 	        Offset to string data
    /// </remarks>
    public struct TextLocationKeyHeader
    {
        #region Private Members
        /// <summary>This member contains the signature of the file. In all Infinity Engine cases it should be 'TLK '.</summary>
        private String signature;

        /// <summary>This member contains the version of the file format. In all Infinity Engine cases it should be 'V1  '.</summary>
        private String version;

        /// <summary>This member contains the identifier representation for the language that the TLK file hold data in.</summary>
        private Int16 languageID;

        /// <summary>This member contains the number of StringReferences in the TLK file</summary>
        private Int32 stringRefCount;

        /// <summary>This member contains the offset within the file, at which is the point where text strings are first located.</summary>
        private Int32 stringsOffset;
        #endregion

        #region Public Properties
        /// <summary>This public property gets or sets the file Signature</summary>
        public String Signature
        {
            get { return signature; }
            set { signature = value; }
        }

        /// <summary>This public property gets or sets the string representation of the file version</summary>
        public String Version
        {
            get { return version; }
            set { version = value; }
        }

        /// <summary>This public property gets or sets the language of the Infinity Engine game</summary>
        public InfinityEngineLanguage Language
        {
            get { return (InfinityEngineLanguage)languageID; }
            set { languageID = (Int16)value; }
        }

        /// <summary>This public property gets or sets the number of string references</summary>
        public Int32 StringReferenceCount
        {
            get { return stringRefCount; }
            set { stringRefCount = value; }
        }

        /// <summary>This public property gets or sets the offset to the first string reference in the TLK file</summary>
        public Int32 StringsReferenceOffset
        {
            get { return stringsOffset; }
            set { stringsOffset = value; }
        }

        /// <summary>This public property returns a culture name reference string based off of the language flag from the TLK file</summary>
        public String CultureReference
        {
            get
            {
                String culture;
                switch (Language)
                {
                    case InfinityEngineLanguage.French:
                        culture = Constants.CultureCodeFrench;
                        break;
                    case InfinityEngineLanguage.German:
                        culture = Constants.CultureCodeGerman;
                        break;
                    case InfinityEngineLanguage.Spanish:
                        culture = Constants.CultureCodeSpanish;
                        break;
                    case InfinityEngineLanguage.English:
                    default:
                        culture = Constants.CultureCodeEnglish;
                        break;
                }
                return culture;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>This public method reads the 18-byte header into the header record</summary>
        public void ReadHeader(Stream Input)
        {
            Byte[] buffer = ReusableIO.BinaryRead(Input, 18);   //header buffer
            Byte[] temp = new Byte[4];
            Encoding encoding = new ASCIIEncoding();

            //signature
            Array.Copy(buffer, temp, 4);
            signature = encoding.GetString(temp);

            //version
            Array.Copy(buffer, 4, temp, 0, 4);
            version = encoding.GetString(temp);

            //languageID
            languageID = ReusableIO.ReadInt16FromArray(buffer, 0x8);

            //StrRef count
            stringRefCount = ReusableIO.ReadInt32FromArray(buffer, 0xA);

            //StrRef offset
            stringsOffset = ReusableIO.ReadInt32FromArray(buffer, 0xE);
        }

        /// <summary>This public method writes the header to an output stream</summary>
        /// <param name="Output">Stream object into which to write to</param>
        public void Write(Stream Output)
        {
            Byte[] writeBytes;

            //signature
            writeBytes = ReusableIO.WriteStringToByteArray(this.signature, 4);
            Output.Write(writeBytes, 0, writeBytes.Length);

            //version
            writeBytes = ReusableIO.WriteStringToByteArray(this.version, 4);
            Output.Write(writeBytes, 0, writeBytes.Length);

            //Language ID
            writeBytes = BitConverter.GetBytes(this.languageID);
            Output.Write(writeBytes, 0, writeBytes.Length);

            //stringRefCount
            writeBytes = BitConverter.GetBytes(this.stringRefCount);
            Output.Write(writeBytes, 0, writeBytes.Length);

            //String data offset
            writeBytes = BitConverter.GetBytes(this.stringsOffset);
            Output.Write(writeBytes, 0, writeBytes.Length);
        }

        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A String containing the member data line by line</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("TextLocationKeyHeader:");
            builder.Append("\n    Signature:  '");
            builder.Append(signature);
            builder.Append("'\n    Version:  '");
            builder.Append(version);
            builder.Append("'\n    Language ID:  ");
            builder.Append(languageID);
            builder.Append("\n    Language:  ");
            builder.Append(Language.ToString("G"));
            builder.Append("\n    StrRef Count:  ");
            builder.Append(stringRefCount);
            builder.Append("\n    StrRef Offset:  ");
            builder.Append(stringsOffset);
            builder.Append("\n\n");

            return builder.ToString();
        }
        #endregion
    }
}