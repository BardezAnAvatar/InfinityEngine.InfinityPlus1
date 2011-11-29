using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Item.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Item.Item1
{
    public class ItemHeader1 : ItemHeader
    {
        /// <summary>Binary size of the struct on disk</summary>
        public new const Int32 StructSize = 114;    //here for signature purposes

        #region Members
        /// <summary>This is the resource1 reference to the replacement item (when it is destroyed?)</summary>
        protected ResourceReference replacementItem;

        /// <summary>Flags indicating the usability of the item</summary>
        protected ItemProhibitions1 itemProhibitionFlags;

        protected ResourceReference descriptionIcon;
        #endregion

        #region Properties
        /// <summary>This is the resource1 reference to the replacement item (when it is destroyed?)</summary>
        public ResourceReference ReplacementItem
        {
            get { return this.replacementItem; }
            set { this.replacementItem = value; }
        }

        public ItemProhibitions1 ItemProhibitionFlags
        {
            get { return this.itemProhibitionFlags; }
            set { this.itemProhibitionFlags = value; }
        }

        public ResourceReference DescriptionIcon
        {
            get { return this.descriptionIcon; }
            set { this.descriptionIcon = value; }
        }
        #endregion

        #region Constructor(s)
        public ItemHeader1()
        {
            this.descriptionIcon = null;
            this.descriptionIdentified = null;
            this.descriptionUnidentified = null;
            this.groundIcon = null;
            this.inventoryIcon = null;
            this.nameIdentified = null;
            this.nameUnidentified = null;
            this.replacementItem = null;

            this.countAbilities = 0;
            this.countEquippedEffects = 0;
        }
        #endregion

        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.descriptionIcon = new ResourceReference();
            this.descriptionIdentified = new StringReference();
            this.descriptionUnidentified = new StringReference();
            this.groundIcon = new ResourceReference();
            this.inventoryIcon = new ResourceReference();
            this.nameIdentified = new StringReference();
            this.nameUnidentified = new StringReference();
            this.replacementItem = new ResourceReference();
        }

        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        public override void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //read remiander
            Byte[] remainingBody = ReusableIO.BinaryRead(input, 106);

            this.nameUnidentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 0);
            this.nameIdentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 4);
            this.replacementItem.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 8, Constants.CultureCodeEnglish);
            this.itemFlags = (Enums.ItemFlags)ReusableIO.ReadUInt32FromArray(remainingBody, 16);
            this.itemType = (Enums.ItemType)ReusableIO.ReadUInt16FromArray(remainingBody, 20);
            this.itemProhibitionFlags = (Enums.ItemProhibitions1)ReusableIO.ReadUInt32FromArray(remainingBody, 22);
            this.weaponAnimation = ReusableIO.ReadStringFromByteArray(remainingBody, 26, Constants.CultureCodeEnglish, 2);
            this.minLevel = ReusableIO.ReadUInt16FromArray(remainingBody, 28);
            this.minStrength = remainingBody[30];
            this.unused1 = remainingBody[31];
            this.minStrengthBonus = remainingBody[32];
            this.kitProhibitions1 = (Enums.KitProhibitions1)remainingBody[33];
            this.minIntelligence = remainingBody[34];
            this.kitProhibitions2 = (Enums.KitProhibitions2)remainingBody[35];
            this.minDexterity = remainingBody[36];
            this.kitProhibitions3 = (Enums.KitProhibitions3)remainingBody[37];
            this.minWisdom = remainingBody[38];
            this.kitProhibitions4 = (Enums.KitProhibitions4)remainingBody[39];
            this.minConstitution = remainingBody[40];
            this.weaponProficiency = (Enums.WeaponProficiency)remainingBody[41];
            this.minCharisma = remainingBody[42];
            this.unused2 = remainingBody[43];
            this.price = ReusableIO.ReadUInt32FromArray(remainingBody, 44);
            this.stackSize = ReusableIO.ReadUInt16FromArray(remainingBody, 48);
            this.inventoryIcon.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 50, Constants.CultureCodeEnglish);
            this.identifyThreshold = ReusableIO.ReadUInt16FromArray(remainingBody, 58);
            this.groundIcon.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 60, Constants.CultureCodeEnglish);
            this.weight = ReusableIO.ReadUInt32FromArray(remainingBody, 68);
            this.descriptionUnidentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 72);
            this.descriptionIdentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 76);
            this.descriptionIcon.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 80, Constants.CultureCodeEnglish);
            this.enchantment = ReusableIO.ReadUInt32FromArray(remainingBody, 88);
            this.offsetAbilities = ReusableIO.ReadUInt32FromArray(remainingBody, 92);
            this.countAbilities = ReusableIO.ReadUInt16FromArray(remainingBody, 96);
            this.offsetAbilityEffects = ReusableIO.ReadUInt32FromArray(remainingBody, 98);
            this.indexEquippedEffects = ReusableIO.ReadUInt16FromArray(remainingBody, 102);
            this.countEquippedEffects = ReusableIO.ReadUInt16FromArray(remainingBody, 104);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, Constants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, Constants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteInt32ToStream(this.nameUnidentified.StringReferenceIndex, output);
            ReusableIO.WriteInt32ToStream(this.nameIdentified.StringReferenceIndex, output);
            ReusableIO.WriteStringToStream(this.replacementItem.ResRef, output, Constants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream((UInt32)this.itemFlags, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.itemType, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.itemProhibitionFlags, output);
            ReusableIO.WriteStringToStream(this.weaponAnimation, output, Constants.CultureCodeEnglish, false, 2);
            ReusableIO.WriteUInt16ToStream(this.minLevel, output);
            output.WriteByte(this.minStrength);
            output.WriteByte(this.unused1);
            output.WriteByte(this.minStrengthBonus);
            output.WriteByte((Byte)this.kitProhibitions1);
            output.WriteByte(this.minIntelligence);
            output.WriteByte((Byte)this.kitProhibitions2);
            output.WriteByte(this.minDexterity);
            output.WriteByte((Byte)this.kitProhibitions3);
            output.WriteByte(this.minWisdom);
            output.WriteByte((Byte)this.kitProhibitions4);
            output.WriteByte(this.minConstitution);
            output.WriteByte((Byte)this.weaponProficiency);
            output.WriteByte(this.minCharisma);
            output.WriteByte(this.unused2);
            ReusableIO.WriteUInt32ToStream(this.price, output);
            ReusableIO.WriteUInt16ToStream(this.stackSize, output);
            ReusableIO.WriteStringToStream(this.inventoryIcon.ResRef, output, Constants.CultureCodeEnglish);
            ReusableIO.WriteUInt16ToStream(this.identifyThreshold, output);
            ReusableIO.WriteStringToStream(this.groundIcon.ResRef, output, Constants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream(this.weight, output);
            ReusableIO.WriteInt32ToStream(this.descriptionUnidentified.StringReferenceIndex, output);
            ReusableIO.WriteInt32ToStream(this.descriptionIdentified.StringReferenceIndex, output);
            ReusableIO.WriteStringToStream(this.descriptionIcon.ResRef, output, Constants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream(this.enchantment, output);
            ReusableIO.WriteUInt32ToStream(this.offsetAbilities, output);
            ReusableIO.WriteUInt16ToStream(this.countAbilities, output);
            ReusableIO.WriteUInt32ToStream(this.offsetAbilityEffects, output);
            ReusableIO.WriteUInt16ToStream(this.indexEquippedEffects, output);
            ReusableIO.WriteUInt16ToStream(this.countEquippedEffects, output);
        }
        #endregion

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
            builder.Append(this.nameUnidentified.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Identified Name StrRef"));
            builder.Append(this.nameIdentified.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Replacement Item"));
            builder.Append(String.Format("'{0}'", this.replacementItem.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Item Flags"));
            builder.Append((UInt32)this.itemFlags);
            builder.Append(StringFormat.ToStringAlignment("Item Flags (enumerated)"));
            builder.Append(this.GetItemFlagString());
            builder.Append(StringFormat.ToStringAlignment("Item Type"));
            builder.Append((UInt16)this.itemType);
            builder.Append(StringFormat.ToStringAlignment("Item Type (description)"));
            builder.Append(this.itemType.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Item Prohibitions"));
            builder.Append((UInt32)this.itemProhibitionFlags);
            builder.Append(StringFormat.ToStringAlignment("Item Prohibitions (enumerated)"));
            builder.Append(this.GetItemProhibitionsString());
            builder.Append(StringFormat.ToStringAlignment("Weapon Animation Prefix"));
            builder.Append(String.Format("'{0}'", this.weaponAnimation));
            builder.Append(StringFormat.ToStringAlignment("Minimum Level to use"));
            builder.Append(this.minLevel);

            //out of order, for readability
            builder.Append(StringFormat.ToStringAlignment("Minimum Strength to use"));
            builder.Append(this.minStrength);
            builder.Append(StringFormat.ToStringAlignment("Minimum Strength Bonus to use"));
            builder.Append(this.minStrengthBonus);
            builder.Append(StringFormat.ToStringAlignment("Minimum Dexterity to use"));
            builder.Append(this.minDexterity);
            builder.Append(StringFormat.ToStringAlignment("Minimum Constitution to use"));
            builder.Append(this.minConstitution);
            builder.Append(StringFormat.ToStringAlignment("Minimum Intelligence to use"));
            builder.Append(this.minIntelligence);
            builder.Append(StringFormat.ToStringAlignment("Minimum Wisdom to use"));
            builder.Append(this.minWisdom);
            builder.Append(StringFormat.ToStringAlignment("Minimum Charisma to use"));
            builder.Append(this.minCharisma);
            builder.Append(StringFormat.ToStringAlignment("Kit prohibition flags 1"));
            builder.Append((Byte)this.kitProhibitions1);
            builder.Append(StringFormat.ToStringAlignment("Kit prohibition flags 1 (enumerated)"));
            builder.Append(this.GetKitProhibitionsString1());
            builder.Append(StringFormat.ToStringAlignment("Kit prohibition flags 2"));
            builder.Append((Byte)this.kitProhibitions2);
            builder.Append(StringFormat.ToStringAlignment("Kit prohibition flags 2 (enumerated)"));
            builder.Append(this.GetKitProhibitionsString2());
            builder.Append(StringFormat.ToStringAlignment("Kit prohibition flags 3"));
            builder.Append((Byte)this.kitProhibitions3);
            builder.Append(StringFormat.ToStringAlignment("Kit prohibition flags 3 (enumerated)"));
            builder.Append(this.GetKitProhibitionsString3());
            builder.Append(StringFormat.ToStringAlignment("Kit prohibition flags 4"));
            builder.Append((Byte)this.kitProhibitions4);
            builder.Append(StringFormat.ToStringAlignment("Kit prohibition flags 4 (enumerated)"));
            builder.Append(this.GetKitProhibitionsString4());
            builder.Append(StringFormat.ToStringAlignment("Weapon Proficency"));
            builder.Append((Byte)this.weaponProficiency);
            builder.Append(StringFormat.ToStringAlignment("Weapon Proficency (description)"));
            builder.Append(this.weaponProficiency.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Unused byte 1"));
            builder.Append(this.unused1);
            builder.Append(StringFormat.ToStringAlignment("Unused byte 2"));
            builder.Append(this.unused2);

            //back to in-order
            builder.Append(StringFormat.ToStringAlignment("Price"));
            builder.Append(this.price);
            builder.Append(StringFormat.ToStringAlignment("Maximum number per stack"));
            builder.Append(this.stackSize);
            builder.Append(StringFormat.ToStringAlignment("Inventory icon"));
            builder.Append(String.Format("'{0}'", this.inventoryIcon.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Identify Threshold"));
            builder.Append(this.identifyThreshold);
            builder.Append(StringFormat.ToStringAlignment("Ground image"));
            builder.Append(String.Format("'{0}'", this.groundIcon.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Weight (in pounds)"));
            builder.Append(this.weight);
            builder.Append(StringFormat.ToStringAlignment("Unidentified Description StrRef"));
            builder.Append(this.descriptionUnidentified.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Identified Description StrRef"));
            builder.Append(this.descriptionIdentified.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Description Image"));
            builder.Append(String.Format("'{0}'", this.descriptionIcon.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Magical Enchantment"));
            builder.Append(this.enchantment);
            builder.Append(StringFormat.ToStringAlignment("Offset to item Abilities"));
            builder.Append(this.offsetAbilities);
            builder.Append(StringFormat.ToStringAlignment("Count of item Abilities"));
            builder.Append(this.countAbilities);
            builder.Append(StringFormat.ToStringAlignment("Offset to item Abilities Effects"));
            builder.Append(this.offsetAbilityEffects);
            builder.Append(StringFormat.ToStringAlignment("Index to item Equipped Effects"));
            builder.Append(this.indexEquippedEffects);
            builder.Append(StringFormat.ToStringAlignment("Count of item Equipped Effects"));
            builder.Append(this.countEquippedEffects);

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which ItemUability1 flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetItemProhibitionsString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.OrderChaotic) == ItemProhibitions1.OrderChaotic, ItemProhibitions1.OrderChaotic.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.NatureEvil) == ItemProhibitions1.NatureEvil, ItemProhibitions1.NatureEvil.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.NatureGood) == ItemProhibitions1.NatureGood, ItemProhibitions1.NatureGood.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.NatureNeutral) == ItemProhibitions1.NatureNeutral, ItemProhibitions1.NatureNeutral.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.OrderLawful) == ItemProhibitions1.OrderLawful, ItemProhibitions1.OrderLawful.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.OrderNeutral) == ItemProhibitions1.OrderNeutral, ItemProhibitions1.OrderNeutral.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.ClassBard) == ItemProhibitions1.ClassBard, ItemProhibitions1.ClassBard.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.ClassCleric) == ItemProhibitions1.ClassCleric, ItemProhibitions1.ClassCleric.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.ClassClericMage) == ItemProhibitions1.ClassClericMage, ItemProhibitions1.ClassClericMage.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.ClassClericThief) == ItemProhibitions1.ClassClericThief, ItemProhibitions1.ClassClericThief.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.ClassClericRanger) == ItemProhibitions1.ClassClericRanger, ItemProhibitions1.ClassClericRanger.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.ClassFighter) == ItemProhibitions1.ClassFighter, ItemProhibitions1.ClassFighter.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.ClassFighterDruid) == ItemProhibitions1.ClassFighterDruid, ItemProhibitions1.ClassFighterDruid.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.ClassFighterMage) == ItemProhibitions1.ClassFighterMage, ItemProhibitions1.ClassFighterMage.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.ClassFighterCleric) == ItemProhibitions1.ClassFighterCleric, ItemProhibitions1.ClassFighterCleric.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.ClassFighterMageCleric) == ItemProhibitions1.ClassFighterMageCleric, ItemProhibitions1.ClassFighterMageCleric.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.ClassFighterMageThief) == ItemProhibitions1.ClassFighterMageThief, ItemProhibitions1.ClassFighterMageThief.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.ClassFighterThief) == ItemProhibitions1.ClassFighterThief, ItemProhibitions1.ClassFighterThief.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.ClassMage) == ItemProhibitions1.ClassMage, ItemProhibitions1.ClassMage.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.ClassMageThief) == ItemProhibitions1.ClassMageThief, ItemProhibitions1.ClassMageThief.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.ClassPaladin) == ItemProhibitions1.ClassPaladin, ItemProhibitions1.ClassPaladin.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.ClassRanger) == ItemProhibitions1.ClassRanger, ItemProhibitions1.ClassRanger.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.ClassThief) == ItemProhibitions1.ClassThief, ItemProhibitions1.ClassThief.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.RaceElf) == ItemProhibitions1.RaceElf, ItemProhibitions1.RaceElf.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.RaceDwarf) == ItemProhibitions1.RaceDwarf, ItemProhibitions1.RaceDwarf.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.RaceHalfElf) == ItemProhibitions1.RaceHalfElf, ItemProhibitions1.RaceHalfElf.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.RaceHalfling) == ItemProhibitions1.RaceHalfling, ItemProhibitions1.RaceHalfling.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.RaceHuman) == ItemProhibitions1.RaceHuman, ItemProhibitions1.RaceHuman.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.RaceGnome) == ItemProhibitions1.RaceGnome, ItemProhibitions1.RaceGnome.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.ClassMonk) == ItemProhibitions1.ClassMonk, ItemProhibitions1.ClassMonk.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.ClassDruid) == ItemProhibitions1.ClassDruid, ItemProhibitions1.ClassDruid.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1.RaceHalfOrc) == ItemProhibitions1.RaceHalfOrc, ItemProhibitions1.RaceHalfOrc.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
    }
}