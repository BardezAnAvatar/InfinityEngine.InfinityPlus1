using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TalkTable
{
    /// <summary>Class representing the contents of a TLK file</summary>
    public class TalkTable : IInfinityFormat
    {
        #region Protected Members
        /// <summary>An array of stringReference objects</summary>
        protected List<TextLocationKeyStringReference> stringReferences;
        #endregion


        #region Public Properties
        /// <summary>This contains the header information of the TLK file</summary>
        public TextLocationKeyHeader Header { get; set; }

        /// <summary>This public Property returns the string reference at the index indicated.</summary>
        /// <param name="index">Int32 representing the place in the array at which to get or set the string reference</param>
        /// <returns>A TextLocationKeyStringReference string reference object</returns>
        public TextLocationKeyStringReference this[Int32 index]
        {
            get { return stringReferences[index]; }
            set { stringReferences[index] = value; }
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
        public Boolean StoreStringsInMemory { get; set; }
        #endregion


        #region Constructor(s)
        /// <summary>Default constructor</summary>
        public TalkTable() : this(true) { }

        /// <summary>Constructor setting storeStringsInMemory</summary>
        /// <param name="storeInMemory">Boolean indicating whether or not to store strings in memory when TLK file is read</param>
        public TalkTable(Boolean storeInMemory)
        {
            this.stringReferences = null;
            this.StoreStringsInMemory = storeInMemory;
        }

        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.stringReferences = new List<TextLocationKeyStringReference>();
            this.Header = new TextLocationKeyHeader();
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the output stream. Reads the whole structure.</summary>
        /// <param name="input">Stream to read from</param>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            if (fullRead)
                this.Read(input);
            else
            {
                this.Header = new TextLocationKeyHeader();
                this.Header.Read(input, false);
            }
        }

        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            this.Header.Read(input);      //read the header

            //seek if able
            //per the IESDP: This section is hardcoded to start at byte 18 from the beginning of the file. The string offsets are relative to the strings section.
            ReusableIO.SeekIfAble(input, 0x12, SeekOrigin.Begin);

            //read the strref blocks
            for (Int32 i = 0; i < this.Header.StringReferenceCount; ++i)
            {
                TextLocationKeyStringReference strref = new TextLocationKeyStringReference(this.Header.CultureReference);
                strref.Initialize();
                strref.ReadStringReferenceEntry(input);
                stringReferences.Add(strref);
            }

            //read the strref strings
            if (this.StoreStringsInMemory)
            {
                for (Int32 i = 0; i < stringReferences.Count; ++i)
                {
                    TextLocationKeyStringReference strref = stringReferences[i];                //copy
                    strref.ReadStringReferenced(input, this.Header.StringsReferenceOffset);     //read
                    stringReferences[i] = strref;                                               //assign
                }
            }
        }

        /// <summary>This public method writes the String reference entry to an output stream</summary>
        /// <param name="output">Stream object w=into which to write to</param>
        public void Write(Stream output)
        {
            /* write the file */
            this.Header.StringReferenceCount = this.stringReferences.Count;
            this.Header.StringsReferenceOffset = TextLocationKeyHeader.StructSize + (TextLocationKeyStringReference.StructSize * stringReferences.Count);
            
            //write header to file
            this.Header.Write(output);

            //write string reference entries
            for (Int32 i = 0; i < this.stringReferences.Count; ++i)
                this.stringReferences[i].Write(output);

            //seek to write the strings
            ReusableIO.SeekIfAble(output, this.Header.StringsReferenceOffset, SeekOrigin.Begin);

            //write string data
            for (Int32 i = 0; i < stringReferences.Count; ++i)
            {
                TextLocationKeyStringReference strref = this.stringReferences[i];           //copy the object
                ReusableIO.WriteStringToStream(strref.StringReferenced, output, this.Header.CultureReference, true);
            }
        }
        #endregion


        #region Public Methods
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
        public TalkTable Clone()
        {
            TalkTable retval = new TalkTable();
            retval.Header = this.Header;
            retval.StoreStringsInMemory = this.StoreStringsInMemory;
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
            builder.Append(StringFormat.ToStringAlignment("store Strings in memory"));
            builder.Append(this.StoreStringsInMemory);
            builder.Append(StringFormat.ToStringAlignment("TLK Header"));
            builder.Append(this.Header.ToString());
            builder.Append(StringFormat.ToStringAlignment("String References"));

            //String references
            for (Int32 index = 0; index < stringReferences.Count; ++index)
            {
                builder.Append(StringFormat.ToStringAlignment("String Reference Index"));
                builder.Append(index);
                builder.Append(stringReferences[index].ToString());
            }

            return builder.ToString();
        }

        /// <summary>This method will write the entire friggin' TLK file to a String builder and return it</summary>
        /// <param name="longDefinition">Boolean indicating whether to write the string references to the string</param>
        /// <returns>A string containing textual representations of internal variables, then the header, then all String ref objects</returns>
        public String ToString(Boolean longDefinition)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Store Strings in memory");
            builder.Append(this.StoreStringsInMemory);
            builder.Append(StringFormat.ToStringAlignment("TLK Header"));
            builder.Append(this.Header.ToString());

            if (longDefinition)
            {
                builder.Append(StringFormat.ToStringAlignment("String References"));

                //String references
                for (Int32 index = 0; index < stringReferences.Count; ++index)
                {
                    builder.Append(("String Reference Index"));
                    builder.Append(index);
                    builder.Append(stringReferences[index].ToString());
                }
            }

            return builder.ToString();
        }
        #endregion
    }
}