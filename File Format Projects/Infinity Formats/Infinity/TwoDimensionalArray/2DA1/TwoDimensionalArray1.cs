using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TwoDimensionalArray._2DA1
{
    /// <summary>An exposure for BioWare 2DA files found in Infinity Engine</summary>
    public class TwoDimensionalArray1 : InfinityFormat
    {
        #region Constants
        /// <summary>
        ///     This constant should be obscure. It is intended to be a friendly
        ///     visible name for something very unlikely to ever be used.
        /// </summary>
        protected static readonly String rowKey = "~!@#$%^&*(@*$^&)(_++#$%^";

        /// <summary>Padding between cells</summary>
        /// <remarks>Length is arbitrary but must be at least 1</remarks>
        protected static readonly Int32 columnPadding = 2;
        #endregion


        #region Members
        /// <summary>This is the default value for the 2DA collection</summary>
        protected String valueDefault;

        /// <summary>This is the collection of row names</summary>
        protected List<String> keysRow;

        /// <summary>This is the collection of column names</summary>
        protected List<String> keysColumn;

        /// <summary>This is the data table containing the data read from a 2DA file</summary>
        protected DataTable values;
        #endregion


        #region Properties(s)
        /// <summary>Retrieves all the data for a given row and returns it as a Dictionary object</summary>
        /// <param name="row">Name of the row to return the values of</param>
        /// <returns>A Dictionary object with a Key of String and a value of String if found, null if not.</returns>
        public Dictionary<String, String> this[String row]
        {
            get { return this.RowToDictionary(row); }
        }
        #endregion


        #region Constructor(s)
        /// <summary>Default constructor</summary>
        public TwoDimensionalArray1()
        {
            this.keysRow = null;
            this.keysColumn = null;
            this.values = null;
        }
        
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.ClearKeys();
            this.values = new DataTable();
        }

        #endregion


        /// <summary>Instantiates the data table and populates its columns</summary>
        /// <param name="columns">IEnumerable collection of Strings (Array, List, etc.) that contains all column names</param>
        protected void GenerateDataTable(IEnumerable<String> columns)
        {
            this.values = new DataTable();
            this.values.Columns.Add(rowKey, typeof(String));

            foreach (String name in columns)
                this.values.Columns.Add(name, typeof(String));
        }

        /// <summary>This public method reads file format from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void Read(Stream input)
        {
            //decrypt or pass through the 2DA file
            MemoryStream binData = InfinityXorEncryption.Decrypt(input);

            //read signature
            Byte[] buffer = ReusableIO.BinaryRead(binData, 8);   //header buffer

            this.signature = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish, 4);
            this.version = ReusableIO.ReadStringFromByteArray(buffer, 4, CultureConstants.CultureCodeEnglish, 4);

            this.ReadBody(binData);
        }

        /// <summary>This public method reads file format from the input stream, after the header has already been read.</summary>
        public override void ReadBody(Stream input)
        {
            this.ClearKeys();

            using (StreamReader strmReader = new StreamReader(input))  //should dispose the MemoryStream
            {
                //discard newline after signature & version
                strmReader.ReadLine();

                //default value
                using (StringReader strReader = new StringReader(strmReader.ReadLine()))
                    this.valueDefault = ReadWord(strReader);

                //read columns
                using (StringReader strReader = new StringReader(strmReader.ReadLine()))
                {
                    while (strReader.Peek() > -1)
                        this.keysColumn.Add(ReadWord(strReader));
                }

                //with column names, instantiate the DataTable
                this.GenerateDataTable(this.keysColumn);

                //read rows
                while (!strmReader.EndOfStream)
                {
                    using (StringReader strReader = new StringReader(strmReader.ReadLine()))
                    {
                        //Add row name
                        String key = ReadWord(strReader);

                        //skip empty lines
                        if (!String.IsNullOrWhiteSpace(key))    //null, String.Empty, or full of whitespace
                        {
                            this.keysRow.Add(key);

                            //set up a row
                            String[] row = new String[this.keysColumn.Count + 1];
                            row[0] = key;

                            //default values
                            for (Int32 i = 1; i < row.Length; ++i)
                                row[i] = null;

                            //add values
                            Int32 columnIndex = 1;
                            while (strReader.Peek() > -1)
                            {
                                row[columnIndex] = ReadWord(strReader);
                                ++columnIndex;
                            }

                            this.values.Rows.Add(row);
                        }
                    }
                }
            }
        }
                
        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public override void Write(Stream output)
        {
            using (StreamWriter writer = new StreamWriter(output))
                this.Write(writer);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="writer">Text writer to write to</param>
        public void Write(TextWriter writer)
        {
            //write signature
            writer.Write(this.signature);

            //write version
            writer.WriteLine(this.version);

            //write default value
            writer.WriteLine(this.valueDefault);
            
            //get widths
            Int32[] widths = this.GetColumnWidths();

            //write column names
            writer.Write(" ".PadRight(widths[0]));
            for (Int32 i = 0; i < this.keysColumn.Count; ++i)
                writer.Write(this.keysColumn[i].PadRight(widths[i + 1]));
            
            writer.WriteLine(String.Empty);

            //write rows
            foreach (DataRow row in this.values.Rows)
            {
                //all bu the last
                for (Int32 i = 0; i < widths.Length - 1; ++i)
                    writer.Write((row[i] as String).PadRight(widths[i]));

                //writeline for the last one
                writer.WriteLine((row[widths.Length - 1] as String).PadRight(widths[widths.Length - 1] ));
            }
        }

        /// <summary>
        ///     This method discards leading whitespace, a "word" (any group of non-whitespace characters)
        ///     and then discards trailing whitespace before the end of line or next "word."
        ///     This method should be preceeded with a condition checking for Reader.Peek()) > -1 before
        ///     calling to ensure it is not wasted CPU time.
        /// </summary>
        /// <param name="Reader">StringReader to read from.</param>
        /// <returns>String.Empty if only whitespace is encountered.</returns>
        /// <remarks>This method was originally written for my Sword of the Stars savegame editor.</remarks>
        protected static String ReadWord(StringReader Reader)
        {
            Int32 temp;
            Char current;
            String word = String.Empty;

            //consume leading whitespace
            while ((temp = Reader.Peek()) > -1 && Char.IsWhiteSpace(current = Convert.ToChar(temp)))
            {
                Reader.Read();
            }

            //consume leading whitespace
            while ((temp = Reader.Peek()) > -1 && !Char.IsWhiteSpace(current = Convert.ToChar(temp)))
            {
                word += current;
                Reader.Read();
            }

            //consume trailing whitespace
            while ((temp = Reader.Peek()) > -1 && Char.IsWhiteSpace(current = Convert.ToChar(temp)))
            {
                Reader.Read();
            }

            return word;
        }

        /// <summary>Generates a string (text file) representing the 2DA file</summary>
        /// <returns>A String containing the contents of the 2DA file when written to file, plus a "2DA" preceeding line.</returns>
        public override String ToString()
        {
            String output = null;
            StringBuilder builder = new StringBuilder();

            using (StringWriter writer = new StringWriter(builder))
            {
                builder.AppendLine("2DA:");
                this.Write(writer);
                output = builder.ToString();
            }

            return output;
        }

        /// <summary>Re-instantiates the key collections</summary>
        protected void ClearKeys()
        {
            this.keysColumn = new List<String>();
            this.keysRow = new List<String>();
        }

        /// <summary>Generates the widths (1 per character) needed to display/write a 2DA file.</summary>
        /// <returns>An Int32 array of values, one for each width of each column</returns>
        protected Int32[] GetColumnWidths()
        {
            Int32[] widths = new Int32[this.keysColumn.Count + 1];

            //get column name widths... the "row" column has no name header, so shift by one when storing
            for (Int32 i = 0; i < this.keysColumn.Count; ++i)
            {
                if (widths[i + 1] < this.keysColumn[i].Length)
                    widths[i + 1] = this.keysColumn[i].Length;
            }

            //get column data widths
            foreach (DataRow row in this.values.Rows)
            {
                for (Int32 i = 0; i < this.values.Columns.Count; ++i)
                {
                    if (widths[i] < (row[i] as String).Length)
                        widths[i] = (row[i] as String).Length;
                }
            }

            //add padding
            for (Int32 i = 0; i < widths.Length; ++i)
                widths[i] += columnPadding;

            return widths;
        }

        /// <summary>Retrieves all the data for a given row and returns it as a Dictionary object</summary>
        /// <param name="rowName">Name of the row to return the values of</param>
        /// <returns>A Dictionary object with a Key of String and a value of String if found, null if not.</returns>
        protected Dictionary<String, String> RowToDictionary(String rowName)
        {
            Dictionary<String, String> rowDict = null;

            //check that the key exists
            if (this.keysRow.Contains(rowName))
            {
                rowDict = new Dictionary<String, String>();
                
                Int32 index = this.keysRow.IndexOf(rowName);

                foreach (String colName in this.keysColumn)
                    rowDict.Add(colName, this.values.Rows[index][colName] as String ?? this.valueDefault);  //coalesce to the default
            }

            return rowDict;
        }
    }
}