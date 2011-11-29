using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Creature;
using Bardez.Projects.InfinityPlus1.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.CRE
{
    /// <summary>User control for testing the Creature 1.0 file class</summary>
	public class Creature1TestControl : HarnessFileBaseTestControlBase<Creature1Test>
	{
        /// <summary>Default constructor</summary>
        public Creature1TestControl()
        {
            this.InitializeComponent();
            this.Harness = new Creature1Test();
            this.InitializeControlFields();
        }
	}
}