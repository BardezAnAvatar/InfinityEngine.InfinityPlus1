using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Creature;
using Bardez.Projects.InfinityPlus1.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.CRE
{
    /// <summary>User control for testing the Creature 1.0 header file class</summary>
	public class Creature1HeaderTestControl : HarnessFileBaseTestControlBase<CreatureHeader1Test>
	{
        /// <summary>Default constructor</summary>
        public Creature1HeaderTestControl()
        {
            this.InitializeComponent();
            this.Harness = new CreatureHeader1Test();
            this.InitializeControlFields();
        }
	}
}