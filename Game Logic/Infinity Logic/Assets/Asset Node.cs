using System;
using System.Collections.Generic;

using Bardez.Projects.InfinityPlus1.Logic.Infinity;

namespace Bardez.Projects.InfinityPlus1.Logic.Infinity.Assets
{
    /// <summary>This class represents a node within an asset tree</summary>
    public class AssetNode
    {
        #region Fields
        /// <summary>Collection of nodes below this node in the asset tree</summary>
        protected List<AssetNode> children;

        /// <summary>Represents an asset in the tree</summary>
        protected AssetReference asset;

        /// <summary>Represents the displayed name of this node</summary>
        protected String name;
        #endregion


        #region Properties
        /// <summary>Collection of nodes below this node in the asset tree</summary>
        public IList<AssetNode> Children { get { return this.children; } }

        /// <summary>Represents this asset in the tree</summary>
        /// <value>Can be null in the case of a folder or the root node</value>
        public AssetReference Asset { get { return this.asset; } }

        /// <summary>Represents the displayed name of this node</summary>
        public String Name { get { return this.name; } }
        #endregion


        #region Construction
        /// <summary>Name definition constructor</summary>
        /// <param name="name">Name of the node</param>
        public AssetNode(String name)
        {
            this.children = new List<AssetNode>();
            this.name = name;
            this.asset = null;
        }

        /// <summary>Asset definition constructor</summary>
        /// <param name="asset">Asset related to this node</param>
        public AssetNode(AssetReference asset)
        {
            this.children = new List<AssetNode>();
            this.name = asset.AssetName;
            this.asset = asset;
        }

        /// <summary>Definition constructor</summary>
        /// <param name="name">Name of the node</param>
        /// <param name="asset">Asset related to this node</param>
        public AssetNode(String name, AssetReference asset)
        {
            this.children = new List<AssetNode>();
            this.name = name;
            this.asset = asset;
        }
        #endregion


        #region Overrides
        /// <summary>Generates a descriptive String for this node</summary>
        /// <returns>A Descriptive string of this node</returns>
        public override String ToString()
        {
            return this.name;
        }
        #endregion
    }
}