﻿using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Effect;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Effect.Effect2;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

namespace Bardez.Projects.InfinityPlus1.Test.Effect
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.Effect.Effect2.Effect2 class.</summary>
    public class Effect2Test : ITester
    {
        protected Effect2 effect;

        public Effect2 Item
        {
            get { return this.effect; }
        }

        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Effect.Effect2Path");
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
            this.effect = new Effect2();
            this.effect.Read(Source);

            Interceptor.WriteMessage(this.effect.ToString());

            Interceptor.WaitForInput();
        }

        public void TestWrite(Stream destination)
        {
            this.effect.Write(destination);
        }
    }
}