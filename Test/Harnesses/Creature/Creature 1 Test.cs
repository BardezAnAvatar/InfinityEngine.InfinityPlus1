using System;
using System.IO;
using Bardez.Projects.Configuration;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Creature1;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

namespace Bardez.Projects.InfinityPlus1.Test.Creature
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Creature1.CreatureHeader1 class.</summary>
    public class Creature1Test : ITester
    {
        protected Creature1 creature;

        public Creature1 Header
        {
            get { return this.creature; }
        }

        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Creature.Creature1Path");
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
            this.creature = new Creature1();
            this.creature.Read(Source);

            Interceptor.WriteMessage(this.creature.ToString());

            Interceptor.WaitForInput();
        }

        public void TestWrite(Stream destination)
        {
            this.creature.Write(destination);
        }
    }
}