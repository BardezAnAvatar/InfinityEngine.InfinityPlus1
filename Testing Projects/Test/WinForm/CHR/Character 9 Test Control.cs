using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Character;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.CHR
{
    /// <summary>User control for testing the Character 2.2 file class</summary>
    public class Character9TestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public Character9TestControl()
        {
            this.InitializeComponent();
            this.Harness = new Character9Test();
            this.InitializeControlFields();
        }
        #endregion
    }
}