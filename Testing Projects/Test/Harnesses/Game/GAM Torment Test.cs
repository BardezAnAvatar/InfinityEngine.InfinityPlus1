using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Version;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.Game
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Version.GameTorment class.</summary>
    public class GameTormentTest : FileTesterBase
    {
        #region Constants
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.GAM.Torment.Path";
        #endregion


        #region Fields
        /// <summary>Format instance to test</summary>
        protected GameTorment gam { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public GameTormentTest()
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
            this.FilePaths = ConfigurationHandlerMulti.GetSettingValues(GameTormentTest.configKey);
        }

        /// <summary>Event to raise for testing instance(s)</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            using (FileStream stream = new FileStream(testArgs.Path, FileMode.Open, FileAccess.Read))
            {
                this.gam = new GameTorment();
                this.DoPostMessage(new MessageEventArgs("Reading Torment GAM file...", "Reading", testArgs.Path));
                this.gam.Read(stream);
                this.DoPostMessage(new MessageEventArgs(this.gam.ToString(), "Output", testArgs.Path));
            }

            using (FileStream dest = new FileStream(testArgs.Path + ".rewrite", FileMode.Create, FileAccess.Write))
                this.TestWrite(dest);
        }

        /// <summary>Writes the data structure back out to a destination stream</summary>
        /// <param name="destination">Stream to write output to</param>
        protected virtual void TestWrite(Stream destination)
        {
            this.gam.Write(destination);
        }
        #endregion
    }
}