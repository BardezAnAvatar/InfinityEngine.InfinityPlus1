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
using Bardez.Projects.InfinityPlus1.Logic.Infinity.Assets;

namespace Bardez.Projects.InfinityPlus1.Tools.ApproachingInfinity.Forms
{
    /// <summary>This is the main form for the Approaching Infinity application</summary>
    public partial class MainForm : Form
    {
        #region Fields
        /// <summary>UI controller</summary>
        protected ApproachingInfinityController controller;
        #endregion


        #region Properties
        /// <summary>Exposes the selected asset tree view</summary>
        protected AssetTree SelectedAssetTree
        {
            get
            {
                AssetTree treeView = AssetTree.AllInstancesLocation; //my favored view, I think

                if (this.viewAssetsOverriddenGroupedToolStripMenuItem.Checked)
                    treeView = AssetTree.OverriddenGrouped;
                else if (this.viewAssetsByTypeToolStripMenuItem.Checked)
                    treeView = AssetTree.AllInstancesGrouped;
                else if (this.viewAssetsByLocationToolStripMenuItem.Checked)
                    treeView = AssetTree.AllInstancesLocation;
                else if (this.viewAssetsOverriddenByLocationAndTypeToolStripMenuItem.Checked)
                    treeView = AssetTree.OverriddenGroupedLocation;
                else if (this.viewAllAssetsByLocationAndTypeToolStripMenuItem.Checked)
                    treeView = AssetTree.AllInstancesGroupedLocation;

                return treeView;
            }
        }
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

            this.controller.OpenKey(chitinKeyFile);
        }

        /// <summary>Sets the view that was set</summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event parameters</param>
        protected void viewToolStripMeuItem_Click(Object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem))
                throw new InvalidOperationException("The sending object was not a ToolStripMenuItem.");

            this.ExclusivelyCheckView(sender as ToolStripMenuItem);
        }

        /// <summary>Form load</summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event parameters</param>
        protected void MainForm_Load(Object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                this.lblGamePath.Text = String.Empty;
                this.lblResourceCount.Text = String.Empty;
                this.lblUserDirectoryPath.Text = String.Empty;
            }
        }
        #endregion


        #region Controller reaction
        /// <summary>Assigns the user path label text</summary>
        /// <param name="path">Path that was found</param>
        protected void AssignUserPath(String path)
        {
            if (path != null)
                this.lblUserDirectoryPath.Text = String.Format("User directory detected:  {0}", path);
            else
                this.lblUserDirectoryPath.Text = "No related user directory detected.";
        }

        /// <summary>Assigns the application path label text</summary>
        /// <param name="path">Path that was found</param>
        protected void AssignGamePath(String path)
        {
            {
                if (path != null)
                    this.lblUserDirectoryPath.Text = String.Format("Application directory detected:  {0}", path);
                else
                    this.lblUserDirectoryPath.Text = "No application directory detected.";
            }
        }

        /// <summary>Refreshes the resource tree</summary>
        protected void RefreshResourceTree()
        {
            AssetNode tree = this.controller.GetAssetTree(this.SelectedAssetTree);
            this.PopulateTree(tree);
        }
        
        /// <summary>Clears the file being displayed</summary>
        protected void ClearFileDisplay()
        {
            //this.splitContainer.Panel2.Controls.Clear();
        }

        /// <summary>Clears the displayed items and restores them to defaults</summary>
        protected void ClearAllDisplay()
        {
            this.treeViewAssets.Nodes.Clear();              //clear the resource tree
            //this.splitContainer.Panel2.Controls.Clear();    //clear the displayed file UC
            this.lblResourceCount.Text = String.Empty;      //clears the resource count
            this.lblGamePath.Text = String.Empty;           //clears the game path
            this.lblUserDirectoryPath.Text = String.Empty;  //clears the directory path
        }
        #endregion


        #region Helper Methods
        /// <summary>Exclusively checks a single view of the display view options.</summary>
        /// <param name="sender">The item that triggers this method</param>
        protected void ExclusivelyCheckView(ToolStripMenuItem sender)
        {
            sender.Checked = true;

            if (sender != this.viewAllAssetsByLocationAndTypeToolStripMenuItem)
                this.viewAllAssetsByLocationAndTypeToolStripMenuItem.Checked = false;

            if (sender != this.viewAssetsByLocationToolStripMenuItem)
                this.viewAssetsByLocationToolStripMenuItem.Checked = false;

            if (sender != this.viewAssetsByTypeToolStripMenuItem)
                this.viewAssetsByTypeToolStripMenuItem.Checked = false;

            if (sender != this.viewAssetsOverriddenByLocationAndTypeToolStripMenuItem)
                this.viewAssetsOverriddenByLocationAndTypeToolStripMenuItem.Checked = false;

            if (sender != this.viewAssetsOverriddenGroupedToolStripMenuItem)
                this.viewAssetsOverriddenGroupedToolStripMenuItem.Checked = false;
        }

        /// <summary>Populates the tree view with the assets provided</summary>
        /// <param name="tree">Tree into which assets should be populated</param>
        protected void PopulateTree(AssetNode tree)
        {
            if (tree == null)
                throw new InvalidOperationException("The AssetNode tree was unexpectedly null.");
            else if (tree.Children == null)
                throw new InvalidOperationException("The tree root's Children was unexpectedly null.");
            else if (tree.Children.Count < 1)
                throw new InvalidOperationException("The tree root's Children was unexpectedly empty.");

            //the topmode node (passed in) is expected to be the root, and can be discarded; its Children are all that is of interest.
            foreach (AssetNode node in tree.Children)
            {
                TreeNode branch = this.PopulateBranch(node);
                this.treeViewAssets.Nodes.Add(branch);
            }
        }

        /// <summary>Recursive function that will interate through each node in the branch and do the same with its children.</summary>
        /// <param name="branch">Branch to populate</param>
        protected TreeNode PopulateBranch(AssetNode branch)
        {
            if (branch == null)
                throw new InvalidOperationException("The AssetNode branch was unexpectedly null.");

            List<TreeNode> children = null;

            if (branch.Children != null && branch.Children.Count > 0)
            {
                children = new List<TreeNode>();
                foreach (AssetNode child in branch.Children)
                    children.Add(this.PopulateBranch(child));
            }

            Int32 indexUnselected = this.GetAssetImageIndex(branch.GeneralType);
            Int32 indexSelected = this.GetAssetImageIndex(branch.GeneralType);

            TreeNode branchRender = null;
            if (children != null)
                branchRender = new TreeNode(branch.Name, indexUnselected, indexUnselected, children.ToArray());
            else
                branchRender = new TreeNode(branch.Name, indexUnselected, indexUnselected);

            return branchRender;
        }

        /// <summary>Maps the asset type to an index for the Treenode</summary>
        /// <param name="asset">Asset type to map</param>
        /// <returns>The index of the image to use</returns>
        protected Int32 GetAssetImageIndex(GeneralizedAssetType asset)
        {
            return 0;
        }
        #endregion

        private void treeViewAssets_AfterSelect(Object sender, TreeViewEventArgs e)
        {
        //    MDI_Test test = new MDI_Test();
        //    test.TopLevel = false;
        //    test.Parent = this;
        //    test.Show();
        }
    }
}