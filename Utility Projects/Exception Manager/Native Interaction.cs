using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Bardez.Projects.ExceptionHandler
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MEMORYSTATUSEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;

        public MEMORYSTATUSEX()
        {
            this.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        protected static extern Boolean GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

        public static MEMORYSTATUSEX GetGlobalMemoryStatusEx()
        {
            MEMORYSTATUSEX mem = new MEMORYSTATUSEX();
            GlobalMemoryStatusEx(mem);
            return mem;
        }
    }
}