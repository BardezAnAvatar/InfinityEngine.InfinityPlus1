using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Mosaic;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.MOS
{
    /// <summary>User control for testing the MOSC file class</summary>
    public class MoscTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public MoscTestControl()
        {
            this.InitializeComponent();
            this.Harness = new MoscTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}