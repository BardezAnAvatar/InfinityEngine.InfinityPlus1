using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.TextLocationKey;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.TextLocationKey
{
    /// <summary>This class tests the usable methods in the InfinityPlus1.Files.TextLocationKeyHeader class</summary>
    public class TextLocationKeyHeaderTest : ITester
    {
        protected TextLocationKeyHeader header;

        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Tlk1.Tlk1Path");
            Test(path);
        }

        public void Test(String Path)
        {
            using (FileStream stream = new FileStream(Path, FileMode.Open))
                Test(stream);
        }

        public void Test(Stream Source)
        {
            header = new TextLocationKeyHeader();
            header.ReadHeader(Source);

            Console.Write(header.ToString());
        }

        public TextLocationKeyHeader Header
        {
            get { return header; }
        }
    }
}