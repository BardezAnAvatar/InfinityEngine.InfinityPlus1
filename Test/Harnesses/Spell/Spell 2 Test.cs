using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell.Spell2;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.Spell
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell.Spell2.Spell2 class.</summary>
    public class Spell2Test : FileTesterBase
    {
        #region Fields
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.Spell.Spell2Path";

        /// <summary>Format instance to test</summary>
        protected Spell2 Spell { get; set; }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public Spell2Test()
        {
            this.InitializeInstance();
        }
        #endregion

        /// <summary>Initializes the test class data</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="e">Specific initialization event parameters</param>
        protected override void InitializeTestData(Object sender, EventArgs e)
        {
            this.FilePaths = ConfigurationHandlerMulti.GetSettingValues(Spell2Test.configKey);
        }

        /// <summary>Event to raise for testing instance(s)</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            using (FileStream stream = new FileStream(testArgs.Path, FileMode.Open, FileAccess.Read))
            {
                this.Spell = new Spell2();
                this.Spell.Read(stream);
            }

            this.DoPostMessage(new MessageEventArgs(this.Spell.ToString(), "Output", testArgs.Path));

            using (FileStream dest = new FileStream(testArgs.Path + ".rewrite", FileMode.Create, FileAccess.Write))
                this.TestWrite(dest);
        }

        /// <summary>Writes the data structure back out to a destination stream</summary>
        /// <param name="destination">Stream to write output to</param>
        protected virtual void TestWrite(Stream destination)
        {
            this.Spell.Write(destination);
        }
    }
}