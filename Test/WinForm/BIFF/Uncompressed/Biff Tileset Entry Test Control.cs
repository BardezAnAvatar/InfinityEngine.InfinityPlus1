using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.BiowareIndexFileFormat.Biff1;
using Bardez.Projects.InfinityPlus1.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.BIFF.Uncompressed
{
    /// <summary>User control for testing the BIFF tileset entry</summary>
    public class BiffTilesetEntryTestControl : HarnessFileBaseTestControlBase<Biff1TilesetEntryTest>
    {
        /// <summary>Default constructor</summary>
        public BiffTilesetEntryTestControl()
        {
            this.InitializeComponent();
            this.Harness = new Biff1TilesetEntryTest();
            this.InitializeControlFields();
        }
    }
}