using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WalledEnvironmentDisplay;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.WalledEnvironmentDisplay
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WalledEnvironmentDisplay.Wed1_3 class.</summary>
    public class WedTest : FileTesterBase
    {
        #region Constants
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.WED.Path";
        #endregion


        #region Fields
        /// <summary>Format instance to test</summary>
        protected Wed1_3 wed { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public WedTest()
        {
            this.InitializeInstance();
        }
        #endregion


        #region Harness methods
        /// <summary>Initializes the test class data</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="e">Specific initialization event parameters</param>
        protected override void InitializeTestData(Object sender, EventArgs e)
        {
            this.FilePaths = ConfigurationHandlerMulti.GetSettingValues(WedTest.configKey);
        }

        /// <summary>Event to raise for testing instance(s)</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            using (FileStream stream = new FileStream(testArgs.Path, FileMode.Open, FileAccess.Read))
            {
                this.wed = new Wed1_3();
                this.DoPostMessage(new MessageEventArgs("Reading WED file...", "Reading", testArgs.Path));
                this.wed.Read(stream);
                this.DoPostMessage(new MessageEventArgs(this.wed.ToString(), "Output", testArgs.Path));
            }

            using (FileStream dest = new FileStream(testArgs.Path + ".rewrite", FileMode.Create, FileAccess.Write))
                this.TestWrite(dest);
        }

        /// <summary>Writes the data structure back out to a destination stream</summary>
        /// <param name="destination">Stream to write output to</param>
        protected virtual void TestWrite(Stream destination)
        {
            this.wed.Write(destination);
        }
        #endregion
    }
}