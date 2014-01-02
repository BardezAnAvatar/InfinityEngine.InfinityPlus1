using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.AmpitudeCodedModulation;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.ACM
{
    /// <summary>User control for testing a BitBlock</summary>
    public class BitBlockTestControl : HarnessNonFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public BitBlockTestControl()
        {
            this.InitializeComponent();
            this.Harness = new BitBlockTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}