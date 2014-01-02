using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.BioWareIndexFileFormat.Biff1;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.BIFF.Uncompressed
{
    /// <summary>User control for testing the BIFF archive</summary>
    public class BiffArchiveTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public BiffArchiveTestControl()
        {
            this.InitializeComponent();
            this.Harness = new Biff1ArchiveTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}