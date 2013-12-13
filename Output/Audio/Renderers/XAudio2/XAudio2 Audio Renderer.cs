using System;
using System.Linq;

using Bardez.Projects.BasicStructures.ThreeDimensional;
using Bardez.Projects.BasicStructures.Win32;
using Bardez.Projects.BasicStructures.Win32.Audio;
using Bardez.Projects.Multimedia.MediaBase.Render.Audio;
using Bardez.Projects.DirectX.X3DAudio;
using Bardez.Projects.DirectX.XAudio2;
using Bardez.Projects.DirectX.XAudio2.XAPO;

namespace Bardez.Projects.InfinityPlus1.Output.Audio.Renderers.XAudio2
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
        /// <summary>Instance of a VoiceGraph that will render the sound with specified effects.</summary>
        protected IVoiceGraph VoiceGraph;
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
            this.VoiceGraph = null;
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
            if (this.VoiceGraph != null)
            {
                this.VoiceGraph.Dispose();
                this.VoiceGraph = null;
            }
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
                VoiceState state = this.VoiceGraph.GetSourceState();
                return state.BuffersQueued < SourceVoice.MaximumBuffersQueued;
            }
        }
        #endregion


        #region IAudioRenderer Methods
        /// <summary>Initializes this audio renderer to use the settings provided</summary>
        /// <param name="audioInfo">Collection of data that indicates what the format of the source audio is</param>
        /// <param name="destinationConfiguration">Configuration of audio channels for rendering output</param>
        /// <param name="behavior">Behavior with which to render audio</param>
        /// <param name="reverb">Settings for the reverberation effect, if any</param>
        /// <param name="targetDeviceName">Name of the target device to be rendered to. If null or not found, system will use the system default device, if appropriate</param>
        public void Initialize(WaveFormatEx audioInfo, SpeakerConfiguration destinationConfiguration, AudioRenderStyle behavior, ReverbSettings reverb, String targetDeviceName)
        {
            if (this.VoiceGraph != null)
                throw new InvalidOperationException("This audio renderer is already Initialized.");

            if (reverb == null)
                this.VoiceGraph = new DirectVoiceGraph(audioInfo, targetDeviceName, behavior);
            else
                this.VoiceGraph = new ReverbVoiceGraph(audioInfo, targetDeviceName, behavior, reverb);

            this.VoiceGraph.FinishedRendering += this.RaiseFinishedRendering;
            this.VoiceGraph.RequestAdditionalData += this.RaiseRequestAdditionalData;
        }

        /// <summary>Submits data for rendering</summary>
        /// <param name="data">Audio samples to render</param>
        public void SubmitSampleData(Byte[] data)
        {
            this.VoiceGraph.SubmitSampleData(data);
        }

        /// <summary>Command to start rendering audio</summary>
        public void StartRendering()
        {
            this.VoiceGraph.StartSource(0U, 0U);
        }

        /// <summary>Sets details for the rendering context (such as 3D coordinates and so on)</summary>
        /// <param name="detail">Rendering context details to set</param>
        public void SetRenderDetails(AudioSourceParams detail)
        {
            this.VoiceGraph.SetGraphRenderDetails(detail);
        }

        /// <summary>Pauses audio playback. Buffers will pick up where left off if rendering resumed</summary>
        /// <param name="finishEffects">Flag to indicate whether any effects should render (e.g.: echo) [true] or if it should halt completely [false]</param>
        public void Pause(Boolean finishEffects)
        {
            this.VoiceGraph.StopSource(0U, 0U);
        }
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

        /// <summary>Raises the <see cref="FinishedRendering" /> event</summary>
        /// <param name="source">Source object raising the event</param>
        /// <param name="ea">EventArgs for this event</param>
        protected void RaiseFinishedRendering(Object source, EventArgs ea)
        {
            if (this.finishedRendering != null)
                this.finishedRendering(source, ea);
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

        /// <summary>Raises the <see cref="RequestAdditionalData" /> event</summary>
        /// <param name="source">Source object raising the event</param>
        /// <param name="ea">EventArgs for this event</param>
        protected void RaiseRequestAdditionalData(Object source, EventArgs ea)
        {
            if (this.requestAdditionalData != null)
                this.requestAdditionalData(source, ea);
        }
        #endregion
    }
}