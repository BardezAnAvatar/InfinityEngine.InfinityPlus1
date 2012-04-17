using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Character.Header
{
    /// <summary>Represents the slot availability for Characters in {PS:T} CHR and GAM files</summary>
    public class CharacterSlots_Torment : CharacterSlots_v1
    {
        #region Constants
        /// <summary>Binary size of the struct on disk</summary>
        private const Int32 StructSize = 60;
        #endregion


        #region Quasi-constants
        /// <summary>Exposes the count of Quick Item slots</summary>
        protected override Int32 QuickItemSlotCount
        {
            get { return 5; }
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
            Byte[] slots = ReusableIO.BinaryRead(input, CharacterSlots_Torment.StructSize);

            for (Int32 i = 0; i < this.QuickWeaponSlotCount; ++i)
                this.QuickWeaponSlotIndeces[i] = ReusableIO.ReadInt16FromArray(slots, (i * 2));

            for (Int32 i = 0; i < this.QuickWeaponSlotCount; ++i)
                this.ShowQuickWeaponsSlot[i] = ReusableIO.ReadUInt16FromArray(slots, 8 + (i * 2));

            for (Int32 i = 0; i < this.QuickSpellSlotCount; ++i)
                this.QuickSpellSlotIndeces[i].ResRef = ReusableIO.ReadStringFromByteArray(slots, 16 + (i * 8), CultureConstants.CultureCodeEnglish);

            for (Int32 i = 0; i < this.QuickItemSlotCount; ++i)
                this.QuickItemSlotIndeces[i] = ReusableIO.ReadInt16FromArray(slots, 40 + (i * 2));

            for (Int32 i = 0; i < this.QuickItemSlotCount; ++i)
                this.ShowQuickItemsSlot[i] = ReusableIO.ReadUInt16FromArray(slots, 50 + (i * 2));
        }
        #endregion
    }
}