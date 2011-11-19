using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Spell;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Spell.Spell1;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

namespace Bardez.Projects.InfinityPlus1.Test.Spell
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.Spell.SpellAbility class.</summary>
    public class SpellAbilityTest : ITester
    {
        protected Spell1Header header;
        protected SpellAbility ability;

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

            Source.Seek(this.header.OffsetAbilities, SeekOrigin.Begin);
            this.ability = new SpellAbility();
            this.ability.Read(Source);

            Interceptor.WriteMessage(this.ability.ToString());
        }

        public Spell1Header Header
        {
            get { return this.header; }
        }

        public SpellAbility Ability
        {
            get { return this.ability; }
        }
    }
}