using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.TextLocationKey;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.TLK
{
    /// <summary>User control for testing the TLK file header class</summary>
    public class TlkTestControl : HarnessFileBaseTestControlBase<TextLocationKeyHeaderTest>
    {
        /// <summary>Default constructor</summary>
        public TlkTestControl()
        {
            this.InitializeComponent();
            this.Harness = new TextLocationKeyHeaderTest();
            this.InitializeControlFields();
        }
    }
}