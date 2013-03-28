using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

using Bardez.Projects.BasicStructures.ThreeDimensional;
using Bardez.Projects.BasicStructures.Win32;
using Bardez.Projects.BasicStructures.Win32.Audio;
using Bardez.Projects.DirectX.X3DAudio;
using Bardez.Projects.DirectX.XAudio2;
using Bardez.Projects.DirectX.XAudio2.FX;
using Bardez.Projects.DirectX.XAudio2.XAPO;
using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Wave;
using Bardez.Projects.InfinityPlus1.Output.Audio.Renderers;
using Bardez.Projects.Multimedia.MediaBase.Render.Audio;

namespace Scratch
{
    internal class XAudio2_reimp
    {
        #region Constants
        //protected static readonly String AudioRiffFile = @"\Multi-Media\Audio\vaqueros02[1].wav";
        protected static readonly String AudioRiffFile = @"\Multi-Media\Audio\start [what is thy bidding].wav";
        #endregion

        internal void TestSomeXAudio2Stuff()
        {
            Byte[] sampleData = null;
            WaveFormatEx format = null;

            using (FileStream fs = File.Open(XAudio2_reimp.AudioRiffFile, FileMode.Open, FileAccess.Read))
            {
                WaveRiffFile wave = new WaveRiffFile();
                wave.Read(fs);

                sampleData = wave.GetWaveData();
                format = wave.GetWaveFormat();
            }

            TimeSpan ts = this.GetSampleLength(sampleData, format);

            //using (FileStream dest = File.Open(String.Format("{0}.dump.raw", riffFile), FileMode.Create, FileAccess.Write))
            //{
            //    dest.Write(sampleData, 0, sampleData.Length);
            //    dest.Flush();
            //}

            //chilling with my samples...
            using (XAudio2Interface xaudio2 = XAudio2Interface.NewInstance())
            {
                UInt32 deviceCount = xaudio2.GetDeviceCount();

                DeviceDetails [] devices = new DeviceDetails[deviceCount];
                for (UInt32 index = 0; index < deviceCount; ++index)
                    devices[index] = xaudio2.GetDeviceDetails(index);


                //so now I have an interface with a lot of device details & format info.
                
                //Try playback

                //Try to force the mastering voice to use a different sample rate
                using (MasteringVoice master = xaudio2.CreateMasteringVoice(format.NumberChannels, format.SamplesPerSec, 0U, 0U))
                {
                    //create a source voice; again, do not allow conversion
                    using (SourceVoice source = xaudio2.CreateSourceVoice(format, 0U))
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

        internal void XAudio2_ThreeeeeeDeeeeeeee_test()
        {
            Byte[] sampleData = null;
            WaveFormatEx format = null;

            using (FileStream fs = File.Open(XAudio2_reimp.AudioRiffFile, FileMode.Open, FileAccess.Read))
            {
                WaveRiffFile wave = new WaveRiffFile();
                wave.Read(fs);

                sampleData = wave.GetWaveData();
                format = wave.GetWaveFormat();
            }

            TimeSpan ts = this.GetSampleLength(sampleData, format);


            //Apply Some vectors
            Vector<Single> playing = new Vector<Single>(15.0F, 15.0F, 15.0F);
            Vector<Single> listener = new Vector<Single>(0.0F, 0.0F, 0.0F);


            //3D Calculations
            SpeakerPositions positions;
            if (format is WaveFormatExtensible)
                positions = (format as WaveFormatExtensible).ChannelMask;
            else if (format.NumberChannels == 1)
                positions = SpeakerPositions.SPEAKER_FRONT_CENTER;
            else if (format.NumberChannels == 2)
                positions = SpeakerPositions.SPEAKER_FRONT_LEFT | SpeakerPositions.SPEAKER_FRONT_RIGHT;
            else
                positions = SpeakerPositions.SPEAKER_ALL;


            //chilling with my samples...
            using (XAudio2Interface xaudio2 = XAudio2Interface.NewInstance())
            {
                UInt32 deviceCount = xaudio2.GetDeviceCount() + 1U;

                DeviceDetails[] devices = new DeviceDetails[deviceCount];
                for (UInt32 index = 0; index < deviceCount; ++index)
                    devices[index] = xaudio2.GetDeviceDetails(index);


                //so now I have an interface with a lot of device details & format info.


                //set up X3DAudio
                X3DAudio audio3D = new X3DAudio((UInt32)positions);
                Bardez.Projects.DirectX.X3DAudio.Listener x3daListener = new Bardez.Projects.DirectX.X3DAudio.Listener();
                x3daListener.Position = listener;

                Bardez.Projects.DirectX.X3DAudio.Emitter x3daEmitter = new Bardez.Projects.DirectX.X3DAudio.Emitter((UInt32)positions);
                x3daEmitter.Position = playing;

                DspSettings settings = new DspSettings(format.NumberChannels, format.NumberChannels);

                X3DAudioCalculationFlags calcFlags = X3DAudioCalculationFlags.Matrix | X3DAudioCalculationFlags.Reverberation | X3DAudioCalculationFlags.Doppler | X3DAudioCalculationFlags.LowPassFilterDirect | X3DAudioCalculationFlags.LowPassFilterReverberation;
                audio3D.CalculateAudio(x3daListener, x3daEmitter, calcFlags, settings);

                //Try playback

                //Try to force the mastering voice to use a different sample rate
                using (MasteringVoice master = xaudio2.CreateMasteringVoice(format.NumberChannels, format.SamplesPerSec, 0U, 0U))
                {
                    //create a source voice; again, do not allow conversion
                    using (SourceVoice source = xaudio2.CreateSourceVoice(format, 0U))
                    {
                        source.SetOutputVoices(new VoiceSendDescriptor[] { new VoiceSendDescriptor(0U, master) });

                        //should be wired up. Try to submit data
                        AudioBuffer buffer = new AudioBuffer(XAudio2Interface.VoiceFlags.EndOfStream, sampleData, 0, 0, 0, 0, 0, IntPtr.Zero);

                        source.SubmitSourceBuffer(buffer, null);


                        //X3DAudio effects:
                        source.SetOutputMatrix(master, format.NumberChannels, format.NumberChannels, settings.CoefficientsMatrix, 0);
                        source.SetFrequencyRatio(settings.DopplerFactor, 0);


                        //now begin playback. Wave is what? 22 seconds? so sleep for 25

                        source.Start();
                        System.Threading.Thread.Sleep(ts);
                    }
                }
            }
        }

        /*
        internal void TestXAudio2IRenderer()
        {
            Byte[] sampleData = null;
            WaveFormatEx format = null;

            using (FileStream fs = File.Open(XAudio2_reimp.AudioRiffFile, FileMode.Open, FileAccess.Read))
            {
                WaveRiffFile wave = new WaveRiffFile();
                wave.Read(fs);

                sampleData = wave.GetWaveData();
                format = wave.GetWaveFormat();
            }
        
            TimeSpan ts = this.GetSampleLength(sampleData, format);

            //chilling with my samples...
            using (IAudioRenderer renderer = new XAudio2AudioRenderer())
            {
                renderer.Initialize(format, new SpeakerConfiguration(format.NumberChannels, format.ChannelMask), AudioRenderStyle.PreBuffered);
                renderer.SubmitSampleData(sampleData);

                //now begin playback. Wave is what? 22 seconds? so sleep until done.
                renderer.StartRendering();
                Thread.Sleep(ts);
            }
        }
        */

        internal void TestXAudio2Reverb()
        {
            Byte[] sampleData = null;
            WaveFormatEx format = null;

            using (FileStream fs = File.Open(XAudio2_reimp.AudioRiffFile, FileMode.Open, FileAccess.Read))
            {
                WaveRiffFile wave = new WaveRiffFile();
                wave.Read(fs);

                sampleData = wave.GetWaveData();
                format = wave.GetWaveFormat();
            }
            
            TimeSpan ts = this.GetSampleLength(sampleData, format);

            //Create XAudio2
            using (XAudio2Interface xaudio2 = XAudio2Interface.NewInstance())
            {
                UInt32 deviceCount = xaudio2.GetDeviceCount();

                DeviceDetails[] devices = new DeviceDetails[deviceCount];
                for (UInt32 index = 0; index < deviceCount; ++index)
                    devices[index] = xaudio2.GetDeviceDetails(index);


                //so now I have an interface with a lot of device details & format info.

                //resample the rate for a reverb
                UInt32 sampleRate = format.SamplesPerSec;
                while (sampleRate < 20000)
                    sampleRate *= 2;   //alt: shift right one bit

                //Try to force the mastering voice to use a different sample rate
                using (MasteringVoice master = xaudio2.CreateMasteringVoice(format.NumberChannels, sampleRate, 0U, 0U))
                {
                    EnvironmentalReverbEffect envReverbEffect = new EnvironmentalReverbEffect();
                    EnvironmentalReverbParameters envReverbParams = EnvironmentalReverbParameters.SewerPipe;
                    List<EffectDescriptor> effectChain = new List<EffectDescriptor>() { new EffectDescriptor(envReverbEffect, true, format.NumberChannels) };

                    //create a submix for the reverb, using a recomputed sample rate
                    using (SubmixVoice submix = xaudio2.CreateSubmixVoice(format.NumberChannels, sampleRate, 0U, 0U, new VoiceSendDescriptor[] { new VoiceSendDescriptor(0U, master) }))
                    {
                        //set the output sends for Submix
                        submix.SetOutputVoices(new VoiceSendDescriptor[] { new VoiceSendDescriptor(0U, master) });
                        ResultCode result = submix.SetEffectChain(effectChain);

                        //create a source voice; again, do not allow conversion
                        using (SourceVoice source = xaudio2.CreateSourceVoice(format, 0U))
                        {
                            source.SetOutputVoices(new VoiceSendDescriptor[] { new VoiceSendDescriptor(0U, submix) });
                            submix.SetEffectParameters(0U, envReverbParams, 0U);

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
            
            return; //just to give me a clue in the Lisp-like brace nightmare I've created
        }

        /// <summary>Gets the Length of the audio samples as a TimeSpan</summary>
        /// <param name="sampleData">Sample day to analyze</param>
        /// <param name="format">Wave format</param>
        /// <returns>The length of the sample data</returns>
        protected TimeSpan GetSampleLength(Byte[] sampleData, WaveFormatEx format)
        {
            Int64 length = ((sampleData.Length * 1000) / format.SamplesPerSec / format.NumberChannels / (format.BitsPerSample / 8)) + 1;
            return new TimeSpan(0, 0, 0, 0, Convert.ToInt32(length));
        }
    }
}