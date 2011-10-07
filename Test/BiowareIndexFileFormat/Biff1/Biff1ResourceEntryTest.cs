using System;
using System.IO;
using System.Text;
using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.BiowareIndexFileFormat.Biff1;

namespace Bardez.Projects.InfinityPlus1.Test.BiowareIndexFileFormat.Biff1
{
    public class Biff1ResourceEntryTest : ITester
    {
        protected Biff1ResourceEntry resEntry;

        public void Test()
        {
            this.Test(ConfigurationHandler.GetSettingValue("Test.Biff1.BiffPath"));
        }

        public void Test(String Path)
        {
            using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                this.Test(stream);
        }

        public void Test(Stream Source)
        {
            this.resEntry = new Biff1ResourceEntry();
            Source.Seek(Int32.Parse(ConfigurationHandler.GetSettingValue("Test.Biff1.ResourceEntry.Offset")), SeekOrigin.Begin);
            this.resEntry.Read(Source);
            Console.Write(this.resEntry.ToString());
        }
    }
}