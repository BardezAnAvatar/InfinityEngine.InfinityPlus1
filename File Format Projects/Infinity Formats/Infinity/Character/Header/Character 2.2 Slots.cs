using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Character.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Character.Header
{
    /// <summary>Represents the slot availability for Characters in {IWD2} CHR and GAM files</summary>
	public class CharacterSlots_v2_2 : CharacterSlotsBase
    {
        #region Constants
        /// <summary>epresents the structure dize on disk of the quick slots</summary>
        private const Int32 StructSize = 306;
        #endregion


        #region Quasi-constants
        /// <summary>Exposes the count of Quick Weapon slots</summary>
        protected virtual Int32 QuickShieldSlotCount
        {
            get { return 4; }
        }

        /// <summary>Exposes the count of Quick Spell slots</summary>
        protected override Int32 QuickSpellSlotCount
        {
            get { return 9; }
        }

        /// <summary>Exposes the count of Quick Spell Class slots</summary>
        protected virtual Int32 QuickSpellSlotClassCount
        {
            get { return 9; }
        }

        /// <summary>Exposes the count of Quick Innate slots</summary>
        protected virtual Int32 QuickInnateSlotCount
        {
            get { return 9; }
        }

        /// <summary>Exposes the count of Quick Bard Song slots</summary>
        protected virtual Int32 QuickBardSongSlotCount
        {
            get { return 9; }
        }

        /// <summary>Exposes the count of programable button slots</summary>
        protected virtual Int32 ButtonSlotCount
        {
            get { return 9; }
        }
        #endregion


        #region Fields
        /// <summary>Represents an array of indeces into SLOTS.IDS for Quick Shields</summary>
        /// <value>(0xFFFF = none)</value>
        public Int16[] QuickShieldSlotIndeces { get; set; }

        /// <summary>Represents an array of indeces into Quick Weapons indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        /// <value>(0xFFFF = none)</value>
        public Int16[] UsableQuickWeaponSlotIndeces { get; set; }

        /// <summary>Represents an array of indeces into Quick Shields indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        /// <value>(0xFFFF = none)</value>
        public Int16[] UsableQuickShieldSlotIndeces { get; set; }

        /// <summary>Array of fields indicating the class associated with the quick spell slot</summary>
        public Byte[] ClassQuickSpells { get; set; }

        /// <summary>First unknown byte, probably unused</summary>
        public Byte Unknown { get; set; }

        /// <summary>Represents an array of indeces into Quick Items indicating whether or not the associated quickslot is usable, evaluated as a Boolean</summary>
        /// <value>(0xFFFF = none)</value>
        public Int16[] UsableQuickItemSlotIndeces { get; set; }

        /// <summary>Represents an array of ResourceReferences for Quick Innate abilities</summary>
        public ResourceReference[] QuickInnateSlotIndeces { get; set; }

        /// <summary>Represents an array of ResourceReferences for Quick Bard Songs</summary>
        public ResourceReference[] QuickBardSongSlotIndeces { get; set; }

        /// <summary>Represents an array of actions currently assigned to programmable button slots</summary>
        public UserInterfaceButton[] ButtonSlotIndeces { get; set; }
        #endregion
        

        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            base.Initialize();

            this.QuickShieldSlotIndeces = new Int16[this.QuickShieldSlotCount];
            this.UsableQuickWeaponSlotIndeces = new Int16[this.QuickWeaponSlotCount];
            this.UsableQuickShieldSlotIndeces = new Int16[this.QuickShieldSlotCount];
            this.ClassQuickSpells = new Byte[this.QuickSpellSlotClassCount];
            this.UsableQuickItemSlotIndeces = new Int16[this.QuickItemSlotCount];
            this.QuickInnateSlotIndeces = new ResourceReference[this.QuickInnateSlotCount];
            this.QuickBardSongSlotIndeces = new ResourceReference[this.QuickBardSongSlotCount];
            this.ButtonSlotIndeces = new UserInterfaceButton[this.ButtonSlotCount];

            for (Int32 i = 0; i < this.QuickInnateSlotCount; ++i)
                this.QuickInnateSlotIndeces[i] = new ResourceReference();

            for (Int32 i = 0; i < this.QuickBardSongSlotCount; ++i)
                this.QuickBardSongSlotIndeces[i] = new ResourceReference();
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
            Byte[] header = ReusableIO.BinaryRead(input, CharacterSlots_v2_2.StructSize);

            //Quick weapon, quick shield combos
            for (Int32 index = 0; index < 4; ++index)
            {
                this.QuickWeaponSlotIndeces[index] = ReusableIO.ReadInt16FromArray(header, (4 * index));
                this.QuickShieldSlotIndeces[index] = ReusableIO.ReadInt16FromArray(header, (4 * index) + 2);
            }

            //Usable flags
            for (Int32 index = 0; index < 4; ++index)
            {
                this.UsableQuickWeaponSlotIndeces[index] = ReusableIO.ReadInt16FromArray(header, 16 + (4 * index));
                this.UsableQuickShieldSlotIndeces[index] = ReusableIO.ReadInt16FromArray(header, 16 + (4 * index) + 2);
            }

            for (Int32 index = 0; index < this.QuickSpellSlotCount; ++index)
                this.QuickSpellSlotIndeces[index].ResRef = ReusableIO.ReadStringFromByteArray(header, 32 + (8 * index), CultureConstants.CultureCodeEnglish);

            for (Int32 index = 0; index < this.QuickSpellSlotClassCount; ++index)
                this.ClassQuickSpells[index] = header[104 + index];

            this.Unknown = header[113];

            //quick items
            for (Int32 index = 0; index < this.QuickItemSlotCount; ++index)
                this.QuickItemSlotIndeces[index] = ReusableIO.ReadInt16FromArray(header, 114 + (2 * index));

            //usable quick items
            for (Int32 index = 0; index < this.QuickItemSlotCount; ++index)
                this.UsableQuickItemSlotIndeces[index] = ReusableIO.ReadInt16FromArray(header, 120 + (2 * index));

            //quick innate
            for (Int32 index = 0; index < this.QuickInnateSlotCount; ++index)
                this.QuickInnateSlotIndeces[index].ResRef = ReusableIO.ReadStringFromByteArray(header, 126 + (8 * index), CultureConstants.CultureCodeEnglish);

            //quick bard song
            for (Int32 index = 0; index < this.QuickBardSongSlotCount; ++index)
                this.QuickBardSongSlotIndeces[index].ResRef = ReusableIO.ReadStringFromByteArray(header, 198 + (8 * index), CultureConstants.CultureCodeEnglish);

            //buttons
            for (Int32 index = 0; index < this.ButtonSlotCount; ++index)
                this.ButtonSlotIndeces[index] = (UserInterfaceButton)ReusableIO.ReadUInt32FromArray(header, 270 + (4 * index));
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public override void Write(Stream output)
        {
            //Quick weapon, quick shield combos
            for (Int32 index = 0; index < 4; ++index)
            {
                ReusableIO.WriteInt16ToStream(this.QuickWeaponSlotIndeces[index], output);
                ReusableIO.WriteInt16ToStream(this.QuickShieldSlotIndeces[index], output);
            }

            //Usable flags
            for (Int32 index = 0; index < 4; ++index)
            {
                ReusableIO.WriteInt16ToStream(this.UsableQuickWeaponSlotIndeces[index], output);
                ReusableIO.WriteInt16ToStream(this.UsableQuickShieldSlotIndeces[index], output);
            }

            for (Int32 index = 0; index < this.QuickSpellSlotCount; ++index)
                ReusableIO.WriteStringToStream(this.QuickSpellSlotIndeces[index].ResRef, output, CultureConstants.CultureCodeEnglish);

            for (Int32 index = 0; index < this.QuickSpellSlotClassCount; ++index)
                output.WriteByte(this.ClassQuickSpells[index]);

            output.WriteByte(this.Unknown);

            //quick items
            for (Int32 index = 0; index < this.QuickItemSlotCount; ++index)
                ReusableIO.WriteInt16ToStream(this.QuickItemSlotIndeces[index], output);

            //usable quick items
            for (Int32 index = 0; index < this.QuickItemSlotCount; ++index)
                ReusableIO.WriteInt16ToStream(this.UsableQuickItemSlotIndeces[index], output);

            //quick innate
            for (Int32 index = 0; index < this.QuickInnateSlotCount; ++index)
                ReusableIO.WriteStringToStream(this.QuickInnateSlotIndeces[index].ResRef, output, CultureConstants.CultureCodeEnglish);

            //quick bard song
            for (Int32 index = 0; index < this.QuickBardSongSlotCount; ++index)
                ReusableIO.WriteStringToStream(this.QuickBardSongSlotIndeces[index].ResRef, output, CultureConstants.CultureCodeEnglish);

            //buttons
            for (Int32 index = 0; index < this.ButtonSlotCount; ++index)
                ReusableIO.WriteUInt32ToStream((UInt32)this.ButtonSlotIndeces[index], output);
        }
        #endregion


        #region ToString() helpers
        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            //quick weapons
            for (Int32 i = 0; i < this.QuickWeaponSlotCount; ++i)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Quick Weapon Index {0}", i)));
                builder.Append(this.QuickWeaponSlotIndeces[i]);
            }

            //quick shields
            for (Int32 i = 0; i < this.QuickShieldSlotCount; ++i)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Quick Shield Index {0}", i)));
                builder.Append(this.QuickShieldSlotIndeces[i]);
            }

            //quick weapons usable
            for (Int32 i = 0; i < this.QuickWeaponSlotCount; ++i)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Quick Weapon Index {0} usable", i)));
                builder.Append(this.UsableQuickWeaponSlotIndeces[i]);
            }

            //quick shields usable
            for (Int32 i = 0; i < this.QuickShieldSlotCount; ++i)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Quick Shield Index {0} usable", i)));
                builder.Append(this.UsableQuickShieldSlotIndeces[i]);
            }

            //quick spells
            for (Int32 i = 0; i < this.QuickSpellSlotCount; ++i)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Quick spell ResRef {0}", i)));
                builder.Append(String.Format("'{0}'", this.QuickSpellSlotIndeces[i].ZResRef));
            }

            //quick spell classes
            for (Int32 i = 0; i < this.QuickSpellSlotClassCount; ++i)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Class of quick spell {0}", i)));
                builder.Append(this.ClassQuickSpells[i]);
            }

            //unknown
            builder.Append(StringFormat.ToStringAlignment("Unknown #1 (Padding?)"));
            builder.Append(this.Unknown);

            //quick items
            for (Int32 i = 0; i < this.QuickItemSlotCount; ++i)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Quick item index {0}", i)));
                builder.Append(this.QuickItemSlotIndeces[i]);
            }

            //quick items usable
            for (Int32 i = 0; i < this.QuickItemSlotCount; ++i)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Quick item index {0} usable", i)));
                builder.Append(this.UsableQuickItemSlotIndeces[i]);
            }

            //quick innate abilities
            for (Int32 i = 0; i < this.QuickInnateSlotCount; ++i)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Quick innate ability ResRef {0}", i)));
                builder.Append(String.Format("'{0}'", this.QuickInnateSlotIndeces[i].ZResRef));
            }

            //quick Bard song abilities
            for (Int32 i = 0; i < this.QuickBardSongSlotCount; ++i)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Quick bard song ResRef {0}", i)));
                builder.Append(String.Format("'{0}'", this.QuickBardSongSlotIndeces[i].ZResRef));
            }

            //buttons
            for (Int32 i = 0; i < this.ButtonSlotCount; ++i)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Customizable button #{0}", i)));
                builder.Append((UInt32)this.ButtonSlotIndeces[i]);
                builder.Append(StringFormat.ToStringAlignment(String.Format("Customizable button #{0} (description)", i)));
                builder.Append(this.ButtonSlotIndeces[i].GetDescription());
            }

            return builder.ToString();
        }
        #endregion
	}
}