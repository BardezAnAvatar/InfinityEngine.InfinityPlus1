using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WalledEnvironmentDisplay;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.WalledEnvironmentDisplay
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WalledEnvironmentDisplay.WedHeader class.</summary>
    public class WedHeaderTest : FileTesterBase
    {
        #region Constants
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.WED.Path";
        #endregion


        #region Fields
        /// <summary>Format instance to test</summary>
        protected WedHeader header { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public WedHeaderTest()
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
            this.FilePaths = ConfigurationHandlerMulti.GetSettingValues(WedHeaderTest.configKey);
        }

        /// <summary>Event to raise for testing instance(s)</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            using (FileStream stream = new FileStream(testArgs.Path, FileMode.Open, FileAccess.Read))
            {
                this.header = new WedHeader();
                this.DoPostMessage(new MessageEventArgs("Reading WED header...", "Reading", testArgs.Path));
                this.header.Read(stream);
                this.DoPostMessage(new MessageEventArgs(this.header.ToString(), "Output", testArgs.Path));
            }

            using (FileStream dest = new FileStream(testArgs.Path + ".header.rewrite", FileMode.Create, FileAccess.Write))
                this.TestWrite(dest);
        }

        /// <summary>Writes the data structure back out to a destination stream</summary>
        /// <param name="destination">Stream to write output to</param>
        protected virtual void TestWrite(Stream destination)
        {
            this.header.Write(destination);
        }
        #endregion
    }
}