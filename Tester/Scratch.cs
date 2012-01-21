﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

using Bardez.Projects.DirectX.Direct2D;
using Direct2D = Bardez.Projects.DirectX.Direct2D;
using ExternalPixelEnums = Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Pixels.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Resize;
using Bardez.Projects.InfinityPlus1.Output.Visual;

namespace Bardez.Projects.InfinityPlus1.Tester
{
    /// <summary>Scratch Windows form to quickly test code</summary>
    public partial class Scratch : Form
    {
        /// <summary>Default constructor</summary>
        public Scratch()
        {
            InitializeComponent();
        }

        /// <summary>Form Load event handler</summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event parameters</param>
        private void Scratch_Load(Object sender, EventArgs e)
        {
            try
            {
                String bmpPath = ConfigurationManager.AppSettings["Test.JPEG.Path"];
                JpegJfifInterchange jpeg;
                using (FileStream fs = new FileStream(bmpPath, FileMode.Open, FileAccess.Read))
                    jpeg = JpegJfifParser.ParseJpegFromStream(fs, new ResizeDelegateInteger(NearestNeighborIntegerSpace.NearestNeighborResampleInteger));

                Application.DoEvents();

                jpeg.Decode();
                Frame frame = jpeg.GetFrame();
                Byte[] data = frame.Pixels.GetPixelData(ExternalPixelEnums.PixelFormat.RGB_B8G8R8, ScanLineOrder.TopDown, 0, 0);

                Int32 key = this.direct2dRenderControl1.AddFrameResource(frame);
                this.direct2dRenderControl1.SetRenderFrame(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}