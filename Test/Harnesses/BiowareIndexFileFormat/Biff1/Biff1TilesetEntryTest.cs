using System;
using System.IO;
using System.Text;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.BiowareIndexFileFormat.Biff1;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.BiowareIndexFileFormat.Biff1
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.BiowareIndexFileFormat.Biff1.Biff1TilesetEntry class.</summary>
    public class Biff1TilesetEntryTest : FileTesterBase
    {
        #region Fields
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.Biff1.BiffPath";

        /// <summary>Constant key to look up in app.config</summary>
        protected const String offsetConfigKey = "Test.Biff1.TilesetEntry.Offset";
        
        /// <summary>Class instance to test</summary>
        protected Biff1TilesetEntry TisEntry { get; set; }
        #endregion
        
        #region Construction
        /// <summary>Default constructor</summary>
        public Biff1TilesetEntryTest()
        {
            this.InitializeInstance();
        }
        #endregion

        /// <summary>Initializes the test class data</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="e">Specific initialization event parameters</param>
        protected override void InitializeTestData(Object sender, EventArgs e)
        {
            this.FilePaths = ConfigurationHandlerMulti.GetSettingValues(Biff1TilesetEntryTest.configKey);
        }

        /// <summary>Event to raise for testing instance(s)</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            using (FileStream stream = new FileStream(testArgs.Path, FileMode.Open, FileAccess.Read))
            {
                this.TisEntry = new Biff1TilesetEntry();
                ReusableIO.SeekIfAble(stream, (Int32.Parse(ConfigurationHandler.GetSettingValue(Biff1TilesetEntryTest.offsetConfigKey))));
                this.TisEntry.Read(stream);

                this.DoPostMessage(new MessageEventArgs(this.TisEntry.ToString(), "Output", testArgs.Path));
            }
        }
    }
}