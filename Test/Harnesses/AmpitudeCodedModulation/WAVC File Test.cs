using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Bardez.Projects.Configuration;
using Bardez.Projects.DirectX.XAudio2;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.ACM;
using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component;
using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Wave;
using Bardez.Projects.InfinityPlus1.Output.Audio;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.ReusableCode;
using Bardez.Projects.Win32.Audio;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.AmpitudeCodedModulation
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.ACM.WavCAudioFile class.</summary>
    /// <remarks>Exteremely similar to AcmFileTest. Merge if further testing</remarks>
    public class WavcFileTest : FileTesterBase, IAudioTester
    {
        #region Fields
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.WAVC.WavPath.IWD.Female_Rogue_2";

        /// <summary>Reference to XAudio2 object</summary>
        protected XAudio2Output Output { get; set; }

        /// <summary>List of audio files decoded</summary>
        protected GenericOrderedDictionary<String, WavCAudioFile> audioFiles;

        /// <summary>Exposure of list of audio files decoded</summary>
        protected GenericOrderedDictionary<String, WavCAudioFile> AudioFiles
        {
            get
            {
                if (this.audioFiles == null)
                    this.audioFiles = new GenericOrderedDictionary<String, WavCAudioFile>();

                return this.audioFiles;
            }
        }

        /// <summary>Flag indicating status of decoded audio data</summary>
        protected Boolean decodedData = false;

        /// <summary>Flag indicating whether or not to render audio data</summary>
        public Boolean RenderAudio { get; set; }
        #endregion
        
        #region Construction
        /// <summary>Default constructor</summary>
        public WavcFileTest()
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
            this.FilePaths = ConfigurationHandlerMulti.GetSettingValues(WavcFileTest.configKey);
            this.ReadFiles();
        }

        /// <summary>Event to raise for testing instance(s)</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            WavCAudioFile file = this.audioFiles[testArgs.Path];

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
        protected virtual void ReadFiles()
        {
            if (!this.decodedData)
                foreach (String path in this.FilePaths)
                    using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        WavCAudioFile file = new WavCAudioFile();
                        file.Read(stream);
                        this.AudioFiles.Add(path, file);
                    }
        }

        /// <summary>Renders the audio file to hardware, used one file at a time</summary>
        /// <param name="file">Source AudioFile to read from</param>
        protected virtual void RenderAudioSamples(WavCAudioFile file)
        {
            WaveFormatEx waveFormat = file.GetWaveFormat();
            Byte[] sampleData = file.GetSampleData();

            Int32 destinationKey = Output.GetDefaultRenderer();
            Int32 sourceKey = Output.CreatePlayback(waveFormat, destinationKey);
            Output.SubmitData(sampleData, sourceKey, false);

            //play audio & Let the sound play
            Boolean isRunning = true;
            while (isRunning)
            {
                VoiceState state = Output.GetSourceVoiceState(sourceKey);
                isRunning = (state != null) && (state.BuffersQueued > 0);
                Thread.Sleep(10);
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
        #endregion

        #region Deprecated Code
        /// <summary>Tests the code, one audio file at a time</summary>
        /// <param name="paths">File paths to test</param>
        [Obsolete("On-the-fly coding remnant")]
        protected virtual void TestProcedurally(String[] paths)
        {
            foreach (String path in paths)
                this.TestCase(this, new TestEventArgs(path));
            
            this.decodedData = true;
        }

        /// <summary>Tests the code, one audio file after another for gapless playback</summary>
        [Obsolete("On-the-fly coding remnant")]
        protected virtual void TestSequentially(String[] paths)
        {
            this.ReadFiles();

            if (this.AudioFiles.Count > 0)
            {
                //load up the initial Source voice
                WaveFormatEx waveFormat = this.AudioFiles[0].GetWaveFormat();
                Int32 destinationKey = Output.GetDefaultRenderer(); //key to the destination output voice
                Int32 sourceKey = Output.CreatePlayback(waveFormat, destinationKey); //key to the source voice

                //prime before loop
                Byte[] sampleData = this.AudioFiles[0].GetSampleData();
                Output.SubmitData(sampleData, sourceKey, true, false);

                //loop
                for (Int32 index = 1; index < this.AudioFiles.Count; ++index)
                {
                    sampleData = this.AudioFiles[index].GetSampleData();
                    Output.SubmitData(sampleData, sourceKey, (this.AudioFiles.Count - 1) > index, false);
                }

                this.Output.StartPlayback(sourceKey);

                //play audio & Let the sound play
                Boolean isRunning = true;
                while (isRunning)
                {
                    VoiceState state = Output.GetSourceVoiceState(sourceKey);
                    isRunning = (state != null) && (state.BuffersQueued > 0);
                    Thread.Sleep(10);
                }
            }
        }
        #endregion
    }
}