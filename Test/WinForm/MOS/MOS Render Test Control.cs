using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.MapOfScreen;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Resize;
using Bardez.Projects.InfinityPlus1.Test.WinForm;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.MOS
{
    /// <summary>Does primitive testing on the Direct2D render target, loading a list of bitmaps selectable for display</summary>
    public partial class MosRenderTestControl : UserControl
    {
        #region Fields
        /// <summary>Object reference to lock on for the User Interface</summary>
        private Object interfaceLock;

        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.MOS1.Path";

        /// <summary>Collection of decoded image paths and frame references</summary>
        protected List<ImageReference> imageList { get; set; }

        /// <summary>Object reference to lock on for multithreaded decoding of images</summary>
        private Object mosCollectionLock;
        #endregion

        /// <summary>Default constructor</summary>
        public MosRenderTestControl()
        {
            InitializeComponent();
            this.interfaceLock = new Object();
            this.mosCollectionLock = new Object();
        }

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
                this.direct2dRenderControl.SetRenderFrameAndRender(-1);
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
                    this.direct2dRenderControl.SetRenderFrameAndRender(imgRef.RenderKey);
            }
        }

        /// <summary>Method that launches the decoding of the image from the config file</summary>
        /// <param name="stateInfo">WaitCallback state parameter</param>
        protected void LaunchImageDecoding(Object stateInfo)
        {
            DateTime start = DateTime.Now, end;    //debg variables

            //load paths from the app.config
            List<String> paths = ConfigurationHandlerMulti.GetSettingValues(MosRenderTestControl.configKey);
            this.imageList = new List<ImageReference>();

            //spool up the strings
            foreach (String path in paths)
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.DecodeSingleImage), path);

            //lock on the threads
            while (true)
            {
                Thread.Sleep(100);  //pause for a fraction of a second

                Boolean finished = false;

                lock (this.mosCollectionLock)
                    finished = (this.imageList.Count == paths.Count);

                if (finished)
                    break;
            }

            end = DateTime.Now;
            TimeSpan decodeTime = end - start;

            //sort the images
            lock (this.mosCollectionLock)    //lock, just to be safe
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
                MapOfScreen1 mos = new MapOfScreen1();
                using (FileStream fs = ReusableIO.OpenFile(path))
                    mos.Read(fs);

                //decode the image, get its frame data
                IMultimediaImageFrame frame = mos.GetFrame();

                //submit a bitap
                Int32 key;
                lock (this.interfaceLock)
                    key = this.direct2dRenderControl.AddFrameResource(frame);

                //add it to the collection
                lock (this.mosCollectionLock)
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
                lock (this.mosCollectionLock)
                    foreach (ImageReference reference in this.imageList)
                        this.lstboxImages.Items.Add(reference);
            }
        }
    }
}