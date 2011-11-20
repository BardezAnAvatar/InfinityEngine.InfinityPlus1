using System;
using System.Windows.Forms;

using Bardez.Projects.ExceptionHandler;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Test.AmpitudeCodedModulation;
using Bardez.Projects.InfinityPlus1.Test.BiowareIndexFileFormat.Biff1;
using Bardez.Projects.InfinityPlus1.Test.Character;
using Bardez.Projects.InfinityPlus1.Test.ChitinKey;
using Bardez.Projects.InfinityPlus1.Test.Creature;
using Bardez.Projects.InfinityPlus1.Test.Dialog;
using Bardez.Projects.InfinityPlus1.Test.Effect;
using Bardez.Projects.InfinityPlus1.Test.Initialization;
using Bardez.Projects.InfinityPlus1.Test.Item;
using Bardez.Projects.InfinityPlus1.Test.Music;
using Bardez.Projects.InfinityPlus1.Test.Output.DirectX;
using Bardez.Projects.InfinityPlus1.Test.ReusableCode;
using Bardez.Projects.InfinityPlus1.Test.Riff;
using Bardez.Projects.InfinityPlus1.Test.Spell;
using Bardez.Projects.InfinityPlus1.Test.StringReferenceCount;
using Bardez.Projects.InfinityPlus1.Test.Store;
using Bardez.Projects.InfinityPlus1.Test.TextLocationKey;
using Bardez.Projects.InfinityPlus1.Test.TwoDimensionalArray;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

using Bardez.Projects.InfinityPlus1.Test.WinForm.ACM;

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
            Application.Run(new TestUI());
            
            //ITester tester = new PlaylistTest();
            //tester.Test();
            //Interceptor.WaitForInput();
        }
    }
}