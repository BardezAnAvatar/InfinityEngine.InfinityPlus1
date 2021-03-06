using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Zlib;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.ReusableCode;

using Ionic.Zlib;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Mosaic
{
    /// <summary>Represents a compressed wrapper for a MOS asset</summary>
    /// <remarks>
    ///     Neither ZGipStream nor DeflateStream work straight out. Using a ZLIB implementation.
    ///     Also, while I could implement RFCs 1950 and 1951, I just felt lazy and I chose not to.
    ///     It is a break from "do it all yourself", but, It would be so much redundant effort that
    ///     I would feel slightly shamed, and the license is liberal, and I tnd to write less efficient code than years-old open source.
    /// </remarks>
    public class MosaicCompressed_v1 : IInfinityFormat, IImage
    {
        #region Fields
        /// <summary>Compressed header of the MOS file</summary>
        public ZlibHeader Header { get; set; }

        /// <summary>Decompressed MOS file asset</summary>
        public Mosaic_v1 File { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.Header = new ZlibHeader();
            this.File = new Mosaic_v1();
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            if (fullRead)
                this.Read(input);
            else
            {
                this.Header = new ZlibHeader();
                this.Header.Read(input, false);
            }
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            //read the header
            this.Header.Read(input);

            //read the data from the file into a byte array and decompress
            Byte[] compressed = new Byte[input.Length - input.Position];
            input.Read(compressed, 0, compressed.Length);
            Byte[] decompressed = ZlibStream.UncompressBuffer(compressed);

            //read file
            using (MemoryStream ms = new MemoryStream(decompressed))
                this.File.Read(ms);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            this.Header.Write(output);

            using (MemoryStream ms = new MemoryStream())
            {
                this.File.Write(ms);            //write file to a buffer stream
                ReusableIO.SeekIfAble(ms, 0L);  //rewind
                Byte[] buffer = ms.GetBuffer();

                using (ZlibStream zlibs = new ZlibStream(output, CompressionMode.Compress, true))
                    zlibs.Write(buffer, 0, buffer.Length);
            }
        }
        #endregion


        #region IImage methods
        /// <summary>Gets a frame image from the pixel data already in place</summary>
        /// <returns>A frame containing the pixel data</returns>
        public IMultimediaImageFrame GetFrame()
        {
            IMultimediaImageFrame frame = null;

            if (this.File != null)
                frame = this.File.GetFrame();

            return frame;
        }

        /// <summary>Gets a sub-image of the current image</summary>
        /// <param name="x">Source X position</param>
        /// <param name="y">Source Y position</param>
        /// <param name="width">Width of sub-image</param>
        /// <param name="height">Height of sub-image</param>
        /// <returns>The requested sub-image</returns>
        public IImage GetSubImage(Int32 x, Int32 y, Int32 width, Int32 height)
        {
            return this.File.GetSubImage(x, y, width, height);
        }
        #endregion
    }
}