using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Flags for the edge of an area</summary>
    [Flags]
    public enum AreaEdgeFlags : uint /* UInt32 */
    {
        PartyRequired   = 1U,
        PartyEnabled    = 2U,
    }
}