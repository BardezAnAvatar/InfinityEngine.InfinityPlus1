using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

//GetMemoryB
using System.Management;

namespace Bardez.Projects.ExceptionHandler
{
    /// <summary>Reveals and wraps system information</summary>
    public static class SystemInfo
    {
        /// <summary>Gets the system up time.</summary>
        /// <returns>A TimeSpan indicating the system up time.</returns>
        private static TimeSpan GetSystemUpTime()
        {
            PerformanceCounter upTime = new PerformanceCounter("System", "System Up Time");
            upTime.NextValue();
            return TimeSpan.FromSeconds(upTime.NextValue());
        }

        /// <summary>This method gets the available system memory</summary>
        /// <returns>A formatted string representing available system memory</returns>
        /// <remarks>This method is slow</remarks>
        private static String GetSystemMemoryA()
        {
            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            return "\tAvailable:                  " + ramCounter.NextValue().ToString() + " MB";
        }

        private static String GetSystemMemoryC()
        {
            Int64 mem = Environment.WorkingSet;
            Double memD = Convert.ToDouble(mem);
            StringBuilder InUse = new StringBuilder();
            InUse.AppendLine("\tIn use:                     " + (memD / 1024.0 / 1024.0).ToString("#####0.0#") + " MB");

            return InUse.ToString();
        }


        private static String GetSystemMemoryD()
        {
            MEMORYSTATUSEX memStatus = MEMORYSTATUSEX.GetGlobalMemoryStatusEx();
            StringBuilder memory = new StringBuilder();
            memory.AppendLine("\tDetail:");
            memory.AppendLine("\t\tLength:                 " + memStatus.dwLength);
            memory.AppendLine("\t\tMemory Load:            " + memStatus.dwMemoryLoad);
            memory.AppendLine("\t\tTotal Physical:         " + (Convert.ToDouble(memStatus.ullTotalPhys) / 1024.0 / 1024.0).ToString("#####0.0#") + " MB");
            memory.AppendLine("\t\tAvailable Physical:     " + (Convert.ToDouble(memStatus.ullAvailPhys) / 1024.0 / 1024.0).ToString("#####0.0#") + " MB");
            memory.AppendLine("\t\tTotal Page File:        " + (Convert.ToDouble(memStatus.ullTotalPageFile) / 1024.0 / 1024.0).ToString("#####0.0#") + " MB");
            memory.AppendLine("\t\tAvailable Page File:    " + (Convert.ToDouble(memStatus.ullAvailPageFile) / 1024.0 / 1024.0).ToString("#####0.0#") + " MB");
            memory.AppendLine("\t\tTotal Virtual:          " + (Convert.ToDouble(memStatus.ullTotalVirtual) / 1024.0 / 1024.0).ToString("#####0.0#") + " MB");
            memory.AppendLine("\t\tAvailable Virtual:      " + (Convert.ToDouble(memStatus.ullAvailVirtual) / 1024.0 / 1024.0).ToString("#####0.0#") + " MB");
            memory.AppendLine("\t\tAvailable Ext. Vir.:    " + (Convert.ToDouble(memStatus.ullAvailExtendedVirtual) / 1024.0 / 1024.0).ToString("#####0.0#") + " MB");
            memory.AppendLine(String.Empty);
            memory.AppendLine(String.Empty);

            return memory.ToString();
        }

        private static String GetAssemblyName()
        {
            return Assembly.GetExecutingAssembly().GetName().Name;
        }

        private static String GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private static String GetDate()
        {
            return DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private static String GetUserName()
        {
            return System.Environment.UserName;
        }

        private static String GetOperatingSystemVersion()
        {
            return System.Environment.OSVersion.VersionString;
        }

        private static String GetOperatingSystemServicePack()
        {
            return System.Environment.OSVersion.ServicePack;
        }

        private static String GetCurrentCultureName()
        {
            return CultureInfo.CurrentCulture.Name;
        }

        private static String GetProcessorCount()
        {
            return Environment.ProcessorCount.ToString();
        }

        private static Boolean IsOs64Bit()
        {
            return Environment.Is64BitOperatingSystem;
        }

        private static Boolean IsProcess64Bit()
        {
            return Environment.Is64BitProcess;
        }

        private static String GetMachineName()
        {
            return Environment.MachineName;
        }

        private static String GetProcessUpTime()
        {
            return (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString();
        }

        private static String GetPrimaryResolution()
        {
            String res = "Unavailable";
            try
            {
                res = System.Windows.Forms.Screen.PrimaryScreen.Bounds.ToString();
            }
            catch { }

            return res;
        }

        private static String GetLoadedModules()
        {
            StringBuilder modules = new StringBuilder();
            Process thisProcess = Process.GetCurrentProcess();
            foreach (ProcessModule module in thisProcess.Modules)
                modules.AppendLine(module.FileName + " " + module.FileVersionInfo.FileVersion);

            return modules.ToString();
        }

        public static String GetSystemInfo()
        {
            StringBuilder systemInfo = new StringBuilder();

            systemInfo.AppendLine("Application:       " + GetAssemblyName());
            systemInfo.AppendLine("Version:           " + GetAssemblyVersion());
            systemInfo.AppendLine("Date:              " + GetDate());
            systemInfo.AppendLine("Computer name:     " + GetMachineName());
            systemInfo.AppendLine("User name:         " + GetUserName());
            systemInfo.AppendLine("Processor Count:   " + GetProcessorCount());
            systemInfo.AppendLine("OS:                " + GetOperatingSystemVersion());
            systemInfo.AppendLine("Is OS 64 Bit:      " + (IsOs64Bit() ? "Yes" : "No"));
            systemInfo.AppendLine("Is Process 64 Bit: " + (IsProcess64Bit() ? "Yes" : "No"));
            systemInfo.AppendLine("Processor Count:   " + GetProcessorCount());
            systemInfo.AppendLine("Service Pack:      " + GetOperatingSystemServicePack());
            systemInfo.AppendLine("Culture:           " + GetCurrentCultureName());
            systemInfo.AppendLine("System up time:    " + GetSystemUpTime().ToString());
            systemInfo.AppendLine("App up time:       " + GetProcessUpTime());
            systemInfo.AppendLine("Resolution:        " + GetPrimaryResolution());
            systemInfo.AppendLine("Memory:");
            systemInfo.Append(GetSystemMemoryC());
            systemInfo.Append(GetSystemMemoryD());
            systemInfo.AppendLine("Loaded Modules:");
            systemInfo.AppendLine(GetLoadedModules());

            return systemInfo.ToString();
        }
    }
}