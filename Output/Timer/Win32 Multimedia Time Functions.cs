using System;
using System.Runtime.InteropServices;

namespace Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Timer
{
    /// <summary>Static wrapper around Win32 multimedia timer functions</summary>
    internal static class Win32MultimediaTimeFunctions
    {
        /// <summary>Accesses the timer's device capabilities.</summary>
        /// <param name="caps">Structure aligned to Win32 structure to explain device minimum and maximum period.</param>
        /// <param name="sizeOfTimerCapabilities">Win32 sze of the structure</param>
        /// <returns>UInt32 value indicating success or failure</returns>
        [DllImport("winmm.dll", EntryPoint = "timeGetDevCaps")]
        internal static extern UInt32 GetDeviceCapabilities(out Win32MultimediaTimerCapabilities caps, UInt32 sizeOfTimerCapabilities);

        /// <summary>Creates a Win32 callback event for the timer</summary>
        /// <param name="delay">Delay of the timer, in milliseconds</param>
        /// <param name="resolution">Resolution of the timer period</param>
        /// <param name="eventDelegate">Delegate to call back to within this timer</param>
        /// <param name="user">User number</param>
        /// <param name="futureEventType">Mode</param>
        /// <returns>UInt32 value indicating success or failure</returns>
        [DllImport("winmm.dll", EntryPoint = "timeSetEvent")]
        internal static extern UInt32 CreateTimerEvent(UInt32 delay, UInt32 resolution, TimeCallbackDelegate eventDelegate, UInt32 user, Win32MultimediaTimerMode futureEventType);

        /// <summary>Halts and destroys the timer's callback in the Win32 message queue</summary>
        /// <param name="id">ID of the timer</param>
        /// <returns>UInt32 value indicating success or failure</returns>
        [DllImport("winmm.dll", EntryPoint = "timeKillEvent")]
        internal static extern UInt32 DestroyTimerEvent(UInt32 id);

        /// <summary>Gets the current Windows uptime</summary>
        /// <returns>UInt32 representing the Windows uptime, in milliseconds</returns>
        [DllImport("winmm.dll", EntryPoint = "timeGetTime")]
        internal static extern UInt32 GetEnvironmentUptime();
    }
}