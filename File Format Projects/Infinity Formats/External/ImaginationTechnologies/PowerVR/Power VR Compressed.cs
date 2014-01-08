using System;
using System.IO;

using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.ReusableCode;
using Ionic.Zlib;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.ImaginationTechnologies.PowerVR
{
    /// <summary>A class containing a zlib-compressed Power VR file</summary>
    public class PowerVrCompressed : IImage
    {
        #region Fields
        /// <summary>Uncompressed size</summary>
        public UInt32 UncompressedSize { get; set; }

        /// <summary>The compressed file</summary>
        public PowerVrImage File { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.File = new PowerVrImage();
        }
        #endregion


        #region IO
        /// <summary>Read method, reads the header from the input stream</summary>
        /// <param name="input">Stream to read the header from</param>
        public void Read(Stream input)
        {
            this.Initialize();

            this.UncompressedSize = ReusableIO.ReadUInt32FromStream(input);

            //read the data from the file into a byte array and decompress
            Byte[] compressed = ReusableIO.BinaryRead(input, input.Length - input.Position);
            Byte[] decompressed = ZlibStream.UncompressBuffer(compressed);

            MemoryStream wrapper = new MemoryStream(decompressed);
            this.File.Read(wrapper);
        }

        /// <summary>Write method, writes the header to the output stream</summary>
        /// <param name="output">Stream to write the header to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteUInt32ToStream(this.UncompressedSize, output);

            Byte[] compressed = null;

            using (MemoryStream memory = new MemoryStream())
            {
                this.File.Write(memory);
                compressed = ZlibStream.CompressBuffer(memory.GetBuffer());
            }

            output.Write(compressed, 0, compressed.Length);
        }
        #endregion


        #region IImage Methods
        /// <summary>Gets a frame image from the pixel data already in place</summary>
        /// <returns>A frame containing the pixel data</returns>
        public IMultimediaImageFrame GetFrame()
        {
            return this.File.GetFrame();
        }
        #endregion
    }
}