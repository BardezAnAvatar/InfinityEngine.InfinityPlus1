using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.ImaginationTechnologies.PowerVR;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.BamMos;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Mosaic
{
    /// <summary>Represents a MOS v2 asset, which references external PVRZ files for its image blocks</summary>
    public class Mosaic_v2 : IInfinityFormat, IImage
    {
        #region Fields
        /// <summary>Header of this MOS file</summary>
        public MosaicHeader_v2 Header { get; set; }

        /// <summary>Collection of the mosaic file's data blocks</summary>
        public List<DataBlock> DataBlocks { get; set ;}

        /// <summary>Collection of related files to this Mosaic, keyed by the data blocks' RelatedFileName</summary>
        public Dictionary<String, PowerVrCompressed> RelatedFiles { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.Header = new MosaicHeader_v2();
            this.DataBlocks = new List<DataBlock>();
            this.RelatedFiles = null;
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void Read(Stream input)
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
                this.Header = new MosaicHeader_v2();
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

            //read data blocks
            ReusableIO.SeekIfAble(input, this.Header.BlockOffset);
            for (Int32 index = 0; index < this.Header.BlockCount; ++index)
            {
                DataBlock block = new DataBlock();
                block.Read(input);
                this.DataBlocks.Add(block);
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            this.Header.Write(output);

            ReusableIO.SeekIfAble(output, this.Header.BlockOffset);
            foreach (DataBlock block in this.DataBlocks)
                block.Write(output);
        }
        #endregion


        #region IImage methods
        /// <summary>Gets a frame image from the pixel data already in place</summary>
        /// <returns>A frame containing the pixel data</returns>
        public IMultimediaImageFrame GetFrame()
        {
            IMultimediaImageFrame frame = null;

            if (this.RelatedFiles != null)
                ;// frame = this.RelatedFiles.GetFrame();

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
            return new BasicImage(ImageManipulation.GetSubImage(this.GetFrame(), x, y, width, height));
        }
        #endregion
    }
}