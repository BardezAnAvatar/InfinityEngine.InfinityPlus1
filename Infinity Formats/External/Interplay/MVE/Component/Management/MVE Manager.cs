using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;

using Bardez.Projects.BasicStructures.Win32.Audio;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Coding;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Interpretation;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Wave.Enums;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.Multimedia.MediaBase.Frame.Video;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management
{
    /// <summary>Represents a basic management class for MVE files</summary>
    /// <remarks>
    ///     This class will be used to open, read, seek, expose and decode a single MVE file's data.
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
    //TODO: Change the first-indexing approch to a streaming approach
    public class MveManager : IMovie, IDisposable
    {
        #region Fields
        /// <summary>Represents an index for data in the MVE file, to read from</summary>
        protected MveChunkOpcodeIndexer MveIndex { get; set; }

        /// <summary>Managing class for the MVE video stream</summary>
        protected MveVideoManager VideoManager { get; set; }

        /// <summary>Managing class for the MVE audio stream</summary>
        protected MveAudioManager AudioManager { get; set; }

        /// <summary>Exposes the event for timer elapse</summary>
        protected event Action<IMultimediaImageFrame> outputFrame;

        /// <summary>Local event to raise to whatever processor that the audio stream has been started, and to start fetching audio data</summary>
        private event Action audioStreamStarted;

        /// <summary>Local event to raise to whatever processor that the audio stream has been stopped, and to cease/pause fetching audio data</summary>
        private event Action audioStreamStopped;
        #endregion


        #region Events
        /// <summary>Exposes the event for timer elapse</summary>
        public event Action<IMultimediaImageFrame> PlayFrame
        {
            add { this.outputFrame += value; }
            remove { this.outputFrame -= value; }
        }

        /// <summary>Public-facing event indicating that the manager has received a start command from the MVE stream</summary>
        public event Action AudioStreamStarted
        {
            add { this.audioStreamStarted += value; }
            remove { this.audioStreamStarted -= value; }
        }

        /// <summary>Public-facing event indicating that the manager has received a stop command from the MVE stream</summary>
        public event Action AudioStreamStopped
        {
            add { this.audioStreamStopped += value; }
            remove { this.audioStreamStopped -= value; }
        }
        #endregion


        #region Properties
        /// <summary>Exposes the length of the video file</summary>
        public virtual TimeSpan VideoLength
        {
            get { return this.VideoManager.VideoLength; }
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
            this.InitializeManagers(indexedMve);
        }

        /// <summary>Initializes the MVE manager</summary>
        /// <param name="indexedMve">Indexed chunk opcode indexer containing reference to all opcodes</param>
        public virtual void InitializeManagers(MveChunkOpcodeIndexer indexedMve)
        {
            this.MveIndex = indexedMve;
            this.AudioManager = new MveAudioManager();
            this.VideoManager = new MveVideoManager();

            //assign event handlers
            this.VideoManager.AudioStreamStarted += this.RaiseStartAudio;
            this.VideoManager.PlayFrame += this.RaisePlayFrame;
            this.AudioManager.AudioStreamStopped += this.RaiseStopAudio;
        }
        #endregion


        #region Destruction
        /// <summary>Disposes unmanaged objects</summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>Disposes of managed and unmanaged objects</summary>
        /// <param name="disposingNotFinalizing">Flag indicating whether we are in the Finalize or the Dispose method</param>
        protected virtual void Dispose(Boolean disposingNotFinalizing)
        {
            if (this.VideoManager != null)
            {
                this.VideoManager.Dispose();
                this.VideoManager = null;
            }

            //clear events
            this.outputFrame = null;
            this.audioStreamStarted = null;
            this.audioStreamStopped = null;
        }

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
            this.AudioManager.InitializeCoder();
            this.VideoManager.InitializeVideoCoder();
        }

        /// <summary>Initializes the audio coder object</summary>
        public virtual void InitializeAudioCoder()
        {
            this.AudioManager.InitializeCoder();
        }

        /// <summary>Initializes the video coder object</summary>
        public virtual void InitializeVideoCoder()
        {
            this.VideoManager.InitializeVideoCoder();
        }

        /// <summary>Loops through indexed opcodes, storing the most recently found opcodes used to decode the video</summary>
        protected virtual void CollectOpcodesComposite()
        {
            VideoOpcodeStream vidOpcodeStream = new VideoOpcodeStream();
            Int32 frameNumber = 0;  //separate from chunkIndex

            for (Int32 chunkIndex = 0; chunkIndex < this.MveIndex.ChunkIndexCollection.Count; ++chunkIndex)
            {
                //opcodes to watch references for
                VideoData vidData = null;
                SetDecodingMap decodingBlockMap = null;
                
                foreach (Opcode opcode in this.MveIndex.ChunkIndexCollection[chunkIndex].Opcodes)
                {
                    switch (opcode.Operation)
                    {
                        case OpcodeVersions.InitializeAudioBuffers0:
                        case OpcodeVersions.InitializeAudioBuffers1:
                            vidOpcodeStream.AddChunk(new MveInitializeAudio());
                            this.CollectInitializeAudioBuffers(opcode.Data as InitializeAudioBuffers, frameNumber);
                            break;
                        case OpcodeVersions.CreateTimer:
                            CreateTimer ct = opcode.Data as CreateTimer;
                            this.VideoManager.FrameRateTimerParameters = new MveTimerParams(ct.Numerator, ct.Denominator);
                            break;
                        //2 cases are unused
                        //case OpcodeVersions.InitializeVideoBuffers0:
                        //case OpcodeVersions.InitializeVideoBuffers1:
                        case OpcodeVersions.InitializeVideoBuffers2:
                            InitializeVideoBuffers ivb = opcode.Data as InitializeVideoBuffers2;
                            this.VideoManager.VideoStreamParameters = new MveVideoParams(ivb.Width, ivb.Height, ((ivb.HighColor & 1) == 1 ? 16 : 8));
                            break;
                        case OpcodeVersions.AudioSilence:
                        case OpcodeVersions.AudioSamples:
                            AudioStream astrm = opcode.Data as AudioStream;
                            this.AudioManager.AddAudioSamples(astrm);
                            break;
                        case OpcodeVersions.SetPalette:
                            vidOpcodeStream.AddChunk(new MvePalette(opcode.Data as SetPalette));
                            break;
                        case OpcodeVersions.SetDecodingMap:
                            decodingBlockMap = opcode.Data as SetDecodingMap;
                            break;
                        case OpcodeVersions.VideoData:
                            vidData = opcode.Data as VideoData;
                            break;
                    }
                }

                //Add a frame?
                MveVideoFrame frame = null;
                if (vidData != null && decodingBlockMap != null)
                {
                    frame = new MveVideoFrame(decodingBlockMap, vidData, frameNumber);
                    vidOpcodeStream.AddChunk(frame);
                    ++frameNumber;
                }
            }

            this.VideoManager.VideoStream = vidOpcodeStream;
        }

        /// <summary>Shared logic for InitializeAudioBuffers0 and InitializeAudioBuffers1</summary>
        /// <param name="opcode">Opcode to pull information from</param>
        /// <param name="frameNumber">Number of the chunk where the opcode occurs</param>
        protected virtual void CollectInitializeAudioBuffers(InitializeAudioBuffers opcode, Int32 frameNumber)
        {
            this.AudioManager.AudioParameters.Channels = opcode.Channels;
            this.AudioManager.AudioParameters.Compressed = opcode.Compressed;
            this.AudioManager.AudioParameters.SampleRate = opcode.SampleRate;
            this.AudioManager.AudioParameters.SampleSize = opcode.SampleSize;

            if (this.AudioManager.AudioParameters.StartFrame == -1)
                this.AudioManager.AudioParameters.StartFrame = frameNumber;
        }
        #endregion


        #region Data Read
        /// <summary>Reads the data from the input stream into the storage opcodes indexed by the manager</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void ReadData(Stream input)
        {
            this.AudioManager.ReadData(input);
            this.VideoManager.ReadData(input);
        }

        /// <summary>Reads the audio data from the input stream into the storage opcodes indexed by the manager</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void ReadAudioData(Stream input)
        {
            this.AudioManager.ReadData(input);
        }

        /// <summary>Reads the video data from the input stream into the storage opcodes indexed by the manager</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void ReadVideoData(Stream input)
        {
            this.VideoManager.ReadData(input);
        }
        #endregion


        #region Audio Data Retrieval
        /// <summary>Decoding launcher to store blocks in cache. Starts a background thread.</summary>
        public virtual void PreemptivelyStartDecodingAudio()
        {
            this.AudioManager.PreemptivelyStartDecodingAudio();
        }

        /// <summary>Retrieves audio block from the data cache</summary>
        /// <param name="blockNumber">Block number to retrieve</param>
        /// <param name="streamNumber">Stream number to retrieve</param>
        /// <returns>Byte Array of samples</returns>
        public virtual Byte[] GetAudioBlock(Int32 blockNumber, Int32 streamNumber)
        {
            return this.AudioManager.GetAudioBlock(blockNumber, streamNumber);
        }
        #endregion


        #region Audio Exposure
        /// <summary>Returns a WaveFormatEx instance from this header data</summary>
        /// <returns>A WaveFormatEx instance to submit to API calls</returns>
        public virtual WaveFormatEx GetWaveFormat()
        {
            return this.AudioManager.GetWaveFormat();
        }

        /// <summary>Exposes the count of audio blocks in the specified stream.</summary>
        /// <param name="streamNumber">Stream to yield the count of.</param>
        /// <returns>The number of blocks in the stream.</returns>
        public virtual Int32 AudioBlockCount(Int32 streamNumber)
        {
            return this.AudioManager.AudioBlockCount(streamNumber);
        }
        #endregion


        #region Video exposure
        /// <summary>Creates a playback timer for the MVE</summary>
        public virtual void StartVideoPlayback()
        {
            this.VideoManager.StartVideoPlayback();
        }

        /// <summary>Stops video playback, if playing</summary>
        public virtual void StopVideoPlayback()
        {
            this.VideoManager.StopVideoPlayback();
        }

        /// <summary>Decodes the video maps from the SetDecodingMap opcodes</summary>
        public virtual void DecodeVideoMaps()
        {
            this.VideoManager.DecodeVideoMaps();
        }

        /// <summary>Fetches the next MediaBase Frame for output</summary>
        /// <returns>A MediaBase Frame for output</returns>
        public virtual IMultimediaImageFrame GetNextFrame()
        {
            return this.VideoManager.GetNextFrame();
        }

        /// <summary>Resets the video stream to the beginning</summary>
        public virtual void ResetVideo()
        {
            this.VideoManager.ResetVideo();
        }
        
        /// <summary>Decoding launcher to store frames in cache. Starts a background thread.</summary>
        public virtual void PreemptivelyStartDecodingVideo()
        {
            this.VideoManager.PreemptivelyStartDecodingVideo();
        }
        #endregion


        #region Event Control
        /// <summary>Control method to clear the TimerElapsed event delegates</summary>
        public virtual void ClearTimerElapsed()
        {
            this.outputFrame = null;
        }

        /// <summary>Event raising method for starting audio</summary>
        protected virtual void RaiseStartAudio()
        {
            if (this.audioStreamStarted != null)
                this.audioStreamStarted();
        }

        /// <summary>Event raising method for stopping audio</summary>
        protected virtual void RaiseStopAudio()
        {
            if (this.audioStreamStopped != null)
                this.audioStreamStopped();
        }

        /// <summary>Event raising method for displaying a frame</summary>
        protected virtual void RaisePlayFrame(IMultimediaImageFrame frame)
        {
            if (this.outputFrame != null)
                this.outputFrame(frame);
        }
        #endregion


        #region Audio/Video timing helpers
        /// <summary>Gets the end time of the MVE as a TimeSpan, taking into consideration the length of the audio and its start position</summary>
        /// <returns>Timespan end code of the audio.</returns>
        protected TimeSpan GetAudioEndTime()
        {
            return this.AudioManager.GetAudioEndTime(this.VideoManager.AudioStartTime);
        }

        /// <summary>Gets the end time span for video based on both video and audio</summary>
        /// <returns>Longer end TimeSpan</returns>
        protected TimeSpan GetMediaLength()
        {
            TimeSpan audio = this.GetAudioEndTime();
            TimeSpan video = this.VideoLength;
            return (audio < video ? video : audio);
        }
        #endregion
    }
}