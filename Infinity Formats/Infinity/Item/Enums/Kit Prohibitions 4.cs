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
    public enum KitProhibitions4 : byte /* Byte */
    {
        [Description("Fighter (Berserker)")]
        FighterBeserker         = 0x01,
        [Description("Fighter (Wizardslayer)")]
        FighterWizardslayer     = 0x02,
        [Description("Fighter (Kensai)")]
        FighterKensai           = 0x04,
        [Description("Paladin (Cavalier)")]
        PaladinCavalier         = 0x08,
        [Description("Fighter (Inquisiter)")]
        PaladnInquisiter        = 0x10,
        [Description("Fighter (Undead Hunter)")]
        PaladinUndeadHunter     = 0x20,
        [Description("Mage (Abjurer)")]
        MageAbjurer             = 0x40,
        [Description("Mage (Conjurer)")]
        MageConjurer            = 0x80,
    }
}