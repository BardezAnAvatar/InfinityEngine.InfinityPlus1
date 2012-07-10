using System;
using System.Windows.Forms;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Character;
using Bardez.Projects.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.CHR
{
    /// <summary>User control for testing the Biography 1 file class</summary>
    public class Biography1TestControl : HarnessFileBaseTestControlBase<Biography1Test>
    {
        /// <summary>Default constructor</summary>
        public Biography1TestControl()
        {
            this.InitializeComponent();
            this.Harness = new Biography1Test();
            this.InitializeControlFields();
        }
    }
}