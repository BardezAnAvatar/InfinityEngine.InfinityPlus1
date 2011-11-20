namespace Bardez.Projects.InfinityPlus1.Test.WinForm
{
    partial class HarnessBaseTestControlBase<HarnessType>
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
            this.chklbTestItems = new System.Windows.Forms.CheckedListBox();
            this.lblListBoxDescriptor = new System.Windows.Forms.Label();
            this.lblOutputDescripor = new System.Windows.Forms.Label();
            this.pnlTest = new System.Windows.Forms.Panel();
            this.btnTestSelected = new System.Windows.Forms.Button();
            this.btnInitialize = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.bntClearLog = new System.Windows.Forms.Button();
            this.logOutput = new Bardez.Projects.InfinityPlus1.Test.WinForm.LoggingControl();
            this.pnlTest.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tblMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // chklbTestItems
            // 
            this.chklbTestItems.CheckOnClick = true;
            this.chklbTestItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chklbTestItems.FormattingEnabled = true;
            this.chklbTestItems.Location = new System.Drawing.Point(3, 23);
            this.chklbTestItems.Name = "chklbTestItems";
            this.chklbTestItems.Size = new System.Drawing.Size(201, 391);
            this.chklbTestItems.TabIndex = 0;
            // 
            // lblListBoxDescriptor
            // 
            this.lblListBoxDescriptor.AutoSize = true;
            this.lblListBoxDescriptor.Location = new System.Drawing.Point(3, 0);
            this.lblListBoxDescriptor.Name = "lblListBoxDescriptor";
            this.lblListBoxDescriptor.Size = new System.Drawing.Size(73, 13);
            this.lblListBoxDescriptor.TabIndex = 1;
            this.lblListBoxDescriptor.Text = "Loaded items:";
            // 
            // lblOutputDescripor
            // 
            this.lblOutputDescripor.AutoSize = true;
            this.lblOutputDescripor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblOutputDescripor.Location = new System.Drawing.Point(210, 0);
            this.lblOutputDescripor.Name = "lblOutputDescripor";
            this.lblOutputDescripor.Size = new System.Drawing.Size(82, 13);
            this.lblOutputDescripor.TabIndex = 0;
            this.lblOutputDescripor.Text = "Harness output:";
            // 
            // pnlTest
            // 
            this.pnlTest.Controls.Add(this.bntClearLog);
            this.pnlTest.Controls.Add(this.btnTestSelected);
            this.pnlTest.Controls.Add(this.btnInitialize);
            this.pnlTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTest.Location = new System.Drawing.Point(3, 3);
            this.pnlTest.Name = "pnlTest";
            this.pnlTest.Size = new System.Drawing.Size(693, 34);
            this.pnlTest.TabIndex = 0;
            // 
            // btnTestSelected
            // 
            this.btnTestSelected.Location = new System.Drawing.Point(84, 3);
            this.btnTestSelected.Name = "btnTestSelected";
            this.btnTestSelected.Size = new System.Drawing.Size(82, 30);
            this.btnTestSelected.TabIndex = 1;
            this.btnTestSelected.Text = "Test Selected";
            this.btnTestSelected.UseVisualStyleBackColor = true;
            this.btnTestSelected.Click += new System.EventHandler(this.btnTestSelected_Click);
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
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Controls.Add(this.chklbTestItems, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblListBoxDescriptor, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblOutputDescripor, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.logOutput, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 43);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(693, 417);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.Controls.Add(this.pnlTest, 0, 0);
            this.tblMain.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.Size = new System.Drawing.Size(699, 463);
            this.tblMain.TabIndex = 2;
            // 
            // bntClearLog
            // 
            this.bntClearLog.Location = new System.Drawing.Point(172, 4);
            this.bntClearLog.Name = "bntClearLog";
            this.bntClearLog.Size = new System.Drawing.Size(82, 30);
            this.bntClearLog.TabIndex = 2;
            this.bntClearLog.Text = "Clear Log";
            this.bntClearLog.UseVisualStyleBackColor = true;
            this.bntClearLog.Click += new System.EventHandler(this.bntClearLog_Click);
            // 
            // logOutput
            // 
            this.logOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logOutput.Location = new System.Drawing.Point(210, 23);
            this.logOutput.Name = "logOutput";
            this.logOutput.Size = new System.Drawing.Size(480, 391);
            this.logOutput.TabIndex = 2;
            // 
            // HarnessBaseTestControlBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tblMain);
            this.Name = "HarnessBaseTestControlBase";
            this.Size = new System.Drawing.Size(699, 463);
            this.pnlTest.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tblMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.CheckedListBox chklbTestItems;
        protected System.Windows.Forms.Label lblListBoxDescriptor;
        protected System.Windows.Forms.Label lblOutputDescripor;
        protected System.Windows.Forms.Panel pnlTest;
        protected System.Windows.Forms.Button btnTestSelected;
        protected System.Windows.Forms.Button btnInitialize;
        protected System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        protected System.Windows.Forms.TableLayoutPanel tblMain;
        private LoggingControl logOutput;
        protected System.Windows.Forms.Button bntClearLog;
    }
}
