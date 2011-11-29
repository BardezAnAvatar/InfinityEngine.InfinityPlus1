using System;
using System.Windows.Forms;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Character;
using Bardez.Projects.InfinityPlus1.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.CHR
{
    /// <summary>User control for testing the Character 2.2 file class</summary>
    public class Character9TestControl : HarnessFileBaseTestControlBase<Character9Test>
    {
        /// <summary>Default constructor</summary>
        public Character9TestControl()
        {
            this.InitializeComponent();
            this.Harness = new Character9Test();
            this.InitializeControlFields();
        }
    }
}