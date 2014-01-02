using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Effect;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.EFF
{
    /// <summary>User control for testing the Effect 2.0 file class</summary>
	public class Effect2TestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public Effect2TestControl()
        {
            this.InitializeComponent();
            this.Harness = new Effect2Test();
            this.InitializeControlFields();
        }
        #endregion
	}
}