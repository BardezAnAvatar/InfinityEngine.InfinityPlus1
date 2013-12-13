using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Game;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.GAM
{
    /// <summary>User control for testing the Planescape: Torment GAM 1.1 file class</summary>
    public class GameTormentTestControl : HarnessFileBaseTestControlBase<GameTormentTest>
    {
        /// <summary>Default constructor</summary>
        public GameTormentTestControl()
        {
            this.InitializeComponent();
            this.Harness = new GameTormentTest();
            this.InitializeControlFields();
        }
    }
}