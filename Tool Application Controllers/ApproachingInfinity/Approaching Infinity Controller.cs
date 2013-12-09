using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.KeyTable;
using Bardez.Projects.InfinityPlus1.Logic.Infinity;

namespace Bardez.Projects.InfinityPlus1.Tools.Controllers.ApproachingInfinity
{
    /// <summary>This class is the controller for the Approaching Infinity application</summary>
    public class ApproachingInfinityController
    {
        #region Fields
        /// <summary>The read and processed chitin.key file</summary>
        protected KeyTable keyFile;

        /// <summary>Resource collection mapper</summary>
        protected ResourceMapper resourceMapper;
        #endregion


        #region Properties
        #endregion


        #region Local events
        /// <summary>Local event that triggers a refresh of the resource tree</summary>
        protected event Action refreshResourceTree;

        /// <summary>Local event that triggers the clearing of the displayed resource tree</summary>
        protected event Action clearFileControl;

        /// <summary>Local event that triggers the clearing of all displayed items</summary>
        protected event Action clearAllDisplay;

        /// <summary>Local event that informs subscribers of detected file count</summary>
        protected event Action<Int64> detectedFileCount;

        /// <summary>Local event that informs subscribers of the assigned application Path</summary>
        protected event Action<String> assignedApplicationPath;

        /// <summary>Local event that informs subscribers of the assigned user Path</summary>
        protected event Action<String> assignedUserPath;
        #endregion


        #region Exposed Events
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

        /// <summary>Exposed event that informs subscribers of detected file count</summary>
        public event Action<Int64> DetectedFileCount
        {
            add { this.detectedFileCount += value; }
            remove { this.detectedFileCount -= value; }
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
        #endregion


        #region Construction
        protected void ClearAll()
        {
            this.keyFile = null;
            this.resourceMapper = null;
            this.
        }
        #endregion


        #region Event Raising Methods
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
        /// <param name="count">Count of files detected</param>
        protected void RaiseDetectedFileCount(Int64 count)
        {
            if (this.detectedFileCount != null)
                this.detectedFileCount(count);
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
        #endregion


        #region Command and Control
        /// <summary>Opens a game and raises the appropriate events</summary>
        /// <param name="path">Path to the key file opened</param>
        public void OpenKey(String path)
        {
            String applicationPath = Path.GetDirectoryName(path);

            this.
        }
        #endregion
    }
}