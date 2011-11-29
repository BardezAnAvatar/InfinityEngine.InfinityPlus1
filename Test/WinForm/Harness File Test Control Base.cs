using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.AmpitudeCodedModulation;
using Bardez.Projects.InfinityPlus1.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm
{
    /// <summary>User Control that is a base for a testing harness User Control</summary>
    public abstract partial class HarnessFileBaseTestControlBase<HarnessType> : UserControl where HarnessType : FileTesterBase
    {
        #region Members
        /// <summary>Testing harness</summary>
        protected HarnessType Harness { get; set; }

        /// <summary>Object for locking on, in toggle controls</summary>
        private Object toggleLock = new Object();
        #endregion

        #region Construction
        /// <summary>Code to initialize the user control</summary>
        protected virtual void InitializeControlFields()
        {
            this.Harness.Logger.LogMessage += new LogMessageHandler(this.PostMessage);
            this.Harness.EndInitialize += new EndInitializeTestClass(this.EndHarnessInitialize);
            this.ToggleControls(false);
        }
        #endregion

        /// <summary>Toggles the availability of controls that are dependand upon testing harness initialization</summary>
        /// <param name="enabled">Flag indicating whether the controls should be enabled or disabled</param>
        protected virtual void ToggleControls(Boolean enabled)
        {
            if (this.InvokeRequired)
                this.Invoke(new VoidInvokeParameterBoolean(this.ToggleControls), new Object[] { enabled });
            else
                lock (this.toggleLock)
                {
                    this.chklbTestItems.Enabled = enabled;
                    this.btnTestSelected.Enabled = enabled;
                    this.btnInvertSelection.Enabled = enabled;
                }
        }
        
        /// <summary>Method called after the initialize event is finished processing</summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Parameters to the event</param>
        protected virtual void EndHarnessInitialize(Object sender, EventArgs e)
        {
            this.LoadHarnessItems();
            this.PostMessage(this, new LogEventArgs(new LogItem(LogType.Informational, String.Format("Initialization ended: {0}", DateTime.Now.ToShortTimeString()), "Initialization", "Ended", this)));
        }

        /// <summary>Method that will post a message to the log collection object. Intended to be attached to an event that the Harness will raise internally.</summary>
        /// <param name="sender">Object sending the message</param>
        /// <param name="message">Log Message being posted</param>
        protected virtual void PostMessage(Object sender, LogEventArgs message)
        {
            this.logOutput.PostMessage(message);
        }

        /// <summary>Abstract method to load harness items</summary>
        protected virtual void LoadHarnessItems()
        {
            if (this.chklbTestItems.InvokeRequired) //check if an invoke is required, call on UI thead
                this.chklbTestItems.Invoke(new VoidInvoke(this.LoadHarnessItems));
            else    //good on existing thread
            {
                foreach (String path in this.Harness.FilePaths)
                    this.chklbTestItems.Items.Add(path, true);

                this.ToggleControls(true);  //enable controls
            }
        }

        #region UI Event Handlers
        /// <summary>Handler for Initialize click event</summary>
        /// <param name="sender">Object sending the event</param>
        /// <param name="e">EventArgs for the click event</param>
        protected virtual void btnInitialize_Click(Object sender, EventArgs e)
        {
            this.chklbTestItems.Items.Clear();  //clear out existing items
            this.ToggleControls(false);
            this.PostMessage(this, new LogEventArgs(new LogItem(LogType.Informational, String.Format("Initialization started: {0}", DateTime.Now.ToShortTimeString()), "Initialization", "Started", this)));
            Thread thread = new Thread(new ThreadStart(this.RunInitializeThread));
            thread.Start();
        }

        /// <summary>Void method to raise the initialization in a separate thread</summary>
        protected virtual void RunInitializeThread()
        {
            this.Harness.DoInitialize(this);
        }

        /// <summary>Inverts the file list selection</summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Standard event arguments</param>
        protected virtual void btnInvertSelection_Click(Object sender, EventArgs e)
        {
            for (Int32 i = 0; i < this.chklbTestItems.Items.Count; ++i)
                this.chklbTestItems.SetItemChecked(i, ! this.chklbTestItems.GetItemChecked(i));
        }

        /// <summary>Handler for Test Selected click event</summary>
        /// <param name="sender">Object sending the event</param>
        /// <param name="e">EventArgs for the click event</param>
        protected virtual void btnTestSelected_Click(Object sender, EventArgs e)
        {
            this.PostMessage(this, new LogEventArgs(new LogItem(LogType.Informational, String.Format("Testing started: {0}", DateTime.Now.ToShortTimeString()), "Testing", "Started", this)));
            Thread thread = new Thread(new ThreadStart(this.RunTestThread));
            thread.Start();
        }

        /// <summary>Void method to raise the testing in a separate thread</summary>
        protected virtual void RunTestThread()
        {
            foreach (Object item in this.chklbTestItems.CheckedItems)
                this.Harness.DoTest(this, new TestEventArgs(item as String));

            this.PostMessage(this, new LogEventArgs(new LogItem(LogType.Informational, String.Format("Testing ended: {0}", DateTime.Now.ToShortTimeString()), "Testing", "Ended", this)));
        }
        #endregion
    }
}