using System;

namespace Bardez.Projects.InfinityPlus1.Utility.UiInterceptor
{
    /// <summary>Interface dictating a user interface output</summary>
    public interface IUserInterfaceInterceptor
    {
        /// <summary>Writes a String to UI output, a large-scale message</summary>
        /// <param name="programOutput">Output to write to display the user</param>
        void WriteMessage(String programOutput);

        /// <summary>Writes a String to UI output, trailing with a newline</summary>
        /// <param name="programOutput">Output to write to display the user</param>
        void Write(String programOutput);

        /// <summary>Writes a String to UI output, trailing with a newline</summary>
        /// <param name="programOutput">Output to write to display the user</param>
        void WriteLine(String programOutput);

        /// <summary>Blocks execution waiting on user input.</summary>
        /// <param name="prompt">Prompt to display to the user</param>
        void WaitForInput(String prompt);

        /// <summary>Blocks execution waiting on user input.</summary>
        void WaitForInput();
    }
}