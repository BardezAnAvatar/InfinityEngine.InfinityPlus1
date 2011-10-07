using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.TextLocationKey
{

    public class TextLocationKey
    {
        #region Protected Members
        /// <summary>This protected member contains the header information of the TLK file</summary>
        protected TextLocationKeyHeader header;

        /// <summary>An array of stringReference objects</summary>
        protected List<TextLocationKeyStringReference> stringReferences;

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

        /// <summary>This public Property returns the string reference at the index indicated.</summary>
        /// <param name="Index">Int32 representing the place in the array at which to get or set the string reference</param>
        /// <returns>A TextLocationKeyStringReference string reference object</returns>
        public TextLocationKeyStringReference this[Int32 Index]
        {
            get
            {
                TextLocationKeyStringReference strref = stringReferences[Index];
                if (strref.StringReferenced == null)
                {
                    ReadStringReferenceFromTLK(Index);
                    strref = stringReferences[Index]; // re-read
                }
                return strref;
            }
            set { stringReferences[Index] = value; }
        }

        /// <summary>This protected member stores the path of the TLK file</summary>
        public String TlkPath
        {
            get { return tlkFilePath; }
            set { tlkFilePath = value; }
        }

        /// <summary>This public property gets the length of the array of string references</summary>
        public Int32 Length
        {
            get { return stringReferences.Count; }
        }

        /// <summary>
        ///     This public property gets and sets the Boolean value indicating whether or not to store strings in memory when the TLK file is read.
        ///     This is used mostly to retain the ability to use low levels of RAM.
        /// </summary>
        public Boolean StoreStringsInMemory
        {
            get { return storeStringsInMemory; }
            set { storeStringsInMemory = value; }
        }
        #endregion

        #region Constructor(s)
        /// <summary>Default constructor</summary>
        public TextLocationKey()
        {
            stringReferences = new List<TextLocationKeyStringReference>();
            storeStringsInMemory = true;
        }

        /// <summary>Constructor setting storeStringsInMemory</summary>
        /// <param name="StoreInMemory">Boolean indicating whether or not to store strings in memory when TLK file is read</param>
        public TextLocationKey(Boolean StoreInMemory)
        {
            stringReferences = new List<TextLocationKeyStringReference>();
            storeStringsInMemory = StoreInMemory;
        }

        /// <summary>Constructor setting storeStringsInMemory</summary>
        /// <param name="StoreInMemory">Boolean indicating whether or not to store strings in memory when TLK file is read</param>
        /// <param name="FilePath">String representing the path to a TLK file</param>
        public TextLocationKey(Boolean StoreInMemory, String FilePath)
        {
            stringReferences = new List<TextLocationKeyStringReference>();
            storeStringsInMemory = StoreInMemory;
            tlkFilePath = FilePath;
        }
        #endregion

        #region Public Methods
        /// <summary>Reads the TLK file using a specified file path</summary>
        /// <param name="FilePath">String describing the path to the TLK file.</param>
        public void ReadFile()
        {
            //use a "using" block to dispose of the stream
            using (Stream fileStream = ReusableIO.OpenFile(tlkFilePath))
            {
                header.ReadHeader(fileStream);      //read the header

                //seek if able
                //per the IESDP: This section is hardcoded to start at byte 18 from the beginning of the file. The string offsets are relative to the strings section.
                ReusableIO.SeekIfAble(fileStream, 0x12, SeekOrigin.Begin);

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
                    for (Int32 i = 0; i < stringReferences.Count; ++i)
                    {
                        TextLocationKeyStringReference strref = stringReferences[i];            //copy
                        strref.ReadStringReferenced(fileStream, header.StringsReferenceOffset, header.CultureReference);        //read
                        stringReferences[i] = strref;                                                                           //assign
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

        /// <summary>Writes the TLK file, assigning the tlk file path to the specified file path</summary>
        /// <param name="FilePath">String describing the path to the TLK file to be written</param>
        public void WriteFile(String FilePath)
        {
            tlkFilePath = FilePath;
            WriteFile();
        }

        public void WriteFile()
        {
            //clone the file
            TextLocationKey clone = this.Clone();
            MemoryStream buffer = new MemoryStream();

            this.header.StringReferenceCount = this.stringReferences.Count;
            this.header.StringsReferenceOffset = 18 + (26 * stringReferences.Count);
            
            //update the string
            for (Int32 i = 0; i < stringReferences.Count; ++i)
            {
                Int32 bufferStartPos = Convert.ToInt32(buffer.Position);                    //current position
                TextLocationKeyStringReference strref = this.stringReferences[i];     //copy the object
                strref.StringOffset = bufferStartPos;
                ReusableIO.WriteStringToStream(strref.StringReferenced, buffer, header.CultureReference);
                strref.StringLength = Convert.ToInt32(buffer.Position) - bufferStartPos;    //new length
                this[i] = strref;
            }

            using(FileStream fileStream = ReusableIO.OpenFile(tlkFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                //write header to file
                header.Write(fileStream);

                //write string reference entries
                for (Int32 i = 0; i < this.stringReferences.Count; ++i)
                    this.stringReferences[i].Write(fileStream);

                //seek to write
                ReusableIO.SeekIfAble(fileStream, header.StringsReferenceOffset, SeekOrigin.Begin);

                //write string data
                Byte[] bufferBytes = buffer.ToArray();
                fileStream.Write(bufferBytes, 0, bufferBytes.Length);
            }
        }

        /// <summary>Adds a String Regerence to the TLK file</summary>
        /// <param name="StringReference">String reference to add to the TLK file</param>
        /// <returns>an Int32 representing the newly added index</returns>
        public Int32 Add(TextLocationKeyStringReference StringReference)
        {
            stringReferences.Add(StringReference);
            return stringReferences.Count - 1;
        }

        /// <summary>This public method copies all relevant data and returns it in an identical, separate instance of a TextLocationKey object</summary>
        /// <returns>A TextLocationKey clone object</returns>
        public TextLocationKey Clone()
        {
            TextLocationKey retval = new TextLocationKey();
            retval.header = header;
            retval.storeStringsInMemory = storeStringsInMemory;
            retval.tlkFilePath = tlkFilePath;
            retval.stringReferences = new List<TextLocationKeyStringReference>(this.stringReferences);

            return retval;
        }
        #endregion

        #region Overridden Methods
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

            //String references
            for (Int32 index = 0; index < stringReferences.Count; ++index)
            {
                builder.Append("String Reference Index:    ");
                builder.Append(index);
                builder.Append("\n\t");
                builder.Append(stringReferences[index].ToString());
            }

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
                builder.Append("\n\nString References:");

                //String references
                for (Int32 index = 0; index < stringReferences.Count; ++index)
                {
                    builder.Append("\n\n\tString Reference Index:    ");
                    builder.Append(index);
                    builder.Append("\n\t");
                    builder.Append(stringReferences[index].ToString());
                }
            }

            return builder.ToString();
        }
        #endregion

        #region Protected Methods
        /// <summary>This protected method will read a specific string reference from the TLK file specified in this object</summary>
        /// <param name="StringReferenceIndex">Integer representing the stringreference index that needs to be read.</param>
        protected void ReadStringReferenceFromTLK(Int32 StringReferenceIndex)
        {
            if (StringReferenceIndex < stringReferences.Count)
            {
                //use a "using" block to dispose of the stream
                using (Stream fileStream = ReusableIO.OpenFile(tlkFilePath))
                {
                    TextLocationKeyStringReference strref = stringReferences[StringReferenceIndex];
                    strref.ReadStringReferenced(fileStream, header.StringsReferenceOffset, header.CultureReference);    //read
                    stringReferences[StringReferenceIndex] = strref;        //re-assign
                }
            }
        }
        #endregion
    }
}