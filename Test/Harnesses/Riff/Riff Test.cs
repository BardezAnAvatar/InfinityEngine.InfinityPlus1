using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.External.RIFF.Component;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.Riff
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.External.RIFF.Component.RiffHeader class.</summary>
    public class RiffTest : FileTesterBase
    {
        #region Fields
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.Riff.RiffPath";

        /// <summary>Format instance to test</summary>
        protected RiffFile Riff { get; set; }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public RiffTest()
        {
            this.InitializeInstance();
        }
        #endregion

        /// <summary>Initializes the test class data</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="e">Specific initialization event parameters</param>
        protected override void InitializeTestData(Object sender, EventArgs e)
        {
            this.FilePaths = ConfigurationHandlerMulti.GetSettingValues(RiffTest.configKey);
        }

        /// <summary>Event to raise for testing instance(s)</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            using (FileStream stream = new FileStream(testArgs.Path, FileMode.Open, FileAccess.Read))
            {
                this.Riff = new RiffFile();
                this.Riff.Read(stream);

                //has to be inside the opened file block due to RIFF code
                this.DoPostMessage(new MessageEventArgs(this.Riff.ToString(), "Output", testArgs.Path));
            }

            //no rewrite at the moment
        }
    }
}