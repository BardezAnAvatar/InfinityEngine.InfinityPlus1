using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Bardez.Projects.Configuration;
using Bardez.Projects.DirectX.XAudio2;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Music;
using Bardez.Projects.InfinityPlus1.Output.Audio;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.Music
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Infinity.Music.Playlist class.</summary>
    public class PlaylistTest : FileTesterBase
    {
        #region Fields
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.MUS.Path";

        /// <summary>Reference to XAudio2 object</summary>
        protected XAudio2Output Output { get; set; }

        /// <summary>Format instance to test</summary>
        protected Playlist playlist { get; set; }

        /// <summary>Unique key context for the output source</summary>
        protected Int32 OutputSoundKey;
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public PlaylistTest()
        {
            this.InitializeInstance();
        }
        #endregion

        /// <summary>Initializes the test class data</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="e">Specific initialization event parameters</param>
        protected override void InitializeTestData(Object sender, EventArgs e)
        {
            this.Output = XAudio2Output.Instance;
            this.FilePaths = ConfigurationHandlerMulti.GetSettingValues(PlaylistTest.configKey);
        }

        /// <summary>Event to raise for testing instance(s)</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            using (FileStream stream = new FileStream(testArgs.Path, FileMode.Open, FileAccess.Read))
            {
                this.playlist = new Playlist();
                this.DoPostMessage(new MessageEventArgs("Reading playlist...", "Reading", testArgs.Path));
                this.playlist.Read(stream);
                this.DoPostMessage(new MessageEventArgs(this.playlist.ToString(), "Output", testArgs.Path));

                this.playlist.RootFilePath = new FileInfo(testArgs.Path).DirectoryName;
                this.TestPlayback();
            }

            using (FileStream dest = new FileStream(testArgs.Path + ".rewrite", FileMode.Create, FileAccess.Write))
                this.TestWrite(dest);
        }

        /// <summary>Writes the data structure back out to a destination stream</summary>
        /// <param name="destination">Stream to write output to</param>
        protected virtual void TestWrite(Stream destination)
        {
            this.playlist.Write(destination);
        }

        #region Helper methods
        /// <summary>Tests audio playback, allowing for an interrupt to occur by hitting enter.</summary>
        public void TestPlayback()
        {
            this.DoPostMessage(new MessageEventArgs("Decoding playlist files...", "Decoding", this.playlist.PlaylistName));
            this.playlist.ReadPlayListItems();  //tell it to load everything
            this.DoPostMessage(new MessageEventArgs("Finished decoding...", "Finished decoding", this.playlist.PlaylistName));

            this.Output = XAudio2Output.Instance;

            //load up the initial Source voice
            this.OutputSoundKey = Output.CreatePlayback(this.playlist.WaveFormat);

            //Adjust callback(s)
            this.Output.AddSourceNeedDataEventhandler(this.OutputSoundKey, new AudioNeedsMoreDataHandler(this.NeedsMoreSamples));

            //submit first data
            this.NeedsMoreSamples();

            //play audio & Let the sound play
            this.Output.StartPlayback(this.OutputSoundKey);

            this.DoPostMessage(new MessageEventArgs("Waiting for playlist interrupt...", "Waiting", this.playlist.PlaylistName));
        }
        
        /// <summary>Callback event handler that sends more data to the output buffer</summary>
        public void NeedsMoreSamples()
        {
            Byte[] samples = this.playlist.GetNext();
            this.Output.SubmitSubsequentData(samples, this.OutputSoundKey, !this.playlist.Interrupted);
        }

        /// <summary>Waits for the playlist to finish playback buffer</summary>
        protected void WaitUntilFinished()
        {
            //play audio & Let the sound play
            Boolean isRunning = true;
            while (isRunning)
            {
                VoiceState state = Output.GetSourceVoiceState(this.OutputSoundKey);
                isRunning = (state != null) && (state.BuffersQueued > 0);
                Thread.Sleep(10);
            }
        }

        /// <summary>Method exposing a stop command</summary>
        public virtual void StopPlayback()
        {
            this.Output.HaltPlayback();
        }

        /// <summary>Method exposing an Interrupt command</summary>
        public virtual void InterruptPlayback()
        {
            this.playlist.Interrupt();
            this.DoPostMessage(new MessageEventArgs("Playback interrupted...", "Interrupt", this.playlist.PlaylistName));

            //wait until finished.
            this.WaitUntilFinished();

            this.DoPostMessage(new MessageEventArgs("Playback completed.", "Playback completed", this.playlist.PlaylistName));
        }
        #endregion
    }
}