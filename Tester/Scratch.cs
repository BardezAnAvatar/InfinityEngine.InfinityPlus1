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
using Bardez.Projects.InfinityPlus1.Files.External.Image.JPEG;
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
                String bmpPath = ConfigurationManager.AppSettings["Test.JPEG.Path"];
                JpegJfifInterchange jpeg;
                using (FileStream fs = new FileStream(bmpPath, FileMode.Open, FileAccess.Read))
                    jpeg = JpegJfifParser.ParseJpegFromStream(fs);

                jpeg.DecodeFloat();
                //jpeg.DecodeInteger();

                Application.DoEvents();

                Frame frame = jpeg.GetFrameFloat();
                //Frame frame = jpeg.GetFrameInteger();
                
                Byte[] data = frame.Pixels.GetPixelData(Files.External.Image.Pixels.Enums.PixelFormat.RGB_B8G8R8, Files.External.Image.Enums.ScanLineOrder.TopDown,   0, 0);

                Int32 key = this.direct2dRenderControl1.AddFrameResource(frame);
                this.direct2dRenderControl1.SetRenderFrame(key);
            }
            catch (Exception ex)
            {
            }
        }
    }
}