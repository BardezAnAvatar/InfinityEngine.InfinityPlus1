using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.BiowareIndexFileFormat.Biff1;
using Bardez.Projects.InfinityPlus1.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.BIFF.Uncompressed
{
    /// <summary>User control for testing the BIFF header</summary>
    public class BiffHeaderTestControl : HarnessFileBaseTestControlBase<Biff1HeaderTest>
    {
        /// <summary>Default constructor</summary>
        public BiffHeaderTestControl()
        {
            this.InitializeComponent();
            this.Harness = new Biff1HeaderTest();
            this.InitializeControlFields();
        }
    }
}