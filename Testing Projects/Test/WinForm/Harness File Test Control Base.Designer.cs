namespace Bardez.Projects.InfinityPlus1.Test.WinForm
{
    partial class HarnessFileBaseTestControlBase
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
            this.btnInvertSelection = new System.Windows.Forms.Button();
            this.btnTestSelected = new System.Windows.Forms.Button();
            this.btnInitialize = new System.Windows.Forms.Button();
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainerHarnessResults = new System.Windows.Forms.SplitContainer();
            this.logOutput = new Bardez.Projects.InfinityPlus1.Test.WinForm.LoggingControl();
            this.pnlTest.SuspendLayout();
            this.tblMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHarnessResults)).BeginInit();
            this.splitContainerHarnessResults.Panel1.SuspendLayout();
            this.splitContainerHarnessResults.Panel2.SuspendLayout();
            this.splitContainerHarnessResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // chklbTestItems
            // 
            this.chklbTestItems.CheckOnClick = true;
            this.chklbTestItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chklbTestItems.FormattingEnabled = true;
            this.chklbTestItems.Location = new System.Drawing.Point(0, 13);
            this.chklbTestItems.Name = "chklbTestItems";
            this.chklbTestItems.Size = new System.Drawing.Size(229, 400);
            this.chklbTestItems.TabIndex = 70;
            // 
            // lblListBoxDescriptor
            // 
            this.lblListBoxDescriptor.AutoSize = true;
            this.lblListBoxDescriptor.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblListBoxDescriptor.Location = new System.Drawing.Point(0, 0);
            this.lblListBoxDescriptor.Name = "lblListBoxDescriptor";
            this.lblListBoxDescriptor.Size = new System.Drawing.Size(73, 13);
            this.lblListBoxDescriptor.TabIndex = 60;
            this.lblListBoxDescriptor.Text = "Loaded items:";
            // 
            // lblOutputDescripor
            // 
            this.lblOutputDescripor.AutoSize = true;
            this.lblOutputDescripor.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOutputDescripor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblOutputDescripor.Location = new System.Drawing.Point(0, 0);
            this.lblOutputDescripor.Name = "lblOutputDescripor";
            this.lblOutputDescripor.Size = new System.Drawing.Size(82, 13);
            this.lblOutputDescripor.TabIndex = 80;
            this.lblOutputDescripor.Text = "Harness output:";
            // 
            // pnlTest
            // 
            this.pnlTest.Controls.Add(this.btnInvertSelection);
            this.pnlTest.Controls.Add(this.btnTestSelected);
            this.pnlTest.Controls.Add(this.btnInitialize);
            this.pnlTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTest.Location = new System.Drawing.Point(3, 3);
            this.pnlTest.Name = "pnlTest";
            this.pnlTest.Size = new System.Drawing.Size(693, 34);
            this.pnlTest.TabIndex = 10;
            // 
            // btnInvertSelection
            // 
            this.btnInvertSelection.Location = new System.Drawing.Point(86, 3);
            this.btnInvertSelection.Name = "btnInvertSelection";
            this.btnInvertSelection.Size = new System.Drawing.Size(90, 30);
            this.btnInvertSelection.TabIndex = 30;
            this.btnInvertSelection.Text = "Invert Selection";
            this.btnInvertSelection.UseVisualStyleBackColor = true;
            this.btnInvertSelection.Click += new System.EventHandler(this.btnInvertSelection_Click);
            // 
            // btnTestSelected
            // 
            this.btnTestSelected.Location = new System.Drawing.Point(182, 3);
            this.btnTestSelected.Name = "btnTestSelected";
            this.btnTestSelected.Size = new System.Drawing.Size(82, 30);
            this.btnTestSelected.TabIndex = 40;
            this.btnTestSelected.Text = "Test Selected";
            this.btnTestSelected.UseVisualStyleBackColor = true;
            this.btnTestSelected.Click += new System.EventHandler(this.btnTestSelected_Click);
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
            // tblMain
            // 
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.Controls.Add(this.splitContainerHarnessResults, 0, 1);
            this.tblMain.Controls.Add(this.pnlTest, 0, 0);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblMain.Size = new System.Drawing.Size(699, 463);
            this.tblMain.TabIndex = 0;
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
            this.splitContainerHarnessResults.Panel1.Controls.Add(this.chklbTestItems);
            this.splitContainerHarnessResults.Panel1.Controls.Add(this.lblListBoxDescriptor);
            // 
            // splitContainerHarnessResults.Panel2
            // 
            this.splitContainerHarnessResults.Panel2.Controls.Add(this.logOutput);
            this.splitContainerHarnessResults.Panel2.Controls.Add(this.lblOutputDescripor);
            this.splitContainerHarnessResults.Size = new System.Drawing.Size(693, 417);
            this.splitContainerHarnessResults.SplitterDistance = 233;
            this.splitContainerHarnessResults.TabIndex = 50;
            // 
            // logOutput
            // 
            this.logOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logOutput.Location = new System.Drawing.Point(0, 13);
            this.logOutput.Name = "logOutput";
            this.logOutput.Size = new System.Drawing.Size(452, 400);
            this.logOutput.TabIndex = 90;
            // 
            // HarnessFileBaseTestControlBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tblMain);
            this.Name = "HarnessFileBaseTestControlBase";
            this.Size = new System.Drawing.Size(699, 463);
            this.pnlTest.ResumeLayout(false);
            this.tblMain.ResumeLayout(false);
            this.splitContainerHarnessResults.Panel1.ResumeLayout(false);
            this.splitContainerHarnessResults.Panel1.PerformLayout();
            this.splitContainerHarnessResults.Panel2.ResumeLayout(false);
            this.splitContainerHarnessResults.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHarnessResults)).EndInit();
            this.splitContainerHarnessResults.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.CheckedListBox chklbTestItems;
        protected System.Windows.Forms.Label lblListBoxDescriptor;
        protected System.Windows.Forms.Label lblOutputDescripor;
        protected System.Windows.Forms.Panel pnlTest;
        protected System.Windows.Forms.Button btnTestSelected;
        protected System.Windows.Forms.Button btnInitialize;
        protected System.Windows.Forms.TableLayoutPanel tblMain;
        protected LoggingControl logOutput;
        protected System.Windows.Forms.SplitContainer splitContainerHarnessResults;
        protected System.Windows.Forms.Button btnInvertSelection;
    }
}
