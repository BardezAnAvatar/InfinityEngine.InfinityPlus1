using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Store;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.STO
{
    /// <summary>User control for testing the Store 1.0 file class</summary>
    public class Store1TestControl : HarnessFileBaseTestControlBase<Store1Test>
    {
        /// <summary>Default constructor</summary>
        public Store1TestControl()
        {
            this.InitializeComponent();
            this.Harness = new Store1Test();
            this.InitializeControlFields();
        }
    }
}