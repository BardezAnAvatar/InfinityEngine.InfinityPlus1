using System;
using System.Windows.Forms;

using Bardez.Projects.ExceptionHandler;

namespace Bardez.Projects.InfinityPlus1.Tester
{
    /// <summary>Main program class/entry point object.</summary>
    internal class Program
    {
        /// <summary>Driving main method application entry point</summary>
        /// <param name="args">String Array of application arguments</param>
        [STAThread]
        internal static void Main(String[] args)
        {
            ExceptionManager.AttachManagerForConsole();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new TestUI());
            Application.Run(new Scratch());

            //ITester tester = new PlaylistTest();
            //tester.Test();
            //Interceptor.WaitForInput();

            //I need a singleton resource container for end form disposal; set up when started, released when closing. for now, just dispose here.
            Bardez.Projects.InfinityPlus1.Output.Audio.XAudio2Output.Instance.Dispose();
            Bardez.Projects.InfinityPlus1.Output.Visual.Direct2dResourceManager.Instance.Dispose(); //clear out all the device-dependent resources
        }
    }
}