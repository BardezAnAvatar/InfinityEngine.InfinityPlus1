using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Spell;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.SPL
{
    /// <summary>User control for testing the Spell file ability effect class</summary>
    public class SpellAbilityEffectTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public SpellAbilityEffectTestControl()
        {
            this.InitializeComponent();
            this.Harness = new SpellAbilityEffectTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}