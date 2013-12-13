using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Riff;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.RIFF
{
    /// <summary>User control for testing the RIFF file class</summary>
    public class RiffTestControl : HarnessFileBaseTestControlBase<RiffTest>
    {
        /// <summary>Default constructor</summary>
        public RiffTestControl()
        {
            this.InitializeComponent();
            this.Harness = new RiffTest();
            this.InitializeControlFields();
        }
    }
}