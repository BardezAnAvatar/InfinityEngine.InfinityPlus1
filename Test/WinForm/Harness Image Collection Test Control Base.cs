using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Resize;
using Bardez.Projects.InfinityPlus1.Test.WinForm;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm
{
    public abstract partial class HarnessImageCollectionTestControlBase<ImageCollection> : UserControl where ImageCollection : IImageSet, IInfinityFormat, new()
    {
        #region Constants
        /// <summary>Rendering key used to blank out an image</summary>
        protected const Int32 BlankRenderKey = -1;
        #endregion


        #region Fields
        /// <summary>Object reference to lock on for the User Interface</summary>
        private Object interfaceLock;

        /// <summary>Collection of decoded image paths and frame references</summary>
        protected List<ImageCollectionReference> imageCollectionList { get; set; }

        /// <summary>Object reference to lock on for multithreaded decoding of images</summary>
        private Object imageCollectionLock;

        /// <summary>Rendering control background color</summary>
        protected Color RenderBackgroundColor
        {
            get { return this.direct2dRenderControl.BackColor; }
            set { this.direct2dRenderControl.BackColor = value; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public HarnessImageCollectionTestControlBase()
        {
            this.InitializeComponent();
            this.DoubleBuffered = true;

            this.interfaceLock = new Object();
            this.imageCollectionLock = new Object();
        }
        #endregion


        #region UI event handlers
        /// <summary>Click event handler for the Initialize button. Loads a list of displayable bitmaps from the config file.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void btnInitialize_Click(Object sender, EventArgs e)
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
        protected virtual void btnClearDisplay_Click(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
                this.RenderDisplay(HarnessImageCollectionTestControlBase<ImageCollection>.BlankRenderKey);
        }

        /// <summary>Event handler for when the selected index of the listbox changes. Sends a new bitmap index to the render target control.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void lstboxImages_SelectedIndexChanged(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                //clear existing output
                this.RenderDisplay(HarnessImageCollectionTestControlBase<ImageCollection>.BlankRenderKey);
                this.lstboxImageCollection.Items.Clear();
                
                //populate new output
                ImageCollectionReference imgColRef = this.lstboxImages.SelectedItem as ImageCollectionReference;
                for (Int32 image = 0; image < imgColRef.ImageList.Count; ++image)
                    this.lstboxImageCollection.Items.Add(imgColRef.ImageList[image]);
            }
        }

        /// <summary>Event handler for when the selected index of the listbox changes. Sends a new Bitmap index to the render target control.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void lstboxImageCollection_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                ImageReference imgRef = this.lstboxImageCollection.SelectedItem as ImageReference;

                if (imgRef != null)
                    this.RenderDisplay(imgRef.RenderKey);
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
        #endregion


        #region Image decoding/loading
        /// <summary>Method that launches the decoding of the image from the config file</summary>
        /// <param name="stateInfo">WaitCallback state parameter</param>
        protected void LaunchImageDecoding(Object stateInfo)
        {
            //load paths from the app.config
            IList<String> paths = this.GetPaths();
            this.imageCollectionList = new List<ImageCollectionReference>();

            //spool up the strings
            for (Int32 pathIndex = 0; pathIndex < paths.Count; ++pathIndex)
            {
                ImageCollectionReference col = new ImageCollectionReference(paths[pathIndex], pathIndex);
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.DecodeSingleImageCollection), col);
            }

            //lock on the threads
            while (true)
            {
                Thread.Sleep(100);  //pause for a fraction of a second

                Boolean finished = false;

                lock (this.imageCollectionLock)
                    finished = (this.imageCollectionList.Count == paths.Count);

                if (finished)
                    break;
            }

            //sort the images
            lock (this.imageCollectionLock)    //lock, just to be safe
                this.imageCollectionList.Sort(ImageCollectionReference.Compare);    //now sort, then add to the UI list

            //load them into the UI
            this.LoadDecodedImages();
        }

        /// <summary>Method to decode a single image file, in a multi-threaded environment</summary>
        /// <param name="imageCollection">ImageCollectionReference containing String of a filepath, casted as an Object for use by ThreadPool</param>
        protected void DecodeSingleImageCollection(Object imageCollection)
        {
            //only proceed if there is a valid cast available
            if (imageCollection is ImageCollectionReference)
            {
                ImageCollectionReference col = imageCollection as ImageCollectionReference;
                String path = col.ImageDescription; //description is the path

                //read the image
                ImageCollection image = new ImageCollection();
                using (FileStream fs = ReusableIO.OpenFile(path))
                    image.Read(fs);

                //loop through the frames in the image set
                for (Int32 frameIndex = 0; frameIndex < image.FrameCount; ++frameIndex)
                {
                    //Get its frame data
                    IMultimediaImageFrame frame = image.GetFrame(frameIndex);

                    //submit a bitap
                    Int32 key;
                    lock (this.interfaceLock)
                        key = this.direct2dRenderControl.AddFrameResource(frame);

                    String imageName = String.Format("Image # {0, 4}", frameIndex);

                    col.ImageList.Add(new ImageReference(imageName, key, frameIndex));
                }

                //add the ImageCollectionReference to the main listbox collection
                lock (this.imageCollectionLock)
                    this.imageCollectionList.Add(col);
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
                    foreach (ImageCollectionReference reference in this.imageCollectionList)
                        this.lstboxImages.Items.Add(reference);
            }
        }
        #endregion


        #region Helper methods
        /// <summary>Sets the render Frame key to -1 and calls the render methood to the UI</summary>
        /// <param name="renderKey">Frame key used to set the rendering image</param>
        /// <remarks>Any locking must be done outside this method</remarks>
        protected virtual void RenderDisplay(Int32 renderKey)
        {
            this.direct2dRenderControl.SetRenderFrameAndRender(renderKey);
        }

        /// <summary>Gets the paths to test from the config file</summary>
        /// <returns>An IList of Strings for file paths</returns>
        protected abstract IList<String> GetPaths();
        #endregion
    }
}