using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareIndexFileFormat.Components;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareIndexFileFormat.Interface;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TileSet.Tis1;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareIndexFileFormat.Version
{
    /// <summary>This class represents a BIFF version 1 file. It can extract resources.</summary>
    public class Biff1Archive : IInfinityFormat, IBiff
    {
        #region Fields
        /// <summary>This member represents the BIFF version1 header.</summary>
        public Biff1Header Header;

        /// <summary>This member represents the collection of resource1 entries within the BIFF.</summary>
        public List<Biff1ResourceEntry> EntriesResource;

        /// <summary>This member represents the collection of tileset entries withing the version 1 BIFF.</summary>
        public List<Biff1TilesetEntry> EntriesTileset;

        /// <summary>This member contains the filepath of the target BIFF archive.</summary>
        public String FilePath;

        /// <summary>This member contains the collection of binary data of resources, if read and set appropriately</summary>
        public List<Byte[]> DataResource;

        /// <summary>This member contains the collection of binary data of tilesets, if read and set appropriately</summary>
        public List<Byte[]> DataTileset;

        /// <summary>This member indicates whether or not to persist data of tilesets and resources during read and/or write</summary>
        public Boolean MaintainData;
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public Biff1Archive()
        {
            this.MaintainData = false;
        }

        /// <summary>Partial definition constructor</summary>
        public Biff1Archive(Boolean maintainBinaryData)
        {
            this.MaintainData = maintainBinaryData;
        }

        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.Header = new Biff1Header();
            this.EntriesResource = new List<Biff1ResourceEntry>();
            this.EntriesTileset = new List<Biff1TilesetEntry>();
            this.DataResource = new List<Byte[]>();
            this.DataTileset = new List<Byte[]>();
        }
        #endregion
        

        #region IO method implemetations
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
                this.Header = new Biff1Header();
                this.Header.Read(input, false);
            }
        }


        /// <summary>Reads the BIFF archive file using incoming stream</summary>
        /// <param name="input">Stream opened to the BIFF archive data</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            this.Header.Read(input);

            //seek to offset
            ReusableIO.SeekIfAble(input, this.Header.OffsetResource, SeekOrigin.Begin);

            for (Int32 i = 0; i < this.Header.CountResource; ++i)
            {
                Biff1ResourceEntry entry = new Biff1ResourceEntry();
                entry.Read(input);
                this.EntriesResource.Add(entry);
            }

            //seek to offset
            ReusableIO.SeekIfAble(input, this.Header.OffsetTileset, SeekOrigin.Begin);

            for (Int32 i = 0; i < this.Header.CountTileset; ++i)
            {
                Biff1TilesetEntry entry = new Biff1TilesetEntry();
                entry.Read(input);
                this.EntriesTileset.Add(entry);
            }

            //Read data is unnecessarily sizable with larger BIFF archives. Only read fully if distinctly directed to.
            if (this.MaintainData)
            {
                Byte[] data;

                //read each resource1
                for (Int32 i = 0; i < this.Header.CountResource; ++i)
                {
                    //seek to offset
                    ReusableIO.SeekIfAble(input, this.EntriesResource[i].OffsetResource, SeekOrigin.Begin);
                    data = ReusableIO.BinaryRead(input, this.EntriesResource[i].SizeResource);
                    this.DataResource.Add(data);
                }

                //read each tileset
                for (Int32 i = 0; i < this.Header.CountTileset; ++i)
                {
                    //seek to offset
                    ReusableIO.SeekIfAble(input, this.EntriesTileset[i].OffsetResource, SeekOrigin.Begin);
                    data = ReusableIO.BinaryRead(input, this.EntriesTileset[i].SizeResource);
                    this.DataTileset.Add(data);
                }
            }
        }

        /// <summary>Reads the BIFF archive file defined in the passed file name</summary>
        /// <param name="filePath">String describing the path to the BIFF archive file.</param>
        public void Read(String filePath)
        {
            this.FilePath = filePath;
            using (FileStream input = ReusableIO.OpenFile(this.FilePath, FileMode.Open, FileAccess.Read))
                this.Read(input);
        }

        /// <summary>This public method writes the BIFF archive data to an output stream</summary>
        /// <param name="output">Stream object into which to write to</param>
        public void Write(Stream output)
        {
            //write header
            this.Header.Write(output);

            //seek to offset
            ReusableIO.SeekIfAble(output, this.Header.OffsetResource);
            for (Int32 i = 0; i < this.Header.CountResource; ++i)
                this.EntriesResource[i].Write(output);

            //seek to offset
            ReusableIO.SeekIfAble(output, this.Header.OffsetTileset);
            for (Int32 i = 0; i < this.Header.CountTileset; ++i)
                this.EntriesTileset[i].Write(output);

            //Read data is unnecessarily sizable with larger BIFF archives. Only read fully if distinctly directed to.
            if (this.MaintainData)
            {
                //read each resource1
                for (Int32 i = 0; i < this.Header.CountResource; ++i)
                {
                    //seek to offset
                    ReusableIO.SeekIfAble(output, this.EntriesResource[i].OffsetResource, SeekOrigin.Begin);
                    output.Write(this.DataResource[i], 0, this.DataResource[i].Length);
                }

                //read each tileset
                for (Int32 i = 0; i < this.Header.CountTileset; ++i)
                {
                    //seek to offset
                    ReusableIO.SeekIfAble(output, this.EntriesTileset[i].OffsetResource, SeekOrigin.Begin);
                    output.Write(this.DataTileset[i], 0, this.DataTileset[i].Length);
                }
            }
        }
        #endregion


        #region IBiff
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
            Byte[] data = this.DataResource[resourceIndex];
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
            if (this.MaintainData || this.DataResource[resourceIndex] == null)
            {
                using (FileStream file = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read))
                {
                    ReusableIO.SeekIfAble(file, this.EntriesResource[resourceIndex].OffsetResource, SeekOrigin.Begin);

                    Byte[] binData = ReusableIO.BinaryRead(file, this.EntriesResource[resourceIndex].SizeResource);
                    if (this.MaintainData)
                        this.DataResource[resourceIndex] = binData;

                    data = new MemoryStream();
                }
            }
            else
                data = new MemoryStream(this.DataResource[resourceIndex]);
            
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

            if (resourceIndex <= this.EntriesResource.Count)
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
            Byte[] data = this.DataTileset[tilesetIndex];
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
            if (this.MaintainData || this.DataResource[tilesetIndex] == null)
            {
                using (FileStream file = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read))
                {
                    ReusableIO.SeekIfAble(file, this.EntriesTileset[tilesetIndex].OffsetResource, SeekOrigin.Begin);
                    binData = ReusableIO.BinaryRead(file, this.EntriesResource[tilesetIndex].SizeResource);

                    if (this.MaintainData)
                        this.DataTileset[tilesetIndex] = binData;
                }
            }
            else
                binData = this.DataTileset[tilesetIndex];

            //Compile a header, then return the data stream.
            Tis1Header tilesetHeader = new Tis1Header();
            tilesetHeader.Signature = "TIS ";
            tilesetHeader.Version = "V1  ";
            tilesetHeader.CountTiles = this.EntriesTileset[tilesetIndex].CountTile;
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

            if (tilesetIndex <= this.EntriesTileset.Count)
                found = true;

            return found;
        }
        #endregion


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
            builder.Append(StringFormat.ToStringAlignment("File path"));
            builder.Append(this.FilePath);
            builder.Append(StringFormat.ToStringAlignment("Maintain data"));
            builder.AppendLine(this.MaintainData.ToString());

            builder.Append(this.Header.ToString());

            builder.Append(StringFormat.ToStringAlignment("Resource Entries"));
            if (LongDefinition)
            {
                for (Int32 i = 0; i < this.EntriesResource.Count; ++i)
                {
                    builder.Append(StringFormat.ToStringAlignment(String.Format("Index {0}", i)));
                    builder.Append(this.EntriesResource[i].ToString());
                }
            }
            else
            {
                builder.Append(StringFormat.ToStringAlignment("Count"));
                builder.AppendLine(this.EntriesResource.Count.ToString());
            }


            builder.Append(StringFormat.ToStringAlignment("Tileset Entries"));
            if (LongDefinition)
            {
                for (Int32 i = 0; i < this.EntriesTileset.Count; ++i)
                {
                    builder.Append(StringFormat.ToStringAlignment(String.Format("Index {0}", i)));
                    builder.Append(this.EntriesTileset[i].ToString());
                }
            }
            else
            {
                builder.Append(StringFormat.ToStringAlignment("Count"));
                builder.AppendLine(this.EntriesTileset.Count.ToString());
            }


            builder.Append(StringFormat.ToStringAlignment("Resource Data"));
            if (LongDefinition && this.DataResource.Count > 0)
            {
                for (Int32 i = 0; i < this.DataResource.Count; ++i)
                {
                    builder.Append(StringFormat.ToStringAlignment(String.Format("Index {0}", i)));
                    builder.Append(this.DataResource[i].ToString());
                }
            }
            else
            {
                builder.Append(StringFormat.ToStringAlignment("Count"));
                builder.AppendLine(this.DataResource.Count.ToString());
            }


            builder.Append(StringFormat.ToStringAlignment("Tileset Data"));
            if (LongDefinition && this.DataTileset.Count > 0)
            {
                for (Int32 i = 0; i < this.DataTileset.Count; ++i)
                {
                    builder.Append(StringFormat.ToStringAlignment(String.Format("Index {0}", i)));
                    builder.Append(this.DataTileset[i].ToString());
                }
            }
            else
            {
                builder.Append(StringFormat.ToStringAlignment("Count"));
                builder.AppendLine(this.DataTileset.Count.ToString());
            }

            return builder.ToString();
        }
        #endregion
    }
}