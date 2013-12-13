namespace Bardez.Projects.InfinityPlus1.Test.WinForm
{
    partial class SavedResourcesTestControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstboxSaves = new System.Windows.Forms.ListBox();
            this.splitContainerHarnessResults = new System.Windows.Forms.SplitContainer();
            this.lblListBoxDescriptor = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lstboxResourceCollection = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExtract = new System.Windows.Forms.Button();
            this.lblOutputDescriptor = new System.Windows.Forms.Label();
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTest = new System.Windows.Forms.Panel();
            this.btnInitialize = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHarnessResults)).BeginInit();
            this.splitContainerHarnessResults.Panel1.SuspendLayout();
            this.splitContainerHarnessResults.Panel2.SuspendLayout();
            this.splitContainerHarnessResults.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tblMain.SuspendLayout();
            this.pnlTest.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstboxSaves
            // 
            this.lstboxSaves.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstboxSaves.FormattingEnabled = true;
            this.lstboxSaves.Location = new System.Drawing.Point(0, 13);
            this.lstboxSaves.Name = "lstboxSaves";
            this.lstboxSaves.Size = new System.Drawing.Size(227, 400);
            this.lstboxSaves.TabIndex = 62;
            this.lstboxSaves.SelectedIndexChanged += new System.EventHandler(this.lstboxSaves_SelectedIndexChanged);
            // 
            // splitContainerHarnessResults
            // 
            this.splitContainerHarnessResults.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainerHarnessResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerHarnessResults.Location = new System.Drawing.Point(3, 43);
            this.splitContainerHarnessResults.Name = "splitContainerHarnessResults";
            // 
            // splitContainerHarnessResults.Panel1
            // 
            this.splitContainerHarnessResults.Panel1.Controls.Add(this.lstboxSaves);
            this.splitContainerHarnessResults.Panel1.Controls.Add(this.lblListBoxDescriptor);
            // 
            // splitContainerHarnessResults.Panel2
            // 
            this.splitContainerHarnessResults.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainerHarnessResults.Panel2.Controls.Add(this.lblOutputDescriptor);
            this.splitContainerHarnessResults.Size = new System.Drawing.Size(693, 417);
            this.splitContainerHarnessResults.SplitterDistance = 231;
            this.splitContainerHarnessResults.TabIndex = 12;
            // 
            // lblListBoxDescriptor
            // 
            this.lblListBoxDescriptor.AutoSize = true;
            this.lblListBoxDescriptor.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblListBoxDescriptor.Location = new System.Drawing.Point(0, 0);
            this.lblListBoxDescriptor.Name = "lblListBoxDescriptor";
            this.lblListBoxDescriptor.Size = new System.Drawing.Size(73, 13);
            this.lblListBoxDescriptor.TabIndex = 61;
            this.lblListBoxDescriptor.Text = "Loaded items:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lstboxResourceCollection, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 13);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.75F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(454, 400);
            this.tableLayoutPanel1.TabIndex = 85;
            // 
            // lstboxResourceCollection
            // 
            this.lstboxResourceCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstboxResourceCollection.FormattingEnabled = true;
            this.lstboxResourceCollection.Location = new System.Drawing.Point(3, 56);
            this.lstboxResourceCollection.Name = "lstboxResourceCollection";
            this.lstboxResourceCollection.Size = new System.Drawing.Size(448, 341);
            this.lstboxResourceCollection.TabIndex = 85;
            this.lstboxResourceCollection.SelectedIndexChanged += new System.EventHandler(this.lstboxResourceCollection_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExtract);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(448, 47);
            this.panel1.TabIndex = 86;
            // 
            // btnExtract
            // 
            this.btnExtract.Enabled = false;
            this.btnExtract.Location = new System.Drawing.Point(3, 3);
            this.btnExtract.Name = "btnExtract";
            this.btnExtract.Size = new System.Drawing.Size(118, 34);
            this.btnExtract.TabIndex = 21;
            this.btnExtract.Text = "Extract Resource";
            this.btnExtract.UseVisualStyleBackColor = true;
            this.btnExtract.Click += new System.EventHandler(this.btnExtract_Click);
            // 
            // lblOutputDescriptor
            // 
            this.lblOutputDescriptor.AutoSize = true;
            this.lblOutputDescriptor.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOutputDescriptor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblOutputDescriptor.Location = new System.Drawing.Point(0, 0);
            this.lblOutputDescriptor.Name = "lblOutputDescriptor";
            this.lblOutputDescriptor.Size = new System.Drawing.Size(61, 13);
            this.lblOutputDescriptor.TabIndex = 81;
            this.lblOutputDescriptor.Text = "Resources:";
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblMain.Controls.Add(this.pnlTest, 0, 0);
            this.tblMain.Controls.Add(this.splitContainerHarnessResults, 0, 1);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.Size = new System.Drawing.Size(699, 463);
            this.tblMain.TabIndex = 1;
            // 
            // pnlTest
            // 
            this.pnlTest.Controls.Add(this.btnInitialize);
            this.pnlTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTest.Location = new System.Drawing.Point(3, 3);
            this.pnlTest.Name = "pnlTest";
            this.pnlTest.Size = new System.Drawing.Size(693, 34);
            this.pnlTest.TabIndex = 11;
            // 
            // btnInitialize
            // 
            this.btnInitialize.Location = new System.Drawing.Point(5, 3);
            this.btnInitialize.Name = "btnInitialize";
            this.btnInitialize.Size = new System.Drawing.Size(75, 30);
            this.btnInitialize.TabIndex = 20;
            this.btnInitialize.Text = "Initialize";
            this.btnInitialize.UseVisualStyleBackColor = true;
            this.btnInitialize.Click += new System.EventHandler(this.btnInitialize_Click);
            // 
            // SavedResourcesTestControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tblMain);
            this.Name = "SavedResourcesTestControl";
            this.Size = new System.Drawing.Size(699, 463);
            this.splitContainerHarnessResults.Panel1.ResumeLayout(false);
            this.splitContainerHarnessResults.Panel1.PerformLayout();
            this.splitContainerHarnessResults.Panel2.ResumeLayout(false);
            this.splitContainerHarnessResults.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHarnessResults)).EndInit();
            this.splitContainerHarnessResults.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tblMain.ResumeLayout(false);
            this.pnlTest.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstboxSaves;
        private System.Windows.Forms.SplitContainer splitContainerHarnessResults;
        protected System.Windows.Forms.Label lblListBoxDescriptor;
        private System.Windows.Forms.TableLayoutPanel tblMain;
        protected System.Windows.Forms.Panel pnlTest;
        protected System.Windows.Forms.Button btnInitialize;
        protected System.Windows.Forms.Label lblOutputDescriptor;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListBox lstboxResourceCollection;
        private System.Windows.Forms.Panel panel1;
        protected System.Windows.Forms.Button btnExtract;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}
