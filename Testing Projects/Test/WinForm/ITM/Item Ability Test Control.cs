using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Item;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.ITM
{
    /// <summary>User control for testing the Item file ability class</summary>
    public class ItemAbilityTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public ItemAbilityTestControl()
        {
            this.InitializeComponent();
            this.Harness = new ItemAbilityTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}