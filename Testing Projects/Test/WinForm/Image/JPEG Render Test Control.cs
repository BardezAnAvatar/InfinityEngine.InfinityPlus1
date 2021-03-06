using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG;
using Bardez.Projects.InfinityPlus1.Test.WinForm;
using Bardez.Projects.Multimedia.MediaBase.Data.Resize;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.Image
{
    /// <summary>Does primitive testing on the Direct2D render target, loading a list of bitmaps selectable for display</summary>
    public partial class JpegRenderTestControl : UserControl
    {
        #region Fields
        /// <summary>Object reference to lock on for the User Interface</summary>
        private Object interfaceLock;

        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.JPEG.Path";

        /// <summary>Collection of decoded JPEG paths and frame references</summary>
        protected List<ImageReference> decodedJpegs { get; set; }

        /// <summary>Object reference to lock on for multithreaded decodeding of JPEGs</summary>
        private Object jpegLock;
        #endregion

        /// <summary>Default constructor</summary>
        public JpegRenderTestControl()
        {
            InitializeComponent();
            this.interfaceLock = new Object();
            this.jpegLock = new Object();
        }

        /// <summary>Click event handler for the Initialize button. Loads a list of displayable bitmaps from the config file.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected void btnInitialize_Click(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                if (this.lstboxImages.Items.Count < 1)
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.LaunchJpegDecoding));
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

        /// <summary>Method that launches the decoding of the JPEG from the config file</summary>
        /// <param name="stateInfo">WaitCallback state parameter</param>
        protected void LaunchJpegDecoding(Object stateInfo)
        {
            DateTime start = DateTime.Now, end;    //debg variables

            //load paths from the app.config
            List<String> paths = ConfigurationHandlerMulti.GetSettingValues(JpegRenderTestControl.configKey);
            this.decodedJpegs = new List<ImageReference>();

            //spool up the strings
            foreach (String path in paths)
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.DecodeSingleJpeg), path);

            //lock on the threads
            while (true)
            {
                Thread.Sleep(100);  //pause for a fraction of a second

                Boolean finished = false;

                lock (this.jpegLock)
                    finished = (this.decodedJpegs.Count == paths.Count);

                if (finished)
                    break;
            }

            end = DateTime.Now;
            TimeSpan decodeTime = end - start;

            //sort the jpegs
            lock (this.jpegLock)    //lock, just to be safe
                this.decodedJpegs.Sort(ImageReference.Compare);    //now sort, then add to the UI list

            //load them into the UI
            this.LoadDecodedJpegItems();
        }

        /// <summary>Method to decode a single JPEG file, in a multi-threaded environment</summary>
        /// <param name="filePath">String of a filepath, casted as an Object for use by ThreadPool</param>
        protected void DecodeSingleJpeg(Object filePath)
        {
            if (filePath is String)
            {
                String path = filePath as String;

                //read the JPEG
                JpegJfifInterchange jpeg;
                using (FileStream fs = ReusableIO.OpenFile(path))
                    jpeg = JpegJfifParser.ParseJpegFromStream(fs, new ResizeDelegateInteger(BilinearFloatSpace.BilinearResampleInteger));

                //decode the JPEG, get its frame data
                jpeg.Decode();
                IMultimediaImageFrame frame = jpeg.GetFrame();
                //dispose the JPEG container?

                //submit a bitap
                Int32 key;
                lock (this.interfaceLock)
                    key = this.direct2dRenderControl.AddFrameResource(frame);

                //add it to the collection
                lock (this.jpegLock)
                    this.decodedJpegs.Add(new ImageReference(path, key));
            }
        }

        /// <summary>Abstract method to load harness items</summary>
        protected virtual void LoadDecodedJpegItems()
        {
            if (this.lstboxImages.InvokeRequired) //check if an invoke is required, call on UI thead
                this.lstboxImages.Invoke(new Action(this.LoadDecodedJpegItems));
            else    //good on existing thread
            {
                lock (this.jpegLock)
                    foreach (ImageReference reference in this.decodedJpegs)
                        this.lstboxImages.Items.Add(reference);
            }
        }
    }
}