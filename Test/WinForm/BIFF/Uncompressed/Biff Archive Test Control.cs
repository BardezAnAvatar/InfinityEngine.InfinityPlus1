using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.BioWareIndexFileFormat.Biff1;
using Bardez.Projects.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.BIFF.Uncompressed
{
    /// <summary>User control for testing the BIFF archive</summary>
    public class BiffArchiveTestControl : HarnessFileBaseTestControlBase<Biff1ArchiveTest>
    {
        /// <summary>Default constructor</summary>
        public BiffArchiveTestControl()
        {
            this.InitializeComponent();
            this.Harness = new Biff1ArchiveTest();
            this.InitializeControlFields();
        }
    }
}