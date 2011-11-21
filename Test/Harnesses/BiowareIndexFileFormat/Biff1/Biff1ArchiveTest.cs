using System;
using System.IO;
using System.Text;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.BiowareIndexFileFormat.Biff1;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.BiowareIndexFileFormat.Biff1
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.BiowareIndexFileFormat.Biff1.Biff1Archive class.</summary>
    public class Biff1ArchiveTest : FileTesterBase
    {
        #region Fields
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.Biff1.BiffPath";

        /// <summary>Format instance to test</summary>
        protected Biff1Archive Archive { get; set; }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public Biff1ArchiveTest()
        {
            this.InitializeInstance();
        }
        #endregion

        /// <summary>Initializes the test class data</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="e">Specific initialization event parameters</param>
        protected override void InitializeTestData(Object sender, EventArgs e)
        {
            this.FilePaths = ConfigurationHandlerMulti.GetSettingValues(Biff1ArchiveTest.configKey);
        }

        /// <summary>Event to raise for testing instance(s)</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            this.Archive = new Biff1Archive();
            this.Archive.Read(testArgs.Path);

            this.DoPostMessage(new MessageEventArgs(this.Archive.ToString()));
        }
    }
}