using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.TextLocationKey;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.TLK
{
    /// <summary>User control for testing the TLK file class</summary>
    public class TlkHeaderTestControl : HarnessFileBaseTestControlBase<TextLocationKeyTest>
    {
        /// <summary>Default constructor</summary>
        public TlkHeaderTestControl()
        {
            this.InitializeComponent();
            this.Harness = new TextLocationKeyTest();
            this.InitializeControlFields();
        }
    }
}