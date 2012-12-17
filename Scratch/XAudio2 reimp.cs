using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.BasicStructures.Win32.Audio;
using Bardez.Projects.DirectX.XAudio2;
using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Wave;

namespace Scratch
{
    internal class XAudio2_reimp
    {
        internal void TestSomeXAudio2Stuff()
        {
            String riffFile = @"\Multi-Media\Audio\vaqueros02[1].wav";

            Byte[] sampleData = null;
            WaveFormatEx format = null;

            using (FileStream fs = File.Open(riffFile, FileMode.Open, FileAccess.Read))
            {
                WaveRiffFile wave = new WaveRiffFile();
                wave.Read(fs);

                sampleData = wave.GetWaveData();
                format = wave.GetWaveFormat();
            }

            Int64 length = ((sampleData.Length * 1000) / format.SamplesPerSec) + 1;
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(length));

            using (FileStream dest = File.Open(String.Format("{0}.dump.raw", riffFile), FileMode.Create, FileAccess.Write))
            {
                dest.Write(sampleData, 0, sampleData.Length);
                dest.Flush();
            }

            //chilling with my samples...
            using (XAudio2Interface xaudio2 = XAudio2Interface.NewInstance())
            {
                UInt32 deviceCount = xaudio2.GetDeviceCount() + 1U;

                DeviceDetails [] devices = new DeviceDetails[deviceCount];
                for (UInt32 index = 0; index < deviceCount; ++index)
                    devices[index] = xaudio2.GetDeviceDetails(index);


                //so now I have an interface with a lot of device details & format info.
                
                //Try playback

                //Try to force the mastering voice to use a different sample rate
                using (MasteringVoice master = xaudio2.CreateMasteringVoice(format.NumberChannels, format.SamplesPerSec, XAudio2Interface.VoiceFlags.NoSampleRateConversion, 0U))
                {
                    //create a source voice; again, do not allow conversion
                    using (SourceVoice source = xaudio2.CreateSourceVoice(format, XAudio2Interface.VoiceFlags.NoSampleRateConversion))
                    {
                        source.SetOutputVoices(new VoiceSendDescriptor[] { new VoiceSendDescriptor(0U, master) });

                        //should be wired up. Try to submit data
                        AudioBuffer buffer = new AudioBuffer(XAudio2Interface.VoiceFlags.EndOfStream, sampleData, 0, 0, 0, 0, 0, IntPtr.Zero);

                        source.SubmitSourceBuffer(buffer, null);


                        //now begin playback. Wave is what? 22 seconds? so sleep for 25

                        source.Start();
                        System.Threading.Thread.Sleep(ts);
                    }
                }
            }
        }
    }
}