using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Game;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.GAM
{
    /// <summary>User control for testing the Baldur's Gate GAM 1.1 file class</summary>
    public class GameBaldurTestControl : HarnessFileBaseTestControlBase<GameBaldurTest>
    {
        /// <summary>Default constructor</summary>
        public GameBaldurTestControl()
        {
            this.InitializeComponent();
            this.Harness = new GameBaldurTest();
            this.InitializeControlFields();
        }
    }
}