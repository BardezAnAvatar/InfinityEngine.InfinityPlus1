using System;
using System.IO;
using System.Text;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.BiowareIndexFileFormat.Biff1;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

namespace Bardez.Projects.InfinityPlus1.Test.BiowareIndexFileFormat.Biff1
{
    public class Biff1TilesetEntryTest : ITester
    {
        protected Biff1TilesetEntry tisEntry;

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
            this.tisEntry = new Biff1TilesetEntry();
            Source.Seek(Int32.Parse(ConfigurationHandler.GetSettingValue("Test.Biff1.TilesetEntry.Offset")), SeekOrigin.Begin);
            this.tisEntry.Read(Source);
            Interceptor.WriteMessage(this.tisEntry.ToString());
        }
    }
}