using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.WaveEffect;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.WFX
{
    /// <summary>User control for testing the WFX file class</summary>
    public class SoundEffectTestControl : HarnessFileBaseTestControlBase<SoundEffectTest>
    {
        /// <summary>Default constructor</summary>
        public SoundEffectTestControl()
        {
            this.InitializeComponent();
            this.Harness = new SoundEffectTest();
            this.InitializeControlFields();
        }
    }
}