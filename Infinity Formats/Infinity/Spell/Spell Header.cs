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
    public abstract class SpellHeader : ItemSpellHeader1
    {
        #region Members
        protected ResourceReference castingCompletionSound;
        protected SpellType type;
        protected SpellProhibitionFlags prohibitionFlags;
        protected SpellCastingGraphics castingGraphics;
        protected SpellSchool school;
        protected SpellNature nature;

        /// <summary>12 apparently unused bytes... This *probably* mirrors the item structure's minimum requirements for usage and kit flags.</summary>
        protected Byte[] reserved1;

        protected UInt32 level;

        /// <summary>An unknown, but is in same position as item header's stack size.</summary>
        protected UInt16 stackSize;

        /// <summary>The engine, according to IESDP, replaces the last character with 'C'</summary>
        protected ResourceReference icon;

        /// <summary>
        ///     14 apparently unused bytes... This *probably* mirrors the item structure's identfy threshold,
        ///     groundicon and weight; I've seen indicative 'rb' in the would-be ground icon, which backs up this theory.
        /// </summary>
        protected Byte[] reserved2;

        /// <summary>
        ///     12 apparently unused bytes... This *probably* mirrors the item structure's description icon,
        ///     and enchantment level; I've seen the "|@" null string data I've seen before, and it is in
        ///     the space that the description image would be.
        /// </summary>
        protected Byte[] reserved3;
	    #endregion

        #region Properties
        public ResourceReference CastingCompletionSound
        {
            get { return this.castingCompletionSound; }
            set { this.castingCompletionSound = value; }
        }

        public SpellType Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        public SpellProhibitionFlags ProhibitionFlags
        {
            get { return this.prohibitionFlags; }
            set { this.prohibitionFlags = value; }
        }

        public SpellCastingGraphics CastingGraphics
        {
            get { return this.castingGraphics; }
            set { this.castingGraphics = value; }
        }

        public SpellSchool School
        {
            get { return this.school; }
            set { this.school = value; }
        }

        public SpellNature Nature
        {
            get { return this.nature; }
            set { this.nature = value; }
        }

        public Byte[] Reserved1
        {
            get { return this.reserved1; }
            set { this.reserved1 = value; }
        }

        public UInt32 Level
        {
            get { return this.level; }
            set { this.level = value; }
        }

        public UInt16 StackSize
        {
            get { return this.stackSize; }
            set { this.stackSize = value; }
        }

        public ResourceReference Icon
        {
            get { return this.icon; }
            set { this.icon = value; }
        }

        public Byte[] Reserved2
        {
            get { return this.reserved2; }
            set { this.reserved2 = value; }
        }

        public Byte[] Reserved3
        {
            get { return this.reserved3; }
            set { this.reserved3 = value; }
        }
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

            StringFormat.AppendSubItem(sb, (this.prohibitionFlags & SpellProhibitionFlags.OrderChaotic) == SpellProhibitionFlags.OrderChaotic, SpellProhibitionFlags.OrderChaotic.GetDescription());
            StringFormat.AppendSubItem(sb, (this.prohibitionFlags & SpellProhibitionFlags.NatureEvil) == SpellProhibitionFlags.NatureEvil, SpellProhibitionFlags.NatureEvil.GetDescription());
            StringFormat.AppendSubItem(sb, (this.prohibitionFlags & SpellProhibitionFlags.NatureGood) == SpellProhibitionFlags.NatureGood, SpellProhibitionFlags.NatureGood.GetDescription());
            StringFormat.AppendSubItem(sb, (this.prohibitionFlags & SpellProhibitionFlags.NatureNeutral) == SpellProhibitionFlags.NatureNeutral, SpellProhibitionFlags.NatureNeutral.GetDescription());
            StringFormat.AppendSubItem(sb, (this.prohibitionFlags & SpellProhibitionFlags.OrderLawful) == SpellProhibitionFlags.OrderLawful, SpellProhibitionFlags.OrderLawful.GetDescription());
            StringFormat.AppendSubItem(sb, (this.prohibitionFlags & SpellProhibitionFlags.OrderNeutral) == SpellProhibitionFlags.OrderNeutral, SpellProhibitionFlags.OrderNeutral.GetDescription());
            StringFormat.AppendSubItem(sb, (this.prohibitionFlags & SpellProhibitionFlags.SchoolAbjurer) == SpellProhibitionFlags.SchoolAbjurer, SpellProhibitionFlags.SchoolAbjurer.GetDescription());
            StringFormat.AppendSubItem(sb, (this.prohibitionFlags & SpellProhibitionFlags.SchoolConjurer) == SpellProhibitionFlags.SchoolConjurer, SpellProhibitionFlags.SchoolConjurer.GetDescription());
            StringFormat.AppendSubItem(sb, (this.prohibitionFlags & SpellProhibitionFlags.SchoolDiviner) == SpellProhibitionFlags.SchoolDiviner, SpellProhibitionFlags.SchoolDiviner.GetDescription());
            StringFormat.AppendSubItem(sb, (this.prohibitionFlags & SpellProhibitionFlags.SchoolEnchanter) == SpellProhibitionFlags.SchoolEnchanter, SpellProhibitionFlags.SchoolEnchanter.GetDescription());
            StringFormat.AppendSubItem(sb, (this.prohibitionFlags & SpellProhibitionFlags.SchoolIllusionist) == SpellProhibitionFlags.SchoolIllusionist, SpellProhibitionFlags.SchoolIllusionist.GetDescription());
            StringFormat.AppendSubItem(sb, (this.prohibitionFlags & SpellProhibitionFlags.SchoolInvoker) == SpellProhibitionFlags.SchoolInvoker, SpellProhibitionFlags.SchoolInvoker.GetDescription());
            StringFormat.AppendSubItem(sb, (this.prohibitionFlags & SpellProhibitionFlags.SchoolNecromancer) == SpellProhibitionFlags.SchoolNecromancer, SpellProhibitionFlags.SchoolNecromancer.GetDescription());
            StringFormat.AppendSubItem(sb, (this.prohibitionFlags & SpellProhibitionFlags.SchoolTransmuter) == SpellProhibitionFlags.SchoolTransmuter, SpellProhibitionFlags.SchoolTransmuter.GetDescription());
            StringFormat.AppendSubItem(sb, (this.prohibitionFlags & SpellProhibitionFlags.SchoolGeneralist) == SpellProhibitionFlags.SchoolGeneralist, SpellProhibitionFlags.SchoolGeneralist.GetDescription());
            StringFormat.AppendSubItem(sb, (this.prohibitionFlags & SpellProhibitionFlags.DivineClericPaladin) == SpellProhibitionFlags.DivineClericPaladin, SpellProhibitionFlags.DivineClericPaladin.GetDescription());
            StringFormat.AppendSubItem(sb, (this.prohibitionFlags & SpellProhibitionFlags.DivineDruidRanger) == SpellProhibitionFlags.DivineDruidRanger, SpellProhibitionFlags.DivineDruidRanger.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}