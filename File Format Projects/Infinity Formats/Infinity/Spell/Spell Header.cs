using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell
{
    /// <summary>Base class for a spell's header</summary>
    public abstract class SpellHeader : ItemSpellHeader
    {
        #region Fields
        /// <summary>Reference to the resouce to use to play a casting completion sound</summary>
        public ResourceReference CastingCompletionSound { get; set; }

        /// <summary>Type of spell</summary>
        public SpellType Type { get; set; }

        /// <summary>Prohibition of spell casters</summary>
        public SpellProhibitionFlags ProhibitionFlags { get; set; }

        /// <summary>Graphics to display while casting the spell</summary>
        public SpellCastingGraphics CastingGraphics { get; set; }

        /// <summary>Minimum level required to use this spell</summary>
        public Byte MinimumLevel { get; set; }

        /// <summary>Magic school associated with this spell</summary>
        public SpellSchool School { get; set; }

        /// <summary>Minimum strength score required to use this item</summary>
        public Byte MinimumStrength { get; set; }

        /// <summary>Secondary nature of the spell</summary>
        public SpellNature Nature { get; set; }

        /// <summary>Minimum strength bonus required to use this item</summary>
        /// <remarks>A strength of 19 or above is considered to have a Strength Bonus of 0, and would therefore not be able to equip any weapon with a Strength Bonus restriction.</remarks>
        public Byte MinimumStrengthBonus { get; set; }

        /// <summary>First set of kit item usage prohibition</summary>
        public Byte KitProhibitions1 { get; set; }

        /// <summary>Minimum intelligence score required to use this item</summary>
        public Byte MinimumIntelligence { get; set; }

        /// <summary>Second set of kit item usage prohibition</summary>
        public Byte KitProhibitions2 { get; set; }

        /// <summary>Minimum dexterity score required to use this item</summary>
        public Byte MinimumDexterity { get; set; }

        /// <summary>Third set of kit item usage prohibition</summary>
        public Byte KitProhibitions3 { get; set; }

        /// <summary>Minimum dexterity score required to use this item</summary>
        public Byte MinimumWisdom { get; set; }

        /// <summary>Fourth set of kit item usage prohibition</summary>
        public Byte KitProhibitions4 { get; set; }

        /// <summary>Minimum constitution score required to use this item</summary>
        public UInt16 MinimumConstitution { get; set; }

        /// <summary>Minimum charisma score required to use this item</summary>
        public UInt16 MinimumCharisma { get; set; }

        /// <summary>Level of the spell</summary>
        public UInt32 Level { get; set; }

        /// <summary>Icon associated with this spell</summary>
        /// <remarks>The engine, according to IESDP, replaces the last character with 'C'</remarks>
        public ResourceReference Icon { get; set; }
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


        #region ToString() Helper(s)
        /// <summary>Generates a human-readable multi-line string for console output that indicates which ItemUability1 flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetProhibitionsString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.ProhibitionFlags & SpellProhibitionFlags.OrderChaotic) == SpellProhibitionFlags.OrderChaotic, SpellProhibitionFlags.OrderChaotic.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ProhibitionFlags & SpellProhibitionFlags.NatureEvil) == SpellProhibitionFlags.NatureEvil, SpellProhibitionFlags.NatureEvil.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ProhibitionFlags & SpellProhibitionFlags.NatureGood) == SpellProhibitionFlags.NatureGood, SpellProhibitionFlags.NatureGood.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ProhibitionFlags & SpellProhibitionFlags.NatureNeutral) == SpellProhibitionFlags.NatureNeutral, SpellProhibitionFlags.NatureNeutral.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ProhibitionFlags & SpellProhibitionFlags.OrderLawful) == SpellProhibitionFlags.OrderLawful, SpellProhibitionFlags.OrderLawful.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ProhibitionFlags & SpellProhibitionFlags.OrderNeutral) == SpellProhibitionFlags.OrderNeutral, SpellProhibitionFlags.OrderNeutral.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ProhibitionFlags & SpellProhibitionFlags.SchoolAbjurer) == SpellProhibitionFlags.SchoolAbjurer, SpellProhibitionFlags.SchoolAbjurer.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ProhibitionFlags & SpellProhibitionFlags.SchoolConjurer) == SpellProhibitionFlags.SchoolConjurer, SpellProhibitionFlags.SchoolConjurer.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ProhibitionFlags & SpellProhibitionFlags.SchoolDiviner) == SpellProhibitionFlags.SchoolDiviner, SpellProhibitionFlags.SchoolDiviner.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ProhibitionFlags & SpellProhibitionFlags.SchoolEnchanter) == SpellProhibitionFlags.SchoolEnchanter, SpellProhibitionFlags.SchoolEnchanter.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ProhibitionFlags & SpellProhibitionFlags.SchoolIllusionist) == SpellProhibitionFlags.SchoolIllusionist, SpellProhibitionFlags.SchoolIllusionist.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ProhibitionFlags & SpellProhibitionFlags.SchoolInvoker) == SpellProhibitionFlags.SchoolInvoker, SpellProhibitionFlags.SchoolInvoker.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ProhibitionFlags & SpellProhibitionFlags.SchoolNecromancer) == SpellProhibitionFlags.SchoolNecromancer, SpellProhibitionFlags.SchoolNecromancer.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ProhibitionFlags & SpellProhibitionFlags.SchoolTransmuter) == SpellProhibitionFlags.SchoolTransmuter, SpellProhibitionFlags.SchoolTransmuter.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ProhibitionFlags & SpellProhibitionFlags.SchoolGeneralist) == SpellProhibitionFlags.SchoolGeneralist, SpellProhibitionFlags.SchoolGeneralist.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ProhibitionFlags & SpellProhibitionFlags.DivineClericPaladin) == SpellProhibitionFlags.DivineClericPaladin, SpellProhibitionFlags.DivineClericPaladin.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ProhibitionFlags & SpellProhibitionFlags.DivineDruidRanger) == SpellProhibitionFlags.DivineDruidRanger, SpellProhibitionFlags.DivineDruidRanger.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}