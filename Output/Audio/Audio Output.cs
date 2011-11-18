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
        /// <returns>An Int64 usable to re-refernce the playback object</returns>
        /// <remarks>This methods assumes that the playback will be re-referenced.</remarks>
        public abstract Int32 CreatePlayback(WaveFormatEx inputData);

        /// <summary>Submit data for immediate playback to the playback device, indicating whether or not it should finalize after playback</summary>
        /// <param name="data">Data to submit</param>
        /// <param name="destination">Source to submit data to</param>
        /// <param name="destination">Destination to output submission to</param>
        /// <param name="expectMore">Flag indicating whether to expect more data</param>
        public abstract void SubmitData(Byte[] data, Int32 source, Int32 destination, Boolean expectMore);

        /// <summary>Adds an event handler to request further data</summary>
        /// <param name="sourcKey">Source key of object to add data to</param>
        /// <param name="handler">Delegate to add more data</param>
        public abstract void AddSourceNeedDataEventhandler(Int32 sourcKey, AudioNeedsMoreDataHandler handler);
    }
}