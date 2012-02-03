using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.WalledEnvironmentDisplay;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.WED
{
    /// <summary>User control for testing the WED file class</summary>
    public class WedDoorTestControl : HarnessFileBaseTestControlBase<WedDoorTest>
    {
        /// <summary>Default constructor</summary>
        public WedDoorTestControl()
        {
            this.InitializeComponent();
            this.Harness = new WedDoorTest();
            this.InitializeControlFields();
        }
    }
}