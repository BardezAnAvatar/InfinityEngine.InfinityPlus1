using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Spell.Enums
{
    /// <summary>Enumerator for Spell casting graphics</summary>
    /// <remarks>
    ///     From IESDP: The 'spark' entries are related to sprklclr.2da (and probably opcode #41)
    ///     From IDP on version 2, and IWD2's SCEFFECT.2DA file, I am interpolating and retroactively applying its contents.
    ///     1-8 is fire
    ///     9-16 is glow
    ///     17-25 is sparks/fountain
    ///     26-34 is sparks/whirl
    /// </remarks>
    public enum SpellCastingGraphics : short /* Int16 */
    {
        None                    = 0x0000,
        [Description("Fire: Aqua")]
        FireAqua,
        [Description("Fire: Blue")]
        FireBlue,
        [Description("Fire: Gold")]
        FireGold,
        [Description("Fire: Green")]
        FireGreen,
        [Description("Fire: Magenta")]
        FireMagenta,
        [Description("Fire: Purple")]
        FirePurple,
        [Description("Fire: Red")]
        FireRed,
        [Description("Fire: White")]
        FireWhite,
        Necromancy,             //= 0x0009,
        Alteration,
        Enchantment,
        Abjuration,
        Illusion,
        Conjuration,
        Invocation,
        Divination,

        [Description("Sparkles Fountain: Aqua (17)")]
        SparklesFountainAqua,
        [Description("Sparkles Fountain: Black (18)")]
        SparklesFountainBlack,
        [Description("Sparkles Fountain: Blue (19)")]
        SparklesFountainBlue,
        [Description("Sparkles Fountain: Gold (20)")]
        SparklesFountainGold,
        [Description("Sparkles Fountain: Green (21)")]
        SparklesFountainGreen,
        [Description("Sparkles Fountain: Magenta (22)")]   //White -> Red, makes sense
        SparklesFountainMagenta,
        [Description("Sparkles Fountain: Purple (13)")]    //Red/Purple makes sense
        SparklesFountainPurple,
        [Description("Sparkles Fountain: Red (24)")]       //White/Red makes sense
        SparklesFountainRed,
        [Description("Sparkles Fountain: White (25)")]
        SparklesFountainWhite,

        [Description("Sparkles Swirl: Aqua (26)")]
        SparklesSwirlAqua,
        [Description("Sparkles Swirl: Black (27)")]
        SparklesSwirlBlack,
        [Description("Sparkles Swirl: Blue (28)")]
        SparklesSwirlBlue,
        [Description("Sparkles Swirl: Gold (29)")]
        SparklesSwirlGold,
        [Description("Sparkles Swirl: Green (30)")]
        SparklesSwirlGreen,
        [Description("Sparkles Swirl: Magenta (31)")]
        SparklesSwirlMagenta,
        [Description("Sparkles Swirl: Purple (32)")]
        SparklesSwirlPurple,
        [Description("Sparkles Swirl: Red (33)")]
        SparklesSwirlRed,
        [Description("Sparkles Swirl: White (34)")]
        SparklesSwirlWhite,
        [Description("Abjuration (Planesape)")]
        PSTAbjuration,
        [Description("Alteration (Planesape)")]
        PSTAlteration,
        [Description("Conjuration (Planesape)")]
        PSTConjuration,
        [Description("Enchantment (Planesape)")]
        PSTEnchantment,
        [Description("Divination (Planesape)")]
        PSTDivination,
        [Description("Illusion (Planesape)")]
        PSTIllusion,
        [Description("Invocation (Planesape)")]
        PSTInvocation,
        [Description("Necromancy (Planesape)")]
        PSTNecromancy,
        [Description("Special/Innate Skill (Planesape)")]
        PSTInnate               = 44,
        [Description("Unknown/Special Skill (Planesape)")]
        PSTSpecialAbility       = 45,
        [Description("No Animation (0xFFFF)")]
        NoAnimation1            = -1,
        [Description("No Animation (0xDC8D)")]
        NoAnimation2            = -29220
    }
}