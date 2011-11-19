using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Item;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Item.Item1;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

namespace Bardez.Projects.InfinityPlus1.Test.Item
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.Item.ItemAbility class.</summary>
    public class ItemAbilityTest : ITester
    {
        protected ItemHeader1 header;
        protected ItemAbility ability;

        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Item.Item1Path");
            this.Test(path);
        }

        public void Test(String Path)
        {
            using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                this.Test(stream);
        }

        public void Test(Stream Source)
        {
            this.header = new ItemHeader1();
            this.header.Read(Source);

            Source.Seek(this.header.OffsetAbilities, SeekOrigin.Begin);
            this.ability = new ItemAbility();
            this.ability.Read(Source);

            Interceptor.WriteMessage(this.ability.ToString());
        }

        public ItemHeader1 Header
        {
            get { return this.header; }
        }

        public ItemAbility Ability
        {
            get { return this.ability; }
        }
    }
}