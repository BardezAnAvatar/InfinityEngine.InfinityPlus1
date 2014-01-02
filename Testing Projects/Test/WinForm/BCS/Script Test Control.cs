using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Script.BCS;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.BCS
{
    /// <summary>User control for testing the BioWare Compiled Script file class</summary>
    public class ScriptTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public ScriptTestControl()
        {
            this.InitializeComponent();
            this.Harness = new BioWareCompiledScriptTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}