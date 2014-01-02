using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.AmpitudeCodedModulation;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.ACM
{
    /// <summary>User control for testing the ACM header</summary>
    public class AcmHeaderTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public AcmHeaderTestControl()
        {
            this.InitializeComponent();
            this.Harness = new AcmHeaderTest();
            this.InitializeControlFields();
        }
        #endregion


        #region Event Handlers
        /// <summary>Handler for Test Selected click event</summary>
        /// <param name="sender">Object sending the event</param>
        /// <param name="e">EventArgs for the click event</param>
        protected override void btnTestSelected_Click(Object sender, EventArgs e)
        {
            foreach (Object item in this.chklbTestItems.CheckedItems)
                this.Harness.DoTest(this, new TestEventArgs(item as String));
        }
        #endregion
    }
}
