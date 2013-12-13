using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Output.DirectX;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.Output.XAudio2
{
    /// <summary>User control for testing XAudio2</summary>
    public class XAudio2TestControl : HarnessNonFileBaseTestControlBase<XAudio2Test>
    {
        /// <summary>Default constructor</summary>
        public XAudio2TestControl()
        {
            this.InitializeComponent();
            this.Harness = new XAudio2Test();
            this.InitializeControlFields();
        }
    }
}