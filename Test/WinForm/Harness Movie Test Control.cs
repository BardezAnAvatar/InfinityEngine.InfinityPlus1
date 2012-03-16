using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm
{
    /// <summary>Represents a base class for UI harnesses that display images</summary>
    public abstract partial class HarnessMovieTestControl<MovieFormat> : UserControl where MovieFormat : IMovie
    {
        #region Fields
        /// <summary>Object reference to lock on for the User Interface</summary>
        private Object interfaceLock;

        /// <summary>Object reference to abort decoding of</summary>
        /// <remarks>Since it is protected, be very careful about what it does...</remarks>
        protected Object abortLock;

        /// <summary>Locking object for the frame key</summary>
        private Object frameKeyLock;

        /// <summary>Acceses the video playback controller</summary>
        protected MovieFormat VideoController { get; set; }

        /// <summary>Represents the Direct2D controller unique key for the currently displayed video frame</summary>
        protected Int32 frameKey { get; set; }

        /// <summary>Thread-accessible variable indicating the current movie path</summary>
        protected String currentMoviePath { get; set; }

        /// <summary>Flag indicating whether or not to render audio data</summary>
        protected Boolean RenderAudio { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the configuration key to use to pull from the app.config or similar source</summary>
        protected abstract String ConfigKey { get; }

        /// <summary>Rendering control background color</summary>
        protected virtual Color RenderBackgroundColor
        {
            get { return this.direct2dRenderControl.BackColor; }
            set { this.direct2dRenderControl.BackColor = value; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public HarnessMovieTestControl()
        {
            this.InitializeComponent();
            
            this.interfaceLock = new Object();
            this.abortLock = new Object();
            this.frameKeyLock = new Object();

            this.frameKey = -1; //default of 0 will free the first frame sent in
            this.RenderAudio = true;
        }
        #endregion


        #region Event handlers
        /// <summary>Click event handler for the Initialize button. Loads a list of displayable bitmaps from the config file.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void btnInitialize_Click(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                if (this.lstboxFiles.Items.Count < 1)
                {
                    //load paths from the app.config
                    List<String> paths = ConfigurationHandlerMulti.GetSettingValues(this.ConfigKey);

                    foreach (String path in paths)
                    {
                        //add it to the collection
                        this.lstboxFiles.Items.Add(path);
                    }
                }
            }
        }

        /// <summary>Click event hander for the Clear Display button. Clears the rendering control of its contents.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void btnClearDisplay_Click(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                this.direct2dRenderControl.SetRenderFrameAndRender(-1, true);

                this.frameKey = -1;
            }
        }

        /// <summary>Displays the next frame decoded from the video manager</summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Even arguments</param>
        protected virtual void btnNextFrame_Click(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                if (this.VideoController != null)
                    this.FetchNextVideoFrame();
            }
        }

        /// <summary>Stops playback of the video</summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Even arguments</param>
        protected virtual void btnStopPlayback_Click(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
                this.StopPlayback();
        }

        /// <summary>Starts playback of the video</summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Even arguments</param>
        protected virtual void btnStartPlayback_Click(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                while (this.VideoController == null)
                    Thread.Sleep(100);

                this.StartPlayback();
            }
        }

        /// <summary>Resets the state of the video back to the start of the video</summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Even arguments</param>
        protected virtual void btnResetVideo_Click(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                if (this.VideoController != null)
                    this.VideoController.ResetVideo();
            }
        }

        /// <summary>Displays a color picker for the background of the rendering control</summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Even arguments</param>
        protected virtual void btnChooseColor_Click(Object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.AllowFullOpen = true;
            cd.AnyColor = true;
            cd.ShowDialog();
            this.RenderBackgroundColor = cd.Color;
        }

        /// <summary>Event handler for when the "Render Audio" checkbox changes</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void chkbxRenderAudio_CheckedChanged(Object sender, EventArgs e)
        {
            this.RenderAudio = this.chkbxRenderAudio.Checked;
        }

        /// <summary>Event handler for when the selected index of the listbox changes. Sends a new bitmap index to the render target control.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void lstboxImages_SelectedIndexChanged(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                String moviePath = this.lstboxFiles.SelectedItem as String;

                //once one is selected, set the visibility of video controls accordingly and start decoding
                if (moviePath == null)
                {
                    //stop playback
                    this.pnlVideoControls.Visible = false;
                }
                else
                {
                    this.pnlVideoControls.Visible = true;

                    lock (this.abortLock)
                        this.currentMoviePath = moviePath;
                    
                    //kick off a thread to decode the video file
                    ThreadPool.QueueUserWorkItem(new WaitCallback((Object o) => { this.DecodeSingleMovie(moviePath); }));
                }
            }
        }
        #endregion


        #region Movie decoding
        /// <summary>
        ///     Method to decode a single movie file, in a multi-threaded environment. It will abort decoding if a change in
        ///     the selected movie occurs.
        /// </summary>
        /// <param name="filePath">String of a filepath, casted as an Object for use by ThreadPool</param>
        protected abstract void DecodeSingleMovie(String filePath);
        #endregion


        #region Video display
        /// <summary>Fetches the nex video frame from the controller. Stops playback if the frame returned was null.</summary>
        protected virtual void FetchNextVideoFrame()
        {
            if (this.VideoController != null)
            {
                Frame frame = this.VideoController.GetNextFrame();

                if (frame == null)
                    this.StopPlayback();
                else
                {
                    lock (this.frameKeyLock)
                    {
                        //get the new one
                        this.frameKey = this.direct2dRenderControl.AddFrameResource(frame);

                        //render
                        this.direct2dRenderControl.SetRenderFrameAndRender(this.frameKey, true);
                    }
                }
            }
        }

        /// <summary>Ceases video (and possibly audio) playback</summary>
        protected virtual void StopPlayback()
        {
            if (this.VideoController != null)
            {
                this.VideoController.StopVideoPlayback();
                this.VideoController.ClearTimerElapsed();
                this.StopAudioPlayback();
            }
        }

        /// <summary>Ceases video and audio playback</summary>
        protected virtual void StartPlayback()
        {
            if (this.VideoController != null)
            {
                this.VideoController.TimerElapsed += new Action(this.FetchNextVideoFrame);
                this.VideoController.StartVideoPlayback();
                this.StartAudioPlayback();
            }
        }
        #endregion


        #region Threading helpers
        /// <summary>Generic control action that will perform an Invoke action (such as a setter) on a control</summary>
        /// <param name="c">Control to query for Invoke</param>
        /// <param name="action">Action to perform</param>
        protected virtual void ControlAction(Control c, Action action)
        {
            if (c.InvokeRequired)
                c.Invoke(action);
            else
                action();
        }
        #endregion


        #region Audio Control
        /// <summary>Halts audio playback</summary>
        protected abstract void StopAudioPlayback();

        /// <summary>Starts audio playback</summary>
        protected abstract void StartAudioPlayback();
        #endregion
    }
}