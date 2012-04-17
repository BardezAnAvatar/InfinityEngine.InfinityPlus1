using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Components;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect.Effect1;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect.Effect2;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature
{
    /// <summary>Base class for any 2nd Edition Creature</summary>
    public abstract class Creature2EBase : IInfinityFormat
    {
        #region Members
        /// <summary>Creature header, most of the data</summary>
        protected Creature2eHeader Header2E { get; set; }

        /// <summary>List of known spells</summary>
        public List<Creature2EKnownSpell> KnownSpells { get; set; }

        /// <summary>List of available overlay memorization</summary>
        /// <remarks>Should always be 7 Priest, 9 Wizard, 1 Innate</remarks>
        public List<Creature2ESpellMemorization> SpellMemorization { get; set; }

        /// <summary>List of spels memorized and prepared for use</summary>
        public List<Creature2EMemorizedSpells> PreparedSpells { get; set; }

        /// <summary>List of effects the creature has temporarily or permanently applied.</summary>
        /// <remarks>Can be a list of either Effect1 or Effect2, so using the base struct</remarks>
        public List<EffectWrapper> Effects { get; set; }

        /// <summary>List of items the creature has in its inventory</summary>
        public List<ItemInstance> Items { get; set; }

        /// <summary>The item slots of available to the creature</summary>
        public GenericOrderedDictionary<String, Int16> ItemSlots { get; set; }

        /// <summary>Index to items indicating the currently selected weapon</summary>
        public Int16 SelectedWeapon { get; set; }

        /// <summary>Index to the selected weapon's currently selected ability</summary>
        public Int16 SelectedWeaponAbility { get; set; }
        #endregion


        #region Properties
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
            get { return this.Header2E.UseEffectStructureVersion2 ? Effect2Wrapper.StructSize : Effect1.StructSize; }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.InstantiateHeader();
            this.KnownSpells = new List<Creature2EKnownSpell>();
            this.SpellMemorization = new List<Creature2ESpellMemorization>();
            this.PreparedSpells = new List<Creature2EMemorizedSpells>();
            this.Effects = new List<EffectWrapper>();
            this.Items = new List<ItemInstance>();
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
                this.Header2E.Read(input, false);
            }
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();

            this.Header2E.Read(input);
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
            if (this.Header2E.OffsetKnownSpells > 0)
            {
                if (input.Position != this.Header2E.OffsetKnownSpells)
                    input.Seek(this.Header2E.OffsetKnownSpells, SeekOrigin.Begin);

                //read known spells
                for (Int32 i = 0; i < this.Header2E.CountKnownSpells; ++i)
                {
                    Creature2EKnownSpell spell = new Creature2EKnownSpell();
                    spell.Read(input);
                    this.KnownSpells.Add(spell);
                }
            }
        }

        /// <summary>Reads the list of spell preparation slots from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadSpellPreparation(Stream input)
        {
            if (this.Header2E.OffsetSpellMemorization > 0)
            {
                if (input.Position != this.Header2E.OffsetSpellMemorization)
                    input.Seek(this.Header2E.OffsetSpellMemorization, SeekOrigin.Begin);

                //read overlay memorization table
                for (Int32 i = 0; i < this.Header2E.CountSpellMemorizations; ++i)
                {
                    Creature2ESpellMemorization memorization = new Creature2ESpellMemorization();
                    memorization.Read(input);
                    this.SpellMemorization.Add(memorization);
                }
            }
        }

        /// <summary>Reads the list of prepared spells from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadPreparedSpells(Stream input)
        {
            if (this.Header2E.OffsetMemorizedSpells > 0)
            {
                if (input.Position != this.Header2E.OffsetMemorizedSpells)
                    input.Seek(this.Header2E.OffsetMemorizedSpells, SeekOrigin.Begin);

                //read prepared spells
                for (Int32 i = 0; i < this.Header2E.CountMemorizedSpells; ++i)
                {
                    Creature2EMemorizedSpells spell = new Creature2EMemorizedSpells();
                    spell.Read(input);
                    this.PreparedSpells.Add(spell);
                }
            }
        }

        /// <summary>Reads the list of effects from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadEffects(Stream input)
        {
            if (this.Header2E.OffsetEffects > 0)
            {
                if (input.Position != this.Header2E.OffsetEffects)
                    input.Seek(this.Header2E.OffsetEffects, SeekOrigin.Begin);

                //read effects
                for (Int32 i = 0; i < this.Header2E.CountEffects; ++i)
                {
                    EffectWrapper effect;

                    if (this.Header2E.UseEffectStructureVersion2)
                        effect = new Effect2Wrapper();
                    else
                        effect = new Effect1Wrapper();

                    effect.Read(input);
                    this.Effects.Add(effect);
                }
            }
        }

        /// <summary>Reads the list of items from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadItems(Stream input)
        {
            if (this.Header2E.OffsetItems > 0)
            {
                if (input.Position != this.Header2E.OffsetItems)
                    input.Seek(this.Header2E.OffsetItems, SeekOrigin.Begin);

                //read prepared spells
                for (Int32 i = 0; i < this.Header2E.CountItems; ++i)
                {
                    ItemInstance item = new ItemInstance();
                    item.Read(input);
                    this.Items.Add(item);
                }
            }
        }

        /// <summary>Reads the table of item slots from the input stream. Includes equipped weapon fields.</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadItemSlots(Stream input)
        {
            if (this.Header2E.OffsetItemSlots > 0)
            {
                if (input.Position != this.Header2E.OffsetItemSlots)
                    input.Seek(this.Header2E.OffsetItemSlots, SeekOrigin.Begin);

                Byte[] data = ReusableIO.BinaryRead(input, 4 + (this.ItemSlots.Count * 2 /* sizeof (UInt16) */));

                for (Int32 i = 0; i < this.ItemSlots.Count; ++i)
                    this.ItemSlots[i] = ReusableIO.ReadInt16FromArray(data, i * 2);

                //Trailing two
                this.SelectedWeapon = ReusableIO.ReadInt16FromArray(data, this.ItemSlots.Count * 2);
                this.SelectedWeaponAbility = ReusableIO.ReadInt16FromArray(data, (this.ItemSlots.Count * 2) + 2);
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            this.MaintainMinimalDataIntegrity();

            this.Header2E.Write(output);
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
            if (this.Header2E.OffsetKnownSpells > 0)
            {
                if (output.Position != this.Header2E.OffsetKnownSpells)
                    output.Seek(this.Header2E.OffsetKnownSpells, SeekOrigin.Begin);

                //write known spells
                foreach (Creature2EKnownSpell spell in this.KnownSpells)
                    spell.Write(output);
            }
        }

        /// <summary>Writes the list of overlay level preparation to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteSpellPreparation(Stream output)
        {
            if (this.Header2E.OffsetSpellMemorization > 0)
            {
                if (output.Position != this.Header2E.OffsetSpellMemorization)
                    output.Seek(this.Header2E.OffsetSpellMemorization, SeekOrigin.Begin);

                //write overlay memorization
                foreach (Creature2ESpellMemorization level in this.SpellMemorization)
                    level.Write(output);
            }
        }

        /// <summary>Writes the list of prepared spells to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WritePreparedSpells(Stream output)
        {
            if (this.Header2E.OffsetMemorizedSpells > 0)
            {
                if (output.Position != this.Header2E.OffsetMemorizedSpells)
                    output.Seek(this.Header2E.OffsetMemorizedSpells, SeekOrigin.Begin);

                //write overlay memorization
                foreach (Creature2EMemorizedSpells spell in this.PreparedSpells)
                    spell.Write(output);
            }
        }

        /// <summary>Writes the list of effects to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteEffects(Stream output)
        {
            if (this.Header2E.OffsetEffects > 0)
            {
                if (output.Position != this.Header2E.OffsetEffects)
                    output.Seek(this.Header2E.OffsetEffects, SeekOrigin.Begin);

                //write overlay memorization
                foreach (EffectWrapper effect in this.Effects)
                    effect.Write(output);
            }
        }

        /// <summary>Writes the list of items to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteItems(Stream output)
        {
            if (this.Header2E.OffsetItems > 0)
            {
                if (output.Position != this.Header2E.OffsetItems)
                    output.Seek(this.Header2E.OffsetItems, SeekOrigin.Begin);

                //write overlay memorization
                foreach (ItemInstance item in this.Items)
                    item.Write(output);
            }
        }

        /// <summary>Writes the table of item slots to the output stream. Includes equipped weapon fields.</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteItemSlots(Stream output)
        {
            if (this.Header2E.OffsetItemSlots > 0)
            {
                if (output.Position != this.Header2E.OffsetItemSlots)
                    output.Seek(this.Header2E.OffsetItemSlots, SeekOrigin.Begin);

                for (Int32 i = 0; i < this.ItemSlots.Count; ++i)
                    ReusableIO.WriteInt16ToStream(this.ItemSlots[i], output);

                //Trailing two
                ReusableIO.WriteInt16ToStream(this.SelectedWeapon, output);
                ReusableIO.WriteInt16ToStream(this.SelectedWeaponAbility, output);
            }
        }
        #endregion


        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            return this.ToString(true);
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="showType">Boolean indicating whether or not to display the leading description line.</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Boolean showType)
        {
            String representation = this.Headline;

            if (showType)
                representation += this.GetStringRepresentation();
            else
                representation = this.GetStringRepresentation();

            return representation;
        }
            
        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected virtual String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(this.Header2E.ToString());
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

            for (Int32 i = 0; i < this.KnownSpells.Count; ++i)
                sb.Append(this.KnownSpells[i].ToString(i + 1));
            
            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the list of memorizable overlay slots</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateSpellMemorizationString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.SpellMemorization.Count; ++i)
                sb.Append(this.SpellMemorization[i].ToString(i + 1));
            
            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the list of prepared spells</summary>
        /// <returns>A multi-line String</returns>
        protected String GeneratePreparedSpellsString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.PreparedSpells.Count; ++i)
                sb.Append(this.PreparedSpells[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the list of effects</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateEffectsString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.Effects.Count; ++i)
                sb.Append(this.Effects[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the list of items</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateItemsString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.Items.Count; ++i)
                sb.Append(this.Items[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the list of item slots</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateItemSlotsString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Item slots:");

            foreach (String key in this.ItemSlots.Keys)
            {
                sb.Append(StringFormat.ToStringAlignment("Item slot '" + key + "'"));
                sb.Append(this.ItemSlots[key]);
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
                Int32 knownSpellsSize = this.KnownSpells.Count * Creature2EKnownSpell.StructSize;
                Int32 spellMemorizationSlotsSize = this.SpellMemorization.Count * Creature2ESpellMemorization.StructSize;
                Int32 preparedSpellsSize = this.PreparedSpells.Count * Creature2EMemorizedSpells.StructSize;
                Int32 effectsSize = this.Effects.Count * this.EffectSize;
                Int32 itemsSize = this.Items.Count * ItemInstance.StructSize;
                Int32 itemSlotsSize = 4 /* trailing two selected indexes */ + (this.ItemSlots.Count * 2 /* sizeof(UInt16) */);

                //reset offsets
                this.Header2E.OffsetKnownSpells = this.HeaderSize;
                this.Header2E.OffsetSpellMemorization = this.Header2E.OffsetKnownSpells + knownSpellsSize;
                this.Header2E.OffsetMemorizedSpells = Convert.ToInt32(this.Header2E.OffsetSpellMemorization + spellMemorizationSlotsSize);
                this.Header2E.OffsetEffects = Convert.ToUInt32(this.Header2E.OffsetMemorizedSpells + preparedSpellsSize);
                this.Header2E.OffsetItems = Convert.ToUInt32(this.Header2E.OffsetEffects + effectsSize);
                this.Header2E.OffsetItemSlots = Convert.ToUInt32(this.Header2E.OffsetItems + itemsSize);
            }
        }

        /// <summary>Determines if any of the offset data sections would overlap one another.</summary>
        /// <returns>A Boolean indicating whether or not any of them overlap</returns>
        protected virtual Boolean Overlaps()
        {
            //6 variables
            Int32 knownSpellsSize = this.KnownSpells.Count * Creature2EKnownSpell.StructSize;
            Int32 spellMemorizationSlotsSize = this.SpellMemorization.Count * Creature2ESpellMemorization.StructSize;
            Int32 preparedSpellsSize = this.PreparedSpells.Count * Creature2EMemorizedSpells.StructSize;
            Int32 effectsSize = this.Effects.Count * this.EffectSize;
            Int32 itemsSize = this.Items.Count * ItemInstance.StructSize;
            Int32 itemSlotsSize = 4 /* trailing two selected indexes */ + (this.ItemSlots.Count * 2 /* sizeof(UInt16) */);

            Boolean overlaps = false;

            //technically, any of these 6 can follow the header in any order. Check for any overlaps.
            if (
                IntExtension.Between(this.Header2E.OffsetKnownSpells, knownSpellsSize, 0, this.HeaderSize)
                || IntExtension.Between(this.Header2E.OffsetSpellMemorization, spellMemorizationSlotsSize, 0, this.HeaderSize)
                || IntExtension.Between(this.Header2E.OffsetMemorizedSpells, preparedSpellsSize, 0, this.HeaderSize)
                || IntExtension.Between(this.Header2E.OffsetEffects, effectsSize, 0, this.HeaderSize)
                || IntExtension.Between(this.Header2E.OffsetItems, itemsSize, 0, this.HeaderSize)
                || IntExtension.Between(this.Header2E.OffsetItemSlots, itemSlotsSize, 0, this.HeaderSize)
                )
                overlaps = true;

            // It's 10:45 PM and I can't think of any better way of doing this shite than writing out all combinations, copy/paste, and then excluding the self.
            if (!overlaps)
            {
                overlaps = 
                    (
                        false
                       //|| IntExtension.Between(this.Header2E.OffsetKnownSpells, knownSpellsSize, this.Header2E.OffsetKnownSpells, this.Header2E.OffsetKnownSpells + knownSpellsSize)
                         || IntExtension.Between(this.Header2E.OffsetKnownSpells, knownSpellsSize, this.Header2E.OffsetSpellMemorization, this.Header2E.OffsetSpellMemorization + spellMemorizationSlotsSize)
                         || IntExtension.Between(this.Header2E.OffsetKnownSpells, knownSpellsSize, this.Header2E.OffsetMemorizedSpells, this.Header2E.OffsetMemorizedSpells + preparedSpellsSize)
                         || IntExtension.Between(this.Header2E.OffsetKnownSpells, knownSpellsSize, this.Header2E.OffsetEffects, this.Header2E.OffsetEffects + effectsSize)
                         || IntExtension.Between(this.Header2E.OffsetKnownSpells, knownSpellsSize, this.Header2E.OffsetItems, this.Header2E.OffsetItems + itemsSize)
                         || IntExtension.Between(this.Header2E.OffsetKnownSpells, knownSpellsSize, this.Header2E.OffsetItemSlots, this.Header2E.OffsetItemSlots + itemSlotsSize)

                         || IntExtension.Between(this.Header2E.OffsetSpellMemorization, spellMemorizationSlotsSize, this.Header2E.OffsetKnownSpells, this.Header2E.OffsetKnownSpells + knownSpellsSize)
                       //|| IntExtension.Between(this.Header2E.OffsetSpellMemorization, spellMemorizationSlotsSize, this.Header2E.OffsetSpellMemorization, this.Header2E.OffsetSpellMemorization + spellMemorizationSlotsSize)
                         || IntExtension.Between(this.Header2E.OffsetSpellMemorization, spellMemorizationSlotsSize, this.Header2E.OffsetMemorizedSpells, this.Header2E.OffsetMemorizedSpells + preparedSpellsSize)
                         || IntExtension.Between(this.Header2E.OffsetSpellMemorization, spellMemorizationSlotsSize, this.Header2E.OffsetEffects, this.Header2E.OffsetEffects + effectsSize)
                         || IntExtension.Between(this.Header2E.OffsetSpellMemorization, spellMemorizationSlotsSize, this.Header2E.OffsetItems, this.Header2E.OffsetItems + itemsSize)
                         || IntExtension.Between(this.Header2E.OffsetSpellMemorization, spellMemorizationSlotsSize, this.Header2E.OffsetItemSlots, this.Header2E.OffsetItemSlots + itemSlotsSize)

                         || IntExtension.Between(this.Header2E.OffsetMemorizedSpells, preparedSpellsSize, this.Header2E.OffsetKnownSpells, this.Header2E.OffsetKnownSpells + knownSpellsSize)
                         || IntExtension.Between(this.Header2E.OffsetMemorizedSpells, preparedSpellsSize, this.Header2E.OffsetSpellMemorization, this.Header2E.OffsetSpellMemorization + spellMemorizationSlotsSize)
                       //|| IntExtension.Between(this.Header2E.OffsetMemorizedSpells, preparedSpellsSize, this.Header2E.OffsetMemorizedSpells, this.Header2E.OffsetMemorizedSpells + preparedSpellsSize)
                         || IntExtension.Between(this.Header2E.OffsetMemorizedSpells, preparedSpellsSize, this.Header2E.OffsetEffects, this.Header2E.OffsetEffects + effectsSize)
                         || IntExtension.Between(this.Header2E.OffsetMemorizedSpells, preparedSpellsSize, this.Header2E.OffsetItems, this.Header2E.OffsetItems + itemsSize)
                         || IntExtension.Between(this.Header2E.OffsetMemorizedSpells, preparedSpellsSize, this.Header2E.OffsetItemSlots, this.Header2E.OffsetItemSlots + itemSlotsSize)

                         || IntExtension.Between(this.Header2E.OffsetEffects, effectsSize, this.Header2E.OffsetKnownSpells, this.Header2E.OffsetKnownSpells + knownSpellsSize)
                         || IntExtension.Between(this.Header2E.OffsetEffects, effectsSize, this.Header2E.OffsetSpellMemorization, this.Header2E.OffsetSpellMemorization + spellMemorizationSlotsSize)
                         || IntExtension.Between(this.Header2E.OffsetEffects, effectsSize, this.Header2E.OffsetMemorizedSpells, this.Header2E.OffsetMemorizedSpells + preparedSpellsSize)
                       //|| IntExtension.Between(this.Header2E.OffsetEffects, effectsSize, this.Header2E.OffsetEffects, this.Header2E.OffsetEffects + effectsSize)
                         || IntExtension.Between(this.Header2E.OffsetEffects, effectsSize, this.Header2E.OffsetItems, this.Header2E.OffsetItems + itemsSize)
                         || IntExtension.Between(this.Header2E.OffsetEffects, effectsSize, this.Header2E.OffsetItemSlots, this.Header2E.OffsetItemSlots + itemSlotsSize)

                         || IntExtension.Between(this.Header2E.OffsetItems, itemsSize, this.Header2E.OffsetKnownSpells, this.Header2E.OffsetKnownSpells + knownSpellsSize)
                         || IntExtension.Between(this.Header2E.OffsetItems, itemsSize, this.Header2E.OffsetSpellMemorization, this.Header2E.OffsetSpellMemorization + spellMemorizationSlotsSize)
                         || IntExtension.Between(this.Header2E.OffsetItems, itemsSize, this.Header2E.OffsetMemorizedSpells, this.Header2E.OffsetMemorizedSpells + preparedSpellsSize)
                         || IntExtension.Between(this.Header2E.OffsetItems, itemsSize, this.Header2E.OffsetEffects, this.Header2E.OffsetEffects + effectsSize)
                       //|| IntExtension.Between(this.Header2E.OffsetItems, itemsSize, this.Header2E.OffsetItems, this.Header2E.OffsetItems + itemsSize)
                         || IntExtension.Between(this.Header2E.OffsetItems, itemsSize, this.Header2E.OffsetItemSlots, this.Header2E.OffsetItemSlots + itemSlotsSize)

                         || IntExtension.Between(this.Header2E.OffsetItemSlots, itemSlotsSize, this.Header2E.OffsetKnownSpells, this.Header2E.OffsetKnownSpells + knownSpellsSize)
                         || IntExtension.Between(this.Header2E.OffsetItemSlots, itemSlotsSize, this.Header2E.OffsetSpellMemorization, this.Header2E.OffsetSpellMemorization + spellMemorizationSlotsSize)
                         || IntExtension.Between(this.Header2E.OffsetItemSlots, itemSlotsSize, this.Header2E.OffsetMemorizedSpells, this.Header2E.OffsetMemorizedSpells + preparedSpellsSize)
                         || IntExtension.Between(this.Header2E.OffsetItemSlots, itemSlotsSize, this.Header2E.OffsetEffects, this.Header2E.OffsetEffects + effectsSize)
                         || IntExtension.Between(this.Header2E.OffsetItemSlots, itemSlotsSize, this.Header2E.OffsetItems, this.Header2E.OffsetItems + itemsSize)
                       //|| IntExtension.Between(this.Header2E.OffsetItemSlots, itemSlotsSize, this.Header2E.OffsetItemSlots, this.Header2E.OffsetItemSlots + itemSlotsSize)
                    );
            }

            return overlaps;
        }
        #endregion
    }
}