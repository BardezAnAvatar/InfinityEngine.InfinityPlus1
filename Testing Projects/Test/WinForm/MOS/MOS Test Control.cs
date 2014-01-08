using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Mosaic;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.MOS
{
    /// <summary>User control for testing the MOS file class</summary>
    public class MosTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public MosTestControl()
        {
            this.InitializeComponent();
            this.Harness = new MosTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}