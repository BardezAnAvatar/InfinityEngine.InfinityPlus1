using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Character;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Character.Version;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.Character
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.Character.Version.Character9 class.</summary>
    public class Character9Test : ITester
    {
        protected Character9 character;

        public Character9 Character
        {
            get { return this.character; }
        }

        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Character.Character9Path");
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
            this.character = new Character9();
            this.character.Read(Source);

            Console.Write(this.character.ToString());

            Console.Write("Press [Enter] to continue...");
            Console.ReadLine();
        }

        public void TestWrite(Stream destination)
        {
            this.character.Write(destination);
        }
    }
}