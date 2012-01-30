using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.TileSet;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.TIS
{
    /// <summary>User control for testing the MOS file class</summary>
    public class TisTestControl : HarnessFileBaseTestControlBase<TisHeaderTest>
    {
        /// <summary>Default constructor</summary>
        public TisTestControl()
        {
            this.InitializeComponent();
            this.Harness = new TisHeaderTest();
            this.InitializeControlFields();
        }
    }
}