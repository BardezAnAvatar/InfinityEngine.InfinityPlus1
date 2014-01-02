namespace Bardez.Projects.InfinityPlus1.Test.WinForm
{
    partial class HarnessAnimationCollectionTestControlBase
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
            this.btnClearDisplay = new System.Windows.Forms.Button();
            this.direct2dRenderControl = new Bardez.Projects.InfinityPlus1.Output.Visual.Direct2dRenderControl();
            this.lstboxImages = new System.Windows.Forms.ListBox();
            this.splitContainerHarnessResults = new System.Windows.Forms.SplitContainer();
            this.lblListBoxDescriptor = new System.Windows.Forms.Label();
            this.splitContainerAnimationCollection = new System.Windows.Forms.SplitContainer();
            this.lstboxAnimationCollection = new System.Windows.Forms.ListBox();
            this.lblOutputDescriptor = new System.Windows.Forms.Label();
            this.splitContainerImageCollection = new System.Windows.Forms.SplitContainer();
            this.lstboxImageCollection = new System.Windows.Forms.ListBox();
            this.lblImageCollection = new System.Windows.Forms.Label();
            this.lblRenderResults = new System.Windows.Forms.Label();
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTest = new System.Windows.Forms.Panel();
            this.btnChooseColor = new System.Windows.Forms.Button();
            this.btnInitialize = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHarnessResults)).BeginInit();
            this.splitContainerHarnessResults.Panel1.SuspendLayout();
            this.splitContainerHarnessResults.Panel2.SuspendLayout();
            this.splitContainerHarnessResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerAnimationCollection)).BeginInit();
            this.splitContainerAnimationCollection.Panel1.SuspendLayout();
            this.splitContainerAnimationCollection.Panel2.SuspendLayout();
            this.splitContainerAnimationCollection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerImageCollection)).BeginInit();
            this.splitContainerImageCollection.Panel1.SuspendLayout();
            this.splitContainerImageCollection.Panel2.SuspendLayout();
            this.splitContainerImageCollection.SuspendLayout();
            this.tblMain.SuspendLayout();
            this.pnlTest.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClearDisplay
            // 
            this.btnClearDisplay.Location = new System.Drawing.Point(86, 3);
            this.btnClearDisplay.Name = "btnClearDisplay";
            this.btnClearDisplay.Size = new System.Drawing.Size(90, 30);
            this.btnClearDisplay.TabIndex = 31;
            this.btnClearDisplay.Text = "Clear Display";
            this.btnClearDisplay.UseVisualStyleBackColor = true;
            this.btnClearDisplay.Click += new System.EventHandler(this.btnClearDisplay_Click);
            // 
            // direct2dRenderControl
            // 
            this.direct2dRenderControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.direct2dRenderControl.Location = new System.Drawing.Point(0, 13);
            this.direct2dRenderControl.Name = "direct2dRenderControl";
            this.direct2dRenderControl.Size = new System.Drawing.Size(163, 400);
            this.direct2dRenderControl.TabIndex = 82;
            // 
            // lstboxImages
            // 
            this.lstboxImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstboxImages.FormattingEnabled = true;
            this.lstboxImages.Location = new System.Drawing.Point(0, 13);
            this.lstboxImages.Name = "lstboxImages";
            this.lstboxImages.Size = new System.Drawing.Size(227, 400);
            this.lstboxImages.TabIndex = 62;
            this.lstboxImages.SelectedIndexChanged += new System.EventHandler(this.lstboxImages_SelectedIndexChanged);
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
            this.splitContainerHarnessResults.Panel1.Controls.Add(this.lstboxImages);
            this.splitContainerHarnessResults.Panel1.Controls.Add(this.lblListBoxDescriptor);
            // 
            // splitContainerHarnessResults.Panel2
            // 
            this.splitContainerHarnessResults.Panel2.Controls.Add(this.splitContainerAnimationCollection);
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
            // splitContainerAnimationCollection
            // 
            this.splitContainerAnimationCollection.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainerAnimationCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerAnimationCollection.Location = new System.Drawing.Point(0, 0);
            this.splitContainerAnimationCollection.Name = "splitContainerAnimationCollection";
            // 
            // splitContainerAnimationCollection.Panel1
            // 
            this.splitContainerAnimationCollection.Panel1.Controls.Add(this.lstboxAnimationCollection);
            this.splitContainerAnimationCollection.Panel1.Controls.Add(this.lblOutputDescriptor);
            // 
            // splitContainerAnimationCollection.Panel2
            // 
            this.splitContainerAnimationCollection.Panel2.Controls.Add(this.splitContainerImageCollection);
            this.splitContainerAnimationCollection.Size = new System.Drawing.Size(458, 417);
            this.splitContainerAnimationCollection.SplitterDistance = 152;
            this.splitContainerAnimationCollection.TabIndex = 84;
            // 
            // lstboxAnimationCollection
            // 
            this.lstboxAnimationCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstboxAnimationCollection.FormattingEnabled = true;
            this.lstboxAnimationCollection.Location = new System.Drawing.Point(0, 13);
            this.lstboxAnimationCollection.Name = "lstboxAnimationCollection";
            this.lstboxAnimationCollection.Size = new System.Drawing.Size(148, 400);
            this.lstboxAnimationCollection.TabIndex = 82;
            this.lstboxAnimationCollection.SelectedIndexChanged += new System.EventHandler(this.lstboxAnimationCollection_SelectedIndexChanged);
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
            this.lblOutputDescriptor.Text = "Animations:";
            // 
            // splitContainerImageCollection
            // 
            this.splitContainerImageCollection.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainerImageCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerImageCollection.Location = new System.Drawing.Point(0, 0);
            this.splitContainerImageCollection.Name = "splitContainerImageCollection";
            // 
            // splitContainerImageCollection.Panel1
            // 
            this.splitContainerImageCollection.Panel1.Controls.Add(this.lstboxImageCollection);
            this.splitContainerImageCollection.Panel1.Controls.Add(this.lblImageCollection);
            // 
            // splitContainerImageCollection.Panel2
            // 
            this.splitContainerImageCollection.Panel2.Controls.Add(this.direct2dRenderControl);
            this.splitContainerImageCollection.Panel2.Controls.Add(this.lblRenderResults);
            this.splitContainerImageCollection.Size = new System.Drawing.Size(302, 417);
            this.splitContainerImageCollection.SplitterDistance = 131;
            this.splitContainerImageCollection.TabIndex = 83;
            // 
            // lstboxImageCollection
            // 
            this.lstboxImageCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstboxImageCollection.FormattingEnabled = true;
            this.lstboxImageCollection.Location = new System.Drawing.Point(0, 13);
            this.lstboxImageCollection.Name = "lstboxImageCollection";
            this.lstboxImageCollection.Size = new System.Drawing.Size(127, 400);
            this.lstboxImageCollection.TabIndex = 63;
            this.lstboxImageCollection.SelectedIndexChanged += new System.EventHandler(this.lstboxImageCollection_SelectedIndexChanged);
            // 
            // lblImageCollection
            // 
            this.lblImageCollection.AutoSize = true;
            this.lblImageCollection.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblImageCollection.Location = new System.Drawing.Point(0, 0);
            this.lblImageCollection.Name = "lblImageCollection";
            this.lblImageCollection.Size = new System.Drawing.Size(85, 13);
            this.lblImageCollection.TabIndex = 64;
            this.lblImageCollection.Text = "Image Collection";
            // 
            // lblRenderResults
            // 
            this.lblRenderResults.AutoSize = true;
            this.lblRenderResults.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRenderResults.Location = new System.Drawing.Point(0, 0);
            this.lblRenderResults.Name = "lblRenderResults";
            this.lblRenderResults.Size = new System.Drawing.Size(80, 13);
            this.lblRenderResults.TabIndex = 0;
            this.lblRenderResults.Text = "Render Output:";
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
            this.pnlTest.Controls.Add(this.btnChooseColor);
            this.pnlTest.Controls.Add(this.btnClearDisplay);
            this.pnlTest.Controls.Add(this.btnInitialize);
            this.pnlTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTest.Location = new System.Drawing.Point(3, 3);
            this.pnlTest.Name = "pnlTest";
            this.pnlTest.Size = new System.Drawing.Size(693, 34);
            this.pnlTest.TabIndex = 11;
            // 
            // btnChooseColor
            // 
            this.btnChooseColor.Location = new System.Drawing.Point(182, 3);
            this.btnChooseColor.Name = "btnChooseColor";
            this.btnChooseColor.Size = new System.Drawing.Size(101, 30);
            this.btnChooseColor.TabIndex = 32;
            this.btnChooseColor.Text = "Background Color";
            this.btnChooseColor.UseVisualStyleBackColor = true;
            this.btnChooseColor.Click += new System.EventHandler(this.btnChooseColor_Click);
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
            // HarnessAnimationCollectionTestControlBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tblMain);
            this.Name = "HarnessAnimationCollectionTestControlBase";
            this.Size = new System.Drawing.Size(699, 463);
            this.splitContainerHarnessResults.Panel1.ResumeLayout(false);
            this.splitContainerHarnessResults.Panel1.PerformLayout();
            this.splitContainerHarnessResults.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHarnessResults)).EndInit();
            this.splitContainerHarnessResults.ResumeLayout(false);
            this.splitContainerAnimationCollection.Panel1.ResumeLayout(false);
            this.splitContainerAnimationCollection.Panel1.PerformLayout();
            this.splitContainerAnimationCollection.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerAnimationCollection)).EndInit();
            this.splitContainerAnimationCollection.ResumeLayout(false);
            this.splitContainerImageCollection.Panel1.ResumeLayout(false);
            this.splitContainerImageCollection.Panel1.PerformLayout();
            this.splitContainerImageCollection.Panel2.ResumeLayout(false);
            this.splitContainerImageCollection.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerImageCollection)).EndInit();
            this.splitContainerImageCollection.ResumeLayout(false);
            this.tblMain.ResumeLayout(false);
            this.pnlTest.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Button btnClearDisplay;
        private System.Windows.Forms.ListBox lstboxImages;
        private System.Windows.Forms.SplitContainer splitContainerHarnessResults;
        protected System.Windows.Forms.Label lblListBoxDescriptor;
        private System.Windows.Forms.TableLayoutPanel tblMain;
        protected System.Windows.Forms.Panel pnlTest;
        protected System.Windows.Forms.Button btnInitialize;
        private System.Windows.Forms.SplitContainer splitContainerImageCollection;
        private System.Windows.Forms.ListBox lstboxImageCollection;
        private System.Windows.Forms.Label lblImageCollection;
        private System.Windows.Forms.Label lblRenderResults;
        protected System.Windows.Forms.Label lblOutputDescriptor;
        protected InfinityPlus1.Output.Visual.Direct2dRenderControl direct2dRenderControl;
        protected System.Windows.Forms.Button btnChooseColor;
        private System.Windows.Forms.SplitContainer splitContainerAnimationCollection;
        private System.Windows.Forms.ListBox lstboxAnimationCollection;
    }
}
