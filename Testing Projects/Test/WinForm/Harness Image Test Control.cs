using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm
{
    /// <summary>Represents a base class for UI harnesses that display images</summary>
    public abstract partial class HarnessImageTestControl : UserControl
    {
        #region Fields
        /// <summary>Object reference to lock on for the User Interface</summary>
        private Object interfaceLock;

        /// <summary>Collection of decoded image paths and frame references</summary>
        protected List<ImageReference> imageList { get; set; }

        /// <summary>Object reference to lock on for multithreaded decoding of images</summary>
        private Object imageCollectionLock;
        #endregion


        #region Properties
        /// <summary>Exposes the configuration key to use to pull from the app.config or similar source</summary>
        protected abstract String ConfigKey { get; }

        /// <summary>Rendering control background color</summary>
        protected Color RenderBackgroundColor
        {
            get { return this.direct2dRenderControl.BackColor; }
            set { this.direct2dRenderControl.BackColor = value; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public HarnessImageTestControl()
        {
            this.InitializeComponent();
            this.DoubleBuffered = true;

            this.interfaceLock = new Object();
            this.imageCollectionLock = new Object();
        }
        #endregion


        #region Event handlers
        /// <summary>Click event handler for the Initialize button. Loads a list of displayable bitmaps from the config file.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected void btnInitialize_Click(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                if (this.lstboxImages.Items.Count < 1)
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.LaunchImageDecoding));
            }
        }

        /// <summary>Click event hander for the Clear Display button. Clears the rendering control of its contents.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected void btnClearDisplay_Click(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                this.direct2dRenderControl.SetRenderFrameAndRender(-1);
            }
        }

        /// <summary>Event handler for when the Choose background color button is clicked</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void btnChooseColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.AllowFullOpen = true;
            cd.AnyColor = true;
            cd.ShowDialog();
            this.RenderBackgroundColor = cd.Color;
        }

        /// <summary>Event handler for when the selected index of the listbox changes. Sends a new bitmap index to the render target control.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        private void lstboxImages_SelectedIndexChanged(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                ImageReference imgRef = this.lstboxImages.SelectedItem as ImageReference;

                if (imgRef != null)
                {
                    this.direct2dRenderControl.SetRenderFrameAndRender(imgRef.RenderKey);
                }
            }
        }
        #endregion


        #region Image decoding
        /// <summary>Opens & reads the image from the provide path</summary>
        /// <param name="path">Path to read the animation from</param>
        /// <returns>The opened & read animation</returns>
        protected abstract IImage ReadImage(String path);

        /// <summary>Method that launches the decoding of the image from the config file</summary>
        /// <param name="stateInfo">WaitCallback state parameter</param>
        protected void LaunchImageDecoding(Object stateInfo)
        {
            DateTime start = DateTime.Now, end;    //debg variables

            //load paths from the app.config
            List<String> paths = ConfigurationHandlerMulti.GetSettingValues(this.ConfigKey);
            this.imageList = new List<ImageReference>();

            //spool up the strings
            foreach (String path in paths)
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.DecodeSingleImage), path);

            //lock on the threads
            while (true)
            {
                Thread.Sleep(100);  //pause for a fraction of a second

                Boolean finished = false;

                lock (this.imageCollectionLock)
                    finished = (this.imageList.Count == paths.Count);

                if (finished)
                    break;
            }

            end = DateTime.Now;
            TimeSpan decodeTime = end - start;

            //sort the images
            lock (this.imageCollectionLock)    //lock, just to be safe
                this.imageList.Sort(ImageReference.Compare);    //now sort, then add to the UI list

            //load them into the UI
            this.LoadDecodedImages();
        }

        /// <summary>Method to decode a single image file, in a multi-threaded environment</summary>
        /// <param name="filePath">String of a filepath, casted as an Object for use by ThreadPool</param>
        protected void DecodeSingleImage(Object filePath)
        {
            if (filePath is String)
            {
                String path = filePath as String;

                //read the image
                IImage image = this.ReadImage(path);

                //decode the image, get its frame data
                IMultimediaImageFrame frame = image.GetFrame();

                //submit a bitap
                Int32 key;
                lock (this.interfaceLock)
                    key = this.direct2dRenderControl.AddFrameResource(frame);

                //add it to the collection
                lock (this.imageCollectionLock)
                    this.imageList.Add(new ImageReference(path, key));
            }
        }

        /// <summary>Abstract method to load harness items</summary>
        protected virtual void LoadDecodedImages()
        {
            if (this.lstboxImages.InvokeRequired) //check if an invoke is required, call on UI thead
                this.lstboxImages.Invoke(new Action(this.LoadDecodedImages));
            else    //good on existing thread
            {
                lock (this.imageCollectionLock)
                    foreach (ImageReference reference in this.imageList)
                        this.lstboxImages.Items.Add(reference);
            }
        }
        #endregion
    }
}