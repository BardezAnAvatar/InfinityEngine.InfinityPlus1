using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Movie;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.MVE
{
    /// <summary>User control for testing the MVE indexing class</summary>
    public class MveOpcodeIndexerTestControl : HarnessFileBaseTestControlBase<MveOpcodeIndexerTest>
    {
        /// <summary>Default constructor</summary>
        public MveOpcodeIndexerTestControl()
        {
            this.InitializeComponent();
            this.Harness = new MveOpcodeIndexerTest();
            this.InitializeControlFields();
        }
    }
}