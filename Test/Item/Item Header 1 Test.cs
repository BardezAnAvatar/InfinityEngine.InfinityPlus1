using System;
using System.IO;
using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Item;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Item.Item1;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.Item
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.Item.Item1.ItemHeader1 class.</summary>
    public class ItemHeader1Test : ITester
    {
        protected ItemHeader1 header;

        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Item.Item1Path");
            this.Test(path);
        }

        public void Test(String Path)
        {
            using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                this.Test(stream);
        }

        public void Test(Stream Source)
        {
            this.header = new ItemHeader1();
            this.header.Read(Source);

            Console.Write(this.header.ToString());
        }

        public ItemHeader1 Header
        {
            get { return this.header; }
        }
    }
}