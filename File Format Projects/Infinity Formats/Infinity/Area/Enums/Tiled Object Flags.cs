using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Flags associated with a tiled object</summary>
    [Flags]
    public enum TiledObjectFlags : uint /* UInt32 */
    {
        /// <summary>Currently in secondary state</summary>
        [Description("Currently in secondary state")]
        SecondaryState  = 1U,

        /// <remarks>BG2 source calls this "Gate"</remarks>
        Transparent     = 2U,
    }
}