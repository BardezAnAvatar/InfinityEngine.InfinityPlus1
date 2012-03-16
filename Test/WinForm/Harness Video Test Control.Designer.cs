﻿namespace Bardez.Projects.InfinityPlus1.Test.WinForm
{
    partial class HarnessVideoTestControl<MovieFormat>
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
            this.pnlVideoControls = new System.Windows.Forms.Panel();
            this.btnNextFrame = new System.Windows.Forms.Button();
            this.btnResetVideo = new System.Windows.Forms.Button();
            this.btnStopPlayback = new System.Windows.Forms.Button();
            this.btnStartPlayback = new System.Windows.Forms.Button();
            this.btnChooseColor = new System.Windows.Forms.Button();
            this.btnClearDisplay = new System.Windows.Forms.Button();
            this.btnInitialize = new System.Windows.Forms.Button();
            this.splitContainerHarnessResults = new System.Windows.Forms.SplitContainer();
            this.lstboxFiles = new System.Windows.Forms.ListBox();
            this.lblListBoxDescriptor = new System.Windows.Forms.Label();
            this.direct2dRenderControl = new Bardez.Projects.InfinityPlus1.Output.Visual.Direct2dRenderControl();
            this.lblRenderResults = new System.Windows.Forms.Label();
            this.tblMain.SuspendLayout();
            this.pnlTest.SuspendLayout();
            this.pnlVideoControls.SuspendLayout();
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
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.Size = new System.Drawing.Size(849, 438);
            this.tblMain.TabIndex = 1;
            // 
            // pnlTest
            // 
            this.pnlTest.Controls.Add(this.pnlVideoControls);
            this.pnlTest.Controls.Add(this.btnChooseColor);
            this.pnlTest.Controls.Add(this.btnClearDisplay);
            this.pnlTest.Controls.Add(this.btnInitialize);
            this.pnlTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTest.Location = new System.Drawing.Point(3, 3);
            this.pnlTest.Name = "pnlTest";
            this.pnlTest.Size = new System.Drawing.Size(843, 41);
            this.pnlTest.TabIndex = 2;
            // 
            // pnlVideoControls
            // 
            this.pnlVideoControls.Controls.Add(this.btnNextFrame);
            this.pnlVideoControls.Controls.Add(this.btnResetVideo);
            this.pnlVideoControls.Controls.Add(this.btnStopPlayback);
            this.pnlVideoControls.Controls.Add(this.btnStartPlayback);
            this.pnlVideoControls.Location = new System.Drawing.Point(287, 3);
            this.pnlVideoControls.Name = "pnlVideoControls";
            this.pnlVideoControls.Size = new System.Drawing.Size(553, 36);
            this.pnlVideoControls.TabIndex = 40;
            this.pnlVideoControls.Visible = false;
            // 
            // btnNextFrame
            // 
            this.btnNextFrame.Location = new System.Drawing.Point(110, 0);
            this.btnNextFrame.Name = "btnNextFrame";
            this.btnNextFrame.Size = new System.Drawing.Size(101, 30);
            this.btnNextFrame.TabIndex = 60;
            this.btnNextFrame.Text = "Next Frame";
            this.btnNextFrame.UseVisualStyleBackColor = true;
            this.btnNextFrame.Click += new System.EventHandler(this.btnNextFrame_Click);
            // 
            // btnResetVideo
            // 
            this.btnResetVideo.Location = new System.Drawing.Point(3, 0);
            this.btnResetVideo.Name = "btnResetVideo";
            this.btnResetVideo.Size = new System.Drawing.Size(101, 30);
            this.btnResetVideo.TabIndex = 50;
            this.btnResetVideo.Text = "Reset Video";
            this.btnResetVideo.UseVisualStyleBackColor = true;
            this.btnResetVideo.Click += new System.EventHandler(this.btnResetVideo_Click);
            // 
            // btnStopPlayback
            // 
            this.btnStopPlayback.Location = new System.Drawing.Point(324, 0);
            this.btnStopPlayback.Name = "btnStopPlayback";
            this.btnStopPlayback.Size = new System.Drawing.Size(101, 30);
            this.btnStopPlayback.TabIndex = 80;
            this.btnStopPlayback.Text = "Stop Playback";
            this.btnStopPlayback.UseVisualStyleBackColor = true;
            this.btnStopPlayback.Click += new System.EventHandler(this.btnStopPlayback_Click);
            // 
            // btnStartPlayback
            // 
            this.btnStartPlayback.Location = new System.Drawing.Point(217, 0);
            this.btnStartPlayback.Name = "btnStartPlayback";
            this.btnStartPlayback.Size = new System.Drawing.Size(101, 30);
            this.btnStartPlayback.TabIndex = 70;
            this.btnStartPlayback.Text = "Start Playback";
            this.btnStartPlayback.UseVisualStyleBackColor = true;
            this.btnStartPlayback.Click += new System.EventHandler(this.btnStartPlayback_Click);
            // 
            // btnChooseColor
            // 
            this.btnChooseColor.Location = new System.Drawing.Point(182, 3);
            this.btnChooseColor.Name = "btnChooseColor";
            this.btnChooseColor.Size = new System.Drawing.Size(101, 30);
            this.btnChooseColor.TabIndex = 30;
            this.btnChooseColor.Text = "Background Color";
            this.btnChooseColor.UseVisualStyleBackColor = true;
            this.btnChooseColor.Click += new System.EventHandler(this.btnChooseColor_Click);
            // 
            // btnClearDisplay
            // 
            this.btnClearDisplay.Location = new System.Drawing.Point(86, 3);
            this.btnClearDisplay.Name = "btnClearDisplay";
            this.btnClearDisplay.Size = new System.Drawing.Size(90, 30);
            this.btnClearDisplay.TabIndex = 20;
            this.btnClearDisplay.Text = "Clear Display";
            this.btnClearDisplay.UseVisualStyleBackColor = true;
            this.btnClearDisplay.Click += new System.EventHandler(this.btnClearDisplay_Click);
            // 
            // btnInitialize
            // 
            this.btnInitialize.Location = new System.Drawing.Point(5, 3);
            this.btnInitialize.Name = "btnInitialize";
            this.btnInitialize.Size = new System.Drawing.Size(75, 30);
            this.btnInitialize.TabIndex = 10;
            this.btnInitialize.Text = "Initialize";
            this.btnInitialize.UseVisualStyleBackColor = true;
            this.btnInitialize.Click += new System.EventHandler(this.btnInitialize_Click);
            // 
            // splitContainerHarnessResults
            // 
            this.splitContainerHarnessResults.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainerHarnessResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerHarnessResults.Location = new System.Drawing.Point(3, 50);
            this.splitContainerHarnessResults.Name = "splitContainerHarnessResults";
            // 
            // splitContainerHarnessResults.Panel1
            // 
            this.splitContainerHarnessResults.Panel1.Controls.Add(this.lstboxFiles);
            this.splitContainerHarnessResults.Panel1.Controls.Add(this.lblListBoxDescriptor);
            // 
            // splitContainerHarnessResults.Panel2
            // 
            this.splitContainerHarnessResults.Panel2.Controls.Add(this.direct2dRenderControl);
            this.splitContainerHarnessResults.Panel2.Controls.Add(this.lblRenderResults);
            this.splitContainerHarnessResults.Size = new System.Drawing.Size(843, 385);
            this.splitContainerHarnessResults.SplitterDistance = 281;
            this.splitContainerHarnessResults.TabIndex = 90;
            // 
            // lstboxImages
            // 
            this.lstboxFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstboxFiles.FormattingEnabled = true;
            this.lstboxFiles.Location = new System.Drawing.Point(0, 13);
            this.lstboxFiles.Name = "lstboxImages";
            this.lstboxFiles.Size = new System.Drawing.Size(277, 368);
            this.lstboxFiles.TabIndex = 110;
            this.lstboxFiles.SelectedIndexChanged += new System.EventHandler(this.lstboxImages_SelectedIndexChanged);
            // 
            // lblListBoxDescriptor
            // 
            this.lblListBoxDescriptor.AutoSize = true;
            this.lblListBoxDescriptor.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblListBoxDescriptor.Location = new System.Drawing.Point(0, 0);
            this.lblListBoxDescriptor.Name = "lblListBoxDescriptor";
            this.lblListBoxDescriptor.Size = new System.Drawing.Size(73, 13);
            this.lblListBoxDescriptor.TabIndex = 100;
            this.lblListBoxDescriptor.Text = "Loaded items:";
            // 
            // direct2dRenderControl
            // 
            this.direct2dRenderControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.direct2dRenderControl.Location = new System.Drawing.Point(0, 13);
            this.direct2dRenderControl.Name = "direct2dRenderControl";
            this.direct2dRenderControl.Size = new System.Drawing.Size(554, 368);
            this.direct2dRenderControl.TabIndex = 130;
            // 
            // lblRenderResults
            // 
            this.lblRenderResults.AutoSize = true;
            this.lblRenderResults.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRenderResults.Location = new System.Drawing.Point(0, 0);
            this.lblRenderResults.Name = "lblRenderResults";
            this.lblRenderResults.Size = new System.Drawing.Size(80, 13);
            this.lblRenderResults.TabIndex = 120;
            this.lblRenderResults.Text = "Render Output:";
            // 
            // HarnessVideoTestControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tblMain);
            this.Name = "HarnessVideoTestControl";
            this.Size = new System.Drawing.Size(849, 438);
            this.tblMain.ResumeLayout(false);
            this.pnlTest.ResumeLayout(false);
            this.pnlVideoControls.ResumeLayout(false);
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
        private System.Windows.Forms.ListBox lstboxFiles;
        protected System.Windows.Forms.Label lblListBoxDescriptor;
        protected InfinityPlus1.Output.Visual.Direct2dRenderControl direct2dRenderControl;
        private System.Windows.Forms.Label lblRenderResults;
        protected System.Windows.Forms.Button btnStopPlayback;
        protected System.Windows.Forms.Button btnStartPlayback;
        protected System.Windows.Forms.Button btnResetVideo;
        private System.Windows.Forms.Panel pnlVideoControls;
        protected System.Windows.Forms.Button btnNextFrame;
    }
}
