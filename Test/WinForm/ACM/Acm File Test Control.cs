using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.AmpitudeCodedModulation;
using Bardez.Projects.InfinityPlus1.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.ACM
{
    /// <summary>User control for testing the ACM file class</summary>
    public class AcmFileTestControl : HarnessBaseTestControlBase<AcmFileTest>
    {
        /// <summary>Default constructor</summary>
        public AcmFileTestControl()
        {
            this.InitializeComponent();
            this.Harness = new AcmFileTest();
            this.InitializeControlFields();
        }

        /// <summary>Handler for Test Selected click event</summary>
        /// <param name="sender">Object sending the event</param>
        /// <param name="e">EventArgs for the click event</param>
        protected override void btnTestSelected_Click(Object sender, EventArgs e)
        {
            foreach (Object item in this.chklbTestItems.CheckedItems)
                this.Harness.DoTest(this, new TestEventArgs(item as String));
        }
    }
}
