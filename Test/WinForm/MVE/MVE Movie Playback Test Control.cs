using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management;
using Bardez.Projects.InfinityPlus1.Output.Audio;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.MVE
{
    /// <summary>User control for testing the MVE audio and video playback</summary>
    public class MveMoviePlaybackTestControl : HarnessMovieTestControl<MveManager>
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
        protected XAudio2Output Output { get; set; }

        /// <summary>Unique key context for the output source</summary>
        protected Int32 OutputSoundKey { get; set; }

        /// <summary>Audio stream index to play back</summary>
        protected Int32 AudioStream { get; set; }

        /// <summary>Audio stream index to play back</summary>
        protected Int32 AudioBlockIndex { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the configuration key to use to pull from the app.config or similar source</summary>
        protected override String ConfigKey
        {
            get { return MveMoviePlaybackTestControl.configKey; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public MveMoviePlaybackTestControl() : base() { }

        /// <summary>Overridden Initialize Component for added controls</summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.AddedInitializeComponent();
        }

        /// <summary>Added Initialize Component logic for added controls</summary>
        protected virtual void AddedInitializeComponent()
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

            this.Output = XAudio2Output.Instance;
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
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                //Read chunks
                MveChunkOpcodeIndexer mve = new MveChunkOpcodeIndexer();

                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                        mve.Read(fs);
                    else
                        goto BreakPoint;
                }

                //read opcodes
                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                        mve.ReadChunkOpcodes(fs);
                    else
                        goto BreakPoint;
                }

                MveManager manager = new MveManager(mve);

                //collect opcode data
                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                        manager.CollectOpcodeIndex();
                    else
                        goto BreakPoint;
                }

                //read audio data
                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                        manager.ReadData(fs);
                    else
                        goto BreakPoint;
                }

                //decode video maps
                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                        manager.DecodeVideoMaps();
                    else
                        goto BreakPoint;
                }

                //Initialize video coders
                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                        manager.InitializeCoders();
                    else
                        goto BreakPoint;
                }

                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                    {
                        if (this.VideoController != null)
                        {
                            this.StopAudioPlayback();
                            this.VideoController.StopVideoPlayback();
                        }

                        this.VideoController = manager;
                    }
                    else
                        goto BreakPoint;
                }

                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                    {
                        if (this.VideoController != null)
                            this.VideoController.StartDecodingAudio();
                    }
                    else
                        goto BreakPoint;
                }

            BreakPoint:
                ;
            }
        }
        #endregion


        #region Audio playback
        /// <summary>Callback event handler that sends more data to the output buffer</summary>
        public virtual void NeedsMoreSamples()
        {
            Byte[] samples = this.VideoController.GetAudioBlock(this.AudioBlockIndex, this.AudioStream);
            ++this.AudioBlockIndex;

            if (this.RenderAudio)       //render audio
                this.Output.SubmitSubsequentData(samples, this.OutputSoundKey, this.AudioBlockIndex < this.VideoController.AudioBlockCount(this.AudioStream));
            else
            {
                samples = new Byte[samples.Length]; //HACK: create a new, blank audio array
                this.Output.SubmitSubsequentData(samples, this.OutputSoundKey, this.AudioBlockIndex < this.VideoController.AudioBlockCount(this.AudioStream));
            }
        }
        #endregion


        #region Audio Control
        /// <summary>Halts audio playback</summary>
        protected override void StopAudioPlayback()
        {
            this.Output.HaltPlayback();
        }

        /// <summary>Starts audio playback</summary>
        protected override void StartAudioPlayback()
        {
            if (this.lstboxFiles.SelectedItem != null)
            {
                String item = this.lstboxFiles.SelectedItem as String;
                this.AudioStream = Int32.Parse(this.cboAudioStream.SelectedItem as String);
                this.AudioBlockIndex = 0;

                //load up the initial Source voice
                this.OutputSoundKey = Output.CreatePlayback(this.VideoController.GetWaveFormat());

                //Adjust callback(s)
                this.Output.AddSourceNeedDataEventhandler(this.OutputSoundKey, new AudioNeedsMoreDataHandler(this.NeedsMoreSamples));
                
                //submit first data
                this.NeedsMoreSamples();

                //play audio & Let the sound play
                this.Output.StartPlayback(this.OutputSoundKey);
            }
        }
        #endregion
    }
}