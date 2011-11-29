using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Item;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.ITM
{
    /// <summary>User control for testing the Item 2.0 file class</summary>
    public class Item2TestControl : HarnessFileBaseTestControlBase<Item2Test>
    {
        /// <summary>Default constructor</summary>
        public Item2TestControl()
        {
            this.InitializeComponent();
            this.Harness = new Item2Test();
            this.InitializeControlFields();
        }
    }
}