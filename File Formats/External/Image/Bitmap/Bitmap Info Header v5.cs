using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.Files.External.Image.Bitmap.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.External.Image.Bitmap
{
    /// <summary>Contains core bitmap header information</summary>
    /// <remarks>Corresponds to the BITMAPV5HEADER Win32 structure, in Wingdi.h</remarks>
    public class BitmapInfoHeader_v5 : BitmapInfoHeader_v4
    {
        #region Static Fields
        /// <summary>Constant indicating the structure size on disk</summary>
        public new const UInt32 CorrespondingStructSize = 124U;
        #endregion


        #region Properties
        /// <summary>Rendering intent for bitmap.</summary>
        public RenderingIntent Intent { get; set; }

        /// <summary>The offset, in bytes, from the beginning of the BitmapInfoHeader_v5 structure to the start of the profile data.</summary>
        /// <remarks>
        ///     If the profile is embedded, profile data is the actual profile, and it is linked. (The profile data is the null-terminated file name of the profile.)
        ///     This cannot be a Unicode string. It must be composed exclusively of characters from the Windows character set (code page 1252).
        ///     These profile members are ignored unless the ColorSpace member specifies ProfileLinked or ProfileEMbedded.
        /// </remarks>
        public UInt32 ProfileDataOffset { get; set; }

        /// <summary>Size, in bytes, of embedded profile data.</summary>
        public UInt32 ProfileSize { get; set; }

        /// <summary>This member has been reserved.</summary>
        /// <value>Its value should be set to zero.</value>
        public UInt32 Reserved { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public BitmapInfoHeader_v5() : base() { }

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
        /// <param name="redMask">Color mask that specifies the red component of each pixel.</param>
        /// <param name="greenMask">Color mask that specifies the green component of each pixel.</param>
        /// <param name="blueMask">Color mask that specifies the blue component of each pixel.</param>
        /// <param name="alphaMask">Color mask that specifies the alpha component of each pixel.</param>
        /// <param name="colorSpace">The color space of the DIB.</param>
        /// <param name="endPoints">Specifies the x, y, and z coordinates of the three colors that correspond to the red, green, and blue endpoints for the logical color space associated with the bitmap.</param>
        /// <param name="gammaRed">Tone response curve for red.</param>
        /// <param name="gammaGreen">Tone response curve for green.</param>
        /// <param name="gammaBlue">Tone response curve for blue.</param>
        /// <param name="intent">Rendering intent for bitmap.</param>
        /// <param name="profileDataOffset">The offset, in bytes, from the beginning of the BitmapInfoHeader_v5 structure to the start of the profile data.</param>
        /// <param name="profileSize">Size, in bytes, of embedded profile data.</param>
        /// <param name="reserved">Value should be set to zero.</param>
        public BitmapInfoHeader_v5(UInt16 width, UInt16 height, UInt16 planes, BitsPerPixel bitCount,
            BitmapCompression compression, UInt32 imageByteSize, Int32 bitsPerMeterX, Int32 bitsPerMeterY, UInt32 colorsUsed, UInt32 colorsImportant,
            UInt32 redMask, UInt32 greenMask, UInt32 blueMask, UInt32 alphaMask, ColorSpace colorSpace, RgbCoordinateTriplet endPoints, UInt32 gammaRed, UInt32 gammaGreen, UInt32 gammaBlue,
            RenderingIntent intent, UInt32 profileDataOffset, UInt32 profileSize, UInt32 reserved)
            : base (width, height, planes, bitCount, compression, imageByteSize, bitsPerMeterX, bitsPerMeterY, colorsUsed, colorsImportant, redMask, greenMask, blueMask, alphaMask, colorSpace, endPoints, gammaRed, gammaGreen, gammaBlue)
        {
            this.Intent = intent;
            this.ProfileDataOffset = profileDataOffset;
            this.ProfileSize = profileSize;
            this.Reserved = reserved;
        }
        #endregion


        #region Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void Read(Stream input)
        {
            base.Read(input);

            Byte[] data = ReusableIO.BinaryRead(input, 16);

            this.Intent = (RenderingIntent)ReusableIO.ReadUInt32FromArray(data, 0);
            this.ProfileDataOffset = ReusableIO.ReadUInt32FromArray(data, 4);
            this.ProfileSize = ReusableIO.ReadUInt32FromArray(data, 8);
            this.Reserved = ReusableIO.ReadUInt32FromArray(data, 12);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            base.Write(output);

            ReusableIO.WriteUInt32ToStream((UInt32)this.Intent, output);
            ReusableIO.WriteUInt32ToStream(this.ProfileDataOffset, output);
            ReusableIO.WriteUInt32ToStream(this.ProfileSize, output);
            ReusableIO.WriteUInt32ToStream(this.Reserved, output);
        }
        #endregion
    }
}