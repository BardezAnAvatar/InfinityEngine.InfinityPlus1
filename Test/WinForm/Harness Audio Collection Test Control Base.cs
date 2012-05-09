using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Resize;
using Bardez.Projects.InfinityPlus1.Test.Harnesses;
using Bardez.Projects.InfinityPlus1.Test.WinForm;
using Bardez.Projects.InfinityPlus1.Utility;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.MVE
{
    /// <summary>Does generic testing on a movie file, playing back selected audio streams</summary>
    public abstract partial class HarnessAudioCollectionTestControlBase<HarnessType> : UserControl where HarnessType : FileTesterBase, IAudioTester
    {
        #region Fields
        /// <summary>Object reference to lock on for the User Interface</summary>
        private Object interfaceLock;

        /// <summary>Testing harness</summary>
        protected HarnessType Harness { get; set; }

        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.MVE.Path";
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public HarnessAudioCollectionTestControlBase()
        {
            this.InitializeComponent();
            this.DoubleBuffered = true;

            this.interfaceLock = new Object();
        }

        /// <summary>Code to initialize the user control</summary>
        protected virtual void InitializeControlFields()
        {
            this.Harness.Logger.LogMessage += new LogMessageHandler(this.PostMessage);
            this.Harness.EndInitialize += new EndInitializeTestClass(this.EndHarnessInitialize);
        }
        #endregion


        #region Event Handlers
        /// <summary>Click event handler for the Initialize button. Loads a list of playable audio files from the config file.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void btnInitialize_Click(Object sender, EventArgs e)
        {
            this.PostMessage(this, new LogEventArgs(new LogItem(LogType.Informational, String.Format("Initializing: {0}", DateTime.Now.ToShortTimeString()), "Testing", "Ended", this)));

            if (this.lstboxFiles.Items.Count < 1)
            {
                List<String> paths = ConfigurationHandlerMulti.GetSettingValues(HarnessAudioCollectionTestControlBase<HarnessType>.configKey);
                lock (this.interfaceLock)
                {
                    foreach (String path in paths)
                        this.lstboxFiles.Items.Add(path);

                    this.cboStream.SelectedIndex = 0;   //select the first index, for precaution
                }
            }

            this.PostMessage(this, new LogEventArgs(new LogItem(LogType.Informational, String.Format("Initialization ended: {0}", DateTime.Now.ToShortTimeString()), "Testing", "Ended", this)));
        }

        /// <summary>Event handler for the Stop Playback button click event</summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Parameters for the event</param>
        protected virtual void btnStopPlayback_Click(Object sender, EventArgs e)
        {
            this.Harness.StopPlayback();
        }

        /// <summary>Event handler for the Play Audio button click event</summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Parameters for the event</param>
        protected virtual void btnPlayAudio_Click(Object sender, EventArgs e)
        {
            if (this.lstboxFiles.SelectedItem != null)
            {
                this.PostMessage(this, new LogEventArgs(new LogItem(LogType.Informational, String.Format("Testing started: {0}", DateTime.Now.ToShortTimeString()), "Testing", "Started", this)));
                String item = this.lstboxFiles.SelectedItem as String;
                Int32 streamNo = Int32.Parse(this.cboStream.SelectedItem as String);
                Thread thread = new Thread(() => this.RunTestThread(item, streamNo));
                thread.Name = "Test thread";
                thread.Start();
            }
        }
        #endregion


        /// <summary>Void method to raise the testing in a separate thread</summary>
        protected virtual void RunTestThread(String item, Int32 index)
        {
            this.Harness.DoTest(this, new AudioTestEventArgs(item, index));
            this.PostMessage(this, new LogEventArgs(new LogItem(LogType.Informational, String.Format("Testing ended: {0}", DateTime.Now.ToShortTimeString()), "Testing", "Ended", this)));
        }

        /// <summary>Method that will post a message to the log collection object. Intended to be attached to an event that the Harness will raise internally.</summary>
        /// <param name="sender">Object sending the message</param>
        /// <param name="message">Log Message being posted</param>
        protected virtual void PostMessage(Object sender, LogEventArgs message)
        {
            this.logOutput.PostMessage(message);
        }

        /// <summary>Method called after the initialize event is finished processing</summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Parameters to the event</param>
        protected virtual void EndHarnessInitialize(Object sender, EventArgs e)
        {
            this.PostMessage(this, new LogEventArgs(new LogItem(LogType.Informational, String.Format("Initialization ended: {0}", DateTime.Now.ToShortTimeString()), "Initialization", "Ended", this)));
        }
    }
}