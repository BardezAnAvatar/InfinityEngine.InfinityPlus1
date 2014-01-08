using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.PowerVR;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.MOS
{
    /// <summary>User control for testing the PVRZ file class</summary>
    public class PvrzTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public PvrzTestControl()
        {
            this.InitializeComponent();
            this.Harness = new PvrzTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}