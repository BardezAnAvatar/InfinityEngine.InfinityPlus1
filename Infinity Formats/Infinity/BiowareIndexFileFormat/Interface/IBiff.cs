using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareIndexFileFormat.Components;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareIndexFileFormat.Interface
{
    /// <summary>Represents common archive actions for a Biff archive</summary>
    public interface IBiff : IInfinityFormat
    {
        #region Extraction
        /// <summary>Opens a file and extracts the resource1 to that file path</summary>
        /// <param name="resourcePath">File path to extract to. Must include file extension</param>
        /// <param name="resourceLocator">Resource locator to match upon</param>
        void ExtractResource(String resourcePath, ResourceLocator1 resourceLocator);

        /// <summary>Opens a file and extracts the tileset to that file path</summary>
        /// <param name="tilesetPath">File path to extract to. Must include file extension</param>
        /// <param name="tilesetLocator">Tileset locator to match upon</param>
        void ExtractTileset(String tilesetPath, ResourceLocator1 tilesetLocator);

        /// <summary>Opens a file and extracts the resource1 to that file path</summary>
        /// <param name="resourcePath">File path to extract to. Must include file extension</param>
        /// <param name="resourceIndex">Resource index to write</param>
        void ExtractResource(String resourcePath, Int32 resourceIndex);

        /// <summary>Opens a file and extracts the tileset to that file path</summary>
        /// <param name="tilesetPath">File path to extract to. Must include file extension</param>
        /// <param name="tilesetIndex">Tileset index to write</param>
        void ExtractTileset(String tilesetPath, Int32 tilesetIndex);

        /// <summary>Extracts the resource1 to the output stream</summary>
        /// <param name="output">Output stream to write to. Not closed during method.</param>
        /// <param name="resourceLocator">Resource locator to match upon</param>
        void ExtractResource(Stream output, ResourceLocator1 resourceLocator);

        /// <summary>Extracts the tileset to the output stream</summary>
        /// <param name="output">Output stream to write to. Not closed during method.</param>
        /// <param name="tilesetLocator">Tileset locator to match upon</param>
        void ExtractTileset(Stream output, ResourceLocator1 tilesetLocator);

        /// <summary>Extracts the resource1 to the output stream</summary>
        /// <param name="output">Output stream to write to. Not closed during method.</param>
        /// <param name="resourceIndex">Resource index to write</param>
        void ExtractResource(Stream output, Int32 resourceIndex);

        /// <summary>Extracts the tileset to the output stream</summary>
        /// <param name="output">Output stream to write to. Not closed during method.</param>
        /// <param name="tilesetIndex">Tileset index to write</param>
        void ExtractTileset(Stream output, Int32 tilesetIndex);

        /// <summary>Extracts the resource1 and returns it in a memorystream</summary>
        /// <param name="resourceLocator">Resource locator to match upon</param>
        /// <returns>A MemoryStrteam of the binary data (for read and the like)</returns>
        MemoryStream ExtractResource(ResourceLocator1 resourceLocator);

        /// <summary>Extracts the tileset and returns it in a memorystream</summary>
        /// <param name="tilesetLocator">Tileset locator to match upon</param>
        /// <returns>A MemoryStrteam of the binary data (for read and the like)</returns>
        MemoryStream ExtractTileset(ResourceLocator1 tilesetLocator);

        /// <summary>Extracts the resource1 and returns it in a memorystream</summary>
        /// <param name="resourceLocator">Resource locator to match upon</param>
        /// <param name="resourceIndex">Resource index to extract</param>
        MemoryStream ExtractResource(Int32 resourceIndex);

        /// <summary>Extracts the tileset and returns it in a memorystream</summary>
        /// <param name="tilesetLocator">Tileset locator to match upon</param>
        /// <param name="tilesetIndex">Tileset index to extract</param>
        MemoryStream ExtractTileset(Int32 tilesetIndex);
        #endregion


        #region Existance
        /// <summary>Check whether the resource1 index exists in the BIFF header</summary>
        /// <param name="resourceLocator">Resource locator to match upon</param>
        /// <returns>A Boolean existing whether the index is populated</returns>
        Boolean ResourceExists(ResourceLocator1 resourceLocator);

        /// <summary>Check whether the resource1 index exists in the BIFF header</summary>
        /// <param name="resourceIndex">Resource index to evaluate</param>
        /// <returns>A Boolean existing whether the index is populated</returns>
        Boolean ResourceExists(Int32 resourceIndex);

        /// <summary>Check whether the tileset index exists in the BIFF header</summary>
        /// <param name="tilesetLocator">Tileset locator to match upon</param>
        /// <returns>A Boolean existing whether the index is populated</returns>
        Boolean TilesetExists(ResourceLocator1 tilesetLocator);

        /// <summary>Check whether the tileset index exists in the BIFF header</summary>
        /// <param name="tilesetIndex">Tileset index to evaluate</param>
        /// <returns>A Boolean existing whether the index is populated</returns>
        Boolean TilesetExists(Int32 tilesetIndex);
        #endregion
    }
}