using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.ChitinKey;
using Bardez.Projects.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.KEY
{
    /// <summary>User control for testing the ChitinKey BIFF entry class</summary>
    public class KeyBiffEntryTestControl : HarnessFileBaseTestControlBase<ChitinKey1BifEntryTest>
    {
        /// <summary>Default constructor</summary>
        public KeyBiffEntryTestControl()
        {
            this.InitializeComponent();
            this.Harness = new ChitinKey1BifEntryTest();
            this.InitializeControlFields();
        }
    }
}