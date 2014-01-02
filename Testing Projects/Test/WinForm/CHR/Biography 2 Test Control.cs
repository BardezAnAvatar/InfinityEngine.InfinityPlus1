using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Character;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.CHR
{
    /// <summary>User control for testing the Biography 2 file class</summary>
    public class Biography2TestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public Biography2TestControl()
        {
            this.InitializeComponent();
            this.Harness = new Biography2Test();
            this.InitializeControlFields();
        }
        #endregion
    }
}