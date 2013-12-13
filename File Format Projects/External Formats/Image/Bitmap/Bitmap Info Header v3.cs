using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Bitmap.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Bitmap
{
    /// <summary>Contains core bitmap header information</summary>
    /// <remarks>Corresponds to the BITMAPINFOHEADER Win32 structure, in Wingdi.h</remarks>
    public class BitmapInfoHeader_v3 : BitmapInfoHeader_v2
    {
        #region Static Fields
        /// <summary>Constant indicating the structure size on disk</summary>
        public new const UInt32 CorrespondingStructSize = 40U;
        #endregion


        #region Properties
        /// <summary>The number of bytes required by the structure.</summary>
        public BitmapCompression Compression { get; set; }

        /// <summary>The size, in bytes, of the image. This may be set to zero for BI_RGB bitmaps.</summary>
        public UInt32 SizeImage { get; set; }

        /// <summary>
        ///     The horizontal resolution, in pixels-per-meter, of the target device for the bitmap.
        ///     An application can use this value to select a bitmap from a resource
        ///     group that best matches the characteristics of the current device.
        /// </summary>
        public Int32 PixelsPerMeterX { get; set; }

        /// <summary>
        ///     The vertical resolution, in pixels-per-meter, of the target device for the bitmap.
        /// </summary>
        public Int32 PixelsPerMeterY { get; set; }

        /// <summary>
        ///     The number of color indexes in the color table that are actually used by the bitmap.
        ///     If this value is zero, the bitmap uses the maximum number of colors corresponding to the value of the BitCount member for the compression mode specified by Compression.
        /// </summary>
        /// <value>
        ///     If ColorsUsed is nonzero and the BitCount member is less than 16, the ColorsUsed member specifies the actual number of colors the graphics engine or device driver accesses.
        ///     If BitCount is 16 or greater, the ColorsUsed member specifies the size of the color table used to optimize performance of the system color palettes.
        ///     If BitCount equals 16 or 32, the optimal color palette starts immediately following the three DWORD masks.
        /// </value>
        /// <remarks>
        ///     When the bitmap array immediately follows the BITMAPINFO structure, it is a packed bitmap. Packed bitmaps are referenced by a single pointer.
        ///     Packed bitmaps require that the ColorsUsed member must be either zero or the actual size of the color table.
        /// </remarks>
        public UInt32 ColorsUsed { get; set; }

        /// <summary>The number of color indexes that are required for displaying the bitmap.</summary>
        /// <value>If this value is zero, all colors are required.</value>
        public UInt32 ColorsImportant { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public BitmapInfoHeader_v3() : base() { }

        /// <summary>Definition constructor</summary>
        /// <param name="width">Width of this bitmap</param>
        /// <param name="height">Height of this bitmap</param>
        /// <param name="planes">Planes of the bitmap (layers?). Usually 1.</param>
        /// <param name="bitCount">The number of bits-per-pixel.</param>
        /// <param name="compression">The type of compression for a compressed bottom-up bitmap.</param>
        /// <param name="imageByteSize">The size, in bytes, of the image.</param>
        /// <param name="bitsPerMeterX">The horizontal resolution, in pixels-per-meter, of the target device for the bitmap.</param>
        /// <param name="bitsPerMeterY">The vertical resolution, in pixels-per-meter, of the target device for the bitmap.</param>
        /// <param name="colorsUsed">The number of color indexes in the color table that are actually used by the bitmap.</param>
        /// <param name="colorsImportant">The number of color indexes that are required for displaying the bitmap.</param>
        public BitmapInfoHeader_v3(UInt16 width, UInt16 height, UInt16 planes, BitsPerPixel bitCount,
            BitmapCompression compression, UInt32 imageByteSize, Int32 bitsPerMeterX, Int32 bitsPerMeterY, UInt32 colorsUsed, UInt32 colorsImportant)
            : base (width, height, planes, bitCount)
        {
            this.Compression = compression;
            this.SizeImage = imageByteSize;
            this.PixelsPerMeterX = bitsPerMeterX;
            this.PixelsPerMeterY = bitsPerMeterY;
            this.ColorsUsed = colorsUsed;
            this.ColorsImportant = colorsImportant;
        }
        #endregion


        #region Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void Read(Stream input)
        {
            base.Read(input);

            Byte[] data = ReusableIO.BinaryRead(input, 24);

            this.Compression = (BitmapCompression)ReusableIO.ReadInt32FromArray(data, 0);
            this.SizeImage = ReusableIO.ReadUInt32FromArray(data, 4);
            this.PixelsPerMeterX = ReusableIO.ReadInt32FromArray(data, 8);
            this.PixelsPerMeterY = ReusableIO.ReadInt32FromArray(data, 12);
            this.ColorsUsed = ReusableIO.ReadUInt32FromArray(data, 16);
            this.ColorsImportant = ReusableIO.ReadUInt32FromArray(data, 20);
        }

        /// <summary>This public method reads file format's height and width from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        protected override void ReadHeightWidth(Stream input)
        {
            Byte[] data = ReusableIO.BinaryRead(input, 8);
            this.Width = ReusableIO.ReadInt32FromArray(data, 0);
            this.Height = ReusableIO.ReadInt32FromArray(data, 4);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            base.Write(output);

            ReusableIO.WriteInt32ToStream((Int32)this.Compression, output);
            ReusableIO.WriteUInt32ToStream(this.SizeImage, output);
            ReusableIO.WriteInt32ToStream(this.PixelsPerMeterX, output);
            ReusableIO.WriteInt32ToStream(this.PixelsPerMeterY, output);
            ReusableIO.WriteUInt32ToStream(this.ColorsUsed, output);
            ReusableIO.WriteUInt32ToStream(this.ColorsImportant, output);
        }

        /// <summary>This public method writes the file format's height and width to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        protected override void WriteHeightWidth(Stream output)
        {
            ReusableIO.WriteInt32ToStream(this.Width, output);
            ReusableIO.WriteInt32ToStream(this.Height, output);
        }
        #endregion
    }
}