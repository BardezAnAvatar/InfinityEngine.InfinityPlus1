using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Effect.Enums
{
    /// <remarks>
    ///     According to IESDP, if more than 1 saving throw is specified, the target
    ///     rolls against their best save.
    /// </remarks>
    [Flags]
    public enum EffectSavingThrow : uint /* UInt32 */
    {
        //2E / 3E = 2E Or 3E
        [Description("Spells (2E)")]
        SpellsOrUnused      = 1,

        [Description("Breath (2E)")]
        BreathOrUnused      = 2,

        [Description("Death (2E) / Fortitude(D20)")]
        DeathOrFortitude    = 4,    //Fortitude

        [Description("Wands (2E) / Reflex(D20)")]
        WandsOrReflex       = 8,    //Reflex

        [Description("Polymorph (2E) / Will(D20)")]
        PolymorphOrWill     = 16,   //Will
    }
}