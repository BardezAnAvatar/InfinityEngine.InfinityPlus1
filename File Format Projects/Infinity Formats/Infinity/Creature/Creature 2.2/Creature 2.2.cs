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

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Creature2_2
{
    /// <summary>Creature version 2.2 data struction</summary>
    /// <remarks>
    ///     It appears that IESDP lies or is at the very least misleading.
    ///     For known spells and memorized spells, it suggests an offset for each or stored spearately.
    ///     In fact, it would apear that instead the data struction is as follows:
    ///     
    ///     Offset to Class Spell level ->
    ///     {
    ///         KnownSpellsArray[Count]
    ///             { index to spell, memorized, remaining, unknown },
    ///         memorized slots,
    ///         free slot
    ///     }
    /// </remarks>
    public class Creature2_2 : IInfinityFormat
    {
        #region Fields
        /// <summary>Creature header, most of the data</summary>
        protected Creature2_2Header header;

        /// <summary>List of known spells & caster memorization</summary>
        public GenericOrderedDictionary<String, D20SpellList> Spells { get; set; }

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
        /// <summary>Creature header, most of the data</summary>
        public Creature2_2Header Header
        {
            get { return this.header; }
            set { this.header = value; }
        }

        /// <summary>Gets the headline for the creature file</summary>
        public String Headline
        {
            get { return "Creature 2.2:"; }
        }

        /// <summary>Gets the size of the header</summary>
        public Int32 HeaderSize
        {
            get { return Creature2_2Header.StructSize; }
        }

        /// <summary>Gets the size of the effects</summary>
        public Int32 EffectSize
        {
            get { return this.header.UseEffectStructureVersion2 ? Effect2Wrapper.StructSize : Effect1.StructSize; }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.InstantiateHeader();
            this.Spells = new GenericOrderedDictionary<String, D20SpellList>();
            this.Effects = new List<EffectWrapper>();
            this.Items = new List<ItemInstance>();
            this.InitializeItemSlots();
        }

        /// <summary>Initializes the item slots ordered dictionary</summary>
        protected void InitializeItemSlots()
        {
            this.ItemSlots = new GenericOrderedDictionary<String, Int16>();

            /* 01 */ this.ItemSlots.Add("Helmet", 0);
            /* 02 */ this.ItemSlots.Add("Armor", 0);
            /* 03 */ this.ItemSlots.Add("Unknown (Shield)", 0);
            /* 04 */ this.ItemSlots.Add("Gloves", 0);
            /* 05 */ this.ItemSlots.Add("Left Ring", 0);
            /* 06 */ this.ItemSlots.Add("Right Ring", 0);
            /* 07 */ this.ItemSlots.Add("Amulet", 0);
            /* 08 */ this.ItemSlots.Add("Belt", 0);
            /* 09 */ this.ItemSlots.Add("Boots", 0);
            /* 10 */ this.ItemSlots.Add("Weapon 1", 0);
            /* 11 */ this.ItemSlots.Add("Shield 1", 0);
            /* 12 */ this.ItemSlots.Add("Weapon 2", 0);
            /* 13 */ this.ItemSlots.Add("Shield 2", 0);
            /* 14 */ this.ItemSlots.Add("Weapon 3", 0);
            /* 15 */ this.ItemSlots.Add("Shield 3", 0);
            /* 16 */ this.ItemSlots.Add("Weapon 4", 0);
            /* 17 */ this.ItemSlots.Add("Shield 4", 0);
            /* 19 */ this.ItemSlots.Add("Quiver 1", 0);
            /* 20 */ this.ItemSlots.Add("Quiver 2", 0);
            /* 21 */ this.ItemSlots.Add("Quiver 3", 0);
            /* 22 */ this.ItemSlots.Add("Quiver 4", 0);
            /* 18 */ this.ItemSlots.Add("Cloak", 0);
            /* 23 */ this.ItemSlots.Add("Quick item 1", 0);
            /* 24 */ this.ItemSlots.Add("Quick item 2", 0);
            /* 25 */ this.ItemSlots.Add("Quick item 3", 0);
            /* 26 */ this.ItemSlots.Add("Inventory 01", 0);
            /* 27 */ this.ItemSlots.Add("Inventory 02", 0);
            /* 28 */ this.ItemSlots.Add("Inventory 03", 0);
            /* 29 */ this.ItemSlots.Add("Inventory 04", 0);
            /* 30 */ this.ItemSlots.Add("Inventory 05", 0);
            /* 31 */ this.ItemSlots.Add("Inventory 06", 0);
            /* 32 */ this.ItemSlots.Add("Inventory 07", 0);
            /* 33 */ this.ItemSlots.Add("Inventory 08", 0);
            /* 34 */ this.ItemSlots.Add("Inventory 09", 0);
            /* 35 */ this.ItemSlots.Add("Inventory 10", 0);
            /* 36 */ this.ItemSlots.Add("Inventory 11", 0);
            /* 37 */ this.ItemSlots.Add("Inventory 12", 0);
            /* 38 */ this.ItemSlots.Add("Inventory 13", 0);
            /* 39 */ this.ItemSlots.Add("Inventory 14", 0);
            /* 40 */ this.ItemSlots.Add("Inventory 15", 0);
            /* 41 */ this.ItemSlots.Add("Inventory 16", 0);
            /* 42 */ this.ItemSlots.Add("Inventory 17", 0);
            /* 43 */ this.ItemSlots.Add("Inventory 18", 0);
            /* 44 */ this.ItemSlots.Add("Inventory 19", 0);
            /* 45 */ this.ItemSlots.Add("Inventory 20", 0);
            /* 46 */ this.ItemSlots.Add("Inventory 21", 0);
            /* 47 */ this.ItemSlots.Add("Inventory 22", 0);
            /* 48 */ this.ItemSlots.Add("Inventory 23", 0);
            /* 49 */ this.ItemSlots.Add("Inventory 24", 0);
            /* 50 */ this.ItemSlots.Add("Magic weapon", 0);
        }

        /// <summary>Instantiates a new header</summary>
        protected void InstantiateHeader()
        {
            this.header = new Creature2_2Header();
        }
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
        public void ReadBody(Stream input)
        {
            this.Initialize();

            this.header.Read(input);
            this.ReadKnownSpells(input);
            this.ReadEffects(input);
            this.ReadItems(input);
            this.ReadItemSlots(input);
        }

        /// <summary>Reads the list of known spells from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadKnownSpells(Stream input)
        {
            foreach (String key in this.header.SpellOffsets.Keys)
                this.ReadKnownLevelSpells(input, this.header.SpellOffsets[key].Offset, this.header.SpellOffsets[key].Count, key);
        }
        
        /// <summary>Reads the list of known spells from the input stream for the spell group</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="offset">Offset to read from</param>
        /// <param name="count">Count of spells to read</param>
        /// <param name="key">Dictionary key to use during add</param>
        protected void ReadKnownLevelSpells(Stream input, UInt32 offset, UInt32 count, String key)
        {
            if (input.Position != offset)
                input.Seek(offset, SeekOrigin.Begin);

            //read known spells
            D20SpellList spellList = new D20SpellList();
            spellList.ReadBody(input, count);
            this.Spells.Add(key, spellList);
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
                this.Effects.Add(effect);
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
                this.Items.Add(item);
            }
        }

        /// <summary>Reads the table of item slots from the input stream. Includes equipped weapon fields.</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadItemSlots(Stream input)
        {
            if (input.Position != this.header.OffsetItemSlots)
                input.Seek(this.header.OffsetItemSlots, SeekOrigin.Begin);

            Byte[] data = ReusableIO.BinaryRead(input, 4 + (this.ItemSlots.Count * 2 /* sizeof (UInt16) */));

            for (Int32 i = 0; i < this.ItemSlots.Count; ++i)
                this.ItemSlots[i] = ReusableIO.ReadInt16FromArray(data, i * 2);

            //Trailing two
            this.SelectedWeapon = ReusableIO.ReadInt16FromArray(data, this.ItemSlots.Count * 2);
            this.SelectedWeaponAbility = ReusableIO.ReadInt16FromArray(data, (this.ItemSlots.Count * 2) + 2);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            //this.MaintainMinimalDataIntegrity();

            this.header.Write(output);
            this.WriteKnownSpells(output);
            this.WriteEffects(output);
            this.WriteItems(output);
            this.WriteItemSlots(output);
        }

        /// <summary>Reads the list of known spells from the input stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteKnownSpells(Stream output)
        {
            foreach (String key in this.header.SpellOffsets.Keys)
                this.WriteKnownLevelSpells(output, this.header.SpellOffsets[key].Offset, key);
        }

        /// <summary>Reads the list of known spells from the input stream for the spell group</summary>
        /// <param name="output">Stream to write to</param>
        /// <param name="offset">Offset to read from</param>
        /// <param name="key">Dictionary key to use during add</param>
        protected void WriteKnownLevelSpells(Stream output, UInt32 offset, String key)
        {
            if (output.Position != offset)
                output.Seek(offset, SeekOrigin.Begin);

            //read known spells
            this.Spells[key].Write(output);
        }

        /// <summary>Writes the list of effects to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteEffects(Stream output)
        {
            if (output.Position != this.header.OffsetEffects)
                output.Seek(this.header.OffsetEffects, SeekOrigin.Begin);

            //write overlay memorization
            foreach (EffectWrapper effect in this.Effects)
                effect.Write(output);
        }

        /// <summary>Writes the list of items to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteItems(Stream output)
        {
            if (output.Position != this.header.OffsetItems)
                output.Seek(this.header.OffsetItems, SeekOrigin.Begin);

            //write overlay memorization
            foreach (ItemInstance item in this.Items)
                item.Write(output);
        }

        /// <summary>Writes the table of item slots to the output stream. Includes equipped weapon fields.</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteItemSlots(Stream output)
        {
            if (output.Position != this.header.OffsetItemSlots)
                output.Seek(this.header.OffsetItemSlots, SeekOrigin.Begin);

            for (Int32 i = 0; i < this.ItemSlots.Count; ++i)
                ReusableIO.WriteInt16ToStream(this.ItemSlots[i], output);

            //Trailing two
            ReusableIO.WriteInt16ToStream(this.SelectedWeapon, output);
            ReusableIO.WriteInt16ToStream(this.SelectedWeaponAbility, output);
        }
        #endregion


        #region Data Integrity
        /// <summary>Maintains mimimal data integrity by not lying to the output data file.</summary>
        /// <remarks>
        ///     IWD2 offsets are LAME -- 9*8 + 3 + 3 offsets... that's 78*77 matches = 6006, joined
        ///     I could match one to itself easily enough in code, but then I would get all those false positives.
        ///     
        ///     I could have a match to method, and, say 78 bits to check exclusion on,
        ///     but that's so heavy handed... but might be the only approach.
        /// </remarks>
        protected void MaintainMinimalDataIntegrity()
        {
            if (this.Overlaps())
            {
                //set first
                this.header.SpellOffsets[0].Offset = Convert.ToUInt32(this.HeaderSize);

                for (Int32 index = 1; index < this.header.SpellOffsets.Count; ++index)
                {
                    //set rest
                    this.header.SpellOffsets[index].Offset = this.header.SpellOffsets[index - 1].OffsetEnd;
                }

                Int32 itemsSize = this.Items.Count * ItemInstance.StructSize;
                Int32 itemSlotsSize = 4 /* trailing two selected indexes */ + (this.ItemSlots.Count * 2 /* sizeof(UInt16) */);

                //items
                this.header.OffsetItems = this.header.SpellOffsets[this.header.SpellOffsets.Count - 1].OffsetEnd;

                //item slots
                this.header.OffsetItemSlots = this.header.OffsetItems + Convert.ToUInt32(itemsSize);

                //effects
                this.header.OffsetEffects = this.header.OffsetItemSlots + Convert.ToUInt32(itemSlotsSize);
            }
        }

        /// <summary>Determines if any of the offset data sections would overlap one another.</summary>
        /// <returns>A Boolean indicating whether or not any of them overlap</returns>
        /// <remarks>IWD2 offsets are LAME</remarks>
        protected Boolean Overlaps()
        {
            return this.OverlapBaseAnyBase()    //base on base
                || this.OverlapBaseAnySpell()   //base on spell
                || this.OverlapSpellAny();      //spell on both
        }

        /// <summary>Determines if any offsets overlap with the header, Items, Item SLots or Effects</summary>
        /// <param name="offset">Offset in question</param>
        /// <param name="size">data size at offset in question</param>
        /// <returns>True if there is any overlap</returns>
        protected Boolean OverlapsSpellAnyBaseOffsets(UInt32 offset, UInt32 size)
        {
            Int32 effectsSize = this.Effects.Count * this.EffectSize;
            Int32 itemsSize = this.Items.Count * ItemInstance.StructSize;
            Int32 itemSlotsSize = 4 /* trailing two selected indexes */ + (this.ItemSlots.Count * 2 /* sizeof(UInt16) */);

            //technically, any of these can follow the header in any order. Check for any overlaps with the header.
            Boolean overlaps =
                     IntExtension.Between(offset, size, 0, this.HeaderSize)
                  || IntExtension.Between(offset, size, this.header.OffsetItems, this.header.OffsetItems + itemsSize)
                  || IntExtension.Between(offset, size, this.header.OffsetItemSlots, this.header.OffsetItemSlots + itemSlotsSize)
                  || IntExtension.Between(offset, size, this.header.OffsetEffects, this.header.OffsetEffects + effectsSize);

            return overlaps;
        }

        /// <summary>Determines if any offsets overlap with other offsets</summary>
        /// <param name="offset">Offset in question</param>
        /// <param name="size">data size at offset in question</param>
        /// <param name="key">key of offset in dictionary, used to exclude a match</param>
        /// <returns>True if there is any overlap</returns>
        protected Boolean OverlapsSpellOffsets(UInt32 offset, Int64 size, String key)
        {
            Boolean overlaps = false;

            foreach (String index in this.header.SpellOffsets)
            {
                if (key != index) //do not check yourself, as you will overlap yourself and wreck this whole thing. Yes, I went there.
                    overlaps |= IntExtension.Between(offset, size, this.header.SpellOffsets[index].Offset, this.header.SpellOffsets[index].OffsetEnd);

                if (overlaps)   //no point continuing through it all
                    break;
            }

            return overlaps;
        }

        /// <summary>Determines if any offsets overlap with other offsets</summary>
        /// <returns>True if there is any overlap</returns>
        protected Boolean OverlapBaseAnyBase()
        {
            Int32 effectsSize = this.Effects.Count * this.EffectSize;
            Int32 itemsSize = this.Items.Count * ItemInstance.StructSize;
            Int32 itemSlotsSize = 4 /* trailing two selected indexes */ + (this.ItemSlots.Count * 2 /* sizeof(UInt16) */);

            Boolean overlaps =
                (
                    false
                  //|| IntExtension.Between(this.header.OffsetEffects, effectsSize, this.header.OffsetEffects, this.header.OffsetEffects + effectsSize)
                    || IntExtension.Between(this.header.OffsetEffects, effectsSize, this.header.OffsetItems, this.header.OffsetItems + itemsSize)
                    || IntExtension.Between(this.header.OffsetEffects, effectsSize, this.header.OffsetItemSlots, this.header.OffsetItemSlots + itemSlotsSize)

                    || IntExtension.Between(this.header.OffsetItems, itemsSize, this.header.OffsetEffects, this.header.OffsetEffects + effectsSize)
                  //|| IntExtension.Between(this.header.OffsetItems, itemsSize, this.header.OffsetItems, this.header.OffsetItems + itemsSize)
                    || IntExtension.Between(this.header.OffsetItems, itemsSize, this.header.OffsetItemSlots, this.header.OffsetItemSlots + itemSlotsSize)

                    || IntExtension.Between(this.header.OffsetItemSlots, itemSlotsSize, this.header.OffsetEffects, this.header.OffsetEffects + effectsSize)
                    || IntExtension.Between(this.header.OffsetItemSlots, itemSlotsSize, this.header.OffsetItems, this.header.OffsetItems + itemsSize)
                  //|| IntExtension.Between(this.header.OffsetItemSlots, itemSlotsSize, this.header.OffsetItemSlots, this.header.OffsetItemSlots + itemSlotsSize)
                );

            return overlaps;
        }

        /// <summary>Determines if any offsets overlap for base offsets to spell offsets</summary>
        /// <returns>True if there is any overlap</returns>
        protected Boolean OverlapBaseAnySpell()
        {
            Int32 effectsSize = this.Effects.Count * this.EffectSize;
            Int32 itemsSize = this.Items.Count * ItemInstance.StructSize;
            Int32 itemSlotsSize = 4 /* trailing two selected indexes */ + (this.ItemSlots.Count * 2 /* sizeof(UInt16) */);

            return
                this.OverlapsSpellOffsets(this.header.OffsetItems, itemsSize, null)
                || this.OverlapsSpellOffsets(this.header.OffsetItemSlots, itemSlotsSize, null)
                || this.OverlapsSpellOffsets(this.header.OffsetEffects, effectsSize, null);
        }

        /// <summary>Determines if any offsets overlap with other offsets</summary>
        /// <returns>True if there is any overlap</returns>
        protected Boolean OverlapSpellAny()
        {
            Boolean overlaps = false;

            foreach (String key in this.header.SpellOffsets)
            {
                //base offsets
                overlaps |= this.OverlapsSpellAnyBaseOffsets(this.header.SpellOffsets[key].Offset, this.header.SpellOffsets[key].Size);

                //spell offsets
                overlaps |= this.OverlapsSpellOffsets(this.header.SpellOffsets[key].Offset, this.header.SpellOffsets[key].Size, key);

                if (overlaps)   //no point continuing through it all
                    break;
            }

            return overlaps;
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

            builder.AppendLine(this.header.ToString());
            builder.AppendLine(this.GenerateKnownSpellsString());
            builder.AppendLine(this.GenerateItemsString());
            builder.AppendLine(this.GenerateItemSlotsString());
            builder.AppendLine(this.GenerateEffectsString());

            return builder.ToString();
        }

        /// <summary>Generates a human-readable console output describing the list of known spells</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateKnownSpellsString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.Spells.Count; ++i)
                sb.Append(this.Spells[i].ToString(i + 1));

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
	}
}