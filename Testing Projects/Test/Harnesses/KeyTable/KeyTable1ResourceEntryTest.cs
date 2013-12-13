using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.KeyTable;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.ChitinKey
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.FileFormats.Infinity.ChitinKey.ChitinKeyResourceEntry class.</summary>
    public class ChitinKey1ResourceEntryTest : FileTesterBase
    {
        #region Fields
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.Key.Key1Path";

        /// <summary>Format instance to test</summary>
        protected KeyTableResourceEntry Entry { get; set; }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public ChitinKey1ResourceEntryTest()
        {
            this.InitializeInstance();
        }
        #endregion

        /// <summary>Initializes the test class data</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="e">Specific initialization event parameters</param>
        protected override void InitializeTestData(Object sender, EventArgs e)
        {
            this.FilePaths = ConfigurationHandlerMulti.GetSettingValues(ChitinKey1ResourceEntryTest.configKey);
        }

        /// <summary>Event to raise for testing instance(s)</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            using (FileStream stream = new FileStream(testArgs.Path, FileMode.Open, FileAccess.Read))
            {
                KeyTableHeader header = new KeyTableHeader();
                header.Read(stream);

                this.Entry = new KeyTableResourceEntry();

                //used to be just an abstract offset, for a specifickey file. Just read the first resource entry
                ReusableIO.SeekIfAble(stream, header.OffsetResource);
                this.Entry.Read(stream);

                this.DoPostMessage(new MessageEventArgs(this.Entry.ToString(), "Output", testArgs.Path));
            }

            using (FileStream dest = new FileStream(testArgs.Path + ".rewrite", FileMode.Create, FileAccess.Write))
                this.TestWrite(dest);
        }

        /// <summary>Writes the data structure back out to a destination stream</summary>
        /// <param name="destination">Stream to write output to</param>
        protected virtual void TestWrite(Stream destination)
        {
            this.Entry.Write(destination);
        }
    }
}