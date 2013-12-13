using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell.Enums
{
    [Flags]
    public enum SpellHeader2Flags : uint /* UInt32 */
    {
        //There are a buttload of unknown flags here.

        [Description("Offensive Spell/Breaks Invisibility")]
        OffensiveSpell      = 0x000400,

        [Description("No Line of Sight Required")]
        NoLineOfSight       = 0x000800,

        [Description("Outdoors only")]
        OutdoorsOnly        = 0x002000,

        [Description("Simplified duration")]
        SimplifiedDuration  = 0x004000,

        [Description("Trigger / Contingency")]
        TriggerContingency  = 0x008000,

        [Description("Out of combat only")]
        OutOfCombatOnly     = 0x010000
    }
}