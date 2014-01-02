using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Spell;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.SPL
{
    /// <summary>User control for testing the Spell 2.0 file header class</summary>
    public class Spell2HeaderTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public Spell2HeaderTestControl()
        {
            this.InitializeComponent();
            this.Harness = new SpellHeader2Test();
            this.InitializeControlFields();
        }
        #endregion
    }
}