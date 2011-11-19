using System;
using System.Text;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.ChitinKey;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

namespace Bardez.Projects.InfinityPlus1.Test.ChitinKey
{
    public class ChitinKey1Test : ITester
    {
        /// <summary>Testing object</summary>
        protected ChitinKey1 keyFile;

        /// <summary>ITester Interface method</summary>
        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Key.Key1Path");            
            this.Test(path);
        }

        /// <summary>Tests the testing object</summary>
        /// <param name="filePath">String describing the location of a TLK file</param>
        public void Test(String filePath)
        {
            StringBuilder buffer = new StringBuilder();
            buffer.AppendLine("Testing read:\n");

            buffer.AppendLine("Initializing at " + DateTime.Now.TimeOfDay.ToString() + ":");            //timestamp
            this.keyFile = new ChitinKey1(true);
            this.keyFile.KeyFilePath = filePath;
            buffer.AppendLine("     Reading at " + DateTime.Now.TimeOfDay.ToString() + ":");            //timestamp
            this.keyFile.Read();

            buffer.AppendLine("     Printing at " + DateTime.Now.TimeOfDay.ToString() + ":");           //timestamp
            buffer.AppendLine(keyFile.ToString(false));
            buffer.AppendLine("     Finished Printing at " + DateTime.Now.TimeOfDay.ToString() + ":");  //timestamp

            Interceptor.WriteMessage(buffer.ToString());

            Interceptor.WaitForInput();
            
            //try to write
            Console.WriteLine("\n\n\nTesting write...\n");
            keyFile.Write(filePath + ".new.key");
        }
    }
}