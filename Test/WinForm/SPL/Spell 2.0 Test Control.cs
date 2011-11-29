using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Spell;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.SPL
{
    /// <summary>User control for testing the Spell 2.0 file class</summary>
    public class Spell2TestControl : HarnessFileBaseTestControlBase<Spell2Test>
    {
        /// <summary>Default constructor</summary>
        public Spell2TestControl()
        {
            this.InitializeComponent();
            this.Harness = new Spell2Test();
            this.InitializeControlFields();
        }
    }
}