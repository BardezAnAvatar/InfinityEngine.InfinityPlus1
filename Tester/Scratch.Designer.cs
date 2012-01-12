namespace Bardez.Projects.InfinityPlus1.Tester
{
    partial class Scratch
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
            this.direct2dRenderControl1 = new Bardez.Projects.InfinityPlus1.Output.Visual.Direct2dRenderControl();
            this.SuspendLayout();
            // 
            // direct2dRenderControl1
            // 
            this.direct2dRenderControl1.Location = new System.Drawing.Point(13, 13);
            this.direct2dRenderControl1.Name = "direct2dRenderControl1";
            this.direct2dRenderControl1.Size = new System.Drawing.Size(739, 438);
            this.direct2dRenderControl1.TabIndex = 0;
            this.direct2dRenderControl1.Dock = System.Windows.Forms.DockStyle.Fill;

            // 
            // Scratch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 463);
            this.Controls.Add(this.direct2dRenderControl1);
            this.Name = "Scratch";
            this.Text = "Scratch";
            this.Load += new System.EventHandler(this.Scratch_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Output.Visual.Direct2dRenderControl direct2dRenderControl1;


    }
}