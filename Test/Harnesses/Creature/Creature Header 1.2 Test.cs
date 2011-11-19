using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Creature1_2;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

namespace Bardez.Projects.InfinityPlus1.Test.Creature
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Creature1.CreatureHeader1 class.</summary>
    public class CreatureHeader1_2Test : ITester
    {
        protected Creature1_2Header header;

        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Creature.Creature1.2Path");
            this.Test(path);
        }

        public void Test(String Path)
        {
            using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                this.Test(stream);
        }

        public void Test(Stream Source)
        {
            this.header = new Creature1_2Header();
            this.header.Read(Source);

            Interceptor.WriteMessage(this.header.ToString());
        }

        public Creature1_2Header Header
        {
            get { return this.header; }
        }
    }
}