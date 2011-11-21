namespace Bardez.Projects.InfinityPlus1.Test.WinForm
{
    partial class HarnessNonFileBaseTestControlBase<HarnessType>
    {
        /// <summary>Required designer variable.</summary>
        protected System.ComponentModel.IContainer components = null;

        /// <summary>Clean up any resources being used.</summary>
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
            this.pnlTest = new System.Windows.Forms.Panel();
            this.bntClearLog = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnInitialize = new System.Windows.Forms.Button();
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlHarnessOutput = new System.Windows.Forms.Panel();
            this.logOutput = new Bardez.Projects.InfinityPlus1.Test.WinForm.LoggingControl();
            this.lblOutputDescripor = new System.Windows.Forms.Label();
            this.pnlTest.SuspendLayout();
            this.tblMain.SuspendLayout();
            this.pnlHarnessOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTest
            // 
            this.pnlTest.Controls.Add(this.bntClearLog);
            this.pnlTest.Controls.Add(this.btnTest);
            this.pnlTest.Controls.Add(this.btnInitialize);
            this.pnlTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTest.Location = new System.Drawing.Point(3, 3);
            this.pnlTest.Name = "pnlTest";
            this.pnlTest.Size = new System.Drawing.Size(693, 34);
            this.pnlTest.TabIndex = 0;
            // 
            // bntClearLog
            // 
            this.bntClearLog.Location = new System.Drawing.Point(172, 3);
            this.bntClearLog.Name = "bntClearLog";
            this.bntClearLog.Size = new System.Drawing.Size(82, 30);
            this.bntClearLog.TabIndex = 2;
            this.bntClearLog.Text = "Clear Log";
            this.bntClearLog.UseVisualStyleBackColor = true;
            this.bntClearLog.Click += new System.EventHandler(this.bntClearLog_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(84, 3);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(82, 30);
            this.btnTest.TabIndex = 1;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnInitialize
            // 
            this.btnInitialize.Location = new System.Drawing.Point(3, 3);
            this.btnInitialize.Name = "btnInitialize";
            this.btnInitialize.Size = new System.Drawing.Size(75, 30);
            this.btnInitialize.TabIndex = 0;
            this.btnInitialize.Text = "Initialize";
            this.btnInitialize.UseVisualStyleBackColor = true;
            this.btnInitialize.Click += new System.EventHandler(this.btnInitialize_Click);
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.Controls.Add(this.pnlTest, 0, 0);
            this.tblMain.Controls.Add(this.pnlHarnessOutput, 0, 1);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblMain.Size = new System.Drawing.Size(699, 463);
            this.tblMain.TabIndex = 2;
            // 
            // pnlHarnessOutput
            // 
            this.pnlHarnessOutput.Controls.Add(this.logOutput);
            this.pnlHarnessOutput.Controls.Add(this.lblOutputDescripor);
            this.pnlHarnessOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHarnessOutput.Location = new System.Drawing.Point(3, 43);
            this.pnlHarnessOutput.Name = "pnlHarnessOutput";
            this.pnlHarnessOutput.Size = new System.Drawing.Size(693, 417);
            this.pnlHarnessOutput.TabIndex = 4;
            // 
            // logOutput
            // 
            this.logOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logOutput.Location = new System.Drawing.Point(0, 13);
            this.logOutput.Name = "logOutput";
            this.logOutput.Size = new System.Drawing.Size(693, 404);
            this.logOutput.TabIndex = 3;
            // 
            // lblOutputDescripor
            // 
            this.lblOutputDescripor.AutoSize = true;
            this.lblOutputDescripor.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOutputDescripor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblOutputDescripor.Location = new System.Drawing.Point(0, 0);
            this.lblOutputDescripor.Name = "lblOutputDescripor";
            this.lblOutputDescripor.Size = new System.Drawing.Size(82, 13);
            this.lblOutputDescripor.TabIndex = 1;
            this.lblOutputDescripor.Text = "Harness output:";
            // 
            // HarnessNonFileBaseTestControlBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tblMain);
            this.Name = "HarnessNonFileBaseTestControlBase";
            this.Size = new System.Drawing.Size(699, 463);
            this.pnlTest.ResumeLayout(false);
            this.tblMain.ResumeLayout(false);
            this.pnlHarnessOutput.ResumeLayout(false);
            this.pnlHarnessOutput.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Panel pnlTest;
        protected System.Windows.Forms.Button btnTest;
        protected System.Windows.Forms.Button btnInitialize;
        protected System.Windows.Forms.TableLayoutPanel tblMain;
        protected System.Windows.Forms.Button bntClearLog;
        private System.Windows.Forms.Panel pnlHarnessOutput;
        protected LoggingControl logOutput;
        protected System.Windows.Forms.Label lblOutputDescripor;
    }
}
