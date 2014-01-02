using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Area;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.ARE
{
    /// <summary>User control for testing the Icewind Dale II ARE 9.1 file class</summary>
    public class Icewind2AreaTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public Icewind2AreaTestControl()
        {
            this.InitializeComponent();
            this.Harness = new Icewind2AreaTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}