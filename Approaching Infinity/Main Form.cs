using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Bardez.Projects.InfinityPlus1.Tools.Controllers.ApproachingInfinity;

namespace Bardez.Projects.InfinityPlus1.Tools.ApproachingInfinity
{
    /// <summary>This is the main form for the Approaching Infinity application</summary>
    public partial class MainForm : Form
    {
        #region Fields
        /// <summary>UI controller</summary>
        protected ApproachingInfinityController controller;
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public MainForm()
        {
            this.InitializeComponent();
            this.controller = new ApproachingInfinityController();

            //attach event handlers
            this.controller.AssignedApplicationPath += this.AssignGamePath;
            this.controller.AssignedUserPath += this.AssignUserPath;
            this.controller.DetectedFileCount += this.AssignFileCount;
            this.controller.ClearFileControl += this.ClearFileDisplay;
            this.controller.ClearAllDisplay += this.ClearAllDisplay;
            this.controller.RefreshResourceTree += this.RefreshResourceTree;
        }
        #endregion


        #region Menu Bar event Handlers
        /// <summary>Pops up the About window</summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event parameters</param>
        protected void aboutToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        /// <summary>Opens the open game dialog</summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event parameters</param>
        private void openGameToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            this.openKeyFileDialog.ShowDialog();
            String chitinKeyFile = this.openKeyFileDialog.FileName;

            //ResourceMapper mapper = new ResourceMapper(Path.GetDirectoryName(chitinKeyFile), chitinKeyFile, true, null);
        }
        #endregion


        #region Controller reaction
        protected void AssignUserPath(String path);
        protected void AssignGamePath(String path);
        protected void AssignFileCount(Int64 count);

        /// <summary>Refreshes the resource tree</summary>
        protected void RefreshResourceTree();
        
        /// <summary>Clears the file being displayed</summary>
        protected void ClearFileDisplay()
        {
            this.splitContainer.Panel2.Controls.Clear();
        }

        /// <summary>Clears the displayed items and restores them to defaults</summary>
        protected void ClearAllDisplay()
        {
            this.treeViewAssets.Nodes.Clear();              //clear the resource tree
            this.splitContainer.Panel2.Controls.Clear();    //clear the displayed file UC
            this.lblResourceCount.Text = String.Empty;      //clears the resource count
            this.lblGamePath.Text = String.Empty;           //clears the game path
            this.lblUserDirectoryPath.Text = String.Empty;  //clears the directory path
        }
        #endregion
    }
}