using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.KeyTable;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.Logic.Infinity.Assets;

namespace Bardez.Projects.InfinityPlus1.UnitTesting.TestClasses.InfinityLogic.Assets
{
    /// <summary>Test class for the AssetLocator constructors and properties</summary>
    [TestClass]
    public class AssetLocatorTest
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

        /// <summary>Tests the constructor from a file system</summary>
        [TestMethod]
        [DataSource(Constants.XmlDataSource, @"\Code\Infinity Engine\Infinity +1\Unit Testing\Test Files\Infinity Logic\Asset\Asset Locator.xml", "AssetLocatorFileSystem", DataAccessMethod.Random)]
        public void AssetLocatorConstructFromFileSystem()
        {
            String parameterPath = TestContext.DataRow["param_path"] as String;
            if (parameterPath == String.Empty)
                parameterPath = null;

            String expectedPath = TestContext.DataRow["result_path"] as String;
            if (expectedPath == String.Empty)
                expectedPath = null;

            String expectedKey = TestContext.DataRow["result_keyFile"] as String;
            if (expectedKey == String.Empty)
                expectedKey = null;

            AssetLocation parameterLocation = (AssetLocation)(Enum.Parse(typeof(AssetLocation), TestContext.DataRow["param_location"] as String));
            AssetLocation expectedLocation = (AssetLocation)(Enum.Parse(typeof(AssetLocation), TestContext.DataRow["result_location"] as String));
            ResourceType parameterType = (ResourceType)(Enum.Parse(typeof(ResourceType), TestContext.DataRow["param_type"] as String));
            ResourceType expectedType = (ResourceType)(Enum.Parse(typeof(ResourceType), TestContext.DataRow["result_type"] as String));
            ResourceLocator1 expectedLocator = null;
            if (!String.IsNullOrEmpty(TestContext.DataRow["result_locator"] as String))
                expectedLocator = new ResourceLocator1(UInt32.Parse(TestContext.DataRow["result_locator"] as String));

            if (expectedLocator != null)
                throw new InvalidOperationException("Test data is invalid; the constructor should result in a null locator.");
            else if (expectedKey != null)
                throw new InvalidOperationException("Test data is invalid; the constructor should result in a null key file.");

            //build
            AssetLocator asset = new AssetLocator(parameterLocation, parameterPath, parameterType);

            //compare
            Assert.AreEqual(asset.KeyFileName, expectedKey);
            Assert.AreEqual(asset.Location, expectedLocation);
            Assert.AreEqual(asset.RelativePath, expectedPath);
            Assert.AreEqual(asset.ResourceLocator, expectedLocator);
            Assert.AreEqual(asset.ResourceType, expectedType);
        }

        /// <summary>Tests the constructor from a file system</summary>
        [TestMethod]
        [DataSource(Constants.XmlDataSource, @"\Code\Infinity Engine\Infinity +1\Unit Testing\Test Files\Infinity Logic\Asset\Asset Locator.xml", "AssetLocatorKeyFile", DataAccessMethod.Random)]
        public void AssetLocatorConstructFromKeyFile()
        {
            String parameterKeyFile = TestContext.DataRow["param_keyFile"] as String;
            if (parameterKeyFile == String.Empty)
                parameterKeyFile = null;

            String parameterKeyResourceName = TestContext.DataRow["param_keyresource_name"] as String;
            if (parameterKeyResourceName == String.Empty)
                parameterKeyResourceName = null;

            ResourceType parameterKeyResourceType = (ResourceType)(Enum.Parse(typeof(ResourceType), TestContext.DataRow["param_keyresource_type"] as String));
            UInt32 parameterKeyResourceLocatorBiff = UInt32.Parse(TestContext.DataRow["param_keyresource_locator_biff"] as String);
            UInt32 parameterKeyResourceLocatorResource = UInt32.Parse(TestContext.DataRow["param_keyresource_locator_resource"] as String);
            UInt32 parameterKeyResourceLocatorTileset = UInt32.Parse(TestContext.DataRow["param_keyresource_locator_tileset"] as String);
            
            AssetLocation expectedLocation = (AssetLocation)(Enum.Parse(typeof(AssetLocation), TestContext.DataRow["result_location"] as String));
            
            String expectedPath = TestContext.DataRow["result_path"] as String;
            if (expectedPath == String.Empty)
                expectedPath = null;

            String expectedkeyFile = TestContext.DataRow["result_keyFile"] as String;
            if (expectedkeyFile == String.Empty)
                expectedkeyFile = null;

            UInt32 expectedKeyResourceLocatorBiff = UInt32.Parse(TestContext.DataRow["result_keyresource_locator_biff"] as String);
            UInt32 expectedKeyResourceLocatorResource = UInt32.Parse(TestContext.DataRow["result_keyresource_locator_resource"] as String);
            UInt32 expectedKeyResourceLocatorTileset = UInt32.Parse(TestContext.DataRow["result_keyresource_locator_tileset"] as String);
            ResourceType expectedKeyResourceType = (ResourceType)(Enum.Parse(typeof(ResourceType), TestContext.DataRow["result_type"] as String));


            if (expectedPath != null)
                throw new InvalidOperationException("Test data is invalid; the constructor should result in a null path.");

            //build
            ResourceLocator1 locator = new ResourceLocator1(parameterKeyResourceLocatorBiff, parameterKeyResourceLocatorTileset, parameterKeyResourceLocatorResource);
            ChitinKeyResourceEntry resource = new ChitinKeyResourceEntry(parameterKeyResourceName, parameterKeyResourceType, locator);
            AssetLocator asset = new AssetLocator(parameterKeyFile, resource);

            //compare
            Assert.AreEqual(asset.KeyFileName, expectedkeyFile);
            Assert.AreEqual(asset.Location, expectedLocation);
            Assert.AreEqual(asset.RelativePath, expectedPath);
            Assert.AreEqual(asset.ResourceLocator.BiffIndex, expectedKeyResourceLocatorBiff);
            Assert.AreEqual(asset.ResourceLocator.ResourceIndex, expectedKeyResourceLocatorResource);
            Assert.AreEqual(asset.ResourceLocator.TilesetIndex, expectedKeyResourceLocatorTileset);
            Assert.AreEqual(asset.ResourceType, expectedKeyResourceType);
        }
    }
}