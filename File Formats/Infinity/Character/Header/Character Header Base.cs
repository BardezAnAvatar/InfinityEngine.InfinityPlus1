using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Character.Header
{
    /// <summary>Character header base class</summary>
    public abstract class CharacterHeaderBase : InfinityFormat
    {
        #region Members
        /// <summary>Offset to the creature structure</summary>
        protected UInt32 offsetCreature;

        /// <summary>Length of the creature structure</summary>
        protected UInt32 lengthCreature;
        
        /// <summary>Index into SLOTS.IDS for Quick Weapon 1</summary>
        /// <value>(0xFFFF = none)</value>
        protected UInt16 quickWeaponSlotIndex1;
        
        /// <summary>Index into SLOTS.IDS for Quick Weapon 2</summary>
        /// <value>(0xFFFF = none)</value>
        protected UInt16 quickWeaponSlotIndex2;
        
        /// <summary>Index into SLOTS.IDS for Quick Weapon 3</summary>
        /// <value>(0xFFFF = none)</value>
        protected UInt16 quickWeaponSlotIndex3;
        
        /// <summary>Index into SLOTS.IDS for Quick Weapon 4</summary>
        /// <value>(0xFFFF = none)</value>
        protected UInt16 quickWeaponSlotIndex4;

        /// <summary>Resource reference to first quick spell</summary>
        protected ResourceReference quickSpell1;
        
        /// <summary>Resource reference to second quick spell</summary>
        protected ResourceReference quickSpell2;
        
        /// <summary>Resource reference to third quick spell</summary>
        protected ResourceReference quickSpell3;
        
        /// <summary>Index into SLOTS.IDS for Quick Item 1</summary>
        /// <value>(0xFFFF = none)</value>
        protected Int16 quickItemSlotIndex1;
        
        /// <summary>Index into SLOTS.IDS for Quick Item 2</summary>
        /// <value>(0xFFFF = none)</value>
        protected Int16 quickItemSlotIndex2;
        
        /// <summary>Index into SLOTS.IDS for Quick Item 3</summary>
        /// <value>(0xFFFF = none)</value>
        protected Int16 quickItemSlotIndex3;
        #endregion

        #region Properties
        /// <summary>Character name, up to 32 bytes</summary>
        public ZString Name { get; set; }

        /// <summary>Offset to the creature structure</summary>
        public UInt32 OffsetCreature
        {
            get { return this.offsetCreature; }
            set { this.offsetCreature = value; }
        }

        /// <summary>Length of the creature structure</summary>
        public UInt32 LengthCreature
        {
            get { return this.lengthCreature; }
            set { this.lengthCreature = value; }
        }
        
        /// <summary>Index into SLOTS.IDS for Quick Weapon 1</summary>
        /// <value>(0xFFFF = none)</value>
        public UInt16 QuickWeaponSlotIndex1
        {
            get { return this.quickWeaponSlotIndex1; }
            set { this.quickWeaponSlotIndex1 = value; }
        }
        
        /// <summary>Index into SLOTS.IDS for Quick Weapon 2</summary>
        /// <value>(0xFFFF = none)</value>
        public UInt16 QuickWeaponSlotIndex2
        {
            get { return this.quickWeaponSlotIndex2; }
            set { this.quickWeaponSlotIndex2 = value; }
        }
        
        /// <summary>Index into SLOTS.IDS for Quick Weapon 3</summary>
        /// <value>(0xFFFF = none)</value>
        public UInt16 QuickWeaponSlotIndex3
        {
            get { return this.quickWeaponSlotIndex3; }
            set { this.quickWeaponSlotIndex3 = value; }
        }
        
        /// <summary>Index into SLOTS.IDS for Quick Weapon 4</summary>
        /// <value>(0xFFFF = none)</value>
        public UInt16 QuickWeaponSlotIndex4
        {
            get { return this.quickWeaponSlotIndex4; }
            set { this.quickWeaponSlotIndex4 = value; }
        }
        
        /// <summary>Resource reference to first quick spell</summary>
        public ResourceReference QuickSpell1
        {
            get { return this.quickSpell1; }
            set { this.quickSpell1 = value; }
        }
        
        /// <summary>Resource reference to second quick spell</summary>
        public ResourceReference QuickSpell2
        {
            get { return this.quickSpell2; }
            set { this.quickSpell2 = value; }
        }
        
        /// <summary>Resource reference to third quick spell</summary>
        public ResourceReference QuickSpell3
        {
            get { return this.quickSpell3; }
            set { this.quickSpell3 = value; }
        }
        
        /// <summary>Index into SLOTS.IDS for Quick Item 1</summary>
        /// <value>(0xFFFF = none)</value>
        public Int16 QuickItemSlotIndex1
        {
            get { return this.quickItemSlotIndex1; }
            set { this.quickItemSlotIndex1 = value; }
        }
        
        /// <summary>Index into SLOTS.IDS for Quick Item 2</summary>
        /// <value>(0xFFFF = none)</value>
        public Int16 QuickItemSlotIndex2
        {
            get { return this.quickItemSlotIndex2; }
            set { this.quickItemSlotIndex2 = value; }
        }
        
        /// <summary>Index into SLOTS.IDS for Quick Item 3</summary>
        /// <value>(0xFFFF = none)</value>
        public Int16 QuickItemSlotIndex3
        {
            get { return this.quickItemSlotIndex3; }
            set { this.quickItemSlotIndex3 = value; }
        }
        #endregion
        
        #region Constructor(s)
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.quickSpell1 = new ResourceReference();
            this.quickSpell2 = new ResourceReference();
            this.quickSpell3 = new ResourceReference();
            this.Name = new ZString();
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
        protected abstract String GetStringRepresentation();
        #endregion
    }
}