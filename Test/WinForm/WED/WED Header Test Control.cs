using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.WalledEnvironmentDisplay;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.WED
{
    /// <summary>User control for testing the WED file class</summary>
    public class WedHeaderTestControl : HarnessFileBaseTestControlBase<WedHeaderTest>
    {
        /// <summary>Default constructor</summary>
        public WedHeaderTestControl()
        {
            this.InitializeComponent();
            this.Harness = new WedHeaderTest();
            this.InitializeControlFields();
        }
    }
}