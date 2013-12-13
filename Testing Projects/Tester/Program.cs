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
        [MTAThread]
        internal static void Main(String[] args)
        {
            ExceptionManager.AttachManagerForWinForms();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TestUI());
            //Application.Run(new Scratch());
            //Application.Run(new ScratchSingleD2D());


            //I need a singleton resource container for end form disposal; set up when started, released when closing. for now, just dispose here.
            Bardez.Projects.InfinityPlus1.Output.Audio.XAudio2Output.DisposeInstance();
            Bardez.Projects.InfinityPlus1.Output.Visual.Direct2dResourceManager.DisposeInstance(); //clear out all the device-dependent resources
        }
    }
}