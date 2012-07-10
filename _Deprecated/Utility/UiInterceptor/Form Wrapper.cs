using System;
using System.Windows.Forms;

namespace Bardez.Projects.Utility.UiInterceptor
{
    /// <summary>Wraps certain Console calls with more easily replacable alternatives</summary>
    public class FormWrapper : IUserInterfaceInterceptor
    {
        /// <summary>Singleton instance</summary>
        /// <remarks>Static constructor</remarks>
        private static FormWrapper singleton = null;

        /// <summary>Locking object</summary>
        private static Object singletonLocker = new Object();

        /// <summary>Default constructor</summary>
        private FormWrapper() { }

        public static FormWrapper Instance
        {
            get
            {
                lock (FormWrapper.singletonLocker)
                {
                    if (FormWrapper.singleton == null)
                        FormWrapper.singleton = new FormWrapper();

                    return FormWrapper.singleton;
                }
            }
        }

        /// <summary>Writes a String to UI output, trailing with a newline</summary>
        /// <param name="programOutput">Output to write to display the user</param>
        public void WriteMessage(String programOutput)
        {
            MessageBox.Show(programOutput, "Message", MessageBoxButtons.OK);
        }

        /// <summary>Writes a String to UI output, trailing with a newline</summary>
        /// <param name="programOutput">Output to write to display the user</param>
        public void Write(String programOutput)
        {
            MessageBox.Show(programOutput);
        }

        /// <summary>Writes a String to UI output, trailing with a newline</summary>
        /// <param name="programOutput">Output to write to display the user</param>
        public void WriteLine(String programOutput)
        {
            MessageBox.Show(programOutput);
        }

        /// <summary>Blocks execution waiting on user input.</summary>
        /// <param name="prompt">Prompt to display to the user</param>
        public void WaitForInput(String prompt)
        {
            if (prompt == null)
                prompt = "Press [OK] to continue...";

            MessageBox.Show(prompt);
        }

        /// <summary>Blocks execution waiting on user input.</summary>
        /// <param name="prompt">Prompt to display to the user</param>
        public void WaitForInput()
        {
            this.WaitForInput(null);
        }
    }
}
