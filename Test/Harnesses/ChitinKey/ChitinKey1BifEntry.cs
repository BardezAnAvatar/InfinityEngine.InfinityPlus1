using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.Files.Infinity.ChitinKey;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

namespace Bardez.Projects.InfinityPlus1.Test.ChitinKey
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.ChitinKey.ChitinKeyBifEntry class.</summary>
    public class ChitinKey1BifEntryTest : ITester
    {
        protected ChitinKeyBifEntry entry;

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
            this.entry = new ChitinKeyBifEntry();
            Source.Seek(24, SeekOrigin.Begin);
            this.entry.Read(Source);

            Interceptor.WriteMessage(this.entry.ToString());
        }

        public ChitinKeyBifEntry Entry
        {
            get { return this.entry; }
        }
    }
}