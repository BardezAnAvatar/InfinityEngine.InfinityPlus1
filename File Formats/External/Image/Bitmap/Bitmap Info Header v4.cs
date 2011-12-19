using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.Files.External.Image.Bitmap.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.External.Image.Bitmap
{
    /// <summary>Contains core bitmap header information</summary>
    /// <remarks>Corresponds to the BITMAPV4HEADER Win32 structure, in Wingdi.h</remarks>
    public class BitmapInfoHeader_v4 : BitmapInfoHeader_v3
    {
        #region Static Fields
        /// <summary>Constant indicating the structure size on disk</summary>
        public new const UInt32 CorrespondingStructSize = 108U;
        #endregion


        #region Properties
        /// <summary>Color mask that specifies the red component of each pixel, valid only if Compression is set to BitFields.</summary>
        public UInt32 RedMask { get; set; }

        /// <summary>Color mask that specifies the green component of each pixel, valid only if Compression is set to BitFields.</summary>
        public UInt32 GreenMask { get; set; }

        /// <summary>Color mask that specifies the blue component of each pixel, valid only if Compression is set to BitFields.</summary>
        public UInt32 BlueMask { get; set; }

        /// <summary>Color mask that specifies the alpha component of each pixel.</summary>
        public UInt32 AlphaMask { get; set; }

        /// <summary>The color space of the DIB.</summary>
        public ColorSpace ColorSpace { get; set; }

        /// <summary>specifies the x, y, and z coordinates of the three colors that correspond to the red, green, and blue endpoints for the logical color space associated with the bitmap.</summary>
        /// <value>This member is ignored unless the <see cref="ColorSpace" /> member specifies CalibratedRgb</value>
        public RgbCoordinateTriplet EndPoints { get; set; }

        /// <summary>Tone response curve for red. This member is ignored unless color values are calibrated RGB values and ColorSace is set to CalibratedRgb. Specified in 16^16 format.</summary>
        public UInt32 GammaRed { get; set; }

        /// <summary>Tone response curve for green. Used if ColorSace is set to CalibratedRgb. Specified as 16^16 format.</summary>
        public UInt32 GammaGreen { get; set; }

        /// <summary>Tone response curve for blue. Used if ColorSace is set to CalibratedRgb. Specified as 16^16 format.</summary>
        public UInt32 GammaBlue { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public BitmapInfoHeader_v4() : base() { }

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
        public BitmapInfoHeader_v4(UInt16 width, UInt16 height, UInt16 planes, BitsPerPixel bitCount,
            BitmapCompression compression, UInt32 imageByteSize, Int32 bitsPerMeterX, Int32 bitsPerMeterY, UInt32 colorsUsed, UInt32 colorsImportant,
            UInt32 redMask, UInt32 greenMask, UInt32 blueMask, UInt32 alphaMask, ColorSpace colorSpace, RgbCoordinateTriplet endPoints, UInt32 gammaRed, UInt32 gammaGreen, UInt32 gammaBlue)
            : base (width, height, planes, bitCount, compression, imageByteSize, bitsPerMeterX, bitsPerMeterY, colorsUsed, colorsImportant)
        {
            this.RedMask = redMask;
            this.GreenMask = greenMask;
            this.BlueMask = blueMask;
            this.AlphaMask = alphaMask;
            this.ColorSpace = colorSpace;
            this.EndPoints = endPoints;
            this.GammaRed = gammaRed;
            this.GammaGreen = gammaGreen;
            this.GammaBlue = gammaBlue;
        }

        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.EndPoints = new RgbCoordinateTriplet();
        }
        #endregion


        #region Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void Read(Stream input)
        {
            base.Read(input);
            
            Byte[] data = ReusableIO.BinaryRead(input, 68);
            
            this.RedMask = ReusableIO.ReadUInt32FromArray(data, 0);
            this.GreenMask = ReusableIO.ReadUInt32FromArray(data, 4);
            this.BlueMask = ReusableIO.ReadUInt32FromArray(data, 8);
            this.AlphaMask = ReusableIO.ReadUInt32FromArray(data, 12);
            this.ColorSpace = (ColorSpace)ReusableIO.ReadInt32FromArray(data, 16);
            this.EndPoints.Red.X = ReusableIO.ReadInt32FromArray(data, 20);
            this.EndPoints.Red.Y = ReusableIO.ReadInt32FromArray(data, 24);
            this.EndPoints.Red.Z = ReusableIO.ReadInt32FromArray(data, 28);
            this.EndPoints.Green.X = ReusableIO.ReadInt32FromArray(data, 32);
            this.EndPoints.Green.Y = ReusableIO.ReadInt32FromArray(data, 36);
            this.EndPoints.Green.Z = ReusableIO.ReadInt32FromArray(data, 40);
            this.EndPoints.Blue.X = ReusableIO.ReadInt32FromArray(data, 44);
            this.EndPoints.Blue.Y = ReusableIO.ReadInt32FromArray(data, 48);
            this.EndPoints.Blue.Z = ReusableIO.ReadInt32FromArray(data, 52);
            this.GammaRed = ReusableIO.ReadUInt32FromArray(data, 56);
            this.GammaGreen = ReusableIO.ReadUInt32FromArray(data, 60);
            this.GammaBlue = ReusableIO.ReadUInt32FromArray(data, 64);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            base.Write(output);

            ReusableIO.WriteUInt32ToStream(this.RedMask, output);
            ReusableIO.WriteUInt32ToStream(this.GreenMask, output);
            ReusableIO.WriteUInt32ToStream(this.BlueMask, output);
            ReusableIO.WriteUInt32ToStream(this.AlphaMask, output);
            ReusableIO.WriteInt32ToStream((Int32)this.ColorSpace, output);
            ReusableIO.WriteInt32ToStream(EndPoints.Red.X, output);
            ReusableIO.WriteInt32ToStream(EndPoints.Red.Y, output);
            ReusableIO.WriteInt32ToStream(EndPoints.Red.Z, output);
            ReusableIO.WriteInt32ToStream(EndPoints.Green.X, output);
            ReusableIO.WriteInt32ToStream(EndPoints.Green.Y, output);
            ReusableIO.WriteInt32ToStream(EndPoints.Green.Z, output);
            ReusableIO.WriteInt32ToStream(EndPoints.Blue.X, output);
            ReusableIO.WriteInt32ToStream(EndPoints.Blue.Y, output);
            ReusableIO.WriteInt32ToStream(EndPoints.Blue.Z, output);
            ReusableIO.WriteUInt32ToStream(this.GammaRed, output);
            ReusableIO.WriteUInt32ToStream(this.GammaGreen, output);
            ReusableIO.WriteUInt32ToStream(this.GammaBlue, output);
        }
        #endregion
    }
}