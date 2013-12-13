using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Item;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.ITM
{
    /// <summary>User control for testing the Item 1.0 file header class</summary>
    public class Item1HeaderTestControl : HarnessFileBaseTestControlBase<ItemHeader1Test>
    {
        /// <summary>Default constructor</summary>
        public Item1HeaderTestControl()
        {
            this.InitializeComponent();
            this.Harness = new ItemHeader1Test();
            this.InitializeControlFields();
        }
    }
}