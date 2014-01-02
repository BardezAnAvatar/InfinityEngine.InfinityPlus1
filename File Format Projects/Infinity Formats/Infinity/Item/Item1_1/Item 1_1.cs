using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item.Item1_1;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item.Item1_1
{
    /// <summary>Item version 1.1</summary>
    public class Item1_1 : ItemBase
    {
        #region Properties
        public ItemHeader1_1 Header
        {
            get { return this.header as ItemHeader1_1; }
            set { this.header = value; }
        }

        protected override String Headline
        {
            get { return "ITEM Version 1.1:"; }
        }

        protected override UInt32 HeaderSize
        {
            get { return ItemHeader1_1.StructSize; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public Item1_1()
        {
            this.Initialize();
        }

        /// <summary>Instantiates a new header</summary>
        protected override void InstantiateHeader()
        {
            this.header = new ItemHeader1_1();
        }
        #endregion
    }
}