using System;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item.Enums
{
    /// <summary>
    ///     This enum is a flag bitfield for item usability:
    /// </summary>

    public enum ItemAbilityProjectileType : ushort /* UInt16 */
    {
        None    = 0x00,
        Arrow   = 0x01,
        Bolt    = 0x02,
        Bullet  = 0x03
    }
}