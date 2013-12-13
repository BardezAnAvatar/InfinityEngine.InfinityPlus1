using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Components;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Creature1_2
{
    /// <summary>Creature version 1.2, used in Planescape: Torment</summary>
    public class Creature1_2 : Creature2EBase
    {
        #region Members
        /// <summary>Animation overlays on top of creature</summary>
        protected List<PstOverlay> overlays;
        #endregion


        #region Properties
        /// <summary>Animation overlays on top of creature</summary>
        public List<PstOverlay> Overlays
        {
            get { return this.overlays; }
            set { this.overlays = value; }
        }

        /// <summary>Creature header, most of the data</summary>
        public Creature1_2Header Header
        {
            get { return this.Header2E as Creature1_2Header; }
            set { this.Header2E = value; }
        }

        /// <summary>Gets the headline for the creature file</summary>
        public override String Headline
        {
            get { return "Creature 1.2:"; }
        }

        /// <summary>Gets the size of the header</summary>
        public override Int32 HeaderSize
        {
            get { return Creature1_2Header.StructSize; }
        }
        #endregion


        #region Constructor(s)
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            base.Initialize();
            this.overlays = new List<PstOverlay>();
        }

        /// <summary>Instantiates a new header</summary>
        protected override void InstantiateHeader()
        {
            this.Header = new Creature1_2Header();
        }

        /// <summary>Initializes the item slots ordered dictionary</summary>
        protected override void InitializeItemSlots()
        {
            this.ItemSlots = new GenericOrderedDictionary<String, Int16>();

            /* 01 */ this.ItemSlots.Add("Helmet/Lens/Right Earring", 0);
            /* 02 */ this.ItemSlots.Add("Armor", 0);
            /* 03 */ this.ItemSlots.Add("Left Tattoo", 0);
            /* 04 */ this.ItemSlots.Add("Hand", 0);
            /* 05 */ this.ItemSlots.Add("Left Ring", 0);
            /* 06 */ this.ItemSlots.Add("Right Ring", 0);
            /* 07 */ this.ItemSlots.Add("Left Earring/Eyeball", 0);
            /* 08 */ this.ItemSlots.Add("Right Tattoo (lower)", 0);
            /* 09 */ this.ItemSlots.Add("Boots", 0);
            /* 10 */ this.ItemSlots.Add("Weapon 1", 0);
            /* 11 */ this.ItemSlots.Add("Weapon 2", 0);
            /* 12 */ this.ItemSlots.Add("Weapon 3", 0);
            /* 13 */ this.ItemSlots.Add("Weapon 4", 0);
            /* 14 */ this.ItemSlots.Add("Quiver 1", 0);
            /* 15 */ this.ItemSlots.Add("Quiver 2", 0);
            /* 16 */ this.ItemSlots.Add("Quiver 3", 0);
            /* 17 */ this.ItemSlots.Add("Quiver 4", 0);
            /* 18 */ this.ItemSlots.Add("Quiver 5", 0);
            /* 19 */ this.ItemSlots.Add("Quiver 6", 0);
            /* 20 */ this.ItemSlots.Add("Right Tattoo (upper)", 0);
            /* 21 */ this.ItemSlots.Add("Quick item 1", 0);
            /* 22 */ this.ItemSlots.Add("Quick item 2", 0);
            /* 23 */ this.ItemSlots.Add("Quick item 3", 0);
            /* 24 */ this.ItemSlots.Add("Quick item 4", 0);
            /* 25 */ this.ItemSlots.Add("Quick item 5", 0);
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
            /* 46 */ this.ItemSlots.Add("Magic weapon", 0);
        }
        #endregion

        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadBody(Stream input)
        {
            this.Initialize();

            this.Header.Read(input);
            this.ReadOverlays(input);
            this.ReadKnownSpells(input);
            this.ReadSpellPreparation(input);
            this.ReadPreparedSpells(input);
            this.ReadEffects(input);
            this.ReadItems(input);
            this.ReadItemSlots(input);
        }

        /// <summary>Reads the list of creature overlays from the output stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadOverlays(Stream input)
        {
            if (input.Position != this.Header.OffsetOverlays)
                input.Seek(this.Header.OffsetOverlays, SeekOrigin.Begin);

            //read known spells
            for (Int32 i = 0; i < (this.Header.SizeOverlays / PstOverlay.StructSize); ++i)
            {
                PstOverlay overlay = new PstOverlay();
                overlay.Read(input);
                this.overlays.Add(overlay);
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        public override void Write(Stream output)
        {
            this.MaintainMinimalDataIntegrity();

            this.Header.Write(output);
            this.WriteOverlays(output);
            this.WriteKnownSpells(output);
            this.WriteSpellPreparation(output);
            this.WritePreparedSpells(output);
            this.WriteEffects(output);
            this.WriteItems(output);
            this.WriteItemSlots(output);
        }

        /// <summary>Writes the list of creature animation overlays to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteOverlays(Stream output)
        {
            if (output.Position != this.Header.OffsetOverlays)
                output.Seek(this.Header.OffsetOverlays, SeekOrigin.Begin);

            //write overlay memorization
            foreach (PstOverlay overlay in this.overlays)
                overlay.Write(output);
        }
        #endregion
        
        #region Data Integrity
        /// <summary>Maintains mimimal data integrity by not lying to the output data file.</summary>
        protected override void MaintainMinimalDataIntegrity()
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
                this.Header.OffsetKnownSpells = this.HeaderSize;
                this.Header.OffsetSpellMemorization = this.Header.OffsetKnownSpells + knownSpellsSize;
                this.Header.OffsetMemorizedSpells = this.Header.OffsetSpellMemorization + spellMemorizationSlotsSize;
                this.Header.OffsetEffects = Convert.ToUInt32(this.Header.OffsetMemorizedSpells + preparedSpellsSize);
                this.Header.OffsetItems = Convert.ToUInt32(this.Header.OffsetEffects + effectsSize);
                this.Header.OffsetItemSlots = Convert.ToUInt32(this.Header.OffsetItems + itemsSize);
            }
        }

        /// <summary>Determines if any of the offset data sections would overlap one another.</summary>
        /// <returns>A Boolean indicating whether or not any of them overlap</returns>
        protected override Boolean Overlaps()
        {
            //6 variables
            Int32 knownSpellsSize = this.KnownSpells.Count * Creature2EKnownSpell.StructSize;
            Int32 spellMemorizationSlotsSize = this.SpellMemorization.Count * Creature2ESpellMemorization.StructSize;
            Int32 preparedSpellsSize = this.PreparedSpells.Count * Creature2EMemorizedSpells.StructSize;
            Int32 effectsSize = this.Effects.Count * this.EffectSize;
            Int32 itemsSize = this.Items.Count * ItemInstance.StructSize;
            Int32 itemSlotsSize = 4 /* trailing two selected indexes */ + (this.ItemSlots.Count * 2 /* sizeof(UInt16) */);
            //this.Header.SizeOverlays already computed/set

            Boolean overlaps = false;

            //technically, any of these 6 can follow the header in any order. Check for any overlaps.
            if (
                IntExtension.Between(this.Header.OffsetKnownSpells, knownSpellsSize, 0, this.HeaderSize)
                || IntExtension.Between(this.Header.OffsetSpellMemorization, spellMemorizationSlotsSize, 0, this.HeaderSize)
                || IntExtension.Between(this.Header.OffsetMemorizedSpells, preparedSpellsSize, 0, this.HeaderSize)
                || IntExtension.Between(this.Header.OffsetEffects, effectsSize, 0, this.HeaderSize)
                || IntExtension.Between(this.Header.OffsetItems, itemsSize, 0, this.HeaderSize)
                || IntExtension.Between(this.Header.OffsetItemSlots, itemSlotsSize, 0, this.HeaderSize)
                )
                overlaps = true;

            // It's 11:44 PM and I still can't think of any better way of doing this shite than writing out all combinations, copy/paste, and then excluding the self.
            if (!overlaps)
            {
                overlaps = 
                    (
                        false
                         //|| IntExtension.Between(this.Header.OffsetOverlays, this.Header.SizeOverlays, this.Header.OffsetOverlays, this.Header.OffsetOverlays + this.Header.SizeOverlays)
                         || IntExtension.Between(this.Header.OffsetOverlays, this.Header.SizeOverlays, this.Header.OffsetKnownSpells, this.Header.OffsetKnownSpells + knownSpellsSize)
                         || IntExtension.Between(this.Header.OffsetOverlays, this.Header.SizeOverlays, this.Header.OffsetSpellMemorization, this.Header.OffsetSpellMemorization + spellMemorizationSlotsSize)
                         || IntExtension.Between(this.Header.OffsetOverlays, this.Header.SizeOverlays, this.Header.OffsetMemorizedSpells, this.Header.OffsetMemorizedSpells + preparedSpellsSize)
                         || IntExtension.Between(this.Header.OffsetOverlays, this.Header.SizeOverlays, this.Header.OffsetEffects, this.Header.OffsetEffects + effectsSize)
                         || IntExtension.Between(this.Header.OffsetOverlays, this.Header.SizeOverlays, this.Header.OffsetItems, this.Header.OffsetItems + itemsSize)
                         || IntExtension.Between(this.Header.OffsetOverlays, this.Header.SizeOverlays, this.Header.OffsetItemSlots, this.Header.OffsetItemSlots + itemSlotsSize)

                         || IntExtension.Between(this.Header.OffsetKnownSpells, knownSpellsSize, this.Header.OffsetOverlays, this.Header.OffsetOverlays + this.Header.SizeOverlays)
                         //|| IntExtension.Between(this.Header.OffsetKnownSpells, knownSpellsSize, this.Header.OffsetKnownSpells, this.Header.OffsetKnownSpells + knownSpellsSize)
                         || IntExtension.Between(this.Header.OffsetKnownSpells, knownSpellsSize, this.Header.OffsetSpellMemorization, this.Header.OffsetSpellMemorization + spellMemorizationSlotsSize)
                         || IntExtension.Between(this.Header.OffsetKnownSpells, knownSpellsSize, this.Header.OffsetMemorizedSpells, this.Header.OffsetMemorizedSpells + preparedSpellsSize)
                         || IntExtension.Between(this.Header.OffsetKnownSpells, knownSpellsSize, this.Header.OffsetEffects, this.Header.OffsetEffects + effectsSize)
                         || IntExtension.Between(this.Header.OffsetKnownSpells, knownSpellsSize, this.Header.OffsetItems, this.Header.OffsetItems + itemsSize)
                         || IntExtension.Between(this.Header.OffsetKnownSpells, knownSpellsSize, this.Header.OffsetItemSlots, this.Header.OffsetItemSlots + itemSlotsSize)

                         || IntExtension.Between(this.Header.OffsetSpellMemorization, spellMemorizationSlotsSize, this.Header.OffsetOverlays, this.Header.OffsetOverlays + this.Header.SizeOverlays)
                         || IntExtension.Between(this.Header.OffsetSpellMemorization, spellMemorizationSlotsSize, this.Header.OffsetKnownSpells, this.Header.OffsetKnownSpells + knownSpellsSize)
                         //|| IntExtension.Between(this.Header.OffsetSpellMemorization, spellMemorizationSlotsSize, this.Header.OffsetSpellMemorization, this.Header.OffsetSpellMemorization + spellMemorizationSlotsSize)
                         || IntExtension.Between(this.Header.OffsetSpellMemorization, spellMemorizationSlotsSize, this.Header.OffsetMemorizedSpells, this.Header.OffsetMemorizedSpells + preparedSpellsSize)
                         || IntExtension.Between(this.Header.OffsetSpellMemorization, spellMemorizationSlotsSize, this.Header.OffsetEffects, this.Header.OffsetEffects + effectsSize)
                         || IntExtension.Between(this.Header.OffsetSpellMemorization, spellMemorizationSlotsSize, this.Header.OffsetItems, this.Header.OffsetItems + itemsSize)
                         || IntExtension.Between(this.Header.OffsetSpellMemorization, spellMemorizationSlotsSize, this.Header.OffsetItemSlots, this.Header.OffsetItemSlots + itemSlotsSize)

                         || IntExtension.Between(this.Header.OffsetMemorizedSpells, preparedSpellsSize, this.Header.OffsetOverlays, this.Header.OffsetOverlays + this.Header.SizeOverlays)
                         || IntExtension.Between(this.Header.OffsetMemorizedSpells, preparedSpellsSize, this.Header.OffsetKnownSpells, this.Header.OffsetKnownSpells + knownSpellsSize)
                         || IntExtension.Between(this.Header.OffsetMemorizedSpells, preparedSpellsSize, this.Header.OffsetSpellMemorization, this.Header.OffsetSpellMemorization + spellMemorizationSlotsSize)
                         //|| IntExtension.Between(this.Header.OffsetMemorizedSpells, preparedSpellsSize, this.Header.OffsetMemorizedSpells, this.Header.OffsetMemorizedSpells + preparedSpellsSize)
                         || IntExtension.Between(this.Header.OffsetMemorizedSpells, preparedSpellsSize, this.Header.OffsetEffects, this.Header.OffsetEffects + effectsSize)
                         || IntExtension.Between(this.Header.OffsetMemorizedSpells, preparedSpellsSize, this.Header.OffsetItems, this.Header.OffsetItems + itemsSize)
                         || IntExtension.Between(this.Header.OffsetMemorizedSpells, preparedSpellsSize, this.Header.OffsetItemSlots, this.Header.OffsetItemSlots + itemSlotsSize)

                         || IntExtension.Between(this.Header.OffsetEffects, effectsSize, this.Header.OffsetOverlays, this.Header.OffsetOverlays + this.Header.SizeOverlays)
                         || IntExtension.Between(this.Header.OffsetEffects, effectsSize, this.Header.OffsetKnownSpells, this.Header.OffsetKnownSpells + knownSpellsSize)
                         || IntExtension.Between(this.Header.OffsetEffects, effectsSize, this.Header.OffsetSpellMemorization, this.Header.OffsetSpellMemorization + spellMemorizationSlotsSize)
                         || IntExtension.Between(this.Header.OffsetEffects, effectsSize, this.Header.OffsetMemorizedSpells, this.Header.OffsetMemorizedSpells + preparedSpellsSize)
                         //|| IntExtension.Between(this.Header.OffsetEffects, effectsSize, this.Header.OffsetEffects, this.Header.OffsetEffects + effectsSize)
                         || IntExtension.Between(this.Header.OffsetEffects, effectsSize, this.Header.OffsetItems, this.Header.OffsetItems + itemsSize)
                         || IntExtension.Between(this.Header.OffsetEffects, effectsSize, this.Header.OffsetItemSlots, this.Header.OffsetItemSlots + itemSlotsSize)

                         || IntExtension.Between(this.Header.OffsetItems, itemsSize, this.Header.OffsetOverlays, this.Header.OffsetOverlays + this.Header.SizeOverlays)
                         || IntExtension.Between(this.Header.OffsetItems, itemsSize, this.Header.OffsetKnownSpells, this.Header.OffsetKnownSpells + knownSpellsSize)
                         || IntExtension.Between(this.Header.OffsetItems, itemsSize, this.Header.OffsetSpellMemorization, this.Header.OffsetSpellMemorization + spellMemorizationSlotsSize)
                         || IntExtension.Between(this.Header.OffsetItems, itemsSize, this.Header.OffsetMemorizedSpells, this.Header.OffsetMemorizedSpells + preparedSpellsSize)
                         || IntExtension.Between(this.Header.OffsetItems, itemsSize, this.Header.OffsetEffects, this.Header.OffsetEffects + effectsSize)
                         //|| IntExtension.Between(this.Header.OffsetItems, itemsSize, this.Header.OffsetItems, this.Header.OffsetItems + itemsSize)
                         || IntExtension.Between(this.Header.OffsetItems, itemsSize, this.Header.OffsetItemSlots, this.Header.OffsetItemSlots + itemSlotsSize)

                         || IntExtension.Between(this.Header.OffsetItemSlots, itemSlotsSize, this.Header.OffsetOverlays, this.Header.OffsetOverlays + this.Header.SizeOverlays)
                         || IntExtension.Between(this.Header.OffsetItemSlots, itemSlotsSize, this.Header.OffsetKnownSpells, this.Header.OffsetKnownSpells + knownSpellsSize)
                         || IntExtension.Between(this.Header.OffsetItemSlots, itemSlotsSize, this.Header.OffsetSpellMemorization, this.Header.OffsetSpellMemorization + spellMemorizationSlotsSize)
                         || IntExtension.Between(this.Header.OffsetItemSlots, itemSlotsSize, this.Header.OffsetMemorizedSpells, this.Header.OffsetMemorizedSpells + preparedSpellsSize)
                         || IntExtension.Between(this.Header.OffsetItemSlots, itemSlotsSize, this.Header.OffsetEffects, this.Header.OffsetEffects + effectsSize)
                         || IntExtension.Between(this.Header.OffsetItemSlots, itemSlotsSize, this.Header.OffsetItems, this.Header.OffsetItems + itemsSize)
                         //|| IntExtension.Between(this.Header.OffsetItemSlots, itemSlotsSize, this.Header.OffsetItemSlots, this.Header.OffsetItemSlots + itemSlotsSize)
                    );
            }

            return overlaps;
        }
        #endregion

        #region ToString() Helpers
        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(this.Header.ToString());
            builder.AppendLine(this.GenerateOverlaysString());
            builder.AppendLine(this.GenerateKnownSpellsString());
            builder.AppendLine(this.GenerateSpellMemorizationString());
            builder.AppendLine(this.GeneratePreparedSpellsString());
            builder.AppendLine(this.GenerateEffectsString());
            builder.AppendLine(this.GenerateItemsString());
            builder.AppendLine(this.GenerateItemSlotsString());

            return builder.ToString();
        }

        /// <summary>Generates a human-readable console output describing the list of animation overlays</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateOverlaysString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < (this.Header.SizeOverlays / PstOverlay.StructSize); ++i)
                sb.Append(this.overlays[i].ToString(i + 1));

            return sb.ToString();
        }
        #endregion
    }
}