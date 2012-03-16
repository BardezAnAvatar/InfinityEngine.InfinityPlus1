using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.Movie
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.MveIndexer class.</summary>
    public class MveIndexerTest : FileTesterBase
    {
        #region Fields
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.MVE.Path";

        /// <summary>Format instance to test</summary>
        protected MveChunkIndexer Mve { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public MveIndexerTest()
        {
            this.InitializeInstance();
        }
        #endregion


        /// <summary>Initializes the test class data</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="e">Specific initialization event parameters</param>
        protected override void InitializeTestData(Object sender, EventArgs e)
        {
            this.FilePaths = ConfigurationHandlerMulti.GetSettingValues(MveIndexerTest.configKey);
        }

        /// <summary>Event to raise for testing instance(s)</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            using (FileStream stream = new FileStream(testArgs.Path, FileMode.Open, FileAccess.Read))
            {
                this.Mve = new MveChunkIndexer();
                this.Mve.Read(stream);
            }

            this.DoPostMessage(new MessageEventArgs(this.Mve.ToString(), "Output", testArgs.Path));
        }

        /// <summary>Writes the data structure back out to a destination stream</summary>
        /// <param name="destination">Stream to write output to</param>
        protected virtual void TestWrite(Stream destination)
        {
            this.Mve.Write(destination);
        }
    }
}