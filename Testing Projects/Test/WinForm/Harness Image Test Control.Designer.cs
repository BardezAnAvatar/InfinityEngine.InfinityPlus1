namespace Bardez.Projects.InfinityPlus1.Test.WinForm
{
    partial class HarnessImageTestControl
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
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTest = new System.Windows.Forms.Panel();
            this.btnChooseColor = new System.Windows.Forms.Button();
            this.btnClearDisplay = new System.Windows.Forms.Button();
            this.btnInitialize = new System.Windows.Forms.Button();
            this.splitContainerHarnessResults = new System.Windows.Forms.SplitContainer();
            this.lstboxImages = new System.Windows.Forms.ListBox();
            this.lblListBoxDescriptor = new System.Windows.Forms.Label();
            this.direct2dRenderControl = new Bardez.Projects.InfinityPlus1.Output.Visual.Direct2dRenderControl();
            this.lblRenderResults = new System.Windows.Forms.Label();
            this.tblMain.SuspendLayout();
            this.pnlTest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHarnessResults)).BeginInit();
            this.splitContainerHarnessResults.Panel1.SuspendLayout();
            this.splitContainerHarnessResults.Panel2.SuspendLayout();
            this.splitContainerHarnessResults.SuspendLayout();
            this.SuspendLayout();
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
            this.tblMain.Size = new System.Drawing.Size(849, 438);
            this.tblMain.TabIndex = 2;
            // 
            // pnlTest
            // 
            this.pnlTest.Controls.Add(this.btnChooseColor);
            this.pnlTest.Controls.Add(this.btnClearDisplay);
            this.pnlTest.Controls.Add(this.btnInitialize);
            this.pnlTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTest.Location = new System.Drawing.Point(3, 3);
            this.pnlTest.Name = "pnlTest";
            this.pnlTest.Size = new System.Drawing.Size(843, 34);
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
            this.splitContainerHarnessResults.Panel2.Controls.Add(this.direct2dRenderControl);
            this.splitContainerHarnessResults.Panel2.Controls.Add(this.lblRenderResults);
            this.splitContainerHarnessResults.Size = new System.Drawing.Size(843, 392);
            this.splitContainerHarnessResults.SplitterDistance = 281;
            this.splitContainerHarnessResults.TabIndex = 12;
            // 
            // lstboxImages
            // 
            this.lstboxImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstboxImages.FormattingEnabled = true;
            this.lstboxImages.Location = new System.Drawing.Point(0, 13);
            this.lstboxImages.Name = "lstboxImages";
            this.lstboxImages.Size = new System.Drawing.Size(277, 375);
            this.lstboxImages.TabIndex = 62;
            this.lstboxImages.SelectedIndexChanged += new System.EventHandler(this.lstboxImages_SelectedIndexChanged);
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
            // direct2dRenderControl
            // 
            this.direct2dRenderControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.direct2dRenderControl.Location = new System.Drawing.Point(0, 13);
            this.direct2dRenderControl.Name = "direct2dRenderControl";
            this.direct2dRenderControl.Size = new System.Drawing.Size(554, 375);
            this.direct2dRenderControl.TabIndex = 84;
            // 
            // lblRenderResults
            // 
            this.lblRenderResults.AutoSize = true;
            this.lblRenderResults.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRenderResults.Location = new System.Drawing.Point(0, 0);
            this.lblRenderResults.Name = "lblRenderResults";
            this.lblRenderResults.Size = new System.Drawing.Size(80, 13);
            this.lblRenderResults.TabIndex = 83;
            this.lblRenderResults.Text = "Render Output:";
            // 
            // HarnessImageTestControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tblMain);
            this.Name = "HarnessImageTestControl";
            this.Size = new System.Drawing.Size(849, 438);
            this.tblMain.ResumeLayout(false);
            this.pnlTest.ResumeLayout(false);
            this.splitContainerHarnessResults.Panel1.ResumeLayout(false);
            this.splitContainerHarnessResults.Panel1.PerformLayout();
            this.splitContainerHarnessResults.Panel2.ResumeLayout(false);
            this.splitContainerHarnessResults.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHarnessResults)).EndInit();
            this.splitContainerHarnessResults.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblMain;
        protected System.Windows.Forms.Panel pnlTest;
        protected System.Windows.Forms.Button btnChooseColor;
        protected System.Windows.Forms.Button btnClearDisplay;
        protected System.Windows.Forms.Button btnInitialize;
        private System.Windows.Forms.SplitContainer splitContainerHarnessResults;
        private System.Windows.Forms.ListBox lstboxImages;
        protected System.Windows.Forms.Label lblListBoxDescriptor;
        protected InfinityPlus1.Output.Visual.Direct2dRenderControl direct2dRenderControl;
        private System.Windows.Forms.Label lblRenderResults;
    }
}
