using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.ReusableCode
{
    /// <summary>User control for testing a BitStream</summary>
    public class BitStreamTestControl : HarnessNonFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public BitStreamTestControl()
        {
            this.InitializeComponent();
            this.Harness = new BitStreamTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}