using System;
using System.IO;
using System.Text;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.BiowareIndexFileFormat.Biff1;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

namespace Bardez.Projects.InfinityPlus1.Test.BiowareIndexFileFormat.Biff1
{
    public class Biff1ArchiveTest : ITester
    {
        protected Biff1Archive archive;

        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Biff1.BiffPath");
            this.Test(path);
        }

        //public void Test(String Path)
        //{
        //    using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
        //        this.Test(stream);
        //}

        public void Test(String path)
        {
            this.archive = new Biff1Archive();
            this.archive.Read(path);

            Interceptor.WriteMessage(this.archive.ToString());
        }

        public void Test(Stream Source)
        {
            this.archive = new Biff1Archive();
            this.archive.Read(Source);

            Interceptor.WriteMessage(this.archive.ToString());
        }
    }
}