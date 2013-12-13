using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Wave;

namespace Bardez.Projects.InfinityPlus1.UnitTesting.ExternalFormats.RIFF.Wave
{
    /// <summary>
    ///     This is a test class for WaveRiffFileTest and is intended
    ///     to contain all WaveRiffFileTest Unit Tests
    /// </summary>
    [TestClass]
    public class WaveRiffFileTest
    {
        #region TestContext
        private TestContext testContextInstance;

        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        #endregion


        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>A test for IsWaveFile</summary>
        [TestMethod]
        [DeploymentItem("External Formats.dll")]
        public void WaveRiffFileIsWaveFileTest()
        {
            WaveRiffFile wave = null;
            using (FileStream input = new FileStream(@"N:\Multi-Media\Audio\Asho.wav", FileMode.Open, FileAccess.Read))
            {
                wave.Read(input);
            }

            //WaveRiffFile_Accessor target = new WaveRiffFile_Accessor(); // TODO: Initialize to an appropriate value
            //bool expected = false; // TODO: Initialize to an appropriate value
            //bool actual = target.IsWaveFile();
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
