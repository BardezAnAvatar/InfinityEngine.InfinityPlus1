using System;
using System.Linq;

using Bardez.Projects.BasicStructures.ThreeDimensional;
using Bardez.Projects.BasicStructures.Win32;
using Bardez.Projects.BasicStructures.Win32.Audio;
using Bardez.Projects.Multimedia.MediaBase.Render.Audio;
using Bardez.Projects.DirectX.X3DAudio;
using Bardez.Projects.DirectX.XAudio2;
using Bardez.Projects.DirectX.XAudio2.XAPO;

namespace Bardez.Projects.InfinityPlus1.Output.Audio.Renderers
{
    /// <summary>Represents a single rendering agent for a single sound</summary>
    /// <remarks>This is a simple playback renderer that does not set up any filters</remarks>
    //TODO: Should the XAudio2 interface be singleton or should it be a singleton to access? I do not know.
    //  per MSDN: "You can create instances of XAudio2 multiple times within a single process."
    //  http://msdn.microsoft.com/en-us/library/windows/desktop/ee415764%28v=vs.85%29.aspx
    //TODO: Support reverberation (XAudio2FX.h - see XAudio2Sound3D sample project)

    public class XAudio2AudioRenderer : IAudioRenderer
    {
        #region Fields
        /// <summary>XAudio2 source voice</summary>
        protected SourceVoice SourceVoice;

        /// <summary>XAudio2 mastering voice</summary>
        protected MasteringVoice MasteringVoice;

        /// <summary>Instance of the XAudio2 interface to access</summary>
        protected XAudio2Interface XAudio2;

        /// <summary>Instance of the X3DAudio data & methods</summary>
        protected X3DAudio X3DAudio;

        /// <summary>Reference to a reverberation parameter</summary>
        /// <remarks>
        ///     Per MSDN, this must not be freed immediately.
        ///     I will release it when it is overwritten.
        ///     This may lead to unexpected behavior if it is *rapidly* set, which I do not expect to be the case.
        /// </remarks>
        protected EffectParameterBase ReverbParameter;

        /// <summary>Explains the expected style of behavior for this audio renderer</summary>
        protected AudioRenderStyle Behavior;

        /// <summary>Contains the details of the audio format</summary>
        protected WaveFormatEx AudioFormat;

        /// <summary>Destination speaker configuration</summary>
        protected SpeakerConfiguration OutputConfiguration;
        #endregion


        #region Events
        /// <summary>Event that occurs when the renderer has finished rendering the provided data stream(s). Used to signal expiration and ready for disposal.</summary>
        private event EventHandler finishedRendering;

        /// <summary>
        ///     Event that occurs when the renderer has processed all provided data and requires additional data
        ///     if the behavior specified is to continue rendering
        /// </summary>
        private event EventHandler requestAdditionalData;
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public XAudio2AudioRenderer()
        {
            this.XAudio2 = XAudio2Interface.NewInstance();
            this.X3DAudio = null;
            this.SourceVoice = null;
            this.MasteringVoice = null;
            this.Behavior = AudioRenderStyle.PreBuffered;
        }
        #endregion


        #region Destructionthis
        /// <summary>Dispose()</summary>
        public void Dispose()
        {
            this.Disposal();
            GC.SuppressFinalize(this);
        }

        /// <summary>Finalize()</summary>
        ~XAudio2AudioRenderer()
        {
            this.Disposal();
        }

        /// <summary>Disposal code</summary>
        public void Disposal()
        {
            if (this.SourceVoice != null)
            {
                this.Pause(false);
                this.SourceVoice.Dispose();
                this.SourceVoice = null;
            }

            if (this.MasteringVoice != null)
            {
                this.MasteringVoice.Dispose();
                this.MasteringVoice = null;
            }

            if (this.XAudio2 != null)
            {
                this.XAudio2.Dispose();
                this.XAudio2 = null;
            }

            if (this.X3DAudio != null)
                this.X3DAudio = null;
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


        #region IAudioRenderer Properties
        /// <summary>Exposes a flag indicating whether or not the renderer can accept new data</summary>
        public Boolean CanAcceptSampleData
        {
            get
            {
                VoiceState state = this.SourceVoice.GetState();
                return state.BuffersQueued < SourceVoice.MaximumBuffersQueued;
            }
        }
        #endregion


        #region IAudioRenderer Methods
        /// <summary>Initializes this audio renderer to use the settings provided</summary>
        /// <param name="audioInfo">Collection of data that indicates what the format of the source audio is</param>
        /// <param name="destinationConfiguration">Configuration of audio channels for rendering output</param>
        /// <param name="behavior">Behavior with which to render audio</param>
        /// <param name="targetDeviceName">Name of the target device to be rendered to. If null or not found, system will use the system default device, if appropriate</param>
        public void Initialize(WaveFormatEx audioInfo, SpeakerConfiguration destinationConfiguration, AudioRenderStyle behavior, String targetDeviceName)
        {
            if (audioInfo == null)
                throw new NullReferenceException("The parameter \"audioInfo\" was null.");
            else if (destinationConfiguration == null)
                throw new NullReferenceException("The parameter \"destinationConfiguration\" was null.");

            //initialize XAudio2
            if (this.MasteringVoice != null)
                throw new InvalidOperationException("This audio renderer is already Initialized.");

            //go through devices to find the target
            UInt32 deviceCount = this.XAudio2.GetDeviceCount() + 1U;

            DeviceDetails[] devices = new DeviceDetails[deviceCount];
            for (UInt32 i = 0; i < deviceCount; ++i)
                devices[i] = this.XAudio2.GetDeviceDetails(i);

            UInt32 renderingDeviceIndex = 0;
            for (UInt32 i = 0U; i < deviceCount; ++i)
                if (devices[i] != null && devices[i].DisplayName == targetDeviceName)
                    renderingDeviceIndex = i;

            //initialize the voices for rendering
            this.MasteringVoice = this.XAudio2.CreateMasteringVoice(audioInfo.NumberChannels, audioInfo.SamplesPerSec, 0U /* no SRC crashes, don't know why */, renderingDeviceIndex);
            this.SourceVoice = this.XAudio2.CreateSourceVoice(audioInfo);

            //copy behavior references
            this.Behavior = behavior;
            this.OutputConfiguration = destinationConfiguration;
        }

        /// <summary>Submits data for rendering</summary>
        /// <param name="data">Audio samples to render</param>
        public void SubmitSampleData(Byte[] data)
        {
            AudioBuffer buffer = new AudioBuffer(XAudio2Interface.VoiceFlags.EndOfStream, data, 0, 0, 0, 0, 0, IntPtr.Zero);

            if (this.Behavior == AudioRenderStyle.Loop)
            {
                buffer.LoopLength = Convert.ToUInt32(data.Length);
                buffer.LoopCount = AudioBuffer.XAUDIO2_LOOP_INFINITE;
            }
            
            this.SourceVoice.SubmitSourceBuffer(buffer, null);
        }

        /// <summary>Command to start rendering audio</summary>
        public void StartRendering()
        {
            //error checking
            if (this.SourceVoice == null)
                throw new NullReferenceException("The Source Voice for this renderer is null.");

            this.SourceVoice.Start(0U, 0U);
        }

        /// <summary>Sets details for the rendering context (such as 3D coordinates and so on)</summary>
        /// <param name="detail">Rendering context details to set</param>
        public void SetRenderDetails(AudioSourceParams detail)
        {
            if (detail == null || detail.Emitter == null || detail.Listener == null)
                //TODO: figure out how to disable 3D sound
                throw new NotImplementedException("How do I turn off 3D effects?");
            else
            {
                this.X3DAudio = new X3DAudio((UInt32)(this.OutputConfiguration.Positions));

                DirectX.X3DAudio.Listener x3daListener = new DirectX.X3DAudio.Listener(detail.Listener);
                DirectX.X3DAudio.Emitter x3daEmitter = new DirectX.X3DAudio.Emitter(detail.Emitter);
                //TODO: Figure out what  detail.MuffleSound would be used for, and where

                DspSettings settings = new DspSettings(this.AudioFormat.NumberChannels, this.OutputConfiguration.Count);

                X3DAudioCalculationFlags calcFlags = X3DAudioCalculationFlags.Matrix | X3DAudioCalculationFlags.Reverberation | X3DAudioCalculationFlags.Doppler | X3DAudioCalculationFlags.LowPassFilterDirect | X3DAudioCalculationFlags.LowPassFilterReverberation;
                this.X3DAudio.CalculateAudio(x3daListener, x3daEmitter, calcFlags, settings);

                //X3DAudio effects:
                this.SourceVoice.SetOutputMatrix(this.MasteringVoice, this.AudioFormat.NumberChannels, this.OutputConfiguration.Count, settings.CoefficientsMatrix, 0);
                this.SourceVoice.SetFrequencyRatio(settings.DopplerFactor, 0);
            }
        }

        /// <summary>Pauses audio playback. Buffers will pick up where left off if rendering resumed</summary>
        /// <param name="finishEffects">Flag to indicate whether any effects should render (e.g.: echo) [true] or if it should halt completely [false]</param>
        public void Pause(Boolean finishEffects)
        {
            this.SourceVoice.Stop(0U, 0U);
        }
        #endregion


        #region Callbacks
        /// <summary>Registers local handlers to callback events</summary>
        protected void RegisterCallbacks()
        {
            if (this.SourceVoice != null)
            {
                this.SourceVoice.Callback.BufferEnd += new XAudio2VoiceBufferEndHandler(this.BufferEndHandler);
                this.SourceVoice.Callback.BufferStart += new XAudio2VoiceBufferStartHandler(this.BufferStartHandler);
                this.SourceVoice.Callback.Error += new XAudio2VoiceErrorHandler(this.ErrorHandler);
                this.SourceVoice.Callback.LoopEnd += new XAudio2VoiceLoopEndHandler(this.LoopEndHandler);
                this.SourceVoice.Callback.ProcessingPassEnd += new XAudio2VoiceProcessingPassEndHandler(this.ProcessingPassEndHandler);
                this.SourceVoice.Callback.ProcessingPassStart += new XAudio2VoiceProcessingPassStartHandler(this.ProcessingPassStartHandler);
                this.SourceVoice.Callback.StreamEnd += new XAudio2VoiceStreamEndHandler(this.StreamEndHandler);
            }
        }

        #region Voice callbacks
        /// <summary>Delegate for when a voice buffer finishes</summary>
        /// <param name="bufferContext">IntPtr for a buffer context</param>
        internal virtual void BufferEndHandler(IntPtr bufferContext)
        {
        }

        /// <summary>Delegate for when a voice buffer starts</summary>
        /// <param name="bufferContext">IntPtr for a buffer context</param>
        internal virtual void BufferStartHandler(IntPtr bufferContext)
        {
        }

        /// <summary>Delegate for when a voice loop ends</summary>
        /// <param name="bufferContext">IntPtr for a buffer context</param>
        internal virtual void LoopEndHandler(IntPtr bufferContext)
        {
        }

        /// <summary>Delegate for when a voice stream ends</summary>
        internal virtual void StreamEndHandler()
        {
        }

        /// <summary>Delegate for when a voice error is raised</summary>
        /// <param name="bufferContext">IntPtr for a buffer context</param>
        /// <param name="error">Error encountered</param>
        internal virtual void ErrorHandler(IntPtr bufferContext, ResultCode error)
        {
        }

        /// <summary>Delegate for when a voice processing pass ends</summary>
        internal virtual void ProcessingPassEndHandler()
        {
        }

        /// <summary>Delegate for when a voice processing pass starts</summary>
        /// <param name="bytesRequired">The number of bytes that need to be submitted to avoid voice starvation</param>
        internal virtual void ProcessingPassStartHandler(UInt32 bytesRequired)
        {
            if (bytesRequired > 0)
                switch (this.Behavior)
                {
                    case AudioRenderStyle.Loop:
                        break;
                    case AudioRenderStyle.PreBuffered:
                        this.RaiseFinishedRendering();
                        break;
                    case AudioRenderStyle.Streaming:
                        this.RaiseRequestAdditionalData();
                        break;
                }
        }
        #endregion
        #endregion


        #region Helper Methods
        /// <summary>Raises the <see cref="FinishedRendering" /> event</summary>
        protected void RaiseFinishedRendering()
        {
            this.RaiseFinishedRendering(new EventArgs());
        }

        /// <summary>Raises the <see cref="FinishedRendering" /> event</summary>
        /// <param name="ea">EventArgs for this event</param>
        protected void RaiseFinishedRendering(EventArgs ea)
        {
            if (this.finishedRendering != null)
                this.finishedRendering(this, ea);
        }

        /// <summary>Raises the <see cref="RequestAdditionalData" /> event</summary>
        protected void RaiseRequestAdditionalData()
        {
            this.RaiseRequestAdditionalData(new EventArgs());
        }

        /// <summary>Raises the <see cref="RequestAdditionalData" /> event</summary>
        /// <param name="ea">EventArgs for this event</param>
        protected void RaiseRequestAdditionalData(EventArgs ea)
        {
            if (this.requestAdditionalData != null)
                this.requestAdditionalData(this, ea);
        }
        #endregion
    }
}