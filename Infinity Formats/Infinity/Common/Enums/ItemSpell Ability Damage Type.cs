using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Common.Enums
{
    public enum ItemSpellAbilityDamageType : ushort /* UInt16 */
    {
        None                    = 0,
        [Description("Default (Piercing or Magic)")]
        PiercingOrMagic         = 1,
        Blunt                   = 2,
        Slashing                = 3,
        Ranged                  = 4,
        Fists                   = 5,
        PiercingBluntMore       = 6,
        PiercingSlashingMore    = 7,
        BluntSlashingLess       = 8,
        [Description("Blunt Missile")]
        BluntMissile            = 9
    }
}