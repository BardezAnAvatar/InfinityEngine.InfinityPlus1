using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.AmpitudeCodedModulation;
using Bardez.Projects.InfinityPlus1.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm
{
    /// <summary>User Control that is a base for a testing harness User Control</summary>
    public abstract partial class HarnessBaseTestControlBase<HarnessType> : UserControl where HarnessType : FileTesterBase
    {
        /// <summary>Testing harness</summary>
        protected HarnessType Harness;

        /// <summary>Code to initialize the user control</summary>
        protected virtual void InitializeControlFields()
        {
            this.Harness.Logger.LogMessage += new LogMessageHandler(this.PostMessage);
            this.Harness.EndInitialize += new EndInitializeTestClass(this.EndHarnessInitialize);
        }

        /// <summary>Method called after the initialize event is finished processing</summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Parameters to the event</param>
        protected virtual void EndHarnessInitialize(Object sender, EventArgs e)
        {
            this.LoadHarnessItems();
        }

        /// <summary>Method that will post a message to the log collection object. Intended to be attached to an event that the Harness will raise internally.</summary>
        /// <param name="sender">Object sending the message</param>
        /// <param name="message">Log Message being posted</param>
        protected virtual void PostMessage(Object sender, LogEventArgs message)
        {
            this.logOutput.PostMessage(message.LogMessage.Message);
        }

        /// <summary>Abstract method to load harness items</summary>
        protected virtual void LoadHarnessItems()
        {
            foreach (String path in this.Harness.FilePaths)
                this.chklbTestItems.Items.Add(path, true);
        }

        /// <summary>Handler for Initialize click event</summary>
        /// <param name="sender">Object sending the event</param>
        /// <param name="e">EventArgs for the click event</param>
        protected virtual void btnInitialize_Click(Object sender, EventArgs e)
        {
            this.Harness.DoInitialize(this);
        }

        /// <summary>Handler for Test Selected click event</summary>
        /// <param name="sender">Object sending the event</param>
        /// <param name="e">EventArgs for the click event</param>
        protected abstract void btnTestSelected_Click(Object sender, EventArgs e);

        /// <summary>Handler for Clear Log click event</summary>
        /// <param name="sender">Object sending the event</param>
        /// <param name="e">EventArgs for the click event</param>
        protected virtual void bntClearLog_Click(Object sender, EventArgs e)
        {
            this.logOutput.ClearControls();
        }
    }
}