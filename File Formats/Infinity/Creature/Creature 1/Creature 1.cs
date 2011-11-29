using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Components;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Effect;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Creature1
{
    public class Creature1 : Creature2EBase
    {
        #region Properties
        /// <summary>Creature header, most of the data</summary>
        public Creature1Header Header
        {
            get { return this.header as Creature1Header; }
            set { this.header = value; }
        }

        /// <summary>Gets the headline for the creature file</summary>
        public override String Headline
        {
            get { return "Creature 1.0:"; }
        }

        /// <summary>Gets the size of the header</summary>
        public override Int32 HeaderSize
        {
            get { return Creature1Header.StructSize; }
        }
        #endregion

        #region Constructor(s)
        /// <summary>Instantiates a new header</summary>
        protected override void InstantiateHeader()
        {
            this.header = new Creature1Header();
        }

        /// <summary>Initializes the item slots ordered dictionary</summary>
        protected override void InitializeItemSlots()
        {
            this.itemSlots = new GenericOrderedDictionary<String, Int16>();

            /* 01 */ this.itemSlots.Add("Helmet", 0);
            /* 02 */ this.itemSlots.Add("Armor", 0);
            /* 03 */ this.itemSlots.Add("Shield", 0);
            /* 04 */ this.itemSlots.Add("Gloves", 0);
            /* 05 */ this.itemSlots.Add("Left Ring", 0);
            /* 06 */ this.itemSlots.Add("Right Ring", 0);
            /* 07 */ this.itemSlots.Add("Amulet", 0);
            /* 08 */ this.itemSlots.Add("Belt", 0);
            /* 09 */ this.itemSlots.Add("Boots", 0);
            /* 10 */ this.itemSlots.Add("Weapon 1", 0);
            /* 11 */ this.itemSlots.Add("Weapon 2", 0);
            /* 12 */ this.itemSlots.Add("Weapon 3", 0);
            /* 13 */ this.itemSlots.Add("Weapon 4", 0);
            /* 14 */ this.itemSlots.Add("Quiver 1", 0);
            /* 15 */ this.itemSlots.Add("Quiver 2", 0);
            /* 16 */ this.itemSlots.Add("Quiver 3", 0);
            /* 17 */ this.itemSlots.Add("Quiver 4", 0);
            /* 18 */ this.itemSlots.Add("Cloak", 0);
            /* 19 */ this.itemSlots.Add("Quick item 1", 0);
            /* 20 */ this.itemSlots.Add("Quick item 2", 0);
            /* 21 */ this.itemSlots.Add("Quick item 3", 0);
            /* 22 */ this.itemSlots.Add("Inventory 01", 0);
            /* 23 */ this.itemSlots.Add("Inventory 02", 0);
            /* 24 */ this.itemSlots.Add("Inventory 03", 0);
            /* 25 */ this.itemSlots.Add("Inventory 04", 0);
            /* 26 */ this.itemSlots.Add("Inventory 05", 0);
            /* 27 */ this.itemSlots.Add("Inventory 06", 0);
            /* 28 */ this.itemSlots.Add("Inventory 07", 0);
            /* 29 */ this.itemSlots.Add("Inventory 08", 0);
            /* 30 */ this.itemSlots.Add("Inventory 09", 0);
            /* 31 */ this.itemSlots.Add("Inventory 10", 0);
            /* 32 */ this.itemSlots.Add("Inventory 11", 0);
            /* 33 */ this.itemSlots.Add("Inventory 12", 0);
            /* 34 */ this.itemSlots.Add("Inventory 13", 0);
            /* 35 */ this.itemSlots.Add("Inventory 14", 0);
            /* 36 */ this.itemSlots.Add("Inventory 15", 0);
            /* 37 */ this.itemSlots.Add("Inventory 16", 0);
            /* 38 */ this.itemSlots.Add("Magic weapon", 0);
        }
        #endregion
    }
}