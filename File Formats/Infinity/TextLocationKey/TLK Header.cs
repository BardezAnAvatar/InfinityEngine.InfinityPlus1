using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
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
    public class TextLocationKeyHeader : InfinityFormat
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 18;

        #region Private Members
        /// <summary>This member contains the identifier representation for the language that the TLK file hold data in.</summary>
        private Int16 languageID;
        #endregion

        #region Public Properties
        /// <summary>This public property gets or sets the language of the Infinity Engine game</summary>
        public InfinityEngineLanguage Language
        {
            get { return (InfinityEngineLanguage)languageID; }
            set { languageID = (Int16)value; }
        }

        /// <summary>This property gets or sets the number of string references</summary>
        public Int32 StringReferenceCount { get; set; }

        /// <summary>This property gets or sets the offset to the first string reference in the TLK file</summary>
        public Int32 StringsReferenceOffset { get; set; }

        /// <summary>This property returns a culture name reference string based off of the language flag from the TLK file</summary>
        public String CultureReference
        {
            get
            {
                String culture;
                switch (Language)
                {
                    case InfinityEngineLanguage.French:
                        culture = CultureConstants.CultureCodeFrench;
                        break;
                    case InfinityEngineLanguage.German:
                        culture = CultureConstants.CultureCodeGerman;
                        break;
                    case InfinityEngineLanguage.Spanish:
                        culture = CultureConstants.CultureCodeSpanish;
                        break;
                    case InfinityEngineLanguage.English:
                    default:
                        culture = CultureConstants.CultureCodeEnglish;
                        break;
                }
                return culture;
            }
        }
        #endregion

        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize() { }
        #endregion

        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadBody(Stream input)
        {
            Byte[] buffer = ReusableIO.BinaryRead(input, 18);   //header buffer

            //In all Infinity Engine cases it should be 'TLK '.
            this.signature = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish, 4);

            //In all Infinity Engine cases it should be 'V1  '.
            this.version = ReusableIO.ReadStringFromByteArray(buffer, 4, CultureConstants.CultureCodeEnglish, 4);

            this.languageID = ReusableIO.ReadInt16FromArray(buffer, 8);
            this.StringReferenceCount = ReusableIO.ReadInt32FromArray(buffer, 10);
            this.StringsReferenceOffset = ReusableIO.ReadInt32FromArray(buffer, 14);
        }

        /// <summary>This public method writes the header to an output stream</summary>
        /// <param name="output">Stream object into which to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteInt16ToStream(this.languageID, output);
            ReusableIO.WriteInt32ToStream(this.StringReferenceCount, output);
            ReusableIO.WriteInt32ToStream(this.StringsReferenceOffset, output);
        }
        #endregion

        #region Public Methods
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A String containing the member data line by line</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ReturnAndIndent("TextLocationKeyHeader:", 0));
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(String.Format("'{0}'", this.signature));
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(String.Format("'{0}'", this.version));
            builder.Append(StringFormat.ToStringAlignment("Language ID"));
            builder.Append(this.languageID);
            builder.Append(StringFormat.ToStringAlignment("Language"));
            builder.Append(this.Language.ToString("G"));
            builder.Append(StringFormat.ToStringAlignment("StrRef Count"));
            builder.Append(this.StringReferenceCount);
            builder.Append(StringFormat.ToStringAlignment("StrRef Offset"));
            builder.Append(this.StringsReferenceOffset);

            return builder.ToString();
        }
        #endregion
    }
}