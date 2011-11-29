using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.ChitinKey;
using Bardez.Projects.InfinityPlus1.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.KEY
{
    /// <summary>User control for testing the ChitinKey BIFF entry class</summary>
    public class KeyHeaderTestControl : HarnessFileBaseTestControlBase<ChitinKey1HeaderTest>
    {
        /// <summary>Default constructor</summary>
        public KeyHeaderTestControl()
        {
            this.InitializeComponent();
            this.Harness = new ChitinKey1HeaderTest();
            this.InitializeControlFields();
        }
    }
}