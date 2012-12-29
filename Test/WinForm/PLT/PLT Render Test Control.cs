using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.PixelLocationTable.Manager;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.PixelLocationTable.Version;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels;
using Bardez.Projects.InfinityPlus1.Test.WinForm;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.PLT
{
    /// <summary>Does primitive testing on the Direct2D render target, loading a list of bitmaps selectable for display</summary>
    public partial class PltRenderTestControl : UserControl
    {
        #region Fields
        /// <summary>Object reference to lock on for the User Interface</summary>
        private Object interfaceLock;

        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.PLT.Path";

        /// <summary>Collection of decoded image paths and frame references</summary>
        protected List<PltDisplayable> imageList { get; set; }

        /// <summary>Object reference to lock on for multithreaded decoding of images</summary>
        private Object imageCollectionLock;

        /// <summary>Current/pevious render key displayed</summary>
        protected Int32 currentRenderKey;
        #endregion


        #region Properties
        /// <summary>Rendering control background color</summary>
        protected Color RenderBackgroundColor
        {
            get { return this.direct2dRenderControl.BackColor; }
            set { this.direct2dRenderControl.BackColor = value; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public PltRenderTestControl()
        {
            this.InitializeComponent();
            this.interfaceLock = new Object();
            this.imageCollectionLock = new Object();
            this.currentRenderKey = -1;

            //assign the selected color event
            this.paletteSelction.PaletteChanged += this.RenderSelectedImage;
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
                this.direct2dRenderControl.SetRenderFrameAndRender(-1);
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
        protected void lstboxImages_SelectedIndexChanged(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                PltDisplayable imgRef = this.lstboxImages.SelectedItem as PltDisplayable;

                //create a new image for it
                this.RenderSelectedImage();
            }
        }
        #endregion


        #region Reading/decoding
        /// <summary>Method that launches the decoding of the image from the config file</summary>
        /// <param name="stateInfo">WaitCallback state parameter</param>
        protected void LaunchImageDecoding(Object stateInfo)
        {
            DateTime start = DateTime.Now, end;    //debg variables

            //load paths from the app.config
            List<String> paths = ConfigurationHandlerMulti.GetSettingValues(PltRenderTestControl.configKey);
            this.imageList = new List<PltDisplayable>();

            //spool up the strings
            for (Int32 index = 0; index < paths.Count; ++index)
            {
                String path = paths[index];
                Int32 order = index;
                ThreadPool.QueueUserWorkItem(o => this.DecodeSingleImage(path, order));
            }

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
                this.imageList.Sort(PltDisplayable.Compare);    //now sort, then add to the UI list

            //load them into the UI
            this.LoadDecodedImages();
        }

        /// <summary>Method to decode a single image file, in a multi-threaded environment</summary>
        /// <param name="path">String of a filepath</param>
        /// <param name="order">Order to sort by</param>
        protected void DecodeSingleImage(String path, Int32 order)
        {
            //read the image
            PixelTable plt = new PixelTable();
            using (FileStream fs = ReusableIO.OpenFile(path))
                plt.Read(fs);

            //create a bogus palette
            List<PixelBase> palette = new List<PixelBase>();
            for (Int32 index = 0; index < 8; ++index)
                palette.Add(new RgbTriplet(255, 255, 255));

            //generat the manager
            PixelTableManager manager = new PixelTableManager(plt);

            //add it to the collection
            lock (this.imageCollectionLock)
                this.imageList.Add(new PltDisplayable(manager, path, order));
        }

        /// <summary>Abstract method to load harness items</summary>
        protected virtual void LoadDecodedImages()
        {
            if (this.lstboxImages.InvokeRequired) //check if an invoke is required, call on UI thead
                this.lstboxImages.Invoke(new Action(this.LoadDecodedImages));
            else    //good on existing thread
            {
                lock (this.imageCollectionLock)
                    foreach (PltDisplayable reference in this.imageList)
                        this.lstboxImages.Items.Add(reference);
            }
        }
        #endregion


        #region Display
        protected void RenderSelectedImage()
        {
            PltDisplayable imgRef = this.lstboxImages.SelectedItem as PltDisplayable;

            if (imgRef != null)
            {
                //Translate the palette from the control to one for the manager
                imgRef.Manager.Palettes = this.paletteSelction.Palettes;

                //decode the image, get its frame data
                IMultimediaImageFrame frame = imgRef.Manager.GetFrame();

                //submit a bitap
                Int32 key;
                lock (this.interfaceLock)
                    key = this.direct2dRenderControl.AddFrameResource(frame);

                //remove old image
                this.direct2dRenderControl.FreeFrameResource(this.currentRenderKey);
                
                //render new one
                this.direct2dRenderControl.SetRenderFrameAndRender(key);

                //persist the rendered key
                this.currentRenderKey = key;
            }
        }
        #endregion
    }
}