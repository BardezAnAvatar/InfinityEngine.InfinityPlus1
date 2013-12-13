using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Spell;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.SPL
{
    /// <summary>User control for testing the Spell 1.0 file class</summary>
    public class Spell1TestControl : HarnessFileBaseTestControlBase<Spell1Test>
    {
        /// <summary>Default constructor</summary>
        public Spell1TestControl()
        {
            this.InitializeComponent();
            this.Harness = new Spell1Test();
            this.InitializeControlFields();
        }
    }
}