using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.TalkOverride;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.TalkOverride
{
    /// <summary>User control for testing the TOH file class</summary>
    public class TalkOverrideTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public TalkOverrideTestControl()
        {
            this.InitializeComponent();
            this.Harness = new TalkOverrideTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}