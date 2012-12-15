using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Bardez.Projects.BasicStructures.Win32.Audio;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Coding;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Wave.Enums;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management
{
    /// <summary>This class manages the decoding and playback of MVE audio</summary>
    public class MveAudioManager : IWaveFormatEx
    {
        #region Fields
        /// <summary>Audio channel parameters</summary>
        public MveAudioChannelParams AudioParameters { get; set; }

        /// <summary>Collection of collection of audio channel data, 16 in total</summary>
        protected IList<IList<AudioStream>> AudioChannels { get; set; }

        /// <summary>Represents the audio coding object for this MVE</summary>
        protected AudioCoding AudioCoder { get; set; }

        /// <summary>Cache list that allows for the blocks to be decoded prior to access, but not slowing down playback</summary>
        protected Byte[][][] AudioBlockCache { get; set; }

        /// <summary>Represents the last requested audio stream number (0 by default)</summary>
        protected Int32 LastRequestedAudioStream { get; set; }

        /// <summary>Locking Object reference for the audio cache</summary>
        private Object audioCacheLock;

        /// <summary>Local event to raise to whatever processor that the audio stream has been stopped, and to cease/pause fetching audio data</summary>
        private event Action audioStreamStopped;
        #endregion


        #region Events
        /// <summary>Public-facing event indicating that the manager has received a stop command from the MVE stream</summary>
        public event Action AudioStreamStopped
        {
            add { this.audioStreamStopped += value; }
            remove { this.audioStreamStopped -= value; }
        }
        #endregion


        #region Properties
        /// <summary>Exposes a WaveFormatEx instance for the appropriate audio parameters</summary>
        public virtual WaveFormatEx WaveFormat
        {
            get { return this.GetWaveFormat(); }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public MveAudioManager()
        {
            this.InitializeManager();
        }

        /// <summary>Initializes the MVE audio manager</summary>
        public virtual void InitializeManager()
        {
            this.AudioParameters = new MveAudioChannelParams();
            this.InitializeAudioChannels();
            this.LastRequestedAudioStream = 0;  //default audio stream

            //locking objects
            this.audioCacheLock = new Object();
        }

        /// <summary>Initializes the cound of audio streams and the channels for each stream</summary>
        protected virtual void InitializeAudioChannels()
        {
            this.AudioChannels = new IList<AudioStream>[16];    // 16 channels of audio
            for (Int32 i = 0; i < 16; ++i)
                this.AudioChannels[i] = new List<AudioStream>();
        }
        #endregion


        #region Data Population
        /// <summary>Initializes the coder objects</summary>
        public virtual void InitializeCoder()
        {
            this.AudioCoder = new AudioCoding(this.AudioParameters.Channels);
        }

        /// <summary>Reads the data from the input stream into the storage opcodes indexed by the manager</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void ReadData(Stream input)
        {
            foreach (IList<AudioStream> stream in this.AudioChannels)
                foreach (AudioStream block in stream)
                    block.ReadData(input);
        }

        /// <summary>Adds audio samples to the appropriate location(s)</summary>
        /// <param name="astrm">AudioStream opcode to analyze</param>
        public virtual void AddAudioSamples(AudioStream astrm)
        {
            if ((astrm.StreamChannel & AudioStreamChannels.Channel00) == AudioStreamChannels.Channel00)
                this.AudioChannels[0].Add(astrm);
            if ((astrm.StreamChannel & AudioStreamChannels.Channel01) == AudioStreamChannels.Channel01)
                this.AudioChannels[1].Add(astrm);
            if ((astrm.StreamChannel & AudioStreamChannels.Channel02) == AudioStreamChannels.Channel02)
                this.AudioChannels[2].Add(astrm);
            if ((astrm.StreamChannel & AudioStreamChannels.Channel03) == AudioStreamChannels.Channel03)
                this.AudioChannels[3].Add(astrm);
            if ((astrm.StreamChannel & AudioStreamChannels.Channel04) == AudioStreamChannels.Channel04)
                this.AudioChannels[4].Add(astrm);
            if ((astrm.StreamChannel & AudioStreamChannels.Channel05) == AudioStreamChannels.Channel05)
                this.AudioChannels[5].Add(astrm);
            if ((astrm.StreamChannel & AudioStreamChannels.Channel06) == AudioStreamChannels.Channel06)
                this.AudioChannels[6].Add(astrm);
            if ((astrm.StreamChannel & AudioStreamChannels.Channel07) == AudioStreamChannels.Channel07)
                this.AudioChannels[7].Add(astrm);
            if ((astrm.StreamChannel & AudioStreamChannels.Channel08) == AudioStreamChannels.Channel08)
                this.AudioChannels[8].Add(astrm);
            if ((astrm.StreamChannel & AudioStreamChannels.Channel09) == AudioStreamChannels.Channel09)
                this.AudioChannels[9].Add(astrm);
            if ((astrm.StreamChannel & AudioStreamChannels.Channel10) == AudioStreamChannels.Channel10)
                this.AudioChannels[10].Add(astrm);
            if ((astrm.StreamChannel & AudioStreamChannels.Channel11) == AudioStreamChannels.Channel11)
                this.AudioChannels[11].Add(astrm);
            if ((astrm.StreamChannel & AudioStreamChannels.Channel12) == AudioStreamChannels.Channel12)
                this.AudioChannels[12].Add(astrm);
            if ((astrm.StreamChannel & AudioStreamChannels.Channel13) == AudioStreamChannels.Channel13)
                this.AudioChannels[13].Add(astrm);
            if ((astrm.StreamChannel & AudioStreamChannels.Channel14) == AudioStreamChannels.Channel14)
                this.AudioChannels[14].Add(astrm);
            if ((astrm.StreamChannel & AudioStreamChannels.Channel15) == AudioStreamChannels.Channel15)
                this.AudioChannels[15].Add(astrm);
        }
        #endregion


        #region Audio Data Retrieval
        /// <summary>Decoding launcher to store blocks in cache. Starts a background thread.</summary>
        public virtual void PreemptivelyStartDecodingAudio()
        {
            Thread decoder = new Thread(() => this.CacheAudioData());    //lambda
            decoder.IsBackground = true;
            decoder.Name = "MVE audio decoder";
            decoder.Start();
        }

        /// <summary>Decoding method to store blocks in a cache. Meant for a background thread.s</summary>
        protected virtual void CacheAudioData()
        {
            this.AudioBlockCache = new Byte[16][][];

            for (Int32 index = 0; index < 16; ++index)
            {
                this.AudioBlockCache[index] = new Byte[this.AudioChannels[index].Count][];
                for (Int32 block = 0; block < this.AudioChannels[index].Count; ++block)
                    this.AudioBlockCache[index][block] = this.FetchAudioBlock(block, index);
            }
        }

        /// <summary>Gets a block of audio from the appropriate data stream</summary>
        /// <param name="blockNumber">Audio block number to retrieve</param>
        /// <param name="streamNumber">Audio stream number to retrieve (0-15)</param>
        /// <returns>A decompressed block of sample data for</returns>
        /// <remarks>This is not fast enough to dispatch audio data</remarks>
        protected virtual Byte[] FetchAudioBlock(Int32 blockNumber, Int32 streamNumber)
        {
            AudioStream chunk = this.AudioChannels[streamNumber][blockNumber];
            Byte[] data = null;
            if (chunk is AudioStreamData)
            {
                AudioStreamData asd = chunk as AudioStreamData;

                if (this.AudioParameters.Compressed)
                    data = this.AudioCoder.DecodeAudioBlock(asd.Data, chunk.UncompressedSampleDataLength);
                else
                    data = asd.Data;
            }
            else
                data = new Byte[chunk.UncompressedSampleDataLength];
            
            return data;
        }

        /// <summary>Retrieves audio block from the data cache</summary>
        /// <param name="blockNumber">Block number to retrieve</param>
        /// <param name="streamNumber">Stream number to retrieve</param>
        /// <returns>Byte Array of samples</returns>
        public virtual Byte[] GetAudioBlock(Int32 blockNumber, Int32 streamNumber)
        {
            this.LastRequestedAudioStream = streamNumber;
            Byte[] block = null;

            if (blockNumber < this.AudioBlockCache[streamNumber].Length)
                block = this.AudioBlockCache[streamNumber][blockNumber];

            return block;
        }
        #endregion


        #region Audio Exposure
        /// <summary>Returns a WaveFormatEx instance from this header data</summary>
        /// <returns>A WaveFormatEx instance to submit to API calls</returns>
        public virtual WaveFormatEx GetWaveFormat()
        {
            WaveFormatEx waveFormat = new WaveFormatEx();
            waveFormat.NumberChannels = Convert.ToUInt16(this.AudioParameters.Channels);
            waveFormat.FormatTag = (UInt16)DataFormat.PCM;
            waveFormat.SamplesPerSec = Convert.ToUInt32(this.AudioParameters.SampleRate);
            waveFormat.BitsPerSample = Convert.ToUInt16(this.AudioParameters.SampleSize);
            waveFormat.BlockAlignment = Convert.ToUInt16((waveFormat.BitsPerSample / 8) * waveFormat.NumberChannels);
            waveFormat.AverageBytesPerSec = waveFormat.SamplesPerSec * waveFormat.BlockAlignment;
            waveFormat.Size = 0;    //no extra data; this is strictly a WaveFormatEx instance 

            return waveFormat;
        }

        /// <summary>Exposes the count of audio blocks in the specified stream.</summary>
        /// <param name="streamNumber">Stream to yield the count of.</param>
        /// <returns>The number of blocks in the stream.</returns>
        public virtual Int32 AudioBlockCount(Int32 streamNumber)
        {
            return this.AudioChannels[streamNumber].Count;
        }
        #endregion


        #region Event Control
        /// <summary>Event raising method for stopping audio</summary>
        protected virtual void RaiseStopAudio()
        {
            if (this.audioStreamStopped != null)
                this.audioStreamStopped();
        }
        #endregion


        #region Audio/Video timing helpers
        /// <summary>Gets the number of samples</summary>
        /// <param name="streamNumber">Channel number to retrieve the sample count of</param>
        /// <returns>Int32 specifying the total number of samples in the audio stream</returns>
        public virtual Int32 AudioStreamSampleCount(Int32 streamNumber)
        {
            Int32 sampleCount = 0;

            IList<AudioStream> stream = this.AudioChannels[streamNumber];
            foreach (AudioStream chunk in stream)
                sampleCount += chunk.UncompressedSampleDataLength;

            return sampleCount / (this.AudioParameters.SampleSize / 8);
        }

        /// <summary>Gets the TimeSpan length of audio data</summary>
        /// <param name="sampleCount">The count of samples to be evaluated</param>
        /// <returns>A TimeSpan representing the audio sample data length in time</returns>
        protected TimeSpan GetAudioLength(Int32 sampleCount)
        {
            Int32 milliseconds = (sampleCount * 1000) / this.AudioParameters.SampleRate;
            return new TimeSpan(0, 0, 0, 0, milliseconds);
        }

        /// <summary>Gets the TimeSpan length of audio data for a given stream</summary>
        /// <param name="streamNumber">Channel number to retrieve the sample count of</param>
        /// <returns>A TimeSpan representing the audio sample data length</returns>
        protected TimeSpan GetAudioStreamTimeLength(Int32 streamNumber)
        {
            return this.GetAudioLength(this.AudioStreamSampleCount(streamNumber));
        }

        /// <summary>Gets the end time of the MVE as a TimeSpan, taking into consideration the length of the audio and its start position</summary>
        /// <param name="streamNumber">Stream number to inquire about</param>
        /// <param name="start">Start time within the media stream to start playback</param>
        /// <returns>Timespan end code of the audio.</returns>
        public TimeSpan GetAudioEndTime(Int32 streamNumber, TimeSpan start)
        {
            TimeSpan length = this.GetAudioStreamTimeLength(streamNumber);
            return start + length;
        }

        /// <summary>Gets the end time of the MVE as a TimeSpan, taking into consideration the length of the audio and its start position</summary>
        /// <param name="streamNumber">Stream number to inquire about</param>
        /// <param name="start">Start time within the media stream to start playback</param>
        /// <returns>Timespan end code of the audio.</returns>
        public TimeSpan GetAudioEndTime(TimeSpan start)
        {
            TimeSpan length = this.GetAudioStreamTimeLength(this.LastRequestedAudioStream);
            return start + length;
        }
        #endregion
    }
}