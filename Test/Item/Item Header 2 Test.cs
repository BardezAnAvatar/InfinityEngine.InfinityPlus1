using System;
using System.IO;
using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Item;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Item.Item2;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.Item
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.Item.Item2.ItemHeader2 class.</summary>
    public class ItemHeader2Test : ITester
    {
        protected ItemHeader2 header;

        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Item.Item2Path");
            this.Test(path);
        }

        public void Test(String Path)
        {
            using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                this.Test(stream);
        }

        public void Test(Stream Source)
        {
            this.header = new ItemHeader2();
            this.header.Read(Source);

            Console.Write(this.header.ToString());
        }

        public ItemHeader2 Header
        {
            get { return this.header; }
        }
    }
}