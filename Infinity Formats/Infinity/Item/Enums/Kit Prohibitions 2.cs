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
    public enum KitProhibitions2 : byte /* Byte */
    {
        [Description("Ranger (Stalker)")]
        RangerStalker       = 0x01,
        [Description("Ranger (Beastmaste)")]
        RangerBeastmaster   = 0x02,
        [Description("Thief (Assassin)")]
        ThiefAssassin       = 0x04,
        [Description("Thief (Bounty Hunter)")]
        ThiefBountyHunter   = 0x08,
        [Description("Thief (Swashbuckler)")]
        ThiefSwashbuckler   = 0x10,
        [Description("Bard (Blade)")]
        BardBlade           = 0x20,
        [Description("Bard (Jester)")]
        BardJester          = 0x40,
        [Description("Bard (Skald)")]
        BardSkald           = 0x80,
    }
}