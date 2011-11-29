using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Creature;
using Bardez.Projects.InfinityPlus1.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.CRE
{
    /// <summary>User control for testing the Creature 9.0 header file class</summary>
	public class Creature9HeaderTestControl : HarnessFileBaseTestControlBase<CreatureHeader9Test>
	{
        /// <summary>Default constructor</summary>
        public Creature9HeaderTestControl()
        {
            this.InitializeComponent();
            this.Harness = new CreatureHeader9Test();
            this.InitializeControlFields();
        }
	}
}