using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Character.Header
{
    /// <summary>Character header version 1</summary>
    public class Character1Header : CharacterHeaderBase
    {
        /// <summary>Binary size of the struct on disk</summary>
        public Int32 StructSize
        {
            get { return  0x64; }
        }

        #region Members
        /// <summary>Field indicating whether or not to show the associated quickslot, evaluated as a Boolean</summary>
        protected UInt16 showQuickWeaponSlot1;
        
        /// <summary>Field indicating whether or not to show the associated quickslot, evaluated as a Boolean</summary>
        protected UInt16 showQuickWeaponSlot2;
        
        /// <summary>Field indicating whether or not to show the associated quickslot, evaluated as a Boolean</summary>
        protected UInt16 showQuickWeaponSlot3;
        
        /// <summary>Field indicating whether or not to show the associated quickslot, evaluated as a Boolean</summary>
        protected UInt16 showQuickWeaponSlot4;
        
        /// <summary>Field indicating whether or not to show the associated quickslot, evaluated as a Boolean</summary>
        protected UInt16 showQuickItemSlot1;
        
        /// <summary>Field indicating whether or not to show the associated quickslot, evaluated as a Boolean</summary>
        protected UInt16 showQuickItemSlot2;
        
        /// <summary>Field indicating whether or not to show the associated quickslot, evaluated as a Boolean</summary>
        protected UInt16 showQuickItemSlot3;
        #endregion

        #region Properties
        /// <summary>Field indicating whether or not to show the associated quickslot, evaluated as a Boolean</summary>
        public UInt16 ShowQuickWeaponSlot1
        {
            get { return this.showQuickWeaponSlot1; }
            set { this.showQuickWeaponSlot1 = value; }
        }
        
        /// <summary>Field indicating whether or not to show the associated quickslot, evaluated as a Boolean</summary>
        public UInt16 ShowQuickWeaponSlot2
        {
            get { return this.showQuickWeaponSlot2; }
            set { this.showQuickWeaponSlot2 = value; }
        }
        
        /// <summary>Field indicating whether or not to show the associated quickslot, evaluated as a Boolean</summary>
        public UInt16 ShowQuickWeaponSlot3
        {
            get { return this.showQuickWeaponSlot3; }
            set { this.showQuickWeaponSlot3 = value; }
        }
        
        /// <summary>Field indicating whether or not to show the associated quickslot, evaluated as a Boolean</summary>
        public UInt16 ShowQuickWeaponSlot4
        {
            get { return this.showQuickWeaponSlot4; }
            set { this.showQuickWeaponSlot4 = value; }
        }

        /// <summary>Field indicating whether or not to show the associated quickslot, evaluated as a Boolean</summary>
        public UInt16 ShowQuickItemSlot1
        {
            get { return this.showQuickItemSlot1; }
            set { this.showQuickItemSlot1 = value; }
        }
        
        /// <summary>Field indicating whether or not to show the associated quickslot, evaluated as a Boolean</summary>
        public UInt16 ShowQuickItemSlot2
        {
            get { return this.showQuickItemSlot2; }
            set { this.showQuickItemSlot2 = value; }
        }
        
        /// <summary>Field indicating whether or not to show the associated quickslot, evaluated as a Boolean</summary>
        public UInt16 ShowQuickItemSlot3
        {
            get { return this.showQuickItemSlot3; }
            set { this.showQuickItemSlot3 = value; }
        }
        #endregion
        
        #region Constructor(s)
        /// <summary>Default constructor</summary>
        public Character1Header()
        {
            this.quickSpell1 = null;
            this.quickSpell2 = null;
            this.quickSpell3 = null;
        }
        #endregion

        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //read remiander
            Byte[] header = ReusableIO.BinaryRead(input, this.StructSize - 8);

            this.Name.Value = ReusableIO.ReadStringFromByteArray(header, 0, CultureConstants.CultureCodeEnglish, 32);
            this.offsetCreature = ReusableIO.ReadUInt32FromArray(header, 32);
            this.lengthCreature = ReusableIO.ReadUInt32FromArray(header, 36);
            this.quickWeaponSlotIndex1 = ReusableIO.ReadUInt16FromArray(header, 40);
            this.quickWeaponSlotIndex2 = ReusableIO.ReadUInt16FromArray(header, 42);
            this.quickWeaponSlotIndex3 = ReusableIO.ReadUInt16FromArray(header, 44);
            this.quickWeaponSlotIndex4 = ReusableIO.ReadUInt16FromArray(header, 46);
            this.showQuickWeaponSlot1 = ReusableIO.ReadUInt16FromArray(header, 48);
            this.showQuickWeaponSlot2 = ReusableIO.ReadUInt16FromArray(header, 50);
            this.showQuickWeaponSlot3 = ReusableIO.ReadUInt16FromArray(header, 52);
            this.showQuickWeaponSlot4 = ReusableIO.ReadUInt16FromArray(header, 54);
            this.quickSpell1.ResRef = ReusableIO.ReadStringFromByteArray(header, 56, CultureConstants.CultureCodeEnglish);
            this.quickSpell2.ResRef = ReusableIO.ReadStringFromByteArray(header, 64, CultureConstants.CultureCodeEnglish);
            this.quickSpell3.ResRef = ReusableIO.ReadStringFromByteArray(header, 72, CultureConstants.CultureCodeEnglish);
            this.quickItemSlotIndex1 = ReusableIO.ReadInt16FromArray(header, 80);
            this.quickItemSlotIndex2 = ReusableIO.ReadInt16FromArray(header, 82);
            this.quickItemSlotIndex3 = ReusableIO.ReadInt16FromArray(header, 84);
            this.showQuickItemSlot1 = ReusableIO.ReadUInt16FromArray(header, 86);
            this.showQuickItemSlot2 = ReusableIO.ReadUInt16FromArray(header, 88);
            this.showQuickItemSlot3 = ReusableIO.ReadUInt16FromArray(header, 90);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.Name.Value, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt32ToStream(this.offsetCreature, output);
            ReusableIO.WriteUInt32ToStream(this.lengthCreature, output);
            ReusableIO.WriteUInt16ToStream(this.quickWeaponSlotIndex1, output);
            ReusableIO.WriteUInt16ToStream(this.quickWeaponSlotIndex2, output);
            ReusableIO.WriteUInt16ToStream(this.quickWeaponSlotIndex3, output);
            ReusableIO.WriteUInt16ToStream(this.quickWeaponSlotIndex4, output);
            ReusableIO.WriteUInt16ToStream(this.showQuickWeaponSlot1, output);
            ReusableIO.WriteUInt16ToStream(this.showQuickWeaponSlot2, output);
            ReusableIO.WriteUInt16ToStream(this.showQuickWeaponSlot3, output);
            ReusableIO.WriteUInt16ToStream(this.showQuickWeaponSlot4, output);
            ReusableIO.WriteStringToStream(this.quickSpell1.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickSpell2.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickSpell3.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt16ToStream(this.quickItemSlotIndex1, output);
            ReusableIO.WriteInt16ToStream(this.quickItemSlotIndex2, output);
            ReusableIO.WriteInt16ToStream(this.quickItemSlotIndex3, output);
            ReusableIO.WriteUInt16ToStream(this.showQuickItemSlot1, output);
            ReusableIO.WriteUInt16ToStream(this.showQuickItemSlot2, output);
            ReusableIO.WriteUInt16ToStream(this.showQuickItemSlot3, output);
        }
        #endregion

        #region ToString() helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            return this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(this.signature);
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(this.version);
            builder.Append(StringFormat.ToStringAlignment("Name"));
            builder.Append(this.Name.Value);
            builder.Append(StringFormat.ToStringAlignment("Creature offset"));
            builder.Append(this.offsetCreature);
            builder.Append(StringFormat.ToStringAlignment("Creature length"));
            builder.Append(this.lengthCreature);
            builder.Append(StringFormat.ToStringAlignment("Quick Weapon Index 1"));
            builder.Append(this.quickWeaponSlotIndex1);
            builder.Append(StringFormat.ToStringAlignment("Quick Weapon Index 2"));
            builder.Append(this.quickWeaponSlotIndex2);
            builder.Append(StringFormat.ToStringAlignment("Quick Weapon Index 3"));
            builder.Append(this.quickWeaponSlotIndex3);
            builder.Append(StringFormat.ToStringAlignment("Quick Weapon Index 4"));
            builder.Append(this.quickWeaponSlotIndex4);
            builder.Append(StringFormat.ToStringAlignment("Show quick weapon slot 1"));
            builder.Append(this.showQuickWeaponSlot1);
            builder.Append(StringFormat.ToStringAlignment("Show quick weapon slot 2"));
            builder.Append(this.showQuickWeaponSlot2);
            builder.Append(StringFormat.ToStringAlignment("Show quick weapon slot 3"));
            builder.Append(this.showQuickWeaponSlot3);
            builder.Append(StringFormat.ToStringAlignment("Show quick weapon slot 4"));
            builder.Append(this.showQuickWeaponSlot4);
            builder.Append(StringFormat.ToStringAlignment("Quick spell ResRef 1"));
            builder.Append("'");
            builder.Append(this.quickSpell1.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick spell ResRef 2"));
            builder.Append("'");
            builder.Append(this.quickSpell2.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick spell ResRef 3"));
            builder.Append("'");
            builder.Append(this.quickSpell3.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick item index 1"));
            builder.Append(this.quickItemSlotIndex1);
            builder.Append(StringFormat.ToStringAlignment("Quick item index 2"));
            builder.Append(this.quickItemSlotIndex2);
            builder.Append(StringFormat.ToStringAlignment("Quick item index 3"));
            builder.Append(this.quickItemSlotIndex3);
            builder.Append(StringFormat.ToStringAlignment("Show quick item 1"));
            builder.Append(this.showQuickItemSlot1);
            builder.Append(StringFormat.ToStringAlignment("Show quick item 2"));
            builder.Append(this.showQuickItemSlot2);
            builder.Append(StringFormat.ToStringAlignment("Show quick item 3"));
            builder.Append(this.showQuickItemSlot3);

            return builder.ToString();
        }
        #endregion
    }
}