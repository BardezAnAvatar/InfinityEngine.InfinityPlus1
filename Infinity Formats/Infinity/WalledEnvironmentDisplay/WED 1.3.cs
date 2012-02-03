using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WalledEnvironmentDisplay.Components;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WalledEnvironmentDisplay
{
    /// <summary>Represents a WED 1.3 file</summary>
    /// <remarks>
    ///     The WED format is very counter intuituve and backwards when compared to other formats;
    ///     The tileset indeces and tile maps are stored as one large array, but treated as if they were a
    ///     separate subsets of data for overlays, doors, etc. This is one format where I do not
    ///     repsect Bioware for having come up with it. Mixing offsets and pointers makes for confusing
    ///     data.
    ///     
    ///     Also, due to some weirdness between engines, it seems that the format RELIES less on offsets
    ///     in the seek and read sense than it does in the pointer mathematical sense, read from pointer to
    ///     pointer, rather than offset + count.
    /// </remarks>
    public class Wed1_3 : IInfinityFormat
    {
        #region Fields
        /// <summary>Represents the file header</summary>
        public WedHeader Header { get; set; }

        /// <summary>The collection of overlays</summary>
        public List<Overlay> Overlays { get; set; }

        /// <summary>The polyons and vertices header</summary>
        public PolygonHeader PolygonHeader { get; set; }

        /// <summary>Collection of door ojects</summary>
        public List<Door> Doors { get; set; }

        /// <summary>Collection of wall groups</summary>
        public List<WallGroup> WallGroups { get; set; }

        /// <summary>Collection of wall group polygons</summary>
        public List<Polygon> WallPolygons { get; set; }

        /// <summary>Lookup table for wall polygon indeces</summary>
        public List<Int16> WallPolygonLookupTable { get; set; }

        /// <summary>Lookup tables to vertices for door polygons</summary>
        public List<DoorVertexIndexCollections> DoorVertexLookupTable { get; set; }

        /// <summary>Collection of vertices</summary>
        public List<Vertex> Vertices { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the height in tiles of the base overlay</summary>
        public Int32 TileHeight
        {
            get { return this.Overlays[0].TileHeight; }
        }

        /// <summary>Exposes the height in tiles of the base overlay</summary>
        public Int32 TileWidth
        {
            get { return this.Overlays[0].TileWidth; }
        }

        /// <summary>Exposes the number of Wall Group polygon groups per 'row'</summary>
        public Int32 WallGroupWidth
        {
            get
            {
                //width = 10 tiles
                Int32 temp = this.TileWidth;
                Int32 width = (temp / 10) + (temp % 10 == 0 ? 0 : 1);

                return width;
            }
        }

        /// <summary>Exposes the number of Wall Group polygon groups per 'row'</summary>
        public Int32 WallGroupHeight
        {
            get
            {
                //height = 7.5 tiles
                Int32 temp = this.TileHeight * 10; //shift up 10 to avoid the divisor
                Int32 height = (temp / 75) + (temp % 75 == 0 ? 0 : 1);

                return height;
            }
        }

        /// <summary>Exposes the overall count of wall groups in this WED</summary>
        public Int64 StreamWallGroupCount
        {
            get
            {
                Int64 limitingOffset = /*(this.Doors.Count > 0) ? Convert.ToInt64(this.Doors[0].OffsetPolygonsClosed) :*/ this.PolygonHeader.OffsetPolygons;
                Int64 offsetDiffSize = (limitingOffset - this.PolygonHeader.OffsetWalls) / WallGroup.StructSize;
                //Int64 actualWallGroupCount = WallGroupHeight * WallGroupWidth;
                return offsetDiffSize;
            }
        }

        /// <summary>Exposes the accurate count of wall groups in this WED</summary>
        public Int64 ActualWallGroupCount
        {
            get { return WallGroupHeight * WallGroupWidth; }
        }

        /// <summary>Exposes the overall count of verteces in this WED</summary>
        /// <remarks>Does so through looping</remarks>
        public Int32 VertexCount
        {
            get
            {
                Int32 count = 0;
                foreach (Polygon p in this.WallPolygons)
                    count += p.VertexCount;

                foreach (Door door in this.Doors)
                {
                    foreach (Polygon p in door.Polygons.Open)
                        count += p.VertexCount;

                    foreach (Polygon p in door.Polygons.Closed)
                        count += p.VertexCount;
                }

                return count;
            }
        }

        /// <summary>Exposes the overall count of verteces in this WED</summary>
        /// <remarks>Does so through looping</remarks>
        public Int32 DoorTilemapIndecesCount
        {
            get
            {
                Int32 count = 0;
                foreach (Door door in this.Doors)
                    count += door.TileCount;

                return count;
            }
        }

        /// <summary>Exposes the overall count of verteces in this WED</summary>
        /// <remarks>Does so through looping</remarks>
        public Int32 DoorPolygonCount
        {
            get
            {
                Int32 count = 0;

                foreach (Door door in this.Doors)
                {
                    foreach (Polygon p in door.Polygons.Open)
                        count += p.VertexCount;

                    foreach (Polygon p in door.Polygons.Closed)
                        count += p.VertexCount;
                }

                return count;
            }
        }

        /// <summary>Exposes the overall count of verteces in this WED</summary>
        /// <remarks>Does so through looping</remarks>
        public Int32 WallGroupIndexCount
        {
            get
            {
                Int32 count = 0;

                foreach (WallGroup wg in this.WallGroups)
                    count += wg.PolygonCount;

                return count;
            }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.Header = new WedHeader();
            this.PolygonHeader = new PolygonHeader();
            this.Overlays = new List<Overlay>();
            this.Doors = new List<Door>();
            this.WallGroups = new List<WallGroup>();
            this.WallPolygons = new List<Polygon>();
            this.WallPolygonLookupTable = new List<Int16>();
            this.DoorVertexLookupTable = new List<DoorVertexIndexCollections>();
            this.Vertices = new List<Vertex>();
        }
        #endregion


        #region IO method implemetations
        #region Read IO
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
                this.Header = new WedHeader();
                this.Header.Read(input, false);
            }
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();


            /*
            there seems to be remainder isses with Infinity Engine data where one game seems to behave differently than others.
            So, my new idea is to consume all data first, then assign the data structures to the appropriate storage.
            */
            
            this.ReadHeader(input);         //read header
            this.ReadOverlays(input);       //read overlays
            this.ReadPolygonHeader(input);  //read secondary header
            this.ReadDoors(input);          //read doors
            this.ReadTilemaps(input);       //read overlay tilemaps; door tilemap offset - first overlay tilemap offset
            this.ReadDoorTilemapIndeces(input);     //read door tileset indeces; read # door tilemap indeces
            this.ReadOverlayTileSetIndeces(input);  //read overlay tileset indeces; read # overlay tilemap indeces
            this.ReadWallGroups(input);     //read wall groups
            this.ReadWallPolygons(input);   //read wall group polygons
            this.ReadDoorPolygons(input);   //read door polygons

            //read wall group polygon indeces; there are pointers for each door, so no need for door polygon indeces?
            this.ReadWallPolygonLookupTable(input);
            
            this.ReadVertices(input);   //read vertices
        }

        /// <summary>Reads the file header from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadHeader(Stream input)
        {
            ReusableIO.SeekIfAble(input, 0L);   //start of file
            this.Header.Read(input);
        }

        /// <summary>This public method reads WED overlays from the input stream.</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadOverlays(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.Header.OffsetOverlays);

            for (Int32 i = 0; i < this.Header.CountOverlay; ++i)
            {
                Overlay overlay = new Overlay();
                overlay.Read(input);
                this.Overlays.Add(overlay);
            }
        }

        /// <summary>This public method reads WED doors from the input stream.</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadDoors(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.Header.OffsetDoors);

            for (Int32 i = 0; i < this.Header.CountDoor; ++i)
            {
                Door door = new Door();
                door.Read(input);
                this.Doors.Add(door);
            }
        }

        /// <summary>This public method reads the WED polygon header from the input stream.</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadPolygonHeader(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.Header.OffsetPolygonHeader);
            this.PolygonHeader.Read(input);
        }

        /// <summary>This public method reads the WED tilemaps from the input stream.</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadTilemaps(Stream input)
        {
            //read the overlays
            for (Int32 overlayIndex = 0; overlayIndex < (this.Overlays.Count - 1); ++overlayIndex)
            {
                //calculate the differential and count
                Int64 differential = this.Overlays[overlayIndex + 1].OffsetTilemap - this.Overlays[overlayIndex].OffsetTilemap;
                this.ReadTilemapsReadCollection(input, overlayIndex, differential);
            }

            //perform finl case, using door tilemap indeces rather than next overlay's tilemap offset
            {
                Int32 overlayIndex = (this.Overlays.Count - 1);

                //calculate the differential and count
                Int64 differential = this.Header.OffsetDoorTileMapIndeces - this.Overlays[overlayIndex].OffsetTilemap;
                this.ReadTilemapsReadCollection(input, overlayIndex, differential);
            }
        }

        /*
        /// <summary>This public method reads the WED tilemaps from the input stream.</summary>
        /// <param name="input">Stream to read from</param>
        [Obsolete("New approach to this")]
        protected void ReadTilemaps(Stream input)
        {
            foreach (Overlay overlay in this.Overlays)
            {
                //seek to reading point
                ReusableIO.SeekIfAble(input, overlay.OffsetTilemap);

                //instantiate
                List<Tilemap> tilemaps = new List<Tilemap>();
                for (Int32 i = 0; i < overlay.StreamTileCount; ++i)
                {
                    Tilemap tilemap = new Tilemap();
                    tilemap.Read(input);
                    tilemaps.Add(tilemap);
                }

                //this.Tilemaps.Add(tilemaps);
            }
        }
        */

        /// <summary>This public method reads the collection of WED tilemap indeces for doors from the input stream.</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadDoorTilemapIndeces(Stream input)
        {
            //seek
            ReusableIO.SeekIfAble(input, this.Header.OffsetDoorTileMapIndeces);
            
            for (Int32 doorIndex = 0; doorIndex < (this.Doors.Count - 1); ++doorIndex)
            {
                Door door = this.Doors[doorIndex];
                Byte[] data = ReusableIO.BinaryRead(input, door.TileCount * 2);
                for (Int32 datumIndex = 0; datumIndex < data.Length; datumIndex += 2)
                    door.TilemapIndeces.Add(ReusableIO.ReadInt16FromArray(data, datumIndex));
            }

            //final case; read all remaining data
            if (this.Doors.Count > 0)
            {
                Door door = this.Doors[(this.Doors.Count - 1)];
                Int64 differential = this.Overlays[0].OffsetTileIndeces - (this.Header.OffsetDoorTileMapIndeces + (door.TileCellIndex * 2));
                Byte[] data = ReusableIO.BinaryRead(input, differential);
                for (Int32 datumIndex = 0; datumIndex < data.Length; datumIndex += 2)
                    door.TilemapIndeces.Add(ReusableIO.ReadInt16FromArray(data, datumIndex));
            }
        }

        /*
        /// <summary>This method reads the WED tileset indeces from the input stream.</summary>
        /// <param name="input">Stream to read from</param>
        [Obsolete("New approach")]
        protected void ReadTileSetIndeces(Stream input)
        {
            for (Int32 index = 0; index < this.Overlays.Count; ++index)
            {
                Overlay overlay = this.Overlays[index];

                ReusableIO.SeekIfAble(input, overlay.OffsetTileIndeces);

                //instantiate a new collection
                List<Int16> tileIndeces = new List<Int16>();
                Int32 indexCount = overlay.TileIndexCount;

                //one read is faster than individual 2-byte reads
                Byte[] data = ReusableIO.BinaryRead(input, indexCount * 2);

                for (Int32 datumIndex = 0; datumIndex < data.Length; datumIndex += 2)
                    tileIndeces.Add(ReusableIO.ReadInt16FromArray(data, datumIndex));

                this.TileSetIndeces.Add(tileIndeces);
            }
        }
        */

        /// <summary>This method reads the WED tileset indeces from the input stream.</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadOverlayTileSetIndeces(Stream input)
        {
            for (Int32 index = 0; index < (this.Overlays.Count - 1); ++index)
            {
                Int64 differential = this.Overlays[index + 1].OffsetTileIndeces - this.Overlays[index].OffsetTileIndeces;
                ReadOverlayTileSetIndecesAddCollection(input, index, differential);
            }

            //final case; read all remaining data between tilemap tileset indeces and ... wallgroups
            if (this.Overlays.Count > 0)
            {
                Int32 index = this.Overlays.Count - 1;
                Int64 differential = this.PolygonHeader.OffsetWalls - this.Overlays[index].OffsetTileIndeces;
                ReadOverlayTileSetIndecesAddCollection(input, index, differential);
            }
        }

        /// <summary>This method reads the WED wall groups from the input stream.</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadWallGroups(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.PolygonHeader.OffsetWalls);

            for (Int32 i = 0; i < this.StreamWallGroupCount; ++i)
            {
                WallGroup wwg = new WallGroup();
                wwg.Read(input);
                this.WallGroups.Add(wwg);
            }
        }

        /// <summary>This method reads the WED wall polygons from the input stream.</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadWallPolygons(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.PolygonHeader.OffsetPolygons);

            for (Int32 i = 0; i < this.PolygonHeader.WallPolygonCount; ++i)
            {
                Polygon polygon = new Polygon();
                polygon.Read(input);
                this.WallPolygons.Add(polygon);
            }
        }

        /// <summary>This method reads the WED door polygons from the input stream.</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadDoorPolygons(Stream input)
        {
            for (Int32 doorIndex = 0; doorIndex < this.Doors.Count; ++doorIndex)
            {
                Door door = this.Doors[doorIndex];

                ReusableIO.SeekIfAble(input, door.OffsetPolygonsOpen);
                for (Int32 opened = 0; opened < door.PolygonCountOpen; ++opened)
                {
                    Polygon p = new Polygon();
                    p.Read(input);
                    door.Polygons.Open.Add(p);
                }

                ReusableIO.SeekIfAble(input, door.OffsetPolygonsClosed);
                for (Int32 closed = 0; closed < door.PolygonCountClosed; ++closed)
                {
                    Polygon p = new Polygon();
                    p.Read(input);
                    door.Polygons.Closed.Add(p);
                }
            }
        }

        /// <summary>This method reads the WED wall polygon lookup table from the input stream.</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadWallPolygonLookupTable(Stream input)
        {
            //seek and read
            ReusableIO.SeekIfAble(input, this.PolygonHeader.OffsetPolygonIndeces);
            Byte[] data = ReusableIO.BinaryRead(input, this.WallGroupIndexCount * 2);

            for (Int32 i = 0; i < data.Length; i += 2)
                this.WallPolygonLookupTable.Add(ReusableIO.ReadInt16FromArray(data, i));
        }

        /// <summary>This method reads the WED vertices from the input stream.</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadVertices(Stream input)
        {
            Int32 vertices = this.VertexCount;
            ReusableIO.SeekIfAble(input, this.PolygonHeader.OffsetVertices);

            for (Int32 i = 0; i < vertices; ++i)
            {
                Vertex vertex = new Vertex();
                vertex.Read(input);
                this.Vertices.Add(vertex);
            }
        }
        #endregion

        #region Write IO
        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            this.MaintainMinimalDataIntegrity();

            this.Header.Write(output);
            this.WriteOverlays(output);
            this.WritePolygonHeader(output);
            this.WriteDoors(output);
            this.WriteTilemaps(output);
            this.WriteDoorTilemapIndeces(output);
            this.WriteTileSetIndeces(output);
            this.WriteWallGroups(output);
            this.WriteWallPolygons(output);
            this.WriteDoorPolygons(output);
            this.WriteWallPolygonLookupTable(output);
            this.WriteVertices(output);
        }

        /// <summary>This public method writes WED overlays to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteOverlays(Stream output)
        {
            foreach (Overlay overlay in this.Overlays)
                overlay.Write(output);
        }

        /// <summary>This public method writes WED doors to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteDoors(Stream output)
        {
            foreach (Door door in this.Doors)
                door.Write(output);
        }

        /// <summary>This public method writes the WED polygon header to the write stream.</summary>
        /// <param name="output">Stream to write to</param>
        protected void WritePolygonHeader(Stream output)
        {
            this.PolygonHeader.Write(output);
        }

        /// <summary>This public method writes the WED tilemaps to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteTilemaps(Stream output)
        {
            //Read overlay tilemaps
            foreach (Overlay overlay in this.Overlays)
                foreach (Tilemap tilemap in overlay.TileSetMapping.Tilemaps)
                    tilemap.Write(output);
        }

        /// <summary>This method writes the collection of WED tilemap indeces for doors to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteDoorTilemapIndeces(Stream output)
        {
            ReusableIO.SeekIfAble(output, this.Header.OffsetDoorTileMapIndeces);

            foreach (Door door in this.Doors)
                foreach (Int16 index in door.TilemapIndeces)
                    ReusableIO.WriteInt16ToStream(index, output);
        }

        /// <summary>This method writes the WED tileset indeces to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteTileSetIndeces(Stream output)
        {
            foreach (Overlay overlay in this.Overlays)
                foreach (Int16 index in overlay.TileSetMapping.TileSetIndeces)
                    ReusableIO.WriteInt16ToStream(index, output);
        }

        /// <summary>This method writes the WED wall groups to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteWallGroups(Stream output)
        {
            foreach (WallGroup wwg in this.WallGroups)
                wwg.Write(output);
        }

        /// <summary>This method writes the WED polygons to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteWallPolygons(Stream output)
        {
            foreach (Polygon p in this.WallPolygons)
                p.Write(output);
        }

        /// <summary>This method writes the WED door polygons to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteDoorPolygons(Stream output)
        {
            foreach (Door door in this.Doors)
            {
                foreach (Polygon p in door.Polygons.Open)
                    p.Write(output);

                foreach (Polygon p in door.Polygons.Closed)
                    p.Write(output);
            }
        }

        /// <summary>This method writes the WED wall polygon lookup table to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteWallPolygonLookupTable(Stream output)
        {
            foreach (Int16 polygonIndex in this.WallPolygonLookupTable)
                ReusableIO.WriteInt16ToStream(polygonIndex, output);
        }

        /// <summary>This method writes the WED vertices to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteVertices(Stream output)
        {
            foreach (Vertex vertex in this.Vertices)
                vertex.Write(output);
        }
        #endregion
        #endregion


        #region ToString() methods
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            return this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            //header
            builder.AppendLine("Header:");
            builder.AppendLine(this.Header.ToString());

            //Overlays
            for (Int32 index = 0; index < this.Overlays.Count; ++index)
                builder.AppendLine(this.Overlays[index].ToString(index));

            //Polygon header
            builder.AppendLine(this.PolygonHeader.ToString(true));

            //Doors
            for (Int32 index = 0; index < this.Doors.Count; ++index)
                builder.AppendLine(this.Doors[index].ToString(index));

            //Tile Maps
            builder.Append("Tile Maps:");
            for (Int32 outer = 0; outer < this.Overlays.Count; ++outer)
            {
                if (this.Overlays[outer].TileSetMapping.Tilemaps != null)
                {
                    builder.Append(StringFormat.ToStringAlignment(String.Format("Tilemap set {0} Count", outer)));
                    builder.Append(this.Overlays[outer].TileSetMapping.Tilemaps.Count);
                }
            }

            //Door Tilemap Indeces
            builder.AppendLine(String.Empty);
            builder.Append("Door Tilemap Index counts:");
            for (Int32 doorIndex = 0; doorIndex < this.Doors.Count; ++doorIndex)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Door {0} Count", doorIndex)));
                builder.Append(this.Doors[doorIndex].TilemapIndeces.Count);
            }

            //Tileset Indeces
            builder.AppendLine(String.Empty);
            builder.Append("Tile Set Indeces:");
            for (Int32 outer = 0; outer < this.Overlays.Count; ++outer)
            {
                if (this.Overlays[outer].TileSetMapping.TileSetIndeces != null)
                {
                    builder.Append(StringFormat.ToStringAlignment(String.Format("Tileset {0} Count", outer)));
                    builder.Append(this.Overlays[outer].TileSetMapping.TileSetIndeces.Count);
                }
            }

            //Wall Groups
            builder.AppendLine(String.Empty);
            for (Int32 index = 0; index < this.WallGroups.Count; ++index)
                builder.AppendLine(this.WallGroups[index].ToString(index));

            //Polygons
            for (Int32 index = 0; index < this.WallPolygons.Count; ++index)
                builder.AppendLine(this.WallPolygons[index].ToString(index));

            //Polygon lookup table
            builder.Append(StringFormat.ToStringAlignment("Polygon Lookup Table count"));
            builder.AppendLine(this.WallPolygonLookupTable.Count.ToString());

            //Vertecies
            for (Int32 index = 0; index < this.Vertices.Count; ++index)
                builder.AppendLine(this.Vertices[index].ToString(index));

            return builder.ToString();
        }
        #endregion

        #region Helper methods
        /// <summary>Reads tilemaps from the input stream and adds them to individual collections</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="threshold">Loop end threshold</param>
        protected List<Tilemap> ReadTilemapsAddCollection(Stream input, Int64 threshold)
        {
            //instantiate
            List<Tilemap> tilemaps = new List<Tilemap>();
            for (Int64 i = 0; i < threshold; ++i)
            {
                Tilemap tilemap = new Tilemap();
                tilemap.Read(input);
                tilemaps.Add(tilemap);
            }

            return tilemaps;
        }

        /// <summary>Reads a tilemap collection from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="overlayIndex">Current overlay index</param>
        /// <param name="differential">Offset differential between current overlay's tilemap offset and next offset</param>
        protected void ReadTilemapsReadCollection(Stream input, Int32 overlayIndex, Int64 differential)
        {
            Int64 diffCount = differential / Tilemap.StructSize;

            Overlay overlay = this.Overlays[overlayIndex];

            //seek to reading point
            ReusableIO.SeekIfAble(input, overlay.OffsetTilemap);
            overlay.TileSetMapping.Tilemaps = this.ReadTilemapsAddCollection(input, diffCount);
        }

        /// <summary>Reads tileset indeces for a tilemap collection</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="index">Current overlay index</param>
        /// <param name="differential">Offset differential between current overlay's tilemap tileset indeces offset and next offset</param>
        protected void ReadOverlayTileSetIndecesAddCollection(Stream input, Int32 index, Int64 differential)
        {
            Overlay overlay = this.Overlays[index];
            ReusableIO.SeekIfAble(input, overlay.OffsetTileIndeces); //seek
            List<Int16> tileIndeces = new List<Int16>();    //instantiate a new collection

            //one read is faster than individual 2-byte reads
            Byte[] data = ReusableIO.BinaryRead(input, differential);

            for (Int32 datumIndex = 0; datumIndex < data.Length; datumIndex += 2)
                tileIndeces.Add(ReusableIO.ReadInt16FromArray(data, datumIndex));

            //assign to the mapping
            overlay.TileSetMapping.TileSetIndeces = tileIndeces;
        }
        #endregion

        
        #region Data Integrity
        /// <summary>Maintains mimimal data integrity by not lying to the output data file.</summary>
        /// <remarks>
        ///     CRE 2.2 was neighboring absurd to allow the user to define offsets;
        ///     to do so with WED is past absurdity. Instead, the minimal data interity will pass through
        ///     all data structures and rewrite offsets and counts as appropriate.
        /// </remarks>
        protected void MaintainMinimalDataIntegrity()
        {
            //proceed through a logical read of the file that is in memory.

            Int64 position = 0; //start at the header

            //logically read the header
            position += WedHeader.StructSize;
            this.Header.OffsetOverlays = Convert.ToUInt32(position);  //set the overlay offset
            
            //logically read the overlays
            position += (Overlay.StructSize * this.Overlays.Count);
            this.Header.OffsetPolygonHeader = Convert.ToUInt32(position);   //set the polygons header offset

            //logicaly read the secondary polygon header
            position += PolygonHeader.StructSize;
            this.Header.OffsetDoors = Convert.ToUInt32(position);   //set the doors header offset

            //logically read the doors
            position += (this.Doors.Count * Door.StructSize);

            //loop through overlays and set the tileset offsets accordingly
            for (Int32 overlayIndex = 0; overlayIndex < this.Overlays.Count; ++overlayIndex)
            {
                Overlay overlay = this.Overlays[overlayIndex];
                overlay.OffsetTilemap = Convert.ToUInt32(position);   //set the tilemap offset

                //logically read the tilemaps
                position += (overlay.TileSetMapping.Tilemaps.Count * Tilemap.StructSize);
            }
            this.Header.OffsetDoorTileMapIndeces = Convert.ToUInt32(position);   //set the doors' tilemap indeces offset

            //loop through doors' tileset indeces
            foreach (Door door in this.Doors)
                position += (door.TilemapIndeces.Count * 2);

            //loop through the overlays' tileset indeces
            for (Int32 overlayIndex = 0; overlayIndex < this.Overlays.Count; ++overlayIndex)
            {
                Overlay overlay = this.Overlays[overlayIndex];
                overlay.OffsetTileIndeces = Convert.ToUInt32(position);   //set the tileset indeces offset

                //logically read the tileset indeces
                position += (overlay.TileSetMapping.TileSetIndeces.Count * 2);
            }

            //set the wall groups' offset
            this.PolygonHeader.OffsetWalls = Convert.ToUInt32(position);
            position += (this.WallGroups.Count * WallGroup.StructSize);     //logically read the wall groups

            //set the walls' polygons' offsets
            this.PolygonHeader.OffsetPolygons = Convert.ToUInt32(position);
            position += (this.WallPolygons.Count * Polygon.StructSize); //logically read the wall polygons


            //loop through the door polygons
            for (Int32 doorIndex = 0; doorIndex < this.Doors.Count; ++doorIndex)
            {
                Door door = this.Doors[doorIndex];

                door.OffsetPolygonsOpen = Convert.ToInt32(position);   //set the open polygons offset
                position += (door.Polygons.Open.Count * Polygon.StructSize);

                door.OffsetPolygonsClosed = Convert.ToInt32(position);   //set the closed polygons offset
                position += (door.Polygons.Closed.Count * Polygon.StructSize);
            }

            //set the polygon indeces offset
            this.PolygonHeader.OffsetPolygonIndeces = Convert.ToUInt32(position);
            position += (this.WallPolygonLookupTable.Count * 2);

            //set the vertex offset
            this.PolygonHeader.OffsetVertices = Convert.ToUInt32(position);

            //no need to logically consume vertices, since no data follows
        }
        #endregion
    }
}