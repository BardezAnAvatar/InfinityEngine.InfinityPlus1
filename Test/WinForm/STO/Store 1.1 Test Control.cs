using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Store;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.STO
{
    /// <summary>User control for testing the Store 1.1 file class</summary>
    public class Store1_1TestControl : HarnessFileBaseTestControlBase<Store1_1Test>
    {
        /// <summary>Default constructor</summary>
        public Store1_1TestControl()
        {
            this.InitializeComponent();
            this.Harness = new Store1_1Test();
            this.InitializeControlFields();
        }
    }
}