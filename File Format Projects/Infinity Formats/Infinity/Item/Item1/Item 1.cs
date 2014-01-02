using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item.Item1;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item.Item1
{
    /// <summary>Item version 1</summary>
    public class Item1 : ItemBase
    {
        #region Properties
        /// <summary>Item's header</summary>
        public ItemHeader1 Header
        {
            get { return this.header as ItemHeader1; }
            set { this.header = value; }
        }

        /// <summary>Item's headline for a friendly-text display</summary>
        protected override String Headline
        {
            get { return "ITEM Version 1.0:"; }
        }

        /// <summary>Overridden structure size</summary>
        protected override UInt32 HeaderSize
        {
            get { return ItemHeader1.StructSize; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public Item1()
        {
            this.Initialize();
        }

        /// <summary>Instantiates a new header</summary>
        protected override void InstantiateHeader()
        {
            this.header = new ItemHeader1();
        }
        #endregion
    }
}