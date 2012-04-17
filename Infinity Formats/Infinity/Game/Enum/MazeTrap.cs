using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Enum
{
    /// <summary>Type of trap </summary>
    [Flags]
    public enum MazeTrap : uint /* UInt32 */
    {
        TrapA = 0U,
        TrapB = 1U,
        TrapC = 2U,
    }
}