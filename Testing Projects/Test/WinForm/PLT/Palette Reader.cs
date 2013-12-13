using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Bitmap;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.PLT
{
    /// <summary>This class reads the palettes MPALETTE and MPAL256 bitmaps and exposes them</summary>
    //TODO: incorporate RANGES12.BMP
    //TODO: allow for RANDCOLR.2DA random(?) values
    public class PaletteReader
    {
        #region Constants
        /// <summary>Constant key to look up in app.config for MPALETTE</summary>
        protected const String configKey12 = "Test.Palette.12.Path";

        /// <summary>Constant key to look up in app.config for MPAL256</summary>
        protected const String configKey256 = "Test.Palette.256.Path";
        #endregion


        #region Fields
        /// <summary>The DIB for the 256-colors palette</summary>
        protected DeviceIndependentBitmap palette_256 { get; set; }

        /// <summary>The DIB for the 12-colors palette</summary>
        protected DeviceIndependentBitmap palette_12 { get; set; }

        /// <summary>12-Color palette collection</summary>
        public IList<Palette> Palette12 { get; set; }

        /// <summary>256-Color palette collection</summary>
        public IList<Palette> Palette256 { get; set; }
        #endregion


        #region DIB Palette handlers
        /// <summary>Reads & populates the palettes</summary>
        public void InitializePalettes()
        {
            this.ReadPalettes();
            this.GeneratePalettes();
        }

        /// <summary>Reads the palette bitmaps from disk</summary>
        protected void ReadPalettes()
        {
            String path256 = ConfigurationHandlerMulti.GetSettingValue(PaletteReader.configKey256);
            String path12 = ConfigurationHandlerMulti.GetSettingValue(PaletteReader.configKey12);
            
            if (path12 != null)
                using (FileStream file = ReusableIO.OpenFile(path12))
                {
                    this.palette_12 = new DeviceIndependentBitmap();
                    this.palette_12.Read(file);
                }

            if (path256 != null)
                using (FileStream file = ReusableIO.OpenFile(path256))
                {
                    this.palette_256 = new DeviceIndependentBitmap();
                    this.palette_256.Read(file);
                }
        }

        /// <summary>Generates palette collections for both the 12- and 256- color bitmaps</summary>
        protected void GeneratePalettes()
        {
            this.Palette12 = this.GetPaletteFromBitmap(this.palette_12);
            this.Palette256 = this.GetPaletteFromBitmap(this.palette_256);
        }

        /// <summary>Generates a list of palettes for the bitmap passed in</summary>
        /// <param name="bitmap">Bitmap to generate palettes from</param>
        /// <returns>A collection of color palettes</returns>
        protected IList<Palette> GetPaletteFromBitmap(DeviceIndependentBitmap bitmap)
        {
            //knowing that it is RGB24, get the palette data by row
            List<Palette> paletteCollection = null;

            if (bitmap != null) //visual studio designer condition
            {
                Int32 rowSize = this.PackedRowByteWidth(bitmap);

                //knowing that it is RGB24, get the palette data by row
                paletteCollection = new List<Palette>();

                Int32 height = bitmap.PixelData.Metadata.Height;
                Int32 position = 0;
                Int32 width = bitmap.PixelData.Metadata.Width;
                MemoryStream nativeData = bitmap.PixelData.NativeBinaryData;

                for (Int32 rowIndex = 0; rowIndex < height; ++rowIndex)
                {
                    List<PixelBase> pixels = new List<PixelBase>();

                    for (Int32 pixel = 0; pixel < width; ++pixel)
                    {
                        //colors are stored backword at this point.
                        RgbTriplet rgbTriplet = new RgbTriplet(nativeData.ReadByteAtOffset(position + 2), nativeData.ReadByteAtOffset(position + 1), nativeData.ReadByteAtOffset(position));
                        position += 3;
                        pixels.Add(rgbTriplet);
                    }

                    position = (rowIndex + 1) * rowSize;
                    Palette pal = new Palette();
                    pal.Pixels = pixels;
                    paletteCollection.Add(pal);
                }

                //it is a bitmap, so reverse the list.
                paletteCollection.Reverse();

                return paletteCollection;
            }

            return paletteCollection;
        }

        /// <summary>Gets the size, in bytes, of a logical row of pixels based off of the bits per pixel</summary>
        /// <param name="bitmap">Bitmap to get the packed width of</param>
        /// <returns>The byte width queried</returns>
        protected Int32 PackedRowByteWidth(DeviceIndependentBitmap bitmap)
        {
            Int32 rowSize = bitmap.PixelData.Metadata.BitsPerDataPixel * bitmap.PixelData.Metadata.Width;     //bits per row for data
            rowSize = (rowSize / 8) + ((rowSize % 8) > 0 ? 1 : 0);                      //bytes per row
            if (bitmap.PixelData.Metadata.HorizontalPacking > 0)
                rowSize += (rowSize % bitmap.PixelData.Metadata.HorizontalPacking);     //packed bytes per row

            return rowSize;
        }
        #endregion
    }
}