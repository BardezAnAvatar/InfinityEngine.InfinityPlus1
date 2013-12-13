using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Coding;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Interpretation;
using Bardez.Projects.InfinityPlus1.NativeFactories.Timer;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.Multimedia.MediaBase.Management;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management
{
    /// <summary>
    ///     This class will manage the video aspects of an MVE file. It will cache forward-decoded frames, retrieve cached frames
    ///     and allow retrieval on a frame-by frame and a playback basis.
    /// </summary>
    public class MveVideoManager : IDisposable
    {
        #region Fields
        /// <summary>Parameters for a frame rate timer</summary>
        public MveTimerParams FrameRateTimerParameters { get; set; }

        /// <summary>Parameters for the video stream</summary>
        public MveVideoParams VideoStreamParameters { get; set; }

        /// <summary>Represents the stream of video opcodes</summary>
        protected VideoOpcodeStream videoStream;

        /// <summary>Represents the video coding object for this MVE</summary>
        protected VideoCodingBase VideoCoder { get; set; }

        /// <summary>Timer for MVE playback</summary>
        protected ITimer timer { get; set; }

        /// <summary>Exposes the event for timer elapse</summary>
        protected event Action<IMultimediaImageFrame> timerElapsed;

        /// <summary>Local event to raise to whatever processor that the audio stream has been started, and to start fetching audio data</summary>
        private event Action audioStreamStarted;

        /// <summary>Represents the collection of frame numbers and their associated timespans</summary>
        protected List<TimeSpan> FrameTimeCodes { get; set; }

        /// <summary>Represents the TimeSpan for the frame at which video playback started</summary>
        protected TimeSpan videoPlaybackStartTime { get; set; }

        /// <summary>Locking Object reference for the video timer</summary>
        private Object videoTimerLock;

        /// <summary>Locking Object reference for the video coder</summary>
        private Object videoCoderLock;
        #endregion


        #region Events
        /// <summary>Exposes the event for timer elapse</summary>
        public event Action<IMultimediaImageFrame> PlayFrame
        {
            add { this.timerElapsed += value; }
            remove { this.timerElapsed -= value; }
        }

        /// <summary>Public-facing event indicating that the manager has received a start command from the MVE stream</summary>
        public event Action AudioStreamStarted
        {
            add { this.audioStreamStarted += value; }
            remove { this.audioStreamStarted -= value; }
        }
        #endregion


        #region Properties
        /// <summary>Exposes the length of the video file</summary>
        public virtual TimeSpan VideoLength
        {
            get
            {
                Double ticks = (Convert.ToDouble(this.FrameRateTimerParameters.Denominator * this.videoStream.FrameCount) / Convert.ToDouble(this.FrameRateTimerParameters.Numerator)) * 10000000.0; //microseconds = 10^-6, I want 10^-7 (100 nanoseconds)
                return new TimeSpan(Convert.ToInt64(ticks));
            }
        }

        /// <summary>Represents the stream of video opcodes</summary>
        public VideoOpcodeStream VideoStream
        {
            set
            {
                this.videoStream = value;   //set operation
                this.videoStream.AudioStreamStarted += this.RaiseStartAudio;    //attach event handler
                this.PopulateFrameTimeCodes();  //collect the frame timecodes
            }
        }

        /// <summary>Exposes the start TimeSpan of audio in this stream</summary>
        public TimeSpan AudioStartTime
        {
            get { return this.FrameTimeCodes[this.videoStream.AudioStartFrame]; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public MveVideoManager()
        {
            this.InitializeManager();
        }

        /// <summary>Initializes the MVE manager</summary>
        public virtual void InitializeManager()
        {
            this.FrameRateTimerParameters = null;
            this.VideoStreamParameters = null;

            //video timer
            this.timer = TimerFactory.BuildTimer();
            this.timer.Elapsed += new Action<TimeSpan>(this.VideoTimerExpired);

            //locking objects
            this.videoTimerLock = new Object();
            this.videoCoderLock = new Object();
        }

        /// <summary>Initializes the coder objects</summary>
        public virtual void InitializeVideoCoder()
        {
            switch (this.VideoStreamParameters.BitsPerPixel)
            {
                case 8:
                    lock (this.videoCoderLock)
                        this.VideoCoder = new VideoCoding8Bit(this.VideoStreamParameters.Width * 8, this.VideoStreamParameters.Height * 8, this.videoStream);
                    break;
                case 16:
                    lock (this.videoCoderLock)
                        this.VideoCoder = new VideoCoding16Bit(this.VideoStreamParameters.Width * 8, this.VideoStreamParameters.Height * 8, this.videoStream);
                    break;
                default:
                    throw new ApplicationException("Member \"VideoStreamParameters.BitsPerPixel\" was neither 8 nor 16.");
            }
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
            if (this.timer != null)
            {
                this.timer.Dispose();   //should implicitly Stop()
                this.timer = null;
            }

            //clear events
            this.timerElapsed = null;
            this.audioStreamStarted = null;
        }

        /// <summary>Finalize destructor method</summary>
        ~MveVideoManager()
        {
            this.StopVideoPlayback();
            this.ClearTimerElapsed();
        }
        #endregion


        #region Data Population
        /// <summary>Populates the frame time codes based on the frame rate</summary>
        protected void PopulateFrameTimeCodes()
        {
            this.FrameTimeCodes = new List<TimeSpan>(); //instantiate a new collection of time codes

            Decimal interval = this.FrameRateTimerParameters.FrameIntervalDecimal;  //frame interval, in milliseconds
            Decimal position = 0M;

            for (Int32 i = 0; i < this.videoStream.FrameCount; ++i)
            {
                this.FrameTimeCodes.Add(new TimeSpan(0, 0, 0, 0, Convert.ToInt32(Math.Round(position)))); 
                position += interval;
            }
        }

        /// <summary>Reads the data from the input stream into the storage opcodes indexed by the manager</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void ReadData(Stream input)
        {
            this.videoStream.ReadData(input);
        }
        #endregion


        #region Video exposure
        /// <summary>Creates a playback timer for the MVE</summary>
        public virtual void StartVideoPlayback()
        {
            //do not set interval, since you want it going off more frequently than 33 or 66 milliseconds
            //this.timer.Interval = (UInt32)this.FrameRateTimerParameters.MillisecondDelay;

            this.videoPlaybackStartTime = this.FrameTimeCodes[this.videoStream.CurrentFramePlayback];
            this.timer.Start();
        }

        /// <summary>Stops video playback, if playing</summary>
        public virtual void StopVideoPlayback()
        {
            if (this.timer != null)
                this.timer.Stop();
        }

        /// <summary>Event handler for expiration of the timer</summary>
        protected virtual void VideoTimerExpired(TimeSpan timeCode)
        {
            lock (this.videoTimerLock)
            {
                //check to see that anyone is still attached to this event
                if (this.timerElapsed != null)
                {
                    TimeSpan effectiveTimeCode = timeCode + this.videoPlaybackStartTime;

                    if (effectiveTimeCode <= this.VideoLength)
                    {
                        Int32 frameNum = this.DetermineWhichFrameToShow(effectiveTimeCode);

                        //loop to catch up as necessary
                        if (frameNum != this.videoStream.CurrentFramePlayback)
                        {
                            IMultimediaImageFrame frame = null;
                            while (frameNum != this.videoStream.CurrentFramePlayback)
                                frame = this.GetNextFrame();

                            this.timerElapsed(frame);
                        }
                    }
                    else //we have run our course
                        this.StopVideoPlayback();
                }
            }
        }

        /// <summary>Decodes the video maps from the SetDecodingMap opcodes</summary>
        public virtual void DecodeVideoMaps()
        {
            this.videoStream.DecodeVideoMaps();
        }

        /// <summary>Fetches the next MediaBase Frame for output</summary>
        /// <returns>A MediaBase Frame for output</returns>
        public virtual IMultimediaImageFrame GetNextFrame()
        {
            IMultimediaImageFrame frame = this.FetchAndReturnMveFrame();
            this.LaunchCacheVideoDataThread(1);
            return frame;
        }

        /// <summary>Fetches the next frame from the stream and passes it to the decoder, storing the result</summary>
        /// <returns>A MediaBase Frame reference</returns>
        protected virtual IMultimediaImageFrame FetchAndDecodeMveFrame()
        {
            MveVideoFrame mveFrame = this.videoStream.FetchNextFrameDecode();
            IMultimediaImageFrame frame = null;

            if (mveFrame != null)
            {
                frame = this.VideoCoder.GetNextFrame(mveFrame);
                mveFrame.FrameBuffer = frame;
            }

            return frame;
        }

        /// <summary>Fetches the next frame from the playback stream returns it</summary>
        /// <returns>A MediaBase Frame reference</returns>
        protected virtual IMultimediaImageFrame FetchAndReturnMveFrame()
        {
            MveVideoFrame mveFrame = this.videoStream.FetchNextFrameOutput();
            IMultimediaImageFrame frame = null;

            if (mveFrame != null)   //swap variables
            {
                frame = mveFrame.FrameBuffer;
                mveFrame.FrameBuffer = null;    //do this to keep the cached data small; destroy once consumed
            }

            return frame;
        }

        /// <summary>Resets the video stream to the beginning</summary>
        public virtual void ResetVideo()
        {
            lock (this.videoCoderLock)
                this.VideoCoder.ResetVideo();

            this.videoPlaybackStartTime = TimeSpan.Zero;
            this.timer.ResetStartTime();

            this.PreemptivelyStartDecodingVideo();  //all the frames at 0 will be unset if we've played any length of video
        }

        /// <summary>Decoding launcher to store frames in cache. Starts a background thread.</summary>
        public virtual void PreemptivelyStartDecodingVideo()
        {
            //decode 30 frames
            this.LaunchCacheVideoDataThread(30);
        }

        /// <summary>Launched a thread to cache video data to the video data thread</summary>
        /// <param name="frameCount">Number of frames to fetch</param>
        protected virtual void LaunchCacheVideoDataThread(Int32 frameCount)
        {
            Thread decoder = new Thread(() => this.CacheVideoData(frameCount));
            decoder.IsBackground = true;
            decoder.Name = "MVE video decoder";
            decoder.Start();
        }

        /// <summary>Decoding method to store video frames in a cache. Meant for a background thread.</summary>
        protected virtual void CacheVideoData(Int32 count)
        {
            //do not get confused and re-assign a frame to the same frame number
            lock (this.videoCoderLock)
            {
                //loop start and end positions
                Int32 start = this.videoStream.CurrentFrameDecoding;
                Int32 end = start + count;

                //cache frames of video
                for (Int32 i = start; i < end; ++i)
                {
                    IMultimediaImageFrame decoded = this.FetchAndDecodeMveFrame();
                    if (decoded == null)
                        break;
                }
            }
        }

        /// <summary>Gets the frame number to display based off of a TimeSpan</summary>
        /// <param name="elapsed">How much time has elapsed since the start of the video</param>
        /// <returns>The frame number to display</returns>
        protected Int32 DetermineWhichFrameToShow(TimeSpan elapsed)
        {
            Int32 frameNumber = 0;
            for (frameNumber = (this.FrameTimeCodes.Count - 1); frameNumber > -1; --frameNumber)
            {
                if (this.FrameTimeCodes[frameNumber] < elapsed)
                    break;
            }

            return frameNumber;
        }
        #endregion


        #region Event Control
        /// <summary>Control method to clear the TimerElapsed event delegates</summary>
        public virtual void ClearTimerElapsed()
        {
            this.timerElapsed = null;
        }

        /// <summary>Event raising methodfor starting audio</summary>
        protected virtual void RaiseStartAudio()
        {
            if (this.audioStreamStarted != null)
                this.audioStreamStarted();
        }
        #endregion
    }
}