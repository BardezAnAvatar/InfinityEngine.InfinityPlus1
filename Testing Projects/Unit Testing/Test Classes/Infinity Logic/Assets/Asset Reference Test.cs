using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.KeyTable;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.Logic.Infinity.Assets;

namespace Bardez.Projects.InfinityPlus1.UnitTesting.TestClasses.InfinityLogic.Assets
{
    /// <summary>Test class for the AssetReference constructors and properties</summary>
    [TestClass]
    public class AssetReferenceTest
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


        [TestMethod]
        [DataSource(Constants.XmlDataSource, @"\Code\Infinity Engine\Infinity +1\Unit Testing\Test Files\Infinity Logic\Asset\Asset Reference.xml", "AssetReferenceFileSystem", DataAccessMethod.Random)]
        public void AssetReferenceFileSystemConstructorTest()
        {
            AssetLocation paramLocation = (AssetLocation)(Enum.Parse(typeof(AssetLocation), Helper.GetContextString("param_location", this.TestContext)));
            String paramPath = Helper.GetContextString("param_path", this.TestContext);
            String expectedAssetName = Helper.GetContextString("result_AssetName", this.TestContext);
            String expectedAssetType = Helper.GetContextString("result_AssetType", this.TestContext);
            Boolean expextedAssetTypeKnown = Boolean.Parse(Helper.GetContextString("result_AssetTypeKnown", this.TestContext));
            String expectedAssetPathDescription = Helper.GetContextString("result_AssetPathDescription", this.TestContext);
            AssetLocation expectedAssetLocatorLocation = (AssetLocation)(Enum.Parse(typeof(AssetLocation), Helper.GetContextString("result_AssetLocator_Location", this.TestContext)));
            String expectedAssetLocatorRelativePath = Helper.GetContextString("result_AssetLocator_RelativePath", this.TestContext);
            String expectedAssetLocatorKeyFileName = Helper.GetContextString("result_AssetLocator_KeyFileName", this.TestContext);
            UInt32? expectedAssetLocatorLocatorBiffIndex = Helper.GetContextUInt32("result_AssetLocator_ResourceLocator_BiffIndex", this.TestContext);
            UInt32? expectedAssetLocatorLocatorTilesetIndex = Helper.GetContextUInt32("result_AssetLocator_ResourceLocator_TilesetIndex", this.TestContext);
            UInt32? expectedAssetLocatorLocatorResourceIndex = Helper.GetContextUInt32("result_AssetLocator_ResourceLocator_ResourceIndex", this.TestContext);
            ResourceType expectedAssetLocatorResourceType = (ResourceType)(Enum.Parse(typeof(ResourceType), Helper.GetContextString("result_AssetLocator_ResourceType", this.TestContext)));

            if (expectedAssetLocatorLocatorBiffIndex != null)
                throw new InvalidOperationException("Test data is invalid; the constructor should result in a null locator.");
            else if (expectedAssetLocatorLocatorTilesetIndex != null)
                throw new InvalidOperationException("Test data is invalid; the constructor should result in a null locator.");
            else if (expectedAssetLocatorLocatorResourceIndex != null)
                throw new InvalidOperationException("Test data is invalid; the constructor should result in a null locator.");
            
            //instantiate
            AssetReference reference = new AssetReference(paramLocation, paramPath);

            //check
            Assert.AreEqual(expectedAssetName, reference.AssetName);
            Assert.AreEqual(expectedAssetType, reference.AssetType);
            Assert.AreEqual(expextedAssetTypeKnown, reference.AssetTypeKnown);
            Assert.AreEqual(expectedAssetPathDescription, reference.AssetPathDescription);
            Assert.IsNotNull(reference.Locator);
            Assert.AreEqual(expectedAssetLocatorKeyFileName, reference.Locator.KeyFileName);
            Assert.AreEqual(expectedAssetLocatorLocation, reference.Locator.Location);
            Assert.AreEqual(expectedAssetLocatorRelativePath, reference.Locator.RelativePath);
            Assert.IsNull(reference.Locator.ResourceLocator);
            Assert.AreEqual(expectedAssetLocatorResourceType, reference.Locator.ResourceType);
        }

        [TestMethod]
        [DataSource(Constants.XmlDataSource, @"\Code\Infinity Engine\Infinity +1\Unit Testing\Test Files\Infinity Logic\Asset\Asset Reference.xml", "AssetReferenceKeyFile", DataAccessMethod.Random)]
        public void AssetReferenceKeyFileConstructorTest()
        {
            String paramKeyName = Helper.GetContextString("param_KeyFileName", this.TestContext);
            String paramKeyFileResourceName = Helper.GetContextString("param_KeyFileResourceEntry_Name", this.TestContext);
            ResourceType paramKeyFileResourceType = (ResourceType)(Enum.Parse(typeof(ResourceType), Helper.GetContextString("param_KeyFileResourceEntry_ResourceType", this.TestContext)));
            UInt32? paramKeyFileResourceBiffIndex = Helper.GetContextUInt32("param_KeyFileResourceEntry_ResourceLocator_BiffIndex", this.TestContext);
            UInt32? paramKeyFileResourceResourceIndex = Helper.GetContextUInt32("param_KeyFileResourceEntry_ResourceLocator_ResourceIndex", this.TestContext);
            UInt32? paramKeyFileResourceTilesetIndex = Helper.GetContextUInt32("param_KeyFileResourceEntry_ResourceLocator_TilesetIndex", this.TestContext);
            String paramKeyFileBiffName = Helper.GetContextString("param_KeyFileBiffEntry_Name", this.TestContext);
            UInt32 bifSize = Convert.ToUInt32(new Random(12345).Next());
            UInt32 offsetName = Convert.ToUInt32(new Random(12345).Next(Convert.ToInt32(bifSize), Int32.MaxValue));
            KeyTableBifLocationEnum locations = (KeyTableBifLocationEnum)(Enum.Parse(typeof(KeyTableBifLocationEnum), Helper.GetContextString("param_KeyFileBiffEntry_LocationFlags", this.TestContext)));

            String expectedAssetName = Helper.GetContextString("result_AssetName", this.TestContext);
            String expectedAssetType = Helper.GetContextString("result_AssetType", this.TestContext);
            Boolean expextedAssetTypeKnown = Boolean.Parse(Helper.GetContextString("result_AssetTypeKnown", this.TestContext));
            String expectedAssetPathDescription = Helper.GetContextString("result_AssetPathDescription", this.TestContext);
            AssetLocation expectedAssetLocatorLocation = (AssetLocation)(Enum.Parse(typeof(AssetLocation), Helper.GetContextString("result_AssetLocator_Location", this.TestContext)));
            String expectedAssetLocatorRelativePath = Helper.GetContextString("result_AssetLocator_RelativePath", this.TestContext);
            String expectedAssetLocatorKeyFileName = Helper.GetContextString("result_AssetLocator_KeyFileName", this.TestContext);
            UInt32? expectedAssetLocatorLocatorBiffIndex = Helper.GetContextUInt32("result_AssetLocator_ResourceLocator_BiffIndex", this.TestContext);
            UInt32? expectedAssetLocatorLocatorTilesetIndex = Helper.GetContextUInt32("result_AssetLocator_ResourceLocator_TilesetIndex", this.TestContext);
            UInt32? expectedAssetLocatorLocatorResourceIndex = Helper.GetContextUInt32("result_AssetLocator_ResourceLocator_ResourceIndex", this.TestContext);
            ResourceType expectedAssetLocatorResourceType = (ResourceType)(Enum.Parse(typeof(ResourceType), Helper.GetContextString("result_AssetLocator_ResourceType", this.TestContext)));

            if (expectedAssetLocatorRelativePath != null)
                throw new InvalidOperationException("Test data is invalid; the constructor should result in a null relative path.");
            

            //instantiate
            KeyTableBifEntry biff = new KeyTableBifEntry(bifSize, offsetName, paramKeyFileBiffName, locations);
            ResourceLocator1 locator = new ResourceLocator1(paramKeyFileResourceBiffIndex.Value, paramKeyFileResourceTilesetIndex.Value, paramKeyFileResourceResourceIndex.Value);
            KeyTableResourceEntry resource = new KeyTableResourceEntry(paramKeyFileResourceName, paramKeyFileResourceType, locator);
            AssetReference reference = new AssetReference(paramKeyName, resource, biff);

            //check
            Assert.AreEqual(expectedAssetName, reference.AssetName);
            Assert.AreEqual(expectedAssetType, reference.AssetType);
            Assert.AreEqual(expextedAssetTypeKnown, reference.AssetTypeKnown);
            Assert.AreEqual(expectedAssetPathDescription, reference.AssetPathDescription);
            Assert.IsNotNull(reference.Locator);
            Assert.AreEqual(expectedAssetLocatorKeyFileName, reference.Locator.KeyFileName);
            Assert.AreEqual(expectedAssetLocatorLocation, reference.Locator.Location);
            Assert.AreEqual(expectedAssetLocatorRelativePath, reference.Locator.RelativePath);
            Assert.AreEqual(expectedAssetLocatorResourceType, reference.Locator.ResourceType);
            Assert.IsNotNull(reference.Locator.ResourceLocator);
            Assert.AreEqual(expectedAssetLocatorLocatorBiffIndex, reference.Locator.ResourceLocator.BiffIndex);
            Assert.AreEqual(expectedAssetLocatorLocatorResourceIndex, reference.Locator.ResourceLocator.ResourceIndex);
            Assert.AreEqual(expectedAssetLocatorLocatorTilesetIndex, reference.Locator.ResourceLocator.TilesetIndex);
        }
    }
}