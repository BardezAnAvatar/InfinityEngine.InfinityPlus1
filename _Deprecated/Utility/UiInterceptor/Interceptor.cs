using System;
using Bardez.Projects.Configuration;

namespace Bardez.Projects.InfinityPlus1.Utility.UiInterceptor
{
    /// <summary>Class to intercept certain UI calls</summary>
    public static class Interceptor
    {
        /// <summary>Other class that will intercept the I/O</summary>
        /// <remarks>Static constuction</remarks>
        private static IUserInterfaceInterceptor interceptor = null;

        /// <summary>Locking object</summary>
        /// <remarks>Static constuction</remarks>
        private static Object locker = new Object();

        private const String interceptorKey = "UIInterceptor.Wrapper";

        /// <summary>Initializes the interceptor member</summary>
        private static void Initialize()
        {
            lock (Interceptor.locker)
            {
                if (Interceptor.interceptor == null)
                {
                    String interceptorType = ConfigurationHandler.GetSettingValue(Interceptor.interceptorKey);

                    switch (interceptorType.ToLower())
                    {
                        case "console":
                            Interceptor.interceptor = ConsoleWrapper.Instance;
                            break;
                        case "form":
                            Interceptor.interceptor = FormWrapper.Instance;
                            break;
                    }
                }
            }
        }

        /// <summary>Writes a String to UI output, trailing with a newline</summary>
        /// <param name="programOutput">Output to write to display the user</param>
        public static void WriteMessage(String programOutput)
        {
            lock (Interceptor.locker)
            {
                Interceptor.Initialize();
                if (Interceptor.interceptor != null)
                    Interceptor.interceptor.WriteMessage(programOutput);
            }
        }

        /// <summary>Writes a String to UI output, trailing with a newline</summary>
        /// <param name="programOutput">Output to write to display the user</param>
        public static void Write(String programOutput)
        {
            lock (Interceptor.locker)
            {
                Interceptor.Initialize();
                if (Interceptor.interceptor != null)
                    Interceptor.interceptor.Write(programOutput);
            }
        }

        /// <summary>Writes a String to UI output, trailing with a newline</summary>
        /// <param name="programOutput">Output to write to display the user</param>
        public static void WriteLine(String programOutput)
        {
            lock (Interceptor.locker)
            {
                Interceptor.Initialize();
                if (Interceptor.interceptor != null)
                    Interceptor.interceptor.WriteLine(programOutput);
            }
        }

        /// <summary>Blocks execution waiting on user input.</summary>
        public static void WaitForInput()
        {
            Interceptor.WaitForInput(null);
        }

        /// <summary>Blocks execution waiting on user input.</summary>
        /// <param name="prompt">Prompt to display to the user</param>
        public static void WaitForInput(String prompt)
        {
            lock (Interceptor.locker)
            {
                Interceptor.Initialize();
                if (Interceptor.interceptor != null)
                    Interceptor.interceptor.WaitForInput(prompt);
            }
        }
    }
}