using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.ACM
{
    /// <summary>Represents an unpacked BitBlock</summary>
    /// <remarks>
    ///     I theorize that since channels are either bogus or meant as output, that the input is
    ///     therefore divided by the number of (playback) samples specified to determine actual stored samples.
    ///     Seems to be accessed as PaddedBlock[row][column] in BerliOS
    /// </remarks>
    public class PackedBlock
    {
        /// <summary>Number of logical columns in the Packed Block</summary>
        public UInt32 Columns { get; set; }

        /// <summary>Number of logical rows in the Packed Block</summary>
        public UInt32 Rows { get; set; }

        /// <summary>The shifting/compression levels of the ACM file</summary>
        public Int32 Levels { get; set; }

        /// <summary>Unpacked sample data</summary>
        /// <remarks>Most definately 32-bit storage; the values here are un-packed by shifting to the right by x bits, where x is the column shifter</remarks>
        public Int32[] Data { get; set; }

        /// <summary>Indexer for the </summary>
        /// <param name="column">Packed Block column</param>
        /// <param name="row">Packed Block row</param>
        /// <returns>The shifted sample data</returns>
        public Int32 this[Int32 column, Int32 row]
        {
            get { return this.Data[this.DataIndex(column, row)]; }
            set { this.Data[this.DataIndex(column, row)] = value; }
        }

        /// <summary>Definition constructor</summary>
        /// <param name="columns">Columns to create</param>
        /// <param name="rows">Rows to create</param>
        /// <param name="levels">The shifting/compression levels of the ACM file</param>
        public PackedBlock(UInt32 columns, UInt32 rows, Int32 levels)
        {
            this.Columns = columns;
            this.Rows = rows;
            this.Data = new Int32[columns * rows];
            this.Levels = levels;
        }

        /// <summary>Gets the data index of the packed data array</summary>
        /// <param name="column">Packed Block column</param>
        /// <param name="row">Packed Block row</param>
        /// <returns>The index of the data</returns>
        /// <remarks>
        ///     I originally thought this was a row*columns + current column simple index, but I was wrong.
        ///     Row needs to be shifted by levels instead
        /// </remarks>
        protected Int64 DataIndex(Int32 column, Int32 row)
        {
            return (row * this.Columns) + column;
            //return (row << this.Levels) + column;
        }
    }
}