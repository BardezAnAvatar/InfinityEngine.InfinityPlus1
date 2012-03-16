using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management
{
    public class MveVideoParams
    {
        /// <summary>Represents the width of the video frames</summary>
        public Int32 Width { get; set; }

        /// <summary>Represents the height of the video frames</summary>
        public Int32 Height { get; set; }

        /// <summary>Represents the number of bits per pixel</summary>
        /// <value>Expects either 16 or 8</value>
        public Int32 BitsPerPixel { get; set; }
    }
}