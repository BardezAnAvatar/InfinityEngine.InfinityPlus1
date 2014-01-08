using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.WorldMap;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.WMP
{
    /// <summary>User control for testing the World Map file class</summary>
    public class WorldMapTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public WorldMapTestControl()
        {
            this.InitializeComponent();
            this.Harness = new WorldmapTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}