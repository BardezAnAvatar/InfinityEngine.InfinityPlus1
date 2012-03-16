using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Timers;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Coding;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Wave.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels;
using Bardez.Projects.Win32.Audio;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management
{
    /// <summary>Represents a basic management class for MVE files</summary>
    /// <remarks>
    ///     This class will be used to open, read, seek, expose and decode a single MVE fles' data.
    ///     It will be designed to expose decoded data in one thread, to seek ahead and read opcodes in another, and to decode ahead of requests in a third.
    ///     My intent is to read the chunk index, then seek through initial opcodes to retrieve Timer, InitializeAudio and InitializeVideo stream info.
    ///     Framecount (and thus play time) are unknown until opcodes are fully read.
    ///     
    ///     Questions: what do I do when I encounter the following situations that I already know occur?
    ///     Duplicate (thankfully redundant) timer
    ///     Redefined audio/video buffers
    ///     
    ///     Answer: For now, skip them.
    /// </remarks>
    public class MveManager : IMovie
    {
        #region Fields
        /// <summary>Represents an index for data in the MVE file, to read from</summary>
        protected MveChunkOpcodeIndexer MveIndex { get; set; }

        /// <summary>Audio channel parameters</summary>
        protected MveAudioChannelParams AudioParameters { get; set; }

        /// <summary>Collection of collection of audio channel data, 16 in total</summary>
        protected IList<IList<AudioStream>> AudioChannels { get; set; }

        /// <summary>Parameters for a frame rate timer</summary>
        protected MveTimerParams FrameRateTimer { get; set; }

        /// <summary>Parameters for the video stream</summary>
        protected MveVideoParams VideoStreamParameters { get; set; }

        /// <summary>Represents the stream of video opcodes</summary>
        protected VideoOpcodeStream VideoStream { get; set; }

        /// <summary>Represents the video coding object for this MVE</summary>
        protected VideoCodingBase VideoCoder { get; set; }

        /// <summary>Palette for the video</summary>
        protected SetPalette Palette { get; set; }

        /// <summary>Represents the audio coding object for this MVE</summary>
        protected AudioCoding AudioCoder { get; set; }

        /// <summary>Cache list that allows for the blocks to be decoded prior to access, but not slowing down playback</summary>
        protected List<List<Byte[]>> AudioBlockCache { get; set; }

        /// <summary>Locking Object reference for the audio cache</summary>
        private Object audioCacheLock;

        /// <summary>Timer for MVE playback</summary>
        protected System.Timers.Timer timer { get; set; }

        /// <summary>Exposes the event for timer elapse</summary>
        protected event Action timerElapsed;

        /// <summary>Locking Object reference for the video decoder</summary>
        private Object videoLock;
        #endregion


        #region Events
        /// <summary>Exposes the event for timer elapse</summary>
        public event Action TimerElapsed
        {
            add { this.timerElapsed += value; }
            remove { this.timerElapsed -= value; }
        }
        #endregion


        #region Properties
        /// <summary>Exposes the length of the video file</summary>
        public virtual TimeSpan VideoLength
        {
            get
            {
                Double ticks = (Convert.ToDouble(this.FrameRateTimer.Denominator * this.VideoStream.Count) / Convert.ToDouble(this.FrameRateTimer.Numerator)) * 10000000.0; //microseconds = 10^-6, I want 10^-7
                return new TimeSpan(Convert.ToInt64(ticks));
            }
        }

        /// <summary>Exposes a WaveFormatEx instance for the appropriate audio parameters</summary>
        public virtual WaveFormatEx WaveFormat
        {
            get  { return this.GetWaveFormat(); }
        }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="indexedMve">Partially read, index MVE</param>
        public MveManager(MveChunkOpcodeIndexer indexedMve)
        {
            this.InitializeManager(indexedMve);
        }

        public virtual void InitializeManager(MveChunkOpcodeIndexer indexedMve)
        {
            this.MveIndex = indexedMve;
            this.AudioParameters = new MveAudioChannelParams();
            this.InitializeAudioChannels();
            this.FrameRateTimer = new MveTimerParams();
            this.VideoStreamParameters = new MveVideoParams();
            this.VideoStream = new VideoOpcodeStream();

            //video timer
            this.timer = new System.Timers.Timer();
            this.timer.Elapsed += new ElapsedEventHandler(this.VideoTimerExpired);

            //caching
            this.audioCacheLock = new Object();
            this.videoLock = new Object();
        }

        /// <summary>Initializes the cound of audio streams and the channels for each stream</summary>
        protected virtual void InitializeAudioChannels()
        {
            this.AudioChannels = new IList<AudioStream>[16];    // 16 channels of audio
            for (Int32 i = 0; i < 16; ++i)
                this.AudioChannels[i] = new List<AudioStream>();
        }
        #endregion


        #region Destruction
        /// <summary>Finalize destructor method</summary>
        ~MveManager()
        {
            this.StopVideoPlayback();
            this.ClearTimerElapsed();
        }
        #endregion


        #region Data Population
        /// <summary>Systematically loops through opcodes looking at each opcode and assigning its reference to an appropriate spot in this manager.</summary>
        public virtual void CollectOpcodeIndex()
        {
            this.CollectOpcodesComposite();
        }

        /// <summary>Initializes the coder objects</summary>
        public virtual void InitializeCoders()
        {
            this.AudioCoder = new AudioCoding(this.AudioParameters.Channels);

            switch (this.VideoStreamParameters.BitsPerPixel)
            {
                case 8:
                    this.VideoCoder = new VideoCoding8Bit(this.VideoStreamParameters.Width * 8, this.VideoStreamParameters.Height * 8, this.VideoStream);
                    break;
                case 16:
                    this.VideoCoder = new VideoCoding16Bit(this.VideoStreamParameters.Width * 8, this.VideoStreamParameters.Height * 8, this.VideoStream);
                    break;
                default:
                    throw new ApplicationException("Member \"VideoStreamParameters.BitsPerPixel\" was neither 8 nor 16.");
            }
        }

        /// <summary>Loops through indexed opcodes, storing the most recently found opcodes used to decode the video</summary>
        protected virtual void CollectOpcodesComposite()
        {
            foreach (ChunkIndex chunk in this.MveIndex.ChunkIndexCollection)
                foreach (Opcode opcode in chunk.Opcodes)
                {
                    switch (opcode.Operation)
                    {
                        case OpcodeVersions.InitializeAudioBuffers0:
                        case OpcodeVersions.InitializeAudioBuffers1:
                            InitializeAudioBuffers iab = opcode.Data as InitializeAudioBuffers;
                            this.AudioParameters.Channels = iab.Channels;
                            this.AudioParameters.Compressed = iab.Compressed;
                            this.AudioParameters.SampleRate = iab.SampleRate;
                            this.AudioParameters.SampleSize = iab.SampleSize;
                            break;
                        case OpcodeVersions.CreateTimer:
                            CreateTimer ct = opcode.Data as CreateTimer;
                            this.FrameRateTimer.Denominator = ct.Denominator;
                            this.FrameRateTimer.Numerator = ct.Numerator;
                            break;
                        //2 cases are unused
                        //case OpcodeVersions.InitializeVideoBuffers0:
                        //case OpcodeVersions.InitializeVideoBuffers1:
                        case OpcodeVersions.InitializeVideoBuffers2:
                            InitializeVideoBuffers ivb = opcode.Data as InitializeVideoBuffers2;
                            this.VideoStreamParameters.Height = ivb.Height;
                            this.VideoStreamParameters.Width = ivb.Width;
                            this.VideoStreamParameters.BitsPerPixel = ((ivb.TrueColor & 1) == 1 ? 16 : 8);
                            break;
                        case OpcodeVersions.AudioSilence:
                        case OpcodeVersions.AudioSamples:
                            AudioStream astrm = opcode.Data as AudioStream;

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

                            break;
                        case OpcodeVersions.SetPalette:
                            this.VideoStream.AddOpcodeData(opcode.Data as SetPalette);
                            break;
                        case OpcodeVersions.SetDecodingMap:
                            this.VideoStream.AddOpcodeData(opcode.Data as SetDecodingMap);
                            break;
                        case OpcodeVersions.VideoData:
                            this.VideoStream.AddOpcodeData(opcode.Data as VideoData);
                            break;
                    }
                }
        }
        #endregion


        #region Data Read
        /// <summary>Reads the data from the input stream into the storage opcodes indexed by the manager</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void ReadData(Stream input)
        {
            foreach (IList<AudioStream> stream in this.AudioChannels)
                foreach (AudioStream block in stream)
                    block.ReadData(input);

            this.VideoStream.ReadData(input);
        }
        #endregion


        #region Audio Data Retrieval
        /// <summary>Decoding launcher to store blocks in cache. Starts a background thread.</summary>
        public virtual void StartDecodingAudio()
        {
            Thread decoder = new Thread(() => this.CacheAudioData());    //lambda
            decoder.IsBackground = true;
            decoder.Start();
        }

        /// <summary>Decoding method to store blocks in a cache. Meant for a background thread.s</summary>
        protected virtual void CacheAudioData()
        {
            this.AudioBlockCache = new List<List<Byte[]>>();

            for (Int32 index = 0; index < 16; ++index)
                this.AudioBlockCache.Add(new List<Byte[]>());

            for (Int32 index = 0; index < 16; ++index)
                for (Int32 block = 0; block < this.AudioChannels[index].Count; ++block)
                    lock (this.audioCacheLock)
                        this.AudioBlockCache[index].Add(this.FetchAudioBlock(block, index));
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
            lock (this.audioCacheLock)
                while (this.AudioBlockCache == null || this.AudioBlockCache[streamNumber] == null || this.AudioBlockCache[streamNumber].Count < blockNumber)
                    Thread.Sleep(80);

            lock (this.audioCacheLock)
                return this.AudioBlockCache[streamNumber][blockNumber];
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


        #region Video exposure
        /// <summary>Creates a playback timer for the MVE</summary>
        public virtual void StartVideoPlayback()
        {
            this.timer.Interval = this.FrameRateTimer.FrameInterval;
            this.timer.Start();
        }

        /// <summary>Event handler for expiration of the timer</summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Parameters for the event</param>
        protected virtual void VideoTimerExpired(Object sender, ElapsedEventArgs e)
        {
            lock (this.videoLock)
            {
                //check to see that anyone is still attached to this event
                if (this.timerElapsed != null)
                    this.timerElapsed();
            }
        }

        /// <summary>Stops video playback, if playing</summary>
        public virtual void StopVideoPlayback()
        {
            if (this.timer != null)
                this.timer.Stop();
        }

        /// <summary>Decodes the video maps from the SetDecodingMap opcodes</summary>
        public virtual void DecodeVideoMaps()
        {
            this.VideoStream.DecodeVideoMaps();
        }

        /// <summary>Fetches the next MediaBase Frame for output</summary>
        /// <returns>A MediaBase Frame for output</returns>
        public virtual Frame GetNextFrame()
        {
            return this.VideoCoder.GetNextFrame();
        }

        /// <summary>Resets the video stream to the beginning</summary>
        public virtual void ResetVideo()
        {
            this.VideoCoder.ResetVideo();
        }
        #endregion


        #region Event Control
        /// <summary>Control method to clear the TimerElapsed event delegates</summary>
        public virtual void ClearTimerElapsed()
        {
            this.timerElapsed = null;
        }
        #endregion
    }
}