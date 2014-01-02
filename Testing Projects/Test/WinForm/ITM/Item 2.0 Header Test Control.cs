using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Item;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.ITM
{
    /// <summary>User control for testing the Item 2.0 file header class</summary>
    public class Item2HeaderTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public Item2HeaderTestControl()
        {
            this.InitializeComponent();
            this.Harness = new ItemHeader2Test();
            this.InitializeControlFields();
        }
        #endregion
    }
}