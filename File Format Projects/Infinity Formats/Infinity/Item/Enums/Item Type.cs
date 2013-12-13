using System;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item.Enums
{
    public enum ItemType : short /* Int16 */
    {
        Miscellaneous        = 0x0000,

        [Description("Amulet or Necklace")]
        AmuletNecklace,
        Armor,
        Belt,
        Boots,
        Arrows,
        Bracers,
        Headgear,
        Keys,
        Potion,
        Ring,
        Scroll,
        Shield,

        //13
        /// <summary>Before IWD2</summary>
        [Description("Food before IWD2")]
        FoodStuffs,
        Bullets,
        Bow,
        Dagger,

        /// <summary>Includes clubs in BG1</summary>
        Mace,

        Sling,

        /// <summary>Short swords</summary>
        [Description("Small Sword")]
        SmallSword,

        /// <summary>Two-handed and bastard in BG2</summary>
        [Description("Long Sword")]
        LongSword,

        Hammer,

        [Description("Morning Star")]
        MorningStar,
        Flail,
        Darts,
        Axe,
        Quarterstaff,
        Crossbow,

        /// <summary>Fist, fist irons, punch daggers, etc.</summary>
        [Description("Hand-to-hand Weapon")]
        HandToHand,

        Spear,

        /// <summary>Two handed axe as well</summary>
        Halberd,

        Bolts,
        [Description("Cloak or Robe")]
        CloakRobe,
        Gold,
        Gem,
        Wand,

        /// <summary>Containers, also the eye value, and broken armor</summary>
        [Description("Container, Broken Armor, or Eye")]
        ContainerEyeBrokenArmor,

        /// <summary>A book, bracelet (PS:T) or broken shield</summary>
        [Description("Book, Bracelet, or Broken Shield")]
        BookBraceletBrokenShield,

        /// <summary>A familiar, earring (PS:T) or broken sword</summary>
        [Description("Familiar, Earring, or Broken Sword")]
        FamiliarEarringBrokenSword,

        Tatoo,
        Lens,

        /// <summary>Buckler or teeth (PS:T)</summary>
        [Description("Buckler, or Teeth")]
        BuckerTeeth,

        Candle,
        [Description("Body (Child)")]
        BodyChild,
        Club,
        [Description("Body (Female)")]
        BodyFemale,

        /// <summary>IWD code for keys</summary>
        Key,

        [Description("Large Shield")]
        LargeShield,
        [Description("Body (Male)")]
        BodyMale,
        [Description("Medium Shield")]
        MediumShield,
        Notes,
        Rod,
        Skull,
        [Description("Small Shield")]
        SmallShield,
        [Description("Body (Spider)")]
        BodySpider,
        Telescope,
        Drink,
        [Description("Great Sword")]
        GreatSword,
        Container,
        [Description("Pelt / Fur")]
        PeltFur,
        [Description("Armor (Leather)")]
        LeatherArmor,
        [Description("Armor (Studded Leather)")]
        StuddedLeatherArmor,
        [Description("Armor (Chain Mail)")]
        ChainMail,
        [Description("Armor (Splint Mail)")]
        SplintMail,
        [Description("Armor (Half Plate)")]
        HalfPlateMail,
        [Description("Armor (Full Plate)")]
        FullPlateMail,
        [Description("Armor (Hide)")]
        HideArmor,
        Robe,
        [Description("Armor (Scale Mail)")]
        ScaleMail,
        [Description("Bastard Sword")]
        BastardSword,
        Scarf,

        /// <summary>Only IWD2</summary>
        [Description("Food (IWD2)")]
        Food,
        Hat,
        Gauntlet
    }
}