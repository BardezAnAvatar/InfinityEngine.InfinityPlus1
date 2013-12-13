using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.StringReferenceCount;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.SRC
{
    /// <summary>User control for testing the String Reference Count file class</summary>
    public class SrcTestControl : HarnessFileBaseTestControlBase<Src1Test>
    {
        /// <summary>Default constructor</summary>
        public SrcTestControl()
        {
            this.InitializeComponent();
            this.Harness = new Src1Test();
            this.InitializeControlFields();
        }
    }
}