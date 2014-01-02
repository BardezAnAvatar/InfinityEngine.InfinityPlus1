using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Creature;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.CRE
{
    /// <summary>User control for testing the Creature 1.2 file class</summary>
	public class Creature1_2TestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public Creature1_2TestControl()
        {
            this.InitializeComponent();
            this.Harness = new Creature1_2Test();
            this.InitializeControlFields();
        }
        #endregion
	}
}