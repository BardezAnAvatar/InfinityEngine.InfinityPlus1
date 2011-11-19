using System;
using System.IO;
using System.Threading;

using Bardez.Projects.Configuration;
using Bardez.Projects.DirectX.XAudio2;
using Bardez.Projects.Win32.Audio;
using Bardez.Projects.InfinityPlus1.Files.External.RIFF.Component;
using Bardez.Projects.InfinityPlus1.Files.External.RIFF.Wave;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.Output.DirectX
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.External.RIFF.Component.RiffHeader and the Bardez.Projects.DirectX.XAudio2.XAudio2Interface classes.</summary>
    public class XAudio2RenderTest : ITester
    {
        protected XAudio2Interface xaudio;

        public XAudio2Interface XAudio
        {
            get { return this.xaudio; }
        }

        /// <summary>Tests the class in question</summary>
        public void Test()
        {
            String[] paths = ConfigurationHandlerMulti.GetSettingValues("Test.Riff.RiffPath").ToArray();
            this.TestMulti(paths);
        }

        /// <summary>Tests a single file</summary>
        /// <param name="path">File to open and read, then replicate</param>
        /// <param name="prompt">Boolean indicating whether or not to prompt between read and write</param>
        public void Test(String path, Boolean prompt)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                this.Test(stream, prompt);
        }

        /// <summary>Tests the code </summary>
        /// <param name="paths"></param>
        public void TestMulti(String[] paths)
        {
            foreach (String path in paths)
            {
                this.Test(path, false);
            }
        }

        /// <summary>Tests the read and ToString() methods of the structure</summary>
        /// <param name="source">Source Stream to read from</param>
        /// <param name="prompt">Boolean indicating whether or not to prompt for pressing [Enter] to continue</param>
        public void Test(Stream source, Boolean prompt)
        {
            RiffFile riffFile = new RiffFile();
            riffFile.Read(source);

            WaveFormatEx waveFormat = this.GetWaveFormat(riffFile);
            Byte[] sampleData = this.GetWaveData(riffFile);

            using (this.xaudio = XAudio2Interface.NewInstance())
            {
                using (MasteringVoice master = this.xaudio.CreateMasteringVoice(waveFormat.NumberChannels, waveFormat.SamplesPerSec))
                {
                    using (SourceVoice sourceVoice = this.xaudio.CreateSourceVoice(waveFormat))
                    {
                        AudioBuffer buffer = new AudioBuffer(0x0040U, sampleData, 0, 0, 0, 0, 0, IntPtr.Zero);

                        sourceVoice.SetOutputVoices(new VoiceSendDescriptor[] { new VoiceSendDescriptor(0, master) });
                        sourceVoice.SubmitSourceBuffer(buffer, null);

                        //this.xaudio.StartEngine();

                        ResultCode result = sourceVoice.Start();

                        //play audio & Let the sound play
                        Boolean isRunning = true;
                        while (result == ResultCode.Success_OK && isRunning)
                        {
                            VoiceState state = sourceVoice.GetState();
                            isRunning = (state.BuffersQueued > 0);
                            Thread.Sleep(10);
                        }
                    }
                }
            }
        }

        /// <summary>Writes the data structure back out to a destination stream</summary>
        /// <param name="destination">Stream to write output to</param>
        public void TestWrite(Stream destination)
        {
            //this.chunk.Write(destination);
        }
        

        public Byte[] GetWaveData(RiffFile riff)
        {
            WaveSampleDataChunk wave = (riff.Header.FindFirstSubChunk(ChunkType.data).Chunk as WaveSampleDataChunk);
            Byte[] data = wave.Data;
            return data;
        }
        
        //todo: move to waveFormatChunk to get a waveformatex object
        public WaveFormatEx GetWaveFormat(RiffFile riff)
        {
            WaveFormatEx waveEx = new WaveFormatEx();
            WaveFormatExtensible waveExtensible = new WaveFormatExtensible();

            WaveFormatChunk format = (riff.Header.FindFirstSubChunk(ChunkType.fmt).Chunk as WaveFormatChunk);
            format.Read();

            waveEx.AverageBytesPerSec = format.ByteRate;
            waveEx.BitsPerSample = format.BitsPerSample;
            waveEx.BlockAlignment = format.BlockAlignment;
            waveEx.FormatTag = (UInt16)format.DataType;
            waveEx.NumberChannels = format.NumChannels;
            waveEx.SamplesPerSec = format.SampleRate;
            waveEx.Size = 0; //Convert.ToUInt16(format.Size);

            return waveEx;
        }
    }
}