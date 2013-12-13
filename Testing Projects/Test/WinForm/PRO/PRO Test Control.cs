using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Projectile;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.PRO
{
    /// <summary>User control for testing the PRO file class</summary>
    public class ProTestControl : HarnessFileBaseTestControlBase<ProjectileTest>
    {
        /// <summary>Default constructor</summary>
        public ProTestControl()
        {
            this.InitializeComponent();
            this.Harness = new ProjectileTest();
            this.InitializeControlFields();
        }
    }
}