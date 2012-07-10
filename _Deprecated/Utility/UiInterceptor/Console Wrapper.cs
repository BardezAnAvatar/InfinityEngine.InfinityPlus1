using System;

namespace Bardez.Projects.Utility.UiInterceptor
{
    /// <summary>Wraps certain Console calls with more easily replacable alternatives</summary>
    public class ConsoleWrapper : IUserInterfaceInterceptor
    {
        /// <summary>Singleton instance</summary>
        /// <remarks>Static constructor</remarks>
        private static ConsoleWrapper singleton = null;

        /// <summary>Locking object</summary>
        private static Object singletonLocker = new Object();

        /// <summary>Default constructor</summary>
        private ConsoleWrapper() { }

        public static ConsoleWrapper Instance
        {
            get
            {
                lock (ConsoleWrapper.singletonLocker)
                {
                    if (ConsoleWrapper.singleton == null)
                        ConsoleWrapper.singleton = new ConsoleWrapper();

                    return ConsoleWrapper.singleton;
                }
            }
        }

        /// <summary>Writes a String to UI output, trailing with a newline</summary>
        /// <param name="programOutput">Output to write to display the user</param>
        public void WriteMessage(String programOutput)
        {
            Console.Write(programOutput);
        }

        /// <summary>Writes a String to UI output, trailing with a newline</summary>
        /// <param name="programOutput">Output to write to display the user</param>
        public void Write(String programOutput)
        {
            Console.Write(programOutput);
        }

        /// <summary>Writes a String to UI output, trailing with a newline</summary>
        /// <param name="programOutput">Output to write to display the user</param>
        public void WriteLine(String programOutput)
        {
            Console.WriteLine(programOutput);
        }

        /// <summary>Blocks execution waiting on user input.</summary>
        /// <param name="prompt">Prompt to display to the user</param>
        public void WaitForInput(String prompt)
        {
            if (prompt == null)
                prompt = "Press [Enter] to continue...";

            Console.Write(prompt);
            Console.ReadLine();
        }

        /// <summary>Blocks execution waiting on user input.</summary>
        /// <param name="prompt">Prompt to display to the user</param>
        public void WaitForInput()
        {
            this.WaitForInput(null);
        }
    }
}
