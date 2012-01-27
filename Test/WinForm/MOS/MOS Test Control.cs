using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.MapOfScreen;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.DLG
{
    /// <summary>User control for testing the MOS file class</summary>
    public class MosTestControl : HarnessFileBaseTestControlBase<MosTest>
    {
        /// <summary>Default constructor</summary>
        public MosTestControl()
        {
            this.InitializeComponent();
            this.Harness = new MosTest();
            this.InitializeControlFields();
        }
    }
}