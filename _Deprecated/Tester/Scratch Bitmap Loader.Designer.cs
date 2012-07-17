namespace Bardez.Projects.InfinityPlus1.Tester
{
    partial class ScratchBitmapLoader
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
            this.btnLoadBitmapScale = new System.Windows.Forms.Button();
            this.listboxLoading = new System.Windows.Forms.ListBox();
            this.btnLoadBitmapIntoLibAVPicture = new System.Windows.Forms.Button();
            this.btnDeclareAndDisposeLibAVPicture = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLoadBitmapScale
            // 
            this.btnLoadBitmapScale.Location = new System.Drawing.Point(13, 13);
            this.btnLoadBitmapScale.Name = "btnLoadBitmapScale";
            this.btnLoadBitmapScale.Size = new System.Drawing.Size(114, 23);
            this.btnLoadBitmapScale.TabIndex = 0;
            this.btnLoadBitmapScale.Text = "Load Bitmap && Scale";
            this.btnLoadBitmapScale.UseVisualStyleBackColor = true;
            this.btnLoadBitmapScale.Click += new System.EventHandler(this.btnLoadBitmapScale_Click);
            // 
            // listboxLoading
            // 
            this.listboxLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listboxLoading.FormattingEnabled = true;
            this.listboxLoading.Location = new System.Drawing.Point(12, 43);
            this.listboxLoading.Name = "listboxLoading";
            this.listboxLoading.Size = new System.Drawing.Size(465, 264);
            this.listboxLoading.TabIndex = 1;
            // 
            // btnLoadBitmapIntoLibAVPicture
            // 
            this.btnLoadBitmapIntoLibAVPicture.Location = new System.Drawing.Point(133, 14);
            this.btnLoadBitmapIntoLibAVPicture.Name = "btnLoadBitmapIntoLibAVPicture";
            this.btnLoadBitmapIntoLibAVPicture.Size = new System.Drawing.Size(153, 23);
            this.btnLoadBitmapIntoLibAVPicture.TabIndex = 2;
            this.btnLoadBitmapIntoLibAVPicture.Text = "Load BMP into LibAVPicture";
            this.btnLoadBitmapIntoLibAVPicture.UseVisualStyleBackColor = true;
            this.btnLoadBitmapIntoLibAVPicture.Click += new System.EventHandler(this.btnLoadBitmapIntoLibAVPicture_Click);
            // 
            // btnDeclareAndDisposeLibAVPicture
            // 
            this.btnDeclareAndDisposeLibAVPicture.Location = new System.Drawing.Point(292, 14);
            this.btnDeclareAndDisposeLibAVPicture.Name = "btnDeclareAndDisposeLibAVPicture";
            this.btnDeclareAndDisposeLibAVPicture.Size = new System.Drawing.Size(182, 23);
            this.btnDeclareAndDisposeLibAVPicture.TabIndex = 3;
            this.btnDeclareAndDisposeLibAVPicture.Text = "Instantiate && Dispose LibAVPicture";
            this.btnDeclareAndDisposeLibAVPicture.UseVisualStyleBackColor = true;
            this.btnDeclareAndDisposeLibAVPicture.Click += new System.EventHandler(this.btnDeclareAndDisposeLibAVPicture_Click);
            // 
            // ScratchBitmapLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 317);
            this.Controls.Add(this.btnDeclareAndDisposeLibAVPicture);
            this.Controls.Add(this.btnLoadBitmapIntoLibAVPicture);
            this.Controls.Add(this.listboxLoading);
            this.Controls.Add(this.btnLoadBitmapScale);
            this.Name = "ScratchBitmapLoader";
            this.Text = "Scrach Memory Leak explorer in SW Scale";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoadBitmapScale;
        private System.Windows.Forms.ListBox listboxLoading;
        private System.Windows.Forms.Button btnLoadBitmapIntoLibAVPicture;
        private System.Windows.Forms.Button btnDeclareAndDisposeLibAVPicture;

    }
}