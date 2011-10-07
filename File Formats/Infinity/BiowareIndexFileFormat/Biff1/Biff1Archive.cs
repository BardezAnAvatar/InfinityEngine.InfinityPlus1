using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Common;
using Bardez.Projects.InfinityPlus1.Files.Infinity.TileSet.Tis1;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.BiowareIndexFileFormat.Biff1
{
    /// <summary>This class represents a BIFF version 1 file. It can extract resources.</summary>
    public class Biff1Archive
    {
        #region Members
        /// <summary>This member represents the BIFF version1 header.</summary>
        protected Biff1Header header;

        /// <summary>This member represents the collection of resource1 entries within the BIFF.</summary>
        protected List<Biff1ResourceEntry> entriesResource;

        /// <summary>This member represents the collection of tileset entries withing the version 1 BIFF.</summary>
        protected List<Biff1TilesetEntry> entriesTileset;

        /// <summary>This member contains the filepath of the target BIFF archive.</summary>
        protected String filePath;

        /// <summary>This member contains the collection of binary data of resources, if read and set appropriately</summary>
        protected List<Byte[]> dataResource;

        /// <summary>This member contains the collection of binary data of tilesets, if read and set appropriately</summary>
        protected List<Byte[]> dataTileset;

        /// <summary>This member indicates whether or not to persist data of tilesets and resources during read and/or write</summary>
        protected Boolean maintainData;
        #endregion

        #region Constructor(s)
        /// <summary>Default constructor</summary>
        public Biff1Archive()
        {
            this.ClearData();       //instantiate resources, entries and header
            this.filePath = null;
            this.maintainData = false;
        }
        #endregion

        /// <summary>Reads the BIFF archive file using incoming stream</summary>
        /// <param name="input">Stream opened to the BIFF archive data</param>
        public void Read(Stream input)
        {
            this.ClearData();       //wipe everything in place

            this.header.Read(input);

            //seek to offset
            ReusableIO.SeekIfAble(input, this.header.OffsetResource, SeekOrigin.Begin);

            for (Int32 i = 0; i < this.header.CountResource; ++i)
            {
                Biff1ResourceEntry entry = new Biff1ResourceEntry();
                entry.Read(input);
                this.entriesResource.Add(entry);
            }

            //seek to offset
            ReusableIO.SeekIfAble(input, this.header.OffsetTileset, SeekOrigin.Begin);

            for (Int32 i = 0; i < this.header.CountTileset; ++i)
            {
                Biff1TilesetEntry entry = new Biff1TilesetEntry();
                entry.Read(input);
                this.entriesTileset.Add(entry);
            }

            //Read data is unnecessarily sizable with larger BIFF archives. Only read fully if distinctly directed to.
            if (this.maintainData)
            {
                Byte[] data;
                this.ClearResources();

                //read each resource1
                for (Int32 i = 0; i < this.header.CountResource; ++i)
                {
                    //seek to offset
                    ReusableIO.SeekIfAble(input, this.entriesResource[i].OffsetResource, SeekOrigin.Begin);
                    data = ReusableIO.BinaryRead(input, this.entriesResource[i].SizeResource);
                    this.dataResource.Add(data);
                }

                //read each tileset
                for (Int32 i = 0; i < this.header.CountTileset; ++i)
                {
                    //seek to offset
                    ReusableIO.SeekIfAble(input, this.entriesTileset[i].OffsetResource, SeekOrigin.Begin);
                    data = ReusableIO.BinaryRead(input, this.entriesTileset[i].SizeResource);
                    this.dataTileset.Add(data);
                }
            }
        }

        /// <summary>Reads the BIFF archive file defined in the passed file name</summary>
        /// <param name="filePath">String describing the path to the BIFF archive file.</param>
        public void Read(String filePath)
        {
            this.filePath = filePath;
            using (FileStream input = ReusableIO.OpenFile(this.filePath, FileMode.Open, FileAccess.Read))
                this.Read(input);
        }

        /// <summary>This public method writes the BIFF archive data to an output stream</summary>
        /// <param name="output">Stream object into which to write to</param>
        public void Write(Stream output)
        {
            //write header
            this.header.Write(output);

            //seek to offset
            ReusableIO.SeekIfAble(output, this.header.OffsetResource, SeekOrigin.Begin);
            for (Int32 i = 0; i < this.header.CountResource; ++i)
                this.entriesResource[i].Write(output);

            //seek to offset
            ReusableIO.SeekIfAble(output, this.header.OffsetTileset, SeekOrigin.Begin);
            for (Int32 i = 0; i < this.header.CountTileset; ++i)
                this.entriesTileset[i].Write(output);

            //Read data is unnecessarily sizable with larger BIFF archives. Only read fully if distinctly directed to.
            if (this.maintainData)
            {
                this.ClearResources();

                //read each resource1
                for (Int32 i = 0; i < this.header.CountResource; ++i)
                {
                    //seek to offset
                    ReusableIO.SeekIfAble(output, this.entriesResource[i].OffsetResource, SeekOrigin.Begin);
                    output.Write(this.dataResource[i], 0, this.dataResource[i].Length);
                }

                //read each tileset
                for (Int32 i = 0; i < this.header.CountTileset; ++i)
                {
                    //seek to offset
                    ReusableIO.SeekIfAble(output, this.entriesTileset[i].OffsetResource, SeekOrigin.Begin);
                    output.Write(this.dataTileset[i], 0, this.dataTileset[i].Length);
                }
            }
        }
        
        /// <summary>Opens a file and extracts the resource1 to that file path</summary>
        /// <param name="resourcePath">File path to extract to. Must include file extension</param>
        /// <param name="resourceLocator">Resource locator to match upon</param>
        public void ExtractResource(String resourcePath, ResourceLocator1 resourceLocator)
        {
            this.ExtractResource(resourcePath, resourceLocator.ResourceIndex);
        }

        /// <summary>Extracts the resource1 to the output stream</summary>
        /// <param name="output">Output stream to write to. Not closed during method.</param>
        /// <param name="resourceLocator">Resource locator to match upon</param>
        public void ExtractResource(Stream output, ResourceLocator1 resourceLocator)
        {
            this.ExtractResource(output, resourceLocator.ResourceIndex);
        }

        /// <summary>Opens a file and extracts the resource1 to that file path</summary>
        /// <param name="resourcePath">File path to extract to. Must include file extension</param>
        /// <param name="resourceIndex">Resource index to write</param>
        public void ExtractResource(String resourcePath, Int32 resourceIndex)
        {
            using (FileStream file = new FileStream(resourcePath, FileMode.OpenOrCreate, FileAccess.Write))
                this.ExtractResource(file, resourceIndex);
        }

        /// <summary>Extracts the resource1 to the output stream</summary>
        /// <param name="output">Output stream to write to. Not closed during method.</param>
        /// <param name="resourceIndex">Resource index to write</param>
        public void ExtractResource(Stream output, Int32 resourceIndex)
        {
            Byte[] data = this.dataResource[resourceIndex];
            output.Write(data, 0, data.Length);
        }

        /// <summary>Extracts the resource1 and returns it in a memorystream</summary>
        /// <param name="resourceLocator">Resource locator to match upon</param>
        /// <returns>A MemoryStrteam of the binary data (for read and the like)</returns>
        public MemoryStream ExtractResource(ResourceLocator1 resourceLocator)
        {
            return this.ExtractResource(resourceLocator.ResourceIndex);
        }

        /// <summary>Extracts the resource1 and returns it in a memorystream</summary>
        /// <param name="resourceLocator">Resource locator to match upon</param>
        /// <param name="resourceIndex">Resource index to extract</param>
        public MemoryStream ExtractResource(Int32 resourceIndex)
        {
            MemoryStream data = null;
            //follow maintaindata and open if not
            if (this.maintainData || this.dataResource[resourceIndex] == null)
            {
                using (FileStream file = new FileStream(this.filePath, FileMode.Open, FileAccess.Read))
                {
                    ReusableIO.SeekIfAble(file, this.entriesResource[resourceIndex].OffsetResource, SeekOrigin.Begin);

                    Byte[] binData = ReusableIO.BinaryRead(file, this.entriesResource[resourceIndex].SizeResource);
                    if (this.maintainData)
                        this.dataResource[resourceIndex] = binData;

                    data = new MemoryStream();
                }
            }
            else
                data = new MemoryStream(this.dataResource[resourceIndex]);
            
            return data;
        }

        /// <summary>Check whether the resource1 index exists in the BIFF header</summary>
        /// <param name="resourceLocator">Resource locator to match upon</param>
        /// <returns>A Boolean existing whether the index is populated</returns>
        public Boolean ResourceExists(ResourceLocator1 resourceLocator)
        {
            return this.ResourceExists(resourceLocator.ResourceIndex);
        }

        /// <summary>Check whether the resource1 index exists in the BIFF header</summary>
        /// <param name="resourceIndex">Resource index to evaluate</param>
        /// <returns>A Boolean existing whether the index is populated</returns>
        public Boolean ResourceExists(Int32 resourceIndex)
        {
            Boolean found = false;

            if (resourceIndex <= this.entriesResource.Count)
                found = true;

            return found;
        }

        /// <summary>Opens a file and extracts the tileset to that file path</summary>
        /// <param name="tilesetPath">File path to extract to. Must include file extension</param>
        /// <param name="tilesetLocator">Tileset locator to match upon</param>
        public void ExtractTileset(String tilesetPath, ResourceLocator1 tilesetLocator)
        {
            this.ExtractTileset(tilesetPath, tilesetLocator.TilesetIndex);
        }

        /// <summary>Extracts the tileset to the output stream</summary>
        /// <param name="output">Output stream to write to. Not closed during method.</param>
        /// <param name="tilesetLocator">Tileset locator to match upon</param>
        public void ExtractTileset(Stream output, ResourceLocator1 tilesetLocator)
        {
            this.ExtractTileset(output, tilesetLocator.TilesetIndex);
        }

        /// <summary>Opens a file and extracts the tileset to that file path</summary>
        /// <param name="tilesetPath">File path to extract to. Must include file extension</param>
        /// <param name="tilesetIndex">Tileset index to write</param>
        public void ExtractTileset(String tilesetPath, Int32 tilesetIndex)
        {
            using (FileStream file = new FileStream(tilesetPath, FileMode.OpenOrCreate, FileAccess.Write))
                this.ExtractTileset(file, tilesetIndex);
        }

        /// <summary>Extracts the tileset to the output stream</summary>
        /// <param name="output">Output stream to write to. Not closed during method.</param>
        /// <param name="tilesetIndex">Tileset index to write</param>
        public void ExtractTileset(Stream output, Int32 tilesetIndex)
        {
            Byte[] data = this.dataTileset[tilesetIndex];
            output.Write(data, 0, data.Length);
        }

        /// <summary>Extracts the tileset and returns it in a memorystream</summary>
        /// <param name="tilesetLocator">Tileset locator to match upon</param>
        /// <returns>A MemoryStrteam of the binary data (for read and the like)</returns>
        public MemoryStream ExtractTileset(ResourceLocator1 tilesetLocator)
        {
            return this.ExtractTileset(tilesetLocator.TilesetIndex);
        }

        /// <summary>Extracts the tileset and returns it in a memorystream</summary>
        /// <param name="tilesetLocator">Tileset locator to match upon</param>
        /// <param name="tilesetIndex">Tileset index to extract</param>
        public MemoryStream ExtractTileset(Int32 tilesetIndex)
        {
            MemoryStream data = null;
            Byte[] binData;

            //follow maintaindata and open if not maintained
            if (this.maintainData || this.dataResource[tilesetIndex] == null)
            {
                using (FileStream file = new FileStream(this.filePath, FileMode.Open, FileAccess.Read))
                {
                    ReusableIO.SeekIfAble(file, this.entriesTileset[tilesetIndex].OffsetResource, SeekOrigin.Begin);
                    binData = ReusableIO.BinaryRead(file, this.entriesResource[tilesetIndex].SizeResource);

                    if (this.maintainData)
                        this.dataTileset[tilesetIndex] = binData;
                }
            }
            else
                binData = this.dataTileset[tilesetIndex];

            //Compile a header, then return the data stream.
            Tis1Header tilesetHeader = new Tis1Header();
            tilesetHeader.Signature = "TIS ";
            tilesetHeader.Version = "V1  ";
            tilesetHeader.CountTiles = this.entriesTileset[tilesetIndex].CountTile;
            tilesetHeader.LengthSingleTileData = 5120;      //4-byte 256-index palette
            tilesetHeader.OffsetTileData = 0x18;            //size of a TIS1 header is 0x18 
            tilesetHeader.DimensionTile = 64;               //the dimension of all tiles is 64x64

            //Instantiate a memory stream and write the header
            data = new MemoryStream(0x18 + binData.Length); //size of a TIS1 header is 0x18
            ReusableIO.SeekIfAble(data, 0, SeekOrigin.Begin);
            tilesetHeader.Write(data);
            data.Write(binData, 0, binData.Length);

            return data;
        }

        /// <summary>Check whether the tileset index exists in the BIFF header</summary>
        /// <param name="tilesetLocator">Tileset locator to match upon</param>
        /// <returns>A Boolean existing whether the index is populated</returns>
        public Boolean TilesetExists(ResourceLocator1 tilesetLocator)
        {
            return this.TilesetExists(tilesetLocator.TilesetIndex);
        }

        /// <summary>Check whether the tileset index exists in the BIFF header</summary>
        /// <param name="tilesetIndex">Tileset index to evaluate</param>
        /// <returns>A Boolean existing whether the index is populated</returns>
        public Boolean TilesetExists(Int32 tilesetIndex)
        {
            Boolean found = false;

            if (tilesetIndex <= this.entriesTileset.Count)
                found = true;

            return found;
        }

        /// <summary>This method resets the resource1 data, tileset data to blank Lists</summary>
        protected void ClearResources()
        {
            this.dataResource = new List<Byte[]>();
            this.dataTileset = new List<Byte[]>();
        }

        /// <summary>This method instantiates a new header, </summary>
        protected void ClearData()
        {
            this.header = new Biff1Header();
            this.entriesResource = new List<Biff1ResourceEntry>();
            this.entriesTileset = new List<Biff1TilesetEntry>();
            this.ClearResources();
        }

        #region ToString(...)
        /// <summary>This method will write the entire BIFF archive to a String builder and return it</summary>
        /// <returns>A string containing textual representations of internal variables, then the header, then entry and data objects</returns>
        public override String ToString()
        {
            return this.ToString(true);
        }

        /// <summary>This method will write the entire BIFF archive to a String builder and return it</summary>
        /// <param name="LongDefinition">Boolean indicating whether to write longer descriptions of the file</param>
        /// <returns>A string containing textual representations of internal variables, then the header, then entry and data objects</returns>
        public String ToString(Boolean LongDefinition)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("File path:          ");
            builder.AppendLine(this.filePath);
            builder.Append("Maintain data:      ");
            builder.AppendLine(this.maintainData.ToString());

            builder.AppendLine("Header:");
            builder.Append(this.header.ToString());

            builder.AppendLine("Resource Entries:");
            if (LongDefinition)
            {
                for (Int32 i = 0; i < this.entriesResource.Count; ++i)
                {
                    builder.Append("\tIndex ");
                    builder.Append(i.ToString());
                    builder.AppendLine(":");
                    builder.Append(this.entriesResource[i].ToString());
                }
            }
            else
            {
                builder.AppendLine("Count:  ");
                builder.AppendLine(this.entriesResource.Count.ToString());
            }


            builder.AppendLine("Tileset Entries:");
            if (LongDefinition)
            {
                for (Int32 i = 0; i < this.entriesTileset.Count; ++i)
                {
                    builder.Append("\tIndex ");
                    builder.Append(i.ToString());
                    builder.AppendLine(":");
                    builder.Append(this.entriesTileset[i].ToString());
                }
            }
            else
            {
                builder.AppendLine("Count:  ");
                builder.AppendLine(this.entriesTileset.Count.ToString());
            }


            builder.AppendLine("Resource Data:");
            if (LongDefinition)
            {
                for (Int32 i = 0; i < this.dataResource.Count; ++i)
                {
                    builder.Append("\tIndex ");
                    builder.Append(i.ToString());
                    builder.AppendLine(":");
                    builder.Append(this.dataResource[i].ToString());
                }
            }
            else
            {
                builder.AppendLine("Count:  ");
                builder.AppendLine(this.dataResource.Count.ToString());
            }


            builder.AppendLine("Tileset Data:");
            if (LongDefinition)
            {
                for (Int32 i = 0; i < this.dataTileset.Count; ++i)
                {
                    builder.Append("\tIndex ");
                    builder.Append(i.ToString());
                    builder.AppendLine(":");
                    builder.Append(this.dataTileset[i].ToString());
                }
            }
            else
            {
                builder.AppendLine("Count:  ");
                builder.AppendLine(this.dataTileset.Count.ToString());
            }

            return builder.ToString();
        }
        #endregion
    }
}