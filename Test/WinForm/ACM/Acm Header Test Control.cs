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
    /// <summary>User control for testing the ACM header</summary>
    public class AcmHeaderTestControl : HarnessBaseTestControlBase<AcmHeaderTest>
    {
        /// <summary>Default constructor</summary>
        public AcmHeaderTestControl()
        {
            this.InitializeComponent();
            this.Harness = new AcmHeaderTest();
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
