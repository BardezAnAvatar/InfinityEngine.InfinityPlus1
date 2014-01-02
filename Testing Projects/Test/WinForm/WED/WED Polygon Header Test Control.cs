using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.WalledEnvironmentDisplay;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.WED
{
    /// <summary>User control for testing the WED file class</summary>
    public class WedPolygonHeaderTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public WedPolygonHeaderTestControl()
        {
            this.InitializeComponent();
            this.Harness = new WedPolygonHeaderTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}