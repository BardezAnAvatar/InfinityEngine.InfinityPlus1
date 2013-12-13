using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Output.DirectX;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.Output.XAudio2
{
    /// <summary>User control for testing XAudio2</summary>
    public class XAudio2RenderTestControl : HarnessNonFileBaseTestControlBase<XAudio2RenderTest>
    {
        /// <summary>Default constructor</summary>
        public XAudio2RenderTestControl()
        {
            this.InitializeComponent();
            this.Harness = new XAudio2RenderTest();
            this.InitializeControlFields();
        }
    }
}