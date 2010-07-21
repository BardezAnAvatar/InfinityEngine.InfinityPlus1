﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using InfinityPlus1.ReusableCode;

namespace InfinityPlus1.Files
{
    /// <summary>This enumerator describes the language of the Infinity Engine game running</summary>
    public enum InfinityEngineLanguage : short /* Int16 */
    {
        English = 0,
        French,     // = 1
        Spanish,    // ?
        German      // = 3
    }

    /// <summary>This enumerator describes the flags assigned to a String Reference entry</summary>
    [Flags]
    public enum InfinityEngineStringReferenceFlags : short /* Int16 */
    {
        None    = 0,
        Text    = 1,
        Sound   = 2,
        Tags    = 4
    }

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
                        culture = "fr";
                        break;
                    case InfinityEngineLanguage.German:
                        culture = "de";
                        break;
                    case InfinityEngineLanguage.Spanish:
                        culture = "en-US";
                        break;
                    case InfinityEngineLanguage.English:
                    default:
                        culture = "es";
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
            languageID = ReusableIO.ReadInt16FromArray(ref buffer, 0x8);

            //StrRef count
            stringRefCount = ReusableIO.ReadInt32FromArray(ref buffer, 0xA);

            //StrRef offset
            stringsOffset = ReusableIO.ReadInt32FromArray(ref buffer, 0xE);
        }

        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A String containing the member data line by line</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("TextLocationKeyHeader:");
            builder.Append("\n    Signature:  '");
            builder.Append(signature);
            builder.Append("'\n      Version:  '");
            builder.Append(version);
            builder.Append("'\n  Language ID:  ");
            builder.Append(languageID);
            builder.Append("\n     Language:  ");
            builder.Append(Language.ToString("G"));
            builder.Append("\n StrRef Count:  ");
            builder.Append(stringRefCount);
            builder.Append("\nStrRef Offset:  ");
            builder.Append(stringsOffset);
            builder.Append("\n\n");

            return builder.ToString();
        }
        #endregion
    }

    /// <summary>This public struct is the entry for a string reference within the TLK file</summary>
    /// <remarks>
    ///     Taken from the Infinity Engine (Infinity Engine Structures Description Project):
    ///     
    ///     Offset  	Size (data type)  	Description
    ///     0x0000 	    2 (word) 	        Flags
    ///                                         * 00 - No message data
    ///                                         * 01 - Text exists
    ///                                         * 02 - Sound exists
    ///                                         * 03 - Standard message. Ambient message. Used for sound without text (BG1) or message displayed over characters head (BG2), Message with tags (for instance &lt;CHARNAME&gt;) for all games except BG2
    ///                                         * 07 - Message with tags (for instance &lt;CHARNAME&gt; ) in BG2 only
    ///     0x0002 	    8 (resref) 	        Resource name of associated sound
    ///     0x000a 	    4 (dword) 	        Volume variance
    ///     0x000e 	    4 (dword) 	        Pitch variance
    ///     0x0012 	    4 (dword) 	        Offset of this string relative to the strings section
    ///     0x0016 	    4 (dword) 	        Length of this string
    /// </remarks>
    public struct TextLocationKeyStringReference
    {
        #region Private Members
        /// <summary>This Int16 member is a flag field of options for the string entry</summary>
        private Int16 strRefFlags;

        /// <summary>This is a reference to the appropriate</summary>
        private ResourceReference soundResRef;

        /// <summary>This is the related sound's volume variance</summary>
        private Int32 volumeVariance;

        /// <summary>This is the related sound's pitch variance</summary>
        private Int32 pitchVariance;

        /// <summary>This is the offset to the string being refrenced</summary>
        private Int32 stringOffset;

        /// <summary>This is the length of the string being referenced</summary>
        private Int32 stringLength;

        /// <summary>This is the actual string referenced</summary>
        private String stringReferenced;
        #endregion

        #region Public Properties
        /// <summary>This public property gets and sets a collection of reference state flags for the string entry</summary>
        public InfinityEngineStringReferenceFlags ReferenceFlags
        {
            get { return (InfinityEngineStringReferenceFlags)strRefFlags; }
            set { strRefFlags = (Int16)value; }
        }

        /// <summary>This public property gets and sets the associated sound's resource reference</summary>
        public ResourceReference SoundResourceReference
        {
            get { return soundResRef; }
            set { soundResRef = value; }
        }

        /// <summary>This public property gets and sets the associated sound's volume variance. There is little explination of this.</summary>
        public Int32 VolumeVariance
        {
            get { return volumeVariance; }
            set { volumeVariance = value; }
        }

        /// <summary>This public property gets and sets the associated sound's pitch variance. There is little explination of this.</summary>
        public Int32 PitchVariance
        {
            get { return pitchVariance; }
            set { pitchVariance = value; }
        }

        /// <summary>This public property gets and sets the offset to the string</summary>
        public Int32 StringOffset
        {
            get { return stringOffset; }
            set { stringOffset = value; }
        }

        /// <summary>This public property gets and sets the length of the string being referenced</summary>
        public Int32 StringLength
        {
            get { return stringLength; }
            set { stringLength = value; }
        }

        /// <summary>This public property gets and sets the string referenced. Setting this will update the stringLength value</summary>
        public String StringReferenced
        {
            get { return stringReferenced; }
            set
            {
                stringReferenced = value;
                Encoding encoding = new ASCIIEncoding();
                stringLength = encoding.GetByteCount(value);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>This method reads a fullString reference</summary>
        /// <param name="Source">Binary Stream to read from</param>
        /// <param name="StringDataOffset">Offset in the stream where the String data begins</param>
        /// <param name="CultureReference">String representing the source culture</param>
        public void ReadStringReferenceFull(Stream Source, Int32 StringDataOffset, String CultureReference)
        {
            ReadStringReferenceEntry(Source, CultureReference);
            ReadStringReferenced(Source, StringDataOffset, CultureReference);
        }

        /// <summary>This method reads the String Reference from the TLK file.</summary>
        /// <param name="Source">The binary Stream to read from</param>
        /// <remarks>Stream must be in the proper location to read, as there is no native seek/offset in the file structure</remarks>
        /// <param name="CultureReference">String representing the source culture</param>
        public void ReadStringReferenceEntry(Stream Source, String CultureReference)
        {
            //read entry
            Byte[] buffer = ReusableIO.BinaryRead(Source, 26);

            //Flags
            strRefFlags = ReusableIO.ReadInt16FromArray(ref buffer, 0);

            //sound resref
            soundResRef = new ResourceReference();
            soundResRef.ResRef = ReusableIO.ReadStringFromByteArray(ref buffer, 2, CultureReference);

            //volume variance
            volumeVariance = ReusableIO.ReadInt32FromArray(ref buffer, 0xA);

            //pitch variance
            pitchVariance = ReusableIO.ReadInt32FromArray(ref buffer, 0xE);

            //string offset
            stringOffset = ReusableIO.ReadInt32FromArray(ref buffer, 0x12);

            //string offset
            stringLength = ReusableIO.ReadInt32FromArray(ref buffer, 0x16);
        }

        /// <summary>This public member overrides the default ToString() method</summary>
        /// <returns>A string containing the values and descriptions of all values in this struct</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("TextLocationKeyStringReference:");
            builder.Append("\n     String Flags: ");
            builder.Append(strRefFlags);
            //print the flags
            if (ReferenceFlags == InfinityEngineStringReferenceFlags.None)
                builder.Append("\n                      * None");
            if ((ReferenceFlags & InfinityEngineStringReferenceFlags.Text) == InfinityEngineStringReferenceFlags.Text)
                builder.Append("\n                      * Text");
            if ((ReferenceFlags & InfinityEngineStringReferenceFlags.Sound) == InfinityEngineStringReferenceFlags.Sound)
                builder.Append("\n                      * Sound");
            if ((ReferenceFlags & InfinityEngineStringReferenceFlags.Tags) == InfinityEngineStringReferenceFlags.Tags)
                builder.Append("\n                      * Tags");

            builder.Append("\n     Sound ResRef: '");
            builder.Append(soundResRef.ResRef);
            builder.Append("'\n  Volume Variance: ");
            builder.Append(volumeVariance);
            builder.Append("\n   Pitch Variance: ");
            builder.Append(pitchVariance);
            builder.Append("\n    String Offset: ");
            builder.Append(stringOffset);
            builder.Append("\n    String Length: ");
            builder.Append(stringLength);
            builder.Append("\nString Referenced: ");
            builder.Append(stringReferenced);               
            builder.Append("\n\n");

            return builder.ToString();
        }

        /// <summary>This private method reads the referenced string from the file</summary>
        /// <param name="Input">Stream to read from</param>
        /// <param name="StringDataOffset">Offset to the strt of the string data</param>
        /// <param name="CultureReference">String representing the source culture</param>
        public void ReadStringReferenced(Stream Input, Int32 StringDataOffset, String CultureReference)
        {
            //seek if necessary
            Int64 absoluteOffset = StringDataOffset + stringOffset;
            ReusableIO.SeekIfAble(Input, absoluteOffset, SeekOrigin.Begin);

            Byte[] buffer = ReusableIO.BinaryRead(Input, stringLength);
            StringReferenced = ReusableIO.ReadStringFromByteArray(ref buffer, 0, CultureReference, stringLength);
        }
        #endregion
    }

    /// <summary>This class represents a collection of String References</summary>
    public class TextLocationKeyStringReferenceCollection
    {
        #region Protected Members
        #endregion

        #region Public Properties
        /// <summary>
        ///     This publoc property gets and sets the Boolean indicates whether or not to store strings in memory when the TLK file is read.
        ///     This is used mostly to retain the ability to use low levels of RAM.
        /// </summary>
        public Boolean StoreStringsInMemory
        {
            get { return storeStringsInMemory; }
            set { storeStringsInMemory = value; }
        }

        /// <summary>This protected member stores the path of the TLK file</summary>
        public String TlkPath
        {
            get { return tlkFilePath; }
            set { tlkFilePath = value; }
        }

        /// <summary>This public Property returns the string reference at the index indicated.</summary>
        /// <param name="Index">Int32 representing the place in the array at which to get or set the string reference</param>
        /// <returns>A TextLocationKeyStringReference string reference object</returns>
        public TextLocationKeyStringReference this[Int32 Index]
        {
            get
            {
                TextLocationKeyStringReference strref = (TextLocationKeyStringReference)(stringReferences[Index]);
                if (strref.StringReferenced == null)
                {
                    using (Stream fileStream = ReusableIO.OpenFile(tlkFilePath))
                    {
                        strref.ReadStringReferenced(fileStream, header.StringsReferenceOffset, header.CultureReference);    //read
                    }
                }
                return strref;
            }
            set { stringReferences[Index] = value; }
        }

        /// <summary>This public property gets the length of the array of string references</summary>
        public Int32 Length
        {
            get { return stringReferences.Count; }
        }
        #endregion

        #region Constructor(s)
        /// <summary>Default constructor</summary>
        public TextLocationKeyStringReferenceCollection()
        {
            stringReferences = new ArrayList();
        }

        /// <summary>Store strings in memory constructor</summary>
        /// <param name="StoreStrings">Boolean indicating hether or not to store strings in memory</param>
        public TextLocationKeyStringReferenceCollection(Boolean StoreStrings)
        {
            stringReferences = new ArrayList();
            storeStringsInMemory = StoreStrings;
        }

        /// <summary>Store strings in memory constructor</summary>
        /// <param name="StoreStrings">Boolean indicating hether or not to store strings in memory</param>
        /// <param name="FilePath">String representing the path to a TLK file</param>
        public TextLocationKeyStringReferenceCollection(Boolean StoreStrings, String FilePath)
        {
            stringReferences = new ArrayList();
            storeStringsInMemory = StoreStrings;
            tlkFilePath = FilePath;
        }
        #endregion

        #region Public Properties
        /// <summary>Adds a String Regerence to the TLK file</summary>
        /// <param name="StringReference">String reference to add to the TLK file</param>
        public void Add(TextLocationKeyStringReference StringReference)
        {
            stringReferences.Add(StringReference);
        }
        #endregion

        #region Public Methods
        /// <summary>Enumerates through all the TextLocationKeyStringReference objects and generates their strings in one collection.</summary>
        /// <returns>A String representation of the entire collection of TextLocationKeyStringReference objects</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Store Strings in memory:    ");
            builder.Append(this.storeStringsInMemory);
            builder.Append("\nPath to TLK file:           ");
            builder.Append(this.tlkFilePath);

            //String references
            for(Int32 index = 0; index < stringReferences.Count; ++index)
            {
                builder.Append("String Reference Index:    ");
                builder.Append(index);
                builder.Append(stringReferences[index].ToString());
            }

            return builder.ToString();
        }
        #endregion
    }

    public class TextLocationKey
    {
        #region Protected Members
        /// <summary>This protected member contains the header information of the TLK file</summary>
        protected TextLocationKeyHeader header;

        /// <summary>This protected member contains the entries of the String References, indexed by the String Reference number</summary>
        protected TextLocationKeyStringReferenceCollection stringReferences;

        /// <summary>This protected member indicates whether or not to store strings in memory when the TLK file is read. This is used mostly to retain the ability to use low levels of RAM.</summary>
        protected Boolean storeStringsInMemory;

        /// <summary>This protected member stores the path of the TLK file</summary>
        protected String tlkFilePath;
        #endregion

        #region Public Properties
        /// <summary>This public property contains the header information of the TLK file</summary>
        public TextLocationKeyHeader Header
        {
            get { return header; }
            set { header = value; }
        }

        /// <summary>This public property contains the entries of the String References, indexed by the String Reference number</summary>
        public TextLocationKeyStringReferenceCollection StringReferences
        {
            get { return stringReferences; }
            set { stringReferences = value; }
        }

        /// <summary>This protected member stores the path of the TLK file</summary>
        public String TlkPath
        {
            get { return tlkFilePath; }
            set { tlkFilePath = value; }
        }
        #endregion

        #region Constructor(s)
        /// <summary>Default constructor</summary>
        public TextLocationKey()
        {
            stringReferences = new TextLocationKeyStringReferenceCollection();
            storeStringsInMemory = true;
        }

        /// <summary>Constructor setting storeStringsInMemory</summary>
        /// <param name="StoreInMemory">Boolean indicating whether or not to store strings in memory when TLK file is read</param>
        public TextLocationKey(Boolean StoreInMemory)
        {
            stringReferences = new TextLocationKeyStringReferenceCollection();
            storeStringsInMemory = StoreInMemory;
        }
        #endregion

        #region Public Methods
        /// <summary>Reads the TLK file using a specified file path</summary>
        /// <param name="FilePath">String describing the path to the file.</param>
        public void ReadFile()
        {
            //use a "using" block to dispose of the stream
            using (Stream fileStream = ReusableIO.OpenFile(tlkFilePath))
            {
                header.ReadHeader(fileStream);      //read the header

                //seek if able
                ReusableIO.SeekIfAble(fileStream, 0x12, SeekOrigin.Begin);  //18 bytes?

                //read the strref blocks
                for (Int32 i = 0; i < header.StringReferenceCount; ++i)
                {
                    TextLocationKeyStringReference strref = new TextLocationKeyStringReference();
                    strref.ReadStringReferenceEntry(fileStream, header.CultureReference);
                    stringReferences.Add(strref);
                }

                //read the strref strings
                if (this.storeStringsInMemory)
                {
                    for (Int32 i = 0; i < stringReferences.Length; ++i)
                    {
                        ReusableIO.SeekIfAble(fileStream, stringReferences[i].StringOffset, SeekOrigin.Begin);
                        TextLocationKeyStringReference strref = stringReferences[i];                                        //copy
                        strref.ReadStringReferenced(fileStream, header.StringsReferenceOffset, header.CultureReference);    //read
                        stringReferences[i] = strref;                                                                       //assign
                    }
                }
            }
        }
        /// <summary>Reads the TLK file, assigning the tlk file path to the specified file path</summary>
        /// <param name="FilePath">String describing the path to the TLK file.</param>
        public void ReadFile(String FilePath)
        {
            tlkFilePath = FilePath;
            ReadFile();
        }

        /// <summary>This public method gets the String referenced at the index described</summary>
        /// <param name="Index">Int32 indexer</param>
        /// <returns>String contained at Index, can be null</returns>
        public String StringReferenceGetAt(Int32 Index)
        {
            String retVal = null;

            if (Index < stringReferences.Length)
            {
                retVal = stringReferences[Index].StringReferenced;
                if (retVal == null)
                {
                    ReadStringReferenceFromTLK(Index);
                    retVal = stringReferences[Index].StringReferenced;      //re-read the string
                }
            }

            return retVal;
        }

        /// <summary>This public method sets the String referenced at the index described</summary>
        /// <param name="Index">Int32 indexer</param>
        /// <param name="StrRef">String to be contained at Index</param>
        /// <returns>Boolean indicating the success of the assignment</returns>
        public Boolean StringReferenceSetAt(Int32 Index, String StrRef)
        {
            Boolean retVal = false;

            if (Index < stringReferences.Length)
            {
                TextLocationKeyStringReference strref = stringReferences[Index];
                strref.StringReferenced = StrRef;
                stringReferences[Index] = strref;
                retVal = true;
            }

            return retVal;
        }

        /// <summary>This public method adds a String Reference to the collection</summary>
        /// <param name="StrRef">String to add</param>
        public void StringReferenceAdd(String StrRef)
        {
            TextLocationKeyStringReference strref = new TextLocationKeyStringReference();
            strref.StringReferenced = StrRef;
            StringReferences.Add(strref);
        }

        /// <summary>This method will write the entire friggin' TLK file to a String builder and return it</summary>
        /// <returns>A string containing textual representations of internal variables, then the header, then all String ref objects</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("store Strings in memory:    ");
            builder.Append(this.storeStringsInMemory);
            builder.Append("\nPath to TLK file:           ");
            builder.Append(this.tlkFilePath);
            builder.Append("\nTLK Header:\n\n");
            builder.Append(this.header.ToString());
            builder.Append("\n\nString References:\n\n");
            builder.Append(this.stringReferences.ToString());

            return builder.ToString();
        }

        /// <summary>This method will write the entire friggin' TLK file to a String builder and return it</summary>
        /// <param name="LongDefinition">Boolean indicating whether to write the string references to the string</param>
        /// <returns>A string containing textual representations of internal variables, then the header, then all String ref objects</returns>
        public String ToString(Boolean LongDefinition)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Store Strings in memory:    ");
            builder.Append(this.storeStringsInMemory);
            builder.Append("\nPath to TLK file:           ");
            builder.Append(this.tlkFilePath);
            builder.Append("\nTLK Header:\n\n");
            builder.Append(this.header.ToString());

            if (LongDefinition)
            {
                builder.Append("\n\nString References:\n\n");
                builder.Append(this.stringReferences.ToString());
            }

            return builder.ToString();
        }
        #endregion

        #region Protected Methods
        /// <summary>This protected method will read a specific string reference from the TLK file specified in this object</summary>
        /// <param name="StringReferenceIndex">Integer representing the stringreference index that needs to be read.</param>
        protected void ReadStringReferenceFromTLK(Int32 StringReferenceIndex)
        {
            if (StringReferenceIndex < stringReferences.Length)
            {
                //use a "using" block to dispose of the stream
                using (Stream fileStream = ReusableIO.OpenFile(tlkFilePath))
                {
                    TextLocationKeyStringReference strref = stringReferences[StringReferenceIndex];
                    strref.ReadStringReferenced(fileStream, header.StringsReferenceOffset, header.CultureReference);    //read
                }
            }
        }
        #endregion
    }
}