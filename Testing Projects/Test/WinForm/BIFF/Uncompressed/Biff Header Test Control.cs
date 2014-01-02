using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.BioWareIndexFileFormat.Biff1;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.BIFF.Uncompressed
{
    /// <summary>User control for testing the BIFF header</summary>
    public class BiffHeaderTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public BiffHeaderTestControl()
        {
            this.InitializeComponent();
            this.Harness = new Biff1HeaderTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}