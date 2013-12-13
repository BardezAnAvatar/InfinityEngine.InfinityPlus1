using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Enums
{
    /// <summary>Valid Kit2e values</summary>
    public enum Kit2e : uint /* UInt32 */
    {
        None                	= 0x00000000U,

        [Description("Fighter: Barbarian")]
        FighterBarbarian    	= 0x00004000U,

        [Description("Mage: Abjurer")]
        MageAbjurer         	= 0x00400000U,

        [Description("Mage: Conjurer")]
        MageConjurer        	= 0x00800000U,

        [Description("Mage: Diviner")]
        MageDiviner         	= 0x01000000U,

        [Description("Mage: Enchanter")]
        MageEnchanter       	= 0x02000000U,

        [Description("Mage: Illuionist")]
        MageIlluionist      	= 0x04000000U,

        [Description("Mage: Invoker")]
        MageInvoker         	= 0x08000000U,

        [Description("Mage: Necromancer")]
        MageNecromancer     	= 0x10000000U,

        [Description("Mage: Transmuter")]
        MageTransmuter      	= 0x20000000U,

        [Description("True Class")]
        TrueClass           	= 0x40000000U,

        [Description("Fighter: Berserker")]
        FighterBerserker    	= 0x40010000U,

        [Description("Fighter: Wizardslayer")]
        FighterWizardslayer 	= 0x40020000U,

        [Description("Fighter: Kensai")]
        FighterKensai       	= 0x40030000U,

        [Description("Paladin: Cavalier")]
        PaladinCavalier     	= 0x40040000U,

        [Description("Paladin: Inquisitor")]
        PaladinInquisitor   	= 0x40050000U,

        [Description("Paladin: UndeadHunter")]
        PaladinUndeadHunter  	= 0x40060000U,

        [Description("Ranger: Archer")]
        RangerArcher        	= 0x40070000U,

        [Description("Ranger: Stalker")]
        RangerStalker       	= 0x40080000U,

        [Description("Ranger: Beastmaster")]
        RangerBeastmaster   	= 0x40090000U,

        [Description("Thief: Assassin")]
        ThiefAssassin       	= 0x400A0000U,

        [Description("Thief: Bounty Hunter")]
        ThiefBountyHunter   	= 0x400B0000U,

        [Description("Thief: Swashbuckler")]
        ThiefSwashbuckler   	= 0x400C0000U,

        [Description("Bard: Blade")]
        BardBlade           	= 0x400D0000U,

        [Description("Bard: Jester")]
        BardJester          	= 0x400E0000U,

        [Description("Bard: Skald")]
        BardSkald           	= 0x400F0000U,

        [Description("Druid: Totemic")]
        DruidTotemic        	= 0x40100000U,

        [Description("Druid: Shapeshifter")]
        DruidShapeshifter   	= 0x40110000U,

        [Description("Druid: Avenger")]
        DruidAvenger        	= 0x40120000U,

        [Description("Cleric of Talos")]
        ClericTalos         	= 0x40130000U,

        [Description("Cleric of Helm")]
        ClericHelm          	= 0x40140000U,

        [Description("Cleric of Lathander")]
        ClericLathander     	= 0x40150000U
    }
}