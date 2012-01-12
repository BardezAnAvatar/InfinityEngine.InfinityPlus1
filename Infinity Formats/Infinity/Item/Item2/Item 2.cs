using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Item;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Item.Item2;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Item.Item2
{
    public class Item2 : ItemBase
    {
        #region Properties
        public ItemHeader2 Header
        {
            get { return this.header as ItemHeader2; }
            set { this.header = value; }
        }

        protected override String Headline
        {
            get { return "ITEM Version 2.0:"; }
        }

        protected override UInt32 HeaderSize
        {
            get { return ItemHeader2.StructSize; }
        }
        #endregion

        #region Constructor(s)
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