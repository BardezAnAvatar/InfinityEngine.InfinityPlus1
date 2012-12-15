using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Bardez.Projects.BasicStructures.Win32.Audio;
using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Multimedia;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.InfinityPlus1.Output.Audio;
using Bardez.Projects.ReusableCode;


namespace Bardez.Projects.InfinityPlus1.Test.WinForm.Movie
{
    /// <summary>User control for testing generic LibAV movie audio and video playback</summary>
    public partial class LibAVMoviePlaybackTestControl : UserControl
    {
        #region Constants
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.LibAV.Movie.Path";
        #endregion


        #region Fields
        /// <summary>Object reference to lock on for the User Interface</summary>
        private Object interfaceLock;

        /// <summary>Object reference to lock around the decoding of an MVE selected</summary>
        /// <remarks>Since it is protected, be very careful about what it does...</remarks>
        protected Object abortLock;

        /// <summary>Locking object for the frame key</summary>
        private Object frameKeyLock;

        /// <summary>Thread-accessible variable indicating the current movie path</summary>
        protected volatile String currentMoviePath;

        /// <summary>Reference to the thread that will be decoding the MVE</summary>
        protected volatile Thread decodingThread;

        /// <summary>Flag indicating whether audio is currently playing</summary>
        protected volatile Boolean IsPlayingAudio;

        /// <summary>Stream renderer for video stream</summary>
        protected VideoStreamRenderManager VideoRenderManager { get; set; }

        /// <summary>Stream renderer for audio stream</summary>
        protected AudioStreamRenderManager AudioRenderManager { get; set; }

        /// <summary>Acceses the multimedia playback controller</summary>
        protected MultimediaMovie MultimediaController { get; set; }

        /// <summary>Represents the Direct2D controller unique key for the currently displayed video frame</summary>
        protected Int32 frameKey { get; set; }

        /// <summary>Datetime used to roughly measure playback FPS</summary>
        protected DateTime previousTime { get; set; }

        /// <summary>Frame number currently displayed</summary>
        protected Int32 frameCountNumber { get; set; }

        /// <summary>Reference to an AudioOutput instance</summary>
        protected AudioOutput AudioOutputController { get; set; }

        /// <summary>Unique key context for the output source</summary>
        protected Int32 AudioOutputSoundSourceKey { get; set; }

        /// <summary>Unique key context for the output destination</summary>
        protected Int32 AudioOutputSoundRenderingKey { get; set; }

        /// <summary>Audio stream index to play back</summary>
        protected Int32 AudioStream { get; set; }

        /// <summary>Audio stream index to play back</summary>
        protected Int32 AudioBlockIndex { get; set; }

        /// <summary>Flag indicating whether or not to render audio data</summary>
        protected Boolean RenderAudio { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the configuration key to use to pull from the app.config or similar source</summary>
        protected String ConfigKey
        {
            get { return LibAVMoviePlaybackTestControl.configKey; }
        }

        /// <summary>Rendering control background color</summary>
        protected virtual Color RenderBackgroundColor
        {
            get { return this.direct2dRenderControl.BackColor; }
            set { this.direct2dRenderControl.BackColor = value; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public LibAVMoviePlaybackTestControl()
        {
            this.InitializeComponent();
            
            this.interfaceLock = new Object();
            this.abortLock = new Object();
            this.frameKeyLock = new Object();

            this.frameKey = -1; //default of 0 will free the first frame sent in
            this.previousTime = DateTime.MinValue;
            this.frameCountNumber = 0;

            this.RenderAudio = true;
            this.IsPlayingAudio = false;
            this.AudioOutputSoundSourceKey = -1;
        }

        /// <summary>Initializes the XAudio2 playback</summary>
        protected void InitializeAudio()
        {
            //get an instance for audio output
            if (this.AudioOutputController == null)
                this.AudioOutputController = XAudio2Output.Instance;    //could be OpenAL or whatever

            //dispose of the current output context
            if (this.AudioOutputSoundSourceKey > -1 && this.AudioOutputController != null)
            {
                this.AudioOutputController.DisposePlayback(this.AudioOutputSoundSourceKey);
                this.AudioOutputSoundSourceKey = -1;
            }

            //create a new one
            if (this.AudioRenderManager != null)
            {
                //get a reference to the wave format data
                WaveFormatEx audioFormat = this.AudioRenderManager.GetWaveFormat();

                //create renderer
                this.AudioOutputSoundRenderingKey = this.AudioOutputController.GetDefaultRenderer();

                //create source
                this.AudioOutputSoundSourceKey = this.AudioOutputController.CreatePlayback(audioFormat, this.AudioOutputSoundRenderingKey);

                //Adjust callback(s)
                if (this.AudioOutputController.HasSourceNeedsDataEventHandler(this.AudioOutputSoundSourceKey))
                    this.AudioOutputController.AddSourceNeedsDataEventHandler(this.AudioOutputSoundSourceKey, new Action(this.NeedsMoreAudioSamples));
            }
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
                if (this.VideoRenderManager != null)
                {
                    this.VideoRenderManager.AttemptRender(TimeSpan.MaxValue);

                    //set the controls
                    this.ToggleResetVideo(true);
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
                this.StartPlayback();
        }

        /// <summary>Resets the state of the video back to the start of the video</summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Even arguments</param>
        protected virtual void btnResetVideo_Click(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                if (this.MultimediaController != null)
                {
                    this.MultimediaController.ResetVideo();
                    this.previousTime = DateTime.MinValue;
                    this.frameCountNumber = 0;
                    this.lblFrameNumberDisplay.Text = "n/a";
                }
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

        /// <summary>Event handler for when the selected index of the listbox changes. Sends a new index to the decoding thread.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void lstboxFiles_SelectedIndexChanged(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                String moviePath = this.lstboxFiles.SelectedItem as String;

                //disable movie controls
                this.ToggleMovieControlAvailability(false);

                //once one is selected, set the visibility of video controls accordingly and start decoding
                if (moviePath == null)
                {
                    //stop playback
                    this.pnlVideoControls.Visible = false;
                }
                else
                {
                    this.pnlVideoControls.Visible = true;

                    if (this.currentMoviePath != moviePath)
                    {
                        this.frameCountNumber = 0;
                        this.lblFrameNumberDisplay.Text = "n/a";

                        lock (this.abortLock)
                            this.currentMoviePath = moviePath;

                        //kick off a thread to decode the video file
                        ThreadPool.QueueUserWorkItem(new WaitCallback((Object o) => { this.DecodeSingleMovie(moviePath); }));
                    }
                }
            }
        }

        /// <summary>Event handler for when the "Render Audio" checkbox changes</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void chkbxRenderAudio_CheckedChanged(Object sender, EventArgs e)
        {
            this.RenderAudio = this.chkbxRenderAudio.Checked;
        }
        #endregion


        #region Movie decoding
        /// <summary>
        ///     Method to decode a single movie file, in a multi-threaded environment. It will abort decoding if a change in
        ///     the selected movie occurs.
        /// </summary>
        /// <param name="filePath">String of a filepath, casted as an Object for use by ThreadPool</param>
        protected void DecodeSingleMovie(String filePath)
        {
            lock (this.abortLock)
            {
                //periodically check whether the process is the valid one.
                //Only assign and decode at the end, when you are fairly sure this is the correct MVE.
                Boolean continueProcess = (filePath == this.currentMoviePath);

                Boolean assignedManagerToField = false;
                MultimediaMovie movie = null;

                if (continueProcess = (filePath == this.currentMoviePath))
                {
                    //stop the existing manager. This method should not be invoked if already on the correct file.
                    if (this.MultimediaController != null)
                    {
                        this.StopPlayback();

                        //dispose and deallocate, hopefully
                        this.MultimediaController.Dispose();
                        this.MultimediaController = null;
                        this.AudioRenderManager = null;
                        this.VideoRenderManager = null;
                    }

                    //Read chunks
                    movie = new MultimediaMovie();
                    if (continueProcess = (filePath == this.currentMoviePath))
                    {
                        movie.Open(filePath);

                        //assign the video controller
                        if (continueProcess = (filePath == this.currentMoviePath))
                        {
                            this.MultimediaController = movie;
                            assignedManagerToField = true;

                            //attach event listeners
                            if (continueProcess = (filePath == this.currentMoviePath))
                            {
                                //attach events to the appropriate streams
                                this.SetRenderStreams();

                                //start decoding
                                if (continueProcess = (filePath == this.currentMoviePath))
                                {
                                    //start atual ecoding
                                    movie.StartDecoding();

                                    this.ToggleNextFrame(true);
                                    this.ToggleStartPlayback(true);
                                }
                            }
                        }
                    }
                }

                //clean up if we are aborting
                if (!continueProcess)
                {
                    if (movie != null)
                    {
                        movie.Dispose();
                        movie = null;
                    }

                    if (assignedManagerToField)
                        this.MultimediaController = null;
                }
            }
        }
        #endregion


        #region Video display
        /// <summary>Processes the next video frame from the controller. Stops playback if the frame returned was null.</summary>
        protected virtual void ProcessNextVideoFrame(Frame frame)
        {
            lock (this.frameKeyLock)
            {
                if (this.MultimediaController != null)
                {
                    if (frame == null)
                        this.StopPlayback();
                    else
                    {
                        //get the new one
                        this.frameKey = this.direct2dRenderControl.AddFrameResource(frame);

                        //render
                        this.direct2dRenderControl.SetRenderFrameAndRender(this.frameKey, true);

                        //display
                        DateTime now = DateTime.Now;
                        if (this.previousTime != DateTime.MinValue)
                        {
                            TimeSpan ts = now - this.previousTime;
                            Double fps = 1000.0 / ts.TotalMilliseconds;

                            this.ControlAction(this.lblFrameRateDisplay, () => { this.lblFrameRateDisplay.Text = String.Format("{0} fps", fps); });
                            this.ControlAction(this.lblFrameNumberDisplay, () => { this.lblFrameNumberDisplay.Text = this.frameCountNumber.ToString(); });
                        }

                        this.previousTime = now;
                        this.frameCountNumber++;
                    }
                }
            }
        }

        /// <summary>Pauses video (and possibly audio) playback</summary>
        protected virtual void PausePlayback()
        {
            if (this.MultimediaController != null)
            {
                this.MultimediaController.StopPlayback();
                this.MultimediaController.ClearTimerElapsed();
            }
        }

        /// <summary>Ceases video (and possibly audio) playback</summary>
        protected virtual void StopPlayback()
        {
            this.PausePlayback();
            this.previousTime = DateTime.MinValue;

            //set the controls
            this.ToggleStartPlayback(true);
            this.ToggleNextFrame(true);
            this.ToggleStopPlayback(false);
        }

        /// <summary>Starts multimedia playback</summary>
        protected virtual void StartPlayback()
        {
            if (this.MultimediaController != null)
            {
                this.MultimediaController.StartPlayback();

                //set the controls
                this.ToggleStartPlayback(false);
                this.ToggleNextFrame(false);
                this.ToggleResetVideo(true);
                this.ToggleStopPlayback(true);
            }
        }
        #endregion


        #region Audio Playback
        /// <summary>Plays back an audio packet, adding it to the output controller's source</summary>
        /// <param name="renderTime">Useless parameter?</param>
        /// <param name="data">Audio data to output</param>
        public void ProcessAudioPacket(TimeSpan renderTime, Byte[] data)
        {
            if (this.AudioOutputController != null)
            {
                if (this.AudioOutputController.CanSubmitBuffer(this.AudioOutputSoundSourceKey))
                    this.AudioOutputController.SubmitData(data, this.AudioOutputSoundSourceKey, true); //true = leave open for subsequent submissions
            }
        }

        public void NeedsMoreAudioSamples()
        {
            this.AudioRenderManager.AttemptRender(TimeSpan.MaxValue);
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


        #region Lib AV multimedia stream selection
        /// <summary>Sets the appropriate rendering streams for the current LibAV multimedia format</summary>
        protected void SetRenderStreams()
        {
            this.VideoRenderManager = this.MultimediaController.GetBestVideoStream();
            this.VideoRenderManager.Render += this.ProcessNextVideoFrame;
            this.VideoRenderManager.Process = true;

            //go off of selection
            this.ControlAction(this.cboAudioStream, () => this.SetSelectedAudioStream());
            // ... or pick 'the best' one
            //this.AudioRenderManager = this.VideoController.GetBestAudioStream();

            this.AudioRenderManager.Process = true;
            this.InitializeAudio();
            this.AudioRenderManager.Render += this.ProcessAudioPacket;
        }

        /// <summary>Sets the selected audio stream</summary>
        /// <param name="streamNumber">Number of the stream to set</param>
        protected void SetSelectedAudioStream(Int32 streamNumber)
        {
            this.AudioRenderManager = this.MultimediaController.AudioStreamRenderers[streamNumber];
        }

        /// <summary>Sets the selected audio stream</summary>
        protected void SetSelectedAudioStream()
        {
            Int32 index = 0;

            if (this.cboAudioStream.SelectedItem != null)
                index = this.cboAudioStream.SelectedIndex;

            this.SetSelectedAudioStream(index);
        }
        #endregion


        #region Control availability
        /// <summary>Toggles the availability of all the movie playback controls</summary>
        /// <param name="available">Flag indicating whether they should be available</param>
        protected void ToggleMovieControlAvailability(Boolean available)
        {
            this.ControlAction(this.btnResetVideo, () => { this.btnResetVideo.Enabled = available; });
            this.ControlAction(this.btnNextFrame, () => { this.btnNextFrame.Enabled = available; });
            this.ControlAction(this.btnStartPlayback, () => { this.btnStartPlayback.Enabled = available; });
            this.ControlAction(this.btnStopPlayback, () => { this.btnStopPlayback.Enabled = available; });
        }

        /// <summary>Toggles the availability of the stop playback control</summary>
        /// <param name="available">Flag indicating whether the control should be available</param>
        protected void ToggleStopPlayback(Boolean available)
        {
            this.ControlAction(this.btnStopPlayback, () => { this.btnStopPlayback.Enabled = available; });
        }

        /// <summary>Toggles the availability of the reset video playback control</summary>
        /// <param name="available">Flag indicating whether the control should be available</param>
        protected void ToggleResetVideo(Boolean available)
        {
            this.ControlAction(this.btnResetVideo, () => { this.btnResetVideo.Enabled = available; });
        }

        /// <summary>Toggles the availability of the next frame playback control</summary>
        /// <param name="available">Flag indicating whether the control should be available</param>
        protected void ToggleNextFrame(Boolean available)
        {
            this.ControlAction(this.btnNextFrame, () => { this.btnNextFrame.Enabled = available; });
        }

        /// <summary>Toggles the availability of the start playback control</summary>
        /// <param name="available">Flag indicating whether the control should be available</param>
        protected void ToggleStartPlayback(Boolean available)
        {
            this.ControlAction(this.btnStartPlayback, () => { this.btnStartPlayback.Enabled = available; });
        }
        #endregion
    }
}