using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Bitmap.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Bitmap
{
    /// <summary>Wrapper class that contains the size of the Info Header and a reference to the base type</summary>
    public class BitmapInfoWrapper
    {
        #region Properties
        /// <summary>The number of bytes required by the structure.</summary>
        public UInt32 Size { get; set; }

        /// <summary>The actual Bitmap Info Header instance</summary>
        public BitmapInfoHeader_v2 Header { get; set; }

        /// <summary>Exposes the Bitmap Info Header v3</summary>
        public BitmapInfoHeader_v3 Header3
        {
            get { return this.Header as BitmapInfoHeader_v3; }
            set { this.Header = value; }
        }

        /// <summary>Exposes the Bitmap Info Header v4</summary>
        public BitmapInfoHeader_v4 Header4
        {
            get { return this.Header as BitmapInfoHeader_v4; }
            set { this.Header = value; }
        }

        /// <summary>Exposes the Bitmap Info Header v5</summary>
        public BitmapInfoHeader_v5 Header5
        {
            get { return this.Header as BitmapInfoHeader_v5; }
            set { this.Header = value; }
        }

        #region InfoHeader exposure, to avoid a double .InfoHeader call
        /// <summary>The width of the bitmap, in pixels.</summary>
        public Int32 Width
        {
            get { return this.Header.Width; }
            set { this.Header.Width = value; }
        }

        /// <summary>The height of the bitmap, in pixels.</summary>
        public Int32 Height
        {
            get { return this.Header.Height; }
            set { this.Header.Height = value; }
        }

        /// <summary>The number of planes for the target device.</summary>
        /// <value>This version's value must be 1.</value>
        public UInt16 Planes
        {
            get { return this.Header.Planes; }
            set { this.Header.Planes = value; }
        }

        /// <summary>The number of bits-per-pixel. This value must be 1, 4, 8, 24 or 32.</summary>
        public BitsPerPixel BitCount
        {
            get { return this.Header.BitCount; }
            set { this.Header.BitCount = value; }
        }
        #endregion
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public BitmapInfoWrapper() { }

        /// <summary>Defintion constructor</summary>
        public BitmapInfoWrapper(UInt32 size, BitmapInfoHeader_v2 infoHeader)
        {
            this.Size = size;
            this.Header = Header;
        }

        /// <summary>Defintion constructor</summary>
        public BitmapInfoWrapper(UInt32 size)
        {
            this.Size = size;
        }
        #endregion


        #region Methods
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            switch (this.Size)
            {
                case BitmapInfoHeader_v2.CorrespondingStructSize:
                    this.Header = new BitmapInfoHeader_v2();
                    break;
                case BitmapInfoHeader_v3.CorrespondingStructSize:
                    this.Header = new BitmapInfoHeader_v3();
                    break;
                case BitmapInfoHeader_v4.CorrespondingStructSize:
                    this.Header = new BitmapInfoHeader_v4();
                    break;
                case BitmapInfoHeader_v5.CorrespondingStructSize:
                    this.Header = new BitmapInfoHeader_v5();
                    break;
            }
        }

        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            Byte[] data = ReusableIO.BinaryRead(input, 4);
            this.Size = ReusableIO.ReadUInt32FromArray(data, 0);

            this.Initialize();

            this.Header.Read(input);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteUInt32ToStream(this.Size, output);
            this.Header.Write(output);
        }
        #endregion
    }
}