using System;
using System.Collections.Generic;

using Bardez.Projects.InfinityPlus1.Utility;

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


        #region Equality
        /// <summary>Overridden (value) equality method</summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>Boolean indicating equality</returns>
        public override Boolean Equals(Object obj)
        {
            Boolean equal = false;  //assume the worst

            try
            {
                if (obj != null && obj is TilesetMappingCollection)
                {
                    TilesetMappingCollection compare = obj as TilesetMappingCollection;
                    equal = this.Tilemaps.Equals<Tilemap>(compare.Tilemaps);
                    
                    if (equal)
                        equal = this.TileSetIndeces.Equals<Int16>(compare.TileSetIndeces);
                }
            }
            catch { equal = false; }    //per MSDN, must not throw exceptions

            return equal;
        }
        #endregion
    }
}