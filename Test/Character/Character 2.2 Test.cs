using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Character;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Character.Version;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.Character
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.Character.Version.Character2_2 class.</summary>
    public class Character2_2Test : ITester
    {
        protected Character2_2 character;

        public Character2_2 Character
        {
            get { return this.character; }
        }

        public void Test()
        {
            //String path = ConfigurationHandler.GetSettingValue("Test.Character.Character2.2Path");
            //this.Test(path);

            String[] paths = ConfigurationHandlerMulti.GetSettingValues("Test.Character.Character2.2Path").ToArray();
            this.TestMulti(paths);
        }

        /// <summary>Tests a single file</summary>
        /// <param name="path">File to open and read, then replicate</param>
        /// <param name="prompt">Boolean indicating whether or not to prompt between read and write</param>
        public void Test(String path, Boolean prompt)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                this.Test(stream, prompt);

            using (FileStream dest = new FileStream(path + ".rewrite", FileMode.Create, FileAccess.Write))
                this.TestWrite(dest);
        }

        /// <summary>Tests the code </summary>
        /// <param name="paths"></param>
        public void TestMulti(String[] paths)
        {
            foreach (String path in paths)
            {
                this.Test(path, false);
            }
        }
        
        /// <summary>Tests the read and ToString() methods of the structure</summary>
        /// <param name="source">Source Stream to read from</param>
        /// <param name="prompt">Boolean indicating whether or not to prompt for pressing [Enter] to continue</param>
        public void Test(Stream source, Boolean prompt)
        {
            this.character = new Character2_2();
            this.character.Read(source);

            Console.Write(this.character.ToString());

            if (prompt)
            {
                Console.Write("Press [Enter] to continue...");
                Console.ReadLine();
            }
        }

        /// <summary>Writes the data structure back out to a destination stream</summary>
        /// <param name="destination">Stream to write output to</param>
        public void TestWrite(Stream destination)
        {
            this.character.Write(destination);
        }
    }
}