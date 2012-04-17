using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Game;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.GAM
{
    /// <summary>User control for testing the Icewind Dale GAM 1.1 file class</summary>
    public class GameIcewindTestControl : HarnessFileBaseTestControlBase<GameIcewindTest>
    {
        /// <summary>Default constructor</summary>
        public GameIcewindTestControl()
        {
            this.InitializeComponent();
            this.Harness = new GameIcewindTest();
            this.InitializeControlFields();
        }
    }
}