using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.Image
{
    /// <summary>Does primitive testing on the Direct2D render target, loading a list of bitmaps selectable for display</summary>
    public partial class JpegRenderTestControl : UserControl
    {
        /// <summary>Local class used for the ListBox control.</summary>
        protected class BitmapReference
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
        }

        #region Fields
        /// <summary>Object reference to lock on for the User Interface</summary>
        private Object interfaceLock;

        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.JPEG.Path";
        #endregion

        /// <summary>Default constructor</summary>
        public JpegRenderTestControl()
        {
            InitializeComponent();
            this.interfaceLock = new Object();
        }

        /// <summary>Click event handler for the Initialize button. Loads a list of displayable bitmaps from the config file.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected void btnInitialize_Click(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                if (this.lstboxImages.Items.Count < 1)
                {
                    //load paths from the app.config
                    List<String> paths = ConfigurationHandlerMulti.GetSettingValues(JpegRenderTestControl.configKey);

                    foreach (String path in paths)
                    {
                        //submit a bitap
                        JpegJfifInterchange jpeg;
                        using (FileStream fs = ReusableIO.OpenFile(path))
                            jpeg = JpegJfifParser.ParseJpegFromStream(fs);

                        jpeg.DecodeFloat();

                        Int32 key = this.direct2dRenderControl.AddFrameResource(jpeg.GetFrameFloat());
                        this.lstboxImages.Items.Add(new BitmapReference(path, key));
                    }
                }
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
    }
}