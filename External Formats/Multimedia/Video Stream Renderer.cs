﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Timers;

using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels.Enums;
using Bardez.Projects.MultiMedia.LibAV;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Multimedia
{
    /// <summary>
    ///     This class represents a LibAV video stream's video render manager, which exports video chunks
    ///     when its attempt render method is successful
    /// </summary>
    public class VideoStreamRenderManager : IStreamRenderManager
    {
        #region Fields
        /// <summary>Event to raise for rendering video output.</summary>
        protected event Action<Frame> render;

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
        public event Action<Frame> Render
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
                    if (peek != null && peek.GetPresentationTimeSpan(this.VideoBuffer.TimeBase) < time)
                    {
                        peek = this.VideoBuffer.ConsumeFrame();
                        Frame frame = this.BuildLibAvFrame(peek);
                        this.RaiseRenderVideo(frame);
                    }
                }
            }
        }
        #endregion


        #region Event control
        /// <summary>Raises the render audio event</summary>
        /// <param name="frame">Frame to pass to renderer</param>
        protected void RaiseRenderVideo(Frame frame)
        {
            if (this.render != null)
                this.render(frame);
        }
        #endregion


        #region Frame construction
        /// <summary>Builds a MediaBase frame for passing to output</summary>
        /// <param name="frameData">LibAV frame to build a MediaBase frame from</param>
        /// <returns>A Built MediaBase frame</returns>
        protected Frame BuildLibAvFrame(FrameBGRA frameData)
        {
            //PixelData pd = new PixelData(frameData.Data, ScanLineOrder.TopDown, MediaBase.Video.Pixels.Enums.PixelFormat.RGBA_B8G8R8A8, frameData.Height, frameData.Width, 0, 0, 32);
            //Frame genFrame = new Frame(pd);
Frame genFrame = null;
            return genFrame;
        }
        #endregion
    }
}