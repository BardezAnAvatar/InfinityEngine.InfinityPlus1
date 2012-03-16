using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Movie;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.MVE
{
    /// <summary>User control for testing the MVE indexing class</summary>
    public class MveIndexerTestControl : HarnessFileBaseTestControlBase<MveIndexerTest>
    {
        /// <summary>Default constructor</summary>
        public MveIndexerTestControl()
        {
            this.InitializeComponent();
            this.Harness = new MveIndexerTest();
            this.InitializeControlFields();
        }
    }
}