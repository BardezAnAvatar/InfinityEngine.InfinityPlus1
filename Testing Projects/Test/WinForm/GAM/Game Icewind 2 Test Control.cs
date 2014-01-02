using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Game;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.GAM
{
    /// <summary>User control for testing the Icewind Dale GAM 2.2 file class</summary>
    public class GameIcewind2TestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public GameIcewind2TestControl()
        {
            this.InitializeComponent();
            this.Harness = new GameIcewind2Test();
            this.InitializeControlFields();
        }
        #endregion
    }
}