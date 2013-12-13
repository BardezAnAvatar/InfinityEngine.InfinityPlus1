using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG
{
    /// <summary>Represents the JPEG frame header, §B.2.2</summary>
    public class JpegFrameHeader : MarkerSegment
    {
        #region Fields
        /// <summary>Specifies the precision in bits for the samples of the components in the frame.</summary>
        /// <value>8 (bit), 12 (bit) or 2-16 (bit) for lossless</value>
        public Byte SamplePrecision { get; set; }

        /// <summary>Number of hoizontal lines *when fully decoded*</summary>
        public UInt16 Height { get; set; }

        /// <summary>Number of vertical lines *when fully decoded*</summary>
        public UInt16 Width { get; set; }

        /// <summary>Number of image components (colorspace channels, hierarchy overlays, etc.) in frame.</summary>
        public Byte ComponentNumbers { get; set; }

        /// <summary>Represents the component-specification parameters collection</summary>
        protected List<FrameComponentParameter> components;
        #endregion


        #region Properties
        /// <summary>Exposes the component-specification parameters collection</summary>
        public List<FrameComponentParameter> Components
        {
            get { return this.components; }
        }

        /// <summary>Gets the number of contiguous blocks in a horizontal line</summary>
        public Int32 ContiguousBlockCountHorizontal
        {
            get
            {
                //must have height fit into a vertical muliple of VerticalSamplingFactor.
                Int32 hBlocks = (this.Width / 8) + ((this.Width % 8) == 0 ? 0 : 1);

                if (hBlocks % this.MaxHorizontalSamplingFactor != 0)
                    hBlocks = ((hBlocks / this.MaxHorizontalSamplingFactor) + 1) * this.MaxHorizontalSamplingFactor;

                return hBlocks;
            }
        }

        /// <summary>Gets the number of contiguous blocks in a vertical line</summary>
        public Int32 ContiguousBlockCountVertical
        {
            get
            {
                //must have height fit into a vertical muliple of VerticalSamplingFactor.
                Int32 vBlocks = (this.Height / 8) + ((this.Height % 8) == 0 ? 0 : 1);

                if (vBlocks % this.MaxVerticalSamplingFactor != 0)
                    vBlocks = ((vBlocks / this.MaxVerticalSamplingFactor) + 1) * this.MaxVerticalSamplingFactor;

                return vBlocks;
            }
        }

        /// <summary>Gets the number of contiguous blocks in a frame</summary>
        public Int32 ContiguousBlockCount
        {
            get { return this.ContiguousBlockCountHorizontal * this.ContiguousBlockCountVertical; }
        }

        /// <summary>Gets the number of contiguous block samples in a horizontal line</summary>
        public Int32 ContiguousBlockWidth
        {
            get { return this.ContiguousBlockCountHorizontal * 8; }
        }

        /// <summary>Gets the number of contiguous block samples in a vertical line</summary>
        public Int32 ContiguousBlockHeight
        {
            get { return this.ContiguousBlockCountVertical * 8; }
        }

        /// <summary>Gets the maximum sampling factor density of the frame (horizontal sampling factor times vertical sampling factor)</summary>
        public Int32 MaxSamplingFactorDensity
        {
            get
            {
                Int32 density = 0;

                foreach (FrameComponentParameter component in this.components)
                    density = (density > (component.HorizontalSamplingFactor * component.VerticalSamplingFactor)) ? density : (component.HorizontalSamplingFactor * component.VerticalSamplingFactor);

                return density;
            }
        }

        /// <summary>Gets the vertical maximum sampling factor of the frame</summary>
        public Int32 MaxVerticalSamplingFactor
        {
            get
            {
                Int32 factor = 0;

                foreach (FrameComponentParameter component in this.components)
                    factor = factor < component.VerticalSamplingFactor ? component.VerticalSamplingFactor : factor;

                return factor;
            }
        }

        /// <summary>Gets the horizontal maximum sampling factor of the frame</summary>
        public Int32 MaxHorizontalSamplingFactor
        {
            get
            {
                Int32 factor = 0;

                foreach (FrameComponentParameter component in this.components)
                    factor = factor < component.HorizontalSamplingFactor ? component.HorizontalSamplingFactor : factor;

                return factor;
            }
        }

        /// <summary>Exposes whether the frame is indicated to be a Huffman coder type or not</summary>
        public Boolean UsesHuffmanCoding
        {
            get
            {
                Boolean isHuffman = false; //pessimism

                //look for Huffman coding marker
                switch (this.Marker)
                {
                    case JpegInterchangeMarkerConstants.StartOfFrameHuffmanBaselineDCT:
                    case JpegInterchangeMarkerConstants.StartOfFrameHuffmanDifferentialLossless:
                    case JpegInterchangeMarkerConstants.StartOfFrameHuffmanDifferentialProgressiveDCT:
                    case JpegInterchangeMarkerConstants.StartOfFrameHuffmanDifferentialSequentialDCT:
                    case JpegInterchangeMarkerConstants.StartOfFrameHuffmanExtendedSequentialDCT:
                    case JpegInterchangeMarkerConstants.StartOfFrameHuffmanLossless:
                    case JpegInterchangeMarkerConstants.StartOfFrameHuffmanProgressiveDCT:
                        isHuffman = true;
                        break;
                }

                return isHuffman;
            }
        }

        /// <summary>Exposes whether the frame is encoded using DCT encoding or not</summary>
        public Boolean UsesDCT
        {
            get
            {
                Boolean dct = false;

                switch (this.Marker)
                {
                    case JpegInterchangeMarkerConstants.StartOfFrameArithmeticDifferentialProgressiveDCT:
                    case JpegInterchangeMarkerConstants.StartOfFrameArithmeticDifferentialSequentialDCT:
                    case JpegInterchangeMarkerConstants.StartOfFrameArithmeticExtendedSequentialDCT:
                    case JpegInterchangeMarkerConstants.StartOfFrameArithmeticProgressiveDCT:
                    case JpegInterchangeMarkerConstants.StartOfFrameHuffmanBaselineDCT:
                    case JpegInterchangeMarkerConstants.StartOfFrameHuffmanDifferentialProgressiveDCT:
                    case JpegInterchangeMarkerConstants.StartOfFrameHuffmanDifferentialSequentialDCT:
                    case JpegInterchangeMarkerConstants.StartOfFrameHuffmanExtendedSequentialDCT:
                    case JpegInterchangeMarkerConstants.StartOfFrameHuffmanProgressiveDCT:
                        dct = true;
                        break;
                }

                return dct;
            }
        }

        /// <summary>Exposes whether the frame is encoded using DCT encoding or not</summary>
        public Boolean UsesProgressive
        {
            get
            {
                Boolean progressive = false;

                switch (this.Marker)
                {
                    case JpegInterchangeMarkerConstants.StartOfFrameArithmeticDifferentialProgressiveDCT:
                    case JpegInterchangeMarkerConstants.StartOfFrameArithmeticProgressiveDCT:
                    case JpegInterchangeMarkerConstants.StartOfFrameHuffmanDifferentialProgressiveDCT:
                    case JpegInterchangeMarkerConstants.StartOfFrameHuffmanProgressiveDCT:
                        progressive = true;
                        break;
                }

                return progressive;
            }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public JpegFrameHeader() { }

        /// <summary>Definition constructor</summary>
        /// <param name="marker">Marker of this substream</param>
        public JpegFrameHeader(UInt16 marker) : base(marker) { }

        /// <summary>Definition constructor</summary>
        /// <param name="marker">Marker of this substream</param>
        /// <param name="length">Length of this substream</param>
        public JpegFrameHeader(UInt16 marker, UInt16 length) : base(marker, length) { }

        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.components = new List<FrameComponentParameter>();
        }
        #endregion


        #region IO Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="tables">The decoder's collection of quantization tables</param>
        public virtual void Read(Stream input)
        {
            this.Initialize();

            this.Length = JpegParser.ReadParameter16(input);

            //measure the length
            Int64 length = this.Length - 2;
            Int64 position = input.Position;

            //read parameters
            this.SamplePrecision = (Byte)input.ReadByte();
            this.Height = JpegParser.ReadParameter16(input);
            this.Width = JpegParser.ReadParameter16(input);
            this.ComponentNumbers = (Byte)input.ReadByte();

            //while there should logically exist another table
            while (length > 0)
            {
                FrameComponentParameter component = new FrameComponentParameter();
                component.Read(input);
                
                //add the table
                this.components.Add(component);

                //recalculate measurements
                length -= input.Position - position;
                position = input.Position;
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            throw new NotImplementedException("Not yet implemented.");
        }
        #endregion
    }
}