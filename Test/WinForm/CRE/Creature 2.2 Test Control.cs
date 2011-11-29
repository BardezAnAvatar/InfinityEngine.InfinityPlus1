using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Creature;
using Bardez.Projects.InfinityPlus1.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.CRE
{
    /// <summary>User control for testing the Creature 2.2 file class</summary>
	public class Creature2_2TestControl : HarnessFileBaseTestControlBase<Creature2_2Test>
	{
        /// <summary>Default constructor</summary>
        public Creature2_2TestControl()
        {
            this.InitializeComponent();
            this.Harness = new Creature2_2Test();
            this.InitializeControlFields();
        }
	}
}