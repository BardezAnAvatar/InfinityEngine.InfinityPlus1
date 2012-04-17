using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Save;
using Bardez.Projects.InfinityPlus1.Test.WinForm;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm
{
    /// <summary>Test class for extracting saved resources in SAV files</summary>
    public partial class SavedResourcesTestControl : UserControl
    {
        #region Constants
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.SAV.Path";
        #endregion


        #region Fields
        /// <summary>Object reference to lock on for the User Interface</summary>
        private Object interfaceLock;

        /// <summary>Collection of read files loaded from the config</summary>
        protected List<String> fileCollectionList { get; set; }

        /// <summary>Object reference to lock on for multithreaded decoding of files</summary>
        private Object fileCollectionLock;

        /// <summary>Manager for currently selected save file</summary>
        private SaveManager Manager { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public SavedResourcesTestControl()
        {
            this.InitializeComponent();
            this.DoubleBuffered = true;

            this.interfaceLock = new Object();
            this.fileCollectionLock = new Object();
        }
        #endregion


        #region UI event handlers
        /// <summary>Click event handler for the Initialize button. Loads a list of save files from the config file.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void btnInitialize_Click(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                if (this.lstboxSaves.Items.Count < 1)
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.LaunchSaveFileCollection));
            }
        }

        /// <summary>Click event handler for the Extract Resource button. Opens a save file dialog to extract the file.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void btnExtract_Click(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                String resourceName = this.lstboxResourceCollection.SelectedItem as String;

                this.saveFileDialog.FileName = resourceName;
                DialogResult result = this.saveFileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    using (MemoryStream memStream = this.Manager.ExtractResource(resourceName))
                    using (Stream file = this.saveFileDialog.OpenFile())
                        memStream.CopyTo(file);
                    
                    MessageBox.Show("Resource extracted.");
                }
            }
        }

        /// <summary>Event handler for when the selected index of the listbox changes. Sends a new bitmap index to the render target control.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void lstboxSaves_SelectedIndexChanged(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                //clear existing output
                this.lstboxResourceCollection.Items.Clear();
                this.btnExtract.Enabled = false;
                
                //populate new output
                String path = this.lstboxSaves.SelectedItem as String;
                SaveFile save = new SaveFile();
                using (FileStream file = ReusableIO.OpenFile(path, FileMode.Open, FileAccess.Read))
                    save.Read(file);
                
                this.Manager = new SaveManager(save);

                foreach (SavedResource resource in this.Manager.SavedResources.Resources)
                    this.lstboxResourceCollection.Items.Add(resource.ResourceName);
            }
        }

        /// <summary>Event handler for when the selected index of the listbox changes. Sends a new Bitmap index to the render target control.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void lstboxResourceCollection_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock (this.interfaceLock)
                this.btnExtract.Enabled = true;
        }
        #endregion


        #region File decoding/loading
        /// <summary>Method that launches the collecting of save files from the config file</summary>
        /// <param name="stateInfo">WaitCallback state parameter</param>
        protected void LaunchSaveFileCollection(Object stateInfo)
        {
            //load paths from the app.config
            IList<String> paths = this.GetPaths();
            this.fileCollectionList = new List<String>();

            //load strings into listbox
            foreach (String path in paths)
                this.ControlAction(this.lstboxSaves, () => { this.lstboxSaves.Items.Add(path); } );
        }
        #endregion


        #region Helper methods
        /// <summary>Gets the paths to test from the config file</summary>
        /// <returns>An IList of Strings for file paths</returns>
        protected IList<String> GetPaths()
        {
            return ConfigurationHandlerMulti.GetSettingValues(SavedResourcesTestControl.configKey);
        }

        /// <summary>Generic control action that will perform an Invoke action (such as a setter) on a control</summary>
        /// <param name="c">Control to query for Invoke</param>
        /// <param name="action">Action to perform</param>
        protected virtual void ControlAction(Control c, Action action)
        {
            if (c.InvokeRequired)
                c.Invoke(action);
            else
                action();
        }
        #endregion
    }
}