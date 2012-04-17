using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Variables;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.VAR
{
    /// <summary>User control for testing the VAR file class</summary>
    public class VariableTestControl : HarnessFileBaseTestControlBase<VariablesTest>
    {
        /// <summary>Default constructor</summary>
        public VariableTestControl()
        {
            this.InitializeComponent();
            this.Harness = new VariablesTest();
            this.InitializeControlFields();
        }
    }
}