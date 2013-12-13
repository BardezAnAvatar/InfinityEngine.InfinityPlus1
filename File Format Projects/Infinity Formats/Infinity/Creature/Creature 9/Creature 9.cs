using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Components;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Creature9
{
    public class Creature9 : Creature2EBase
    {
        #region Properties
        /// <summary>Creature header, most of the data</summary>
        public Creature9Header Header
        {
            get { return this.Header2E as Creature9Header; }
            set { this.Header2E = value; }
        }

        /// <summary>Gets the headline for the creature file</summary>
        public override String Headline
        {
            get { return "Creature 9.0:"; }
        }

        /// <summary>Gets the size of the header</summary>
        public override Int32 HeaderSize
        {
            get { return Creature9Header.StructSize; }
        }
        #endregion

        #region Constructor(s)
        /// <summary>Instantiates a new header</summary>
        protected override void InstantiateHeader()
        {
            this.Header = new Creature9Header();
        }

        /// <summary>Initializes the item slots ordered dictionary</summary>
        protected override void InitializeItemSlots()
        {
            this.ItemSlots = new GenericOrderedDictionary<String, Int16>();

            /* 01 */ this.ItemSlots.Add("Helmet", 0);
            /* 02 */ this.ItemSlots.Add("Armor", 0);
            /* 03 */ this.ItemSlots.Add("Shield", 0);
            /* 04 */ this.ItemSlots.Add("Gloves", 0);
            /* 05 */ this.ItemSlots.Add("Left Ring", 0);
            /* 06 */ this.ItemSlots.Add("Right Ring", 0);
            /* 07 */ this.ItemSlots.Add("Amulet", 0);
            /* 08 */ this.ItemSlots.Add("Belt", 0);
            /* 09 */ this.ItemSlots.Add("Boots", 0);
            /* 10 */ this.ItemSlots.Add("Weapon 1", 0);
            /* 11 */ this.ItemSlots.Add("Weapon 2", 0);
            /* 12 */ this.ItemSlots.Add("Weapon 3", 0);
            /* 13 */ this.ItemSlots.Add("Weapon 4", 0);
            /* 14 */ this.ItemSlots.Add("Quiver 1", 0);
            /* 15 */ this.ItemSlots.Add("Quiver 2", 0);
            /* 16 */ this.ItemSlots.Add("Quiver 3", 0);
            /* 17 */ this.ItemSlots.Add("Quiver 4", 0);
            /* 18 */ this.ItemSlots.Add("Cloak", 0);
            /* 19 */ this.ItemSlots.Add("Quick item 1", 0);
            /* 20 */ this.ItemSlots.Add("Quick item 2", 0);
            /* 21 */ this.ItemSlots.Add("Quick item 3", 0);
            /* 22 */ this.ItemSlots.Add("Inventory 01", 0);
            /* 23 */ this.ItemSlots.Add("Inventory 02", 0);
            /* 24 */ this.ItemSlots.Add("Inventory 03", 0);
            /* 25 */ this.ItemSlots.Add("Inventory 04", 0);
            /* 26 */ this.ItemSlots.Add("Inventory 05", 0);
            /* 27 */ this.ItemSlots.Add("Inventory 06", 0);
            /* 28 */ this.ItemSlots.Add("Inventory 07", 0);
            /* 29 */ this.ItemSlots.Add("Inventory 08", 0);
            /* 30 */ this.ItemSlots.Add("Inventory 09", 0);
            /* 31 */ this.ItemSlots.Add("Inventory 10", 0);
            /* 32 */ this.ItemSlots.Add("Inventory 11", 0);
            /* 33 */ this.ItemSlots.Add("Inventory 12", 0);
            /* 34 */ this.ItemSlots.Add("Inventory 13", 0);
            /* 35 */ this.ItemSlots.Add("Inventory 14", 0);
            /* 36 */ this.ItemSlots.Add("Inventory 15", 0);
            /* 37 */ this.ItemSlots.Add("Inventory 16", 0);
            /* 38 */ this.ItemSlots.Add("Magic weapon", 0);
        }
        #endregion
    }
}