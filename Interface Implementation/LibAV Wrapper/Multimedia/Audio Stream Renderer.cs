using System;
using System.Timers;

using Bardez.Projects.BasicStructures.Win32.Audio;
using Bardez.Projects.Multimedia.LibAV;
using Bardez.Projects.Multimedia.MediaBase.Render;

namespace Bardez.Projects.Multimedia.LibAV.Wrapper
{
    /// <summary>
    ///     This class represents a LibAV audio stream's audio render manager, which exports audio chunks
    ///     when its attempt render method is successful
    /// </summary>
    public class AudioStreamRenderManager : IAVStreamRenderManager, IWaveFormatEx
    {
        #region Fields
        /// <summary>Event to raise for rendering audio output.</summary>
        protected event Action<TimeSpan, Byte[]> render;

        /// <summary>Buffer and processing info for this audio stream</summary>
        protected StreamProcessingBuffer<FrameAudioInt16> AudioBuffer { get; set; }

        /// <summary>Locking object to allow the audio renderer to de-couple from the video renderer</summary>
        private Object renderLock;
        #endregion


        #region Properties
        /// <summary>Exposs a flag indicating whether or not to process this stream</summary>
        public Boolean Process
        {
            get { return this.AudioBuffer.Process; }
            set { this.AudioBuffer.Process = value; }
        }

        /// <summary>exposes the stream index this manager renders for</summary>
        public Int32 StreamIndex
        {
            get { return this.AudioBuffer.Index; }
        }
        #endregion


        #region Exposed Events
        /// <summary>Event to raise for rendering audio output.</summary>
        public event Action<TimeSpan, Byte[]> Render
        {
            add { this.render += value; }
            remove { this.render -= value; }
        }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="buffer">Buffer to create the manager for</param>
        public AudioStreamRenderManager(StreamProcessingBuffer<FrameAudioInt16> buffer)
        {
            this.AudioBuffer = buffer;
            this.renderLock = new Object();
        }
        #endregion


        #region Command & Control
        /// <summary>
        ///     Attempts to render audio if the buffer is processed and the timespan is within the
        ///     rendering threshold
        /// </summary>
        /// <param name="time">Time code to render by</param>
        public void AttemptRender(TimeSpan time)
        {
            lock (this.renderLock)
            {
                if (this.AudioBuffer.Process)
                {
                    FrameAudioInt16 peek = this.AudioBuffer.PeekFrame();
                    if (peek != null && peek.GetPresentationStartTimeSpan(this.AudioBuffer.TimeBase) < time)
                    {
                        peek = this.AudioBuffer.ConsumeFrame();
                        this.RaiseRenderAudio(time, peek.Data);
                    }
                }
            }
        }
        #endregion


        #region Event control
        /// <summary>Raises the render audio event</summary>
        /// <param name="timeCode">TimeSpan of this event</param>
        /// <param name="data">Data to be rendered</param>
        protected void RaiseRenderAudio(TimeSpan timeCode, Byte[] data)
        {
            if (this.render != null)
                this.render(timeCode, data);
        }
        #endregion


        #region IWaveFormatEx exposure
        /// <summary>Returns a WaveFormatEx instance from this header data</summary>
        /// <returns>A WaveFormatEx instance to submit to API calls</returns>
        public WaveFormatEx GetWaveFormat()
        {
            WaveFormatEx waveFormat = new WaveFormatEx();
            waveFormat.NumberChannels = Convert.ToUInt16(this.AudioBuffer.Codec.ChannelCount);
            waveFormat.FormatTag = (UInt16)AudioDataFormat.PCM;  //this may differ in very specific conditions.
            waveFormat.SamplesPerSec = Convert.ToUInt32(this.AudioBuffer.Codec.SampleRate);

            //HACK: This is not guaranteed
            waveFormat.BitsPerSample = Convert.ToUInt16(16);

            waveFormat.BlockAlignment = Convert.ToUInt16((waveFormat.BitsPerSample / 8) * waveFormat.NumberChannels);
            waveFormat.AverageBytesPerSec = waveFormat.SamplesPerSec * waveFormat.BlockAlignment;
            waveFormat.Size = 0;    //no extra data; this is strictly a WaveFormatEx instance 

            return waveFormat;
        }
        #endregion
    }
}