using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WorldMap.Component;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WorldMap
{
    /// <summary>A World Map, version 1</summary>
    public class WorldMap1 : IInfinityFormat
    {
        #region Fields
        /// <summary>The world map header</summary>
        public WorldMapHeader Header { get; set; }

        /// <summary>Collection of maps in the world map</summary>
        public List<MapEntry> Maps { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Header = new WorldMapHeader();
            this.Maps = new List<MapEntry>();
        }
        #endregion


        #region I/O Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            if (fullRead)
                this.Read(input);
            else
            {
                this.Header = new WorldMapHeader();
                this.Header.Read(input, false);
            }
        }

        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //header
            this.Header.Read(input);

            //maps
            for (Int32 index = 0; index < this.Header.MapCount; ++index)
            {
                //maps are stored in sequential order right after the header; if there are multiple maps, we will need to seek back to the current position after reading this map's data.
                ReusableIO.SeekIfAble(input, this.Header.MapOffset + (MapEntry.StructSize * index));

                MapEntry map = new MapEntry();
                map.Read(input);
                this.Maps.Add(map);
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public virtual void Write(Stream output)
        {
            //data integrity
            this.MaintainMinimalDataIntegrity();

            //header
            this.Header.Write(output);

            //maps
            for (Int32 index = 0; index < this.Maps.Count; ++index)
            {
                //maps are stored in sequential order right after the header; if there are multiple maps, we will need to seek back to the current position after reading this map's data.
                ReusableIO.SeekIfAble(output, this.Header.MapOffset + (MapEntry.StructSize * index));

                this.Maps[index].Write(output);
            }
        }
        #endregion


        #region Data integrity
        /// <summary>Maintains mimimal data integrity by not lying to the output data file.</summary>
        protected void MaintainMinimalDataIntegrity()
        {
            List<Tuple<Int64, Int64>> offsets = this.CollectOffsets();
            if (this.OffsetsOverlap(offsets))
                this.ResetOffsets();
        }

        /// <summary>Collects a set of offsets and their lengths to be examined</summary>
        /// <returns>A List of Tuples (Offset and length)</returns>
        protected List<Tuple<Int64, Int64>> CollectOffsets()
        {
            List<Tuple<Int64, Int64>> offsetCollection = new List<Tuple<Int64, Int64>>();

            //header
            offsetCollection.Add(new Tuple<Int64, Int64>(0L, WorldMapHeader.StructSize));

            //maps
            offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.MapOffset, (this.Header.MapCount * MapEntry.StructSize)));

            //map items
            foreach (MapEntry map in this.Maps)
            {
                //areas
                offsetCollection.Add(new Tuple<Int64, Int64>(map.AreaOffset, (map.AreaCount * AreaEntry.StructSize)));

                //Area links
                offsetCollection.Add(new Tuple<Int64, Int64>(map.AreaLinkOffset, (map.AreaLinkCount * AreaLink.StructSize)));
            }

            return offsetCollection;
        }

        /// <summary>Traverses a collection of Tuples (offset and length) and determines if any overlap</summary>
        /// <param name="offsets">Collection of offset tuples</param>
        /// <returns>True if any overlap; false if not</returns>
        protected Boolean OffsetsOverlap(List<Tuple<Int64, Int64>> offsets)
        {
            Boolean overlaps = false;

            for (Int32 i = 0; i < offsets.Count; ++i)
                for (Int32 j = 0; j < offsets.Count; ++j)
                {
                    if (i == j)
                        continue;

                    if (IntExtension.Between(offsets[i].Item1, offsets[i].Item2, offsets[j].Item1, offsets[j].Item1 + offsets[j].Item2))
                    {
                        overlaps = true;
                        goto ExitPoint; //short-circuit
                    }
                }

        //short-circuit destination
        ExitPoint:

            return overlaps;
        }

        /// <summary>Resets the offsets within the file stream</summary>
        protected virtual void ResetOffsets()
        {
            Int64 offset = 0;

            //skip header
            offset += WorldMapHeader.StructSize;

            //maps
            this.Header.MapOffset = Convert.ToUInt32(offset);
            offset += (this.Header.MapCount * MapEntry.StructSize);

            //map items
            foreach (MapEntry map in this.Maps)
            {
                //areas
                map.AreaOffset = Convert.ToUInt32(offset);
                offset += (map.AreaCount * AreaLink.StructSize);

                //area links
                map.AreaLinkOffset = Convert.ToUInt32(offset);
                offset += (map.AreaLinkCount * AreaEntry.StructSize);
            }
        }
        #endregion


        #region ToString methods
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("World Map version 1.0");
            builder.AppendLine(this.Header.ToString());
            builder.AppendLine(this.GenerateMapString(builder));

            return builder.ToString();
        }

        /// <summary>Generates a human-readable console output describing the maps</summary>
        /// <param name="sb">StringBuilder to write to</param>
        /// <returns>A multi-line String</returns>
        protected String GenerateMapString(StringBuilder sb)
        {
            for (Int32 i = 0; i < this.Maps.Count; ++i)
                sb.Append(this.Maps[i].ToString(i + 1));

            return sb.ToString();
        }
        #endregion
    }
}