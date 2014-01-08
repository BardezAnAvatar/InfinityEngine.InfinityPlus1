using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Mathematics.DXT;
using Bardez.Projects.InfinityPlus1.FileFormats.External.ImaginationTechnologies.PowerVR.Component;
using Bardez.Projects.InfinityPlus1.FileFormats.External.ImaginationTechnologies.PowerVR.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.ImaginationTechnologies.PowerVR
{
    /// <summary>A class containing a Power VR file</summary>
    public class PowerVrImage : IImage
    {
        #region Fields
        /// <summary>File Header</summary>
        public PvrHeader Header { get; set; }

        /// <summary>Collection of metadata</summary>
        public List<MetaData> MetaData { get; set; }

        /// <summary>Actual pixel data</summary>
        public Byte[] Data { get; set; }
        #endregion


        #region Properties
        /// <summary>Packed height of the pixel data</summary>
        protected UInt32 DataHeight
        {
            get
            {
                UInt32 height = this.Header.Height;
                if (height % 4 > 0)
                    height = height / 4 + 4;

                return height;
            }

        }
        /// <summary>Packed width of the pixel data</summary>
        protected UInt32 DataWidth
        {
            get
            {
                UInt32 width = this.Header.Width;
                if (width % 4 > 0)
                    width = width / 4 + 4;

                return width;
            }

        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.Header = new PvrHeader();
            this.MetaData = new List<MetaData>();
        }
        #endregion


        #region IO
        /// <summary>Read method, reads the header from the input stream</summary>
        /// <param name="input">Stream to read the header from</param>
        public void Read(Stream input)
        {
            this.Initialize();

            //header
            this.Header.Read(input);

            //meta data
            UInt32 length = 0;
            while (length < this.Header.MetaDataSize)
            {
                MetaData meta = new MetaData();
                meta.Read(input);
                this.MetaData.Add(meta);

                length += meta.Size + 12U;
            }

            //pixel data
            this.Data = ReusableIO.BinaryRead(input, input.Length - input.Position);
        }

        /// <summary>Write method, writes the header to the output stream</summary>
        /// <param name="output">Stream to write the header to</param>
        public void Write(Stream output)
        {
            this.Header.Write(output);

            foreach (MetaData meta in this.MetaData)
                meta.Write(output);

            output.Write(this.Data, 0, this.Data.Length);
        }
        #endregion


        #region IImage Methods
        /// <summary>Gets a frame image from the pixel data already in place</summary>
        /// <returns>A frame containing the pixel data</returns>
        public IMultimediaImageFrame GetFrame()
        {
            Byte[] data = this.GetPixelData();

            Int64 oX = 0;
            Int64 oY = 0;

            //note: the image is encoded in 4x4 two-byte blocks, so its padding needs to reflect this.
            PixelData pd = new PixelData(data, ScanLineOrder.TopDown, PixelFormat.RGBA_B8G8R8A8, Convert.ToInt32(this.Header.Height), Convert.ToInt32(this.Header.Width), 16, 16, 32, oX, oY);
            IMultimediaImageFrame getFrame = new BasicImageFrame(pd);

            return getFrame;
        }

        /// <summary>Gets a Byte array of the pixel data</summary>
        /// <returns>A Byte array of the pixel data</returns>
        protected Byte[] GetPixelData()
        {
            Byte[] data = null;

            if (this.Header == null)
                throw new NullReferenceException("The file header was undexpectedly null.");

            switch (this.Header.PixelFormat)
            {
                case PvrPixelFormat.DXT1:
                    data = this.GetDecodedPixelData(DXT1.DecodePixels);
                    break;

                case PvrPixelFormat.DXT5:
                    data = this.GetDecodedPixelData(DXT5.DecodePixels);
                    break;

                default:
                    throw new NotSupportedException("The only PowerVR implementation currently supported is DXT1.");
            }

            return data;
        }

        /// <summary>Decodes pixel data with the provided decoding function</summary>
        /// <param name="decoder">Decoding method to use</param>
        /// <returns>The decoded pixel data</returns>
        protected Byte[] GetDecodedPixelData(Func<Byte[], Byte[]> decoder)
        {
            //write the decoded data
            Byte[] data = new Byte[this.DataHeight * this.DataWidth * 4];   //decoded data is RGB32, which is 4 Bytes per pixel
            Int32 rowIndex = 0, columnIndex = 0;

            //Note about the compression. It is not run-length encoded, but square-encoded (like JPEG), so data will have to be re-ordered.
            for (Int32 index = 0; index < this.Data.Length; index += 8)
            {
                //source data
                Byte[] pixelData = new Byte[8];
                Array.Copy(this.Data, index, pixelData, 0, 8);

                //decoded data, 32-bit RGBA
                Byte[] decoded = DXT1.DecodePixels(pixelData);

                //write each 4 pixels where they belong
                for (Int32 y = 0; y < 4; ++y)
                {
                    Int64 tempIndex = (((rowIndex + y) * this.DataWidth) + (columnIndex)) * 4;
                    Array.Copy(decoded, (y * 16 /* 4 pixels * 4 bytes */), data, tempIndex, 16 /* 4 pixels * 4 bytes */);
                }

                //update indecies
                columnIndex += 4;
                if (columnIndex == this.DataWidth)
                {
                    rowIndex += 4;
                    columnIndex = 0;
                }
            }

            return data;
        }
        #endregion
    }
}