using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Spell;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.SPL
{
    /// <summary>User control for testing the Spell file ability class</summary>
    public class SpellAbilityTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public SpellAbilityTestControl()
        {
            this.InitializeComponent();
            this.Harness = new SpellAbilityTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}