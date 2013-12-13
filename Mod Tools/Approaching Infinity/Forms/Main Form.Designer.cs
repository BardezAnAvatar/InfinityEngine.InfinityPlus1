namespace Bardez.Projects.InfinityPlus1.Tools.ApproachingInfinity.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelStatus = new System.Windows.Forms.Panel();
            this.lblUserDirectoryPath = new System.Windows.Forms.Label();
            this.lblResourceCount = new System.Windows.Forms.Label();
            this.lblGamePath = new System.Windows.Forms.Label();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.recentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resourceTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewAssetsOverriddenGroupedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewAssetsByTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewAssetsByLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewAssetsOverriddenByLocationAndTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewAllAssetsByLocationAndTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openKeyFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.treeViewAssets = new System.Windows.Forms.TreeView();
            this.splitter = new System.Windows.Forms.Splitter();
            this.tabCtrlFiles = new Bardez.Projects.WinForms.Controls.ClosableTabControl();
            this.lblGameInstalled = new System.Windows.Forms.Label();
            this.panelStatus.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelStatus
            // 
            this.panelStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStatus.Controls.Add(this.lblGameInstalled);
            this.panelStatus.Controls.Add(this.lblUserDirectoryPath);
            this.panelStatus.Controls.Add(this.lblResourceCount);
            this.panelStatus.Controls.Add(this.lblGamePath);
            this.panelStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelStatus.Location = new System.Drawing.Point(0, 400);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(954, 62);
            this.panelStatus.TabIndex = 0;
            // 
            // lblUserDirectoryPath
            // 
            this.lblUserDirectoryPath.AutoSize = true;
            this.lblUserDirectoryPath.Location = new System.Drawing.Point(3, 28);
            this.lblUserDirectoryPath.Name = "lblUserDirectoryPath";
            this.lblUserDirectoryPath.Size = new System.Drawing.Size(107, 13);
            this.lblUserDirectoryPath.TabIndex = 2;
            this.lblUserDirectoryPath.Text = "{User Directory Path}";
            // 
            // lblResourceCount
            // 
            this.lblResourceCount.AutoSize = true;
            this.lblResourceCount.Location = new System.Drawing.Point(3, 2);
            this.lblResourceCount.Name = "lblResourceCount";
            this.lblResourceCount.Size = new System.Drawing.Size(68, 13);
            this.lblResourceCount.TabIndex = 0;
            this.lblResourceCount.Text = "Detected {0}";
            // 
            // lblGamePath
            // 
            this.lblGamePath.AutoSize = true;
            this.lblGamePath.Location = new System.Drawing.Point(3, 15);
            this.lblGamePath.Name = "lblGamePath";
            this.lblGamePath.Size = new System.Drawing.Size(37, 13);
            this.lblGamePath.TabIndex = 1;
            this.lblGamePath.Text = "{Path}";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameToolStripMenuItem,
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(954, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip";
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openGameToolStripMenuItem,
            this.openFileToolStripMenuItem,
            this.refreshTreeToolStripMenuItem,
            this.toolStripSeparator1,
            this.recentToolStripMenuItem,
            this.toolStripSeparator2,
            this.quitToolStripMenuItem});
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.gameToolStripMenuItem.Text = "Game";
            // 
            // openGameToolStripMenuItem
            // 
            this.openGameToolStripMenuItem.Name = "openGameToolStripMenuItem";
            this.openGameToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.openGameToolStripMenuItem.Text = "Open Game";
            this.openGameToolStripMenuItem.Click += new System.EventHandler(this.openGameToolStripMenuItem_Click);
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.openFileToolStripMenuItem.Text = "Open File";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // refreshTreeToolStripMenuItem
            // 
            this.refreshTreeToolStripMenuItem.Enabled = false;
            this.refreshTreeToolStripMenuItem.Name = "refreshTreeToolStripMenuItem";
            this.refreshTreeToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.refreshTreeToolStripMenuItem.Text = "Refresh Tree";
            this.refreshTreeToolStripMenuItem.Click += new System.EventHandler(this.refreshTreeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(136, 6);
            // 
            // recentToolStripMenuItem
            // 
            this.recentToolStripMenuItem.Name = "recentToolStripMenuItem";
            this.recentToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.recentToolStripMenuItem.Text = "Recent";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(136, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.searchToolStripMenuItem.Text = "Search";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resourceTreeToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // resourceTreeToolStripMenuItem
            // 
            this.resourceTreeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewAssetsOverriddenGroupedToolStripMenuItem,
            this.viewAssetsByTypeToolStripMenuItem,
            this.viewAssetsByLocationToolStripMenuItem,
            this.viewAssetsOverriddenByLocationAndTypeToolStripMenuItem,
            this.viewAllAssetsByLocationAndTypeToolStripMenuItem});
            this.resourceTreeToolStripMenuItem.Name = "resourceTreeToolStripMenuItem";
            this.resourceTreeToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.resourceTreeToolStripMenuItem.Text = "Resource Tree";
            // 
            // viewAssetsOverriddenGroupedToolStripMenuItem
            // 
            this.viewAssetsOverriddenGroupedToolStripMenuItem.Name = "viewAssetsOverriddenGroupedToolStripMenuItem";
            this.viewAssetsOverriddenGroupedToolStripMenuItem.Size = new System.Drawing.Size(323, 22);
            this.viewAssetsOverriddenGroupedToolStripMenuItem.Text = "Display assets, overridden, by type";
            this.viewAssetsOverriddenGroupedToolStripMenuItem.Click += new System.EventHandler(this.viewToolStripMeuItem_Click);
            // 
            // viewAssetsByTypeToolStripMenuItem
            // 
            this.viewAssetsByTypeToolStripMenuItem.Name = "viewAssetsByTypeToolStripMenuItem";
            this.viewAssetsByTypeToolStripMenuItem.Size = new System.Drawing.Size(323, 22);
            this.viewAssetsByTypeToolStripMenuItem.Text = "Display all assets, by type";
            this.viewAssetsByTypeToolStripMenuItem.Click += new System.EventHandler(this.viewToolStripMeuItem_Click);
            // 
            // viewAssetsByLocationToolStripMenuItem
            // 
            this.viewAssetsByLocationToolStripMenuItem.Checked = true;
            this.viewAssetsByLocationToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.viewAssetsByLocationToolStripMenuItem.Name = "viewAssetsByLocationToolStripMenuItem";
            this.viewAssetsByLocationToolStripMenuItem.Size = new System.Drawing.Size(323, 22);
            this.viewAssetsByLocationToolStripMenuItem.Text = "Display all assets by location";
            this.viewAssetsByLocationToolStripMenuItem.Click += new System.EventHandler(this.viewToolStripMeuItem_Click);
            // 
            // viewAssetsOverriddenByLocationAndTypeToolStripMenuItem
            // 
            this.viewAssetsOverriddenByLocationAndTypeToolStripMenuItem.Name = "viewAssetsOverriddenByLocationAndTypeToolStripMenuItem";
            this.viewAssetsOverriddenByLocationAndTypeToolStripMenuItem.Size = new System.Drawing.Size(323, 22);
            this.viewAssetsOverriddenByLocationAndTypeToolStripMenuItem.Text = "Display assets, overridden, by location and type";
            this.viewAssetsOverriddenByLocationAndTypeToolStripMenuItem.Click += new System.EventHandler(this.viewToolStripMeuItem_Click);
            // 
            // viewAllAssetsByLocationAndTypeToolStripMenuItem
            // 
            this.viewAllAssetsByLocationAndTypeToolStripMenuItem.Name = "viewAllAssetsByLocationAndTypeToolStripMenuItem";
            this.viewAllAssetsByLocationAndTypeToolStripMenuItem.Size = new System.Drawing.Size(323, 22);
            this.viewAllAssetsByLocationAndTypeToolStripMenuItem.Text = "Display all assets by location and type";
            this.viewAllAssetsByLocationAndTypeToolStripMenuItem.Click += new System.EventHandler(this.viewToolStripMeuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // openKeyFileDialog
            // 
            this.openKeyFileDialog.FileName = "chitin.key";
            this.openKeyFileDialog.Filter = "Key Files|*.key";
            this.openKeyFileDialog.Title = "Please select an infinity key file (chitin.key)";
            // 
            // treeViewAssets
            // 
            this.treeViewAssets.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeViewAssets.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewAssets.Location = new System.Drawing.Point(0, 24);
            this.treeViewAssets.Name = "treeViewAssets";
            this.treeViewAssets.Size = new System.Drawing.Size(204, 376);
            this.treeViewAssets.TabIndex = 4;
            this.treeViewAssets.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewAssets_NodeMouseDoubleClick);
            // 
            // splitter
            // 
            this.splitter.Location = new System.Drawing.Point(204, 24);
            this.splitter.Name = "splitter";
            this.splitter.Size = new System.Drawing.Size(3, 376);
            this.splitter.TabIndex = 5;
            this.splitter.TabStop = false;
            // 
            // tabCtrlFiles
            // 
            this.tabCtrlFiles.ConfirmClose = false;
            this.tabCtrlFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlFiles.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabCtrlFiles.Location = new System.Drawing.Point(207, 24);
            this.tabCtrlFiles.Name = "tabCtrlFiles";
            this.tabCtrlFiles.SelectedIndex = 0;
            this.tabCtrlFiles.Size = new System.Drawing.Size(747, 376);
            this.tabCtrlFiles.TabIndex = 6;
            // 
            // lblGameInstalled
            // 
            this.lblGameInstalled.AutoSize = true;
            this.lblGameInstalled.Location = new System.Drawing.Point(3, 41);
            this.lblGameInstalled.Name = "lblGameInstalled";
            this.lblGameInstalled.Size = new System.Drawing.Size(85, 13);
            this.lblGameInstalled.TabIndex = 3;
            this.lblGameInstalled.Text = "{Installed Game}";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 462);
            this.Controls.Add(this.tabCtrlFiles);
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.treeViewAssets);
            this.Controls.Add(this.panelStatus);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "Approaching Infinity";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panelStatus.ResumeLayout(false);
            this.panelStatus.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshTreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem recentToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label lblResourceCount;
        private System.Windows.Forms.Label lblGamePath;
        private System.Windows.Forms.OpenFileDialog openKeyFileDialog;
        private System.Windows.Forms.Label lblUserDirectoryPath;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resourceTreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewAssetsByTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewAssetsByLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewAssetsOverriddenGroupedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewAssetsOverriddenByLocationAndTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewAllAssetsByLocationAndTypeToolStripMenuItem;
        private System.Windows.Forms.TreeView treeViewAssets;
        private System.Windows.Forms.Splitter splitter;
        private WinForms.Controls.ClosableTabControl tabCtrlFiles;
        private System.Windows.Forms.Label lblGameInstalled;
    }
}