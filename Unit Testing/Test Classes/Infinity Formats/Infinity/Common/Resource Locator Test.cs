using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;

namespace Bardez.Projects.InfinityPlus1.UnitTesting.TestClasses.InfinityFormats.Infinity.Common
{
    /// <summary>This class tests the construction of a Resource Locator</summary>
    [TestClass]
    public class ResourceLocatorTest
    {
        #region TestContext
        private TestContext testContextInstance;

        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }
        #endregion

        /// <summary>Tests the Resource Locator construction from individual values</summary>
        [TestMethod]
        //[DataSource(Constants.XmlDataSource, @"\Code\Infinity Engine\Infinity +1\Unit Testing\Test Files\Infinity Formats\Infinity\Common\Resource Locator Test Data.xml", "ResourceLocatorConstruction", DataAccessMethod.Random)]
        [DataSource("System.Data.Odbc", @"Dsn=Excel Files;Driver={Microsoft Excel Driver (*.xlsx)};dbq=\Code\Infinity Engine\Infinity +1\Unit Testing\Test Files\Infinity Formats\Infinity\Common\Resource Locator Test Data.xlsx;defaultdir=.;driverid=790;maxbuffersize=2048;pagetimeout=5;readonly=true", "TestData$", DataAccessMethod.Random)]
        public void ResourceLocator1ConstructionTest()
        {
            UInt32? paramBiffIndex = Helper.GetContextUInt32("param_BiffIndex", this.TestContext);
            UInt32? paramTilesetIndex = Helper.GetContextUInt32("param_TilesetIndex", this.TestContext);
            UInt32? paramResourceIndex = Helper.GetContextUInt32("param_ResourceIndex", this.TestContext);
            Boolean? resultMatches = Helper.GetContextBoolean("result_matches", this.TestContext);

            if (paramBiffIndex == null || paramTilesetIndex == null || paramResourceIndex == null || resultMatches == null)
                throw new InvalidOperationException("Test data is invalid; there should be no null test data.");

            //construction
            ResourceLocator1 locator = new ResourceLocator1(paramBiffIndex.Value, paramTilesetIndex.Value, paramResourceIndex.Value);

            //test the equality of the values
            Boolean equality = 
                (
                    (paramBiffIndex.Value == locator.BiffIndex) &&
                    (paramTilesetIndex.Value == locator.TilesetIndex) &&
                    (paramResourceIndex.Value == locator.ResourceIndex)
                );

            Assert.AreEqual(resultMatches.Value, equality);
        }
    }
}