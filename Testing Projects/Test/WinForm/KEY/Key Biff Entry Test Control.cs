﻿using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.KeyTable;
using Bardez.Projects.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.KEY
{
    /// <summary>User control for testing the ChitinKey BIFF entry class</summary>
    public class KeyBiffEntryTestControl : HarnessFileBaseTestControlBase<KeyTable1BifEntryTest>
    {
        /// <summary>Default constructor</summary>
        public KeyBiffEntryTestControl()
        {
            this.InitializeComponent();
            this.Harness = new KeyTable1BifEntryTest();
            this.InitializeControlFields();
        }
    }
}