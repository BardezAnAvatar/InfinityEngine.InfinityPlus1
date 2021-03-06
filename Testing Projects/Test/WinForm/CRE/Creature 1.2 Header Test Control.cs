using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Creature;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.CRE
{
    /// <summary>User control for testing the Creature 1.2 header file class</summary>
	public class Creature1_2HeaderTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public Creature1_2HeaderTestControl()
        {
            this.InitializeComponent();
            this.Harness = new CreatureHeader1_2Test();
            this.InitializeControlFields();
        }
        #endregion
    }
}