using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Area;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.ARE
{
    /// <summary>User control for testing the Baldur's Gate / Galdur's Gate II ARE 1.0 file class</summary>
    public class BaldurAreaTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public BaldurAreaTestControl()
        {
            this.InitializeComponent();
            this.Harness = new BaldurAreaTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}