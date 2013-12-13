using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Game;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.GAM
{
    /// <summary>User control for testing the Baldur's Gate 2 GAM 2.0 file class</summary>
    public class GameBaldurTest2Control : HarnessFileBaseTestControlBase<GameBaldur2Test>
    {
        /// <summary>Default constructor</summary>
        public GameBaldurTest2Control()
        {
            this.InitializeComponent();
            this.Harness = new GameBaldur2Test();
            this.InitializeControlFields();
        }
    }
}