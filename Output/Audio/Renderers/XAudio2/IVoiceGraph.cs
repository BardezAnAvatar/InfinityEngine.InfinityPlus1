using System;

using Bardez.Projects.BasicStructures.Win32;
using Bardez.Projects.Multimedia.MediaBase.Render.Audio;
using Bardez.Projects.DirectX.X3DAudio;
using Bardez.Projects.DirectX.XAudio2;
using Bardez.Projects.DirectX.XAudio2.XAPO;

namespace Bardez.Projects.InfinityPlus1.Output.Audio.Renderers.XAudio2
{
    /// <summary>This interface defines common methods to a voice graph</summary>
    public interface IVoiceGraph : IDisposable
    {
        #region Exposed Events
        /// <summary>Event that occurs when the renderer has finished rendering the provided data stream(s). Used to signal expiration and ready for disposal.</summary>
        event EventHandler FinishedRendering;

        /// <summary>
        ///     Event that occurs when the renderer has processed all provided data and requires additional data
        ///     if the behavior specified is to continue rendering
        /// </summary>
        event EventHandler RequestAdditionalData;
        #endregion


        #region Public methods
        /// <summary>Gets the source voice's Voice State</summary>
        /// <returns>The VoiceState of the Source Voice</returns>
        VoiceState GetSourceState();

        /// <summary>Sets details for the rendering context (such as 3D coordinates and so on)</summary>
        /// <param name="detail">Rendering context details to set</param>
        void SetGraphRenderDetails(AudioSourceParams detail);

        /// <summary>Submits data for rendering</summary>
        /// <param name="data">Audio samples to render</param>
        ResultCode SubmitSampleData(Byte[] data);

        /// <summary>Starts the rendering of the source voice</summary>
        /// <returns>The system result code of the operation</returns>
        ResultCode StartSource();

        /// <summary>Starts the rendering of the source voice</summary>
        /// <param name="flags">Any flags to the render</param>
        /// <param name="operationSet">Operation set for opertion queueing</param>
        /// <returns>The system result code of the operation</returns>
        ResultCode StartSource(UInt32 flags, UInt32 operationSet);

        /// <summary>
        ///     Stops consumption and processing of audio by this voice. Delivers the result
        ///     to any connected submix or mastering voices, or to the output device.
        /// </summary>
        /// <param name="flags">Flags that control how the voice is started. Must be 0.</param>
        /// <param name="operationSet">Operation set of the effect (XAUDIO2_COMMIT_NOW == 0?), identifiying a batch</param>
        /// <returns>S_OK on success, otherwise an error code.</returns>
        ResultCode StopSource(UInt32 flags, UInt32 operationSet);

        /// <summary>Sets the volume of the source voice</summary>
        /// <param name="volume">Percentage of the volume to set</param>
        /// <returns>The system result code of the operation</returns>
        ResultCode SetVolumeSource(Single volume);
        #endregion
    }
}