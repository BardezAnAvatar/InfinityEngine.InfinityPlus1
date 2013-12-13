using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Enums
{
    /// <summary>Represents additional special IWD2 flags</summary>
    /// <remarks>Discovered by Avenger</remarks>
    [Flags]
    public enum SpecialFlags3E : byte /* Byte */
    {
        /// <summary>Automatically succeed concentration rolls</summary>
        [Description("Automatically succeed concentration rolls")]
        ConcentrationSuccess = 1,

        /// <summary>Critical hit damage is not applied</summary>
        [Description("Critical hit damage is not applied")]
        ImmunteToCriticalHits = 2,

        /// <summary>Cannot take levels in Paladin, if you chose a confliciting class</summary>
        [Description("Cannot take levels in Paladin, if you chose a confliciting class")]
        CannotTakeLevelsInPaladin = 4,

        /// <summary>Cannot take levels in Monk, if you chose a confliciting class</summary>
        [Description("Cannot take levels in Monk, if you chose a confliciting class")]
        CannotTakeLevelsInMonk = 8,
    }
}