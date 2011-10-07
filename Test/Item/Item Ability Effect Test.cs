using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Effect;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Effect.Effect1;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Item;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Item.Item1;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.Item
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.Common.ItmSpl.ItemSpellAbilityEffect class.</summary>
    public class ItemSpellAbilityEffectTest : ITester
    {
        protected ItemHeader1 header;
        protected Effect1 effect;

        #region Properties
        public ItemHeader1 Header
        {
            get { return this.header; }
        }

        public Effect1 Effect
        {
            get { return this.effect; }
        }
        #endregion

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

            Source.Seek(this.header.OffsetAbilityEffects, SeekOrigin.Begin);
            this.effect = new Effect1();
            this.effect.Read(Source);

            Console.Write(this.effect.ToString());
        }
    }
}