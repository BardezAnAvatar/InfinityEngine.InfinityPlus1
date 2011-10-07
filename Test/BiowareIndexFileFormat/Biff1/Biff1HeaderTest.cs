using System;
using System.IO;
using System.Text;
using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.BiowareIndexFileFormat.Biff1;

namespace Bardez.Projects.InfinityPlus1.Test.BiowareIndexFileFormat.Biff1
{
    public class Biff1HeaderTest : ITester
    {
        protected Biff1Header header;

        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Biff1.BiffPath");
            this.Test(path);
        }

        public void Test(String Path)
        {
            using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                this.Test(stream);
        }

        public void Test(Stream Source)
        {
            this.header = new Biff1Header();
            this.header.Read(Source);

            Console.Write(this.header.ToString());
        }
    }
}