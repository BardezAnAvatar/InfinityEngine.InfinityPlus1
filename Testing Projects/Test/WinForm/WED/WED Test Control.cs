using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.WalledEnvironmentDisplay;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.WED
{
    /// <summary>User control for testing the WED file class</summary>
    public class WedTestControl : HarnessFileBaseTestControlBase<WedTest>
    {
        /// <summary>Default constructor</summary>
        public WedTestControl()
        {
            this.InitializeComponent();
            this.Harness = new WedTest();
            this.InitializeControlFields();
        }
    }
}