using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.Files.External.Image.JPEG.Enums;
using Bardez.Projects.ReusableCode;

using Bardez.Projects.InfinityPlus1.Files.Base;

namespace Bardez.Projects.InfinityPlus1.Files.External.Image.JPEG
{
    /// <summary>Represents the binary data stored within the JFIF APP0 opening data stream.</summary>
    public class JfifHeader : MarkerSegment
    {
        #region Fields
        /// <summary>Represents the identifier of the JFIF header</summary>
        /// <value>Should always be a NUL-terminated "JFIF", 5 bytes</value>
        public ZString Identifier { get; set; }

        /// <summary>Major JFIF version</summary>
        public Byte VersionMajor { get; set; }

        /// <summary>Minor JFIF version</summary>
        public Byte VersionMinor { get; set; }

        /// <summary>Units for pixel density fields</summary>
        public DensityUnit DensityUnits { get; set; }

        /// <summary>Integer horizontal pixel density</summary>
        public UInt16 DensityHorizontal { get; set; }

        /// <summary>Integer vertical  pixel density</summary>
        public UInt16 DensityVertical { get; set; }

        /// <summary>Horizontal size of embedded JFIF thumbnail in pixels</summary>
        public Byte ThumbnailWidth { get; set; }

        /// <summary>Vertical size of embedded JFIF thumbnail in pixels</summary>
        public Byte ThumbnailHeight { get; set; }

        /// <summary>Uncompressed 24 bit RGB raster thumbnail</summary>
        public Byte[] ThumbnailData { get; set; }
        #endregion


        #region Properties
        /// <summary>Gets the size of the JFIF APP0 data, including the size of the length member</summary>
        public Int32 Size
        {
            get { return Convert.ToUInt16(16 + (this.ThumbnailData == null ? 0 : this.ThumbnailData.Length)); }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Identifier = new ZString();
            this.ThumbnailData = null;
        }
        #endregion


        #region IO Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="size">Size of the application field to read (size counts as two bytes, so this field will be subtracted by 2 internally)</param>
        public virtual void Read(Stream input, Int32 size)
        {
            //adjust the size
            size -= 2;

            //instantiate ZString
            this.Initialize();

            Byte[] memberData = ReusableIO.BinaryRead(input, size);

            this.Identifier.Source = ReusableIO.ReadStringFromByteArray(memberData, 0, CultureConstants.CultureCodeEnglish, 5);
            this.VersionMajor = memberData[5];
            this.VersionMinor = memberData[6];
            this.DensityUnits = (DensityUnit)memberData[7];
            this.DensityHorizontal = ReusableIO.ReadUInt16FromArray(memberData, 8, Endianness.BigEndian);
            this.DensityVertical = ReusableIO.ReadUInt16FromArray(memberData, 10, Endianness.BigEndian);
            this.ThumbnailWidth = memberData[12];
            this.ThumbnailHeight = memberData[13];

            //conditionally instantiate and read the thumbnail data
            if (size > 14)
            {
                this.ThumbnailData = new Byte[size - 14];
                Array.Copy(memberData, 14, this.ThumbnailData, 0, size - 14);
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}