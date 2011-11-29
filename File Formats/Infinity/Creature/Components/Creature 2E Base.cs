using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Components;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Effect;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Effect.Effect1;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Effect.Effect2;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Creature
{
    public abstract class Creature2EBase : IInfinityFormat
    {
        #region Members
        /// <summary>Creature header, most of the data</summary>
        protected Creature2eHeader header;

        /// <summary>List of known spells</summary>
        protected List<Creature2EKnownSpell> knownSpells;

        /// <summary>List of available overlay memorization</summary>
        /// <remarks>Should always be 7 Priest, 9 Wizard, 1 Innate</remarks>
        protected List<Creature2ESpellMemorization> spellMemorization;

        /// <summary>List of spels memorized and prepared for use</summary>
        protected List<Creature2EMemorizedSpells> preparedSpells;

        /// <summary>List of effects the creature has temporarily or permanently applied.</summary>
        /// <remarks>Can be a list of either Effect1 or Effect2, so using the base struct</remarks>
        protected List<EffectWrapper> effects;

        /// <summary>List of items the creature has in its inventory</summary>
        protected List<ItemInstance> items;

        /// <summary>The item slots of available to the creature</summary>
        protected GenericOrderedDictionary<String, Int16> itemSlots;

        /// <summary>Index to items indicating the currently selected weapon</summary>
        protected Int16 selectedWeapon;

        /// <summary>Index to the selected weapon's currently selected ability</summary>
        protected Int16 selectedWeaponAbility;
        #endregion

        #region Properties
        #region Members
        /// <summary>List of known spells</summary>
        public List<Creature2EKnownSpell> KnownSpells
        {
            get { return this.knownSpells; }
            set { this.knownSpells = value; }
        }

        /// <summary>List of available overlay memorization</summary>
        /// <remarks>Should always be 7 Priest, 9 Wizard, 1 Innate</remarks>
        public List<Creature2ESpellMemorization> SpellMemorization
        {
            get { return this.spellMemorization; }
            set { this.spellMemorization = value; }
        }

        /// <summary>List of spels memorized and prepared for use</summary>
        public List<Creature2EMemorizedSpells> PreparedSpells
        {
            get { return this.preparedSpells; }
            set { this.preparedSpells = value; }
        }

        /// <summary>List of effects the creature has temporarily or permanently applied.</summary>
        /// <remarks>Can be a list of either Effect1 or Effect2, so using the base struct</remarks>
        public List<EffectWrapper> Effects
        {
            get { return this.effects; }
            set { this.effects = value; }
        }

        /// <summary>List of items the creature has in its inventory</summary>
        public List<ItemInstance> Items
        {
            get { return this.items; }
            set { this.items = value; }
        }

        /// <summary>The item slots of available to the creature</summary>
        public GenericOrderedDictionary<String, Int16> ItemSlots
        {
            get { return this.itemSlots; }
            set { this.itemSlots = value; }
        }

        /// <summary>Index to items indicating the currently selected weapon</summary>
        public Int16 SelectedWeapon
        {
            get { return this.selectedWeapon; }
            set { this.selectedWeapon = value; }
        }

        /// <summary>Index to the selected weapon's currently selected ability</summary>
        public Int16 SelectedWeaponAbility
        {
            get { return this.selectedWeaponAbility; }
            set { this.selectedWeaponAbility = value; }
        }
        #endregion

        /// <summary>Gets the headline for the creature file</summary>
        public abstract String Headline
        {
            get;
        }

        /// <summary>Gets the size of the header</summary>
        public abstract Int32 HeaderSize
        {
            get;
        }

        /// <summary>Gets the size of the effects</summary>
        public Int32 EffectSize
        {
            get { return this.header.UseEffectStructureVersion2 ? Effect2Wrapper.StructSize : Effect1.StructSize; }
        }
        #endregion

        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.InstantiateHeader();
            this.knownSpells = new List<Creature2EKnownSpell>();
            this.spellMemorization = new List<Creature2ESpellMemorization>();
            this.preparedSpells = new List<Creature2EMemorizedSpells>();
            this.effects = new List<EffectWrapper>();
            this.items = new List<ItemInstance>();
            this.InitializeItemSlots();
        }

        /// <summary>Initializes the item slots ordered dictionary</summary>
        protected abstract void InitializeItemSlots();

        /// <summary>Instantiates a new header</summary>
        protected abstract void InstantiateHeader();
        #endregion

        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            if (fullRead)
                this.Read(input);
            else
            {
                this.InstantiateHeader();
                this.header.Read(input, false);
            }
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();

            this.header.Read(input);
            this.ReadKnownSpells(input);
            this.ReadSpellPreparation(input);
            this.ReadPreparedSpells(input);
            this.ReadEffects(input);
            this.ReadItems(input);
            this.ReadItemSlots(input);
        }

        /// <summary>Reads the list of known spells from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadKnownSpells(Stream input)
        {
            if (input.Position != this.header.OffsetKnownSpells)
                input.Seek(this.header.OffsetKnownSpells, SeekOrigin.Begin);

            //read known spells
            for (Int32 i = 0; i < this.header.CountKnownSpells; ++i)
            {
                Creature2EKnownSpell spell = new Creature2EKnownSpell();
                spell.Read(input);
                this.knownSpells.Add(spell);
            }
        }

        /// <summary>Reads the list of spell preparation slots from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadSpellPreparation(Stream input)
        {
            if (input.Position != this.header.OffsetSpellMemorization)
                input.Seek(this.header.OffsetSpellMemorization, SeekOrigin.Begin);

            //read overlay memorization table
            for (Int32 i = 0; i < this.header.CountSpellMemorizations; ++i)
            {
                Creature2ESpellMemorization memorization = new Creature2ESpellMemorization();
                memorization.Read(input);
                this.spellMemorization.Add(memorization);
            }
        }

        /// <summary>Reads the list of prepared spells from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadPreparedSpells(Stream input)
        {
            if (input.Position != this.header.OffsetMemorizedSpells)
                input.Seek(this.header.OffsetMemorizedSpells, SeekOrigin.Begin);

            //read prepared spells
            for (Int32 i = 0; i < this.header.CountMemorizedSpells; ++i)
            {
                Creature2EMemorizedSpells spell = new Creature2EMemorizedSpells();
                spell.Read(input);
                this.preparedSpells.Add(spell);
            }
        }

        /// <summary>Reads the list of effects from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadEffects(Stream input)
        {
            if (input.Position != this.header.OffsetEffects)
                input.Seek(this.header.OffsetEffects, SeekOrigin.Begin);

            //read effects
            for (Int32 i = 0; i < this.header.CountEffects; ++i)
            {
                EffectWrapper effect;
                    
                if (this.header.UseEffectStructureVersion2)
                    effect = new Effect2Wrapper();
                else
                    effect = new Effect1Wrapper();

                effect.Read(input);
                this.effects.Add(effect);
            }
        }

        /// <summary>Reads the list of items from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadItems(Stream input)
        {
            if (input.Position != this.header.OffsetItems)
                input.Seek(this.header.OffsetItems, SeekOrigin.Begin);

            //read prepared spells
            for (Int32 i = 0; i < this.header.CountItems; ++i)
            {
                ItemInstance item = new ItemInstance();
                item.Read(input);
                this.items.Add(item);
            }
        }

        /// <summary>Reads the table of item slots from the input stream. Includes equipped weapon fields.</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadItemSlots(Stream input)
        {
            if (input.Position != this.header.OffsetItemSlots)
                input.Seek(this.header.OffsetItemSlots, SeekOrigin.Begin);

            Byte[] data = ReusableIO.BinaryRead(input, 4 + (this.itemSlots.Count * 2 /* sizeof (UInt16) */));

            for (Int32 i = 0; i < this.itemSlots.Count; ++i)
                this.itemSlots[i] = ReusableIO.ReadInt16FromArray(data, i * 2);

            //Trailing two
            this.selectedWeapon = ReusableIO.ReadInt16FromArray(data, this.itemSlots.Count * 2);
            this.selectedWeaponAbility = ReusableIO.ReadInt16FromArray(data, (this.itemSlots.Count * 2) + 2);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            this.MaintainMinimalDataIntegrity();

            this.header.Write(output);
            this.WriteKnownSpells(output);
            this.WriteSpellPreparation(output);
            this.WritePreparedSpells(output);
            this.WriteEffects(output);
            this.WriteItems(output);
            this.WriteItemSlots(output);
        }

        /// <summary>Writes the list of known spells to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteKnownSpells(Stream output)
        {
            if (output.Position != this.header.OffsetKnownSpells)
                output.Seek(this.header.OffsetKnownSpells, SeekOrigin.Begin);

            //write known spells
            foreach (Creature2EKnownSpell spell in this.knownSpells)
                spell.Write(output);
        }

        /// <summary>Writes the list of overlay level preparation to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteSpellPreparation(Stream output)
        {
            if (output.Position != this.header.OffsetSpellMemorization)
                output.Seek(this.header.OffsetSpellMemorization, SeekOrigin.Begin);

            //write overlay memorization
            foreach (Creature2ESpellMemorization level in this.spellMemorization)
                level.Write(output);
        }

        /// <summary>Writes the list of prepared spells to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WritePreparedSpells(Stream output)
        {
            if (output.Position != this.header.OffsetMemorizedSpells)
                output.Seek(this.header.OffsetMemorizedSpells, SeekOrigin.Begin);

            //write overlay memorization
            foreach (Creature2EMemorizedSpells spell in this.preparedSpells)
                spell.Write(output);
        }

        /// <summary>Writes the list of effects to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteEffects(Stream output)
        {
            if (output.Position != this.header.OffsetEffects)
                output.Seek(this.header.OffsetEffects, SeekOrigin.Begin);

            //write overlay memorization
            foreach (EffectWrapper effect in this.effects)
                effect.Write(output);
        }

        /// <summary>Writes the list of items to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteItems(Stream output)
        {
            if (output.Position != this.header.OffsetItems)
                output.Seek(this.header.OffsetItems, SeekOrigin.Begin);

            //write overlay memorization
            foreach (ItemInstance item in this.items)
                item.Write(output);
        }

        /// <summary>Writes the table of item slots to the output stream. Includes equipped weapon fields.</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteItemSlots(Stream output)
        {
            if (output.Position != this.header.OffsetItemSlots)
                output.Seek(this.header.OffsetItemSlots, SeekOrigin.Begin);

            for (Int32 i = 0; i < this.itemSlots.Count; ++i)
                ReusableIO.WriteInt16ToStream(this.itemSlots[i], output);

            //Trailing two
            ReusableIO.WriteInt16ToStream(this.selectedWeapon, output);
            ReusableIO.WriteInt16ToStream(this.selectedWeaponAbility, output);
        }
        #endregion

        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(this.Headline);
            builder.AppendLine(this.header.ToString());
            builder.AppendLine(this.GenerateKnownSpellsString());
            builder.AppendLine(this.GenerateSpellMemorizationString());
            builder.AppendLine(this.GeneratePreparedSpellsString());
            builder.AppendLine(this.GenerateEffectsString());
            builder.AppendLine(this.GenerateItemsString());
            builder.AppendLine(this.GenerateItemSlotsString());

            return builder.ToString();
        }

        /// <summary>Generates a human-readable console output describing the list of known spells</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateKnownSpellsString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.knownSpells.Count; ++i)
                sb.Append(this.knownSpells[i].ToString(i + 1));
            
            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the list of memorizable overlay slots</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateSpellMemorizationString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.spellMemorization.Count; ++i)
                sb.Append(this.spellMemorization[i].ToString(i + 1));
            
            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the list of prepared spells</summary>
        /// <returns>A multi-line String</returns>
        protected String GeneratePreparedSpellsString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.preparedSpells.Count; ++i)
                sb.Append(this.preparedSpells[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the list of effects</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateEffectsString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.effects.Count; ++i)
                sb.Append(this.effects[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the list of items</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateItemsString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.items.Count; ++i)
                sb.Append(this.items[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the list of item slots</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateItemSlotsString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Item slots:");

            foreach (String key in this.itemSlots.Keys)
            {
                sb.Append(StringFormat.ToStringAlignment("Item slot '" + key + "'"));
                sb.Append(this.itemSlots[key]);
            }

            return sb.ToString();
        }
        #endregion

        #region Data Integrity
        /// <summary>Maintains mimimal data integrity by not lying to the output data file.</summary>
        protected virtual void MaintainMinimalDataIntegrity()
        {
            if (this.Overlaps())
            {
                //6 variables
                Int32 knownSpellsSize = this.knownSpells.Count * Creature2EKnownSpell.StructSize;
                Int32 spellMemorizationSlotsSize = this.spellMemorization.Count * Creature2ESpellMemorization.StructSize;
                Int32 preparedSpellsSize = this.preparedSpells.Count * Creature2EMemorizedSpells.StructSize;
                Int32 effectsSize = this.effects.Count * this.EffectSize;
                Int32 itemsSize = this.items.Count * ItemInstance.StructSize;
                Int32 itemSlotsSize = 4 /* trailing two selected indexes */ + (this.itemSlots.Count * 2 /* sizeof(UInt16) */);

                //reset offsets
                this.header.OffsetKnownSpells = Convert.ToUInt32(this.HeaderSize);
                this.header.OffsetSpellMemorization = Convert.ToUInt32(this.header.OffsetKnownSpells + knownSpellsSize);
                this.header.OffsetMemorizedSpells = Convert.ToUInt32(this.header.OffsetSpellMemorization + spellMemorizationSlotsSize);
                this.header.OffsetEffects = Convert.ToUInt32(this.header.OffsetMemorizedSpells + preparedSpellsSize);
                this.header.OffsetItems = Convert.ToUInt32(this.header.OffsetEffects + effectsSize);
                this.header.OffsetItemSlots = Convert.ToUInt32(this.header.OffsetItems + itemsSize);
            }
        }

        /// <summary>Determines if any of the offset data sections would overlap one another.</summary>
        /// <returns>A Boolean indicating whether or not any of them overlap</returns>
        protected virtual Boolean Overlaps()
        {
            //6 variables
            Int32 knownSpellsSize = this.knownSpells.Count * Creature2EKnownSpell.StructSize;
            Int32 spellMemorizationSlotsSize = this.spellMemorization.Count * Creature2ESpellMemorization.StructSize;
            Int32 preparedSpellsSize = this.preparedSpells.Count * Creature2EMemorizedSpells.StructSize;
            Int32 effectsSize = this.effects.Count * this.EffectSize;
            Int32 itemsSize = this.items.Count * ItemInstance.StructSize;
            Int32 itemSlotsSize = 4 /* trailing two selected indexes */ + (this.itemSlots.Count * 2 /* sizeof(UInt16) */);

            Boolean overlaps = false;

            //technically, any of these 6 can follow the header in any order. Check for any overlaps.
            if (
                IntExtension.Between(this.header.OffsetKnownSpells, knownSpellsSize, 0, this.HeaderSize)
                || IntExtension.Between(this.header.OffsetSpellMemorization, spellMemorizationSlotsSize, 0, this.HeaderSize)
                || IntExtension.Between(this.header.OffsetMemorizedSpells, preparedSpellsSize, 0, this.HeaderSize)
                || IntExtension.Between(this.header.OffsetEffects, effectsSize, 0, this.HeaderSize)
                || IntExtension.Between(this.header.OffsetItems, itemsSize, 0, this.HeaderSize)
                || IntExtension.Between(this.header.OffsetItemSlots, itemSlotsSize, 0, this.HeaderSize)
                )
                overlaps = true;

            // It's 10:45 PM and I can't think of any better way of doing this shite than writing out all combinations, copy/paste, and then excluding the self.
            if (!overlaps)
            {
                overlaps = 
                    (
                        false
                       //|| IntExtension.Between(this.header.OffsetKnownSpells, knownSpellsSize, this.header.OffsetKnownSpells, this.header.OffsetKnownSpells + knownSpellsSize)
                         || IntExtension.Between(this.header.OffsetKnownSpells, knownSpellsSize, this.header.OffsetSpellMemorization, this.header.OffsetSpellMemorization + spellMemorizationSlotsSize)
                         || IntExtension.Between(this.header.OffsetKnownSpells, knownSpellsSize, this.header.OffsetMemorizedSpells, this.header.OffsetMemorizedSpells + preparedSpellsSize)
                         || IntExtension.Between(this.header.OffsetKnownSpells, knownSpellsSize, this.header.OffsetEffects, this.header.OffsetEffects + effectsSize)
                         || IntExtension.Between(this.header.OffsetKnownSpells, knownSpellsSize, this.header.OffsetItems, this.header.OffsetItems + itemsSize)
                         || IntExtension.Between(this.header.OffsetKnownSpells, knownSpellsSize, this.header.OffsetItemSlots, this.header.OffsetItemSlots + itemSlotsSize)

                         || IntExtension.Between(this.header.OffsetSpellMemorization, spellMemorizationSlotsSize, this.header.OffsetKnownSpells, this.header.OffsetKnownSpells + knownSpellsSize)
                       //|| IntExtension.Between(this.header.OffsetSpellMemorization, spellMemorizationSlotsSize, this.header.OffsetSpellMemorization, this.header.OffsetSpellMemorization + spellMemorizationSlotsSize)
                         || IntExtension.Between(this.header.OffsetSpellMemorization, spellMemorizationSlotsSize, this.header.OffsetMemorizedSpells, this.header.OffsetMemorizedSpells + preparedSpellsSize)
                         || IntExtension.Between(this.header.OffsetSpellMemorization, spellMemorizationSlotsSize, this.header.OffsetEffects, this.header.OffsetEffects + effectsSize)
                         || IntExtension.Between(this.header.OffsetSpellMemorization, spellMemorizationSlotsSize, this.header.OffsetItems, this.header.OffsetItems + itemsSize)
                         || IntExtension.Between(this.header.OffsetSpellMemorization, spellMemorizationSlotsSize, this.header.OffsetItemSlots, this.header.OffsetItemSlots + itemSlotsSize)

                         || IntExtension.Between(this.header.OffsetMemorizedSpells, preparedSpellsSize, this.header.OffsetKnownSpells, this.header.OffsetKnownSpells + knownSpellsSize)
                         || IntExtension.Between(this.header.OffsetMemorizedSpells, preparedSpellsSize, this.header.OffsetSpellMemorization, this.header.OffsetSpellMemorization + spellMemorizationSlotsSize)
                       //|| IntExtension.Between(this.header.OffsetMemorizedSpells, preparedSpellsSize, this.header.OffsetMemorizedSpells, this.header.OffsetMemorizedSpells + preparedSpellsSize)
                         || IntExtension.Between(this.header.OffsetMemorizedSpells, preparedSpellsSize, this.header.OffsetEffects, this.header.OffsetEffects + effectsSize)
                         || IntExtension.Between(this.header.OffsetMemorizedSpells, preparedSpellsSize, this.header.OffsetItems, this.header.OffsetItems + itemsSize)
                         || IntExtension.Between(this.header.OffsetMemorizedSpells, preparedSpellsSize, this.header.OffsetItemSlots, this.header.OffsetItemSlots + itemSlotsSize)

                         || IntExtension.Between(this.header.OffsetEffects, effectsSize, this.header.OffsetKnownSpells, this.header.OffsetKnownSpells + knownSpellsSize)
                         || IntExtension.Between(this.header.OffsetEffects, effectsSize, this.header.OffsetSpellMemorization, this.header.OffsetSpellMemorization + spellMemorizationSlotsSize)
                         || IntExtension.Between(this.header.OffsetEffects, effectsSize, this.header.OffsetMemorizedSpells, this.header.OffsetMemorizedSpells + preparedSpellsSize)
                       //|| IntExtension.Between(this.header.OffsetEffects, effectsSize, this.header.OffsetEffects, this.header.OffsetEffects + effectsSize)
                         || IntExtension.Between(this.header.OffsetEffects, effectsSize, this.header.OffsetItems, this.header.OffsetItems + itemsSize)
                         || IntExtension.Between(this.header.OffsetEffects, effectsSize, this.header.OffsetItemSlots, this.header.OffsetItemSlots + itemSlotsSize)

                         || IntExtension.Between(this.header.OffsetItems, itemsSize, this.header.OffsetKnownSpells, this.header.OffsetKnownSpells + knownSpellsSize)
                         || IntExtension.Between(this.header.OffsetItems, itemsSize, this.header.OffsetSpellMemorization, this.header.OffsetSpellMemorization + spellMemorizationSlotsSize)
                         || IntExtension.Between(this.header.OffsetItems, itemsSize, this.header.OffsetMemorizedSpells, this.header.OffsetMemorizedSpells + preparedSpellsSize)
                         || IntExtension.Between(this.header.OffsetItems, itemsSize, this.header.OffsetEffects, this.header.OffsetEffects + effectsSize)
                       //|| IntExtension.Between(this.header.OffsetItems, itemsSize, this.header.OffsetItems, this.header.OffsetItems + itemsSize)
                         || IntExtension.Between(this.header.OffsetItems, itemsSize, this.header.OffsetItemSlots, this.header.OffsetItemSlots + itemSlotsSize)

                         || IntExtension.Between(this.header.OffsetItemSlots, itemSlotsSize, this.header.OffsetKnownSpells, this.header.OffsetKnownSpells + knownSpellsSize)
                         || IntExtension.Between(this.header.OffsetItemSlots, itemSlotsSize, this.header.OffsetSpellMemorization, this.header.OffsetSpellMemorization + spellMemorizationSlotsSize)
                         || IntExtension.Between(this.header.OffsetItemSlots, itemSlotsSize, this.header.OffsetMemorizedSpells, this.header.OffsetMemorizedSpells + preparedSpellsSize)
                         || IntExtension.Between(this.header.OffsetItemSlots, itemSlotsSize, this.header.OffsetEffects, this.header.OffsetEffects + effectsSize)
                         || IntExtension.Between(this.header.OffsetItemSlots, itemSlotsSize, this.header.OffsetItems, this.header.OffsetItems + itemsSize)
                       //|| IntExtension.Between(this.header.OffsetItemSlots, itemSlotsSize, this.header.OffsetItemSlots, this.header.OffsetItemSlots + itemSlotsSize)
                    );
            }

            return overlaps;
        }
        #endregion
    }
}