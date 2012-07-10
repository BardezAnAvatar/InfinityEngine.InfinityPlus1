using System;
using System.Windows.Forms;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Character;
using Bardez.Projects.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.CHR
{
    /// <summary>User control for testing the Character 2.2 file class</summary>
    public class Character2TestControl : HarnessFileBaseTestControlBase<Character2_2Test>
    {
        /// <summary>Default constructor</summary>
        public Character2TestControl()
        {
            this.InitializeComponent();
            this.Harness = new Character2_2Test();
            this.InitializeControlFields();
        }
    }
}