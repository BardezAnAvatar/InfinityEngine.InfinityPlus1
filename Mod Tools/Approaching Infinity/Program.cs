using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Bardez.Projects.InfinityPlus1.Tools.ApproachingInfinity.Forms;

namespace Bardez.Projects.InfinityPlus1.Tools.ApproachingInfinity
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}