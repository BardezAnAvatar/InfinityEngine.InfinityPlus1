using System;
using System.IO;
using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Creature9;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.Creature
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Creature9.CreatureHeader1 class.</summary>
    public class CreatureHeader9Test : ITester
    {
        protected Creature9Header header;

        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Creature.Creature9Path");
            this.Test(path);
        }

        public void Test(String Path)
        {
            using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                this.Test(stream);
        }

        public void Test(Stream Source)
        {
            this.header = new Creature9Header();
            this.header.Read(Source);

            Console.Write(this.header.ToString());
        }

        public Creature9Header Header
        {
            get { return this.header; }
        }
    }
}