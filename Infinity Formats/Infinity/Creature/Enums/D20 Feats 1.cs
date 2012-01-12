using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Enums
{
    /// <summary>First 64 bits of the feats bitfield</summary>
    [Flags]
    public enum D20Feats1 : ulong /* UInt64 */
    {
        [Description("Aegis of Rime")]
        AEGIS_OF_RIME               = 0x0000000000000001U,

        [Description("Ambidexterity")]
        AMBIDEXTERITY               = 0x0000000000000002U,

        [Description("Aqua Mortis")]
        AQUA_MORTIS                 = 0x0000000000000004U,

        /// <remarks>value 1 = light, 2 = medium, 3 = heavy.</remarks>
        [Description("Armor Proficiency")]
        ARMOR_PROF                  = 0x0000000000000008U,

        [Description("Armored Arcana")]
        ARMORED_ARCANA              = 0x0000000000000010U,

        [Description("Arterial Strike")]
        ARTERIAL_STRIKE             = 0x0000000000000020U,

        [Description("Blind Fight")]
        BLIND_FIGHT                 = 0x0000000000000040U,

        [Description("Bullheaded")]
        BULLHEADED                  = 0x0000000000000080U,

        [Description("Cleave")]
        CLEAVE                      = 0x0000000000000100U,

        [Description("Combat Casting")]
        COMBAT_CASTING              = 0x0000000000000200U,

        [Description("Conundrum")]
        CONUNDRUM                   = 0x0000000000000400U,

        [Description("Crippling Strike")]
        CRIPPLING_STRIKE            = 0x0000000000000800U,

        [Description("Dash")]
        DASH                        = 0x0000000000001000U,

        [Description("Deflect Arrows")]
        DEFLECT_ARROWS              = 0x0000000000002000U,

        [Description("Dirty Fighting")]
        DIRTY_FIGHTING              = 0x0000000000004000U,

        [Description("Discipline")]
        DISCIPLINE                  = 0x0000000000008000U,

        [Description("Dodge")]
        DODGE                       = 0x0000000000010000U,

        [Description("Envenom Weapon")]
        ENVENOM_WEAPON              = 0x0000000000020000U,

        [Description("Exotic Weapon Proficiency: Bastard Sword")]
        EXOTIC_BASTARD              = 0x0000000000040000U,

        [Description("Expertise")]
        EXPERTISE                   = 0x0000000000080000U,

        [Description("Extra Rage")]
        EXTRA_RAGE                  = 0x0000000000100000U,
        
        [Description("Conundrum")]
        EXTRA_SHAPESHIFTING         = 0x0000000000200000U,

        [Description("Extra Smiting")]
        EXTRA_SMITING               = 0x0000000000400000U,

        [Description("Extra Turning")]
        EXTRA_TURNING               = 0x0000000000800000U,

        [Description("Fiendslayer")]
        FIENDSLAYER                 = 0x0000000001000000U,

        [Description("Forester")]
        FORESTER                    = 0x0000000002000000U,

        [Description("Great Fortitude")]
        GREAT_FORTITUDE             = 0x0000000004000000U,

        [Description("Hamstring")]
        HAMSTRING                   = 0x0000000008000000U,

        [Description("Heretics' Bane")]
        HERETICS_BANE               = 0x0000000010000000U,

        [Description("Heroic Inspiration")]
        HEROIC_INSPIRATION          = 0x0000000020000000U,

        [Description("Improved Critical")]
        IMPROVED_CRITICAL           = 0x0000000040000000U,

        [Description("Improved Evasion")]
        IMPROVED_EVASION            = 0x0000000080000000U,

        [Description("Improved Initiative")]
        IMPROVED_INITIATIVE         = 0x0000000100000000U,

        [Description("Improved Turning")]
        IMPROVED_TURNING            = 0x0000000200000000U,

        [Description("Iron Will")]
        IRON_WILL                   = 0x0000000400000000U,

        [Description("Lightning Reflexes")]
        LIGHTNING_REFLEXES          = 0x0000000800000000U,

        [Description("Lingering Song")]
        LINGERING_SONG              = 0x0000001000000000U,

        [Description("Luck of Heroes")]
        LUCK_OF_HEROES              = 0x0000002000000000U,

        [Description("Martial Weapon Proficiency: Axe")]
        MARTIAL_AXE                 = 0x0000004000000000U,

        [Description("Martial Weapon Proficiency: Bow")]
        MARTIAL_BOW                 = 0x0000008000000000U,

        [Description("Martial Weapon Proficiency: Flail")]
        MARTIAL_FLAIL               = 0x0000010000000000U,

        [Description("Martial Weapon Proficiency: Greatsword")]
        MARTIAL_GREATSWORD          = 0x0000020000000000U,

        [Description("Martial Weapon Proficiency: Hammer")]
        MARTIAL_HAMMER              = 0x0000040000000000U,

        [Description("Martial Weapon Proficiency: Large Sword")]
        MARTIAL_LARGESWORD          = 0x0000080000000000U,

        [Description("Martial Weapon Proficiency: Polearm")]
        MARTIAL_POLEARM             = 0x0000100000000000U,

        [Description("Maximized Attacks")]
        MAXIMIZED_ATTACKS           = 0x0000200000000000U,

        [Description("Mercantile Background")]
        MERCANTILE_BACKGROUND       = 0x0000400000000000U,

        [Description("Power Attack")]
        POWER_ATTACK                = 0x0000800000000000U,

        [Description("Precise Shot")]
        PRECISE_SHOT                = 0x0001000000000000U,

        [Description("Rapid Shot")]
        RAPID_SHOT                  = 0x0002000000000000U,

        [Description("Resist Poison")]
        RESIST_POISON               = 0x0004000000000000U,

        [Description("Scion of Storms")]
        SCION_OF_STORMS             = 0x0008000000000000U,

        [Description("Shield Proficiency")]
        SHIELD_PROF                 = 0x0010000000000000U,

        [Description("Martial Weapon Proficiency: Crosbow")]
        SIMPLE_CROSSBOW             = 0x0020000000000000U,

        [Description("Martial Weapon Proficiency: Mace")]
        SIMPLE_MACE                 = 0x0040000000000000U,

        [Description("Martial Weapon Proficiency: Missile")]
        SIMPLE_MISSILE              = 0x0080000000000000U,

        [Description("Martial Weapon Proficiency: Quarterstaff")]
        SIMPLE_QUARTERSTAFF         = 0x0100000000000000U,

        [Description("Martial Weapon Proficiency: Small Blade")]
        SIMPLE_SMALLBLADE           = 0x0200000000000000U,
        
        [Description("Slippery Mind")]
        SLIPPERY_MIND               = 0x0400000000000000U,

        [Description("Snake Blood")]
        SNAKE_BLOOD                 = 0x0800000000000000U,

        [Description("Spell Focus: Enchantment")]
        SPELL_FOCUS_ENCHANTMENT     = 0x1000000000000000U,

        [Description("Spell Focus: Evocation")]
        SPELL_FOCUS_EVOCATION       = 0x2000000000000000U,

        [Description("Spell Focus: Necromancy")]
        SPELL_FOCUS_NECROMANCY      = 0x4000000000000000U,

        [Description("Spell Focus: Transmute")]
        SPELL_FOCUS_TRANSMUTE       = 0x8000000000000000U
    }
}
