using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Store;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.STO
{
    /// <summary>User control for testing the Store 9.0 file class</summary>
    public class Store9TestControl : HarnessFileBaseTestControlBase<Store9Test>
    {
        /// <summary>Default constructor</summary>
        public Store9TestControl()
        {
            this.InitializeComponent();
            this.Harness = new Store9Test();
            this.InitializeControlFields();
        }
    }
}