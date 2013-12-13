using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.ChitinKey;
using Bardez.Projects.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.KEY
{
    /// <summary>User control for testing the ChitinKey BIFF entry class</summary>
    public class KeyResourceEntryTestControl : HarnessFileBaseTestControlBase<KeyTable1ResourceEntryTest>
    {
        /// <summary>Default constructor</summary>
        public KeyResourceEntryTestControl()
        {
            this.InitializeComponent();
            this.Harness = new KeyTable1ResourceEntryTest();
            this.InitializeControlFields();
        }
    }
}