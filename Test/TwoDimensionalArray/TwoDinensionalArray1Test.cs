using System;
using System.IO;
using System.Text;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.TwoDimensionalArray._2DA1;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

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

            StringBuilder buffer = new StringBuilder();

            buffer.AppendLine(twoDeeAy.ToString());

            buffer.Append("\n\n\nGet a specific value: ");
            //conditional...
            buffer.AppendLine("[\"1\"][\"HitAnimation\"]: ");
            buffer.AppendLine(twoDeeAy["1"]["HitAnimation"]);

            Interceptor.WriteMessage(buffer.ToString());
        }
    }
}