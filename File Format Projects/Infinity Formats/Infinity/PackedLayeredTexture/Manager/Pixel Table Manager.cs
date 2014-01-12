using System;
using System.Collections.Generic;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.PackedLayeredTexture.Component;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.PackedLayeredTexture.Version;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels.Enums;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.PackedLayeredTexture.Manager
{
    /// <summary>Manager class that exposes the IImage formats for a PLT file, since its palette must be set, and it does not store a palette</summary>
    public class PixelTableManager : IImage
    {
        #region Fields
        /// <summary>Plt instance</summary>
        public PixelTable PLT { get; set; }

        /// <summary>Palette from which to derive colors</summary>
        public IList<Palette> Palettes { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public PixelTableManager() { }

        /// <summary>Partial definition constructor</summary>
        public PixelTableManager(PixelTable plt)
        {
            this.PLT = plt;
        }

        /// <summary>Definition constructor</summary>
        public PixelTableManager(PixelTable plt, IList<Palette> palettes)
        {
            this.PLT = plt;
            this.Palettes = palettes;
        }
        #endregion


        #region IImage methods
        /// <summary>Gets a frame image from the pixel data already in place</summary>
        /// <returns>A frame containing the pixel data</returns>
        public IMultimediaImageFrame GetFrame()
        {
            IMultimediaImageFrame frame = new BasicImageFrame(this.GetPixelData());
            return frame;
        }

        /// <summary>Gets a sub-image of the current image</summary>
        /// <param name="x">Source X position</param>
        /// <param name="y">Source Y position</param>
        /// <param name="width">Width of sub-image</param>
        /// <param name="height">Height of sub-image</param>
        /// <returns>The requested sub-image</returns>
        public IImage GetSubImage(Int32 x, Int32 y, Int32 width, Int32 height)
        {
            return new BasicImage(ImageManipulation.GetSubImage(this.GetPixelData(), x, y, width, height));
        }
        #endregion


        #region Frame support methods
        /// <summary>Builds a pixel data object from the MOS file</summary>
        /// <returns>A new pixeldata object</returns>
        protected PixelData GetPixelData()
        {
            Byte[] data = this.GetPixels();
            PixelData pd = new PixelData(data, ScanLineOrder.BottomUp, PixelFormat.RGBA_B8G8R8A8, null, this.PLT.Header.Height, this.PLT.Header.Width, 0, 0, 32);
            return pd;
        }

        /// <summary>Gets the pixel data array from the tiles' data</summary>
        /// <returns>An BGRA array of pixels</returns>
        protected Byte[] GetPixels()
        {
            Int32 width = this.PLT.Header.Width, height = this.PLT.Header.Height;

            //instantiate the return array
            Byte[] pixels = new Byte[4 * width * height];

            Int32 pixelDataIndex = 0;
            for (Int32 y = 0; y < height; ++y)          //loop through vertical rows
            {
                for (Int32 x = 0; x < width; ++x)       //loop through horizontal columns
                {
                    Pixel pixel = this.PLT.Pixels[(y * width) + x];

                    //go grayscale if no palette provided, use 'intensity'
                    if (this.Palettes == null)
                    {
                        if (pixel.Intensity == Byte.MaxValue)
                        {
                            //perform integer math to avoid floating point slowness
                            pixels[pixelDataIndex] = Byte.MinValue;         //blue
                            pixels[pixelDataIndex + 1] = Byte.MinValue;     //green
                            pixels[pixelDataIndex + 2] = Byte.MinValue;     //red
                            pixels[pixelDataIndex + 3] = Byte.MinValue;     //transparent, and pre-multiplied
                        }
                        else
                        {
                            //perform integer math to avoid floating point slowness
                            pixels[pixelDataIndex] = pixel.Intensity;      //blue
                            pixels[pixelDataIndex + 1] = pixel.Intensity;  //green
                            pixels[pixelDataIndex + 2] = pixel.Intensity;  //red
                            pixels[pixelDataIndex + 3] = Byte.MaxValue; //opaque, and pre-multiplied
                        }
                    }
                    else
                    {
                        //select the channel for the palette. 1-6 are individual, 7-127 are shadow
                        Int32 channel = pixel.MappedColor & 0x7F;
                        if (channel > 7)
                            channel = 7;

                        Palette palette = this.Palettes[channel];
                        Byte[] color = palette.PixelData[pixel.Intensity];   //it needs to be an RGB triplet at least, in BGRA

                        //perform integer math to avoid floating point slowness
                        pixels[pixelDataIndex] = color[0];              //blue
                        pixels[pixelDataIndex + 1] = color[1];          //green
                        pixels[pixelDataIndex + 2] = color[2];          //red
                        pixels[pixelDataIndex + 3] = Byte.MaxValue;     //opaque, and pre-multiplied

                        //if channel == 7, premultiply it to 50% transparency
                        if (channel == 7)
                        {
                            pixels[pixelDataIndex] /= 2;
                            pixels[pixelDataIndex + 1] /= 2;
                            pixels[pixelDataIndex + 2] /= 2;
                            pixels[pixelDataIndex + 3] /= 2;
                        }

                        //finally, if color == 255, premultiply it all to 0
                        if (color[0] == 0 && color[1] == 255 && color[2] == 0)
                        {
                            pixels[pixelDataIndex] = 0;
                            pixels[pixelDataIndex + 1] = 0;
                            pixels[pixelDataIndex + 2] = 0;
                            pixels[pixelDataIndex + 3] = 0;
                        }
                    }

                    pixelDataIndex += 4;
                }
            }

            return pixels;
        }
        #endregion
    }
}