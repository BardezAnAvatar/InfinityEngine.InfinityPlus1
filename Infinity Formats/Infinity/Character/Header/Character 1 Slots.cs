using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Character.Header
{
    /// <summary>Represents the slot availability for Characters in {Baldur's Gate 1&2, IWD} CHR and GAM files</summary>
    public class CharacterSlots_v1 : CharacterSlotsBase
    {
        #region Constants
        /// <summary>Binary size of the struct on disk</summary>
        private const Int32 StructSize = 52;
        #endregion


        #region Fields
        /// <summary>Represents an array of indeces into Quick Weapons indicating whether or not to show the associated quickslot, evaluated as a Boolean</summary>
        public UInt16[] ShowQuickWeaponsSlot { get; set; }

        /// <summary>Represents an array of indeces into Quick Items indicating whether or not to show the associated quickslot, evaluated as a Boolean</summary>
        public UInt16[] ShowQuickItemsSlot { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            base.Initialize();

            this.ShowQuickWeaponsSlot = new UInt16[this.QuickWeaponSlotCount];
            this.ShowQuickItemsSlot = new UInt16[this.QuickItemSlotCount];
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //read remiander
            Byte[] slots = ReusableIO.BinaryRead(input, CharacterSlots_v1.StructSize);

            for (Int32 i = 0; i < this.QuickWeaponSlotCount; ++i)
                this.QuickWeaponSlotIndeces[i] = ReusableIO.ReadInt16FromArray(slots, (i * 2));

            for (Int32 i = 0; i < this.QuickWeaponSlotCount; ++i)
                this.ShowQuickWeaponsSlot[i] = ReusableIO.ReadUInt16FromArray(slots, 8 + (i * 2));

            for (Int32 i = 0; i < this.QuickSpellSlotCount; ++i)
                this.QuickSpellSlotIndeces[i].ResRef = ReusableIO.ReadStringFromByteArray(slots, 16 + (i * 8), CultureConstants.CultureCodeEnglish);

            for (Int32 i = 0; i < this.QuickItemSlotCount; ++i)
                this.QuickItemSlotIndeces[i] = ReusableIO.ReadInt16FromArray(slots, 40 + (i * 2));

            for (Int32 i = 0; i < this.QuickItemSlotCount; ++i)
                this.ShowQuickItemsSlot[i] = ReusableIO.ReadUInt16FromArray(slots, 46 + (i * 2));
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public override void Write(Stream output)
        {
            for (Int32 i = 0; i < this.QuickWeaponSlotCount; ++i)
                ReusableIO.WriteInt16ToStream(this.QuickWeaponSlotIndeces[i], output);

            for (Int32 i = 0; i < this.QuickWeaponSlotCount; ++i)
                ReusableIO.WriteUInt16ToStream(this.ShowQuickWeaponsSlot[i], output);

            for (Int32 i = 0; i < this.QuickSpellSlotCount; ++i)
                ReusableIO.WriteStringToStream(this.QuickSpellSlotIndeces[i].ResRef, output, CultureConstants.CultureCodeEnglish);

            for (Int32 i = 0; i < this.QuickItemSlotCount; ++i)
                ReusableIO.WriteInt16ToStream(this.QuickItemSlotIndeces[i], output);

            for (Int32 i = 0; i < this.QuickItemSlotCount; ++i)
                ReusableIO.WriteUInt16ToStream(this.ShowQuickItemsSlot[i], output);
        }
        #endregion


        #region ToString() helpers
        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            for (Int32 i = 0; i < this.QuickWeaponSlotCount; ++i)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Quick Weapon Index {0}", i)));
                builder.Append(this.QuickWeaponSlotIndeces[i]);
            }

            for (Int32 i = 0; i < this.QuickWeaponSlotCount; ++i)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Show quick weapon slot {0}", i)));
                builder.Append(this.ShowQuickWeaponsSlot[i]);
            }

            for (Int32 i = 0; i < this.QuickSpellSlotCount; ++i)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Quick spell ResRef {0}", i)));
                builder.Append(String.Format("'{0}'", this.QuickSpellSlotIndeces[i].ZResRef));
            }

            for (Int32 i = 0; i < this.QuickItemSlotCount; ++i)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Quick item index {0}", i)));
                builder.Append(this.QuickItemSlotIndeces[i]);
            }

            for (Int32 i = 0; i < this.QuickItemSlotCount; ++i)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Show quick item {0}", i)));
                builder.Append(this.ShowQuickItemsSlot[i]);
            }

            return builder.ToString();
        }
        #endregion
    }
}