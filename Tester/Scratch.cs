using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Bardez.Projects.DirectX.Direct2D;
using Direct2D = Bardez.Projects.DirectX.Direct2D;
using Bardez.Projects.InfinityPlus1.Files.External.Image;
using Bardez.Projects.InfinityPlus1.Files.External.Image.Bitmap;
using Bardez.Projects.InfinityPlus1.Output.Visual;

namespace Bardez.Projects.InfinityPlus1.Tester
{
    public partial class Scratch : Form
    {
        public Scratch()
        {
            InitializeComponent();
        }

        private void Scratch_Load(Object sender, EventArgs e)
        {
            try
            {
                String bmpPath = ConfigurationSettings.AppSettings["Test.BMP.Path"];
                DeviceIndependentBitmap dib = new DeviceIndependentBitmap();
                using (FileStream fs = new FileStream(bmpPath, FileMode.Open, FileAccess.Read))
                    dib.Read(fs);

                //get the Bitmap
                Frame bitmap = dib.GetFrame();
                Int32 resourceKey = this.direct2dRenderControl1.AddFrameResource(bitmap);

                //set the underlying control's rendering image to read from
                this.direct2dRenderControl1.SetRenderFrame(resourceKey);
            }
            catch (Exception ex)
            {
            }
        }
    }
}