using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.External.Interplay.ACM;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.AmpitudeCodedModulation
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.External.Interplay.ACM.Header class.</summary>
    public class AcmHeaderTest : FileTesterBase
    {
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.ACM.AcmPath";

        protected AcmHeader acmFile { get; set; }

        /// <summary>Default constructor</summary>
        public AcmHeaderTest()
        {
            this.InitializeInstance();
        }

        /// <summary>Initializes the test class data</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="e">Specific initialization event parameters</param>
        protected override void InitializeTestData(object sender, EventArgs e)
        {
            this.FilePaths = ConfigurationHandlerMulti.GetSettingValues(AcmHeaderTest.configKey);
        }

        /// <summary>Event to raise for testing a specific value</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            using (FileStream stream = new FileStream(testArgs.Path, FileMode.Open, FileAccess.Read))
                this.TestRead(stream);

            //using (FileStream dest = new FileStream(path + ".rewrite", FileMode.Create, FileAccess.Write))
            //    this.TestWrite(dest);
        }

        /// <summary>Tests the read and ToString() methods of the structure</summary>
        /// <param name="source">Source Stream to read from</param>
        protected void TestRead(Stream source)
        {
            this.acmFile = new AcmHeader();
            this.acmFile.Read(source);
            this.DoPostMessage(new MessageEventArgs(this.acmFile.ToString()));
        }

        /// <summary>Writes the data structure back out to a destination stream</summary>
        /// <param name="destination">Stream to write output to</param>
        protected void TestWrite(Stream destination)
        {
            this.acmFile.Write(destination);
        }
    }
}