using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Creature;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.CRE
{
    /// <summary>User control for testing the Creature 9.0 file class</summary>
	public class Creature9TestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public Creature9TestControl()
        {
            this.InitializeComponent();
            this.Harness = new Creature9Test();
            this.InitializeControlFields();
        }
        #endregion
	}
}