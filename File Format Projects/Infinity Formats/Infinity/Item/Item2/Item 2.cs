using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item.Item2;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item.Item2
{
    /// <summary>Version 2 item</summary>
    public class Item2 : ItemBase
    {
        #region Properties
        /// <summary>Header of this item, version 2</summary>
        public ItemHeader2 Header
        {
            get { return this.header as ItemHeader2; }
            set { this.header = value; }
        }

        /// <summary>Descriptive headline of the item</summary>
        protected override String Headline
        {
            get { return "ITEM Version 2.0:"; }
        }

        /// <summary>Size of the header</summary>
        protected override UInt32 HeaderSize
        {
            get { return ItemHeader2.StructSize; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public Item2()
        {
            this.Initialize();
        }

        /// <summary>Instantiates a new header</summary>
        protected override void InstantiateHeader()
        {
            this.header = new ItemHeader2();
        }
        #endregion
    }
}