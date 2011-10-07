using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Item;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Item.Item1;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Item.Item1
{
    public class Item1 : ItemBase
    {
        #region Properties
        public ItemHeader1 Header
        {
            get { return this.header as ItemHeader1; }
            set { this.header = value; }
        }

        protected override String Headline
        {
            get { return "ITEM Version 1.0:\n"; }
        }

        protected override UInt32 HeaderSize
        {
            get { return ItemHeader1.StructSize; }
        }
        #endregion

        #region Constructor(s)
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