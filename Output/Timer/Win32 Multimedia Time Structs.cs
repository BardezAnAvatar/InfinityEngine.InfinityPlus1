using System;
using System.Runtime.InteropServices;

namespace Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Timer
{
    /// <summary>Structure representing the multimedia timer's capabilities from the native library</summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct Win32MultimediaTimerCapabilities
    {
        /// <summary>Minium suported period (in milliseconds)</summary>
        public UInt32 PeriodMin;

        /// <summary>Maximum suported period (in milliseconds)</summary>
        public UInt32 PeriodMax;
    }

    /// <summary>Enumerator describng the timer event types</summary>
    internal enum Win32MultimediaTimerMode : uint /* UInt32 */
    {
        /// <summary>Occurs only once</summary>
        OneShot = 0,        //#define TIME_ONESHOT    0x0000   /* program timer for single event */

        /// <summary>Timer event reoccurs periodically.</summary>
        Periodic = 1,       //#define TIME_PERIODIC   0x0001   /* program for continuous periodic event */
    }

    /// <summary>Callback delegate of WIn32 API</summary>
    /// <param name="timerId">Timer ID</param>
    /// <param name="message">Message</param>
    /// <param name="user">User number</param>
    /// <param name="param1">Parameter # 1</param>
    /// <param name="param2">Parameter # 2</param>
    internal delegate void TimeCallbackDelegate(UInt32 timerId, UInt32 message, IntPtr user, IntPtr param1, IntPtr param2);
}