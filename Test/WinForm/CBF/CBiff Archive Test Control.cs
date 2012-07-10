using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.BioWareIndexFileFormat.CBiff;
using Bardez.Projects.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.CBF
{
    /// <summary>User control for testing the IWD C-BIFF archive</summary>
    //TODO: Create an extractable interface for both SAV and BIFF archives, to expose to a control for
    //  resource extraction
    public class CBiffArchiveTestControl : HarnessFileBaseTestControlBase<CBiffArchiveTest>
    {
        /// <summary>Default constructor</summary>
        public CBiffArchiveTestControl()
        {
            this.InitializeComponent();
            this.Harness = new CBiffArchiveTest();
            this.InitializeControlFields();
        }
    }
}