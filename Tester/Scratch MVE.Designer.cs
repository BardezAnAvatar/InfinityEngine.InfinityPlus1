namespace Bardez.Projects.InfinityPlus1.Tester
{
    partial class ScratchMve
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
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.direct2dRenderControl1 = new Bardez.Projects.InfinityPlus1.Output.Visual.Direct2dRenderControl();
            this.btnNextFrame = new System.Windows.Forms.Button();
            this.labelFrameNum = new System.Windows.Forms.Label();
            this.labelFrameNumDisplay = new System.Windows.Forms.Label();
            this.btnPlay = new System.Windows.Forms.Button();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnStop = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnReset = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.direct2dRenderControl1);
            this.panel1.Location = new System.Drawing.Point(12, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(664, 494);
            this.panel1.TabIndex = 1;
            // 
            // direct2dRenderControl1
            // 
            this.direct2dRenderControl1.BackColor = System.Drawing.SystemColors.Control;
            this.direct2dRenderControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.direct2dRenderControl1.Location = new System.Drawing.Point(0, 0);
            this.direct2dRenderControl1.Name = "direct2dRenderControl1";
            this.direct2dRenderControl1.Size = new System.Drawing.Size(660, 490);
            this.direct2dRenderControl1.TabIndex = 0;
            // 
            // btnNextFrame
            // 
            this.btnNextFrame.Location = new System.Drawing.Point(3, 3);
            this.btnNextFrame.Name = "btnNextFrame";
            this.btnNextFrame.Size = new System.Drawing.Size(75, 23);
            this.btnNextFrame.TabIndex = 0;
            this.btnNextFrame.Text = "Next Frame";
            this.btnNextFrame.UseVisualStyleBackColor = true;
            this.btnNextFrame.Click += new System.EventHandler(this.btnNextFrame_Click);
            // 
            // labelFrameNum
            // 
            this.labelFrameNum.AutoSize = true;
            this.labelFrameNum.Location = new System.Drawing.Point(3, 6);
            this.labelFrameNum.Name = "labelFrameNum";
            this.labelFrameNum.Size = new System.Drawing.Size(49, 13);
            this.labelFrameNum.TabIndex = 2;
            this.labelFrameNum.Text = "Frame #:";
            // 
            // labelFrameNumDisplay
            // 
            this.labelFrameNumDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFrameNumDisplay.AutoSize = true;
            this.labelFrameNumDisplay.Location = new System.Drawing.Point(58, 6);
            this.labelFrameNumDisplay.Name = "labelFrameNumDisplay";
            this.labelFrameNumDisplay.Size = new System.Drawing.Size(0, 13);
            this.labelFrameNumDisplay.TabIndex = 3;
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(84, 3);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(96, 23);
            this.btnPlay.TabIndex = 4;
            this.btnPlay.Text = "Start Playback";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnStop);
            this.pnlButtons.Controls.Add(this.panel3);
            this.pnlButtons.Controls.Add(this.btnReset);
            this.pnlButtons.Controls.Add(this.btnNextFrame);
            this.pnlButtons.Controls.Add(this.btnPlay);
            this.pnlButtons.Location = new System.Drawing.Point(87, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(512, 25);
            this.pnlButtons.TabIndex = 5;
            this.pnlButtons.Visible = false;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(186, 3);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(85, 23);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "Stop Playback";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.labelFrameNum);
            this.panel3.Controls.Add(this.labelFrameNumDisplay);
            this.panel3.Location = new System.Drawing.Point(400, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(109, 21);
            this.panel3.TabIndex = 6;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(277, 3);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "Reset Video";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // Scratch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 535);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Name = "Scratch";
            this.Text = "Scratch";
            this.Load += new System.EventHandler(this.Scratch_Load);
            this.panel1.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private Output.Visual.Direct2dRenderControl direct2dRenderControl1;
        private System.Windows.Forms.Button btnNextFrame;
        private System.Windows.Forms.Label labelFrameNum;
        private System.Windows.Forms.Label labelFrameNumDisplay;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnStop;




    }
}