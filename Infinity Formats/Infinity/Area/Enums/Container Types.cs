using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Container type</summary>
    public enum ContainerType : ushort /* UInt16 */
    {
        None        = 0,
        Bag         = 1,
        Chest       = 2,
        Drawer      = 3,
        Pile        = 4,
        Table       = 5,
        Shelf       = 6,
        Altar       = 7,
        Invisible   = 8,
        Spellbook   = 9,
        Body        = 10,
        Barrel      = 11,
        Crate       = 12,
    }
}