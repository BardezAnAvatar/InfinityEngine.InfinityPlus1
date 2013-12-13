using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Bardez.Projects.BasicStructures.Win32.Audio;
using Bardez.Projects.Multimedia.MediaBase.Render.Audio;

namespace Bardez.Projects.InfinityPlus1.Output.Audio
{
    /// <summary>This audio renderer writes the received files to disk</summary>
    /// <remarks>This renderer does not even pretend to be real-time or approximate-time. If set to loop, it will probably generate a LOT of data.</remarks>
    public class RawSampleFileWriterAudioRenderer : IAudioRenderer
    {
        #region Fields
        /// <summary>Output stream to write data to.</summary>
        protected FileStream OutputStream;

        /// <summary>Output stream to write data to.</summary>
        protected Object OutputStreamLock;

        /// <summary>Collection of submitted audio samples. Just a simple buffer to collect.</summary>
        protected Queue<Byte[]> SampleBuffer;

        /// <summary>Locking object for submittedSamples</summary>
        protected Object SampleBufferLock;

        /// <summary>Thread that performs the sample writing to disk</summary>
        protected Thread writingThread;

        /// <summary>Locking Object that blocks the access to the 'exit processing' flag</summary>
        private Object exitThreadLock;

        /// <summary>Flag that indicates whether the application should exit the I/O flag</summary>
        private Boolean exitThread;

        /// <summary>Field that indicates the type of rendering desired</summary>
        protected AudioRenderStyle renderingStyle;

        /// <summary>Locking Object that blocks the access to the RenderingStyle</summary>
        private Object RenderingStyleLock;
        #endregion


        #region Properties
        /// <summary>Thread-safe exposure of the flag indicating whether the application should exit the I/O flag</summary>
        protected Boolean ExitThread
        {
            get
            {
                lock (this.exitThreadLock)
                    return this.exitThread;
            }
            set
            {
                lock (this.exitThreadLock)
                    this.exitThread = value;
            }
        }

        /// <summary>Thread-safe exposure of the the field that indicates the type of rendering desired</summary>
        protected AudioRenderStyle RenderingStyle
        {
            get
            {
                lock (this.RenderingStyleLock)
                    return this.renderingStyle;
            }
            set
            {
                lock (this.RenderingStyleLock)
                    this.renderingStyle = value;
            }
        }
        #endregion


        #region Events
        /// <summary>Event that occurs when the renderer has finished rendering the provided data stream(s). Used to signal expiration and ready for disposal.</summary>
        protected event EventHandler finishedRendering;

        /// <summary>
        ///     Event that occurs when the renderer has processed all provided data and requires additional data
        ///     if the behavior specified is to continue rendering
        /// </summary>
        protected event EventHandler requestAdditionalData;
        #endregion


        #region Construction
        /// <summary>Open Stream constructor</summary>
        /// <param name="destination">Path at which to open the FileStream</param>
        /// <param name="mode">FileMode to open the writer with</param>
        /// <param name="access">FileAccess with which to access the file</param>
        public RawSampleFileWriterAudioRenderer(String destination, FileMode mode, FileAccess access)
        {
            //durp
            if (access == FileAccess.Read)
                throw new InvalidOperationException(String.Format("A file WRITER need to have WRITE permissions to the file. The permission given was {0}.", access.ToString()));

            //instantiate locking references
            this.OutputStreamLock = new Object();
            this.exitThreadLock = new Object();
            this.SampleBufferLock = new Object();
            this.RenderingStyleLock = new Object();

            lock (this.SampleBufferLock)
                this.SampleBuffer = new Queue<Byte[]>();

            lock (this.OutputStreamLock)
                this.OutputStream = new FileStream(destination, mode, access);
        }

        /// <summary>Open Stream constructor</summary>
        /// <param name="destination">Path at which to open the FileStream</param>
        public RawSampleFileWriterAudioRenderer(String destination)
            : this (destination, FileMode.Create, FileAccess.Write) { }
        #endregion


        #region Destruction
        /// <summary>Disposal method</summary>
        public void Dispose()
        {
            this.ExitThread = true;

            lock (this.SampleBufferLock)
                this.SampleBuffer = null;

            lock (this.OutputStreamLock)
            {
                this.OutputStream.Flush();
                this.OutputStream.Dispose();
            }

            this.writingThread = null;
            this.exitThreadLock = null;
            this.OutputStreamLock = null;
            this.SampleBufferLock = null;

            GC.SuppressFinalize(this);
        }
        #endregion


        #region IAudioRenderer Properties
        /// <summary>Exposes a flag indicating whether or not the renderer can accept new data</summary>
        public Boolean CanAcceptSampleData
        {
            get { return true; }
        }
        #endregion


        #region IAudioRenderer Events
        /// <summary>Event that occurs when the renderer has finished rendering the provided data stream(s). Used to signal expiration and ready for disposal.</summary>
        public event EventHandler FinishedRendering
        {
            add { this.finishedRendering += value; }
            remove { this.finishedRendering -= value; }
        }

        /// <summary>
        ///     Event that occurs when the renderer has processed all provided data and requires additional data
        ///     if the behavior specified is to continue rendering
        /// </summary>
        public event EventHandler RequestAdditionalData
        {
            add { this.requestAdditionalData += value; }
            remove { this.requestAdditionalData -= value; }
        }
        #endregion


        #region IAudioRenderer Methods
        /// <summary>Initializes this audio renderer to use the settings provided</summary>
        /// <param name="audioInfo">Collection of data that indicates what the format of the source audio is</param>
        /// <param name="destinationConfiguration">Configuration of audio channels for rendering output</param>
        /// <param name="behavior">Behavior with which to render audio</param>
        /// <param name="targetDeviceName">Name of the target device to be rendered to. If null or not found, system will use the system default device, if appropriate</param>
        public void Initialize(WaveFormatEx audioInfo, SpeakerConfiguration destinationConfiguration, AudioRenderStyle behavior, ReverbSettings reverb, String targetDeviceName)
        {
            this.RenderingStyle = behavior;

            //A raw sample file writer does not care about wave data format. An MP3 container would, a sound card driver would, a *.wav file container would, but not a raw-sample file writer.
        }

        /// <summary>Submits data for rendering</summary>
        /// <param name="data">Audio samples to render</param>
        public void SubmitSampleData(Byte[] data)
        {
            lock (this.SampleBufferLock)
                this.SampleBuffer.Enqueue(data);
        }

        /// <summary>Command to start rendering audio</summary>
        public void StartRendering()
        {
            this.ExitThread = false;

            if (this.writingThread == null)
            {
                Thread writer = new Thread(this.WriteSamples);
                writer.Name = "Raw Sample File Writer Audio Renderer disk I/O thread";
                writer.Priority = ThreadPriority.Lowest;    //this is an I/O thread. Who cares?
                this.writingThread = writer;
                writer.Start();
            }
        }

        /// <summary>Sets details for the rendering context (such as 3D coordinates and so on)</summary>
        /// <param name="detail">Rendering context details to set</param>
        public void SetRenderDetails(AudioSourceParams detail)
        {
            //This raw sample file writer does not care about this. A sound card driver would, but not a raw-sample file writer.
        }

        /// <summary>Pauses audio playback. Buffers will pick up where left off if rendering resumed</summary>
        /// <param name="finishEffects">Flag to indicate whether any effects should render (e.g.: echo) [true] or if it should halt completely [false]</param>
        public void Pause(Boolean finishEffects)
        {
            this.ExitThread = true;
            this.writingThread = null;
        }
        #endregion


        #region Methods
        /// <summary>Method that writes the sample data to disk until told to stop</summary>
        protected void WriteSamples()
        {
            while (!this.ExitThread)
            {
                Byte[] samples = null;

                lock (this.SampleBufferLock)
                {
                    if (this.SampleBuffer.Count > 0)
                        samples = this.SampleBuffer.Dequeue();
                }

                if (samples != null)
                {
                    lock (this.OutputStreamLock)
                        this.OutputStream.Write(samples, 0, samples.Length);
                }

                lock (this.RenderingStyleLock)
                {
                    //re-queue the samples
                    if (this.renderingStyle == AudioRenderStyle.Loop && samples != null)
                    {
                        lock (this.SampleBufferLock)
                            this.SampleBuffer.Enqueue(samples);
                    }
                    else if (samples == null)
                    {
                        if (this.renderingStyle == AudioRenderStyle.PreBuffered)
                        {
                            this.RaiseFinishedRendering();
                            this.ExitThread = true; //we're finished, so exit
                        }
                        else if (this.renderingStyle == AudioRenderStyle.Streaming)
                            this.RaiseRequestAdditionalData();
                    }
                }

                //sleep so as to not burn the CPU
                Thread.Sleep(50);
            }
        }

        /// <summary>Raises the FinishedRendering event</summary>
        protected void RaiseFinishedRendering()
        {
            if (this.finishedRendering != null)
                this.finishedRendering(this, new EventArgs());
        }

        /// <summary>Raises the RequestAdditionalData event</summary>
        protected void RaiseRequestAdditionalData()
        {
            if (this.requestAdditionalData != null)
                this.requestAdditionalData(this, new EventArgs());
        }
        #endregion
    }
}