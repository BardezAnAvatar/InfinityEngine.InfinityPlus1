using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.External.ImaginationTechnologies.PowerVR.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.ImaginationTechnologies.PowerVR.Component
{
    /// <summary>Header of a PVR file</summary>
    public class PvrHeader
    {
        #region Constants
        /// <summary>The size on one variable on disk.</summary>
        public const Int32 StructSize = 52;

        /// <summary>The expected magic number indicating a correct version</summary>
        public static readonly UInt32 VersionMagicNumber = 0x50565203U;
        #endregion


        #region Fields
        /// <summary>Version of the PVR file</summary>
        public UInt32 Version { get; set; }

        /// <summary>Any flags set</summary>
        public HeaderFlags Flags { get; set; }

        /// <summary>Least significant bytes of pixel format</summary>
        public UInt32 PixelFormatA { get; set; }

        /// <summary>Most significant bytes of pixel format</summary>
        public UInt32 PixelFormatB { get; set; }

        /// <summary>Color space of the pixel data</summary>
        public ColorSpace ColorSpace { get; set; }

        /// <summary>Format of data channels</summary>
        public ChannelType ChannelType { get; set; }

        /// <summary>Height of the image</summary>
        public UInt32 Height { get; set; }

        /// <summary>Width of the image</summary>
        public UInt32 Width { get; set; }

        /// <summary>Depth of the image</summary>
        public UInt32 Depth { get; set; }

        /// <summary>Number of surfaces in this image</summary>
        /// <remarks>Used for texture arrays</remarks>
        public UInt32 SurfaceCount { get; set; }

        /// <summary>Number of faces for this image</summary>
        /// <remarks>Number of faces in a cube map</remarks>
        public UInt32 FaceCount { get; set; }

        /// <summary>number of MIP-Map levels present including the top level</summary>
        /// <remarks>A value of one means that only the top level texture exists</remarks>
        public UInt32 MipMapCount { get; set; }

        /// <summary>Total size in bytes of all metadata following the header</summary>
        public UInt32 MetaDataSize { get; set; }
        #endregion


        #region Properties
        /// <summary>Flag indicating whether the pixel format is enumerated or not</summary>
        public Boolean EnumeratedPixelFormat
        {
            get { return this.PixelFormatB == 0; }
        }

        /// <summary>Exposes an enumeration of the pixel format</summary>
        public PvrPixelFormat PixelFormat
        {
            get { return (PvrPixelFormat)this.PixelFormatA; }
            set { this.PixelFormatA = (UInt32)value; }
        }
        #endregion


        #region IO
        /// <summary>Read method, reads the header from the input stream</summary>
        /// <param name="input">Stream to read the header from</param>
        public void Read(Stream input)
        {
            Byte[] header = ReusableIO.BinaryRead(input, PvrHeader.StructSize);

            this.Version = ReusableIO.ReadUInt32FromArray(header, 0);
            this.Flags = (HeaderFlags)ReusableIO.ReadUInt32FromArray(header, 4);
            this.PixelFormatA = ReusableIO.ReadUInt32FromArray(header, 8);
            this.PixelFormatB = ReusableIO.ReadUInt32FromArray(header, 12);
            this.ColorSpace = (ColorSpace)ReusableIO.ReadUInt32FromArray(header, 16);
            this.ChannelType = (ChannelType)ReusableIO.ReadUInt32FromArray(header, 20);
            this.Height = ReusableIO.ReadUInt32FromArray(header, 24);
            this.Width = ReusableIO.ReadUInt32FromArray(header, 28);
            this.Depth = ReusableIO.ReadUInt32FromArray(header, 32);
            this.SurfaceCount = ReusableIO.ReadUInt32FromArray(header, 36);
            this.FaceCount = ReusableIO.ReadUInt32FromArray(header, 40);
            this.MipMapCount = ReusableIO.ReadUInt32FromArray(header, 44);
            this.MetaDataSize = ReusableIO.ReadUInt32FromArray(header, 48);
        }

        /// <summary>Write method, writes the header to the output stream</summary>
        /// <param name="output">Stream to write the header to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteUInt32ToStream(this.Version, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.Flags, output);
            ReusableIO.WriteUInt32ToStream(this.PixelFormatA, output);
            ReusableIO.WriteUInt32ToStream(this.PixelFormatB, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.ColorSpace, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.ChannelType, output);
            ReusableIO.WriteUInt32ToStream(this.Height, output);
            ReusableIO.WriteUInt32ToStream(this.Width, output);
            ReusableIO.WriteUInt32ToStream(this.Depth, output);
            ReusableIO.WriteUInt32ToStream(this.SurfaceCount, output);
            ReusableIO.WriteUInt32ToStream(this.FaceCount, output);
            ReusableIO.WriteUInt32ToStream(this.MipMapCount, output);
            ReusableIO.WriteUInt32ToStream(this.MetaDataSize, output);
        }
        #endregion
    }
}