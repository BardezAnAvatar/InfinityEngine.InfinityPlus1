using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.TalkOverride;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.TOT
{
    /// <summary>User control for testing the TOT file class</summary>
    public class TalkOverrideTableTestControl : HarnessFileBaseTestControlBase<TalkOverrideTableTest>
    {
        /// <summary>Default constructor</summary>
        public TalkOverrideTableTestControl()
        {
            this.InitializeComponent();
            this.Harness = new TalkOverrideTableTest();
            this.InitializeControlFields();
        }
    }
}