﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Bardez.Projects.Configuration;
using Bardez.Projects.DirectX.XAudio2;
using Bardez.Projects.InfinityPlus1.Output.Audio;
using Bardez.Projects.Win32.Audio;
using Bardez.Projects.InfinityPlus1.Files.External.Interplay.ACM;
using Bardez.Projects.InfinityPlus1.Files.External.RIFF.Component;
using Bardez.Projects.InfinityPlus1.Files.External.RIFF.Wave;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.AmpitudeCodedModulation
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.External.Interplay.ACM.WavCAudioFile class.</summary>
    public class WavcFileTest : ITester
    {
        protected const String configKey = "Test.WAVC.WavPath.IWD.Female_Rogue_2";
        protected XAudio2Output output;
        protected List<WavCAudioFile> audioFiles;
        protected List<WavCAudioFile> AudioFiles
        {
            get
            {
                if (this.audioFiles == null)
                    this.audioFiles = new List<WavCAudioFile>();

                return this.audioFiles;
            }
        }
        protected Boolean decodedData = false;

        public void Test()
        {
            this.output = XAudio2Output.Instance;

            String[] paths = ConfigurationHandlerMulti.GetSettingValues(configKey).ToArray();
            //this.TestProcedurally(paths);
            this.TestSequentially(paths);

            //String path = ConfigurationHandlerMulti.GetSettingValue("Test.ACM.AcmPath");
            //this.Test(path, false);
            //this.TestMulti(paths);
        }

        /// <summary>Tests the code, one audio file at a time</summary>
        /// <param name="paths">File paths to test</param>
        public void TestProcedurally(String[] paths)
        {
            foreach (String path in paths)
                this.Test(path, false);

            this.decodedData = true;
        }

        /// <summary>Tests the code, one audio file after another for gapless playback</summary>
        public void TestSequentially(String[] paths)
        {
            this.ReadFiles(paths);

            Int32 key = -1; //key to the source voice
            if (this.AudioFiles.Count > 0)
            {
                //load up the initial Source voice
                WaveFormatEx waveFormat = this.AudioFiles[0].GetWaveFormat();
                key = output.CreatePlayback(waveFormat);

                //prime before loop
                Byte[] sampleData = this.AudioFiles[0].GetSampleData();
                output.SubmitData(sampleData, key, 0, true, false);

                //loop
                for (Int32 index = 1; index < this.AudioFiles.Count; ++index)
                {
                    sampleData = this.AudioFiles[index].GetSampleData();
                    output.SubmitSubsequentData(sampleData, key, (this.AudioFiles.Count - 1) > index);
                }

                this.output.StartPlayback(key);

                //play audio & Let the sound play
                Boolean isRunning = true;
                while (isRunning)
                {
                    VoiceState state = output.GetSourceVoiceState(key);
                    isRunning = (state != null) && (state.BuffersQueued > 0);
                    Thread.Sleep(10);
                }
            }
        }
        
        /// <summary>Tests a single file</summary>
        /// <param name="path">File to open and read, then replicate</param>
        /// <param name="prompt">Boolean indicating whether or not to prompt between read and write</param>
        public void Test(String path, Boolean prompt)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                this.Test(stream, prompt);
        }

        /// <summary>Tests the read and ToString() methods of the structure</summary>
        /// <param name="source">Source Stream to read from</param>
        /// <param name="prompt">Boolean indicating whether or not to prompt for pressing [Enter] to continue</param>
        public void Test(Stream source, Boolean prompt)
        {
            WavCAudioFile file = new WavCAudioFile();
            file.Read(source);
            this.AudioFiles.Add(file);

            if (true)           //render audio
                this.RenderAudioProcedurally(file);
        }

        /// <summary>Saves the raw PCM samples to disk</summary>
        /// <param name="file">AudioFile to read samples from</param>
        protected static void SaveRawPcmToDisk(WavCAudioFile file)
        {
            String path = ConfigurationHandlerMulti.GetSettingValue("Test.ACM.AcmPath") + ".acm.raw";
            using (FileStream dest = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                Byte[] sampleData = file.GetSampleData();
                dest.Write(sampleData, 0, sampleData.Length);
            }
        }

        /// <summary>Renders the audio file to hardware, used one file at a time</summary>
        /// <param name="file">Source AudioFile to read from</param>
        protected void RenderAudioProcedurally(WavCAudioFile file)
        {
            WaveFormatEx waveFormat = file.GetWaveFormat();
            Byte[] sampleData = file.GetSampleData();

            Int32 key = output.CreatePlayback(waveFormat);
            output.SubmitData(sampleData, key, 0, false);

            //play audio & Let the sound play /* I've made myself a neat little race condition here! */
            Boolean isRunning = true;
            while (isRunning)
            {
                VoiceState state = output.GetSourceVoiceState(key);
                isRunning = (state != null) && (state.BuffersQueued > 0);
                Thread.Sleep(10);
            }
        }

        /// <summary>Reads all audiofiles to the AudioFiles list</summary>
        /// <param name="paths">Filesystem paths for files to read</param>
        protected void ReadFiles(String[] paths)
        {
            if (!this.decodedData)
                foreach (String path in paths)
                    using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        WavCAudioFile file = new WavCAudioFile();
                        file.Read(stream);
                        this.AudioFiles.Add(file);
                    }
        }
    }
}