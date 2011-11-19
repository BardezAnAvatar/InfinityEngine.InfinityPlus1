using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Store;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Store.Version;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

namespace Bardez.Projects.InfinityPlus1.Test.Store
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.Store.Version.Store1_1 class.</summary>
    public class Store1_1Test : ITester
    {
        protected Store1_1 store;

        public Store1_1 Store
        {
            get { return this.store; }
        }

        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Store.Store1.1Path");
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
            this.store = new Store1_1();
            this.store.Read(Source);

            Interceptor.WriteMessage(this.store.ToString());

            Interceptor.WaitForInput();
        }

        public void TestWrite(Stream destination)
        {
            this.store.Write(destination);
        }
    }
}