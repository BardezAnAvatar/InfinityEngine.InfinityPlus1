using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item.Item1_1
{
    public class ItemHeader1_1 : ItemHeader
    {
        /// <summary>Binary size of the struct on disk</summary>
        public new const Int32 StructSize = 154;    //here for signature purposes

        #region Members
        /// <summary>This is the resource1 reference to the sund played when the item is placed down in inventory</summary>
        protected ResourceReference dropSound;

        /// <summary>Flags indicating the usability of the item</summary>
        protected ItemProhibitions1_1 itemProhibitionFlags;

        /// <summary>This is the resource1 reference to the sund played when the item is pickd up in inventory</summary>
        protected ResourceReference pickupSound;

        protected ResourceReference dialog;

        protected StringReference conversableLabel;

        protected UInt16 paperdollColor;    //UInt32?

        /// <summary>26 byte -- 24 if paperdoll is UInt32-- reserved array</summary>
        protected Byte[] reserved;
        #endregion

        #region Properties
        /// <summary>This is the resource1 reference to the sund played when the item is placed down in inventory</summary>
        public ResourceReference DropSound
        {
            get { return this.dropSound; }
            set { this.dropSound = value; }
        }

        public ItemProhibitions1_1 ItemProhibitionFlags
        {
            get { return this.itemProhibitionFlags; }
            set { this.itemProhibitionFlags = value; }
        }

        /// <summary>This is the resource1 reference to the sund played when the item is picked up in inventory</summary>
        public ResourceReference PickupSound
        {
            get { return this.pickupSound; }
            set { this.pickupSound = value; }
        }

        public ResourceReference Dialog
        {
            get { return this.dialog; }
            set { this.dialog = value; }
        }

        public StringReference ConversableLabel
        {
            get { return this.conversableLabel; }
            set { this.conversableLabel = value; }
        }

        public UInt16 PaperdollColor
        {
            get { return this.paperdollColor; }
            set { this.paperdollColor = value; }
        }

        public Byte[] Reserved
        {
            get { return this.reserved; }
            set { this.reserved = value; }
        }
        #endregion

        #region Constructor(s)
        public ItemHeader1_1()
        {
            this.dialog = null;
            this.descriptionIdentified = null;
            this.descriptionUnidentified = null;
            this.dropSound = null;
            this.groundIcon = null;
            this.inventoryIcon = null;
            this.nameIdentified = null;
            this.nameUnidentified = null;
            this.pickupSound = null;
            this.conversableLabel = null;
            this.reserved = null;
            
            this.countAbilities = 0;
            this.countEquippedEffects = 0;
        }
        #endregion

        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.dialog = new ResourceReference();
            this.descriptionIdentified = new StringReference();
            this.descriptionUnidentified = new StringReference();
            this.dropSound = new ResourceReference();
            this.groundIcon = new ResourceReference();
            this.inventoryIcon = new ResourceReference();
            this.nameIdentified = new StringReference();
            this.nameUnidentified = new StringReference();
            this.pickupSound = new ResourceReference();
            this.conversableLabel = new StringReference();
            this.reserved = new Byte[26];
        }

        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        public override void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //read remiander
            Byte[] remainingBody = ReusableIO.BinaryRead(input, 146);

            this.nameUnidentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 0);
            this.nameIdentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 4);
            this.dropSound.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 8, CultureConstants.CultureCodeEnglish);
            this.itemFlags = (Enums.ItemFlags)ReusableIO.ReadUInt32FromArray(remainingBody, 16);
            this.itemType = (Enums.ItemType)ReusableIO.ReadUInt16FromArray(remainingBody, 20);
            this.itemProhibitionFlags = (Enums.ItemProhibitions1_1)ReusableIO.ReadUInt32FromArray(remainingBody, 22);
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
            this.pickupSound.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 80, CultureConstants.CultureCodeEnglish);
            this.enchantment = ReusableIO.ReadUInt32FromArray(remainingBody, 88);
            this.offsetAbilities = ReusableIO.ReadUInt32FromArray(remainingBody, 92);
            this.countAbilities = ReusableIO.ReadUInt16FromArray(remainingBody, 96);
            this.offsetAbilityEffects = ReusableIO.ReadUInt32FromArray(remainingBody, 98);
            this.indexEquippedEffects = ReusableIO.ReadUInt16FromArray(remainingBody, 102);
            this.countEquippedEffects = ReusableIO.ReadUInt16FromArray(remainingBody, 104);
            this.dialog.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 106, CultureConstants.CultureCodeEnglish);
            this.conversableLabel.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 114);
            this.paperdollColor = ReusableIO.ReadUInt16FromArray(remainingBody, 118);
            Array.Copy(remainingBody, 120, this.reserved, 0, 26);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteInt32ToStream(this.nameUnidentified.StringReferenceIndex, output);
            ReusableIO.WriteInt32ToStream(this.nameIdentified.StringReferenceIndex, output);
            ReusableIO.WriteStringToStream(this.dropSound.ResRef, output, CultureConstants.CultureCodeEnglish);
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
            ReusableIO.WriteStringToStream(this.pickupSound.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream(this.enchantment, output);
            ReusableIO.WriteUInt32ToStream(this.offsetAbilities, output);
            ReusableIO.WriteUInt16ToStream(this.countAbilities, output);
            ReusableIO.WriteUInt32ToStream(this.offsetAbilityEffects, output);
            ReusableIO.WriteUInt16ToStream(this.indexEquippedEffects, output);
            ReusableIO.WriteUInt16ToStream(this.countEquippedEffects, output);
            ReusableIO.WriteStringToStream(this.dialog.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.conversableLabel.StringReferenceIndex, output);
            ReusableIO.WriteUInt16ToStream(this.paperdollColor, output);
            output.Write(this.reserved, 0, this.reserved.Length);
        }
        #endregion

        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("ITEM Version 1.1 Header:");
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(String.Format("'{0}'", this.signature));
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(String.Format("'{0}'", this.version));
            builder.Append(StringFormat.ToStringAlignment("Unidentified Name StrRef"));
            builder.Append(this.nameUnidentified.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Identified Name StrRef"));
            builder.Append(this.nameIdentified.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Dropping sound"));
            builder.Append(String.Format("'{0}'", this.dropSound.ZResRef));
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
            builder.Append(StringFormat.ToStringAlignment("Pickup sound"));
            builder.Append(String.Format("'{0}'", this.pickupSound.ZResRef));
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
            builder.Append(StringFormat.ToStringAlignment("Dialog"));
            builder.Append(String.Format("'{0}'", this.dialog.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Conversable label StrRef"));
            builder.Append(this.conversableLabel.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Paperdoll color"));
            builder.Append(this.paperdollColor);
            builder.Append(StringFormat.ToStringAlignment("Reserved Data"));
            builder.Append(StringFormat.ByteArrayToHexString(this.reserved));

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which ItemUability1 flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetItemProhibitionsString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.OrderChaotic) == ItemProhibitions1_1.OrderChaotic, ItemProhibitions1_1.OrderChaotic.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.NatureEvil) == ItemProhibitions1_1.NatureEvil, ItemProhibitions1_1.NatureEvil.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.NatureGood) == ItemProhibitions1_1.NatureGood, ItemProhibitions1_1.NatureGood.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.NatureNeutral) == ItemProhibitions1_1.NatureNeutral, ItemProhibitions1_1.NatureNeutral.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.OrderLawful) == ItemProhibitions1_1.OrderLawful, ItemProhibitions1_1.OrderLawful.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.OrderNeutral) == ItemProhibitions1_1.OrderNeutral, ItemProhibitions1_1.OrderNeutral.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.FactionSensate) == ItemProhibitions1_1.FactionSensate, ItemProhibitions1_1.FactionSensate.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.ClassCleric) == ItemProhibitions1_1.ClassCleric, ItemProhibitions1_1.ClassCleric.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.FactionGodsmen) == ItemProhibitions1_1.FactionGodsmen, ItemProhibitions1_1.FactionGodsmen.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.FactionAnarchist) == ItemProhibitions1_1.FactionAnarchist, ItemProhibitions1_1.FactionAnarchist.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.FactionChaosmen) == ItemProhibitions1_1.FactionChaosmen, ItemProhibitions1_1.FactionChaosmen.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.ClassFighter) == ItemProhibitions1_1.ClassFighter, ItemProhibitions1_1.ClassFighter.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.FactionNone) == ItemProhibitions1_1.FactionNone, ItemProhibitions1_1.FactionNone.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.ClassFighterMage) == ItemProhibitions1_1.ClassFighterMage, ItemProhibitions1_1.ClassFighterMage.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.FactionDustmen) == ItemProhibitions1_1.FactionDustmen, ItemProhibitions1_1.FactionDustmen.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.FactionMercyKillers) == ItemProhibitions1_1.FactionMercyKillers, ItemProhibitions1_1.FactionMercyKillers.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.FactionIndependants) == ItemProhibitions1_1.FactionIndependants, ItemProhibitions1_1.FactionIndependants.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.ClassFighterThief) == ItemProhibitions1_1.ClassFighterThief, ItemProhibitions1_1.ClassFighterThief.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.ClassMage) == ItemProhibitions1_1.ClassMage, ItemProhibitions1_1.ClassMage.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.ClassMageThief) == ItemProhibitions1_1.ClassMageThief, ItemProhibitions1_1.ClassMageThief.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.CharacterDakkon) == ItemProhibitions1_1.CharacterDakkon, ItemProhibitions1_1.CharacterDakkon.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.CharacterFallFromGrace) == ItemProhibitions1_1.CharacterFallFromGrace, ItemProhibitions1_1.CharacterFallFromGrace.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.ClassThief) == ItemProhibitions1_1.ClassThief, ItemProhibitions1_1.ClassThief.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.CharacterVhailor) == ItemProhibitions1_1.CharacterVhailor, ItemProhibitions1_1.CharacterVhailor.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.CharacterIgnus) == ItemProhibitions1_1.CharacterIgnus, ItemProhibitions1_1.CharacterIgnus.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.CharacterMorte) == ItemProhibitions1_1.CharacterMorte, ItemProhibitions1_1.CharacterMorte.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.CharacterNordom) == ItemProhibitions1_1.CharacterNordom, ItemProhibitions1_1.CharacterNordom.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.Unknown1) == ItemProhibitions1_1.Unknown1, ItemProhibitions1_1.Unknown1.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.CharacterAnnah) == ItemProhibitions1_1.CharacterAnnah, ItemProhibitions1_1.CharacterAnnah.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.Unknown2) == ItemProhibitions1_1.Unknown2, ItemProhibitions1_1.Unknown2.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.CharacterNamelessOne) == ItemProhibitions1_1.CharacterNamelessOne, ItemProhibitions1_1.CharacterNamelessOne.GetDescription());
            StringFormat.AppendSubItem(sb, (this.itemProhibitionFlags & ItemProhibitions1_1.Unknown3) == ItemProhibitions1_1.Unknown3, ItemProhibitions1_1.Unknown3.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}