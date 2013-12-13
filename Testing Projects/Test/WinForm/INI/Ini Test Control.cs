using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Initialization;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.INI
{
    /// <summary>User control for testing the INI file class</summary>
    public class IniTestControl : HarnessFileBaseTestControlBase<IniTest>
    {
        /// <summary>Default constructor</summary>
        public IniTestControl()
        {
            this.InitializeComponent();
            this.Harness = new IniTest();
            this.InitializeControlFields();
        }
    }
}