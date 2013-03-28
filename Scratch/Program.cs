using System;

namespace Scratch
{
    /// <summary>Main program class/entry point object.</summary>
    internal class Program
    {
        /// <summary>Driving main method application entry point</summary>
        /// <param name="args">String Array of application arguments</param>
        [MTAThread]
        internal static void Main(String[] args)
        {
            XAudio2_reimp test = new XAudio2_reimp();
            //test.TestSomeXAudio2Stuff();
            //test.XAudio2_ThreeeeeeDeeeeeeee_test();
            //test.TestXAudio2IRenderer();
            test.TestXAudio2Reverb();
        }
    }
}