using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.StringReferenceCount;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

namespace Bardez.Projects.InfinityPlus1.Test.StringReferenceCount
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.StringReferenceCount.StringReferenceCount1 class.</summary>
    public class Src1Test : ITester
    {
        protected StringReferenceCount1 biography;

        public StringReferenceCount1 Src
        {
            get { return this.biography; }
        }

        public void Test()
        {
            String[] paths = ConfigurationHandlerMulti.GetSettingValues("Test.StringReference.Src1Path").ToArray();
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
            this.biography = new StringReferenceCount1();
            this.biography.Read(source);

            Interceptor.WriteMessage(this.biography.ToString());

            if (prompt)
            {
                Interceptor.WaitForInput();
            }
        }

        /// <summary>Writes the data structure back out to a destination stream</summary>
        /// <param name="destination">Stream to write output to</param>
        public void TestWrite(Stream destination)
        {
            this.biography.Write(destination);
        }
    }
}