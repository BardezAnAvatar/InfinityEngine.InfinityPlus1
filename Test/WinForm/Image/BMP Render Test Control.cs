using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Bitmap;
using Bardez.Projects.InfinityPlus1.Test.WinForm;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.Image
{
    /// <summary>Does primitive testing on the Direct2D render target, loading a list of bitmaps selectable for display</summary>
    public partial class BitmapRenderTestControl : UserControl
    {
        #region Fields
        /// <summary>Object reference to lock on for the User Interface</summary>
        private Object interfaceLock;

        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.BMP.Path";
        #endregion

        /// <summary>Default constructor</summary>
        public BitmapRenderTestControl()
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
                    List<String> paths = ConfigurationHandlerMulti.GetSettingValues(BitmapRenderTestControl.configKey);

                    foreach (String path in paths)
                    {
                        //submit a bitap
                        DeviceIndependentBitmap bitmap = new DeviceIndependentBitmap();
                        using (FileStream fs = ReusableIO.OpenFile(path))
                            bitmap.Read(fs);

                        Int32 key = this.direct2dRenderControl.AddFrameResource(bitmap.GetFrame());
                        this.lstboxImages.Items.Add(new ImageReference(path, key));
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
    }
}