using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Bitmap;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels.Enums;
using Bardez.Projects.MultiMedia.LibAV;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Tester
{
    public partial class ScratchBitmapLoader : Form
    {
        #region Constants
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.BMP.MemLeak.Path";
        #endregion


        #region Fields
        public Int32 bitmapCount, memoryBitmapCount, pictureCount;
        #endregion


        #region Events
        public ScratchBitmapLoader()
        {
            this.bitmapCount = 0;
            this.memoryBitmapCount = 0;
            this.pictureCount = 0;

            InitializeComponent();
        }

        protected void btnLoadBitmapScale_Click(Object sender, EventArgs e)
        {
            this.LoadBitmap();
        }

        protected void btnLoadBitmapIntoLibAVPicture_Click(object sender, EventArgs e)
        {
            this.CopyLibAVPictureFromMemory();
        }

        protected void btnDeclareAndDisposeLibAVPicture_Click(object sender, EventArgs e)
        {
            this.InstantiateLibAVPicture();
        }
        #endregion


        #region Methods
        protected void LoadBitmap()
        {
            String path = ConfigurationHandlerMulti.GetSettingValue(ScratchBitmapLoader.configKey);

            DeviceIndependentBitmap bitmap = new DeviceIndependentBitmap();
            using (FileStream fs = ReusableIO.OpenFile(path))
                bitmap.Read(fs);

            using (MemoryStream ms = bitmap.PixelData.GetPixelData(PixelFormat.RGBA_B8G8R8A8, ScanLineOrder.TopDown, 0, 0))
            {
                String loggedMessage = String.Format("Loading DIBimap # {0}", ++this.bitmapCount);
                this.listboxLoading.Items.Add(loggedMessage);
            }
        }

        protected void InstantiateLibAVPicture()
        {
            using (LibAVPicture test = LibAVPicture.BuildPicture(LibAVPictureDetail.Build(1080, 1920, LibAVPixelFormat.PIX_FMT_BGRA)))
            {
                String loggedMessage = String.Format("Intantiating LibAVPicture # {0}", ++this.pictureCount);
                this.listboxLoading.Items.Add(loggedMessage);
            }
        }

        protected void CopyLibAVPictureFromMemory()
        {
            String path = ConfigurationHandlerMulti.GetSettingValue(ScratchBitmapLoader.configKey);

            DeviceIndependentBitmap bitmap = new DeviceIndependentBitmap();
            using (FileStream fs = ReusableIO.OpenFile(path))
                bitmap.Read(fs);

            MemoryStream ms = bitmap.PixelData.NativeBinaryData;
            LibAVPictureDetail detail = LibAVPictureDetail.Build(bitmap.BitmapInfo.Width, bitmap.BitmapInfo.Height, LibAVPixelFormat.PIX_FMT_BGR24);

            using (LibAVPicture test = LibAVPicture.BuildPicture(detail, ms))
            {
                String loggedMessage = String.Format("Intantiating LibAVPicture from DIBitmap # {0}", ++this.memoryBitmapCount);
                this.listboxLoading.Items.Add(loggedMessage);
            }
        }
        #endregion
    }
}