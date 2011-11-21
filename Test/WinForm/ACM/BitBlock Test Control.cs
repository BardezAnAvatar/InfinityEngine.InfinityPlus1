using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.AmpitudeCodedModulation;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.ACM
{
    public class BitBlockTestControl : HarnessNonFileBaseTestControlBase<BitBlockTest>
    {
        /// <summary>Default constructor</summary>
        public BitBlockTestControl()
        {
            this.InitializeComponent();
            this.Harness = new BitBlockTest();
            this.InitializeControlFields();
        }
    }
}