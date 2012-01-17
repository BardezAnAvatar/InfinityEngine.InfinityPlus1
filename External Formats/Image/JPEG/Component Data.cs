using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG
{
    /// <summary>Abstract implementation of Component Data.</summary>
    /// <typeparam name="Primitive">A primitive type, intended to be of Int32 or Double types</typeparam>
    public abstract class ComponentData<Primitive> where Primitive : struct, IConvertible
    {
        #region UnzigZag lookup Table
        /// <summary>Reference table to look up a block sample index for a given input zig-zag index</summary>
        public static readonly Int32[] UnZigZagReference =
        {
            0,  1,  8,  16, 9,  2,  3,  10,
            17, 24, 32, 25, 18, 11, 4,  5,
            12, 19, 26, 33, 40, 48, 41, 34,
            27, 20, 13, 6,  7,  14, 21, 28,
            35, 42, 49, 56, 57, 50, 43, 36,
            29, 22, 15, 23, 30, 37, 44, 51,
            58, 59, 52, 45, 38, 31, 39, 46,
            53, 60, 61, 54, 47, 55, 62, 63
        };
        
        /// <summary>Reference table to look up a zig-zag index for a given input block sample index</summary>
        public static readonly Int32[] ZigZagReference =
        {
            0,  1,  5,  6,  14, 15, 27, 28,
            2,  4,  7,  13, 16, 26, 29, 42,
            3,  8,  12, 17, 25, 30, 41, 43,
            9,  11, 18, 24, 31, 40, 44, 53,
            10, 19, 23, 32, 39, 45, 52, 54,
            20, 22, 33, 38, 46, 51, 55, 60,
            21, 34, 37, 47, 50, 56, 59, 61,
            35, 36, 48, 49, 57, 58, 62, 63
        };
        #endregion


        #region Fields
        /// <summary>Height of the component data</summary>
        public Int32 Height { get; set; }

        /// <summary>Width of the component data</summary>
        public Int32 Width { get; set; }

        /// <summary>A unique label to the n-th component in the sequence of frame component specification parameters.</summary>
        /// <remarks>These values shall be used in the scan headers to identify the components in the scan.</remarks>
        public Byte Identifier { get; set; }

        /// <summary>Specifies the relationship between the component horizontal dimension and maximum image dimension X</summary>
        /// <remarks>Also specifies the number of horizontal data units of component C in each MCU, when more than one component is encoded in a scan.</remarks>
        /// <value>1 to 4</value>
        public Byte HorizontalSamplingFactor { get; set; }

        /// <summary>Specifies the relationship between the component vertical dimension and maximum image dimension Y</summary>
        /// <remarks>Also specifies the number of vertical data units of component C in each MCU, when more than one component is encoded in a scan.</remarks>
        /// <value>1 to 4</value>
        public Byte VerticalSamplingFactor { get; set; }

        /// <summary>Specifies one of four possible quantization table destinations from which the quantization table to use for dequantization of DCT coefficients of component C is retrieved</summary>
        public Byte QuantizationTableIndex { get; set; }

        /// <summary>Represents the buffer of entropy-decoded coefficient data into Bytes for lossless, DCT, or heirarchal decoding</summary>
        /// <value>Per §F.2.1.3 The coefficients are represented as two’s complement integers.</value>
        public Int32[,][] SourceData { get; set; }

        /// <summary>2D array of decoded component data blocks</summary>
        public Primitive[,][] SampleData { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the number of data units per MCU, based on the sampling factor</summary>
        public Int32 McuDataSize
        {
            get { return this.VerticalSamplingFactor * this.HorizontalSamplingFactor; }
        }

        /// <summary>Gets the number of contiguous blocks in a horizontal line</summary>
        public Int32 ContiguousBlockCountHorizontal
        {
            get
            {
                //must have height fit into a vertical muliple of VerticalSamplingFactor.
                Int32 hBlocks = (this.Width / 8) + ((this.Width % 8) == 0 ? 0 : 1);

                if (hBlocks % this.HorizontalSamplingFactor != 0)
                    hBlocks = ((hBlocks / this.HorizontalSamplingFactor) + 1) * this.HorizontalSamplingFactor;

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

                if (vBlocks % this.VerticalSamplingFactor != 0)
                    vBlocks = ((vBlocks / this.VerticalSamplingFactor) + 1) * this.VerticalSamplingFactor;

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
        #endregion


        #region Exposed Methods
        /// <summary>Performs the mathematical transforms after de-coding from the input stream</summary>
        public void DecodeData(QuantizationTable qt, Int32 samplePrecision, Boolean doAcPrediction = false)
        {
            //De-quantize the coefficients to reconstruct an approximate collection of forward DCT coefficients
            this.Dequantize(qt);

            //smooth blocks
            if (doAcPrediction)
                this.SmoothingPrediction();

            //perform the IDCT
            this.InverseDiscreteCosineTransform();
        }
        #endregion


        #region Abstract methods
        /// <summary>Dequantizes the component data list into the sample data location</summary>
        /// <param name="componentData">List of component data to de-quantize</param>
        /// <param name="quantizationElements">Quantization table elements to dequantize against</param>
        protected abstract void Dequantize(QuantizationTable qt);

        /// <summary>Attempts to implement JPEG specification §K.2.8.1</summary>
        /// <param name="component">Component to smooth</param>
        /// <remarks>
        ///     My thanks to the following, indicating another divide by 8 (see the IDCT fast implementation). I need to just start dividing by when I get funny results.
        ///     Adaptive AC-Coefficient Prediction for Image Compression and Blind Watermarking
        ///     (K. Veeraswamy, S. Srinivas Kumar)
        ///     http://www.academypublisher.com/jmm/vol03/no01/jmm03011622.pdf
        /// </remarks>
        protected abstract void SmoothingPrediction();

        /// <summary>Performs the inverse DCT on the list of FDCT values</summary>
        /// <param name="fDctList">List of integer Forward DCT values</param>
        /// <remarks>
        ///     See JPEG specification §A.3.3
        ///     
        ///     Captures a floating-point list of data, to preserve data when generating RGB values from YCbCr data.
        /// </remarks>
        protected abstract void InverseDiscreteCosineTransform();

        /// <summary>Gets the output sample data, in sample order</summary>
        /// <returns>a Byte array of sample data</returns>
        public abstract Primitive[] GetSampleData();
        #endregion
    }
}