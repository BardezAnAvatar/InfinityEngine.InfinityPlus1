using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.TalkOverride;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.TOH
{
    /// <summary>User control for testing the TOH file class</summary>
    public class TalkOverrideHeaderTestControl : HarnessFileBaseTestControlBase<TalkOverrideHeaderTest>
    {
        /// <summary>Default constructor</summary>
        public TalkOverrideHeaderTestControl()
        {
            this.InitializeComponent();
            this.Harness = new TalkOverrideHeaderTest();
            this.InitializeControlFields();
        }
    }
}