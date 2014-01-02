using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.BioWareIndexFileFormat.Biff1;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.BIFF.Uncompressed
{
    /// <summary>User control for testing the BIFF resource entry</summary>
    public class BiffResourceEntryTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public BiffResourceEntryTestControl()
        {
            this.InitializeComponent();
            this.Harness = new Biff1ResourceEntryTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}