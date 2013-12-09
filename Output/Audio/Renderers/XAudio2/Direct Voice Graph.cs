using System;

using Bardez.Projects.BasicStructures.Win32;
using Bardez.Projects.BasicStructures.Win32.Audio;
using Bardez.Projects.Multimedia.MediaBase.Render.Audio;
using Bardez.Projects.DirectX.X3DAudio;
using Bardez.Projects.DirectX.XAudio2;
using Bardez.Projects.DirectX.XAudio2.XAPO;

namespace Bardez.Projects.InfinityPlus1.Output.Audio.Renderers.XAudio2
{
    /// <summary>Represents the Voice graph for a renderer that has a direct path and no reverberation path</summary>
    public class DirectVoiceGraph : IVoiceGraph
    {
        #region Fields
        /// <summary>Instance of the XAudio2 interface to access</summary>
        protected XAudio2Interface XAudio2;

        /// <summary>XAudio2 source voice</summary>
        protected SourceVoice SourceVoice;

        /// <summary>XAudio2 mastering voice</summary>
        protected MasteringVoice MasteringVoice;

        /// <summary>Instance of the X3DAudio data & methods</summary>
        protected X3DAudio X3DAudio;

        /// <summary>Contains the details of the audio format</summary>
        protected WaveFormatEx AudioFormat;

        /// <summary>Explains the expected style of behavior for this audio renderer</summary>
        protected AudioRenderStyle Behavior;

        /// <summary>Destination speaker configuration</summary>
        protected SpeakerConfiguration OutputSpeakerConfiguration;

        /// <summary>The last <see cref="FilterParameter" /> set from Source Voice to Mastering Voice</summary>
        protected FilterParameter DirectPathFilterParameter;
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


        #region Exposed Events
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


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="sourceAudioFormat">Format of the source audio</param>
        /// <param name="renderingDeviceName">Name of the target device to be rendered to. If null or not found, system will use the system default device, if appropriate</param>
        /// <param name="behavior">Behavior with which to render audio</param>
        public DirectVoiceGraph(WaveFormatEx sourceAudioFormat, String renderingDeviceName, AudioRenderStyle behavior)
        {
            this.SourceVoice = null;
            this.MasteringVoice = null;
            this.X3DAudio = null;

            if (sourceAudioFormat == null)
                throw new NullReferenceException("The parameter \"audioInfo\" was null.");

            //store the input audio format
            this.AudioFormat = sourceAudioFormat;
            this.Behavior = behavior;

            //initialize XAudio2
            this.XAudio2 = XAudio2Interface.NewInstance();

            //go through devices to find the target
            UInt32 deviceCount = this.XAudio2.GetDeviceCount();

            DeviceDetails[] devices = new DeviceDetails[deviceCount];
            for (UInt32 i = 0; i < deviceCount; ++i)
                devices[i] = this.XAudio2.GetDeviceDetails(i);

            UInt32 renderingDeviceIndex = 0;
            for (UInt32 i = 0U; i < deviceCount; ++i)
                if (devices[i] != null && devices[i].DisplayName == renderingDeviceName)
                    renderingDeviceIndex = i;

            DeviceDetails renderDevice = devices[renderingDeviceIndex];
            this.OutputSpeakerConfiguration = new SpeakerConfiguration(renderDevice.OutputFormat.NumberChannels, renderDevice.OutputFormat.ChannelMask);

            //initialize the voices for rendering
            this.MasteringVoice = this.XAudio2.CreateMasteringVoice(this.AudioFormat.NumberChannels, this.AudioFormat.SamplesPerSec, 0U /* no SRC crashes, don't know why */, renderingDeviceIndex);
            this.SourceVoice = this.XAudio2.CreateSourceVoice(this.AudioFormat);
            this.SourceVoice.SetOutputVoices(new VoiceSendDescriptor[] { new VoiceSendDescriptor(0U, this.MasteringVoice) });   //render from source to both submix and to mastering
        }
        #endregion


        #region IDispose methods
        /// <summary>Disposal method</summary>
        public void Dispose()
        {
            if (this.SourceVoice != null)
            {
                this.StopSource(0U, 0U);
                this.SourceVoice.Dispose();
                this.SourceVoice = null;
            }

            if (this.MasteringVoice != null)
            {
                this.MasteringVoice.Dispose();
                this.MasteringVoice = null;
            }

            if (this.X3DAudio != null)
                this.X3DAudio = null;

            if (this.XAudio2 != null)
            {
                this.XAudio2.Dispose();
                this.XAudio2 = null;
            }
        }
        #endregion


        #region IVoiceGraph methods
        /// <summary>Sets details for the rendering context (such as 3D coordinates and so on)</summary>
        /// <param name="detail">Rendering context details to set</param>
        public void SetGraphRenderDetails(AudioSourceParams detail)
        {
            if (detail == null || detail.Emitter == null || detail.Listener == null)
            {
                //Doppler:
                //  Set Source Voice's Output Matrix calculations to 1.0f, to match the source channel mapping
                //  Set Source Voice's frequency ratio to 1.0F
                this.SourceVoice.SetOutputMatrix(this.MasteringVoice, this.AudioFormat.NumberChannels, this.OutputSpeakerConfiguration.Count, DspSettings.GenerateCoefficientMatrix(this.AudioFormat.NumberChannels, this.OutputSpeakerConfiguration.Count), 0U);

                //Matrix:   Implemented in other flags' settings

                //Reverberation:
                //  Set Submix Voice's Output Matrix calculations to 1.0f, to match the source channel mapping
                //this.SourceVoice.SetOutputMatrix(this.SubmixVoice, this.AudioFormat.NumberChannels, this.OutputSpeakerConfiguration.Count, DspSettings.GenerateCoefficientMatrix(this.AudioFormat.NumberChannels, this.OutputSpeakerConfiguration.Count), 0U);

                //Low-pass filter Direct:
                //  Set Matering Voice's Output Filter Parameters equal to values 1.0F, 1.0F and type low-pass filter
                //  per MSDN (http://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.xaudio2.xaudio2_filter_parameters%28v=vs.85%29.aspx)
                //  this "is acoustically equivalent to the filter being fully bypassed."
                this.DirectPathFilterParameter = new FilterParameter(FilterTypes.LowPassFilter, 1.0F, 1.0F);
                this.SourceVoice.SetOutputFilterParameters(this.MasteringVoice, this.DirectPathFilterParameter, 0U);

                //Low-pass filter Reverb:
                //  Set Submix Voice's Output Filter Parameters equal to values 1.0F, 1.0F and type low-pass filter
                //  per MSDN (http://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.xaudio2.xaudio2_filter_parameters%28v=vs.85%29.aspx)
                //  this "is acoustically equivalent to the filter being fully bypassed."
                //this.ReverbPathFilterParameter = new FilterParameter(FilterTypes.LowPassFilter, 1.0F, 1.0F);
                //this.SourceVoice.SetOutputFilterParameters(this.SubmixVoice, this.DirectPathFilterParameter, 0U);

                //TODO: how to disable the reverb?
            }
            else
            {
                this.X3DAudio = new X3DAudio((UInt32)(this.OutputSpeakerConfiguration.Positions));

                DirectX.X3DAudio.Listener x3daListener = new DirectX.X3DAudio.Listener(detail.Listener);
                DirectX.X3DAudio.Emitter x3daEmitter = new DirectX.X3DAudio.Emitter(detail.Emitter);

                DspSettings settings = new DspSettings(this.AudioFormat.NumberChannels, this.OutputSpeakerConfiguration.Count);

                X3DAudioCalculationFlags calcFlags = X3DAudioCalculationFlags.Matrix | X3DAudioCalculationFlags.Reverberation | X3DAudioCalculationFlags.Doppler | X3DAudioCalculationFlags.LowPassFilterDirect | X3DAudioCalculationFlags.LowPassFilterReverberation;
                this.X3DAudio.CalculateAudio(x3daListener, x3daEmitter, calcFlags, settings);

                //X3DAudio effects:
                this.SourceVoice.SetOutputMatrix(this.MasteringVoice, this.AudioFormat.NumberChannels, this.OutputSpeakerConfiguration.Count, settings.CoefficientsMatrix, 0);
                this.SourceVoice.SetFrequencyRatio(settings.DopplerFactor, 0);
            }
        }
        
        /// <summary>Gets the source voice's Voice State</summary>
        /// <returns>The VoiceState of the Source Voice</returns>
        public VoiceState GetSourceState()
        {
            return this.SourceVoice.GetState();
        }

        /// <summary>Submits data for rendering</summary>
        /// <param name="data">Audio samples to render</param>
        public ResultCode SubmitSampleData(Byte[] data)
        {
            AudioBuffer buffer = new AudioBuffer(XAudio2Interface.VoiceFlags.EndOfStream, data, 0, 0, 0, 0, 0, IntPtr.Zero);

            if (this.Behavior == AudioRenderStyle.Loop)
            {
                buffer.LoopLength = Convert.ToUInt32(data.Length);
                buffer.LoopCount = AudioBuffer.XAUDIO2_LOOP_INFINITE;
            }

            return this.SourceVoice.SubmitSourceBuffer(buffer, null);
        }

        /// <summary>Sets the volume of the source voice</summary>
        /// <param name="volume">Percentage of the volume to set</param>
        /// <returns>The system result code of the operation</returns>
        public ResultCode SetVolumeSource(Single volume)
        {
            return this.SourceVoice.SetVolume(volume, 0U);
        }

        /// <summary>Starts the rendering of the source voice</summary>
        /// <returns>The system result code of the operation</returns>
        public ResultCode StartSource()
        {
            return StartSource(0U, 0U);
        }

        /// <summary>Starts the rendering of the source voice</summary>
        /// <param name="flags">Any flags to the render</param>
        /// <param name="operationSet">Operation set for opertion queueing</param>
        /// <returns>The system result code of the operation</returns>
        public ResultCode StartSource(UInt32 flags, UInt32 operationSet)
        {
            //error checking
            if (this.SourceVoice == null)
                throw new NullReferenceException("The Source Voice for this renderer is null.");

            return this.SourceVoice.Start(flags, operationSet);
        }

        /// <summary>
        ///     Stops consumption and processing of audio by this voice. Delivers the result
        ///     to any connected submix or mastering voices, or to the output device.
        /// </summary>
        /// <param name="flags">Flags that control how the voice is started. Must be 0.</param>
        /// <param name="operationSet">Operation set of the effect (XAUDIO2_COMMIT_NOW == 0?), identifiying a batch</param>
        /// <returns>S_OK on success, otherwise an error code.</returns>
        public ResultCode StopSource(UInt32 flags, UInt32 operationSet)
        {
            return this.SourceVoice.Stop(flags, operationSet);
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