using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Timers;

using Bardez.Projects.BasicStructures.Win32.Audio;
using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Wave.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels;
using Bardez.Projects.InfinityPlus1.NativeFactories.Timer;
using Bardez.Projects.MultiMedia.LibAV;

namespace Bardez.Projects.Multimedia.LibAV.Wrapper
{
    /// <summary>Represents a controller for LibAV playback of audio + video</summary>
    public class MultimediaMovie
    {
        #region Fields
        /// <summary>Collection of renderers for audio streams</summary>
        public List<AudioStreamRenderManager> AudioStreamRenderers { get; set; }

        /// <summary>Collection of renderers for video streams</summary>
        public List<VideoStreamRenderManager> VideoStreamRenderers { get; set; }

        /// <summary>Container for the libav file</summary>
        protected Container libavContainer { get; set; }

        /// <summary>Represents the buffers for reading/playback</summary>
        protected StreamBuffers buffers { get; set; }

        /// <summary>Represents the stream index of the rendered video stream</summary>
        protected Int32 VideoStreamIndex { get; set; }

        /// <summary>Represents the stream index of the presented audio stream</summary>
        protected Int32 AudioStreamIndex { get; set; }

        /// <summary>Timer for movie playback</summary>
        protected ITimer timer { get; set; }

        /// <summary>Current presentation TimeSpan for displaying frames</summary>
        protected TimeSpan currentPresentationTime { get; set; }

        /// <summary>The TimeSpan from which the movie started playing</summary>
        protected TimeSpan videoPlaybackStartTime { get; set; }

        /// <summary>Flag indicating whether audio is presently rendering</summary>
        protected Boolean renderingAudio;

        /// <summary>Locking Object reference for the video timer</summary>
        private Object videoTimerLock;
        #endregion


        #region Properties
        /// <summary>Exposes the length of the video file</summary>
        public virtual TimeSpan MovieLength
        {
            get
            {
                TimeSpan length = TimeSpan.Zero;
                
                Double ticks = (this.libavContainer.TimeBase.Value * this.libavContainer.Duration) * 10000000.0; //microseconds = 10^-6, I want 10^-7 (100 nanoseconds)
                length = new TimeSpan(Convert.ToInt64(ticks));

                return length;
            }
        }

        /// <summary>Exposes the underlying audio streams, allowing them to have event listeners attached</summary>
        public Dictionary<Int32, StreamProcessingBuffer<FrameAudioInt16>> AudioStreams
        {
            get { return this.buffers.StreamsAudio; }
        }

        /// <summary>Exposes the underlying video streams, allowing them to have event listeners attached</summary>
        public Dictionary<Int32, StreamProcessingBuffer<FrameBGRA>> VideoStreams
        {
            get { return this.buffers.StreamsVideo; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public MultimediaMovie()
        {
            this.videoTimerLock = new Object();

            //video timer
            this.timer = TimerFactory.BuildTimer(8);    //try every 8 milliseconds (60 fps = 16.666 ; 17, take roughly half of that)
            this.timer.Elapsed += new Action<TimeSpan>(this.PresentationTimerExpired);
        }
        #endregion


        #region Destruction
        /// <summary>Disposal method</summary>
        public void Dispose()
        {
            if (this.libavContainer != null)
            {
                this.libavContainer.Dispose();
                this.libavContainer = null;
            }

            if (this.buffers != null)
                this.buffers = null;

            if (this.timer != null)
            {
                this.timer.Dispose();   //should implicitly Stop()
                this.timer = null;
            }
        }
        #endregion


        #region IMovie implementation
        /// <summary>Resets the video stream to the beginning</summary>
        public void ResetVideo()
        {
            this.currentPresentationTime = TimeSpan.Zero;

            //TODO: implement seeking in LibAV wrapper
            throw new NotImplementedException("seeking not yet implemented in LibAV wrapper");
        }

        /// <summary>Creates a playback timer for the MVE</summary>
        public virtual void StartPlayback()
        {
            //do not set interval, since you want it going off more frequently than 33 or 66 milliseconds
            //this.timer.Interval = (UInt32)this.FrameRateTimerParameters.MillisecondDelay;

            this.videoPlaybackStartTime = this.currentPresentationTime;
            this.timer.Start();
        }

        /// <summary>Stops video playback, if playing</summary>
        public virtual void StopPlayback()
        {
            this.StopTimer();
        }
        #endregion


        #region Timer code
        /// <summary>Event handler for expiration of the timer</summary>
        protected virtual void PresentationTimerExpired(TimeSpan timeCode)
        {
            lock (this.videoTimerLock)
            {
                TimeSpan effectiveTimeCode = timeCode + this.videoPlaybackStartTime;
                //if (effectiveTimeCode > this.MovieLength)
                //{
                //    //stop the timer
                //    this.StopTimer();
                //}
                //else
                {
                    foreach (AudioStreamRenderManager audio in this.AudioStreamRenderers)
                        audio.AttemptRender(effectiveTimeCode);

                    foreach (VideoStreamRenderManager video in this.VideoStreamRenderers)
                        video.AttemptRender(effectiveTimeCode);
                }
            }
        }
        #endregion


        #region Command and Control
        /// <summary>Opens the specified multimedia file</summary>
        /// <param name="path">path to the file to open</param>
        public void Open(String path)
        {
            this.libavContainer = new Container();
            this.libavContainer.OpenMediaFile(path);
            this.libavContainer.ReadStreamInfo();

            Int32 bufferSize = Int32.Parse(Configuration.ConfigurationHandlerMulti.GetSettingValue("LibAVStreamBufferSize"));

            IList<StreamInfo> streams = this.libavContainer.Streams;
            this.buffers = new StreamBuffers(streams, bufferSize);

            //copy a list of indeces to initialize
            Int32[] streamIndeces = new Int32[streams.Count];
            for (Int32 index = 0; index < streams.Count; ++index)
                streamIndeces[index] = streams[index].Index;

            this.libavContainer.LoadCodecs(streamIndeces);


            //link the buffers to audio/video playback
            this.AudioStreamRenderers = new List<AudioStreamRenderManager>();
            foreach (StreamProcessingBuffer<FrameAudioInt16> audio in this.AudioStreams.Values)
                this.AudioStreamRenderers.Add(new AudioStreamRenderManager(audio));

            this.VideoStreamRenderers = new List<VideoStreamRenderManager>();
            foreach (StreamProcessingBuffer<FrameBGRA> video in this.VideoStreams.Values)
                this.VideoStreamRenderers.Add(new VideoStreamRenderManager(video));
        }

        /// <summary>Launches the LibAV data reading process</summary>
        public void StartDecoding()
        {
            Thread decoder = new Thread(() => { this.libavContainer.DecodeStreams(this.buffers); });
            decoder.IsBackground = true;
            decoder.Name = "Video decoder";
            decoder.Start();
        }

        /// <summary>Sets the collection flag for a specified audio stream</summary>
        /// <param name="streamIndex">Stream index to affect</param>
        /// <param name="value">Value to set</param>
        public void ToggleAudioStreamCollection(Int32 streamIndex, Boolean value)
        {
            this.buffers.StreamsAudio[streamIndex].Process = value;
        }

        /// <summary>Sets the collection flag for a specified video stream</summary>
        /// <param name="streamIndex">Stream index to affect</param>
        /// <param name="value">Value to set</param>
        public void ToggleVideoStreamCollection(Int32 streamIndex, Boolean value)
        {
            this.buffers.StreamsVideo[streamIndex].Process = value;
        }

        /// <summary>Exposes the first audio stream found in audio stream renderers</summary>
        /// <returns>The first audio stream found in audio stream renderers</returns>
        public AudioStreamRenderManager GetBestAudioStream()
        {
            Int32 index = this.libavContainer.FindBestStream(MediaType.AVMEDIA_TYPE_AUDIO);

            AudioStreamRenderManager audio = null;
            if (this.AudioStreamRenderers != null)
                for (Int32 stream = 0; stream < this.AudioStreamRenderers.Count; ++stream)
                    if (this.AudioStreamRenderers[stream].StreamIndex == index)
                        audio = this.AudioStreamRenderers[stream];

            return audio;
        }

        /// <summary>Exposes the first video stream found in video stream renderers</summary>
        /// <returns>The first video stream found in video stream renderers</returns>
        public VideoStreamRenderManager GetBestVideoStream()
        {
            Int32 index = this.libavContainer.FindBestStream(MediaType.AVMEDIA_TYPE_VIDEO);

            VideoStreamRenderManager video = null;
            if (this.VideoStreamRenderers != null)
                for (Int32 stream = 0; stream < this.VideoStreamRenderers.Count; ++stream)
                    if (this.VideoStreamRenderers[stream].StreamIndex == index)
                        video = this.VideoStreamRenderers[stream];

            return video;
        }

        /// <summary>Control method to clear the TimerElapsed event delegates</summary>
        public virtual void ClearTimerElapsed()
        {
            this.StopTimer();
        }
        #endregion


        #region Helpers
        /// <summary>Stops the time playback</summary>
        protected virtual void StopTimer()
        {
            if (this.timer != null)
                this.timer.Stop();
        }

        /// <summary>Starts the time playback</summary>
        protected virtual void StartTimer()
        {
            if (this.timer != null)
                this.timer.Start();
        }
        #endregion
    }
}