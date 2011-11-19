using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Creature9;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

namespace Bardez.Projects.InfinityPlus1.Test.Creature
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Creature9.CreatureHeader1 class.</summary>
    public class CreatureHeader9Test : ITester
    {
        public Creature9Header Header { get; set; }

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
            this.Header = new Creature9Header();
            this.Header.Read(Source);

            Interceptor.WriteMessage(this.Header.ToString());
        }
    }
}