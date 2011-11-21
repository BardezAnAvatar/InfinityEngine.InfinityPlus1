using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Bardez.Projects.Configuration;
using Bardez.Projects.DirectX.XAudio2;
using Bardez.Projects.InfinityPlus1.Files.External.Interplay.ACM;
using Bardez.Projects.InfinityPlus1.Files.External.RIFF.Component;
using Bardez.Projects.InfinityPlus1.Files.External.RIFF.Wave;
using Bardez.Projects.InfinityPlus1.Output.Audio;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.ReusableCode;
using Bardez.Projects.Win32.Audio;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.AmpitudeCodedModulation
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.External.Interplay.ACM.AcmAudioFile class.</summary>
    public class AcmFileTest : FileTesterBase
    {
        #region Fields
        /// <summary>Constant key to look up in app.config</summary>
        public const String configKey = "Test.ACM.AcmPath.BT1";

        /// <summary>Reference to XAudio2 object</summary>
        protected XAudio2Output Output { get; set; }

        /// <summary>List of audio files decoded</summary>
        protected GenericOrderedDictionary<String, AcmAudioFile> audioFiles;

        /// <summary>Exposure of list of audio files decoded</summary>
        protected GenericOrderedDictionary<String, AcmAudioFile> AudioFiles
        {
            get
            {
                if (this.audioFiles == null)
                    this.audioFiles = new GenericOrderedDictionary<String, AcmAudioFile>();

                return this.audioFiles;
            }
        }

        /// <summary>Flag indicating status of decoded audio data</summary>
        protected Boolean decodedData;

        /// <summary>Flag indicating whether or not to render audio data</summary>
        public Boolean RenderAudio { get; set; }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public AcmFileTest()
        {
            this.InitializeInstance();
            this.decodedData = false;
            this.RenderAudio = true;
        }
        #endregion

        /// <summary>Initializes the test class data</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="e">Specific initialization event parameters</param>
        protected override void InitializeTestData(Object sender, EventArgs e)
        {
            this.Output = XAudio2Output.Instance;
            this.FilePaths = ConfigurationHandlerMulti.GetSettingValues(AcmFileTest.configKey);
            this.ReadFiles();
        }

        /// <summary>Event to raise for testing instance(s)</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            AcmAudioFile file = this.audioFiles[testArgs.Path];

            if (this.RenderAudio)       //render audio
                this.RenderAudioSamples(file);
            else                        //save to disk for analysis
                SaveRawPcmToDisk(file, testArgs.Path);
        }

        /// <summary>Method exposing a stop command</summary>
        public virtual void StopPlayback()
        {
            this.Output.HaltPlayback();
        }

        #region Helper Methods
        /// <summary>Reads all audiofiles to the AudioFiles list</summary>
        protected void ReadFiles()
        {
            if (!this.decodedData)
                foreach (String path in this.FilePaths)
                    using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        AcmAudioFile file = new AcmAudioFile();
                        file.Read(stream);
                        this.AudioFiles.Add(path, file);
                    }
        }

        /// <summary>Saves the raw PCM samples to disk</summary>
        /// <param name="file">AudioFile to read samples from</param>
        /// <param name="filePath">Base filpath to rewrite to</param>
        protected static void SaveRawPcmToDisk(AcmAudioFile file, String filePath)
        {
            String path = filePath + ".pcm.raw";
            using (FileStream dest = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                Byte[] sampleData = file.GetSampleData();
                dest.Write(sampleData, 0, sampleData.Length);
            }
        }

        /// <summary>Renders the audio file to hardware, used one file at a time</summary>
        /// <param name="file">Source AudioFile to read from</param>
        protected void RenderAudioSamples(AcmAudioFile file)
        {
            WaveFormatEx waveFormat = file.GetWaveFormat();
            Byte[] sampleData = file.GetSampleData();

            Int32 key = Output.CreatePlayback(waveFormat);
            Output.SubmitData(sampleData, key, 0, false);

            //play audio & Let the sound play
            Boolean isRunning = true;
            while (isRunning)
            {
                VoiceState state = Output.GetSourceVoiceState(key);
                isRunning = (state != null) && (state.BuffersQueued > 0);
                Thread.Sleep(10);
            }
        }
        #endregion

        #region Deprecated Code
        /// <summary>Tests the code, one audio file at a time</summary>
        /// <param name="paths">File paths to test</param>
        [Obsolete("On-the-fly coding remnant")]
        protected void TestProcedurally(String[] paths)
        {
            foreach (String path in paths)
                this.TestCase(this, new TestEventArgs(path));

            this.decodedData = true;
        }

        /// <summary>Tests the code, one audio file after another for gapless playback</summary>
        [Obsolete("On-the-fly coding remnant")]
        protected void TestSequentially(String[] paths)
        {
            this.ReadFiles();

            Int32 key = -1; //key to the source voice
            if (this.AudioFiles.Count > 0)
            {
                //load up the initial Source voice
                WaveFormatEx waveFormat = this.AudioFiles[0].AcmHeader.GetWaveFormat();
                key = Output.CreatePlayback(waveFormat);

                //prime before loop
                Byte[] sampleData = this.AudioFiles[0].GetSampleData();
                Output.SubmitData(sampleData, key, 0, true, false);

                //loop
                for (Int32 index = 1; index < Math.Min(11, this.AudioFiles.Count); ++index)  //I counted 12 samples to sound out the main audio loop
                {
                    sampleData = this.AudioFiles[index].GetSampleData();
                    Output.SubmitSubsequentData(sampleData, key, (this.AudioFiles.Count - 1) > index);
                }

                //final entry
                if (this.AudioFiles.Count > 11)
                {
                    sampleData = this.AudioFiles[11].GetSampleData();
                    Output.SubmitSubsequentData(sampleData, key, false, false);
                }

                this.Output.StartPlayback(key);

                //play audio & Let the sound play
                Boolean isRunning = true;
                while (isRunning)
                {
                    VoiceState state = Output.GetSourceVoiceState(key);
                    isRunning = (state != null) && (state.BuffersQueued > 0);
                    Thread.Sleep(10);
                }
            }
        }
        #endregion
    }
}