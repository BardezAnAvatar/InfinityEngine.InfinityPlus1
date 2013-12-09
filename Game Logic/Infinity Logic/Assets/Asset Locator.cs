using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.KeyTable;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;

namespace Bardez.Projects.InfinityPlus1.Logic.Infinity.Assets
{
    /// <summary>This class represents the information necessary to locate a specific asset</summary>
    public class AssetLocator
    {
        #region Fields
        /// <summary>The location relative to the executable for which to locate the asset</summary>
        protected readonly AssetLocation location;

        /// <summary>The relative path to the asset if it is located in the file system</summary>
        /// <value>null or a populated relative path to the location flags specified</value>
        protected readonly String relativePath;

        /// <summary>Name of the key file which holds the resource locator</summary>
        /// <value>null if the value is from the file system, otherwise "chitin.key" or "xp1.key", etc.</value>
        protected readonly String keyFileName;

        /// <summary>Resource locator if it is not a file</summary>
        protected readonly ResourceLocator1 resourceLocator;

        /// <summary>The type of resource</summary>
        /// <remarks>Needed to determine if the resource is a tileset or not</remarks>
        protected readonly ResourceType resourceType;
        #endregion


        #region Properties
        /// <summary>The location relative to the executable for which to locate the asset</summary>
        public AssetLocation Location { get { return this.location; } }

        /// <summary>The relative path to the asset if it is located in the file system</summary>
        /// <value>null or a populated relative path to the location flags specified</value>
        public String RelativePath { get { return this.relativePath; } }

        /// <summary>Name of the key file which holds the resource locator</summary>
        /// <value>null if the value is from the file system, otherwise "chitin.key" or "xp1.key", etc.</value>
        public String KeyFileName { get { return this.keyFileName; } }

        /// <summary>Resource locator if it is not a file</summary>
        public ResourceLocator1 ResourceLocator { get { return this.resourceLocator; } }

        /// <summary>The type of resource</summary>
        /// <remarks>Needed to determine if the resource is a tileset or not</remarks>
        public ResourceType ResourceType { get { return this.resourceType; } }
        #endregion


        #region Construction
        /// <summary>File system definition constructor</summary>
        /// <param name="location">The location relative to the executable for which to locate the asset</param>
        /// <param name="path">The relative path to the asset located in the file system</param>
        /// <param name="type">Type of the resource</param>
        public AssetLocator(AssetLocation location, String path, ResourceType type)
        {
            this.keyFileName = null;
            this.resourceLocator = null;
            this.location = location;
            this.relativePath = path;
            this.resourceType = type;
        }

        /// <summary>Key file definition constructor</summary>
        /// <param name="location">The containing key name of the asset</param>
        /// <param name="resource">The chitin.key resource entry</param>
        public AssetLocator(String keyFile, ChitinKeyResourceEntry resource)
        {
            this.keyFileName = keyFile;
            this.resourceLocator = resource.ResourceLocator;
            this.relativePath = null;
            this.location = AssetLocation.LookUp;
            this.resourceType = resource.ResourceType;
        }
        #endregion
    }
}