using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component;

namespace Bardez.Projects.InfinityPlus1.UnitTesting.ExternalFormats.RIFF.Component
{
    /// <summary>
    ///     This is a test class for RiffFileTest and is intended
    ///     to contain all RiffFileTest Unit Tests
    /// </summary>
    [TestClass]
    public class RiffFileTest
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
        //[ClassInitialize]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>A test for Read</summary>
        [TestMethod]
        [DataSource(Constants.XmlDataSource, @"\Code\Infinity Engine\Infinity +1\Unit Testing\Test Files\External Formats\RIFF\Component\Riff File FourCC Test.xml", "RIFF", DataAccessMethod.Random)]
        public void ReadTest()
        {
            String filename = TestContext.DataRow["FilePath"] as String;
            String expectedType = TestContext.DataRow["RiffFourCCType"] as String;
            String actual = null;
            
            //HACK: generate a RIFF Report
            StringBuilder chunkReport = new StringBuilder();

            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                RiffFile target = new RiffFile();
                target.Read(fs);
                actual = target.ContainerTypeName;

                target.WriteString(chunkReport);
            }

            //writer takes ownership of report, so do not dispose.
            FileStream report = new FileStream(String.Format("{0}.RIFF Report.txt", filename), FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(report))
            {
                writer.Write(chunkReport);
                writer.Flush();
            }

            Assert.AreEqual(expectedType, actual);
        }
    }
}