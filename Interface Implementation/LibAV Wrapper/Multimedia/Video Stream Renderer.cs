using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Timers;

using Bardez.Projects.Multimedia.LibAV;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels.Enums;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.Multimedia.MediaBase.Render;

namespace Bardez.Projects.Multimedia.LibAV.Wrapper
{
    /// <summary>
    ///     This class represents a LibAV video stream's video render manager, which exports video chunks
    ///     when its attempt render method is successful
    /// </summary>
    public class VideoStreamRenderManager : IAVStreamRenderManager
    {
        #region Fields
        /// <summary>Event to raise for rendering video output.</summary>
        protected event Action<IMultimediaImageFrame> render;

        /// <summary>Buffer and processing info for this video stream</summary>
        protected StreamProcessingBuffer<FrameBGRA> VideoBuffer { get; set; }

        /// <summary>Loking object reference for the render attempt</summary>
        private Object renderLock;
        #endregion


        #region Properties
        /// <summary>Exposs a flag indicating whether or not to process this stream</summary>
        public Boolean Process
        {
            get { return this.VideoBuffer.Process; }
            set { this.VideoBuffer.Process = value; }
        }

        /// <summary>exposes the stream index this manager renders for</summary>
        public Int32 StreamIndex
        {
            get { return this.VideoBuffer.Index; }
        }
        #endregion


        #region Exposed Events
        /// <summary>Event to raise for rendering video output.</summary>
        public event Action<IMultimediaImageFrame> Render
        {
            add { this.render += value; }
            remove { this.render -= value; }
        }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="buffer">Buffer to create the manager for</param>
        public VideoStreamRenderManager(StreamProcessingBuffer<FrameBGRA> buffer)
        {
            this.VideoBuffer = buffer;
            this.renderLock = new Object();
        }
        #endregion


        #region Command & Control
        /// <summary>
        ///     Attempts to render video if the buffer is processed and the timespan is within the
        ///     rendering threshold
        /// </summary>
        /// <param name="time">Time code to render by</param>
        public void AttemptRender(TimeSpan time)
        {
            lock (this.renderLock)
            {
                if (this.VideoBuffer.Process)
                {
                    FrameBGRA peek = this.VideoBuffer.PeekFrame();
                    if (peek != null && peek.GetPresentationStartTimeSpan(this.VideoBuffer.TimeBase) < time)
                    {
                        peek = this.VideoBuffer.ConsumeFrame();
                        IMultimediaImageFrame frame = this.BuildFrameFromLibAV(peek);
                        this.RaiseRenderVideo(frame);
                    }
                }
            }
        }
        #endregion


        #region Event control
        /// <summary>Raises the render audio event</summary>
        /// <param name="frame">Frame to pass to renderer</param>
        protected void RaiseRenderVideo(IMultimediaImageFrame frame)
        {
            if (this.render != null)
                this.render(frame);
        }
        #endregion


        #region Frame construction
        /// <summary>Builds a MediaBase frame for passing to output</summary>
        /// <param name="frameData">LibAV frame to build a MediaBase frame from</param>
        /// <returns>A Built MediaBase frame</returns>
        protected IMultimediaImageFrame BuildFrameFromLibAV(FrameBGRA frameData)
        {
            PixelData pd = new PixelData(frameData.Data.ToArray(), ScanLineOrder.TopDown, frameData.Detail.Format.ToPixelFormat(), frameData.Detail.Height, frameData.Detail.Width, 0, 0, 32);
            IMultimediaImageFrame genFrame = new BasicImageFrame(pd);
            return genFrame;
        }
        #endregion
    }
}