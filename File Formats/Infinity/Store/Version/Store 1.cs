using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Item.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Store.Components;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Store.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Store.Version
{
    /// <summary>Version 1.0 store, used in Baldur's Gate and Baldur's Gate II (and Icewind Dale no expan?)</summary>
    public class Store1 : IInfinityFormat
    {
        #region Members
        /// <summary>Store file header</summary>
        protected StoreHeader1 header;

        /// <summary>Items in store</summary>
        protected List<AvailableItem> items;

        /// <summary>Drinks in store</summary>
        protected List<AvailableDrink> drinks;

        /// <summary>Healing in store</summary>
        protected List<AvailableSpell> healing;

        /// <summary>Types of items purchasedin store</summary>
        /// <remarks>These are read and written as UInt32s in STOs, but UInt16 in ITMs</remarks>
        protected List<ItemType> purchaseTypes;
        #endregion

        #region Properties
        /// <summary>Store file header</summary>
        public StoreHeader1 Header
        {
            get { return this.header; }
            set { this.header = value; }
        }

        /// <summary>Items in store</summary>
        public List<AvailableItem> Items
        {
            get { return this.items; }
            set { this.items = value; }
        }

        /// <summary>Drinks in store</summary>
        public List<AvailableDrink> Drinks
        {
            get { return this.drinks; }
            set { this.drinks = value; }
        }

        /// <summary>Healing in store</summary>
        public List<AvailableSpell> Healing
        {
            get { return this.healing; }
            set { this.healing = value; }
        }

        /// <summary>Types of items purchasedin store</summary>
        public List<ItemType> PurchaseTypes
        {
            get { return this.purchaseTypes; }
            set { this.purchaseTypes = value; }
        }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public Store1()
        {
            this.drinks = null;
            this.header = null;
            this.healing = null;
            this.items = null;
            this.purchaseTypes = null;
        }

        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.drinks = new List<AvailableDrink>();
            this.header = new StoreHeader1();
            this.healing = new List<AvailableSpell>();
            this.items = new List<AvailableItem>();
            this.purchaseTypes = new List<ItemType>();
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
                this.header = new StoreHeader1();
                this.header.Read(input, false);
            }
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            this.header.Read(input);
            this.ReadAvailableItems(input);
            this.ReadAvailableDrinks(input);
            this.ReadAvailableSpells(input);
            this.ReadPurchaseTypes(input);
        }

        /// <summary>Reads the list of available items from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadAvailableItems(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.header.OffsetItems, SeekOrigin.Begin);

            //read items
            for (Int32 i = 0; i < this.header.CountItems; ++i)
            {
                AvailableItem item = new AvailableItem();
                item.Read(input);
                this.items.Add(item);
            }
        }

        /// <summary>Reads the list of available drinks from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadAvailableDrinks(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.header.OffsetDrinks, SeekOrigin.Begin);

            //read drinks
            for (Int32 i = 0; i < this.header.CountDrinks; ++i)
            {
                AvailableDrink drink = new AvailableDrink();
                drink.Read(input);
                this.drinks.Add(drink);
            }
        }

        /// <summary>Reads the list of avaiable spells from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadAvailableSpells(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.header.OffsetHealing, SeekOrigin.Begin);

            //read spells
            for (Int32 i = 0; i < this.header.CountHealing; ++i)
            {
                AvailableSpell spell = new AvailableSpell();
                spell.Read(input);
                this.healing.Add(spell);
            }
        }

        /// <summary>Reads the list of effects from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadPurchaseTypes(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.header.OffsetPurchaseTypes, SeekOrigin.Begin);

            Byte[] data = ReusableIO.BinaryRead(input, this.header.CountPurchaseTypes * 4 /* sizeof(UInt32) */);

            //read types
            for (Int32 i = 0; i < this.header.CountPurchaseTypes; ++i)
            {
                ItemType type = (ItemType)Convert.ToUInt16(ReusableIO.ReadUInt32FromArray(data, i * 4));
                this.purchaseTypes.Add(type);
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            this.MaintainMinimalDataIntegrity();

            this.header.Write(output);
            this.WriteAvailableItems(output);
            this.WriteAvailableDrinks(output);
            this.WriteAvailableSpells(output);
            this.WritePurchaseTypes(output);
        }

        /// <summary>Writes the list of available items to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteAvailableItems(Stream output)
        {
            ReusableIO.SeekIfAble(output, this.header.OffsetItems, SeekOrigin.Begin);

            //write items
            foreach (AvailableItem item in this.items)
                item.Write(output);
        }

        /// <summary>Writes the list available drinks to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteAvailableDrinks(Stream output)
        {
            ReusableIO.SeekIfAble(output, this.header.OffsetDrinks, SeekOrigin.Begin);

            //write drinks
            foreach (AvailableDrink drink in this.drinks)
                drink.Write(output);
        }

        /// <summary>Writes the list of available (healing) spells to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteAvailableSpells(Stream output)
        {
            ReusableIO.SeekIfAble(output, this.header.OffsetHealing, SeekOrigin.Begin);

            //write spells
            foreach (AvailableSpell spell in this.healing)
                spell.Write(output);
        }

        /// <summary>Writes the list of item types purchased to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WritePurchaseTypes(Stream output)
        {
            ReusableIO.SeekIfAble(output, this.header.OffsetPurchaseTypes, SeekOrigin.Begin);

            //write purchase types
            foreach (ItemType type in this.purchaseTypes)
                ReusableIO.WriteUInt32ToStream((UInt16)type, output);
        }
        #endregion

        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Store version 1.0");
            builder.Append(this.header.ToString());
            builder.Append(this.GenerateItemsString());
            builder.Append(this.GenerateDrinksString());
            builder.Append(this.GenerateHealingSpellsString());
            builder.Append(this.GeneratePurchaseTypesString());
            builder.Append("\n\n");

            return builder.ToString();
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

        /// <summary>Generates a human-readable console output describing the list of drinks</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateDrinksString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.drinks.Count; ++i)
                sb.Append(this.drinks[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the list of healing spells</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateHealingSpellsString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.healing.Count; ++i)
                sb.Append(this.healing[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the list of purchase types</summary>
        /// <returns>A multi-line String</returns>
        protected String GeneratePurchaseTypesString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(StringFormat.ToStringAlignment("Purchase types"));

            for (Int32 i = 0; i < this.purchaseTypes.Count; ++i)
                StringFormat.AppendSubItem(sb, true, this.purchaseTypes[i].GetDescription());

            return sb.ToString();
        }
        #endregion

        #region Data Integrity
        /// <summary>Maintains mimimal data integrity by not lying to the output data file.</summary>
        protected void MaintainMinimalDataIntegrity()
        {
            if (this.Overlaps())
            {
                //4 variables
                Int32 availableItemsSize = this.items.Count * AvailableItem.StructSize;
                Int32 availableDrinksSize = this.drinks.Count * AvailableDrink.StructSize;
                Int32 availableSpellsSize = this.healing.Count * AvailableSpell.StructSize;
                Int32 availablePurchaseTypesSize = this.purchaseTypes.Count * 2 /* sizeof(ItemType / UInt16) */;

                //reset offsets
                this.header.OffsetItems = Convert.ToUInt32(this.header.StructSize);
                this.header.OffsetDrinks = Convert.ToUInt32(this.header.OffsetItems + availableItemsSize);
                this.header.OffsetHealing = Convert.ToUInt32(this.header.OffsetDrinks + availableDrinksSize);
                this.header.OffsetPurchaseTypes = Convert.ToUInt32(this.header.OffsetHealing + availableSpellsSize);
            }
        }

        /// <summary>Determines if any of the offset data sections would overlap one another.</summary>
        /// <returns>A Boolean indicating whether or not any of them overlap</returns>
        protected Boolean Overlaps()
        {
            //4 variables
            Int32 availableItemsSize = this.items.Count * AvailableItem.StructSize;
            Int32 availableDrinksSize = this.drinks.Count * AvailableDrink.StructSize;
            Int32 availableSpellsSize = this.healing.Count * AvailableSpell.StructSize;
            Int32 availablePurchaseTypesSize = this.purchaseTypes.Count * 2 /* sizeof(UInt16) */;

            Boolean overlaps = false;

            //technically, any of these 6 can follow the header in any order. Check for any overlaps.
            if (
                   IntExtension.Between(this.header.OffsetItems, availableItemsSize, 0, this.header.StructSize)
                || IntExtension.Between(this.header.OffsetDrinks, availableDrinksSize, 0, this.header.StructSize)
                || IntExtension.Between(this.header.OffsetHealing, availableSpellsSize, 0, this.header.StructSize)
                || IntExtension.Between(this.header.OffsetPurchaseTypes, availablePurchaseTypesSize, 0, this.header.StructSize)
                )
                overlaps = true;

            // It's 11:42 PM and I can't think of any better way of doing this shite than writing out all combinations, copy/paste, and then excluding the self.
            if (!overlaps)
            {
                overlaps =
                    (
                        false
                       //|| IntExtension.Between(this.header.OffsetItems, availableItemsSize, this.header.OffsetItems, this.header.OffsetItems + availableItemsSize)
                         || IntExtension.Between(this.header.OffsetItems, availableItemsSize, this.header.OffsetDrinks, this.header.OffsetDrinks + availableDrinksSize)
                         || IntExtension.Between(this.header.OffsetItems, availableItemsSize, this.header.OffsetHealing, this.header.OffsetHealing + availableSpellsSize)
                         || IntExtension.Between(this.header.OffsetItems, availableItemsSize, this.header.OffsetPurchaseTypes, this.header.OffsetPurchaseTypes + availablePurchaseTypesSize)

                         || IntExtension.Between(this.header.OffsetDrinks, availableDrinksSize, this.header.OffsetItems, this.header.OffsetItems + availableItemsSize)
                       //|| IntExtension.Between(this.header.OffsetDrinks, availableDrinksSize, this.header.OffsetDrinks, this.header.OffsetDrinks + availableDrinksSize)
                         || IntExtension.Between(this.header.OffsetDrinks, availableDrinksSize, this.header.OffsetHealing, this.header.OffsetHealing + availableSpellsSize)
                         || IntExtension.Between(this.header.OffsetDrinks, availableDrinksSize, this.header.OffsetPurchaseTypes, this.header.OffsetPurchaseTypes + availablePurchaseTypesSize)

                         || IntExtension.Between(this.header.OffsetHealing, availableSpellsSize, this.header.OffsetItems, this.header.OffsetItems + availableItemsSize)
                         || IntExtension.Between(this.header.OffsetHealing, availableSpellsSize, this.header.OffsetDrinks, this.header.OffsetDrinks + availableDrinksSize)
                       //|| IntExtension.Between(this.header.OffsetHealing, availableSpellsSize, this.header.OffsetHealing, this.header.OffsetHealing + availableSpellsSize)
                         || IntExtension.Between(this.header.OffsetHealing, availableSpellsSize, this.header.OffsetPurchaseTypes, this.header.OffsetPurchaseTypes + availablePurchaseTypesSize)

                         || IntExtension.Between(this.header.OffsetPurchaseTypes, availablePurchaseTypesSize, this.header.OffsetItems, this.header.OffsetItems + availableItemsSize)
                         || IntExtension.Between(this.header.OffsetPurchaseTypes, availablePurchaseTypesSize, this.header.OffsetDrinks, this.header.OffsetDrinks + availableDrinksSize)
                         || IntExtension.Between(this.header.OffsetPurchaseTypes, availablePurchaseTypesSize, this.header.OffsetHealing, this.header.OffsetHealing + availableSpellsSize)
                       //|| IntExtension.Between(this.header.OffsetPurchaseTypes, availablePurchaseTypesSize, this.header.OffsetPurchaseTypes, this.header.OffsetPurchaseTypes + availablePurchaseTypesSize)
                    );
            }

            return overlaps;
        }
        #endregion
    }
}