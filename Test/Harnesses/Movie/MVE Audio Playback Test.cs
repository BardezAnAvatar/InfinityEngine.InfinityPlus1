using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Bardez.Projects.Configuration;
using Bardez.Projects.DirectX.XAudio2;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management;
using Bardez.Projects.InfinityPlus1.Output.Audio;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Test.Harnesses;
using Bardez.Projects.InfinityPlus1.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.Movie
{
    /// <summary>This class tests the audio playback abilities of the structure in the Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management.Mve.MveManager class</summary>
    public class MveAudioPlaybackTest : FileTesterBase, IAudioTester
    {
        #region Constants
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.MVE.Path";
        #endregion


        #region Fields
        /// <summary>Reference to XAudio2 object</summary>
        protected XAudio2Output Output { get; set; }

        /// <summary>Format instance to test</summary>
        protected MveManager mve { get; set; }

        /// <summary>Unique key context for the output source</summary>
        protected Int32 OutputSoundKey { get; set; }

        /// <summary>Audio stream index to play back</summary>
        protected Int32 AudioStream { get; set; }

        /// <summary>Audio stream index to play back</summary>
        protected Int32 AudioBlockIndex { get; set; }
        #endregion
        

        #region Construction
        /// <summary>Default constructor</summary>
        public MveAudioPlaybackTest()
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
            this.FilePaths = ConfigurationHandlerMulti.GetSettingValues(MveAudioPlaybackTest.configKey);
        }

        /// <summary>Event to raise for testing instance(s)</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            AudioTestEventArgs audioArgs = testArgs as AudioTestEventArgs;
            this.AudioStream = audioArgs.Channel;

            using (FileStream stream = new FileStream(audioArgs.Path, FileMode.Open, FileAccess.Read))
            {
                this.DoPostMessage(new MessageEventArgs("Reading MVE...", "Reading", testArgs.Path));
                //Read chunks
                MveChunkOpcodeIndexer mveIndexer = new MveChunkOpcodeIndexer();
                mveIndexer.Read(stream);

                //read opcodes
                mveIndexer.ReadChunkOpcodes(stream);

                //collect opcode data
                this.mve = new MveManager(mveIndexer);
                this.mve.CollectOpcodeIndex();

                //read audio data
                this.mve.ReadData(stream);

                this.DoPostMessage(new MessageEventArgs("Finished reading MVE.", "Output", testArgs.Path));
                this.TestPlayback();
            }
        }

        #region Helper methods
        /// <summary>Tests audio playback, allowing for an interrupt to occur by hitting enter.</summary>
        public void TestPlayback()
        {
            this.DoPostMessage(new MessageEventArgs("Readying playback...", "Waiting", "Starting"));

            this.mve.StartDecodingAudio();

            this.Output = XAudio2Output.Instance;
            this.AudioBlockIndex = 0;

            //load up the initial Source voice
            this.OutputSoundKey = Output.CreatePlayback(this.mve.WaveFormat);

            //Adjust callback(s)
            this.Output.AddSourceNeedDataEventhandler(this.OutputSoundKey, new AudioNeedsMoreDataHandler(this.NeedsMoreSamples));

            //submit first data
            this.NeedsMoreSamples();

            //play audio & Let the sound play
            this.Output.StartPlayback(this.OutputSoundKey);

            this.DoPostMessage(new MessageEventArgs("Playing Audio...", "Playing", "Audio"));
        }

        /// <summary>Callback event handler that sends more data to the output buffer</summary>
        public void NeedsMoreSamples()
        {
            Byte[] samples = this.mve.GetAudioBlock(this.AudioBlockIndex, this.AudioStream);
            ++this.AudioBlockIndex;

            this.Output.SubmitSubsequentData(samples, this.OutputSoundKey, this.AudioBlockIndex < this.mve.AudioBlockCount(this.AudioStream));
        }

        /// <summary>Method exposing a stop command</summary>
        public virtual void StopPlayback()
        {
            this.Output.HaltPlayback();
        }
        #endregion
    }
}