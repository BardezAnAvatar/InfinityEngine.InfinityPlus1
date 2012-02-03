using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WalledEnvironmentDisplay.Components
{
    /// <summary>Represent the naturally joined Tilemap structure to Tileset indeces</summary>
    /// <remarks>The tilemap maps an overlay's tiles to tileset indeces.</remarks>
    public class TilesetMappingCollection
    {
        #region Fields
        /// <summary>Collection reference to tilemap data</summary>
        public List<Tilemap> Tilemaps { get; set; }

        /// <summary>Collection of tile indeces</summary>
        /// <remarks>Maps the tile map references to specific tiles in the referenced tile set</remarks>
        public List<Int16> TileSetIndeces { get; set; }
        #endregion
    }
}