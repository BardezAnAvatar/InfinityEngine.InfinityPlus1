using System;

using Bardez.Projects.Win32.Audio;

namespace Bardez.Projects.InfinityPlus1.Output.Audio
{
    /// <summary>Abstract class meant to provide a basis for methods and members for base audio ouput.</summary>
    /// <remarks>Intended to write an OpenAL and a DirectX child class for each</remarks>
    public abstract class AudioOutput
    {
        /// <summary>Creates an instance of the underlying/associated audio type and returns a unique identifier for re-reference</summary>
        /// <param name="inputData">PCM info describing the source</param>
        /// <param name="rendererIndex">Index to the rendering object this voice will supply</param>
        /// <returns>An Int32 usable to re-reference the playback object</returns>
        /// <remarks>This methods assumes that the playback will be re-referenced.</remarks>
        public abstract Int32 CreatePlayback(WaveFormatEx inputData, Int32 rendererIndex);

        /// <summary>Creates an audio rendering target</summary>
        /// <param name="inputFormat">WaveFormatEx for output</param>
        /// <returns>An Int32 usable to re-reference the playback rendering object</returns>
        public abstract Int32 CreateRenderer(WaveFormatEx inputFormat);

        /// <summary>Gets the default audio renderer</summary>
        /// <returns>An Int32 usable to re-reference the default playback rendering object</returns>
        public abstract Int32 GetDefaultRenderer();

        /// <summary>Disposes of the playback mechanism created by <see cref="CreatePlayback"/> and referenced by the value it returned</summary>
        /// <param name="playbackSource">Integer key created by <see cref="CreatePlayback"/></param>
        public abstract void DisposePlayback(Int32 playbackSource);

        /// <summary>Submit data for playback to the playback device, indicating whether or not it should finalize after this buffer and whether or not to play immediately</summary>
        /// <param name="data">Data to submit</param>
        /// <param name="source">Source to submit data to</param>
        /// <param name="expectMore">Flag indicating whether to expect more data</param>
        /// <param name="startPlayback">Flag indicating whether start playback</param>
        public abstract void SubmitData(Byte[] data, Int32 source, Boolean expectMore = false, Boolean startPlayback = true);

        /// <summary>Adds an event handler to request further data</summary>
        /// <param name="sourcKey">Source key of object to add data to</param>
        /// <param name="handler">Delegate to add more data</param>
        public abstract void AddSourceNeedsDataEventHandler(Int32 sourcKey, Action handler);

        /// <summary>Indicates whether or not the instance currently has a connection to a NeedMoreSampleData handler</summary>
        /// <param name="sourceKey">source key to a audio source</param>
        /// <returns>A Flag indicating whether the event handler has been set</returns>
        public abstract Boolean HasSourceNeedsDataEventHandler(Int32 sourceKey);
    }
}