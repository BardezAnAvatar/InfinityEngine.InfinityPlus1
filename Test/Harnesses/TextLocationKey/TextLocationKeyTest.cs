using System;
using System.IO;
using System.Text;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TalkTable;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.TextLocationKey
{
    /// <summary>This class tests the usable methods in the InfinityPlus1.Files.TextLocationKey class</summary>
    public class TextLocationKeyTest : FileTesterBase
    {
        #region Fields
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.Tlk1.Tlk1Path";

        /// <summary>Format instance to test</summary>
        protected FileFormats.Infinity.TalkTable.TalkTable TlkFile { get; set; }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public TextLocationKeyTest()
        {
            this.InitializeInstance();
        }
        #endregion

        /// <summary>Initializes the test class data</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="e">Specific initialization event parameters</param>
        protected override void InitializeTestData(Object sender, EventArgs e)
        {
            this.FilePaths = ConfigurationHandlerMulti.GetSettingValues(TextLocationKeyTest.configKey);
        }

        /// <summary>Event to raise for testing instance(s)</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            StringBuilder buffer = new StringBuilder();

            using (FileStream stream = new FileStream(testArgs.Path, FileMode.Open, FileAccess.Read))
            {
                this.TlkFile = new FileFormats.Infinity.TalkTable.TalkTable(false);
                this.TlkFile.Read(stream);

                buffer.AppendLine(TlkFile.ToString(false));
                buffer.Append("String Reference #25641:");
                buffer.AppendLine(TlkFile[25641].ToString());

                buffer.AppendLine("Testing full read:");
                this.TlkFile = new FileFormats.Infinity.TalkTable.TalkTable();
                this.TlkFile.Read(stream);

                buffer.Append("String Reference #12345:");
                buffer.AppendLine(TlkFile[12345].ToString());
            }

            this.DoPostMessage(new MessageEventArgs(buffer.ToString(), "Output", testArgs.Path));

            using (FileStream dest = new FileStream(testArgs.Path + ".rewrite", FileMode.Create, FileAccess.Write))
                this.TestWrite(dest);
        }

        /// <summary>Writes the data structure back out to a destination stream</summary>
        /// <param name="destination">Stream to write output to</param>
        protected virtual void TestWrite(Stream destination)
        {
            this.TlkFile.Write(destination);
        }
    }
}