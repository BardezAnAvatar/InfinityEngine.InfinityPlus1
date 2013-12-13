using System;
using System.ComponentModel;


namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item.Enums
{
    public enum WeaponProficiency : byte /* Byte */
    {
        None                        = 0x00,
        [Description("Bastard Sword")]
        BastardSword                = 0x59,
        [Description("Long Sword")]
        LongSword                   = 0x5A,
        [Description("Small Sword")]
        ShortSword                  = 0x5B,
        Axe                         = 0x5C,
        [Description("Great Sword")]
        TwoHandedSword              = 0x5D,
        Katana                      = 0x5E,
        [Description("Scimitar/Wakizashi/Ninja-to")]
        ScimitarWakizashiNinjaTo    = 0x5F,
        Dagger                      = 0x60,
        [Description("War Hammer")]
        WarHammer                   = 0x61,
        Spear                       = 0x62,
        Halberd                     = 0x63,
        [Description("Flail/Morning Star")]
        FlailMorningstar            = 0x64,
        Mace                        = 0x65,
        Quarterstaff                = 0x66,
        Crossbow                    = 0x67,
        [Description("Long Bow")]
        LongBow                     = 0x68,
        [Description("Short Bow")]
        ShortBow                    = 0x69,
        Darts                       = 0x6A,
        Sling                       = 0x6B,
        Blackjack                   = 0x6C,
        Gun                         = 0x6D,
        [Description("Martial Arts")]
        MartialArts                 = 0x6E,
        [Description("Two-Handed Weapon Skill")]
        TwoHandedWeaponSkill        = 0x6F,
        [Description("Sword and Shield Skill")]
        SwordAndShieldSkill         = 0x70,
        [Description("Single Weapon Skill")]
        SingleWeaponSkill           = 0x71,
        [Description("Two-Weapon Skill")]
        TwoWeaponSkill              = 0x72,
        Club                        = 0x73,
        [Description("Unused Proficiency #2")]
        ExtraProficiency2           = 0x74,
        [Description("Unused Proficiency #3")]
        ExtraProficiency3           = 0x75,
        [Description("Unused Proficiency #4")]
        ExtraProficiency4           = 0x76,
        [Description("Unused Proficiency #5")]
        ExtraProficiency5           = 0x77,
        [Description("Unused Proficiency #6")]
        ExtraProficiency6           = 0x78,
        [Description("Unused Proficiency #7")]
        ExtraProficiency7           = 0x79,
        [Description("Unused Proficiency #8")]
        ExtraProficiency8           = 0x7A,
        [Description("Unused Proficiency #9")]
        ExtraProficiency9           = 0x7B,
        [Description("Unused Proficiency #10")]
        ExtraProficiency10          = 0x7C,
        [Description("Unused Proficiency #11")]
        ExtraProficiency11          = 0x7D,
        [Description("Unused Proficiency #12")]
        ExtraProficiency12          = 0x7E,
        [Description("Unused Proficiency #13")]
        ExtraProficiency13          = 0x7F,
        [Description("Unused Proficiency #14")]
        ExtraProficiency14          = 0x80,
        [Description("Unused Proficiency #15")]
        ExtraProficiency15          = 0x81,
        [Description("Unused Proficiency #16")]
        ExtraProficiency16          = 0x82,
        [Description("Unused Proficiency #17")]
        ExtraProficiency17          = 0x83,
        [Description("Unused Proficiency #18")]
        ExtraProficiency18          = 0x84,
        [Description("Unused Proficiency #19")]
        ExtraProficiency19          = 0x85,
        [Description("Unused Proficiency #20")]
        ExtraProficiency20          = 0x86
    }
}
