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
    ///     Bit 	Byte 1 	                Byte 2 	                    Byte 3 	                        Byte 4
    ///     0 	    Cleric of Talos 	    Stalker Ranger 	            Diviner 	                    Beserker Fighter
    ///     1 	    Cleric of Helm 	        Beastmaster Ranger 	        Enchanter 	                    Wizardslayer Fighter
    ///     2 	    Cleric of Lathlander 	Assassin Thief 	            Illusionist 	                Kensai Fighter
    ///     3 	    Totemic Druid 	        Bounty Hunter Thief 	    Invoker 	                    Cavalier Paladin
    ///     4 	    Shapeshifter Druid 	    Swashbuckler Thief 	        Necromancer 	                Inquisiter Paladin
    ///     5 	    Avenger Druid 	        Blade Bard 	                Transmuter 	                    Undead Hunter Paladin
    ///     6 	    Barbarian 	            Jester Bard 	            All (no kit) 	                Abjurer
    ///     7 	    Wildmage 	            Skald Bard 	                Ferlain (sic) Archer Ranger 	Conjurer
    /// </summary>
    [Flags]
    public enum KitProhibitions3 : byte /* Byte */
    {
        [Description("Mage (Diviner)")]
        MageDiviner             = 0x01,
        [Description("Mage (Enchanter)")]
        MageEnchanter           = 0x02,
        [Description("Mage (Illusionist)")]
        MageIllusionist         = 0x04,
        [Description("Mage (Invoker)")]
        MageInvoker             = 0x08,
        [Description("Mage (Necromancer)")]
        MageNecromancer         = 0x10,
        [Description("Mage (Transmuter)")]
        MageTransmuter          = 0x20,
        [Description("All (No Kit)")]
        AllNoKit                = 0x40,
        [Description("Ranger (Archer)")]
        RangerArcher            = 0x80,
    }
}