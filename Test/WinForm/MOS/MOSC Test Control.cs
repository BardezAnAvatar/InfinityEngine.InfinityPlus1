using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.MapOfScreen;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.MOS
{
    /// <summary>User control for testing the MOSC file class</summary>
    public class MoscTestControl : HarnessFileBaseTestControlBase<MoscTest>
    {
        /// <summary>Default constructor</summary>
        public MoscTestControl()
        {
            this.InitializeComponent();
            this.Harness = new MoscTest();
            this.InitializeControlFields();
        }
    }
}