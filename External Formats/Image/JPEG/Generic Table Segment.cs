using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG
{
    /// <summary>Represents a generic marker segment which defines one or more tables</summary>
    /// <remarks>See pages 39-42, JPEG spec, §B.2.4.1 - B.2.4.3</remarks>
    public class GenericTableSegment<TableType> : MarkerSegment where TableType : GenericTable, new()
    {
        #region Fields
        /// <summary>List of tables</summary>
        public List<TableType> Tables { get; set; }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public GenericTableSegment() : base()
        {
            this.Initialize();
        }

        /// <summary>Definition constructor</summary>
        /// <param name="marker">Marker of this substream</param>
        /// <param name="length">Length of this substream</param>
        public GenericTableSegment(UInt16 marker, UInt16 length) : this()
        {
            this.Marker = marker;
            this.Length = length;
        }

        /// <summary>Initializes Lists</summary>
        protected virtual void Initialize()
        {
            this.Tables = new List<TableType>();
        }
        #endregion

        #region IO Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="tables">The decoder's collection of quantization tables</param>
        public virtual void Read(Stream input, TableType[] tables)
        {
            //measure the length
            Int64 length = this.Length - 2;
            Int64 position = input.Position;

            //while there should logically exist another table
            while (length > 0)
            {
                TableType table = new TableType();
                table.Read(input);

                //add the table
                this.Tables.Add(table);

                //assign the table
                tables[table.TableDestinationIndex] = table;

                //recalculate measurements
                length -= input.Position - position;
                position = input.Position;
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            throw new NotImplementedException("Not yet implemented.");
        }
        #endregion
    }
}