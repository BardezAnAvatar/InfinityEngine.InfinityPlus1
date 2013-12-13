using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item.Item2
{
    public class ItemHeader2 : ItemHeader
    {
        /// <summary>Binary size of the struct on disk</summary>
        public new const Int32 StructSize = 130;    //here for signature purposes

        #region Members
        /// <summary>This is the resource1 reference to the replacement item (when it is destroyed?)</summary>
        protected ResourceReference replacementItem;

        /// <summary>Flags indicating the prohibition of the item</summary>
        protected ItemProhibitions2 itemProhibitionFlags;

        protected ResourceReference descriptionIcon;

        /// <summary>16 reserved bytes</summary>
        protected Byte[] reserved;
        #endregion

        #region Properties
        /// <summary>This is the resource1 reference to the replacement item (when it is destroyed?)</summary>
        public ResourceReference ReplacementItem
        {
            get { return this.replacementItem; }
            set { this.replacementItem = value; }
        }

        public ItemProhibitions2 ItemProhibitionFlags
        {
            get { return itemProhibitionFlags; }
            set { itemProhibitionFlags = value; }
        }

        public ResourceReference DescriptionIcon
        {
            get { return descriptionIcon; }
            set { descriptionIcon = value; }
        }

        public Byte[] Reserved
        {
            get { return reserved; }
            set { reserved = value; }
        }
        #endregion

        #region Constructor(s)
        public ItemHeader2()
        {
            this.descriptionIcon = null;
            this.descriptionIdentified = null;
            this.descriptionUnidentified = null;
            this.groundIcon = null;
            this.inventoryIcon = null;
            this.nameIdentified = null;
            this.nameUnidentified = null;
            this.replacementItem = null;
            this.reserved = null;

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
            this.reserved = new Byte[16];
        }

        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        public override void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //read remiander
            Byte[] remainingBody = ReusableIO.BinaryRead(input, 122);

            this.nameUnidentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 0);
            this.nameIdentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 4);
            this.replacementItem.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 8, CultureConstants.CultureCodeEnglish);
            this.itemFlags = (Enums.ItemFlags)ReusableIO.ReadUInt32FromArray(remainingBody, 16);
            this.itemType = (Enums.ItemType)ReusableIO.ReadUInt16FromArray(remainingBody, 20);
            this.itemProhibitionFlags = (Enums.ItemProhibitions2)ReusableIO.ReadUInt32FromArray(remainingBody, 22);
            this.weaponAnimation = ReusableIO.ReadStringFromByteArray(remainingBody, 26, CultureConstants.CultureCodeEnglish, 2);
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
            this.inventoryIcon.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 50, CultureConstants.CultureCodeEnglish);
            this.identifyThreshold = ReusableIO.ReadUInt16FromArray(remainingBody, 58);
            this.groundIcon.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 60, CultureConstants.CultureCodeEnglish);
            this.weight = ReusableIO.ReadUInt32FromArray(remainingBody, 68);
            this.descriptionUnidentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 72);
            this.descriptionIdentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 76);
            this.descriptionIcon.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 80, CultureConstants.CultureCodeEnglish);
            this.enchantment = ReusableIO.ReadUInt32FromArray(remainingBody, 88);
            this.offsetAbilities = ReusableIO.ReadUInt32FromArray(remainingBody, 92);
            this.countAbilities = ReusableIO.ReadUInt16FromArray(remainingBody, 96);
            this.offsetAbilityEffects = ReusableIO.ReadUInt32FromArray(remainingBody, 98);
            this.indexEquippedEffects = ReusableIO.ReadUInt16FromArray(remainingBody, 102);
            this.countEquippedEffects = ReusableIO.ReadUInt16FromArray(remainingBody, 104);
            Array.Copy(remainingBody, 106, this.reserved, 0, 16);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteInt32ToStream(this.nameUnidentified.StringReferenceIndex, output);
            ReusableIO.WriteInt32ToStream(this.nameIdentified.StringReferenceIndex, output);
            ReusableIO.WriteStringToStream(this.replacementItem.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream((UInt32)this.itemFlags, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.itemType, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.itemProhibitionFlags, output);
            ReusableIO.WriteStringToStream(this.weaponAnimation, output, CultureConstants.CultureCodeEnglish, false, 2);
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
            ReusableIO.WriteStringToStream(this.inventoryIcon.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt16ToStream(this.identifyThreshold, output);
            ReusableIO.WriteStringToStream(this.groundIcon.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream(this.weight, output);
            ReusableIO.WriteInt32ToStream(this.descriptionUnidentified.StringReferenceIndex, output);
            ReusableIO.WriteInt32ToStream(this.descriptionIdentified.StringReferenceIndex, output);
            ReusableIO.WriteStringToStream(this.descriptionIcon.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream(this.enchantment, output);
            ReusableIO.WriteUInt32ToStream(this.offsetAbilities, output);
            ReusableIO.WriteUInt16ToStream(this.countAbilities, output);
            ReusableIO.WriteUInt32ToStream(this.offsetAbilityEffects, output);
            ReusableIO.WriteUInt16ToStream(this.indexEquippedEffects, output);
            ReusableIO.WriteUInt16ToStream(this.countEquippedEffects, output);
            output.Write(this.reserved, 0, this.reserved.Length);
        }
        #endregion

        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ReturnAndIndent("ITEM Version 2.0 Header:", 0));
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
            builder.Append(StringFormat.ToStringAlignment("Reserved Data"));
            builder.Append(StringFormat.ByteArrayToHexString(this.reserved));

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which ItemUability1 flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetItemProhibitionsString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions2.ClassBarbarian) == ItemProhibitions2.ClassBarbarian, ItemProhibitions2.ClassBarbarian.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions2.ClassBard) == ItemProhibitions2.ClassBard, ItemProhibitions2.ClassBard.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions2.ClassCleric) == ItemProhibitions2.ClassCleric, ItemProhibitions2.ClassCleric.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions2.ClassDruid) == ItemProhibitions2.ClassDruid, ItemProhibitions2.ClassDruid.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions2.ClassFighter) == ItemProhibitions2.ClassFighter, ItemProhibitions2.ClassFighter.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions2.ClassMonk) == ItemProhibitions2.ClassMonk, ItemProhibitions2.ClassMonk.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions2.ClassPaladin) == ItemProhibitions2.ClassPaladin, ItemProhibitions2.ClassPaladin.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions2.ClassRanger) == ItemProhibitions2.ClassRanger, ItemProhibitions2.ClassRanger.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions2.ClassRogue) == ItemProhibitions2.ClassRogue, ItemProhibitions2.ClassRogue.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions2.ClassSorcerer) == ItemProhibitions2.ClassSorcerer, ItemProhibitions2.ClassSorcerer.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions2.ClassWizard) == ItemProhibitions2.ClassWizard, ItemProhibitions2.ClassWizard.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions2.Unknown1) == ItemProhibitions2.Unknown1, ItemProhibitions2.Unknown1.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions2.OrderChaotic) == ItemProhibitions2.OrderChaotic, ItemProhibitions2.OrderChaotic.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions2.NatureEvil) == ItemProhibitions2.NatureEvil, ItemProhibitions2.NatureEvil.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions2.NatureGood) == ItemProhibitions2.NatureGood, ItemProhibitions2.NatureGood.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions2.NatureNeutral) == ItemProhibitions2.NatureNeutral, ItemProhibitions2.NatureNeutral.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions2.OrderLawful) == ItemProhibitions2.OrderLawful, ItemProhibitions2.OrderLawful.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions2.OrderNeutral) == ItemProhibitions2.OrderNeutral, ItemProhibitions2.OrderNeutral.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}