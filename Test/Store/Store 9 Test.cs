using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Store;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Store.Version;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.Store
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.Store.Version.Store9 class.</summary>
    public class Store9Test : ITester
    {
        protected Store9 store;

        public Store9 Store
        {
            get { return this.store; }
        }

        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Store.Store9Path");
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
            this.store = new Store9();
            this.store.Read(Source);

            Console.Write(this.store.ToString());

            Console.Write("Press [Enter] to continue...");
            Console.ReadLine();
        }

        public void TestWrite(Stream destination)
        {
            this.store.Write(destination);
        }
    }
}