using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.Logic.Infinity.Assets
{
    /// <summary>This class represents a single save folder and its resource contents</summary>
    public class SaveFolder
    {
        #region Fields
        /// <summary>Path to the save folder</summary>
        protected String saveFolderDirectory;

        /// <summary>Collection of assets within this save folder</summary>
        protected List<AssetReference> resources;
        #endregion


        #region Properties
        /// <summary>Path to the save folder</summary>
        public String SaveFolderPath { get { return this.saveFolderDirectory; } }

        /// <summary>Exposes the save folder's name</summary>
        public String SaveFolderName { get { return System.IO.Path.GetFileName(this.saveFolderDirectory); } }   //exposes the actual save folder's name

        /// <summary>Collection of assets within this save folder</summary>
        public IList<AssetReference> Resources { get { return this.resources; } }
        #endregion


        #region Construction
        /// <summary>Partial definition constructor</summary>
        /// <param name="directory">Directory location of this save folder</param>
        public SaveFolder(String directory)
        {
            this.saveFolderDirectory = directory;
            this.resources = new List<AssetReference>();
        }
        #endregion
    }
}