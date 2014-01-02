using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Spell;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.SPL
{
    /// <summary>User control for testing the Spell 1.0 file header class</summary>
    public class Spell1HeaderTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public Spell1HeaderTestControl()
        {
            this.InitializeComponent();
            this.Harness = new SpellHeader1Test();
            this.InitializeControlFields();
        }
        #endregion
    }
}