using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Item;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.ITM
{
    /// <summary>User control for testing the Item 1.0 file class</summary>
    public class Item1TestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public Item1TestControl()
        {
            this.InitializeComponent();
            this.Harness = new Item1Test();
            this.InitializeControlFields();
        }
        #endregion
    }
}