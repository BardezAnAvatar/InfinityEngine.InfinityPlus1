using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Creature;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.CRE
{
    /// <summary>User control for testing the Creature 9.0 header file class</summary>
	public class Creature9HeaderTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public Creature9HeaderTestControl()
        {
            this.InitializeComponent();
            this.Harness = new CreatureHeader9Test();
            this.InitializeControlFields();
        }
        #endregion
    }
}