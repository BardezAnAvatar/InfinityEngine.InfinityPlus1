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
    public class CreatureHeader1Test : ITester
    {
        protected Creature1Header header;

        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Creature.Creature1Path");
            this.Test(path);
        }

        public void Test(String Path)
        {
            using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                this.Test(stream);
        }

        public void Test(Stream Source)
        {
            this.header = new Creature1Header();
            this.header.Read(Source);

            Interceptor.WriteMessage(this.header.ToString());
        }

        public Creature1Header Header
        {
            get { return this.header; }
        }
    }
}