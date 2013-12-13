using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.TwoDimensionalArray;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm._2DA
{
    /// <summary>User control for testing the 2DA file class</summary>
    public class _2DAFileTestControl : HarnessFileBaseTestControlBase<TwoDinensionalArray1Test>
    {
        /// <summary>Default constructor</summary>
        public _2DAFileTestControl()
        {
            this.InitializeComponent();
            this.Harness = new TwoDinensionalArray1Test();
            this.InitializeControlFields();
        }
    }
}