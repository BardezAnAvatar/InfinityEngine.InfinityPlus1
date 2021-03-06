using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.BasicStructures.Math;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels.Enums;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.ReusableCode;
using Bardez.Projects.Utility;

namespace Bardez.Projects.Multimedia.MediaBase.Data.Pixels
{
    /// <summary>Represents a single-source for an image's pixel data. This class will record the input type, and output pixel data as requested, converting as necessary</summary>
    public class PixelData
    {
        #region Fields
        /// <summary>Exposes the group of metadata</summary>
        public ImageMetadata Metadata { get; set; }

        /// <summary>Represents the binary array of pixel data read from source.</summary>
        /// <value>The data must be either palette-indexed or fully decoded to a multiple of 8 bits. Y'UV 4:2:2 would not be acceptable, not Y'V12, but Y'UV 4:4:4 would be.</value>
        private MemoryStream nativeBinaryData;
        #endregion


        #region Properties
        /// <summary>Represents the binary array of pixel data read from source.</summary>
        /// <value>The data must be either palette-indexed or fully decoded to a multiple of 8 bits. Y'UV 4:2:2 would not be acceptable, not Y'V12, but Y'UV 4:4:4 would be.</value>
        public MemoryStream NativeBinaryData
        {
            get { return this.nativeBinaryData; }
            set
            {
                if (this.nativeBinaryData != null)
                    this.nativeBinaryData.Dispose();

                this.nativeBinaryData = value;
            }
        }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="data">MemoryStream containing pixel data</param>
        /// <param name="metadata">Metadata for the pixels, declaring form, shape and other parameters</param>
        public PixelData(MemoryStream data, ImageMetadata metadata)
        {
            this.nativeBinaryData = data;
            this.Metadata = metadata;
        }

        /// <summary>Definition constructor</summary>
        /// <param name="binary">Byte data of the pixel data</param>
        /// <param name="metadata">Metadata for the pixels, declaring form, shape and other parameters</param>
        public PixelData(Byte[] binary, ImageMetadata metadata)
            : this(new MemoryStream(binary), metadata) { }

        /// <summary>Definition constructor, assumes no palette</summary>
        /// <param name="binary">Byte data of the pixel data</param>
        /// <param name="order">Scan line order of the data</param>
        /// <param name="format">Pixel format of the data</param>
        /// <param name="palette">Data palette of the data</param>
        /// <param name="height">Height of the data</param>
        /// <param name="width">Width of the data</param>
        /// <param name="rowPacking">Row packing of the data</param>
        /// <param name="heightPacking">Height packing of the data</param>
        /// <param name="bitsPerStreamPixel">The number of bits per pixel in the binary data stream. If paletted, use index size.</param>
        /// <param name="originX">Horizontal origin of the frame</param>
        /// <param name="originY">Vertical origin of the frame</param>
        public PixelData(MemoryStream binary, ScanLineOrder order, PixelFormat format, Palette palette, Int32 height, Int32 width, Int32 rowPacking, Int32 heightPacking, Int32 bitsPerStreamPixel, Int64 originX, Int64 originY)
            : this(binary, new ImageMetadata(height, width, rowPacking, heightPacking, bitsPerStreamPixel, originX, originY, format, order, Rational.One, palette)) { }

        /// <summary>Definition constructor, assumes no palette</summary>
        /// <param name="binary">Byte data of the pixel data</param>
        /// <param name="order">Scan line order of the data</param>
        /// <param name="format">Pixel format of the data</param>
        /// <param name="palette">Data palette of the data</param>
        /// <param name="height">Height of the data</param>
        /// <param name="width">Width of the data</param>
        /// <param name="rowPacking">Row packing of the data</param>
        /// <param name="heightPacking">Height packing of the data</param>
        /// <param name="bitsPerStreamPixel">The number of bits per pixel in the binary data stream. If paletted, use index size.</param>
        /// <param name="originX">Horizontal origin of the frame</param>
        /// <param name="originY">Vertical origin of the frame</param>
        public PixelData(Byte[] binary, ScanLineOrder order, PixelFormat format, Palette palette, Int32 height, Int32 width, Int32 rowPacking, Int32 heightPacking, Int32 bitsPerStreamPixel, Int64 originX, Int64 originY)
            : this (new MemoryStream(binary), order, format, palette, height, width, rowPacking, heightPacking, bitsPerStreamPixel, 0L, 0L) { }

        /// <summary>Definition constructor, assumes no palette</summary>
        /// <param name="binary">Byte data of the pixel data</param>
        /// <param name="order">Scan line order of the data</param>
        /// <param name="format">Pixel format of the data</param>
        /// <param name="palette">Data palette of the data</param>
        /// <param name="height">Height of the data</param>
        /// <param name="width">Width of the data</param>
        /// <param name="rowPacking">Row packing of the data</param>
        /// <param name="heightPacking">Height packing of the data</param>
        /// <param name="bitsPerStreamPixel">The number of bits per pixel in the binary data stream. If paletted, use index size.</param>
        /// <param name="originX">Horizontal origin of the frame</param>
        /// <param name="originY">Vertical origin of the frame</param>
        public PixelData(Byte[] binary, ScanLineOrder order, PixelFormat format, Palette palette, Int32 height, Int32 width, Int32 rowPacking, Int32 heightPacking, Int32 bitsPerStreamPixel)
            : this(binary, order, format, palette, height, width, rowPacking, heightPacking, bitsPerStreamPixel, 0L, 0L) { }

        /// <summary>Definition constructor, assumes no palette</summary>
        /// <param name="binary">Byte data of the pixel data</param>
        /// <param name="order">Scan line order of the data</param>
        /// <param name="format">Pixel format of the data</param>
        /// <param name="height">Height of the data</param>
        /// <param name="width">Width of the data</param>
        /// <param name="rowPacking">Row packing of the data</param>
        /// <param name="heightPacking">Height packing of the data</param>
        /// <param name="bitsPerStreamPixel">The number of bits per pixel in the binary data stream</param>
        public PixelData(Byte[] binary, ScanLineOrder order, PixelFormat format, Int32 height, Int32 width, Int32 rowPacking, Int32 heightPacking, Int32 bitsPerStreamPixel, Int64 originX, Int64 originY)
            : this(binary, order, format, null, height, width, rowPacking, heightPacking, bitsPerStreamPixel, originX, originY) { }

        /// <summary>Definition constructor, assumes no palette</summary>
        /// <param name="binary">Byte data of the pixel data</param>
        /// <param name="order">Scan line order of the data</param>
        /// <param name="format">Pixel format of the data</param>
        /// <param name="height">Height of the data</param>
        /// <param name="width">Width of the data</param>
        /// <param name="rowPacking">Row packing of the data</param>
        /// <param name="heightPacking">Height packing of the data</param>
        /// <param name="bitsPerStreamPixel">The number of bits per pixel in the binary data stream</param>
        public PixelData(Byte[] binary, ScanLineOrder order, PixelFormat format, Int32 height, Int32 width, Int32 rowPacking, Int32 heightPacking, Int32 bitsPerStreamPixel)
            : this(binary, order, format, height, width, rowPacking, heightPacking, bitsPerStreamPixel, 0L, 0L) { }
        #endregion


        #region Methods
        #region DecodePalette methods
        /// <summary>Decodes the palette data to a decoded binary data Byte array</summary>
        /// <returns>The palette-decoded pixel data, aligned to the current row order</returns>
        protected MemoryStream DecodePaletteData()
        {
            MemoryStream bitmapData = null;

            if (this.Metadata.DataPalette != null)
            {
                Int32 rowSize = this.Metadata.RowDataSize;

                bitmapData = new MemoryStream(rowSize * this.Metadata.RowCount);

                //now, interpret the pixel data
                for (Int32 row = 0; row < this.Metadata.Height; ++row)
                {
                    Byte[] interim = this.DecodePaletteDataRow(row);  //read a row of pixels
                    bitmapData.WriteAtOffset(row * rowSize, interim);
                }
            }

            return bitmapData;
        }

        /// <summary>Decodes a row of palette data to a decoded binary data Byte array</summary>
        /// <param name="row">Row number to read</param>
        /// <returns>The palette-decoded pixel data row</returns>
        /// <remarks>The decoded data will share the same packing as the encoded; if a row is packed to 4 bytes, the decoded data is also. Same with vertical resolution.</remarks>
        protected Byte[] DecodePaletteDataRow(Int32 row)
        {
            //get starting position
            Byte[] decompressed = new Byte[PixelCalculations.PackedRowByteWidth(this.Metadata.DataPalette.BitsPerPixel, this.Metadata.HorizontalPacking, this.Metadata.Width)];
            Int32 location = row * PixelCalculations.PackedRowByteWidth(this.Metadata.BitsPerDataPixel, this.Metadata.HorizontalPacking, this.Metadata.Width);

            //decompress data (yay!)
            //read the packed data, 1 value at a time, and write the palette entry to the decompressed stream.
            //the only valid palettes I know are 8 bit or less, being 1, 2, 4, 8. I'm going to only implement those, for now.

            if (this.Metadata.BitsPerDataPixel == 8)    //optimized method
                this.DecodePaletteDataRow8Bit(decompressed, location);
            else
                this.DecodePaletteDataRow2nBit(decompressed, location);

            return decompressed;
        }

        /// <summary>Decodes a row of a generic 2^n-bit palette data to a decoded binary data Byte array</summary>
        /// <param name="decompressed">Decompressed Byte array to write to</param>
        /// <param name="location">Location in source stream to start from</param>
        protected void DecodePaletteDataRow2nBit(Byte[] decompressed, Int32 location)
        {
            //Metadata fetch
            Int32 width = this.Metadata.Width;
            Int32 bpp = this.Metadata.BitsPerDataPixel;

            Int32 xByte = 0, xBits = 0;

            //stored references to speed up code
            IList<Byte[]> pixBinData = this.Metadata.DataPalette.PixelData;
            MemoryStream nativeData = this.NativeBinaryData;

            for (Int32 x = 0; x < width; ++x)
            {
                Byte value = nativeData.ReadByteAtOffset(location + xByte);   //get the current byte, assign it to a shiftable variable
                xBits += bpp;

                //wrap as needed
                if (xBits > 7)
                    ++xByte;

                if (xBits > 8)
                    xBits -= 8;

                //Bit shifting.
                Int32 xBitsMinusData = xBits - bpp;
                Byte mask = (Byte)((0xFF >> (8 - xBits)) << xBitsMinusData);
                Byte index = (Byte)((value &= mask) >> xBitsMinusData);

                //now we need the pixel.
                Byte[] data = pixBinData[index];
                Int32 len = data.Length;

                Int32 position = len * x;

                //Array.Copy is slow here...
                for (Int32 i = 0; i < len; ++i)
                    decompressed[position + i] = data[i];
            }
        }

        /// <summary>Decodes a row of 8-bit palette data to a decoded binary data Byte array</summary>
        /// <param name="decompressed">Decompressed Byte array to write to</param>
        /// <param name="location">Location in source stream to start from</param>
        protected virtual void DecodePaletteDataRow8Bit(Byte[] decompressed, Int32 sourceLocation)
        {
            //stored references to speed up code
            IList<Byte[]> pixBinData = this.Metadata.DataPalette.PixelData;
            MemoryStream nativeData = this.NativeBinaryData;
            Int32 destPosition = 0;
            Int32 dataLen = this.Metadata.DataPalette.BitsPerPixel / 8;  //might be a hack
            Int32 end = sourceLocation + this.Metadata.Width;

            for (; sourceLocation < end; ++sourceLocation)
            {
                Byte value = nativeData.ReadByteAtOffset(sourceLocation);   //get the current byte, assign it to a shiftable variable

                //now we need the pixel.
                Byte[] data = pixBinData[value];

                //Array.Copy is slow here...
                for (Int32 i = 0; i < dataLen; ++i)
                    decompressed[destPosition++] = data[i];
            }
        }
        #endregion

        /// <summary>Retrieves the pixel data in the specified format, in the specified scan line order</summary>
        /// <param name="pixelConverter">Interface used to convert the pixel data if necessary</param>
        /// <param name="format">Expected output format of the data</param>
        /// <param name="order">Expected scan line order of the output data</param>
        /// <param name="horizontalPacking">Horizontal packing of bytes for output</param>
        /// <param name="verticalPacking">Vertical packing of rows for output</param>
        /// <returns>Binary pixel data of the converted data</returns>
        public MemoryStream GetPixelData(IPixelConverter pixelConverter, PixelFormat format, ScanLineOrder order, Int32 horizontalPacking, Int32 verticalPacking)
        {
            MemoryStream dataStream = this.NativeBinaryData;

            //decode data from palette
            if (this.Metadata.DataPalette != null)
                dataStream = this.DecodePaletteData();

            //flip if necessary
            if (this.Metadata.Order != order)
                dataStream = pixelConverter.FlipVertical(dataStream, this.Metadata);    //vertically swap each pixel row

            //strip packing
            if (this.Metadata.HorizontalPacking != 0 || this.Metadata.VerticalPacking != 0)
                dataStream = pixelConverter.AdjustForPacking(dataStream, this.Metadata, 0, 0);     // it should know the existing packing for this instance, specify the new packing

            //convert as necessary
            if (format != this.Metadata.Format)
                dataStream = pixelConverter.ConvertData(dataStream, this.Metadata.Format, format, horizontalPacking, verticalPacking, this.Metadata.Width, this.Metadata.Height, this.Metadata.ExpandedBitsPerPixel);

            //widen data to the destination horizontal and vertical packing specified
            if (horizontalPacking != 0 || verticalPacking != 0)
            {
                ImageMetadata metadata = new ImageMetadata(this.Metadata.Height, this.Metadata.Width, 0, 0, this.Metadata.BitsPerDataPixel, this.Metadata.OriginX, this.Metadata.OriginY, this.Metadata.Format, this.Metadata.Order, this.Metadata.AspectRatio, null);
                dataStream = pixelConverter.AdjustForPacking(dataStream, metadata, horizontalPacking, verticalPacking);     // it should know the existing packing for this instance, specify the new packing
            }

            //in a return state
            return dataStream;
        }
        #endregion


        #region Cloning
        /// <summary>Performs a deep copy of the object and returns another, separate instance of it.</summary>
        public PixelData Clone()
        {
            Byte[] data = this.NativeBinaryData.ToArray();
            return new PixelData(data, this.Metadata.DeepClone());
        }
        #endregion


        #region Stream closing
        /// <summary>Assigns the Stream reference, closing the existing stream if it is not the existing </summary>
        /// <param name="oldData">Old stream's data</param>
        /// <param name="newData">New stream to assign</param>
        protected void AssignStreamAndConditionallyCloseSource(ref Stream oldData, Stream newData)
        {
            //original reference is neither null nor is it native data
            if (oldData != this.NativeBinaryData && oldData != null)
                oldData.Dispose();

            oldData = newData;
        }
        #endregion
    }
}