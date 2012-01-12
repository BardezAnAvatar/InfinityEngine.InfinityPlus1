using System;
using System.ComponentModel;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item.Enums
{
    /// <summary>
    ///     This enum is a flag bitfield for item usability:
    ///     
    ///     From IESDB:
    ///     Bit 	Byte 1 	                            Byte 2 	                    Byte 3 	                        Byte 4
    ///     0 	    Unusable by Chatoic 	            Unusable by Godsmen 	    Unusable by Indeps 	            Unusable by Ignus
    ///     1 	    Unusable by Evil 	                Unusable by Anarchist 	    Unusable by Fighter Thief 	    Unusable by Morte
    ///     2 	    Unusable by Good 	                Unusable by Chaosmen 	    Unusable by Mage 	            Unusable by Nordom
    ///     3 	    Unusable by Good-Evil Neutral 	    Unusable by Fighter 	    Unusable by Mage Thief 	        Unknown
    ///     4 	    Unusable by Lawful 	                No Faction  	            Unusable by Dak'kon 	        Unusable by Annah
    ///     5 	    Unusable by Lawful-Chaotic Neutral 	Unusable by Fighter Mage 	Unusable by Fall-From-Grace 	Unknown
    ///     6 	    Unusable by Sensates 	            Unusable by Dustmen 	    Unusable by Thief 	            Unusable by Nameless One
    ///     7 	    Unusable by Priest 	                Unusable by Mercykillers 	Unusable by Vhailor 	        Unknown
    /// </summary>
    [Flags]
    public enum ItemProhibitions1_1 : uint /* UInt32 */
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
        [Description("Faction: Sensate")]
        FactionSensate          = 0x00000040U,
        [Description("Class: Cleric")]
        ClassCleric             = 0x00000080U,
        [Description("Faction: Godsmen")]
        FactionGodsmen          = 0x00000100U,
        [Description("Faction: Anarchist")]
        FactionAnarchist        = 0x00000200U,
        [Description("Faction: Chaosmen")]
        FactionChaosmen         = 0x00000400U,
        [Description("Class: Fighter")]
        ClassFighter            = 0x00000800U,
        [Description("Faction: None")]
        FactionNone             = 0x00001000U,
        [Description("Class: Fighter/Mage")]
        ClassFighterMage        = 0x00002000U,
        [Description("Faction: Dustmen")]
        FactionDustmen          = 0x00004000U,
        [Description("Faction: Mercykillers")]
        FactionMercyKillers     = 0x00008000U,
        [Description("Faction: Independants")]
        FactionIndependants     = 0x00010000U,
        [Description("Class: Fighter/Thief")]
        ClassFighterThief       = 0x00020000U,
        [Description("Class: Mage")]
        ClassMage               = 0x00040000U,
        [Description("Class: Mage/Thief")]
        ClassMageThief          = 0x00080000U,
        [Description("Character: Dak'kon")]
        CharacterDakkon         = 0x00100000U,
        [Description("Character: Fall-From-Grace")]
        CharacterFallFromGrace  = 0x00200000U,
        [Description("Class: Thief")]
        ClassThief = 0x00400000U,
        [Description("Character: Vhailor")]
        CharacterVhailor        = 0x00800000U,
        [Description("Character: Ignus")]
        CharacterIgnus          = 0x01000000U,
        [Description("Character: Morte")]
        CharacterMorte          = 0x02000000U,
        [Description("Character: Nordom")]
        CharacterNordom         = 0x04000000U,

        /// <summary>Infinity Explorer suggests "humans", but does not indicate which unknown</summary>
        [Description("Unknown #1")]
        Unknown1                = 0x08000000U,
        [Description("Character: Annah")]
        CharacterAnnah          = 0x10000000U,

        [Description("Unknown #2")]
        Unknown2                = 0x20000000U,
        [Description("Character: Nameless One")]
        CharacterNamelessOne    = 0x40000000U,
        [Description("Unknown #3")]
        Unknown3                = 0x80000000U
    }
}