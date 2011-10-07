using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Enums
{
    [Flags]
    public enum ItemInstanceFlags : uint /* UInt32 */
    {
        Identified          = 1U,
        Unstealable         = 2U,
        Stolen              = 4U,

        [Description("Magical (IWD) or Undroppable (BG)")]
        MagicalUndroppable  = 8U
    }
}