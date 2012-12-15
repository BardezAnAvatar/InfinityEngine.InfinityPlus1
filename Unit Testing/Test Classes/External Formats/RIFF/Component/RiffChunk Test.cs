using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component;

namespace Bardez.Projects.InfinityPlus1.UnitTesting
{
    /// <summary>
    ///     This is a test class for RiffChunkTest and is intended
    ///     to contain all RiffChunkTest Unit Tests
    /// </summary>
    [TestClass]
    public class RiffChunkTest
    {
        #region TestContext
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
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


        /// <summary>A test for ReadChunkHeader</summary>
        [TestMethod]
        public void RiffChunkReadChunkHeaderTest()
        {
            String filename = String.Empty;

            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                RiffChunk target = null;
                target.ReadChunkHeader();
            }
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>A test for ReadFourCC</summary>
        /// <remarks>Passes if a valid </remarks>
        [TestMethod]
        public void RiffChunkReadFourCCTest()
        {
            String filename = null;
            filename = @"\Multi-Media\Audio\Asho.wav";
            //filename = @"\Multi-Media\Audio\Bored.wav";
            //filename = @"\Multi-Media\Audio\Gotcha Bitch.wav";
            //filename = @"\Multi-Media\Audio\start [what is thy bidding].wav";
            //filename = @"\Multi-Media\Audio\TORMS_DISS-04-THA_LONER.wav";
            //filename = @"\Multi-Media\Audio\vaqueros02[1].wav";

            filename = @"\Multi-Media\Audio\MacGyver Theme.ogg";

            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                ChunkType type = RiffChunk.ReadFourCC(fs);
            }
        }
    }
}
