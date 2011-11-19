using System;
using System.Text;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.TextLocationKey;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

namespace Bardez.Projects.InfinityPlus1.Test.TextLocationKey
{
    public class TextLocationKeyTest : ITester
    {
        /// <summary>Testing object</summary>
        protected Files.Infinity.TextLocationKey.TextLocationKey tlkFile;

        /// <summary>ITester Interface method</summary>
        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Tlk1.Tlk1Path");
            Test(path);
        }

        /// <summary>Tests the testing object</summary>
        /// <param name="filePath">String describing the location of a TLK file</param>
        public void Test(String filePath)
        {
            StringBuilder buffer = new StringBuilder();

            buffer.AppendLine("Testing ad hoc read:\n");

            //Console.WriteLine("Initializing at " + DateTime.Now.TimeOfDay.ToString() + ":");        //timestamp
            tlkFile = new Files.Infinity.TextLocationKey.TextLocationKey(false);
            tlkFile.TlkPath = filePath;
            //Console.WriteLine("     Reading at " + DateTime.Now.TimeOfDay.ToString() + ":");        //timestamp
            tlkFile.ReadFile();

            //Console.Write(tlkFile.ToString());        //printing the entire dialog.tlk...that will take far too long.
            buffer.AppendLine(tlkFile.ToString(false));

            buffer.Append("String Reference #25641:");
            buffer.AppendLine(tlkFile[25641].ToString());


            //try to write
            buffer.AppendLine("\n\n\nTesting no-read TLK write...\n");
            tlkFile.WriteFile(filePath + ".new.tlk");




            buffer.AppendLine("\n\n\nTesting full read:\n");

            //Console.WriteLine("Initializing at " + DateTime.Now.TimeOfDay.ToString() + ":");        //timestamp
            tlkFile = new Files.Infinity.TextLocationKey.TextLocationKey();
            tlkFile.TlkPath = filePath;
            //Console.WriteLine("     Reading at " + DateTime.Now.TimeOfDay.ToString() + ":");        //timestamp
            tlkFile.ReadFile();

            //Console.Write(tlkFile.ToString());        //printing the entire dialog.tlk...that will take far too long.
            buffer.AppendLine(tlkFile.ToString(false));

            buffer.Append("String Reference #12345:");
            buffer.AppendLine(tlkFile[12345].ToString());


            //try to write
            buffer.AppendLine("\n\n\nTesting full write...\n");
            Interceptor.WriteMessage(buffer.ToString());

            tlkFile.WriteFile(filePath + ".new2.tlk");
        }
    }
}