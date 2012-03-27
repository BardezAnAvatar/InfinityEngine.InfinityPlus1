using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management
{
    /// <summary>Represents general video playback parameters for an MVE file</summary>
    public class MveVideoParams
    {
        #region Fields
        /// <summary>Represents the width of the video frames</summary>
        public Int32 Width { get; set; }

        /// <summary>Represents the height of the video frames</summary>
        public Int32 Height { get; set; }

        /// <summary>Represents the number of bits per pixel</summary>
        /// <value>Expects either 16 or 8</value>
        public Int32 BitsPerPixel { get; set; }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="width">Width of the video frames</param>
        /// <param name="height">Height of the video frames</param>
        /// <param name="bitsPerPixel">Bits per pixel of the stored data</param>
        public MveVideoParams(Int32 width, Int32 height, Int32 bitsPerPixel)
        {
            this.Width = width;
            this.Height = height;
            this.BitsPerPixel = bitsPerPixel;
        }
        #endregion
    }
}