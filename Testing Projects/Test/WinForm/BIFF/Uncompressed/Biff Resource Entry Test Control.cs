using System;
using System.Windows.Forms;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.BioWareIndexFileFormat.Biff1;
using Bardez.Projects.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.BIFF.Uncompressed
{
    /// <summary>User control for testing the BIFF resource entry</summary>
    public class BiffResourceEntryTestControl : HarnessFileBaseTestControlBase<Biff1ResourceEntryTest>
    {
        /// <summary>Default constructor</summary>
        public BiffResourceEntryTestControl()
        {
            this.InitializeComponent();
            this.Harness = new Biff1ResourceEntryTest();
            this.InitializeControlFields();
        }
    }
}