using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item.Item1
{
    /// <summary>Header for a version 1 item</summary>
    public class ItemHeader1 : ItemHeader
    {
        #region Constants
        /// <summary>Binary size of the struct on disk</summary>
        public new const Int32 StructSize = 114;    //here for signature purposes
        #endregion


        #region Members
        /// <summary>This is the resource1 reference to the replacement item (when it is destroyed?)</summary>
        public ResourceReference ReplacementItem { get; set; }

        /// <summary>Flags indicating the usability of the item</summary>
        public ItemProhibitions1 ItemProhibitionFlags { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public ItemHeader1()
        {
            this.DescriptionIcon = null;
            this.DescriptionIdentified = null;
            this.DescriptionUnidentified = null;
            this.GroundIcon = null;
            this.InventoryIcon = null;
            this.NameIdentified = null;
            this.NameUnidentified = null;
            this.ReplacementItem = null;

            this.CountAbilities = 0;
            this.CountEquippedEffects = 0;
        }

        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.DescriptionIcon = new ResourceReference();
            this.DescriptionIdentified = new StringReference();
            this.DescriptionUnidentified = new StringReference();
            this.GroundIcon = new ResourceReference();
            this.InventoryIcon = new ResourceReference();
            this.NameIdentified = new StringReference();
            this.NameUnidentified = new StringReference();
            this.ReplacementItem = new ResourceReference();
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        public override void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //read remiander
            Byte[] remainingBody = ReusableIO.BinaryRead(input, 106);

            this.NameUnidentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 0);
            this.NameIdentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 4);
            this.ReplacementItem.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 8, CultureConstants.CultureCodeEnglish);
            this.ItemFlags = (Enums.ItemFlags)ReusableIO.ReadUInt32FromArray(remainingBody, 16);
            this.ItemType = (Enums.ItemType)ReusableIO.ReadUInt16FromArray(remainingBody, 20);
            this.ItemProhibitionFlags = (Enums.ItemProhibitions1)ReusableIO.ReadUInt32FromArray(remainingBody, 22);
            this.OverlayAnimation = ReusableIO.ReadStringFromByteArray(remainingBody, 26, CultureConstants.CultureCodeEnglish, 2);
            this.MinimumLevel = ReusableIO.ReadUInt16FromArray(remainingBody, 28);
            this.MinimumStrength = ReusableIO.ReadUInt16FromArray(remainingBody, 30);
            this.MinimumStrengthBonus = remainingBody[32];
            this.KitProhibitions1 = (Enums.KitProhibitions1)remainingBody[33];
            this.MinimumIntelligence = remainingBody[34];
            this.KitProhibitions2 = (Enums.KitProhibitions2)remainingBody[35];
            this.MinimumDexterity = remainingBody[36];
            this.KitProhibitions3 = (Enums.KitProhibitions3)remainingBody[37];
            this.MinimumWisdom = remainingBody[38];
            this.KitProhibitions4 = (Enums.KitProhibitions4)remainingBody[39];
            this.MinimumConstitution = remainingBody[40];
            this.WeaponProficiency = (Enums.WeaponProficiency)remainingBody[41];
            this.MinimumCharisma = ReusableIO.ReadUInt16FromArray(remainingBody, 42);
            this.BaseCost = ReusableIO.ReadUInt32FromArray(remainingBody, 44);
            this.StackSize = ReusableIO.ReadUInt16FromArray(remainingBody, 48);
            this.InventoryIcon.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 50, CultureConstants.CultureCodeEnglish);
            this.IdentifyThreshold = ReusableIO.ReadUInt16FromArray(remainingBody, 58);
            this.GroundIcon.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 60, CultureConstants.CultureCodeEnglish);
            this.Weight = ReusableIO.ReadUInt32FromArray(remainingBody, 68);
            this.DescriptionUnidentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 72);
            this.DescriptionIdentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 76);
            this.DescriptionIcon.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 80, CultureConstants.CultureCodeEnglish);
            this.Enchantment = ReusableIO.ReadUInt32FromArray(remainingBody, 88);
            this.OffsetAbilities = ReusableIO.ReadUInt32FromArray(remainingBody, 92);
            this.CountAbilities = ReusableIO.ReadUInt16FromArray(remainingBody, 96);
            this.OffsetAbilityEffects = ReusableIO.ReadUInt32FromArray(remainingBody, 98);
            this.IndexEquippedEffects = ReusableIO.ReadUInt16FromArray(remainingBody, 102);
            this.CountEquippedEffects = ReusableIO.ReadUInt16FromArray(remainingBody, 104);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteInt32ToStream(this.NameUnidentified.StringReferenceIndex, output);
            ReusableIO.WriteInt32ToStream(this.NameIdentified.StringReferenceIndex, output);
            ReusableIO.WriteStringToStream(this.ReplacementItem.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream((UInt32)this.ItemFlags, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.ItemType, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.ItemProhibitionFlags, output);
            ReusableIO.WriteStringToStream(this.OverlayAnimation, output, CultureConstants.CultureCodeEnglish, false, 2);
            ReusableIO.WriteUInt16ToStream(this.MinimumLevel, output);
            ReusableIO.WriteUInt16ToStream(this.MinimumStrength, output);
            output.WriteByte(this.MinimumStrengthBonus);
            output.WriteByte((Byte)this.KitProhibitions1);
            output.WriteByte(this.MinimumIntelligence);
            output.WriteByte((Byte)this.KitProhibitions2);
            output.WriteByte(this.MinimumDexterity);
            output.WriteByte((Byte)this.KitProhibitions3);
            output.WriteByte(this.MinimumWisdom);
            output.WriteByte((Byte)this.KitProhibitions4);
            output.WriteByte(this.MinimumConstitution);
            output.WriteByte((Byte)this.WeaponProficiency);
            ReusableIO.WriteUInt16ToStream(this.MinimumCharisma, output);
            ReusableIO.WriteUInt32ToStream(this.BaseCost, output);
            ReusableIO.WriteUInt16ToStream(this.StackSize, output);
            ReusableIO.WriteStringToStream(this.InventoryIcon.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt16ToStream(this.IdentifyThreshold, output);
            ReusableIO.WriteStringToStream(this.GroundIcon.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream(this.Weight, output);
            ReusableIO.WriteInt32ToStream(this.DescriptionUnidentified.StringReferenceIndex, output);
            ReusableIO.WriteInt32ToStream(this.DescriptionIdentified.StringReferenceIndex, output);
            ReusableIO.WriteStringToStream(this.DescriptionIcon.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream(this.Enchantment, output);
            ReusableIO.WriteUInt32ToStream(this.OffsetAbilities, output);
            ReusableIO.WriteUInt16ToStream(this.CountAbilities, output);
            ReusableIO.WriteUInt32ToStream(this.OffsetAbilityEffects, output);
            ReusableIO.WriteUInt16ToStream(this.IndexEquippedEffects, output);
            ReusableIO.WriteUInt16ToStream(this.CountEquippedEffects, output);
        }
        #endregion


        #region ToString override(s)
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("ITEM Version 1.0 Header:");
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(String.Format("'{0}'", this.signature));
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(String.Format("'{0}'", this.version));
            builder.Append(StringFormat.ToStringAlignment("Unidentified Name StrRef"));
            builder.Append(this.NameUnidentified.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Identified Name StrRef"));
            builder.Append(this.NameIdentified.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Replacement Item"));
            builder.Append(String.Format("'{0}'", this.ReplacementItem.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Item Flags"));
            builder.Append((UInt32)this.ItemFlags);
            builder.Append(StringFormat.ToStringAlignment("Item Flags (enumerated)"));
            builder.Append(this.GetItemFlagString());
            builder.Append(StringFormat.ToStringAlignment("Item Type"));
            builder.Append((UInt16)this.ItemType);
            builder.Append(StringFormat.ToStringAlignment("Item Type (description)"));
            builder.Append(this.ItemType.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Item Prohibitions"));
            builder.Append((UInt32)this.ItemProhibitionFlags);
            builder.Append(StringFormat.ToStringAlignment("Item Prohibitions (enumerated)"));
            builder.Append(this.GetItemProhibitionsString());
            builder.Append(StringFormat.ToStringAlignment("Overlay Animation Prefix"));
            builder.Append(String.Format("'{0}'", this.OverlayAnimation));
            builder.Append(StringFormat.ToStringAlignment("Minimum Level to use"));
            builder.Append(this.MinimumLevel);

            //out of order, for readability
            builder.Append(StringFormat.ToStringAlignment("Minimum Strength to use"));
            builder.Append(this.MinimumStrength);
            builder.Append(StringFormat.ToStringAlignment("Minimum Strength Bonus to use"));
            builder.Append(this.MinimumStrengthBonus);
            builder.Append(StringFormat.ToStringAlignment("Minimum Dexterity to use"));
            builder.Append(this.MinimumDexterity);
            builder.Append(StringFormat.ToStringAlignment("Minimum Constitution to use"));
            builder.Append(this.MinimumConstitution);
            builder.Append(StringFormat.ToStringAlignment("Minimum Intelligence to use"));
            builder.Append(this.MinimumIntelligence);
            builder.Append(StringFormat.ToStringAlignment("Minimum Wisdom to use"));
            builder.Append(this.MinimumWisdom);
            builder.Append(StringFormat.ToStringAlignment("Minimum Charisma to use"));
            builder.Append(this.MinimumCharisma);
            builder.Append(StringFormat.ToStringAlignment("Kit prohibition flags 1"));
            builder.Append((Byte)this.KitProhibitions1);
            builder.Append(StringFormat.ToStringAlignment("Kit prohibition flags 1 (enumerated)"));
            builder.Append(this.GetKitProhibitionsString1());
            builder.Append(StringFormat.ToStringAlignment("Kit prohibition flags 2"));
            builder.Append((Byte)this.KitProhibitions2);
            builder.Append(StringFormat.ToStringAlignment("Kit prohibition flags 2 (enumerated)"));
            builder.Append(this.GetKitProhibitionsString2());
            builder.Append(StringFormat.ToStringAlignment("Kit prohibition flags 3"));
            builder.Append((Byte)this.KitProhibitions3);
            builder.Append(StringFormat.ToStringAlignment("Kit prohibition flags 3 (enumerated)"));
            builder.Append(this.GetKitProhibitionsString3());
            builder.Append(StringFormat.ToStringAlignment("Kit prohibition flags 4"));
            builder.Append((Byte)this.KitProhibitions4);
            builder.Append(StringFormat.ToStringAlignment("Kit prohibition flags 4 (enumerated)"));
            builder.Append(this.GetKitProhibitionsString4());
            builder.Append(StringFormat.ToStringAlignment("Weapon Proficency"));
            builder.Append((Byte)this.WeaponProficiency);
            builder.Append(StringFormat.ToStringAlignment("Weapon Proficency (description)"));
            builder.Append(this.WeaponProficiency.GetDescription());

            //back to in-order
            builder.Append(StringFormat.ToStringAlignment("Base Cost"));
            builder.Append(this.BaseCost);
            builder.Append(StringFormat.ToStringAlignment("Maximum number per stack"));
            builder.Append(this.StackSize);
            builder.Append(StringFormat.ToStringAlignment("Inventory icon"));
            builder.Append(String.Format("'{0}'", this.InventoryIcon.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Identify Threshold"));
            builder.Append(this.IdentifyThreshold);
            builder.Append(StringFormat.ToStringAlignment("Ground image"));
            builder.Append(String.Format("'{0}'", this.GroundIcon.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Weight (in pounds)"));
            builder.Append(this.Weight);
            builder.Append(StringFormat.ToStringAlignment("Unidentified Description StrRef"));
            builder.Append(this.DescriptionUnidentified.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Identified Description StrRef"));
            builder.Append(this.DescriptionIdentified.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Description Image"));
            builder.Append(String.Format("'{0}'", this.DescriptionIcon.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Magical Enchantment"));
            builder.Append(this.Enchantment);
            builder.Append(StringFormat.ToStringAlignment("Offset to item Abilities"));
            builder.Append(this.OffsetAbilities);
            builder.Append(StringFormat.ToStringAlignment("Count of item Abilities"));
            builder.Append(this.CountAbilities);
            builder.Append(StringFormat.ToStringAlignment("Offset to item Abilities Effects"));
            builder.Append(this.OffsetAbilityEffects);
            builder.Append(StringFormat.ToStringAlignment("Index to item Equipped Effects"));
            builder.Append(this.IndexEquippedEffects);
            builder.Append(StringFormat.ToStringAlignment("Count of item Equipped Effects"));
            builder.Append(this.CountEquippedEffects);

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which ItemUability1 flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetItemProhibitionsString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.OrderChaotic) == ItemProhibitions1.OrderChaotic, ItemProhibitions1.OrderChaotic.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.NatureEvil) == ItemProhibitions1.NatureEvil, ItemProhibitions1.NatureEvil.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.NatureGood) == ItemProhibitions1.NatureGood, ItemProhibitions1.NatureGood.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.NatureNeutral) == ItemProhibitions1.NatureNeutral, ItemProhibitions1.NatureNeutral.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.OrderLawful) == ItemProhibitions1.OrderLawful, ItemProhibitions1.OrderLawful.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.OrderNeutral) == ItemProhibitions1.OrderNeutral, ItemProhibitions1.OrderNeutral.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.ClassBard) == ItemProhibitions1.ClassBard, ItemProhibitions1.ClassBard.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.ClassCleric) == ItemProhibitions1.ClassCleric, ItemProhibitions1.ClassCleric.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.ClassClericMage) == ItemProhibitions1.ClassClericMage, ItemProhibitions1.ClassClericMage.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.ClassClericThief) == ItemProhibitions1.ClassClericThief, ItemProhibitions1.ClassClericThief.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.ClassClericRanger) == ItemProhibitions1.ClassClericRanger, ItemProhibitions1.ClassClericRanger.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.ClassFighter) == ItemProhibitions1.ClassFighter, ItemProhibitions1.ClassFighter.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.ClassFighterDruid) == ItemProhibitions1.ClassFighterDruid, ItemProhibitions1.ClassFighterDruid.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.ClassFighterMage) == ItemProhibitions1.ClassFighterMage, ItemProhibitions1.ClassFighterMage.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.ClassFighterCleric) == ItemProhibitions1.ClassFighterCleric, ItemProhibitions1.ClassFighterCleric.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.ClassFighterMageCleric) == ItemProhibitions1.ClassFighterMageCleric, ItemProhibitions1.ClassFighterMageCleric.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.ClassFighterMageThief) == ItemProhibitions1.ClassFighterMageThief, ItemProhibitions1.ClassFighterMageThief.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.ClassFighterThief) == ItemProhibitions1.ClassFighterThief, ItemProhibitions1.ClassFighterThief.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.ClassMage) == ItemProhibitions1.ClassMage, ItemProhibitions1.ClassMage.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.ClassMageThief) == ItemProhibitions1.ClassMageThief, ItemProhibitions1.ClassMageThief.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.ClassPaladin) == ItemProhibitions1.ClassPaladin, ItemProhibitions1.ClassPaladin.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.ClassRanger) == ItemProhibitions1.ClassRanger, ItemProhibitions1.ClassRanger.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.ClassThief) == ItemProhibitions1.ClassThief, ItemProhibitions1.ClassThief.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.RaceElf) == ItemProhibitions1.RaceElf, ItemProhibitions1.RaceElf.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.RaceDwarf) == ItemProhibitions1.RaceDwarf, ItemProhibitions1.RaceDwarf.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.RaceHalfElf) == ItemProhibitions1.RaceHalfElf, ItemProhibitions1.RaceHalfElf.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.RaceHalfling) == ItemProhibitions1.RaceHalfling, ItemProhibitions1.RaceHalfling.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.RaceHuman) == ItemProhibitions1.RaceHuman, ItemProhibitions1.RaceHuman.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.RaceGnome) == ItemProhibitions1.RaceGnome, ItemProhibitions1.RaceGnome.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.ClassMonk) == ItemProhibitions1.ClassMonk, ItemProhibitions1.ClassMonk.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.ClassDruid) == ItemProhibitions1.ClassDruid, ItemProhibitions1.ClassDruid.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ItemProhibitionFlags & ItemProhibitions1.RaceHalfOrc) == ItemProhibitions1.RaceHalfOrc, ItemProhibitions1.RaceHalfOrc.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}