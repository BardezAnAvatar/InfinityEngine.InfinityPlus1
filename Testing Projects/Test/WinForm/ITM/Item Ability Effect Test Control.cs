using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Item;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.ITM
{
    /// <summary>User control for testing the Item file ability effect class</summary>
    public class ItemAbilityEffectTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public ItemAbilityEffectTestControl()
        {
            this.InitializeComponent();
            this.Harness = new ItemAbilityEffectTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}