using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Creature9;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.Creature
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Creature9.CreatureHeader1 class.</summary>
    public class Creature9Test : ITester
    {
        /// <summary>Test instance</summary>
        protected Creature9 creature;

        /// <summary>Test instance exposure</summary>
        public Creature9 Header
        {
            get { return this.creature; }
        }

        /// <summary>Test harness method, gets the path from the app.config file</summary>
        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Creature.Creature9Path");
            this.Test(path);
        }

        /// <summary>Test harness method, reads and re-writes the file from the path passed in, writing to a .rewrite extension</summary>
        /// <param name="path">Array of directory & filename combinations to read from</param>
        public void Test(String[] paths)
        {
            foreach (String path in paths)
            {
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    this.Test(stream);

                using (FileStream dest = new FileStream(path + ".rewrite", FileMode.OpenOrCreate, FileAccess.Write))
                    this.TestWrite(dest);
            }
        }

        /// <summary>Test harness method, reads and re-writes the file from the path passed in, writing to a .rewrite extension</summary>
        /// <param name="path">directory & filename to read from</param>
        public void Test(String path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                this.Test(stream);

            using (FileStream dest = new FileStream(path + ".rewrite", FileMode.OpenOrCreate, FileAccess.Write))
                this.TestWrite(dest);
        }

        /// <summary>Tests the read method(s) on the provided stream</summary>
        /// <param name="source">Stream to read from</param>
        public void Test(Stream source)
        {
            this.creature = new Creature9();
            this.creature.Read(source);

            Console.Write(this.creature.ToString());

            Console.Write("Press [Enter] to continue...");
            Console.ReadLine();
        }

        /// <summary>Tests the write method(s) on the provided stream</summary>
        /// <param name="destination">Stream to write to</param>
        public void TestWrite(Stream destination)
        {
            this.creature.Write(destination);
        }
    }
}