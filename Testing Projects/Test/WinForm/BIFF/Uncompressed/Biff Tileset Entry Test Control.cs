using System;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.BioWareIndexFileFormat.Biff1;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.BIFF.Uncompressed
{
    /// <summary>User control for testing the BIFF tileset entry</summary>
    public class BiffTilesetEntryTestControl : HarnessFileBaseTestControlBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public BiffTilesetEntryTestControl()
        {
            this.InitializeComponent();
            this.Harness = new Biff1TilesetEntryTest();
            this.InitializeControlFields();
        }
        #endregion
    }
}