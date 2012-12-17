using System;
using System.IO;
using System.Text;
using System.Threading;

using Bardez.Projects.BasicStructures.Win32;
using Bardez.Projects.BasicStructures.Win32.Audio;
using Bardez.Projects.Configuration;
using Bardez.Projects.DirectX.XAudio2;
using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component;
using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Wave;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.Output.DirectX
{
    /// <summary>
    ///     This class tests the usable methods in the Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component.RiffHeader
    ///     and the Bardez.Projects.DirectX.XAudio2.XAudio2Interface classes.
    /// </summary>
    public class XAudio2RenderTest : TesterBase
    {
        #region Fields
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.Riff.RiffPath";

        /// <summary>Format instance to test</summary>
        protected XAudio2Interface XAudio { get; set; }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public XAudio2RenderTest()
        {
            this.InitializeInstance();
        }
        #endregion

        /// <summary>Initializes the test class data</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="e">Specific initialization event parameters</param>
        /// <remarks>No implementation due to lack of initialization to this test class</remarks>
        protected override void InitializeTestData(Object sender, EventArgs e) { }

        /// <summary>Event to raise for testing instance(s)</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            using (FileStream stream = new FileStream(ConfigurationHandlerMulti.GetSettingValue(XAudio2RenderTest.configKey), FileMode.Open, FileAccess.Read))
            {
                WaveRiffFile waveRiff = new WaveRiffFile();
                waveRiff.Read(stream);
                WaveFormatEx waveFormat = waveRiff.GetWaveFormat();
                Byte[] sampleData = this.GetWaveData(waveRiff);

                using (this.XAudio = XAudio2Interface.NewInstance())
                {
                    using (MasteringVoice master = this.XAudio.CreateMasteringVoice(waveFormat.NumberChannels, waveFormat.SamplesPerSec))
                    {
                        using (SourceVoice sourceVoice = this.XAudio.CreateSourceVoice(waveFormat))
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
        }

        /// <summary>Gets binary sample data from Riff file</summary>
        /// <param name="riff">Riff file to read data from</param>
        /// <returns>Byte array of sample data</returns>
        protected virtual Byte[] GetWaveData(RiffFile riff)
        {
            IRiffChunk wave = riff.RootChunk.FindFirstSubChunk(ChunkType.data).Chunk;
            return wave.Data;
        }
    }
}