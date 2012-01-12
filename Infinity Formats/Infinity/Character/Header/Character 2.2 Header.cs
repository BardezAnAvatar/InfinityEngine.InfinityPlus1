using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Character.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Character.Header
{
    /// <summary>Character header version 2.2</summary>
    public class Character2_2Header : CharacterHeaderBase
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 0x224;

        #region Members
        /// <summary>Index into SLOTS.IDS for Quick Shield 1</summary>
        /// <value>(0xFFFF = none)</value>
        protected UInt16 quickShieldSlotIndex1;
        
        /// <summary>Index into SLOTS.IDS for Quick Shield 2</summary>
        /// <value>(0xFFFF = none)</value>
        protected UInt16 quickShieldSlotIndex2;
        
        /// <summary>Index into SLOTS.IDS for Quick Shield 3</summary>
        /// <value>(0xFFFF = none)</value>
        protected UInt16 quickShieldSlotIndex3;
        
        /// <summary>Index into SLOTS.IDS for Quick Shield 4</summary>
        /// <value>(0xFFFF = none)</value>
        protected UInt16 quickShieldSlotIndex4;

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        protected Int16 usableQuickWeaponSlot1;

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        protected Int16 usableQuickWeaponSlot2;

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        protected Int16 usableQuickWeaponSlot3;

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        protected Int16 usableQuickWeaponSlot4;

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        protected Int16 usableQuickShieldSlot1;

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        protected Int16 usableQuickShieldSlot2;

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        protected Int16 usableQuickShieldSlot3;

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        protected Int16 usableQuickShieldSlot4;
        
        /// <summary>Resource reference to fourth quick spell</summary>
        protected ResourceReference quickSpell4;

        /// <summary>Resource reference to fifth quick spell</summary>
        protected ResourceReference quickSpell5;

        /// <summary>Resource reference to sixth quick spell</summary>
        protected ResourceReference quickSpell6;

        /// <summary>Resource reference to seventh quick spell</summary>
        protected ResourceReference quickSpell7;

        /// <summary>Resource reference to eighth quick spell</summary>
        protected ResourceReference quickSpell8;

        /// <summary>Resource reference to ninth quick spell</summary>
        protected ResourceReference quickSpell9;

        /// <summary>Field indicating the class associated with the quick spell slot</summary>
        protected Byte classQuickSpell1;

        /// <summary>Field indicating the class associated with the quick spell slot</summary>
        protected Byte classQuickSpell2;

        /// <summary>Field indicating the class associated with the quick spell slot</summary>
        protected Byte classQuickSpell3;

        /// <summary>Field indicating the class associated with the quick spell slot</summary>
        protected Byte classQuickSpell4;

        /// <summary>Field indicating the class associated with the quick spell slot</summary>
        protected Byte classQuickSpell5;

        /// <summary>Field indicating the class associated with the quick spell slot</summary>
        protected Byte classQuickSpell6;

        /// <summary>Field indicating the class associated with the quick spell slot</summary>
        protected Byte classQuickSpell7;

        /// <summary>Field indicating the class associated with the quick spell slot</summary>
        protected Byte classQuickSpell8;

        /// <summary>Field indicating the class associated with the quick spell slot</summary>
        protected Byte classQuickSpell9;

        /// <summary>First unknown byte, probably unused</summary>
        protected Byte unknown1;

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        protected Int16 usableQuickItemSlot1;

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        protected Int16 usableQuickItemSlot2;

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        protected Int16 usableQuickItemSlot3;

        /// <summary>Resource reference to first quick Innate ability</summary>
        protected ResourceReference quickInnate1;

        /// <summary>Resource reference to second quick Innate ability</summary>
        protected ResourceReference quickInnate2;

        /// <summary>Resource reference to third quick Innate ability</summary>
        protected ResourceReference quickInnate3;
        
        /// <summary>Resource reference to fourth quick Innate ability</summary>
        protected ResourceReference quickInnate4;

        /// <summary>Resource reference to fifth quick Innate ability</summary>
        protected ResourceReference quickInnate5;

        /// <summary>Resource reference to sixth quick Innate ability</summary>
        protected ResourceReference quickInnate6;

        /// <summary>Resource reference to seventh quick Innate ability</summary>
        protected ResourceReference quickInnate7;

        /// <summary>Resource reference to eighth quick Innate ability</summary>
        protected ResourceReference quickInnate8;

        /// <summary>Resource reference to ninth quick Innate ability</summary>
        protected ResourceReference quickInnate9;

        /// <summary>Resource reference to first quick Song</summary>
        protected ResourceReference quickSong1;

        /// <summary>Resource reference to second quick Song</summary>
        protected ResourceReference quickSong2;

        /// <summary>Resource reference to third quick Song</summary>
        protected ResourceReference quickSong3;
        
        /// <summary>Resource reference to fourth quick Song</summary>
        protected ResourceReference quickSong4;

        /// <summary>Resource reference to fifth quick Song</summary>
        protected ResourceReference quickSong5;

        /// <summary>Resource reference to sixth quick Song</summary>
        protected ResourceReference quickSong6;

        /// <summary>Resource reference to seventh quick Song</summary>
        protected ResourceReference quickSong7;

        /// <summary>Resource reference to eighth quick Song</summary>
        protected ResourceReference quickSong8;

        /// <summary>Resource reference to ninth quick Song</summary>
        protected ResourceReference quickSong9;

        /// <summary>Current button action assigned to associated slot</summary>
        protected UserInterfaceButton button1;

        /// <summary>Current button action assigned to associated slot</summary>
        protected UserInterfaceButton button2;

        /// <summary>Current button action assigned to associated slot</summary>
        protected UserInterfaceButton button3;

        /// <summary>Current button action assigned to associated slot</summary>
        protected UserInterfaceButton button4;

        /// <summary>Current button action assigned to associated slot</summary>
        protected UserInterfaceButton button5;

        /// <summary>Current button action assigned to associated slot</summary>
        protected UserInterfaceButton button6;

        /// <summary>Current button action assigned to associated slot</summary>
        protected UserInterfaceButton button7;

        /// <summary>Current button action assigned to associated slot</summary>
        protected UserInterfaceButton button8;

        /// <summary>Current button action assigned to associated slot</summary>
        protected UserInterfaceButton button9;

        /// <summary>Unknown 2 Bytes after configurable slots</summary>
        protected UInt16 unknown2;

        /// <summary>Level of the associated spell slot</summary>
        protected Byte levelSpell1;

        /// <summary>Level of the associated spell slot</summary>
        protected Byte levelSpell2;

        /// <summary>Level of the associated spell slot</summary>
        protected Byte levelSpell3;

        /// <summary>Level of the associated spell slot</summary>
        protected Byte levelSpell4;

        /// <summary>Level of the associated spell slot</summary>
        protected Byte levelSpell5;

        /// <summary>Level of the associated spell slot</summary>
        protected Byte levelSpell6;

        /// <summary>Level of the associated spell slot</summary>
        protected Byte levelSpell7;

        /// <summary>Level of the associated spell slot</summary>
        protected Byte levelSpell8;

        /// <summary>Level of the associated spell slot</summary>
        protected Byte levelSpell9;

        /// <summary>Third unknown byte after spell slot levels</summary>
        /// <remarks>Probably unused</remarks>
        protected Byte unknown3;

        /// <summary>14 unknown bytes</summary>
        protected Byte[] unknown4;

        /// <summary>Soundset resource reference</summary>
        protected ResourceReference soundSet;

        /// <summary>128 reserved bytes. Probably unused.</summary>
        protected Byte[] reserved;
        #endregion

        #region Properties
        /// <summary>Index into SLOTS.IDS for Quick Shield 1</summary>
        /// <value>(0xFFFF = none)</value>
        public UInt16 QuickShieldSlotIndex1
        {
            get { return this.quickShieldSlotIndex1; }
            set { this.quickShieldSlotIndex1 = value; }
        }

        /// <summary>Index into SLOTS.IDS for Quick Shield 2</summary>
        /// <value>(0xFFFF = none)</value>
        public UInt16 QuickShieldSlotIndex2
        {
            get { return this.quickShieldSlotIndex2; }
            set { this.quickShieldSlotIndex2 = value; }
        }

        /// <summary>Index into SLOTS.IDS for Quick Shield 3</summary>
        /// <value>(0xFFFF = none)</value>
        public UInt16 QuickShieldSlotIndex3
        {
            get { return this.quickShieldSlotIndex3; }
            set { this.quickShieldSlotIndex3 = value; }
        }

        /// <summary>Index into SLOTS.IDS for Quick Shield 4</summary>
        /// <value>(0xFFFF = none)</value>
        public UInt16 QuickShieldSlotIndex4
        {
            get { return this.quickShieldSlotIndex4; }
            set { this.quickShieldSlotIndex4 = value; }
        }

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        public Int16 UsableQuickWeaponSlot1
        {
            get { return this.usableQuickWeaponSlot1; }
            set { this.usableQuickWeaponSlot1 = value; }
        }

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        public Int16 UsableQuickWeaponSlot2
        {
            get { return this.usableQuickWeaponSlot2; }
            set { this.usableQuickWeaponSlot2 = value; }
        }

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        public Int16 UsableQuickWeaponSlot3
        {
            get { return this.usableQuickWeaponSlot3; }
            set { this.usableQuickWeaponSlot3 = value; }
        }

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        public Int16 UsableQuickWeaponSlot4
        {
            get { return this.usableQuickWeaponSlot4; }
            set { this.usableQuickWeaponSlot4 = value; }
        }

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        public Int16 UsableQuickShieldSlot1
        {
            get { return this.usableQuickShieldSlot1; }
            set { this.usableQuickShieldSlot1 = value; }
        }

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        public Int16 UsableQuickShieldSlot2
        {
            get { return this.usableQuickShieldSlot2; }
            set { this.usableQuickShieldSlot2 = value; }
        }

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        public Int16 UsableQuickShieldSlot3
        {
            get { return this.usableQuickShieldSlot3; }
            set { this.usableQuickShieldSlot3 = value; }
        }

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        public Int16 UsableQuickShieldSlot4
        {
            get { return this.usableQuickShieldSlot4; }
            set { this.usableQuickShieldSlot4 = value; }
        }

        /// <summary>Resource reference to fourth quick spell</summary>
        public ResourceReference QuickSpell4
        {
            get { return this.quickSpell4; }
            set { this.quickSpell4 = value; }
        }

        /// <summary>Resource reference to fifth quick spell</summary>
        public ResourceReference QuickSpell5
        {
            get { return this.quickSpell5; }
            set { this.quickSpell5 = value; }
        }

        /// <summary>Resource reference to sixth quick spell</summary>
        public ResourceReference QuickSpell6
        {
            get { return this.quickSpell6; }
            set { this.quickSpell6 = value; }
        }

        /// <summary>Resource reference to seventh quick spell</summary>
        public ResourceReference QuickSpell7
        {
            get { return this.quickSpell7; }
            set { this.quickSpell7 = value; }
        }

        /// <summary>Resource reference to eighth quick spell</summary>
        public ResourceReference QuickSpell8
        {
            get { return this.quickSpell8; }
            set { this.quickSpell8 = value; }
        }

        /// <summary>Resource reference to ninth quick spell</summary>
        public ResourceReference QuickSpell9
        {
            get { return this.quickSpell9; }
            set { this.quickSpell9 = value; }
        }

        /// <summary>Field indicating the class associated with the quick spell slot</summary>
        public Byte ClassQuickSpell1
        {
            get { return this.classQuickSpell1; }
            set { this.classQuickSpell1 = value; }
        }

        /// <summary>Field indicating the class associated with the quick spell slot</summary>
        public Byte ClassQuickSpell2
        {
            get { return this.classQuickSpell2; }
            set { this.classQuickSpell2 = value; }
        }

        /// <summary>Field indicating the class associated with the quick spell slot</summary>
        public Byte ClassQuickSpell3
        {
            get { return this.classQuickSpell3; }
            set { this.classQuickSpell3 = value; }
        }

        /// <summary>Field indicating the class associated with the quick spell slot</summary>
        public Byte ClassQuickSpell4
        {
            get { return this.classQuickSpell4; }
            set { this.classQuickSpell4 = value; }
        }

        /// <summary>Field indicating the class associated with the quick spell slot</summary>
        public Byte ClassQuickSpell5
        {
            get { return this.classQuickSpell5; }
            set { this.classQuickSpell5 = value; }
        }

        /// <summary>Field indicating the class associated with the quick spell slot</summary>
        public Byte ClassQuickSpell6
        {
            get { return this.classQuickSpell6; }
            set { this.classQuickSpell6 = value; }
        }

        /// <summary>Field indicating the class associated with the quick spell slot</summary>
        public Byte ClassQuickSpell7
        {
            get { return this.classQuickSpell7; }
            set { this.classQuickSpell7 = value; }
        }

        /// <summary>Field indicating the class associated with the quick spell slot</summary>
        public Byte ClassQuickSpell8
        {
            get { return this.classQuickSpell8; }
            set { this.classQuickSpell8 = value; }
        }

        /// <summary>Field indicating the class associated with the quick spell slot</summary>
        public Byte ClassQuickSpell9
        {
            get { return this.classQuickSpell9; }
            set { this.classQuickSpell9 = value; }
        }

        /// <summary>First unknown byte, probably unused</summary>
        public Byte Unknown1
        {
            get { return this.unknown1; }
            set { this.unknown1 = value; }
        }

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        public Int16 UsableQuickItemSlot1
        {
            get { return this.usableQuickItemSlot1; }
            set { this.usableQuickItemSlot1 = value; }
        }

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        public Int16 UsableQuickItemSlot2
        {
            get { return this.usableQuickItemSlot2; }
            set { this.usableQuickItemSlot2 = value; }
        }

        /// <summary>Field indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        public Int16 UsableQuickItemSlot3
        {
            get { return this.usableQuickItemSlot3; }
            set { this.usableQuickItemSlot3 = value; }
        }

        /// <summary>Resource reference to first quick Innate ability</summary>
        public ResourceReference QuickInnate1
        {
            get { return this.quickInnate1; }
            set { this.quickInnate1 = value; }
        }

        /// <summary>Resource reference to second quick Innate ability</summary>
        public ResourceReference QuickInnate2
        {
            get { return this.quickInnate2; }
            set { this.quickInnate2 = value; }
        }

        /// <summary>Resource reference to third quick Innate ability</summary>
        public ResourceReference QuickInnate3
        {
            get { return this.quickInnate3; }
            set { this.quickInnate3 = value; }
        }

        /// <summary>Resource reference to fourth quick Innate ability</summary>
        public ResourceReference QuickInnate4
        {
            get { return this.quickInnate4; }
            set { this.quickInnate4 = value; }
        }

        /// <summary>Resource reference to fifth quick Innate ability</summary>
        public ResourceReference QuickInnate5
        {
            get { return this.quickInnate5; }
            set { this.quickInnate5 = value; }
        }

        /// <summary>Resource reference to sixth quick Innate ability</summary>
        public ResourceReference QuickInnate6
        {
            get { return this.quickInnate6; }
            set { this.quickInnate6 = value; }
        }

        /// <summary>Resource reference to seventh quick Innate ability</summary>
        public ResourceReference QuickInnate7
        {
            get { return this.quickInnate7; }
            set { this.quickInnate7 = value; }
        }

        /// <summary>Resource reference to eighth quick Innate ability</summary>
        public ResourceReference QuickInnate8
        {
            get { return this.quickInnate8; }
            set { this.quickInnate8 = value; }
        }

        /// <summary>Resource reference to ninth quick Innate ability</summary>
        public ResourceReference QuickInnate9
        {
            get { return this.quickInnate9; }
            set { this.quickInnate9 = value; }
        }

        /// <summary>Resource reference to first quick Song</summary>
        public ResourceReference QuickSong1
        {
            get { return this.quickSong1; }
            set { this.quickSong1 = value; }
        }

        /// <summary>Resource reference to second quick Song</summary>
        public ResourceReference QuickSong2
        {
            get { return this.quickSong2; }
            set { this.quickSong2 = value; }
        }

        /// <summary>Resource reference to third quick Song</summary>
        public ResourceReference QuickSong3
        {
            get { return this.quickSong3; }
            set { this.quickSong3 = value; }
        }

        /// <summary>Resource reference to fourth quick Song</summary>
        public ResourceReference QuickSong4
        {
            get { return this.quickSong4; }
            set { this.quickSong4 = value; }
        }

        /// <summary>Resource reference to fifth quick Song</summary>
        public ResourceReference QuickSong5
        {
            get { return this.quickSong5; }
            set { this.quickSong5 = value; }
        }

        /// <summary>Resource reference to sixth quick Song</summary>
        public ResourceReference QuickSong6
        {
            get { return this.quickSong6; }
            set { this.quickSong6 = value; }
        }

        /// <summary>Resource reference to seventh quick Song</summary>
        public ResourceReference QuickSong7
        {
            get { return this.quickSong7; }
            set { this.quickSong7 = value; }
        }

        /// <summary>Resource reference to eighth quick Song</summary>
        public ResourceReference QuickSong8
        {
            get { return this.quickSong8; }
            set { this.quickSong8 = value; }
        }

        /// <summary>Resource reference to ninth quick Song</summary>
        public ResourceReference QuickSong9
        {
            get { return this.quickSong9; }
            set { this.quickSong9 = value; }
        }

        /// <summary>Current button action assigned to associated slot</summary>
        public UserInterfaceButton Button1
        {
            get { return this.button1; }
            set { this.button1 = value; }
        }

        /// <summary>Current button action assigned to associated slot</summary>
        public UserInterfaceButton Button2
        {
            get { return this.button2; }
            set { this.button2 = value; }
        }

        /// <summary>Current button action assigned to associated slot</summary>
        public UserInterfaceButton Button3
        {
            get { return this.button3; }
            set { this.button3 = value; }
        }

        /// <summary>Current button action assigned to associated slot</summary>
        public UserInterfaceButton Button4
        {
            get { return this.button4; }
            set { this.button4 = value; }
        }

        /// <summary>Current button action assigned to associated slot</summary>
        public UserInterfaceButton Button5
        {
            get { return this.button5; }
            set { this.button5 = value; }
        }

        /// <summary>Current button action assigned to associated slot</summary>
        public UserInterfaceButton Button6
        {
            get { return this.button6; }
            set { this.button6 = value; }
        }

        /// <summary>Current button action assigned to associated slot</summary>
        public UserInterfaceButton Button7
        {
            get { return this.button7; }
            set { this.button7 = value; }
        }

        /// <summary>Current button action assigned to associated slot</summary>
        public UserInterfaceButton Button8
        {
            get { return this.button8; }
            set { this.button8 = value; }
        }

        /// <summary>Current button action assigned to associated slot</summary>
        public UserInterfaceButton Button9
        {
            get { return this.button9; }
            set { this.button9 = value; }
        }

        /// <summary>Unknown 2 Bytes after configurable slots</summary>
        public UInt16 Unknown2
        {
            get { return this.unknown2; }
            set { this.unknown2 = value; }
        }

        /// <summary>Level of the associated spell slot</summary>
        public Byte LevelSpell1
        {
            get { return this.levelSpell1; }
            set { this.levelSpell1 = value; }
        }

        /// <summary>Level of the associated spell slot</summary>
        public Byte LevelSpell2
        {
            get { return this.levelSpell2; }
            set { this.levelSpell2 = value; }
        }

        /// <summary>Level of the associated spell slot</summary>
        public Byte LevelSpell3
        {
            get { return this.levelSpell3; }
            set { this.levelSpell3 = value; }
        }

        /// <summary>Level of the associated spell slot</summary>
        public Byte LevelSpell4
        {
            get { return this.levelSpell4; }
            set { this.levelSpell4 = value; }
        }

        /// <summary>Level of the associated spell slot</summary>
        public Byte LevelSpell5
        {
            get { return this.levelSpell5; }
            set { this.levelSpell5 = value; }
        }

        /// <summary>Level of the associated spell slot</summary>
        public Byte LevelSpell6
        {
            get { return this.levelSpell6; }
            set { this.levelSpell6 = value; }
        }

        /// <summary>Level of the associated spell slot</summary>
        public Byte LevelSpell7
        {
            get { return this.levelSpell7; }
            set { this.levelSpell7 = value; }
        }

        /// <summary>Level of the associated spell slot</summary>
        public Byte LevelSpell8
        {
            get { return this.levelSpell8; }
            set { this.levelSpell8 = value; }
        }

        /// <summary>Level of the associated spell slot</summary>
        public Byte LevelSpell9
        {
            get { return this.levelSpell9; }
            set { this.levelSpell9 = value; }
        }

        /// <summary>Third unknown byte after spell slot levels</summary>
        /// <remarks>Probably unused</remarks>
        public Byte Unknown3
        {
            get { return this.unknown3; }
            set { this.unknown3 = value; }
        }

        /// <summary>14 unknown bytes</summary>
        public Byte[] Unknown4
        {
            get { return this.unknown4; }
            set { this.unknown4 = value; }
        }

        /// <summary>Soundset resource reference</summary>
        public ResourceReference SoundSet
        {
            get { return this.soundSet; }
            set { this.soundSet = value; }
        }

        /// <summary>Biff archive in which the soundset is stored. 20 Bytes.</summary>
        public ZString SoundSetBiff { get; set; }

        /// <summary>128 reserved bytes. Probably unused.</summary>
        public Byte[] Reserved
        {
            get { return this.reserved; }
            set { this.reserved = value; }
        }
        #endregion
        
        #region Constructor(s)
        /// <summary>Default constructor</summary>
        public Character2_2Header()
        {
            this.quickSpell1 = null;
            this.quickSpell2 = null;
            this.quickSpell3 = null;
            this.quickSpell4 = null;
            this.quickSpell5 = null;
            this.quickSpell6 = null;
            this.quickSpell7 = null;
            this.quickSpell8 = null;
            this.quickSpell9 = null;
            this.quickInnate1 = null;
            this.quickInnate2 = null;
            this.quickInnate3 = null;
            this.quickInnate4 = null;
            this.quickInnate5 = null;
            this.quickInnate6 = null;
            this.quickInnate7 = null;
            this.quickInnate8 = null;
            this.quickInnate9 = null;
            this.quickSong1 = null;
            this.quickSong2 = null;
            this.quickSong3 = null;
            this.quickSong4 = null;
            this.quickSong5 = null;
            this.quickSong6 = null;
            this.quickSong7 = null;
            this.quickSong8 = null;
            this.quickSong9 = null;
            this.unknown4 = null;
            this.soundSet = null;
            this.reserved = null;
        }

        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            base.Initialize();

            this.quickSpell4 = new ResourceReference();
            this.quickSpell5 = new ResourceReference();
            this.quickSpell6 = new ResourceReference();
            this.quickSpell7 = new ResourceReference();
            this.quickSpell8 = new ResourceReference();
            this.quickSpell9 = new ResourceReference();
            this.quickInnate1 = new ResourceReference();
            this.quickInnate2 = new ResourceReference();
            this.quickInnate3 = new ResourceReference();
            this.quickInnate4 = new ResourceReference();
            this.quickInnate5 = new ResourceReference();
            this.quickInnate6 = new ResourceReference();
            this.quickInnate7 = new ResourceReference();
            this.quickInnate8 = new ResourceReference();
            this.quickInnate9 = new ResourceReference();
            this.quickSong1 = new ResourceReference();
            this.quickSong2 = new ResourceReference();
            this.quickSong3 = new ResourceReference();
            this.quickSong4 = new ResourceReference();
            this.quickSong5 = new ResourceReference();
            this.quickSong6 = new ResourceReference();
            this.quickSong7 = new ResourceReference();
            this.quickSong8 = new ResourceReference();
            this.quickSong9 = new ResourceReference();
            this.unknown4 = new Byte[14];
            this.soundSet = new ResourceReference();
            this.SoundSetBiff = new ZString();
            this.reserved = new Byte[128];
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
            Byte[] header = ReusableIO.BinaryRead(input, StructSize - 8);

            this.Name.Source = ReusableIO.ReadStringFromByteArray(header, 0, CultureConstants.CultureCodeEnglish, 32);
            this.offsetCreature = ReusableIO.ReadUInt32FromArray(header, 32);
            this.lengthCreature = ReusableIO.ReadUInt32FromArray(header, 36);
            this.quickWeaponSlotIndex1 = ReusableIO.ReadUInt16FromArray(header, 40);
            this.quickShieldSlotIndex1 = ReusableIO.ReadUInt16FromArray(header, 42);
            this.quickWeaponSlotIndex2 = ReusableIO.ReadUInt16FromArray(header, 44);
            this.quickShieldSlotIndex2 = ReusableIO.ReadUInt16FromArray(header, 46);
            this.quickWeaponSlotIndex3 = ReusableIO.ReadUInt16FromArray(header, 48);
            this.quickShieldSlotIndex3 = ReusableIO.ReadUInt16FromArray(header, 50);
            this.quickWeaponSlotIndex4 = ReusableIO.ReadUInt16FromArray(header, 52);
            this.quickShieldSlotIndex4 = ReusableIO.ReadUInt16FromArray(header, 54);
            this.usableQuickWeaponSlot1 = ReusableIO.ReadInt16FromArray(header, 56);
            this.usableQuickShieldSlot1 = ReusableIO.ReadInt16FromArray(header, 58);
            this.usableQuickWeaponSlot2 = ReusableIO.ReadInt16FromArray(header, 60);
            this.usableQuickShieldSlot2 = ReusableIO.ReadInt16FromArray(header, 62);
            this.usableQuickWeaponSlot3 = ReusableIO.ReadInt16FromArray(header, 64);
            this.usableQuickShieldSlot3 = ReusableIO.ReadInt16FromArray(header, 66);
            this.usableQuickWeaponSlot4 = ReusableIO.ReadInt16FromArray(header, 68);
            this.usableQuickShieldSlot4 = ReusableIO.ReadInt16FromArray(header, 70);
            this.quickSpell1.ResRef = ReusableIO.ReadStringFromByteArray(header, 72, CultureConstants.CultureCodeEnglish);
            this.quickSpell2.ResRef = ReusableIO.ReadStringFromByteArray(header, 80, CultureConstants.CultureCodeEnglish);
            this.quickSpell3.ResRef = ReusableIO.ReadStringFromByteArray(header, 88, CultureConstants.CultureCodeEnglish);
            this.quickSpell4.ResRef = ReusableIO.ReadStringFromByteArray(header, 96, CultureConstants.CultureCodeEnglish);
            this.quickSpell5.ResRef = ReusableIO.ReadStringFromByteArray(header, 104, CultureConstants.CultureCodeEnglish);
            this.quickSpell6.ResRef = ReusableIO.ReadStringFromByteArray(header, 112, CultureConstants.CultureCodeEnglish);
            this.quickSpell7.ResRef = ReusableIO.ReadStringFromByteArray(header, 120, CultureConstants.CultureCodeEnglish);
            this.quickSpell8.ResRef = ReusableIO.ReadStringFromByteArray(header, 128, CultureConstants.CultureCodeEnglish);
            this.quickSpell9.ResRef = ReusableIO.ReadStringFromByteArray(header, 136, CultureConstants.CultureCodeEnglish);
            this.classQuickSpell1 = header[144];
            this.classQuickSpell2 = header[145];
            this.classQuickSpell3 = header[146];
            this.classQuickSpell4 = header[147];
            this.classQuickSpell5 = header[148];
            this.classQuickSpell6 = header[149];
            this.classQuickSpell7 = header[150];
            this.classQuickSpell8 = header[151];
            this.classQuickSpell9 = header[152];
            this.unknown1 = header[153];
            this.quickItemSlotIndex1 = ReusableIO.ReadInt16FromArray(header, 154);
            this.quickItemSlotIndex2 = ReusableIO.ReadInt16FromArray(header, 156);
            this.quickItemSlotIndex3 = ReusableIO.ReadInt16FromArray(header, 158);
            this.usableQuickItemSlot1 = ReusableIO.ReadInt16FromArray(header, 160);
            this.usableQuickItemSlot2 = ReusableIO.ReadInt16FromArray(header, 162);
            this.usableQuickItemSlot3 = ReusableIO.ReadInt16FromArray(header, 164);
            this.quickInnate1.ResRef = ReusableIO.ReadStringFromByteArray(header, 166, CultureConstants.CultureCodeEnglish);
            this.quickInnate2.ResRef = ReusableIO.ReadStringFromByteArray(header, 174, CultureConstants.CultureCodeEnglish);
            this.quickInnate3.ResRef = ReusableIO.ReadStringFromByteArray(header, 182, CultureConstants.CultureCodeEnglish);
            this.quickInnate4.ResRef = ReusableIO.ReadStringFromByteArray(header, 190, CultureConstants.CultureCodeEnglish);
            this.quickInnate5.ResRef = ReusableIO.ReadStringFromByteArray(header, 198, CultureConstants.CultureCodeEnglish);
            this.quickInnate6.ResRef = ReusableIO.ReadStringFromByteArray(header, 206, CultureConstants.CultureCodeEnglish);
            this.quickInnate7.ResRef = ReusableIO.ReadStringFromByteArray(header, 214, CultureConstants.CultureCodeEnglish);
            this.quickInnate8.ResRef = ReusableIO.ReadStringFromByteArray(header, 222, CultureConstants.CultureCodeEnglish);
            this.quickInnate9.ResRef = ReusableIO.ReadStringFromByteArray(header, 230, CultureConstants.CultureCodeEnglish);
            this.quickSong1.ResRef = ReusableIO.ReadStringFromByteArray(header, 238, CultureConstants.CultureCodeEnglish);
            this.quickSong2.ResRef = ReusableIO.ReadStringFromByteArray(header, 246, CultureConstants.CultureCodeEnglish);
            this.quickSong3.ResRef = ReusableIO.ReadStringFromByteArray(header, 254, CultureConstants.CultureCodeEnglish);
            this.quickSong4.ResRef = ReusableIO.ReadStringFromByteArray(header, 262, CultureConstants.CultureCodeEnglish);
            this.quickSong5.ResRef = ReusableIO.ReadStringFromByteArray(header, 270, CultureConstants.CultureCodeEnglish);
            this.quickSong6.ResRef = ReusableIO.ReadStringFromByteArray(header, 278, CultureConstants.CultureCodeEnglish);
            this.quickSong7.ResRef = ReusableIO.ReadStringFromByteArray(header, 286, CultureConstants.CultureCodeEnglish);
            this.quickSong8.ResRef = ReusableIO.ReadStringFromByteArray(header, 294, CultureConstants.CultureCodeEnglish);
            this.quickSong9.ResRef = ReusableIO.ReadStringFromByteArray(header, 302, CultureConstants.CultureCodeEnglish);
            this.button1 = (UserInterfaceButton)ReusableIO.ReadUInt32FromArray(header, 310);
            this.button2 = (UserInterfaceButton)ReusableIO.ReadUInt32FromArray(header, 314);
            this.button3 = (UserInterfaceButton)ReusableIO.ReadUInt32FromArray(header, 318);
            this.button4 = (UserInterfaceButton)ReusableIO.ReadUInt32FromArray(header, 322);
            this.button5 = (UserInterfaceButton)ReusableIO.ReadUInt32FromArray(header, 326);
            this.button6 = (UserInterfaceButton)ReusableIO.ReadUInt32FromArray(header, 330);
            this.button7 = (UserInterfaceButton)ReusableIO.ReadUInt32FromArray(header, 334);
            this.button8 = (UserInterfaceButton)ReusableIO.ReadUInt32FromArray(header, 338);
            this.button9 = (UserInterfaceButton)ReusableIO.ReadUInt32FromArray(header, 342);
            this.unknown2 = ReusableIO.ReadUInt16FromArray(header, 346);
            this.levelSpell1 = header[348];
            this.levelSpell2 = header[349];
            this.levelSpell3 = header[350];
            this.levelSpell4 = header[351];
            this.levelSpell5 = header[352];
            this.levelSpell6 = header[353];
            this.levelSpell7 = header[354];
            this.levelSpell8 = header[355];
            this.levelSpell9 = header[356];
            this.unknown3 = header[357];
            Array.Copy(header, 358, this.unknown4, 0, 14);
            this.soundSet.ResRef = ReusableIO.ReadStringFromByteArray(header, 372, CultureConstants.CultureCodeEnglish);
            this.SoundSetBiff.Source = ReusableIO.ReadStringFromByteArray(header, 380, CultureConstants.CultureCodeEnglish, 32);
            Array.Copy(header, 412, this.reserved, 0, 128);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.Name.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt32ToStream(this.offsetCreature, output);
            ReusableIO.WriteUInt32ToStream(this.lengthCreature, output);
            ReusableIO.WriteUInt16ToStream(this.quickWeaponSlotIndex1, output);
            ReusableIO.WriteUInt16ToStream(this.quickShieldSlotIndex1, output);
            ReusableIO.WriteUInt16ToStream(this.quickWeaponSlotIndex2, output);
            ReusableIO.WriteUInt16ToStream(this.quickShieldSlotIndex2, output);
            ReusableIO.WriteUInt16ToStream(this.quickWeaponSlotIndex3, output);
            ReusableIO.WriteUInt16ToStream(this.quickShieldSlotIndex3, output);
            ReusableIO.WriteUInt16ToStream(this.quickWeaponSlotIndex4, output);
            ReusableIO.WriteUInt16ToStream(this.quickShieldSlotIndex4, output);
            ReusableIO.WriteInt16ToStream(this.usableQuickWeaponSlot1, output);
            ReusableIO.WriteInt16ToStream(this.usableQuickShieldSlot1, output);
            ReusableIO.WriteInt16ToStream(this.usableQuickWeaponSlot2, output);
            ReusableIO.WriteInt16ToStream(this.usableQuickShieldSlot2, output);
            ReusableIO.WriteInt16ToStream(this.usableQuickWeaponSlot3, output);
            ReusableIO.WriteInt16ToStream(this.usableQuickShieldSlot3, output);
            ReusableIO.WriteInt16ToStream(this.usableQuickWeaponSlot4, output);
            ReusableIO.WriteInt16ToStream(this.usableQuickShieldSlot4, output);
            ReusableIO.WriteStringToStream(this.quickSpell1.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickSpell2.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickSpell3.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickSpell4.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickSpell5.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickSpell6.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickSpell7.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickSpell8.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickSpell9.ResRef, output, CultureConstants.CultureCodeEnglish);
            output.WriteByte(this.classQuickSpell1);
            output.WriteByte(this.classQuickSpell2);
            output.WriteByte(this.classQuickSpell3);
            output.WriteByte(this.classQuickSpell4);
            output.WriteByte(this.classQuickSpell5);
            output.WriteByte(this.classQuickSpell6);
            output.WriteByte(this.classQuickSpell7);
            output.WriteByte(this.classQuickSpell8);
            output.WriteByte(this.classQuickSpell9);
            output.WriteByte(this.unknown1);
            ReusableIO.WriteInt16ToStream(this.quickItemSlotIndex1, output);
            ReusableIO.WriteInt16ToStream(this.quickItemSlotIndex2, output);
            ReusableIO.WriteInt16ToStream(this.quickItemSlotIndex3, output);
            ReusableIO.WriteInt16ToStream(this.usableQuickItemSlot1, output);
            ReusableIO.WriteInt16ToStream(this.usableQuickItemSlot2, output);
            ReusableIO.WriteInt16ToStream(this.usableQuickItemSlot3, output);
            ReusableIO.WriteStringToStream(this.quickInnate1.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickInnate2.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickInnate3.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickInnate4.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickInnate5.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickInnate6.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickInnate7.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickInnate8.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickInnate9.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickSong1.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickSong2.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickSong3.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickSong4.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickSong5.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickSong6.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickSong7.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickSong8.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.quickSong9.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream((UInt32)this.button1, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.button2, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.button3, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.button4, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.button5, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.button6, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.button7, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.button8, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.button9, output);
            ReusableIO.WriteUInt16ToStream(this.unknown2, output);
            output.WriteByte(this.levelSpell1);
            output.WriteByte(this.levelSpell2);
            output.WriteByte(this.levelSpell3);
            output.WriteByte(this.levelSpell4);
            output.WriteByte(this.levelSpell5);
            output.WriteByte(this.levelSpell6);
            output.WriteByte(this.levelSpell7);
            output.WriteByte(this.levelSpell8);
            output.WriteByte(this.levelSpell9);
            output.WriteByte(this.unknown3);
            output.Write(this.unknown4, 0, 14);
            ReusableIO.WriteStringToStream(this.soundSet.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.SoundSetBiff.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            output.Write(this.reserved, 0, 128);
        }
        #endregion

        #region ToString() helpers
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
            builder.Append(StringFormat.ToStringAlignment("Quick Shield Index 1"));
            builder.Append(this.quickShieldSlotIndex1);
            builder.Append(StringFormat.ToStringAlignment("Quick Weapon Index 2"));
            builder.Append(this.quickWeaponSlotIndex2);
            builder.Append(StringFormat.ToStringAlignment("Quick Shield Index 2"));
            builder.Append(this.quickShieldSlotIndex2);
            builder.Append(StringFormat.ToStringAlignment("Quick Weapon Index 3"));
            builder.Append(this.quickWeaponSlotIndex3);
            builder.Append(StringFormat.ToStringAlignment("Quick Shield Index 3"));
            builder.Append(this.quickShieldSlotIndex3);
            builder.Append(StringFormat.ToStringAlignment("Quick Weapon Index 4"));
            builder.Append(this.quickWeaponSlotIndex4);
            builder.Append(StringFormat.ToStringAlignment("Quick Shield Index 4"));
            builder.Append(this.quickShieldSlotIndex4);
            builder.Append(StringFormat.ToStringAlignment("Quick Weapon slot 1 usable"));
            builder.Append(this.usableQuickWeaponSlot1);
            builder.Append(StringFormat.ToStringAlignment("Quick Shield slot 1 usable"));
            builder.Append(this.usableQuickShieldSlot1);
            builder.Append(StringFormat.ToStringAlignment("Quick Weapon slot 2 usable"));
            builder.Append(this.usableQuickWeaponSlot2);
            builder.Append(StringFormat.ToStringAlignment("Quick Shield slot 2 usable"));
            builder.Append(this.usableQuickShieldSlot2);
            builder.Append(StringFormat.ToStringAlignment("Quick Weapon slot 3 usable"));
            builder.Append(this.usableQuickWeaponSlot3);
            builder.Append(StringFormat.ToStringAlignment("Quick Shield slot 3 usable"));
            builder.Append(this.usableQuickShieldSlot3);
            builder.Append(StringFormat.ToStringAlignment("Quick Weapon slot 4 usable"));
            builder.Append(this.usableQuickWeaponSlot4);
            builder.Append(StringFormat.ToStringAlignment("Quick Shield slot 4 usable"));
            builder.Append(this.usableQuickShieldSlot4);
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
            builder.Append(StringFormat.ToStringAlignment("Quick spell ResRef 4"));
            builder.Append("'");
            builder.Append(this.quickSpell4.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick spell ResRef 5"));
            builder.Append("'");
            builder.Append(this.quickSpell5.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick spell ResRef 6"));
            builder.Append("'");
            builder.Append(this.quickSpell6.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick spell ResRef 7"));
            builder.Append("'");
            builder.Append(this.quickSpell7.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick spell ResRef 8"));
            builder.Append("'");
            builder.Append(this.quickSpell8.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick spell ResRef 9"));
            builder.Append("'");
            builder.Append(this.quickSpell9.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Class of quick spell 1"));
            builder.Append(this.classQuickSpell1);
            builder.Append(StringFormat.ToStringAlignment("Class of quick spell 2"));
            builder.Append(this.classQuickSpell2);
            builder.Append(StringFormat.ToStringAlignment("Class of quick spell 3"));
            builder.Append(this.classQuickSpell3);
            builder.Append(StringFormat.ToStringAlignment("Class of quick spell 4"));
            builder.Append(this.classQuickSpell4);
            builder.Append(StringFormat.ToStringAlignment("Class of quick spell 5"));
            builder.Append(this.classQuickSpell5);
            builder.Append(StringFormat.ToStringAlignment("Class of quick spell 6"));
            builder.Append(this.classQuickSpell6);
            builder.Append(StringFormat.ToStringAlignment("Class of quick spell 7"));
            builder.Append(this.classQuickSpell7);
            builder.Append(StringFormat.ToStringAlignment("Class of quick spell 8"));
            builder.Append(this.classQuickSpell8);
            builder.Append(StringFormat.ToStringAlignment("Class of quick spell 9"));
            builder.Append(this.classQuickSpell9);
            builder.Append(StringFormat.ToStringAlignment("Unknown #1 (Padding?)"));
            builder.Append(this.unknown1);
            builder.Append(StringFormat.ToStringAlignment("Quick item index 1"));
            builder.Append(this.quickItemSlotIndex1);
            builder.Append(StringFormat.ToStringAlignment("Quick item index 2"));
            builder.Append(this.quickItemSlotIndex2);
            builder.Append(StringFormat.ToStringAlignment("Quick item index 3"));
            builder.Append(this.quickItemSlotIndex3);
            builder.Append(StringFormat.ToStringAlignment("Quick item 1 usable"));
            builder.Append(this.usableQuickItemSlot1);
            builder.Append(StringFormat.ToStringAlignment("Quick item 2 usable"));
            builder.Append(this.usableQuickItemSlot2);
            builder.Append(StringFormat.ToStringAlignment("Quick item 3 usable"));
            builder.Append(this.usableQuickItemSlot3);
            builder.Append(StringFormat.ToStringAlignment("Quick Innate ResRef 1"));
            builder.Append("'");
            builder.Append(this.quickInnate1.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick Innate ResRef 2"));
            builder.Append("'");
            builder.Append(this.quickInnate2.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick Innate ResRef 3"));
            builder.Append("'");
            builder.Append(this.quickInnate3.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick Innate ResRef 4"));
            builder.Append("'");
            builder.Append(this.quickInnate4.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick Innate ResRef 5"));
            builder.Append("'");
            builder.Append(this.quickInnate5.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick Innate ResRef 6"));
            builder.Append("'");
            builder.Append(this.quickInnate6.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick Innate ResRef 7"));
            builder.Append("'");
            builder.Append(this.quickInnate7.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick Innate ResRef 8"));
            builder.Append("'");
            builder.Append(this.quickInnate8.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick Innate ResRef 9"));
            builder.Append("'");
            builder.Append(this.quickInnate9.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick Song ResRef 1"));
            builder.Append("'");
            builder.Append(this.quickSong1.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick Song ResRef 2"));
            builder.Append("'");
            builder.Append(this.quickSong2.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick Song ResRef 3"));
            builder.Append("'");
            builder.Append(this.quickSong3.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick Song ResRef 4"));
            builder.Append("'");
            builder.Append(this.quickSong4.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick Song ResRef 5"));
            builder.Append("'");
            builder.Append(this.quickSong5.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick Song ResRef 6"));
            builder.Append("'");
            builder.Append(this.quickSong6.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick Song ResRef 7"));
            builder.Append("'");
            builder.Append(this.quickSong7.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick Song ResRef 8"));
            builder.Append("'");
            builder.Append(this.quickSong8.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Quick Song ResRef 9"));
            builder.Append("'");
            builder.Append(this.quickSong9.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Customizable button #1"));
            builder.Append((UInt32)this.button1);
            builder.Append(StringFormat.ToStringAlignment("Customizable button #1 (description)"));
            builder.Append(this.button1.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Customizable button #2"));
            builder.Append((UInt32)this.button2);
            builder.Append(StringFormat.ToStringAlignment("Customizable button #2 (description)"));
            builder.Append(this.button2.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Customizable button #3"));
            builder.Append((UInt32)this.button3);
            builder.Append(StringFormat.ToStringAlignment("Customizable button #3 (description)"));
            builder.Append(this.button3.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Customizable button #4"));
            builder.Append((UInt32)this.button4);
            builder.Append(StringFormat.ToStringAlignment("Customizable button #4 (description)"));
            builder.Append(this.button4.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Customizable button #5"));
            builder.Append((UInt32)this.button5);
            builder.Append(StringFormat.ToStringAlignment("Customizable button #5 (description)"));
            builder.Append(this.button5.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Customizable button #6"));
            builder.Append((UInt32)this.button6);
            builder.Append(StringFormat.ToStringAlignment("Customizable button #6 (description)"));
            builder.Append(this.button6.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Customizable button #7"));
            builder.Append((UInt32)this.button7);
            builder.Append(StringFormat.ToStringAlignment("Customizable button #7 (description)"));
            builder.Append(this.button7.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Customizable button #8"));
            builder.Append((UInt32)this.button8);
            builder.Append(StringFormat.ToStringAlignment("Customizable button #8 (description)"));
            builder.Append(this.button8.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Customizable button #9"));
            builder.Append((UInt32)this.button9);
            builder.Append(StringFormat.ToStringAlignment("Customizable button #9 (description)"));
            builder.Append(this.button9.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Unknown #2 (Padding?)"));
            builder.Append(this.unknown2);
            builder.Append(StringFormat.ToStringAlignment("Level of quick spell 1"));
            builder.Append(this.levelSpell1);
            builder.Append(StringFormat.ToStringAlignment("Level of quick spell 2"));
            builder.Append(this.levelSpell2);
            builder.Append(StringFormat.ToStringAlignment("Level of quick spell 3"));
            builder.Append(this.levelSpell3);
            builder.Append(StringFormat.ToStringAlignment("Level of quick spell 4"));
            builder.Append(this.levelSpell4);
            builder.Append(StringFormat.ToStringAlignment("Level of quick spell 5"));
            builder.Append(this.levelSpell5);
            builder.Append(StringFormat.ToStringAlignment("Level of quick spell 6"));
            builder.Append(this.levelSpell6);
            builder.Append(StringFormat.ToStringAlignment("Level of quick spell 7"));
            builder.Append(this.levelSpell7);
            builder.Append(StringFormat.ToStringAlignment("Level of quick spell 8"));
            builder.Append(this.levelSpell8);
            builder.Append(StringFormat.ToStringAlignment("Level of quick spell 9"));
            builder.Append(this.levelSpell9);
            builder.Append(StringFormat.ToStringAlignment("Unknown #3 (Padding?)"));
            builder.Append(this.unknown3);
            builder.Append(StringFormat.ToStringAlignment("Unknown #4"));
            builder.Append(StringFormat.ByteArrayToHexString(this.unknown4));
            builder.Append(StringFormat.ToStringAlignment("Sound set prefix"));
            builder.Append("'");
            builder.Append(this.soundSet.ZResRef);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Sound set archive"));
            builder.Append("'");
            builder.Append(this.SoundSetBiff.Value);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Trailing reservd bytes"));
            builder.AppendLine(StringFormat.ByteArrayToHexString(this.reserved));


            return builder.ToString();
        }
        #endregion
    }
}