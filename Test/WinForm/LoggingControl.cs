using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Bardez.Projects.InfinityPlus1.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm
{
    /// <summary>Control intended to display loggin messages raised by another class</summary>
    public partial class LoggingControl : UserControl
    {
        /// <summary>Default constructor</summary>
        public LoggingControl()
        {
            InitializeComponent();
        }

        /// <summary>Posts a message to this control</summary>
        /// <param name="message">Message to be posted</param>
        public virtual void PostMessage(LogEventArgs logMessage)
        {
            this.AddMessageControl(logMessage.LogMessage);
        }

        /// <summary>Adds a new TextBox control to the panel containing output</summary>
        /// <param name="logMessage">The new item to add</param>
        protected virtual void AddMessageControl(LogItem logMessage)
        {
            if (this.InvokeRequired)
                this.Invoke(new VoidLogItemParameterInvoke(this.AddMessageControl), new Object[] { logMessage });
            else
                this.lstvwLog.Items.Add(new ListViewItem(new String[] { logMessage.Description, logMessage.MessageContext, logMessage.Message }));
        }

        /// <summary>Event handler for when a selected index changes</summary>
        /// <param name="sender">Object sending the event</param>
        /// <param name="e">Standard event arguments</param>
        protected virtual void lstvwLog_SelectedIndexChanged(Object sender, EventArgs e)
        {
            try
            {
                if (this.lstvwLog.SelectedIndices.Count > 0)
                    this.txtbxMessage.Text = this.lstvwLog.Items[this.lstvwLog.SelectedIndices[0]].SubItems[2].Text;
            }
            catch { }
        }
    }
}