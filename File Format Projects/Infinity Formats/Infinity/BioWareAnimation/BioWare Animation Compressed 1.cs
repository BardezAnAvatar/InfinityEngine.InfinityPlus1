using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Zlib;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
//using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.ReusableCode;

using Ionic.Zlib;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareAnimation
{
    /// <summary>Represents a compressed wrapper for a BAM asset</summary>
    /// <remarks>Neither ZGipStream nor DeflateStream work straight out. Using a ZLIB implementation.</remarks>
    public class BioWareAnimationCompressed_v1 : IInfinityFormat, IImageSet, IAnimation
    {
        #region Fields
        /// <summary>Compressed header of the MOS file</summary>
        public ZlibHeader Header { get; set; }

        /// <summary>Decompressed MOS file asset</summary>
        public BioWareAnimation1 File { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.Header = new ZlibHeader();
            this.File = new BioWareAnimation1();
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


        #region IImageSet Method(s)
        /// <summary>Returns the frame with the associated index in the tileset</summary>
        /// <param name="index">Index of the frame to retrieve</param>
        /// <returns>The frane at the associated index</returns>
        public IMultimediaImageFrame GetFrame(Int32 index)
        {
            return this.File.GetFrame(index);
        }

        /// <summary>IImageSet property that exposes the frame count</summary>
        public Int64 FrameCount
        {
            get { return this.File.FrameEntries.Count; }
        }
        #endregion


        #region IAnimation Methods
        /// <summary>Returns an IList containing an IList of indeces meant to be used in conjunction with <see cref="IImageSet.GetFrame(Int32)"/></summary>
        /// <returns>An IList with items inside being IList of Int32 indeces to the frames returned from <see cref="IImageSet.GetFrame(Int32)"/> using the same index key</returns>
        public IList<IList<Int32>> GetFrameAnimations()
        {
            return this.File.GetFrameAnimations();
        }
        #endregion
    }
}