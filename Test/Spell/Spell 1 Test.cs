﻿using System;
using System.IO;
using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Spell;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Spell.Spell1;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.Spell
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.Spell.Spell1.Spell1 class.</summary>
    public class Spell1Test : ITester
    {
        protected Spell1 item;

        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Spell.Spell1Path");
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
            this.item = new Spell1();
            this.item.Read(Source);

            Console.Write(this.item.ToString());

            Console.Write("Press [Enter] to continue...");
            Console.ReadLine();
        }

        public void TestWrite(Stream destination)
        {
            this.item.Write(destination);
        }

        public Spell1 Spell
        {
            get { return this.item; }
        }
    }
}