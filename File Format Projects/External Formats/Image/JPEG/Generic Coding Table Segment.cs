using System;
using System.Collections.Generic;
using System.IO;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG
{
    /// <summary>Represents a generic marker segment which defines one or more tables</summary>
    /// <remarks>See pages 39-42, JPEG spec, §B.2.4.1 - B.2.4.3</remarks>
    public class GenericCodingTableSegment<TableType> : GenericTableSegment<TableType> where TableType : GenericCodingTable, new()
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public GenericCodingTableSegment() : base() { }

        /// <summary>Definition constructor</summary>
        /// <param name="marker">Marker of this substream</param>
        /// <param name="length">Length of this substream</param>
        public GenericCodingTableSegment(UInt16 marker, UInt16 length) : base(marker, length) { }
        #endregion

        #region IO Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="tables">The decoder's collection of quantization tables</param>
        public virtual void Read(Stream input, GenericCodingTable[] dcTables, GenericCodingTable[] acTables)
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
                if (table.TableClass == 0)
                    dcTables[table.TableDestinationIndex] = table;
                else if (table.TableClass == 1)
                    acTables[table.TableDestinationIndex] = table;

                //recalculate measurements
                length -= input.Position - position;
                position = input.Position;
            }
        }
        #endregion
    }
}