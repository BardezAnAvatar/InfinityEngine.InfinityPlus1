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
    ///     Bit 	Byte 1 	                Byte 2 	                    Byte 3 	                        Byte 4
    ///     0 	    Cleric of Talos 	    Stalker Ranger 	            Diviner 	                    Beserker Fighter
    ///     1 	    Cleric of Helm 	        Beastmaster Ranger 	        Enchanter 	                    Wizardslayer Fighter
    ///     2 	    Cleric of Lathlander 	Assassin Thief 	            Illusionist 	                Kensai Fighter
    ///     3 	    Totemic Druid 	        Bounty Hunter Thief 	    Invoker 	                    Cavalier Paladin
    ///     4 	    Shapeshifter Druid 	    Swashbuckler Thief 	        Necromancer 	                Inquisiter Paladin
    ///     5 	    Avenger Druid 	        Blade Bard 	                Transmuter 	                    Undead Hunter Paladin
    ///     6 	    Barbarian 	            Jester Bard 	            All (no kit) 	                Abjurer
    ///     7 	    Wildmage 	            Skald Bard 	                Ferlain (sic) [Archer Ranger] 	Conjurer
    /// </summary>
    [Flags]
    public enum KitProhibitions1 : byte /* Byte */
    {
        [Description("Cleric of Talos")]
        ClericTalos         = 0x01,
        [Description("Cleric of Helm")]
        ClericHelm          = 0x02,
        [Description("Cleric of Lathlander")]
        ClericLathlander    = 0x04,
        [Description("Druid (Totemic)")]
        DruidTotemic        = 0x08,
        [Description("Druid (Shapeshifter)")]
        DruidShapeshifter   = 0x10,
        [Description("Druid (Avenger)")]
        DruidAvenger        = 0x20,
        [Description("Class: Barbarian")]
        ClassBarbarian      = 0x40,
        [Description("Mage (Wild)")]
        MageWild            = 0x80,
    }
}