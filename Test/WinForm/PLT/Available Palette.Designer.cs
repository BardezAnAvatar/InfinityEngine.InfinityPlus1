namespace Bardez.Projects.InfinityPlus1.Test.WinForm.PLT
{
    partial class AvailablePalette
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
            this.btnSelect = new System.Windows.Forms.Button();
            this.labelPaletteNumber = new System.Windows.Forms.Label();
            this.panelColor = new System.Windows.Forms.Panel();
            this.panelName = new System.Windows.Forms.Panel();
            this.txtbxPaletteName = new System.Windows.Forms.TextBox();
            this.panelColorNumber = new System.Windows.Forms.Panel();
            this.panelColor.SuspendLayout();
            this.panelName.SuspendLayout();
            this.panelColorNumber.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSelect.Location = new System.Drawing.Point(32, 1);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(0);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(23, 23);
            this.btnSelect.TabIndex = 0;
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // labelPaletteNumber
            // 
            this.labelPaletteNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPaletteNumber.Location = new System.Drawing.Point(0, 0);
            this.labelPaletteNumber.Name = "labelPaletteNumber";
            this.labelPaletteNumber.Size = new System.Drawing.Size(86, 19);
            this.labelPaletteNumber.TabIndex = 1;
            this.labelPaletteNumber.Text = "[Palette Number]";
            this.labelPaletteNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelColor
            // 
            this.panelColor.Controls.Add(this.btnSelect);
            this.panelColor.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelColor.Location = new System.Drawing.Point(0, 0);
            this.panelColor.Margin = new System.Windows.Forms.Padding(0);
            this.panelColor.Name = "panelColor";
            this.panelColor.Size = new System.Drawing.Size(86, 25);
            this.panelColor.TabIndex = 0;
            // 
            // panelName
            // 
            this.panelName.Controls.Add(this.txtbxPaletteName);
            this.panelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelName.Location = new System.Drawing.Point(0, 44);
            this.panelName.Name = "panelName";
            this.panelName.Size = new System.Drawing.Size(86, 30);
            this.panelName.TabIndex = 1;
            // 
            // txtbxPaletteName
            // 
            this.txtbxPaletteName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtbxPaletteName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtbxPaletteName.Location = new System.Drawing.Point(0, 0);
            this.txtbxPaletteName.Multiline = true;
            this.txtbxPaletteName.Name = "txtbxPaletteName";
            this.txtbxPaletteName.ReadOnly = true;
            this.txtbxPaletteName.Size = new System.Drawing.Size(86, 30);
            this.txtbxPaletteName.TabIndex = 3;
            this.txtbxPaletteName.Text = "[Palette Name]";
            this.txtbxPaletteName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panelColorNumber
            // 
            this.panelColorNumber.Controls.Add(this.labelPaletteNumber);
            this.panelColorNumber.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelColorNumber.Location = new System.Drawing.Point(0, 25);
            this.panelColorNumber.Name = "panelColorNumber";
            this.panelColorNumber.Size = new System.Drawing.Size(86, 19);
            this.panelColorNumber.TabIndex = 0;
            // 
            // AvailablePalette
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelName);
            this.Controls.Add(this.panelColorNumber);
            this.Controls.Add(this.panelColor);
            this.MinimumSize = new System.Drawing.Size(85, 74);
            this.Name = "AvailablePalette";
            this.Size = new System.Drawing.Size(86, 74);
            this.panelColor.ResumeLayout(false);
            this.panelName.ResumeLayout(false);
            this.panelName.PerformLayout();
            this.panelColorNumber.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Label labelPaletteNumber;
        private System.Windows.Forms.Panel panelColor;
        private System.Windows.Forms.Panel panelName;
        private System.Windows.Forms.TextBox txtbxPaletteName;
        private System.Windows.Forms.Panel panelColorNumber;
    }
}
