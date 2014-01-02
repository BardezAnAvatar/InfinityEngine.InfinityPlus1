using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.WalledEnvironmentDisplay;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.WED
{
    /// <summary>User control for testing the WED file class</summary>
    public class WedOverlayTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public WedOverlayTestControl()
        {
            this.InitializeComponent();
            this.Harness = new WedOverlayTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}