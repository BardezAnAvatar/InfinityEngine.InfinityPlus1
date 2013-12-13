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
            this.splitLogger = new System.Windows.Forms.SplitContainer();
            this.lstvwLog = new System.Windows.Forms.ListView();
            this.clmhdrName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmhdrContext = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblLog = new System.Windows.Forms.Label();
            this.txtbxMessage = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitLogger)).BeginInit();
            this.splitLogger.Panel1.SuspendLayout();
            this.splitLogger.Panel2.SuspendLayout();
            this.splitLogger.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitLogger
            // 
            this.splitLogger.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitLogger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitLogger.Location = new System.Drawing.Point(0, 0);
            this.splitLogger.Name = "splitLogger";
            // 
            // splitLogger.Panel1
            // 
            this.splitLogger.Panel1.Controls.Add(this.lstvwLog);
            this.splitLogger.Panel1.Controls.Add(this.lblLog);
            // 
            // splitLogger.Panel2
            // 
            this.splitLogger.Panel2.Controls.Add(this.txtbxMessage);
            this.splitLogger.Panel2.Controls.Add(this.lblMessage);
            this.splitLogger.Size = new System.Drawing.Size(726, 511);
            this.splitLogger.SplitterDistance = 291;
            this.splitLogger.TabIndex = 0;
            // 
            // lstvwLog
            // 
            this.lstvwLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmhdrName,
            this.clmhdrContext});
            this.lstvwLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstvwLog.FullRowSelect = true;
            this.lstvwLog.Location = new System.Drawing.Point(0, 13);
            this.lstvwLog.MultiSelect = false;
            this.lstvwLog.Name = "lstvwLog";
            this.lstvwLog.Size = new System.Drawing.Size(287, 494);
            this.lstvwLog.TabIndex = 1;
            this.lstvwLog.UseCompatibleStateImageBehavior = false;
            this.lstvwLog.View = System.Windows.Forms.View.Details;
            this.lstvwLog.SelectedIndexChanged += new System.EventHandler(this.lstvwLog_SelectedIndexChanged);
            // 
            // clmhdrName
            // 
            this.clmhdrName.Text = "Name";
            this.clmhdrName.Width = 75;
            // 
            // clmhdrContext
            // 
            this.clmhdrContext.Text = "Context";
            this.clmhdrContext.Width = 400;
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLog.Location = new System.Drawing.Point(0, 0);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(28, 13);
            this.lblLog.TabIndex = 0;
            this.lblLog.Text = "Log:";
            // 
            // txtbxMessage
            // 
            this.txtbxMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtbxMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtbxMessage.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.txtbxMessage.Location = new System.Drawing.Point(0, 13);
            this.txtbxMessage.Multiline = true;
            this.txtbxMessage.Name = "txtbxMessage";
            this.txtbxMessage.ReadOnly = true;
            this.txtbxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtbxMessage.Size = new System.Drawing.Size(427, 494);
            this.txtbxMessage.TabIndex = 1;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMessage.Location = new System.Drawing.Point(0, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(53, 13);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Message:";
            // 
            // LoggingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitLogger);
            this.Name = "LoggingControl";
            this.Size = new System.Drawing.Size(726, 511);
            this.splitLogger.Panel1.ResumeLayout(false);
            this.splitLogger.Panel1.PerformLayout();
            this.splitLogger.Panel2.ResumeLayout(false);
            this.splitLogger.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitLogger)).EndInit();
            this.splitLogger.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitLogger;
        private System.Windows.Forms.ListView lstvwLog;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.TextBox txtbxMessage;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.ColumnHeader clmhdrName;
        private System.Windows.Forms.ColumnHeader clmhdrContext;

    }
}
