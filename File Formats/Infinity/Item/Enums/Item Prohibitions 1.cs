using System;
using System.ComponentModel;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Item.Enums
{
    /// <summary>
    ///     This enum is a flag bitfield for item usability:
    ///     
    ///     From IESDB:
    ///     Bit 	Byte 1 	    Byte 2 	                Byte 3 	            Byte 4
    ///     0 	    Chaotic... 	Cleric/Mage 	        Fighter/Mage/Thief 	Dwarf
    ///     1 	    ...Evil 	Cleric/Thief 	        Fighter/Thief 	    Half-Elf
    ///     2 	    ...Good 	Cleric/Ranger 	        Mage 	            Halfling
    ///     3 	    ...Neutral 	Fighter 	            Mage/Thief 	        Human
    ///     4 	    Lawful... 	Fighter/Druid 	        Paladin 	        Gnome
    ///     5 	    Neutral... 	Fighter/Mage 	        Ranger 	            Monk
    ///     6 	    Bard 	    Fighter/Cleric 	        Thief 	            Druid
    ///     7 	    Cleric 	    Fighter/Mage/Cleric     Elf 	            Half-Orc
    /// </summary>
    [Flags]
    public enum ItemProhibitions1 : uint /* UInt32 */
    {
        [Description("Chaotic...")]
        OrderChaotic            = 0x00000001U,
        [Description("...Evil")]
        NatureEvil              = 0x00000002U,
        [Description("...Good")]
        NatureGood              = 0x00000004U,
        [Description("...Neutral")]
        NatureNeutral           = 0x00000008U,
        [Description("Lawful...")]
        OrderLawful             = 0x00000010U,
        [Description("Neutral...")]
        OrderNeutral            = 0x00000020U,
        [Description("Class: Bard")]
        ClassBard               = 0x00000040U,
        [Description("Class: Cleric")]
        ClassCleric             = 0x00000080U,
        [Description("Class: Cleric/Mage")]
        ClassClericMage         = 0x00000100U,
        [Description("Class: Cleric/Thief")]
        ClassClericThief        = 0x00000200U,
        [Description("Class: Cleric/Ranger")]
        ClassClericRanger       = 0x00000400U,
        [Description("Class: Fighter")]
        ClassFighter            = 0x00000800U,
        [Description("Class: Fighter/Druid")]
        ClassFighterDruid       = 0x00001000U,
        [Description("Class: Fighter/Mage")]
        ClassFighterMage        = 0x00002000U,
        [Description("Class: Fighter/Cleric")]
        ClassFighterCleric      = 0x00004000U,
        [Description("Class: Fighter/Mage/Cleric")]
        ClassFighterMageCleric  = 0x00008000U,
        [Description("Class: Fighter/Mage/Thief")]
        ClassFighterMageThief   = 0x00010000U,
        [Description("Class: Fighter/Thief")]
        ClassFighterThief       = 0x00020000U,
        [Description("Class: Mage")]
        ClassMage               = 0x00040000U,
        [Description("Class: Mage/Thief")]
        ClassMageThief          = 0x00080000U,
        [Description("Class: Paladin")]
        ClassPaladin            = 0x00100000U,
        [Description("Class: Ranger")]
        ClassRanger             = 0x00200000U,
        [Description("Class: Thief")]
        ClassThief              = 0x00400000U,
        [Description("Race: Elf")]
        RaceElf                 = 0x00800000U,
        [Description("Race: Dwarf")]
        RaceDwarf               = 0x01000000U,
        [Description("Race: Half-Elf")]
        RaceHalfElf             = 0x02000000U,
        [Description("Race: Halfling")]
        RaceHalfling            = 0x04000000U,
        [Description("Race: Human")]
        RaceHuman               = 0x08000000U,
        [Description("Race: Gnome")]
        RaceGnome               = 0x10000000U,
        [Description("Class: Monk")]
        ClassMonk               = 0x20000000U,
        [Description("Class: Druid")]
        ClassDruid              = 0x40000000U,
        [Description("Race: Half-Orc")]
        RaceHalfOrc             = 0x80000000U
    }
}