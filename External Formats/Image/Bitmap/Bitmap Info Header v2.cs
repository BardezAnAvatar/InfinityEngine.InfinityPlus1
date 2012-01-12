using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Bitmap.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Bitmap
{
    /// <summary>Contains core bitmap header information</summary>
    /// <remarks>Corresponds to the BITMAPCOREHEADER Win32 structure, in Wingdi.h</remarks>
    public class BitmapInfoHeader_v2
    {
        #region Static Fields
        /// <summary>Constant indicating the structure size on disk</summary>
        public const UInt32 CorrespondingStructSize = 12U;
        #endregion


        #region Properties
        /// <summary>The width of the bitmap, in pixels.</summary>
        public Int32 Width { get; set; }

        /// <summary>The height of the bitmap, in pixels.</summary>
        public Int32 Height { get; set; }

        /// <summary>The number of planes for the target device.</summary>
        /// <value>This version's value must be 1.</value>
        public UInt16 Planes { get; set; }

        /// <summary>The number of bits-per-pixel. This value must be 1, 4, 8, 24 or 32.</summary>
        public BitsPerPixel BitCount { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public BitmapInfoHeader_v2()
        {
            this.Planes = 1;
        }

        /// <summary>Definition constructor</summary>
        /// <param name="width">Width of this bitmap</param>
        /// <param name="height">Height of this bitmap</param>
        /// <param name="planes">Planes of the bitmap (layers?). Usually 1.</param>
        /// <param name="bitCount">The number of bits-per-pixel.</param>
        public BitmapInfoHeader_v2(UInt16 width, UInt16 height, UInt16 planes, BitsPerPixel bitCount)
        {
            this.Width = width;
            this.Height = height;
            this.Planes = planes;
            this.BitCount = bitCount;
        }
        #endregion


        #region Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            this.ReadHeightWidth(input);

            Byte[] data = ReusableIO.BinaryRead(input, 4);

            this.Planes = ReusableIO.ReadUInt16FromArray(data, 0);
            this.BitCount = (BitsPerPixel)ReusableIO.ReadUInt16FromArray(data, 2);
        }

        /// <summary>This public method reads file format's height and width from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        protected virtual void ReadHeightWidth(Stream input)
        {
            Byte[] data = ReusableIO.BinaryRead(input, 4);
            this.Width = ReusableIO.ReadUInt16FromArray(data, 0);
            this.Height = ReusableIO.ReadUInt16FromArray(data, 2);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteUInt16ToStream(this.Planes, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.BitCount, output);
        }

        /// <summary>This public method writes the file format's height and width to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        protected virtual void WriteHeightWidth(Stream output)
        {
            ReusableIO.WriteInt16ToStream(Convert.ToInt16(this.Width), output);
            ReusableIO.WriteInt16ToStream(Convert.ToInt16(this.Height), output);
        }
        #endregion
    }
}