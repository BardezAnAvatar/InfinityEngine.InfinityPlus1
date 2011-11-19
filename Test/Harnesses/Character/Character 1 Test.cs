using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Character;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Character.Version;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

namespace Bardez.Projects.InfinityPlus1.Test.Character
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.Character.Version.Character1 class.</summary>
    public class Character1Test : ITester
    {
        protected Character1 character;

        public Character1 Character
        {
            get { return this.character; }
        }

        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Character.Character1Path");
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
            this.character = new Character1();
            this.character.Read(Source);

            Interceptor.WriteMessage(this.character.ToString());

            Interceptor.WaitForInput();
        }

        public void TestWrite(Stream destination)
        {
            this.character.Write(destination);
        }
    }
}