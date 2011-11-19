using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Spell;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Spell.Spell1;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

namespace Bardez.Projects.InfinityPlus1.Test.Spell
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.Spell.Spell1.Spell1Header class.</summary>
    public class SpellHeader1Test : ITester
    {
        protected Spell1Header header;

        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Spell.Spell1Path");
            this.Test(path);
        }

        public void Test(String Path)
        {
            using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                this.Test(stream);
        }

        public void Test(Stream Source)
        {
            this.header = new Spell1Header();
            this.header.Read(Source);

            Interceptor.WriteMessage(this.header.ToString());
        }

        public Spell1Header Header
        {
            get { return this.header; }
        }
    }
}