using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums
{
    [Flags]
    public enum ItemInstanceFlags : uint /* UInt32 */
    {
        Identified          = 1U,
        Unstealable         = 2U,
        Stolen              = 4U,

        [Description("Lore Requirement (IWD2) or Undroppable (BG, etc.)")]
        UndroppableOrLoreRequirement  = 8U
    }
}