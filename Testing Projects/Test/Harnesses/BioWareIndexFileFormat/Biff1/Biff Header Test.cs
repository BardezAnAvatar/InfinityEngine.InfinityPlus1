using System;
using System.IO;
using System.Text;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareIndexFileFormat.Components;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.BioWareIndexFileFormat.Biff1
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BiowareIndexFileFormat.Biff1.Biff1Header class.</summary>
    public class Biff1HeaderTest : FileTesterBase
    {
        #region Fields
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.Biff1.BiffPath";

        /// <summary>Class instance to test</summary>
        protected Biff1Header Header { get; set; }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public Biff1HeaderTest()
        {
            this.InitializeInstance();
        }
        #endregion

        /// <summary>Initializes the test class data</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="e">Specific initialization event parameters</param>
        protected override void InitializeTestData(Object sender, EventArgs e)
        {
            this.FilePaths = ConfigurationHandlerMulti.GetSettingValues(Biff1HeaderTest.configKey);
        }

        /// <summary>Event to raise for testing instance(s)</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            using (FileStream stream = new FileStream(testArgs.Path, FileMode.Open, FileAccess.Read))
            {
                this.Header = new Biff1Header();
                this.Header.Read(stream);

                this.DoPostMessage(new MessageEventArgs(this.Header.ToString(), "Output", testArgs.Path));
            }
        }
    }
}