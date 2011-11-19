using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Initialization;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

namespace Bardez.Projects.InfinityPlus1.Test.Initialization
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.StringReferenceCount.StringReferenceCount1 class.</summary>
    public class IniTest : ITester
    {
        protected InitConfig ini;

        public InitConfig Ini
        {
            get { return this.ini; }
        }

        public void Test()
        {
            String[] paths = ConfigurationHandlerMulti.GetSettingValues("Test.Initialization.IniPath").ToArray();
            this.TestMulti(paths);
        }
        
        /// <summary>Tests a single file</summary>
        /// <param name="path">File to open and read, then replicate</param>
        /// <param name="prompt">Boolean indicating whether or not to prompt between read and write</param>
        public void Test(String path, Boolean prompt)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                this.Test(stream, prompt);

            using (FileStream dest = new FileStream(path + ".rewrite", FileMode.Create, FileAccess.Write))
                this.TestWrite(dest);
        }

        /// <summary>Tests the code </summary>
        /// <param name="paths"></param>
        public void TestMulti(String[] paths)
        {
            foreach (String path in paths)
            {
                this.Test(path, false);
            }
        }

        /// <summary>Tests the read and ToString() methods of the structure</summary>
        /// <param name="source">Source Stream to read from</param>
        /// <param name="prompt">Boolean indicating whether or not to prompt for pressing [Enter] to continue</param>
        public void Test(Stream source, Boolean prompt)
        {
            this.ini = new InitConfig();
            this.ini.Read(source);

            Interceptor.WriteMessage(this.ini.ToString());

            if (prompt)
            {
                Interceptor.WaitForInput();
            }
        }

        /// <summary>Writes the data structure back out to a destination stream</summary>
        /// <param name="destination">Stream to write output to</param>
        public void TestWrite(Stream destination)
        {
            this.ini.Write(destination);
        }
    }
}