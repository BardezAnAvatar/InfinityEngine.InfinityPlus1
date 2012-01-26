using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Resize;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.Image
{
    /// <summary>Does primitive testing on the Direct2D render target, loading a list of bitmaps selectable for display</summary>
    public partial class JpegRenderTestControl : UserControl
    {
        /// <summary>Local class used for the ListBox control.</summary>
        protected class BitmapReference : IComparable
        {
            #region Fields
            /// <summary>Exposes the path to the bitmap file</summary>
            public String BitmapPath { get; set; }

            /// <summary>Represents the unique key to the bitmap in device memory.</summary>
            public Int32 RenderKey { get; set; }
            #endregion


            #region Construction
            /// <summary>Default constructor</summary>
            public BitmapReference() { }

            /// <summary>Default constructor</summary>
            public BitmapReference(String path, Int32 key)
            {
                this.BitmapPath = path;
                this.RenderKey = key;
            }
            #endregion


            #region Overridden Methods
            /// <summary>Overrides the ToString() method, returning the file path.</summary>
            /// <returns>BitmapPath</returns>
            public override String ToString()
            {
                return this.BitmapPath;
            }
            #endregion


            public Int32 CompareTo(Object comparable)
            {
                Int32 comparison;

                if (comparable is BitmapReference)
                    comparison = BitmapReference.Compare(this, comparable as BitmapReference);
                else
                    comparison = -1;

                return comparison;
            }

            public static Int32 Compare(BitmapReference left, BitmapReference right)
            {
                Int32 comparison;

                if (left == right)      //same reference
                    comparison = 0;
                else if (left == null)  //not both null
                    comparison = -1;
                else if (right == null)  //not both null
                    comparison = 1;
                else
                    comparison = String.Compare(left.BitmapPath, right.BitmapPath);

                return comparison;
            }
        }

        #region Fields
        /// <summary>Object reference to lock on for the User Interface</summary>
        private Object interfaceLock;

        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.JPEG.Path";

        /// <summary>Collection of decoded JPEG paths and frame references</summary>
        protected List<BitmapReference> decodedJpegs { get; set; }

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
            {
                this.direct2dRenderControl.SetRenderFrame(-1);
                this.direct2dRenderControl.Render();
            }
        }

        /// <summary>Event handler for when the selected index of the listbox changes. Sends a new bitmap index to the render target control.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        private void lstboxImages_SelectedIndexChanged(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                BitmapReference bmpRef = this.lstboxImages.SelectedItem as BitmapReference;

                if (bmpRef != null)
                {
                    this.direct2dRenderControl.SetRenderFrame(bmpRef.RenderKey);
                    this.direct2dRenderControl.Render();
                }
            }
        }

        /// <summary>Method that launches the decoding of the JPEG from the config file</summary>
        /// <param name="stateInfo">WaitCallback state parameter</param>
        protected void LaunchJpegDecoding(Object stateInfo)
        {
            DateTime start = DateTime.Now, end;    //debg variables

            //load paths from the app.config
            List<String> paths = ConfigurationHandlerMulti.GetSettingValues(JpegRenderTestControl.configKey);
            this.decodedJpegs = new List<BitmapReference>();

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
                this.decodedJpegs.Sort(BitmapReference.Compare);    //now sort, then add to the UI list

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
                Frame frame = jpeg.GetFrame();
                //dispose the JPEG container?

                //submit a bitap
                Int32 key;
                lock (this.interfaceLock)
                    key = this.direct2dRenderControl.AddFrameResource(frame);

                //add it to the collection
                lock (this.jpegLock)
                    this.decodedJpegs.Add(new BitmapReference(path, key));
            }
        }

        /// <summary>Abstract method to load harness items</summary>
        protected virtual void LoadDecodedJpegItems()
        {
            if (this.lstboxImages.InvokeRequired) //check if an invoke is required, call on UI thead
                this.lstboxImages.Invoke(new VoidInvoke(this.LoadDecodedJpegItems));
            else    //good on existing thread
            {
                lock (this.jpegLock)
                    foreach (BitmapReference reference in this.decodedJpegs)
                        this.lstboxImages.Items.Add(reference);
            }
        }
    }
}