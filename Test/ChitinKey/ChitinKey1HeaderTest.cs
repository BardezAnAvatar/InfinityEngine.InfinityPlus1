using System;
using System.IO;
using Bardez.Projects.InfinityPlus1.Files.Infinity.ChitinKey;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.ChitinKey
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.ChitinKey.ChitinKeyHeader class.</summary>
    public class ChitinKey1HeaderTest : ITester
    {
        protected ChitinKeyHeader header;

        public void Test()
        {
            String path;
            //path = @"E:\Test Data\Infinity Engine\BG\";
            path = @"E:\Test Data\Infinity Engine\BG2\";
            //path = @"E:\Test Data\Infinity Engine\IWD\";
            //path = @"E:\Test Data\Infinity Engine\IWD2\";
            //path = @"E:\Test Data\Infinity Engine\PST\";
            path += "chitin.key";
            this.Test(path);
        }

        public void Test(String Path)
        {
            using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                this.Test(stream);
        }

        public void Test(Stream Source)
        {
            this.header = new ChitinKeyHeader();
            this.header.Read(Source);

            Console.Write(this.header.ToString());
        }

        public ChitinKeyHeader Header
        {
            get { return this.header; }
        }
    }
}