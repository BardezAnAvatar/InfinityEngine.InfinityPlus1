using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Effect.Enums
{
    [Flags]
    public enum EffectResistance : byte /* Byte */
    {
        /// <summary>This is magical</summary>
        Dispellable         = 1,

        /// <summary>Affected by resistance</summary>
        [Description("Ignores Resistance")]
        IgnoreResistance    = 2,
    }
}
