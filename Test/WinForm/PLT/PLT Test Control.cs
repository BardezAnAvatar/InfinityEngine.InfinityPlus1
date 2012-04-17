using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.PixelLocationTable;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.PLT
{
    /// <summary>User control for testing the PLT file class</summary>
    public class PltTestControl : HarnessFileBaseTestControlBase<PltTest>
    {
        /// <summary>Default constructor</summary>
        public PltTestControl()
        {
            this.InitializeComponent();
            this.Harness = new PltTest();
            this.InitializeControlFields();
        }
    }
}