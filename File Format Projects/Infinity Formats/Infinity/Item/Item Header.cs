using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item
{
    /// <summary>Generic item data header</summary>
    public abstract class ItemHeader : ItemSpellHeader
    {
        #region Fields
        /// <summary>Flags indicating the nature of the item</summary>
        public ItemFlags ItemFlags { get; set; }

        /// <summary>Value indicating the type of the item</summary>
        public ItemType ItemType { get; set; }

        /// <summary>Two-character code indicating the weapon animation</summary>
        /// <value>
        ///     Value(ASCII) 	Value(Hex) 	Description
        ///     "  " 	        2020h 	    [none]
        ///     "2A" 	        3241h 	    Leather Armor
        ///     "3A" 	        3341h 	    Chainmail
        ///     "4A" 	        3441h 	    Plate Mail
        ///     "2W" 	        3257h 	    Robe
        ///     "3W" 	        3357h 	    Robe
        ///     "4W" 	        3457h 	    Robe
        ///     "AX" 	        4158h 	    Axe
        ///     "BW" 	        4257h 	    Bow
        ///     "CB" 	        4342h 	    Crossbow
        ///     "CL" 	        434Ch 	    Club
        ///     "D1" 	        4431h 	    Buckler
        ///     "D2" 	        4432h 	    Shield (Small)
        ///     "D3" 	        4433h 	    Shield (Medium)
        ///     "D4" 	        4434h 	    Shield (Large)
        ///     "DD" 	        4444h 	    Dagger
        ///     "FL" 	        464Ch 	    Flail
        ///     "FS" 	        4653h 	    Flame Sword
        ///     "H0" 	        4830h 	    Small Vertical Horns
        ///     "H1" 	        4831h 	    Large Horizontal Horns
        ///     "H2" 	        4832h 	    Feather Wings
        ///     "H3" 	        4833h 	    Top Plume
        ///     "H4" 	        4834h 	    Dragon Wings
        ///     "H5" 	        4835h 	    Feather Sideburns
        ///     "H6" 	        4836h 	    Large Curved Horns (incorrect paperdoll image)
        ///     "HB" 	        4842h 	    Halberd
        ///     "MC" 	        4D43h 	    Mace
        ///     "MS" 	        4D53h 	    Morning Star
        ///     "QS" 	        5153h 	    Quarter Staff (Metal)
        ///     "S1" 	        5331h 	    Sword 1-Handed
        ///     "S2" 	        5332h 	    Sword 2-Handed
        ///     "SL" 	        534Ch 	    Sling
        ///     "SP" 	        5350h 	    Spear
        ///     "SS" 	        5353h 	    Short Sword
        ///     "WH" 	        5748h 	    War Hammer
        ///     "S3" 	        5333h 	    Katana #
        ///     "SC" 	        5343h 	    Scimitar #
        /// </value>
        public String OverlayAnimation { get; set; }

        /// <summary>Minimum level required to use this item</summary>
        public UInt16 MinimumLevel { get; set; }

        /// <summary>Minimum strength score required to use this item</summary>
        public UInt16 MinimumStrength { get; set; }

        /// <summary>Minimum strength bonus required to use this item</summary>
        /// <remarks>A strength of 19 or above is considered to have a Strength Bonus of 0, and would therefore not be able to equip any weapon with a Strength Bonus restriction.</remarks>
        public Byte MinimumStrengthBonus { get; set; }

        /// <summary>First set of kit item usage prohibition</summary>
        public KitProhibitions1 KitProhibitions1 { get; set; }

        /// <summary>Minimum intelligence score required to use this item</summary>
        public Byte MinimumIntelligence { get; set; }

        /// <summary>Second set of kit item usage prohibition</summary>
        public KitProhibitions2 KitProhibitions2 { get; set; }

        /// <summary>Minimum dexterity score required to use this item</summary>
        public Byte MinimumDexterity { get; set; }

        /// <summary>Third set of kit item usage prohibition</summary>
        public KitProhibitions3 KitProhibitions3 { get; set; }

        /// <summary>Minimum dexterity score required to use this item</summary>
        public Byte MinimumWisdom { get; set; }

        /// <summary>Fourth set of kit item usage prohibition</summary>
        public KitProhibitions4 KitProhibitions4 { get; set; }

        /// <summary>Minimum constitution score required to use this item</summary>
        public Byte MinimumConstitution { get; set; }

        /// <summary>Weapon proficiency associated with this item</summary>
        public WeaponProficiency WeaponProficiency { get; set; }

        /// <summary>Minimum charisma score required to use this item</summary>
        public UInt16 MinimumCharisma { get; set; }

        /// <summary>Base cost for this item</summary>
        public UInt32 BaseCost { get; set; }

        /// <summary>Icon associated with this item in inventory</summary>
        public ResourceReference InventoryIcon { get; set; }

        /// <summary>Level of enchantment when calculating magical enchantment to hit creatures with damage resistance. Not attack or damage bonus.</summary>
        public UInt32 Enchantment { get; set; }
	    #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the output stream. Reads the whole structure.</summary>
        public override void Read(Stream input)
        {
            //read signature
            Byte[] buffer = ReusableIO.BinaryRead(input, 8);   //header buffer

            this.signature = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish, 4);
            this.version = ReusableIO.ReadStringFromByteArray(buffer, 4, CultureConstants.CultureCodeEnglish, 4);

            this.ReadBody(input);
        }
        #endregion


        #region ToString() helpers
        /// <summary>Generates a human-readable multi-line string for console output that indicates which ItemFlags flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetItemFlagString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.ItemFlags & ItemFlags.CriticalItem) == ItemFlags.CriticalItem, ItemFlags.CriticalItem.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemFlags & ItemFlags.TwoHanded) == ItemFlags.TwoHanded, ItemFlags.TwoHanded.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemFlags & ItemFlags.Movable) == ItemFlags.Movable, ItemFlags.Movable.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemFlags & ItemFlags.Displayable) == ItemFlags.Displayable, ItemFlags.Displayable.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemFlags & ItemFlags.Cursed) == ItemFlags.Cursed, ItemFlags.Cursed.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemFlags & ItemFlags.CannotScribe) == ItemFlags.CannotScribe, ItemFlags.CannotScribe.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemFlags & ItemFlags.Magical) == ItemFlags.Magical, ItemFlags.Magical.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemFlags & ItemFlags.Bow) == ItemFlags.Bow, ItemFlags.Bow.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemFlags & ItemFlags.Silver) == ItemFlags.Silver, ItemFlags.Silver.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemFlags & ItemFlags.ColdIron) == ItemFlags.ColdIron, ItemFlags.ColdIron.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemFlags & ItemFlags.Stolen) == ItemFlags.Stolen, ItemFlags.Stolen.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemFlags & ItemFlags.Conversable) == ItemFlags.Conversable, ItemFlags.Conversable.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemFlags & ItemFlags.Pulsating) == ItemFlags.Pulsating, ItemFlags.Pulsating.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which KitUability1 flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetKitProhibitionsString1()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.KitProhibitions1 & KitProhibitions1.ClericTalos) == KitProhibitions1.ClericTalos, KitProhibitions1.ClericTalos.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions1 & KitProhibitions1.ClericHelm) == KitProhibitions1.ClericHelm, KitProhibitions1.ClericHelm.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions1 & KitProhibitions1.ClericLathlander) == KitProhibitions1.ClericLathlander, KitProhibitions1.ClericLathlander.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions1 & KitProhibitions1.DruidTotemic) == KitProhibitions1.DruidTotemic, KitProhibitions1.DruidTotemic.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions1 & KitProhibitions1.DruidShapeshifter) == KitProhibitions1.DruidShapeshifter, KitProhibitions1.DruidShapeshifter.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions1 & KitProhibitions1.DruidAvenger) == KitProhibitions1.DruidAvenger, KitProhibitions1.DruidAvenger.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions1 & KitProhibitions1.ClassBarbarian) == KitProhibitions1.ClassBarbarian, KitProhibitions1.ClassBarbarian.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions1 & KitProhibitions1.MageWild) == KitProhibitions1.MageWild, KitProhibitions1.MageWild.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which KitUability2 flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetKitProhibitionsString2()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.KitProhibitions2 & KitProhibitions2.RangerStalker) == KitProhibitions2.RangerStalker, KitProhibitions2.RangerStalker.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions2 & KitProhibitions2.RangerBeastmaster) == KitProhibitions2.RangerBeastmaster, KitProhibitions2.RangerBeastmaster.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions2 & KitProhibitions2.ThiefAssassin) == KitProhibitions2.ThiefAssassin, KitProhibitions2.ThiefAssassin.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions2 & KitProhibitions2.ThiefBountyHunter) == KitProhibitions2.ThiefBountyHunter, KitProhibitions2.ThiefBountyHunter.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions2 & KitProhibitions2.ThiefSwashbuckler) == KitProhibitions2.ThiefSwashbuckler, KitProhibitions2.ThiefSwashbuckler.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions2 & KitProhibitions2.BardBlade) == KitProhibitions2.BardBlade, KitProhibitions2.BardBlade.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions2 & KitProhibitions2.BardJester) == KitProhibitions2.BardJester, KitProhibitions2.BardJester.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions2 & KitProhibitions2.BardSkald) == KitProhibitions2.BardSkald, KitProhibitions2.BardSkald.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which KitUability3 flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetKitProhibitionsString3()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.KitProhibitions3 & KitProhibitions3.MageDiviner) == KitProhibitions3.MageDiviner, KitProhibitions3.MageDiviner.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions3 & KitProhibitions3.MageEnchanter) == KitProhibitions3.MageEnchanter, KitProhibitions3.MageEnchanter.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions3 & KitProhibitions3.MageIllusionist) == KitProhibitions3.MageIllusionist, KitProhibitions3.MageIllusionist.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions3 & KitProhibitions3.MageInvoker) == KitProhibitions3.MageInvoker, KitProhibitions3.MageInvoker.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions3 & KitProhibitions3.MageNecromancer) == KitProhibitions3.MageNecromancer, KitProhibitions3.MageNecromancer.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions3 & KitProhibitions3.MageTransmuter) == KitProhibitions3.MageTransmuter, KitProhibitions3.MageTransmuter.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions3 & KitProhibitions3.AllNoKit) == KitProhibitions3.AllNoKit, KitProhibitions3.AllNoKit.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions3 & KitProhibitions3.RangerArcher) == KitProhibitions3.RangerArcher, KitProhibitions3.RangerArcher.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which KitUability4 flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetKitProhibitionsString4()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.KitProhibitions4 & KitProhibitions4.FighterBeserker) == KitProhibitions4.FighterBeserker, KitProhibitions4.FighterBeserker.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions4 & KitProhibitions4.FighterWizardslayer) == KitProhibitions4.FighterWizardslayer, KitProhibitions4.FighterWizardslayer.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions4 & KitProhibitions4.FighterKensai) == KitProhibitions4.FighterKensai, KitProhibitions4.FighterKensai.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions4 & KitProhibitions4.PaladinCavalier) == KitProhibitions4.PaladinCavalier, KitProhibitions4.PaladinCavalier.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions4 & KitProhibitions4.PaladnInquisiter) == KitProhibitions4.PaladnInquisiter, KitProhibitions4.PaladnInquisiter.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions4 & KitProhibitions4.PaladinUndeadHunter) == KitProhibitions4.PaladinUndeadHunter, KitProhibitions4.PaladinUndeadHunter.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions4 & KitProhibitions4.MageAbjurer) == KitProhibitions4.MageAbjurer, KitProhibitions4.MageAbjurer.GetDescription());
            StringFormat.AppendSubItem(sb, (this.KitProhibitions4 & KitProhibitions4.MageConjurer) == KitProhibitions4.MageConjurer, KitProhibitions4.MageConjurer.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}