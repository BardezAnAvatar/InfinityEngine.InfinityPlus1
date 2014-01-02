using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Save;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.SAV
{
    /// <summary>User control for testing the SAVE file class</summary>
    public class SaveTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public SaveTestControl()
        {
            this.InitializeComponent();
            this.Harness = new SaveFileTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}