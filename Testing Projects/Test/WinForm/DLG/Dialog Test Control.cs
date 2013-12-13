using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Dialog;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.DLG
{
    /// <summary>User control for testing the Dialog file class</summary>
    public class DialogTestControl : HarnessFileBaseTestControlBase<DialogTest>
    {
        /// <summary>Default constructor</summary>
        public DialogTestControl()
        {
            this.InitializeComponent();
            this.Harness = new DialogTest();
            this.InitializeControlFields();
        }
    }
}