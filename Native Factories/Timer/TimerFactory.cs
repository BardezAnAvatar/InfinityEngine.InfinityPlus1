using System;

using Bardez.Projects.Multimedia.MediaBase.Management;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Timer;

namespace Bardez.Projects.InfinityPlus1.NativeFactories.Timer
{
    /// <summary>Factory for ITimer</summary>
    public static class TimerFactory
    {
        /// <summary>Constructs the default timer for the environment</summary>
        /// <returns>ITimer Instance</returns>
        public static ITimer BuildTimer()
        {
            return new Win32MultimediaTimer();
        }

        /// <summary>Constructs the default timer for the environment</summary>
        /// <param name="frequency">Frequency period of the timer interval</param>
        /// <returns>ITimer Instance</returns>
        public static ITimer BuildTimer(UInt32 frequency)
        {
            return new Win32MultimediaTimer(frequency);
        }
    }
}