using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Bardez.Projects.BasicStructures.Win32.Audio;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management;
using Bardez.Projects.InfinityPlus1.NativeFactories.Timer;
using Bardez.Projects.InfinityPlus1.Output.Audio;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.MVE
{
    /// <summary>User control for testing the MVE audio and video playback</summary>
    public class MveMoviePlaybackTestControl : HarnessMovieTestControl
    {
        #region Constants
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.MVE.Path";
        #endregion


        #region Controls
        /// <summary>Label for the Audio Stream dropdown box</summary>
        protected Label lblStream;

        /// <summary>Audio stream playback dropdown box</summary>
        protected ComboBox cboAudioStream;
        #endregion


        #region Fields
        /// <summary>Reference to XAudio2 object</summary>
        protected XAudio2Output AudioOutput { get; set; }

        /// <summary>Unique key context for the output source</summary>
        protected Int32 AudioOutputSoundSourceKey { get; set; }

        /// <summary>Unique key context for the output rendering destination</summary>
        protected Int32 AudioOutputSoundRenderingKey { get; set; }

        /// <summary>Audio stream index to play back</summary>
        protected Int32 AudioStream { get; set; }

        /// <summary>Audio stream index to play back</summary>
        protected Int32 AudioBlockIndex { get; set; }

        /// <summary>Flag indicating whether audio is currently playing</summary>
        protected volatile Boolean IsPlayingAudio;
        #endregion


        #region Properties
        /// <summary>Exposes the configuration key to use to pull from the app.config or similar source</summary>
        protected override String ConfigKey
        {
            get { return MveMoviePlaybackTestControl.configKey; }
        }

        /// <summary>Exposes the Video Controller as an MveManager</summary>
        protected MveManager VideoControllerMVE
        {
            get { return this.VideoController as MveManager; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public MveMoviePlaybackTestControl() : base()
        {
            this.IsPlayingAudio = false;
        }

        /// <summary>Overridden Initialize Component for added controls</summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.MveAudioAddedInitializeComponent();
        }

        /// <summary>Added Initialize Component logic for added controls</summary>
        protected virtual void MveAudioAddedInitializeComponent()
        {
            this.lblStream = new Label();
            this.cboAudioStream = new ComboBox();

            this.pnlTest.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblStream
            // 
            this.lblStream.AutoSize = true;
            this.lblStream.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStream.Location = new System.Drawing.Point(0, 379);
            this.lblStream.Name = "lblStream";
            this.lblStream.Size = new System.Drawing.Size(113, 13);
            this.lblStream.TabIndex = 64;
            this.lblStream.Text = "Audio Stream Number:";
            // 
            // cboStream
            // 
            this.cboAudioStream.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cboAudioStream.FormattingEnabled = true;
            this.cboAudioStream.Items.AddRange(new Object[] {
                "0",
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9",
                "10",
                "11",
                "12",
                "13",
                "14",
                "15"});
            this.cboAudioStream.Location = new System.Drawing.Point(0, 392);
            this.cboAudioStream.Name = "cboAudioStream";
            this.cboAudioStream.Size = new System.Drawing.Size(227, 21);
            this.cboAudioStream.TabIndex = 63;

            //Attach controls
            this.splitContainerHarnessResults.Panel1.Controls.Add(this.lblStream);
            this.splitContainerHarnessResults.Panel1.Controls.Add(this.cboAudioStream);

            this.pnlTest.ResumeLayout(false);
            this.pnlTest.PerformLayout();
            this.ResumeLayout(false);
        }
        #endregion


        #region Event handlers
        /// <summary>Overridden Click event handler for the Initialize button. Loads a list of displayable movies from the config file.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected override void btnInitialize_Click(Object sender, EventArgs e)
        {
            base.btnInitialize_Click(sender, e);

            this.AudioOutput = XAudio2Output.Instance;
            this.cboAudioStream.SelectedIndex = 0;   //select the first index, for precaution
        }
        #endregion


        #region Movie decoding
        /// <summary>
        ///     Method to decode a single movie file, in a multi-threaded environment. It will abort decoding if a change in
        ///     the selected movie occurs.
        /// </summary>
        /// <param name="filePath">String of a filepath, casted as an Object for use by ThreadPool</param>
        protected override void DecodeSingleMovie(String filePath)
        {
            lock (this.abortLock)
            {
                //periodically check whether the process is the valid one.
                //Only assign and decode at the end, when you are fairly sure this is the correct MVE.
                Boolean continueProcess = (filePath == this.currentMoviePath);

                Boolean assignedManagerToField = false;
                
                FileStream sourceFile = null;    //soource file to read from

                try
                {
                    MveManager manager = null;
                    MveChunkOpcodeIndexer mve = null;

                    if (continueProcess = (filePath == this.currentMoviePath))
                    {
                        //stop the existing manager. This method should not be invoked if already on the correct file.
                        if (this.VideoController != null)
                        {
                            this.StopAudioPlayback();
                            this.VideoController.StopVideoPlayback();
                        }

                        //open the file
                        sourceFile = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                        //Read chunks
                        mve = new MveChunkOpcodeIndexer();
                        if (continueProcess = (filePath == this.currentMoviePath))
                        {
                            mve.Read(sourceFile);

                            //read opcodes
                            if (continueProcess = (filePath == this.currentMoviePath))
                            {
                                mve.ReadChunkOpcodes(sourceFile);

                                //collect opcode data
                                if (continueProcess = (filePath == this.currentMoviePath))
                                {
                                    manager = new MveManager(mve, TimerFactory.BuildTimer());
                                    manager.CollectOpcodeIndex();

                                    //read audio data
                                    if (continueProcess = (filePath == this.currentMoviePath))
                                    {
                                        manager.ReadData(sourceFile);

                                        //close the file since no longer needed
                                        sourceFile.Dispose();
                                        sourceFile = null;

                                        //decode video maps
                                        if (continueProcess = (filePath == this.currentMoviePath))
                                        {
                                            manager.DecodeVideoMaps();

                                            //Initialize video coders
                                            if (continueProcess = (filePath == this.currentMoviePath))
                                            {
                                                manager.InitializeCoders();

                                                //attach audio event listeners
                                                if (continueProcess = (filePath == this.currentMoviePath))
                                                {
                                                    manager.AudioStreamStarted += () => { this.ControlAction(this.lstboxFiles, this.StartAudioPlayback); };
                                                    manager.AudioStreamStopped += this.StopAudioPlayback;
                    
                                                    //start decoding
                                                    if (continueProcess = (filePath == this.currentMoviePath))
                                                    {
                                                        manager.PreemptivelyStartDecodingAudio();
                                                        manager.PreemptivelyStartDecodingVideo();

                                                        //assign the video controller
                                                        if (continueProcess = (filePath == this.currentMoviePath))
                                                        {
                                                            this.VideoController = manager;
                                                            assignedManagerToField = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //clean up if we are aborting
                    if (!continueProcess)
                    {
                        if (manager != null)
                        {
                            manager.Dispose();
                            manager = null;
                        }

                        if (assignedManagerToField)
                            this.VideoController = null;
                    }
                }
                finally
                {
                    //close the file if it is still open
                    if (sourceFile != null)
                        sourceFile.Dispose();
                }
            }
        }
        #endregion


        #region Audio playback
        /// <summary>Callback event handler that sends more data to the output buffer</summary>
        public virtual void NeedsMoreSamples()
        {
            Byte[] samples = this.VideoController.GetAudioBlock(this.AudioBlockIndex, this.AudioStream);
            if (samples == null)
            {
                this.StopAudioPlayback();
                this.AudioBlockIndex = 0;
            }
            else
            {
                ++this.AudioBlockIndex;

                if (!this.RenderAudio)
                    samples = new Byte[samples.Length]; //HACK: create a new, blank audio array

                //render audio
                this.AudioOutput.SubmitData(samples, this.AudioOutputSoundSourceKey, this.AudioBlockIndex < this.VideoControllerMVE.AudioBlockCount(this.AudioStream), false);
            }
        }
        #endregion


        #region Audio Control
        /// <summary>Halts audio playback</summary>
        protected override void StopAudioPlayback()
        {
            this.AudioOutput.HaltPlayback();
            this.IsPlayingAudio = false;
        }

        /// <summary>Starts audio playback</summary>
        protected override void StartAudioPlayback()
        {
            if (!this.IsPlayingAudio)
            {
                this.IsPlayingAudio = true;

                this.AudioStream = Int32.Parse(this.cboAudioStream.SelectedItem as String);
                this.AudioBlockIndex = 0;

                //use this twice, so keep the reference
                WaveFormatEx audioFormat = this.VideoController.GetWaveFormat();

                //keep track of the output rendering reference
                this.AudioOutputSoundRenderingKey = AudioOutput.GetDefaultRenderer();

                //load up the initial Source voice
                this.AudioOutputSoundSourceKey = AudioOutput.CreatePlayback(audioFormat, this.AudioOutputSoundRenderingKey);

                //Adjust callback(s)
                this.AudioOutput.AddSourceNeedsDataEventHandler(this.AudioOutputSoundSourceKey, new Action(this.NeedsMoreSamples));

                //submit first data
                this.NeedsMoreSamples();

                //play audio & Let the sound play
                this.AudioOutput.StartPlayback(this.AudioOutputSoundSourceKey);
            }
        }
        #endregion
    }
}