using System;
using System.Runtime.InteropServices;

using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;

namespace Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Timer
{
    /// <summary>Exposes the Win32 high-resolution Multimedia timer</summary>
    public class Win32MultimediaTimer : ITimer
    {
        #region Constants
        /// <summary>Constant representing that no error has occurred</summary>
        private const UInt32 TIMERR_NOERROR = 0U;
        #endregion


        #region Static fields
        /// <summary>Static reference to a capabilities null-typed reference</summary>
        private static Win32MultimediaTimerCapabilities? Capabilities { get; set; }
        #endregion


        #region Fields
        /// <summary>Win32 timer ID</summary>
        protected UInt32 TimerId { get; set; }

        /// <summary>Frequency period of the timer interval</summary>
        public UInt32 Interval { get; set; }

        /// <summary>Expected reslution (error) of the timer</summary>
        protected UInt32 Resolution { get; set; }

        /// <summary>Indicates whether the timer is runing</summary>
        public Boolean Running { get; set; }

        /// <summary>Timer event</summary>
        private event Action<TimeSpan> elapsed;

        /// <summary>Storage for the callback delegate.</summary>
        /// <remarks>Necessary so that the GC does not remove the delegate instance.</remarks>
        private TimeCallbackDelegate CallbackDelegate { get; set; }

        /// <summary>Represents when the timer was last started (stop sets to <see cref="TimeSpan.Zero"/>)</summary>
        public TimeSpan StartTime { get; set; }

        private Object timerLock;
        #endregion


        #region Properties
        /// <summary>Exposes the minimum period supported</summary>
        public UInt32 MinimumPeriod
        {
            get { return Win32MultimediaTimer.Capabilities.Value.PeriodMin; }
        }

        /// <summary>Exposes the maximum period supported</summary>
        public UInt32 MaximumPeriod
        {
            get { return Win32MultimediaTimer.Capabilities.Value.PeriodMax; }
        }
        #endregion


        #region Events
        /// <summary>Public-facing timer event</summary>
        public event Action<TimeSpan> Elapsed
        {
            add { this.elapsed += value;}
            remove { this.elapsed -= value; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public Win32MultimediaTimer()
        {
            if (Win32MultimediaTimer.Capabilities == null)
                Win32MultimediaTimer.Capabilities = this.GetTimerCapabilities();

            this.Resolution = 1U;
            this.Interval = this.MinimumPeriod;
            this.Running = false;
            this.StartTime = TimeSpan.Zero;
            this.timerLock = new Object();
        }

        /// <summary>Partial definition constructor</summary>
        /// <param name="frequency">Frequency period of the timer interval</param>
        public Win32MultimediaTimer(UInt32 frequency) : this(frequency, 1U) { }

        /// <summary>Definition constructor</summary>
        /// <param name="frequency">Frequency period of the timer interval</param>
        /// <param name="resolution">Resolution (error margin) of the timer</param>
        public Win32MultimediaTimer(UInt32 frequency, UInt32 resolution)
        {
            if (Win32MultimediaTimer.Capabilities == null)
                Win32MultimediaTimer.Capabilities = this.GetTimerCapabilities();

            this.Resolution = resolution;
            this.Interval = frequency;
            this.Running = false;
            this.StartTime = TimeSpan.Zero;
            this.timerLock = new Object();
        }

        /// <summary>Dispose method to clean up umanaged resources (Win32 timer)</summary>
        public void Dispose()
        {
            this.Stop();
        }
        #endregion


        #region Timer Methods
        /// <summary>Starts the timer</summary>
        public virtual void Start()
        {
            if (!this.Running)
            {
                this.CallbackDelegate = this.Win32Callback;

                // Create and start timer.
                this.TimerId = Win32MultimediaTimeFunctions.CreateTimerEvent(this.Interval, this.Resolution, this.CallbackDelegate, 0, Win32MultimediaTimerMode.Periodic);
                
                // If the timer was created successfully.
                if (this.TimerId != 0U) //NULL
                {
                    this.ResetStartTime();

                    //set to running
                    this.Running = true;
                }
                else
                    throw new ApplicationException("Unable to start the multimedia Timer.");
            }
        }

        /// <summary>Stops the timer</summary>
        public virtual void Stop()
        {
            if (this.Running)
            {
                UInt32 result = Win32MultimediaTimeFunctions.DestroyTimerEvent(this.TimerId);

                // If the timer was stopped successfully.
                if (result == Win32MultimediaTimer.TIMERR_NOERROR)
                {
                    this.StartTime = TimeSpan.Zero;
                    this.Running = false;
                }
                else
                    throw new ApplicationException("Unable to stop the multimedia Timer.");
            }
        }

        /// <summary>Resets the time for when the timer goes off</summary>
        public virtual void ResetStartTime()
        {
            lock (this.timerLock)
            {
                //get system time for start time
                UInt32 uptime = Win32MultimediaTimeFunctions.GetEnvironmentUptime();
                Int32 upTimeSec = (Int32)(uptime / 1000);
                Int32 upTimeMilliSec = (Int32)(uptime % 1000);
                this.StartTime = new TimeSpan(0, 0, 0, upTimeSec, upTimeMilliSec);
            }
        }
        
        /// <summary>Callback method for WIn32 API</summary>
        /// <param name="timerId">Timer ID</param>
        /// <param name="message">Message</param>
        /// <param name="user">User number</param>
        /// <param name="param1">Parameter # 1</param>
        /// <param name="param2">Parameter # 2</param>
        protected virtual void Win32Callback(UInt32 timerId, UInt32 message, IntPtr user, IntPtr param1, IntPtr param2)
        {
            TimeSpan raise;
            lock (this.timerLock)
            {
                //get system time for start time
                UInt32 uptime = Win32MultimediaTimeFunctions.GetEnvironmentUptime();
                Int32 upTimeSec = (Int32)(uptime / 1000);
                Int32 upTimeMilliSec = (Int32)(uptime % 1000);
                TimeSpan WinUpTime = new TimeSpan(0, 0, 0, upTimeSec, upTimeMilliSec);
                raise = WinUpTime - this.StartTime;
            }

            this.elapsed(raise);
        }
        #endregion


        #region Helper Methods
        /// <summary>Gets the system's multimedia timer capabilities</summary>
        /// <returns>Win32MultimediaTimerCapabilities instance</returns>
        internal virtual Win32MultimediaTimerCapabilities GetTimerCapabilities()
        {
            Win32MultimediaTimerCapabilities caps;
            Win32MultimediaTimeFunctions.GetDeviceCapabilities(out caps, 8U);
            return caps;
        }
        #endregion
    }
}