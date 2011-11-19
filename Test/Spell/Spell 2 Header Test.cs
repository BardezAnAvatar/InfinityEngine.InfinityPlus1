using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Spell;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Spell.Spell2;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

namespace Bardez.Projects.InfinityPlus1.Test.Spell
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.Spell.Spell2.Spell2Header class.</summary>
    public class SpellHeader2Test : ITester
    {
        protected Spell2Header header;

        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Spell.Spell2Path");
            this.Test(path);
        }

        public void Test(String Path)
        {
            using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                this.Test(stream);
        }

        public void Test(Stream Source)
        {
            this.header = new Spell2Header();
            this.header.Read(Source);

            Interceptor.WriteMessage(this.header.ToString());
        }

        public Spell2Header Header
        {
            get { return this.header; }
        }
    }
}