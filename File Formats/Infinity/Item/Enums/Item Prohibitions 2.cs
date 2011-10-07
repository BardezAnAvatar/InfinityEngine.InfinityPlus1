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
    ///     Bit 	Byte 1 	    Byte 2 	        Byte 3 	    Byte 4
    ///     0 	    Barbarian 	Rogue 	        Lawful 	    Unknown
    ///     1 	    Bard 	    Sorcerer 	    Neutral 	Unknown
    ///     2 	    Cleric 	    Wizard 	        Unknown 	Unknown
    ///     3 	    Druid 	    Unknown 	    Unknown 	Unknown
    ///     4 	    Fighter 	Chaotic 	    Unknown 	Unknown
    ///     5 	    Monk 	    Evil 	        Unknown 	Unknown
    ///     6 	    Paladin 	Good 	        Unknown 	Unknown
    ///     7 	    Ranger 	    ... Neutral     Unknown 	Unknown
    /// </summary>
    [Flags]
    public enum ItemProhibitions2 : uint /* UInt32 */
    {
        [Description("Class: Barbarian")]
        ClassBarbarian          = 0x00000001U,
        [Description("Class: Bard")]
        ClassBard               = 0x00000002U,
        [Description("Class: Cleric")]
        ClassCleric             = 0x00000004U,
        [Description("Class: Druid")]
        ClassDruid              = 0x00000008U,
        [Description("Class: Fighter")]
        ClassFighter            = 0x00000010U,
        [Description("Class: Monk")]
        ClassMonk               = 0x00000020U,
        [Description("Class: Paladin")]
        ClassPaladin            = 0x00000040U,
        [Description("Class: Ranger")]
        ClassRanger             = 0x00000080U,
        [Description("Class: Rogue")]
        ClassRogue              = 0x00000100U,
        [Description("Class: Sorcerer")]
        ClassSorcerer           = 0x00000200U,
        [Description("Class: Wizard")]
        ClassWizard             = 0x00000400U,
        [Description("Unknown (Placeholder/separator?)")]
        Unknown1                = 0x00000800U,
        [Description("Chaotic...")]
        OrderChaotic            = 0x00001000U,
        [Description("...Evil")]
        NatureEvil              = 0x00002000U,
        [Description("...Good")]
        NatureGood              = 0x00004000U,
        [Description("...Neutral")]
        NatureNeutral           = 0x00008000U,
        [Description("Lawful...")]
        OrderLawful             = 0x00010000U,
        [Description("Neutral...")]
        OrderNeutral            = 0x00020000U
    }
}