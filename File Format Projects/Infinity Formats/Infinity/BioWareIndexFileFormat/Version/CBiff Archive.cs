using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareIndexFileFormat.Interface;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Save;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareIndexFileFormat.Version
{
    /// <summary>Represents a *.CBF archive for IWD, essentially a BIFF inside a *.SAV file</summary>
    public class CBiffArchive : IBiff
    {
        #region Fields
        /// <summary>BIFF archive of the data</summary>
        public Biff1Archive Archive { get; set; }

        /// <summary>Represents the *.SAV format encapsulating the BIFF archive</summary>
        public SaveFile CompressedArchive { get; set; }

        /// <summary>SaveManager that manages the compression of the archive</summary>
        public SaveManager CompressionManager { get; set; }

        /// <summary>Represents the archive name within the CBiff</summary>
        public String ArchiveName { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.Archive = new Biff1Archive(true);
            this.CompressedArchive = new SaveFile();
            this.CompressionManager = new SaveManager();
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
            this.Read(input);
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            //read the archive compressed archive
            this.CompressedArchive.Read(input);
            this.CompressionManager.SavedResources = this.CompressedArchive;

            //extract the first file in the SAV file, should be only one BIFF per file
            this.ArchiveName = this.CompressedArchive.Resources[0].ResourceName;
            using (MemoryStream decompressedData = this.CompressionManager.ExtractResource(this.ArchiveName))
                this.Archive.Read(decompressedData);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                this.Archive.Write(memStream);

                ReusableIO.SeekIfAble(memStream, 0);

                //remove any existing BIFFs
                this.CompressionManager.SavedResources.Resources.Clear();
                this.CompressionManager.AddResource(this.ArchiveName, memStream, Convert.ToInt32(memStream.Length));
            }

            //write the compressed archive
            this.CompressedArchive.Write(output);
        }
        #endregion


        #region IBiff
        /// <summary>Opens a file and extracts the resource1 to that file path</summary>
        /// <param name="resourcePath">File path to extract to. Must include file extension</param>
        /// <param name="resourceLocator">Resource locator to match upon</param>
        public void ExtractResource(String resourcePath, ResourceLocator1 resourceLocator)
        {
            this.Archive.ExtractResource(resourcePath, resourceLocator);
        }

        /// <summary>Extracts the resource1 to the output stream</summary>
        /// <param name="output">Output stream to write to. Not closed during method.</param>
        /// <param name="resourceLocator">Resource locator to match upon</param>
        public void ExtractResource(Stream output, ResourceLocator1 resourceLocator)
        {
            this.Archive.ExtractResource(output, resourceLocator);
        }

        /// <summary>Opens a file and extracts the resource1 to that file path</summary>
        /// <param name="resourcePath">File path to extract to. Must include file extension</param>
        /// <param name="resourceIndex">Resource index to write</param>
        public void ExtractResource(String resourcePath, Int32 resourceIndex)
        {
            this.Archive.ExtractResource(resourcePath, resourceIndex);
        }

        /// <summary>Extracts the resource1 to the output stream</summary>
        /// <param name="output">Output stream to write to. Not closed during method.</param>
        /// <param name="resourceIndex">Resource index to write</param>
        public void ExtractResource(Stream output, Int32 resourceIndex)
        {
            this.Archive.ExtractResource(output, resourceIndex);
        }

        /// <summary>Extracts the resource1 and returns it in a memorystream</summary>
        /// <param name="resourceLocator">Resource locator to match upon</param>
        /// <returns>A MemoryStrteam of the binary data (for read and the like)</returns>
        public MemoryStream ExtractResource(ResourceLocator1 resourceLocator)
        {
            return this.Archive.ExtractResource(resourceLocator);
        }

        /// <summary>Extracts the resource1 and returns it in a memorystream</summary>
        /// <param name="resourceLocator">Resource locator to match upon</param>
        /// <param name="resourceIndex">Resource index to extract</param>
        public MemoryStream ExtractResource(Int32 resourceIndex)
        {
            return this.Archive.ExtractResource(resourceIndex);
        }

        /// <summary>Check whether the resource1 index exists in the BIFF header</summary>
        /// <param name="resourceLocator">Resource locator to match upon</param>
        /// <returns>A Boolean existing whether the index is populated</returns>
        public Boolean ResourceExists(ResourceLocator1 resourceLocator)
        {
            return this.Archive.ResourceExists(resourceLocator);
        }

        /// <summary>Check whether the resource1 index exists in the BIFF header</summary>
        /// <param name="resourceIndex">Resource index to evaluate</param>
        /// <returns>A Boolean existing whether the index is populated</returns>
        public Boolean ResourceExists(Int32 resourceIndex)
        {
            return this.Archive.ResourceExists(resourceIndex);
        }

        /// <summary>Opens a file and extracts the tileset to that file path</summary>
        /// <param name="tilesetPath">File path to extract to. Must include file extension</param>
        /// <param name="tilesetLocator">Tileset locator to match upon</param>
        public void ExtractTileset(String tilesetPath, ResourceLocator1 tilesetLocator)
        {
            this.Archive.ExtractTileset(tilesetPath, tilesetLocator);
        }

        /// <summary>Extracts the tileset to the output stream</summary>
        /// <param name="output">Output stream to write to. Not closed during method.</param>
        /// <param name="tilesetLocator">Tileset locator to match upon</param>
        public void ExtractTileset(Stream output, ResourceLocator1 tilesetLocator)
        {
            this.Archive.ExtractTileset(output, tilesetLocator);
        }

        /// <summary>Opens a file and extracts the tileset to that file path</summary>
        /// <param name="tilesetPath">File path to extract to. Must include file extension</param>
        /// <param name="tilesetIndex">Tileset index to write</param>
        public void ExtractTileset(String tilesetPath, Int32 tilesetIndex)
        {
            this.Archive.ExtractTileset(tilesetPath, tilesetIndex);
        }

        /// <summary>Extracts the tileset to the output stream</summary>
        /// <param name="output">Output stream to write to. Not closed during method.</param>
        /// <param name="tilesetIndex">Tileset index to write</param>
        public void ExtractTileset(Stream output, Int32 tilesetIndex)
        {
            this.Archive.ExtractTileset(output, tilesetIndex);
        }

        /// <summary>Extracts the tileset and returns it in a memorystream</summary>
        /// <param name="tilesetLocator">Tileset locator to match upon</param>
        /// <returns>A MemoryStrteam of the binary data (for read and the like)</returns>
        public MemoryStream ExtractTileset(ResourceLocator1 tilesetLocator)
        {
            return this.Archive.ExtractTileset(tilesetLocator);
        }

        /// <summary>Extracts the tileset and returns it in a memorystream</summary>
        /// <param name="tilesetLocator">Tileset locator to match upon</param>
        /// <param name="tilesetIndex">Tileset index to extract</param>
        public MemoryStream ExtractTileset(Int32 tilesetIndex)
        {
            return this.Archive.ExtractTileset(tilesetIndex);
        }

        /// <summary>Check whether the tileset index exists in the BIFF header</summary>
        /// <param name="tilesetLocator">Tileset locator to match upon</param>
        /// <returns>A Boolean existing whether the index is populated</returns>
        public Boolean TilesetExists(ResourceLocator1 tilesetLocator)
        {
            return this.Archive.TilesetExists(tilesetLocator);
        }

        /// <summary>Check whether the tileset index exists in the BIFF header</summary>
        /// <param name="tilesetIndex">Tileset index to evaluate</param>
        /// <returns>A Boolean existing whether the index is populated</returns>
        public Boolean TilesetExists(Int32 tilesetIndex)
        {
            return this.Archive.TilesetExists(tilesetIndex);
        }
        #endregion


        #region ToString(...) overrides
        /// <summary>This method will write the entire BIFF archive to a String builder and return it</summary>
        /// <returns>A string containing textual representations of internal variables, then the header, then entry and data objects</returns>
        public override String ToString()
        {
            return this.Archive.ToString();
        }

        /// <summary>This method will write the entire BIFF archive to a String builder and return it</summary>
        /// <param name="longDefinition">Boolean indicating whether to write longer descriptions of the file</param>
        /// <returns>A string containing textual representations of internal variables, then the header, then entry and data objects</returns>
        public String ToString(Boolean longDefinition)
        {
            return this.Archive.ToString(longDefinition);
        }
        #endregion
    }
}