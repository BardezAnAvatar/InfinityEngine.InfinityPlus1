using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.TileSet;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.TIS
{
    /// <summary>User control for testing the TIS file class</summary>
    public class TisHeaderTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public TisHeaderTestControl()
        {
            this.InitializeComponent();
            this.Harness = new TisHeaderTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}