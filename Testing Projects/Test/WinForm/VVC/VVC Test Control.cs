using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.VectoredEffectVideoCell;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.VVC
{
    /// <summary>User control for testing the VVC file class</summary>
    public class VectoredEffectTestControl : HarnessFileBaseTestControlBase<VvcTest>
    {
        /// <summary>Default constructor</summary>
        public VectoredEffectTestControl()
        {
            this.InitializeComponent();
            this.Harness = new VvcTest();
            this.InitializeControlFields();
        }
    }
}