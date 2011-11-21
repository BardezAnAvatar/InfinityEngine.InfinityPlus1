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
    public abstract partial class HarnessNonFileBaseTestControlBase<HarnessType> : UserControl where HarnessType : TesterBase
    {
        /// <summary>Delegate for threaded application, where Invoke is required</summary>
        protected delegate void VoidInvoke();

        /// <summary>Delegate for threaded application, where Invoke is required</summary>
        protected delegate void VoidInvokeParameterBoolean(Boolean parameter);

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
                    this.btnTest.Enabled = enabled;
                }
        }
        
        /// <summary>Method called after the initialize event is finished processing</summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Parameters to the event</param>
        protected virtual void EndHarnessInitialize(Object sender, EventArgs e)
        {
            this.LoadHarnessItems();
            this.PostMessage(this, new LogEventArgs(new LogItem(LogType.Informational, String.Format("Initialization ended: {0}", DateTime.Now.ToShortTimeString()))));
            this.ToggleControls(true);  //enable controls
        }

        /// <summary>Method that will post a message to the log collection object. Intended to be attached to an event that the Harness will raise internally.</summary>
        /// <param name="sender">Object sending the message</param>
        /// <param name="message">Log Message being posted</param>
        protected virtual void PostMessage(Object sender, LogEventArgs message)
        {
            this.logOutput.PostMessage(message.LogMessage.Message);
        }

        /// <summary>Abstract method to load harness items</summary>
        protected virtual void LoadHarnessItems() { }

        #region UI Event Handlers
        /// <summary>Handler for Initialize click event</summary>
        /// <param name="sender">Object sending the event</param>
        /// <param name="e">EventArgs for the click event</param>
        protected virtual void btnInitialize_Click(Object sender, EventArgs e)
        {
            this.ToggleControls(false);
            this.PostMessage(this, new LogEventArgs(new LogItem(LogType.Informational, String.Format("Initialization started: {0}", DateTime.Now.ToShortTimeString()))));
            Thread thread = new Thread(new ThreadStart(this.RunInitializeThread));
            thread.Start();
        }

        /// <summary>Void method to raise the initialization in a separate thread</summary>
        protected virtual void RunInitializeThread()
        {
            this.Harness.DoInitialize(this);
        }

        /// <summary>Handler for Test Selected click event</summary>
        /// <param name="sender">Object sending the event</param>
        /// <param name="e">EventArgs for the click event</param>
        protected virtual void btnTest_Click(Object sender, EventArgs e)
        {
            this.PostMessage(this, new LogEventArgs(new LogItem(LogType.Informational, String.Format("Testing started: {0}", DateTime.Now.ToShortTimeString()))));
            Thread thread = new Thread(new ThreadStart(this.RunTestThread));
            thread.Start();
        }

        /// <summary>Void method to raise the testing in a separate thread</summary>
        protected virtual void RunTestThread()
        {
            this.Harness.DoTest(this, new TestEventArgs());
            this.PostMessage(this, new LogEventArgs(new LogItem(LogType.Informational, String.Format("Testing ended: {0}", DateTime.Now.ToShortTimeString()))));
        }

        /// <summary>Handler for Clear Log click event</summary>
        /// <param name="sender">Object sending the event</param>
        /// <param name="e">EventArgs for the click event</param>
        protected virtual void bntClearLog_Click(Object sender, EventArgs e)
        {
            this.logOutput.ClearControls();
        }
        #endregion
    }
}