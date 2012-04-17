using System;
using System.IO;

using Bardez.Projects.ReusableCode;
using Ionic.Zlib;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Save
{
    /// <summary>A managing class for *.SAV files, to allow adding resources, and extracting resources</summary>
    public class SaveManager
    {
        #region Fields
        /// <summary>Represents the contents of a saved file</summary>
        public SaveFile SavedResources { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public SaveManager() { }

        /// <summary>Definitionconstructor</summary>
        /// <param name="resouces">Existing SaveFile reference</param>
        public SaveManager(SaveFile resouces)
        {
            this.SavedResources = resouces;
        }
        #endregion


        #region Methods
        /// <summary>Adds a resource to the collection of persisted files.</summary>
        /// <param name="resourceName">Name of the resource being added</param>
        /// <param name="data">Stream containing the data to be stored & compressed. Must be set to the start read position</param>
        /// <param name="length">Length of the resource to be added</param>
        public void AddResource(String resourceName, Stream data, Int32 length)
        {
            Byte[] binData = ReusableIO.BinaryRead(data, length);

            //check to see if the resource currently exists
            SavedResource resource = this.FindResource(resourceName);
            Boolean found = (resource != null);

            if (!found)
            {
                resource = new SavedResource();
                resource.Initialize();
                resource.ResourceName = resourceName;
            }

            resource.DecompressedDataSize = length;

            //compress the data
            resource.CompressedData = ZlibStream.CompressBuffer(binData);

            if (!found)
                this.SavedResources.Resources.Add(resource);
        }

        /// <summary>Extracts a resouce from the compressed resources collection</summary>
        /// <param name="resourceName">Name of the resource being extracted</param>
        /// <returns>A MemoryStream containing the decompressed resource data</returns>
        public MemoryStream ExtractResource(String resourceName)
        {
            SavedResource resource = this.FindResource(resourceName);

            if (resource == null)
                throw new ApplicationException("Could not find the requested resource.");

            MemoryStream memStream = new MemoryStream(ZlibStream.UncompressBuffer(resource.CompressedData));
            return memStream;
        }

        /// <summary>Determines whether a resource can be found in the collection</summary>
        /// <param name="resourceName">Name of the resource to locate</param>
        /// <returns>Boolean indicating whether the resource has been found</returns>
        public Boolean ResourceExists(String resourceName)
        {
            SavedResource resource = this.FindResource(resourceName);
            return resource != null;
        }

        /// <summary>Finds a saved resource entry in the saved resources collection</summary>
        /// <param name="name">Name of the resource to locate</param>
        /// <returns>The refernce to the saved resource, otherwise null</returns>
        protected SavedResource FindResource(String name)
        {
            SavedResource resource = null;

            foreach (SavedResource res in this.SavedResources.Resources)
                if (res.ResourceName == name)
                {
                    resource = res;
                    break;
                }

            return resource;
        }
        #endregion
    }
}