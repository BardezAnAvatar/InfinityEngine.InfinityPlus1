using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.KeyTable;
using Bardez.Projects.InfinityPlus1.Information;
using Bardez.Projects.InfinityPlus1.Logic.Infinity.Assets;
using Bardez.Projects.InfinityPlus1.Logic.Infinity.Factories;

namespace Bardez.Projects.InfinityPlus1.Tools.Controllers.ApproachingInfinity
{
    /// <summary>This class is the controller for the Approaching Infinity application</summary>
    public class ApproachingInfinityController
    {
        #region Fields
        /// <summary>Asset collection and exposure manager</summary>
        protected IAssetManager assetManager;
        #endregion


        #region Properties
        #endregion


        #region Local UI Processing Events
        /// <summary>Local event that triggers a refresh of the resource tree</summary>
        protected event Action refreshResourceTree;

        /// <summary>Local event that triggers the clearing of the displayed file</summary>
        protected event Action clearFileControl;

        /// <summary>Local event that triggers the clearing of all displayed items</summary>
        /// <remarks>This means the tree, displayed items such as directories of count of files found</remarks>
        protected event Action clearAllDisplay;

        /// <summary>Local event that informs subscribers of the assigned application Path</summary>
        protected event Action<String> assignedApplicationPath;

        /// <summary>Local event that informs subscribers of the assigned user Path</summary>
        protected event Action<String> assignedUserPath;

        /// <summary>Local event that informs subscribers of the assigned detected game</summary>
        protected event Action<GameInstall> assignedDetectedGame;

        /// <summary>Local event that informs subscribers of the assigned detected game engine</summary>
        protected event Action<GameEngine> assignedDetectedEngine;
        #endregion


        //#region Local Asset Processing Events
        ///// <summary>Local event that informs subscribers to open the provided (IE versioned) key table as they see fit</summary>
        //protected event Action<KeyTable> assetOpenKeyTableInfinity;

        //#endregion


        #region Exposed UI Processing Events
        /// <summary>Exposed event that triggers the refresh of the resource tree</summary>
        public event Action RefreshResourceTree
        {
            add { this.refreshResourceTree += value; }
            remove { this.refreshResourceTree -= value; }
        }

        /// <summary>Exposed event that triggers the clearing of the displayed resource tree</summary>
        public event Action ClearFileControl
        {
            add { this.clearFileControl += value; }
            remove { this.clearFileControl -= value; }
        }

        /// <summary>Exposed event that triggers the clearing of all displayed items</summary>
        public event Action ClearAllDisplay
        {
            add { this.clearAllDisplay += value; }
            remove { this.clearAllDisplay -= value; }
        }

        /// <summary>Exposed event that informs subscribers of the assigned application Path</summary>
        public event Action<String> AssignedApplicationPath
        {
            add { this.assignedApplicationPath += value; }
            remove { this.assignedApplicationPath -= value; }
        }

        /// <summary>Exposed event that informs subscribers of the assigned user Path</summary>
        public event Action<String> AssignedUserPath
        {
            add { this.assignedUserPath += value; }
            remove { this.assignedUserPath -= value; }
        }

        /// <summary>Exposed event that informs subscribers of the assigned detected game</summary>
        protected event Action<GameInstall> AssignedDetectedGame
        {
            add { this.assignedDetectedGame += value; }
            remove { this.assignedDetectedGame -= value; }
        }

        /// <summary>Exposed event that informs subscribers of the assigned detected game engine</summary>
        protected event Action<GameEngine> AssignedDetectedEngine
        {
            add { this.assignedDetectedEngine += value; }
            remove { this.assignedDetectedEngine -= value; }
        }
        #endregion


        //#region Exposed Asset Processing Events
        ///// <summary>Exposed event that informs subscribers to open the provided (IE versioned) key table as they see fit</summary>
        //public event Action<KeyTable> AssetOpenKeyTableInfinity
        //{
        //    add { this.assetOpenKeyTableInfinity += value; }
        //    remove { this.assetOpenKeyTableInfinity -= value; }
        //}
        //#endregion


        #region Construction
        /// <summary>Clears the data that is wrapped by this controller</summary>
        protected void ClearAllData()
        {
            this.assetManager = null;
        }
        #endregion


        #region UI Event Raising Methods
        /// <summary>Raises the RefreshResourceTree event</summary>
        protected void RaiseRefreshResourceTree()
        {
            if (this.refreshResourceTree != null)
                this.refreshResourceTree();
        }

        /// <summary>Raises the ClearFileControl event</summary>
        protected void RaiseClearFileControl()
        {
            if (this.clearFileControl != null)
                this.clearFileControl();
        }

        /// <summary>Raises the ClearAllDisplay event</summary>
        protected void RaiseClearAllDisplay()
        {
            if (this.clearAllDisplay != null)
                this.clearAllDisplay();
        }

        /// <summary>Raises the ClearFileControl event</summary>
        /// <param name="path">Path of the application assigned</param>
        protected void RaiseAssignedApplicationPath(String path)
        {
            if (this.assignedApplicationPath != null)
                this.assignedApplicationPath(path);
        }

        /// <summary>Raises the ClearFileControl event</summary>
        /// <param name="path">Path of the user directory assigned</param>
        protected void RaiseAssignedUserPath(String path)
        {
            if (this.assignedUserPath != null)
                this.assignedUserPath(path);
        }

        /// <summary>Raises the AssignedDetectedGame event</summary>
        /// <param name="game">Detected Game instance</param>
        protected void RaiseAssignedDetectedGame(GameInstall game)
        {
            if (this.assignedDetectedGame != null)
                this.assignedDetectedGame(game);
        }

        /// <summary>Raises the AssignedDetectedEngine event</summary>
        /// <param name="engine">Detected Game engine</param>
        protected void RaiseAssignedDetectedEngine(GameEngine engine)
        {
            if (this.assignedDetectedEngine != null)
                this.assignedDetectedEngine(engine);
        }
        #endregion


        //#region Asset Event Raising Methods
        ///// <summary>Raises the AssetOpenKeyTableInfinity event</summary>
        //protected void RaiseAssetOpenKeyTableInfinity(KeyTable key)
        //{
        //    if (this.assetOpenKeyTableInfinity != null)
        //        this.assetOpenKeyTableInfinity(key);
        //}
        //#endregion


        #region Command and Control
        /// <summary>Opens a game and raises the appropriate events</summary>
        /// <param name="path">Path to the key file opened</param>
        public void OpenKey(String path)
        {
            //tell the UI to wipe its tree, its displayed control and its
            this.RaiseClearAllDisplay();

            String applicationPath = Path.GetDirectoryName(path);

            this.assetManager = AssetManagerFactory.BuildAssetManager(applicationPath);

            //now that the controller has gotten the assets, raise the events to the UI to react accordingly
            this.RaiseAssignedApplicationPath(this.assetManager.GetApplicationDirectory());
            this.RaiseAssignedUserPath(this.assetManager.GetUserDirectory());
            this.RaiseRefreshResourceTree();
        }

        /// <summary>Gets the asset tree which is built to accomodate the specified view</summary>
        /// <param name="viewType">Type of asset tree to build</param>
        /// <returns>The built asset tree's root node</returns>
        public AssetNode GetAssetTree(AssetTree viewType)
        {
            AssetNode tree = null;

            switch (viewType)
            {
                case AssetTree.OverriddenGrouped:
                    tree = this.assetManager.GetAssetTree_GroupedOverridden();
                    break;

                case AssetTree.AllInstancesGrouped:
                    tree = this.assetManager.GetAssetTree_GroupedAllInstances();
                    break;

                case AssetTree.AllInstancesLocation:
                    tree = this.assetManager.GetAssetTree_LocationInstances();
                    break;

                case AssetTree.OverriddenGroupedLocation:
                    tree = this.assetManager.GetAssetTree_LocationGroupedOverridden();
                    break;

                case AssetTree.AllInstancesGroupedLocation:
                    //TODO: implement view #5
                    break;
            }

            return tree;
        }

        /// <summary>Command to open the specified asset</summary>
        /// <param name="reference">Reference to the asset to open</param>
        public void OpenAsset(AssetReference reference)
        {
            Stream data = this.assetManager.GetAssetInstance(reference.Locator);

            //determine what kind of resource I have here
        }
        #endregion
    }
}