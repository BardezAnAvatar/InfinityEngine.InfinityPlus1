using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Item;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.ITM
{
    /// <summary>User control for testing the Item 1.1 file header class</summary>
    public class Item1_1HeaderTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public Item1_1HeaderTestControl()
        {
            this.InitializeComponent();
            this.Harness = new ItemHeader1_1Test();
            this.InitializeControlFields();
        }
        #endregion
    }
}