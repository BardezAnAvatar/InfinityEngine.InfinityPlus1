using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.BioWareIndexFileFormat.CBiff;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.CBF
{
    /// <summary>User control for testing the IWD C-BIFF archive</summary>
    //TODO: Create an extractable interface for both SAV and BIFF archives, to expose to a control for
    //  resource extraction
    public class CBiffArchiveTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public CBiffArchiveTestControl()
        {
            this.InitializeComponent();
            this.Harness = new CBiffArchiveTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}