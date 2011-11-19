using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Item;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Item.Item1_1;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

namespace Bardez.Projects.InfinityPlus1.Test.Item
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.Item.Item1.Item1 class.</summary>
    public class Item1_1Test : ITester
    {
        protected Item1_1 item;

        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Item.Item1_1Path");
            this.Test(path);
        }

        public void Test(String Path)
        {
            using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                this.Test(stream);

            using (FileStream dest = new FileStream(Path + ".rewrite", FileMode.OpenOrCreate, FileAccess.Write))
                this.TestWrite(dest);
        }

        public void Test(Stream Source)
        {
            this.item = new Item1_1();
            this.item.Read(Source);

            Interceptor.WriteMessage(this.item.ToString());

            Interceptor.WaitForInput();
        }

        public void TestWrite(Stream destination)
        {
            this.item.Write(destination);
        }

        public Item1_1 Item
        {
            get { return this.item; }
        }
    }
}