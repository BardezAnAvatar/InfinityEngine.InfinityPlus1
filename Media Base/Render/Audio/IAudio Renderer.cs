﻿using System;

using Bardez.Projects.BasicStructures.Win32.Audio;

namespace Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Render.Audio
{
    /// <summary>This interface defines commands to receive and process audio data</summary>
    /// <remarks>
    ///     Defined behavior of this interface is to have an engine that can render audio samples.
    ///     Samples should be processed in the order that they are received (to the best of locking ability).
    ///         * Samples should not be joined into a larger set. They should be maintained in a collection and
    ///           processed in order accordingly.
    ///     When all data received has been processed, raise the event <see cref="RequestAdditionalData" /> if streaming.
    ///     When all data received has been processed, or the renderer has been aborted, raise the event <see cref="FinishedRendering" />.
    ///     There will be two rendering modes:
    ///         1) All submitted data is processed and then rendering stops. This shall be known as <see cref="AudioRenderStyle.PreBuffered" />
    ///         2) All submitted data is processed and then renderer asks for additional data. This shall be known as <see cref="AudioRenderStyle.Streaming" />
    ///     When aborted, regardless of the rendering mode, all rendering of data is to cease.
    ///         1) If realtime (XAudio2, OpenAL), this abort should interrupt all playback streams
    ///         2) If processing (disk writer, etc.), this abort should interrupt after the current sample set consumption.
    /// </remarks>
    public interface IAudioRenderer : IDisposable
    {
        /// <summary>Event that occurs when the renderer has finished rendering the provided data stream(s). Used to signal expiration and ready for disposal.</summary>
        event EventHandler FinishedRendering;

        /// <summary>
        ///     Event that occurs when the renderer has processed all provided data and requires additional data
        ///     if the behavior specified is to continue rendering
        /// </summary>
        event EventHandler RequestAdditionalData;

        /// <summary>Initializes this audio renderer to use the settings provided</summary>
        /// <param name="audioInfo">Collection of data that indicates what the format of the source audio is</param>
        void Initialize(WaveFormatEx audioInfo);

        /// <summary>Exposes a flag indicating whether or not the renderer can accept new data</summary>
        Boolean CanAcceptSampleData { get; }

        /// <summary>Submits data for rendering</summary>
        /// <param name="data">Audio samples to render</param>
        void SubmitSampleData(Byte[] data);

        void StartRendering(AudioRenderStyle behavior);

        void SetRenderDetails(AudioSourceParams detail);

        /// <summary>Pauses audio playback. Buffers will pick up where left off if rendering resumed</summary>
        /// <param name="finishEffects">Flag to indicate whether any effects should render (e.g.: echo) [true] or if it should halt completely [false]</param>
        void Pause(Boolean finishEffects);
    }

    /// <summary>Public enumeration of styles of rendering audio</summary>
    public enum AudioRenderStyle
    {
        /// <summary>Expects chunks of data to be fed to it until it is told to stop playing</summary>
        Streaming,

        /// <summary>Expects to process all queued chunks of data and then stop</summary>
        PreBuffered,

        /// <summary>Expects to be fed an existing set of buffers and will loop on what has been set</summary>
        Loop,
    }
}