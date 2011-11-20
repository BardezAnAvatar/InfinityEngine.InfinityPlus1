namespace Bardez.Projects.InfinityPlus1.Test.WinForm
{
    partial class LoggingControl
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
            this.pnlControlParent = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlControlParent
            // 
            this.pnlControlParent.AutoScroll = true;
            this.pnlControlParent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlControlParent.Location = new System.Drawing.Point(0, 0);
            this.pnlControlParent.Name = "pnlControlParent";
            this.pnlControlParent.Size = new System.Drawing.Size(132, 139);
            this.pnlControlParent.TabIndex = 0;
            // 
            // LoggingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlControlParent);
            this.Name = "LoggingControl";
            this.Size = new System.Drawing.Size(132, 139);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlControlParent;
    }
}
