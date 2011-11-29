using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.TextLocationKey;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.TLK
{
    /// <summary>User control for testing the TLK file strref class</summary>
    public class TlkStrrefTestControl : HarnessFileBaseTestControlBase<TextLocationKeyStringReferenceEntryTest>
    {
        /// <summary>Default constructor</summary>
        public TlkStrrefTestControl()
        {
            this.InitializeComponent();
            this.Harness = new TextLocationKeyStringReferenceEntryTest();
            this.InitializeControlFields();
        }
    }
}