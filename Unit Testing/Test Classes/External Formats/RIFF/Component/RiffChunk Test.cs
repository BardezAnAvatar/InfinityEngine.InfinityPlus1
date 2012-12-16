using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component;

namespace Bardez.Projects.InfinityPlus1.UnitTesting.ExternalFormats.RIFF.Component
{
    /// <summary>
    ///     This is a test class for RiffChunkTest and is intended
    ///     to contain all RiffChunkTest Unit Tests
    /// </summary>
    [TestClass]
    public class RiffChunkTest
    {
        #region Constants
        /// <summary>Path to the test file for these Unit Tests</summary>
        private const String TestFile = "";

        /// <summary>Name of the FourCC test's virtual table name</summary>
        private const String FourCC_Table = "";
        #endregion


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
        [DataSource(Constants.XmlDataSource, @"\Code\Infinity Engine\Infinity +1\Unit Testing\Test Files\External Formats\RIFF\Component\Riff Chunk FourCC Test.xml", "FourCC", DataAccessMethod.Random)]
        public void RiffChunkReadFourCCTest()
        {
            String filename = TestContext.DataRow["FilePath"] as String;
            String expectedType = TestContext.DataRow["ExpectedFourCC"] as String;
            String actual = null;

            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    ChunkType type = RiffChunk.ReadFourCC(fs);
                    actual = type.ToString();
                }
                catch
                {
                    actual = "Exception";
                }
            }

            Assert.AreEqual(expectedType, actual);
        }
    }
}
