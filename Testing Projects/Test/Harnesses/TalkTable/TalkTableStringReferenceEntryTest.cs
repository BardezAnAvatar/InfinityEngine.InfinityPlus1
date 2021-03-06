using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TalkTable;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.TextLocationKey
{
    /// <summary>This class tests the usable methods in the InfinityPlus1.Files.TextLocationKeyStringReference class</summary>
    public class TalkTableStringReferenceEntryTest : FileTesterBase
    {
        #region Fields
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.Tlk1.Tlk1Path";

        /// <summary>Format instance to test</summary>
        protected TalkTableStringReference Entry { get; set; }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public TalkTableStringReferenceEntryTest()
        {
            this.InitializeInstance();
        }
        #endregion

        /// <summary>Initializes the test class data</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="e">Specific initialization event parameters</param>
        protected override void InitializeTestData(Object sender, EventArgs e)
        {
            this.FilePaths = ConfigurationHandlerMulti.GetSettingValues(TalkTableStringReferenceEntryTest.configKey);
        }

        /// <summary>Event to raise for testing instance(s)</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            using (FileStream stream = new FileStream(testArgs.Path, FileMode.Open, FileAccess.Read))
            {
                TalkTableHeader header = new TalkTableHeader();
                header.Read(stream);

                this.Entry = new TalkTableStringReference(CultureConstants.CultureCodeEnglish);
                this.Entry.Read(stream);

                this.Entry.ReadStringReferenceFull(stream, header.StringsReferenceOffset);
            }

            this.DoPostMessage(new MessageEventArgs(this.Entry.ToString(), "Output", testArgs.Path));

            using (FileStream dest = new FileStream(testArgs.Path + ".strref.rewrite", FileMode.Create, FileAccess.Write))
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