using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums
{
    /// <summary>Behavior associated with an ability's charge depletion</summary>
    public enum ItemSpellAbilityDepletionBehavior : ushort /* UInt16 */
    {
        None            = 0,
        Expended        = 1,
        [Description("Expended (Does not play sound)")]
        ExpendedNoSound = 2,
        [Description("Recharges daily")]
        DailyCharges    = 3
    }
}