namespace Bardez.Projects.InfinityPlus1.Test.WinForm.MVE
{
    partial class HarnessAudioCollectionTestControlBase
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
        protected virtual void InitializeComponent()
        {
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTest = new System.Windows.Forms.Panel();
            this.btnInitialize = new System.Windows.Forms.Button();
            this.btnStopPlayback = new System.Windows.Forms.Button();
            this.chkbxRenderAudio = new System.Windows.Forms.CheckBox();
            this.btnPlayAudio = new System.Windows.Forms.Button();
            this.splitContainerHarnessResults = new System.Windows.Forms.SplitContainer();
            this.lstboxFiles = new System.Windows.Forms.ListBox();
            this.lblStream = new System.Windows.Forms.Label();
            this.cboStream = new System.Windows.Forms.ComboBox();
            this.lblListBoxDescriptor = new System.Windows.Forms.Label();
            this.logOutput = new Bardez.Projects.InfinityPlus1.Test.WinForm.LoggingControl();
            this.lblOutputDescriptor = new System.Windows.Forms.Label();
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
            this.tblMain.Size = new System.Drawing.Size(699, 463);
            this.tblMain.TabIndex = 0;
            // 
            // pnlTest
            // 
            this.pnlTest.Controls.Add(this.btnInitialize);
            this.pnlTest.Controls.Add(this.btnStopPlayback);
            this.pnlTest.Controls.Add(this.chkbxRenderAudio);
            this.pnlTest.Controls.Add(this.btnPlayAudio);
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
            // btnStopPlayback
            // 
            this.btnStopPlayback.Location = new System.Drawing.Point(174, 3);
            this.btnStopPlayback.Name = "btnStopPlayback";
            this.btnStopPlayback.Size = new System.Drawing.Size(96, 30);
            this.btnStopPlayback.TabIndex = 5;
            this.btnStopPlayback.Text = "Stop Playback";
            this.btnStopPlayback.UseVisualStyleBackColor = true;
            this.btnStopPlayback.Click += new System.EventHandler(this.btnStopPlayback_Click);
            // 
            // chkbxRenderAudio
            // 
            this.chkbxRenderAudio.AutoSize = true;
            this.chkbxRenderAudio.Checked = true;
            this.chkbxRenderAudio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbxRenderAudio.Location = new System.Drawing.Point(276, 11);
            this.chkbxRenderAudio.Name = "chkbxRenderAudio";
            this.chkbxRenderAudio.Size = new System.Drawing.Size(90, 17);
            this.chkbxRenderAudio.TabIndex = 3;
            this.chkbxRenderAudio.Text = "Render audio";
            this.chkbxRenderAudio.UseVisualStyleBackColor = true;
            // 
            // btnPlayAudio
            // 
            this.btnPlayAudio.Location = new System.Drawing.Point(86, 3);
            this.btnPlayAudio.Name = "btnPlayAudio";
            this.btnPlayAudio.Size = new System.Drawing.Size(82, 30);
            this.btnPlayAudio.TabIndex = 40;
            this.btnPlayAudio.Text = "Play Audio";
            this.btnPlayAudio.UseVisualStyleBackColor = true;
            this.btnPlayAudio.Click += new System.EventHandler(this.btnPlayAudio_Click);
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
            this.splitContainerHarnessResults.Panel1.Controls.Add(this.lstboxFiles);
            this.splitContainerHarnessResults.Panel1.Controls.Add(this.lblStream);
            this.splitContainerHarnessResults.Panel1.Controls.Add(this.cboStream);
            this.splitContainerHarnessResults.Panel1.Controls.Add(this.lblListBoxDescriptor);
            // 
            // splitContainerHarnessResults.Panel2
            // 
            this.splitContainerHarnessResults.Panel2.Controls.Add(this.logOutput);
            this.splitContainerHarnessResults.Panel2.Controls.Add(this.lblOutputDescriptor);
            this.splitContainerHarnessResults.Size = new System.Drawing.Size(693, 417);
            this.splitContainerHarnessResults.SplitterDistance = 231;
            this.splitContainerHarnessResults.TabIndex = 12;
            // 
            // lstboxFiles
            // 
            this.lstboxFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstboxFiles.FormattingEnabled = true;
            this.lstboxFiles.Location = new System.Drawing.Point(0, 13);
            this.lstboxFiles.Name = "lstboxFiles";
            this.lstboxFiles.Size = new System.Drawing.Size(227, 366);
            this.lstboxFiles.TabIndex = 62;
            // 
            // lblStream
            // 
            this.lblStream.AutoSize = true;
            this.lblStream.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStream.Location = new System.Drawing.Point(0, 379);
            this.lblStream.Name = "lblStream";
            this.lblStream.Size = new System.Drawing.Size(113, 13);
            this.lblStream.TabIndex = 64;
            this.lblStream.Text = "Audio Stream Number:";
            // 
            // cboStream
            // 
            this.cboStream.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cboStream.FormattingEnabled = true;
            this.cboStream.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15"});
            this.cboStream.Location = new System.Drawing.Point(0, 392);
            this.cboStream.Name = "cboStream";
            this.cboStream.Size = new System.Drawing.Size(227, 21);
            this.cboStream.TabIndex = 63;
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
            // logOutput
            // 
            this.logOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logOutput.Location = new System.Drawing.Point(0, 13);
            this.logOutput.Name = "logOutput";
            this.logOutput.Size = new System.Drawing.Size(454, 400);
            this.logOutput.TabIndex = 90;
            // 
            // lblOutputDescriptor
            // 
            this.lblOutputDescriptor.AutoSize = true;
            this.lblOutputDescriptor.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOutputDescriptor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblOutputDescriptor.Location = new System.Drawing.Point(0, 0);
            this.lblOutputDescriptor.Name = "lblOutputDescriptor";
            this.lblOutputDescriptor.Size = new System.Drawing.Size(84, 13);
            this.lblOutputDescriptor.TabIndex = 81;
            this.lblOutputDescriptor.Text = "Harness Output:";
            // 
            // HarnessAudioCollectionTestControlBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tblMain);
            this.Name = "HarnessAudioCollectionTestControlBase";
            this.Size = new System.Drawing.Size(699, 463);
            this.tblMain.ResumeLayout(false);
            this.pnlTest.ResumeLayout(false);
            this.pnlTest.PerformLayout();
            this.splitContainerHarnessResults.Panel1.ResumeLayout(false);
            this.splitContainerHarnessResults.Panel1.PerformLayout();
            this.splitContainerHarnessResults.Panel2.ResumeLayout(false);
            this.splitContainerHarnessResults.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHarnessResults)).EndInit();
            this.splitContainerHarnessResults.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.TableLayoutPanel tblMain;
        protected System.Windows.Forms.Panel pnlTest;
        protected System.Windows.Forms.Button btnInitialize;
        protected System.Windows.Forms.SplitContainer splitContainerHarnessResults;
        protected System.Windows.Forms.Label lblListBoxDescriptor;
        protected System.Windows.Forms.Label lblOutputDescriptor;
        protected System.Windows.Forms.ListBox lstboxFiles;
        protected System.Windows.Forms.Button btnPlayAudio;
        protected LoggingControl logOutput;

        /// <summary>Checkbox for whether or not to render audio</summary>
        protected System.Windows.Forms.CheckBox chkbxRenderAudio;

        /// <summary>Button to stop playback</summary>
        protected System.Windows.Forms.Button btnStopPlayback;
        protected System.Windows.Forms.Label lblStream;
        protected System.Windows.Forms.ComboBox cboStream;
    }
}
