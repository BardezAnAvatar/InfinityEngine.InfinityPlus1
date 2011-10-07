using System;
using System.IO;
using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.TwoDimensionalArray._2DA1;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.TwoDimensionalArray
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.TwoDimensionalArray._2DA1 class</summary>
    public class TwoDinensionalArray1Test : ITester
    {
        protected TwoDimensionalArray1 twoDeeAy;
        public void Test()
        {
            this.Test(ConfigurationHandler.GetSettingValue("Test.2Da1.2daPath"));
        }

        public void Test(String Path)
        {
            using (FileStream stream = new FileStream(Path, FileMode.Open))
                Test(stream);
        }

        public void Test(Stream source)
        {
            twoDeeAy = new TwoDimensionalArray1();
            twoDeeAy.Read(source);

            Console.Write(twoDeeAy.ToString());

            Console.Write("\n\n\nGet a specific value: ");
            //conditional...
            Console.WriteLine("[\"1\"][\"HitAnimation\"]: ");
            Console.WriteLine(twoDeeAy["1"]["HitAnimation"]);
        }
    }
}