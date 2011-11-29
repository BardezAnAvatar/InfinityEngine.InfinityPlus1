using System;
using System.Windows.Forms;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Character;
using Bardez.Projects.InfinityPlus1.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.CHR
{
    /// <summary>User control for testing the Character 1 file class</summary>
    public class Character1TestControl : HarnessFileBaseTestControlBase<Character1Test>
    {
        /// <summary>Default constructor</summary>
        public Character1TestControl()
        {
            this.InitializeComponent();
            this.Harness = new Character1Test();
            this.InitializeControlFields();
        }
    }
}