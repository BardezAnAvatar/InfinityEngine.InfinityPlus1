using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Bitmap.Enums;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels.Enums;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Bitmap
{
    /// <summary>Represents a raster graphics image file format used to store bitmap digital images, independently of the display device (such as a graphics adapter).</summary>
    /// <remarks>
    ///     MSDN has a lot of Bitmap information.
    ///     For storage related data, see: http://msdn.microsoft.com/en-us/library/dd183391%28v=vs.85%29.aspx
    ///     For additional tehcnical info, see: http://en.wikipedia.org/wiki/BMP_file_format
    /// </remarks>
    public class DeviceIndependentBitmap : IImage
    {
        #region Properties
        /// <summary>contains information about the type, size, and layout of a DIB.</summary>
        public BitmapFileHeader FileHeader { get; set; }

        /// <summary>
        ///     Specifies the width and height of the bitmap, in pixels;
        ///     the color format (count of color planes and color bits-per-pixel);
        ///     whether the bitmap data was compressed before storage and the type of compression used;
        ///     the number of bytes of bitmap data; the resolution of the bitmap;
        ///     and the number of colors represented in the data.
        /// </summary>
        public BitmapInfoWrapper BitmapInfo { get; set; }

        /// <summary>Contains the pixel data. Either is a reference to a palette index or actual pixel data.</summary>
        /// <value>Each 'row' of data has a width with an alignment multiple of 4 bytes.</value>
        public PixelData PixelData { get; set; }

        //Profile data is not implemented
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.FileHeader = new BitmapFileHeader();
            this.BitmapInfo = new BitmapInfoWrapper();
        }
        #endregion


        #region Methods
        /// <summary>Gets the size, in bytes, of a logical row of pixels based off of the bits per pixel</summary>
        /// <param name="bitsPerPixel">The bits per pixel of the row (compressed or decompressed)</param>
        /// <returns>The byte width queried</returns>
        protected Int32 GetBytePackedWidth(Int32 bitsPerPixel, Int32 width)
        {
            Int32 rowSize = bitsPerPixel * width;                   //bits per row for data
            rowSize = (rowSize / 8) + (rowSize % 8) > 0 ? 1 : 0;    //bytes per row
            rowSize += (rowSize % 4);                               //packed bytes per row

            return rowSize;
        }

        /// <summary>Reads the bitmap pixel data from the input stream</summary>
        /// <param name="input">Input stream to read from</param>
        protected void ReadData(Stream input)
        {
            Palette pal = null;

            //is there a palette to read?
            switch (this.BitmapInfo.BitCount)
            {
                case BitsPerPixel.Bpp1:
                case BitsPerPixel.Bpp2:
                case BitsPerPixel.Bpp4:
                case BitsPerPixel.Bpp8:
                    pal = this.ReadPalette(input);
                    break;
            }

            //seek to the pixel data
            ReusableIO.SeekIfAble(input, this.FileHeader.BitmapDataOffset);

            //read the pixel data, aligned to 4 bytes
            Int64 rowWidth = ((((UInt32)this.BitmapInfo.BitCount) * this.BitmapInfo.Width) / 32U) * 4U;
            Int64 dataSize = rowWidth * this.BitmapInfo.Height;

            Int64 pixelDataSize = this.BitmapInfo.Header is BitmapInfoHeader_v3 ? this.BitmapInfo.Header3.SizeImage : dataSize;
            if (pixelDataSize == 0) //BI_RBG?
                pixelDataSize = dataSize;

            Byte[] pixelBinData = ReusableIO.BinaryRead(input, pixelDataSize);

            //decode RLE
            if (this.BitmapInfo.Header is BitmapInfoHeader_v3)
                switch (this.BitmapInfo.Header3.Compression)
                {
                    case BitmapCompression.Rle4:
                        pixelBinData = this.DecodeRLE4(pixelBinData);
                        break;
                    case BitmapCompression.Rle8:
                        pixelBinData = this.DecodeRLE8(pixelBinData);
                        break;
                    default:
                        break;
                }

            // set up the height and scan line order, and pixel format
            Int32 height = this.BitmapInfo.Height;
            ScanLineOrder order = ScanLineOrder.BottomUp;

            if (height < 0)
            {
                order = ScanLineOrder.TopDown;
                height = -height;
            }

            //set the pixel format
            PixelFormat format = PixelFormat.RGBA_B8G8R8A8; //default

            switch (this.BitmapInfo.BitCount)
            {
                case BitsPerPixel.Bpp1:
                case BitsPerPixel.Bpp2:
                case BitsPerPixel.Bpp4:
                case BitsPerPixel.Bpp8:
                    if (this.BitmapInfo.Header is BitmapInfoHeader_v3)
                        format = PixelFormat.RGBA_B8G8R8A8;
                    else
                        format = PixelFormat.RGB_B8G8R8;
                    break;
                case BitsPerPixel.Bpp16:
                    format = PixelFormat.RGB_B5G5R5X1;
                    break;
                case BitsPerPixel.Bpp24:
                    format = PixelFormat.RGB_B8G8R8;
                    break;
                case BitsPerPixel.Bpp32:
                    format = PixelFormat.RGBA_B8G8R8A8;
                    break;
            }

            //create the pixel data object
            this.PixelData = new PixelData(pixelBinData, order, format, pal, height, this.BitmapInfo.Width, 4, 0, (Int32)this.BitmapInfo.BitCount);
        }

        /// <summary>Reads the palette from the bitmap file's input stream</summary>
        /// <param name="input">Input stream to read from</param>
        /// <returns>A populted Palette object</returns>
        protected Palette ReadPalette(Stream input)
        {
            Palette pal = null;

            if (this.BitmapInfo.Header != null)
            {
                pal = new Palette();
                UInt32 paletteSize;

                //get the default palette size
                switch (this.BitmapInfo.BitCount)
                {
                    case BitsPerPixel.Bpp1:
                        paletteSize = 2U;
                        break;
                    case BitsPerPixel.Bpp2:
                        paletteSize = 4U;
                        break;
                    case BitsPerPixel.Bpp4:
                        paletteSize = 16U;
                        break;
                    case BitsPerPixel.Bpp8:
                    default:
                        paletteSize = 256U;
                        break;
                }

                // 24 bpp if version2 header, otherwise 32
                pal.BitsPerPixel = this.BitmapInfo.Header is BitmapInfoHeader_v3 ? 32 : 24;  //RGBA or RGBX (X is unused)
                pal.Pixels = new List<PixelBase>();

                //if colors used is available, use it instead of full palette size
                if (this.BitmapInfo.Header is BitmapInfoHeader_v3)
                    paletteSize = this.BitmapInfo.Header3.ColorsUsed;

                //read the palette pixels
                for (Int32 color = 0; color < paletteSize; ++color)
                {
                    RgbTriplet pixel;

                    Byte[] pixelBytes = ReusableIO.BinaryRead(input, (pal.BitsPerPixel / 8));

                    // do the conversion to a pixel :§
                    if (this.BitmapInfo.BitCount == BitsPerPixel.Bpp32)
                    {
                        if (pal.BitsPerPixel == 24)
                            pixel = new RgbQuad(pixelBytes[2], pixelBytes[1], pixelBytes[0], (Byte)255);  //invent an opaque alpha channel
                        else
                            pixel = new RgbQuad(pixelBytes[2], pixelBytes[1], pixelBytes[0], pixelBytes[3]);
                    }
                    else
                    {
                        if (pal.BitsPerPixel == 24)
                            pixel = new RgbTriplet(pixelBytes[2], pixelBytes[1], pixelBytes[0]);
                        else
                            pixel = new RgbTriplet(pixelBytes[2], pixelBytes[1], pixelBytes[0]); //discard the alpha channel for 24-bit, since it is unused
                    }

                    //store the pixel
                    pal.Pixels.Add(pixel);
                }
            }
            else
                throw new ApplicationException("Bitmap info header version 3 not availalbe. Unknown palette size.");

            return pal;
        }

        /// <summary>Performs RLE-4 decoding</summary>
        /// <param name="data">RLE data to decode</param>
        /// <returns>RLE-decoded data, still at a 4bpp data resolution</returns>
        protected Byte[] DecodeRLE4(Byte[] data)
        {
            //create destination array
            Int32 rowWidth = this.GetBytePackedWidth(4, this.BitmapInfo.Width);
            Byte[] decoded = new Byte[rowWidth * this.BitmapInfo.Height];

            //current decoded pixel offsets
            Int32 x = 0, y = 0;

            //reads two bytes at a time from the input data.
            for (Int32 index = 0; index < data.Length; index += 2)
            {
                if (data[index] == 0)   //absolute mode
                {
                    Int32 colorIndeces = data[index + 1];

                    if (colorIndeces == 0)          //actually an escape sequence, next line
                    {
                        x = 0;
                        ++y;
                    }
                    else if (colorIndeces == 1)     //actually an escape sequence, end of file
                        break;
                    else if (colorIndeces == 2)     //actually an escape sequence, delta
                    {
                        x += data[index + 2];
                        y += data[index + 3];
                        index += 2;
                    }
                    else //read these absolute data indeces
                    {
                        for (Int32 absoluteIndex = 0; absoluteIndex < colorIndeces; ++absoluteIndex)
                        {
                            Byte temp = data[index + 2 + (absoluteIndex / 2)];
                            if (absoluteIndex % 2 == 0)
                            {
                                temp &= 0xF0;
                                temp >>= 4;
                            }
                            else
                                temp &= 0x0F;

                            this.Set4BppIndex(decoded, x, y, rowWidth, temp);
                            ++x;
                        }

                        //increment RLE reading index
                        index += (colorIndeces / 2) + (colorIndeces % 2 == 1 ? 1 : 0);
                    }
                }
                else    //encoded mode
                {
                    for (Int32 rleLoop = 0; rleLoop < data[index]; ++rleLoop)
                    {
                        Byte value = data[index + 1];

                        if (rleLoop % 2 == 0)
                        {
                            value &= 0xF0;
                            value >>= 4;
                        }
                        else
                            value &= 0x0F;

                        this.Set4BppIndex(decoded, x, y, rowWidth, value);
                        ++x;
                    }
                }
            }

            return decoded;
        }

        /// <summary>Sets a data index for 4 bpp</summary>
        /// <param name="data">Data to set</param>
        /// <param name="x">Horizontal position to set</param>
        /// <param name="y">Vertical position to set</param>
        /// <param name="rowWidth">Byte width of a row</param>
        /// <param name="value">Value to set (lower 4 bits)</param>
        protected void Set4BppIndex(Byte[] data, Int32 x, Int32 y, Int32 rowWidth, Byte value)
        {
            Int32 rowOffset = y * rowWidth;
            Int32 columnOffset = x / 2;

            //lower 4 bits of the value
            value &= 0x0F;

            //might need to set the upper bits
            if (x % 2 == 0)
                value <<= 4;

            data[rowOffset + columnOffset] |= value;
        }

        /// <summary>Performs RLE-8 decoding</summary>
        /// <param name="data">RLE data to decode</param>
        /// <returns>RLE-decoded data, still at an 8bpp data resolution</returns>
        protected Byte[] DecodeRLE8(Byte[] data)
        {
            //create destination array
            Int32 rowWidth = this.GetBytePackedWidth(8, this.BitmapInfo.Width);
            Byte[] decoded = new Byte[rowWidth * this.BitmapInfo.Height];

            //current decoded pixel offsets
            Int32 x = 0, y = 0;

            //reads two bytes at a time from the input data.
            for (Int32 index = 0; index < data.Length; index += 2)
            {
                if (data[index] == 0)   //absolute mode
                {
                    Int32 colorIndeces = data[index + 1];

                    if (colorIndeces == 0)          //actually an escape sequence, next line
                    {
                        x = 0;
                        ++y;
                    }
                    else if (colorIndeces == 1)     //actually an escape sequence, end of file
                        break;
                    else if (colorIndeces == 2)     //actually an escape sequence, delta
                    {
                        x += data[index + 2];
                        y += data[index + 3];
                        index += 2;
                    }
                    else //read these absolute data indeces
                    {
                        for (Int32 absoluteIndex = 0; absoluteIndex < colorIndeces; ++absoluteIndex)
                        {
                            Byte temp = data[index + 2 + absoluteIndex];
                            this.Set8BppIndex(decoded, x, y, rowWidth, temp);
                            ++x;
                        }

                        //increment RLE reading index
                        index += colorIndeces;
                    }
                }
                else    //encoded mode
                {
                    Byte value = data[index + 1];
                    for (Int32 rleLoop = 0; rleLoop < data[index]; ++rleLoop)
                    {
                        this.Set8BppIndex(decoded, x, y, rowWidth, value);
                        ++x;
                    }
                }
            }

            return decoded;
        }

        /// <summary>Sets a data index for 8 bpp</summary>
        /// <param name="data">Data to set</param>
        /// <param name="x">Horizontal position to set</param>
        /// <param name="y">Vertical position to set</param>
        /// <param name="rowWidth">Byte width of a row</param>
        /// <param name="value">Value to set (lower 4 bits)</param>
        protected void Set8BppIndex(Byte[] data, Int32 x, Int32 y, Int32 rowWidth, Byte value)
        {
            Int32 rowOffset = y * rowWidth;
            data[rowOffset + x] = value;
        }
        #endregion


        #region IO Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            this.Initialize();

            this.FileHeader.Read(input);
            this.BitmapInfo.Read(input);

            this.ReadData(input);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            this.FileHeader.Write(output);
            this.BitmapInfo.Write(output);
        }
        #endregion


        #region IImage methods
        /// <summary>Gets a frame image from the pixel data already in place</summary>
        /// <returns>A frame containing the pixel data</returns>
        public IMultimediaImageFrame GetFrame()
        {
            IMultimediaImageFrame frame = new BasicImageFrame(this.PixelData);
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
            return new BasicImage(ImageManipulation.GetSubImage(this.PixelData, x, y, width, height));
        }
        #endregion
    }
}