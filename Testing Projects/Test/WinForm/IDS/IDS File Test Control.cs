using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Identifiers;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.IDS
{
    /// <summary>User control for testing the IDS file class</summary>
    public class IdsFileTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public IdsFileTestControl()
        {
            this.InitializeComponent();
            this.Harness = new Identifier_v1_Test();
            this.InitializeControlFields();
        }
        #endregion
    }
}