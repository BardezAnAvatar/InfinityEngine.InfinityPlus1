using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WorldMap;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.WorldMap
{
    /// <summary>Testing harness for worldmap files</summary>
    public class WorldmapTest : FileTesterBase
    {
        #region Constants
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.WMP.Path";
        #endregion


        #region Fields
        /// <summary>Format instance to test</summary>
        protected WorldMap1 worldMap { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public WorldmapTest()
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
            this.FilePaths = ConfigurationHandlerMulti.GetSettingValues(WorldmapTest.configKey);
        }

        /// <summary>Event to raise for testing instance(s)</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            using (FileStream stream = new FileStream(testArgs.Path, FileMode.Open, FileAccess.Read))
            {
                this.worldMap = new WorldMap1();
                this.DoPostMessage(new MessageEventArgs("Reading WMP file...", "Reading", testArgs.Path));
                this.worldMap.Read(stream);
                this.DoPostMessage(new MessageEventArgs(this.worldMap.ToString(), "Output", testArgs.Path));
            }

            using (FileStream dest = new FileStream(testArgs.Path + ".rewrite", FileMode.Create, FileAccess.Write))
                this.TestWrite(dest);
        }

        /// <summary>Writes the data structure back out to a destination stream</summary>
        /// <param name="destination">Stream to write output to</param>
        protected virtual void TestWrite(Stream destination)
        {
            this.worldMap.Write(destination);
        }
        #endregion
    }
}