using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Character;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.CHR
{
    /// <summary>User control for testing the Biography 1 file class</summary>
    public class Biography1TestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public Biography1TestControl()
        {
            this.InitializeComponent();
            this.Harness = new Biography1Test();
            this.InitializeControlFields();
        }
        #endregion
    }
}