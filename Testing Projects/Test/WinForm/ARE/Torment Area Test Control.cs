using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Area;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.ARE
{
    /// <summary>User control for testing the Icewind Dale ARE 1.0 file class</summary>
    public class TormentAreaTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public TormentAreaTestControl()
        {
            this.InitializeComponent();
            this.Harness = new TormentAreaTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}