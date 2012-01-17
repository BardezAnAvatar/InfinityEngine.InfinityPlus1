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
            //re-order the blocks to fit the image grid
            //this.ReorderBlockData();

            //De-quantize the coefficients to reconstruct an approximate collection of forward DCT coefficients
            this.Dequantize(qt);

            //Undo the zig-zag coefficient between dequantizing and IDCT. Block order does not matter.
            //this.UnZigZag();

            //smooth blocks
            if (doAcPrediction)
                this.SmoothingPrediction();

            //perform the IDCT
            this.InverseDiscreteCosineTransform();

            //undo the 0-center shifting to restore back to 0 - 255 unsigned value range
            //this.UndoLevelShift(samplePrecision);

            //re-order the samples to form a true top-down image
            //this.ReorderBlockSampleData();
            //this.ReorderBlockSampleDataIndexed();
        }
        #endregion


        #region Reordering
        /*
        /// <summary>Reorders the component data 8x8 blocks based on multiple vertical sampling levels</summary>
        /// <param name="componentData">Component to unshuffle</param>
        /// <remarks>Performed on Source Data.</remarks>
        protected void ReorderBlockData()
        {
            //The way data comes in, 8x8 blocks are stored in a sampling zig-zag.
            //See JPEG specification, §A.2.3.
            List<Int32>[,] unzigged = new List<Int32>[this.ContiguousBlockCountHorizontal, this.ContiguousBlockCountVertical];
            Int32 x = 0, y = 0;

            for (Int32 blockIndex = 0; blockIndex < this.ContiguousBlockCount; blockIndex += this.McuDataSize)
            {
                for (Int32 mcuVerticalIndex = 0; mcuVerticalIndex < this.VerticalSamplingFactor; ++mcuVerticalIndex)
                {
                    for (Int32 mcuHorizontalIndex = 0; mcuHorizontalIndex < this.HorizontalSamplingFactor; ++mcuHorizontalIndex)
                    {
                        Int32 dataIndex = (blockIndex + (mcuVerticalIndex * this.HorizontalSamplingFactor) + mcuHorizontalIndex) * 64;
                        List<Int32> block = this.SourceData.GetRange(dataIndex, 64);

                        unzigged[(x + mcuHorizontalIndex), (y + mcuVerticalIndex)] = block;
                    }
                }

                //increment my baseline indecies
                x += this.HorizontalSamplingFactor;

                if (x == this.ContiguousBlockCountHorizontal)
                {
                    x = 0;
                    y += this.VerticalSamplingFactor;
                }
            }

            //After the MCU order has been unshuffled, there remains the issue that data is stored in 8x8 blocks,
            //rather than in sample order. We need to further loop through the data
            //and re-arrange samples into the actual sample order. This has to be done *after* all block processing, however.
            //In the interim, persist the reorganized blocks.
            List<Int32> output = new List<Int32>(this.SourceData.Count);

            for (y = 0; y < this.ContiguousBlockCountVertical; ++y)
                for (x = 0; x < this.ContiguousBlockCountHorizontal; ++x)
                    output.AddRange(unzigged[x, y]);

            this.SourceData = output;
        }
        */
        
        /*
        /// <summary>Reorders the component data 8x8 blocks based on multiple vertical sampling levels</summary>
        /// <param name="componentData">Component to unshuffle</param>
        /// <remarks>Performed on Sample Data.</remarks>
        protected void ReorderBlockSampleData()
        {
            List<Primitive>[,] blocks = new List<Primitive>[this.ContiguousBlockCountHorizontal, this.ContiguousBlockCountVertical];
            Int32 indexer = 0;
            for (Int32 y = 0; y < this.ContiguousBlockCountVertical; ++y)
            {
                for (Int32 x = 0; x < this.ContiguousBlockCountHorizontal; ++x)
                {
                    blocks[x, y] = this.SampleData.GetRange(indexer, 64);
                    indexer += 64;
                }
            }

            //Application has the unzigged data. Now it shall un-block the samples.
            List<Primitive> unblocked = new List<Primitive>();

            for (Int32 y = 0; y < this.ContiguousBlockCountVertical; ++y)
                for (Int32 blockScanline = 0; blockScanline < 8; ++blockScanline)
                {
                    Int32 scanLineBaseIndex = blockScanline * 8;
                    for (Int32 x = 0; x < this.ContiguousBlockCountHorizontal; ++x)
                    {
                        //avoid addrange, getrange
                        unblocked.Add(blocks[x, y][scanLineBaseIndex]);
                        unblocked.Add(blocks[x, y][scanLineBaseIndex + 1]);
                        unblocked.Add(blocks[x, y][scanLineBaseIndex + 2]);
                        unblocked.Add(blocks[x, y][scanLineBaseIndex + 3]);
                        unblocked.Add(blocks[x, y][scanLineBaseIndex + 4]);
                        unblocked.Add(blocks[x, y][scanLineBaseIndex + 5]);
                        unblocked.Add(blocks[x, y][scanLineBaseIndex + 6]);
                        unblocked.Add(blocks[x, y][scanLineBaseIndex + 7]);
                        //unblocked.AddRange(blocks[x, y].GetRange(blockScanline * 8, 8));
                    }
                }

            this.SampleData = unblocked;
        }
        */

        /*
        /// <summary>Reorders the component data 8x8 blocks based on multiple vertical sampling levels</summary>
        /// <param name="samplePrecision">Sample precision to uncenter around 0 by.</param>
        /// <remarks>Performed on Sample Data. Performs the level shifting during this process.</remarks>
        protected void ReorderBlockSampleDataIndexed(Int32 samplePrecision)
        {
            Int32[,] blocks = new Int32[this.ContiguousBlockCountHorizontal, this.ContiguousBlockCountVertical];
            Int32 indexer = 0;
            for (Int32 y = 0; y < this.ContiguousBlockCountVertical; ++y)
            {
                for (Int32 x = 0; x < this.ContiguousBlockCountHorizontal; ++x)
                {
                    blocks[x, y] = indexer;
                    indexer += 64;
                }
            }

            //Application has the unzigged data. Now it shall un-block the samples.
            List<Primitive> unblocked = new List<Primitive>();

            for (Int32 y = 0; y < this.ContiguousBlockCountVertical; ++y)
                for (Int32 blockScanline = 0; blockScanline < 8; ++blockScanline)
                {
                    Int32 scanLineBaseIndex = blockScanline * 8;
                    for (Int32 x = 0; x < this.ContiguousBlockCountHorizontal; ++x)
                    {
                        unblocked.Add(this.SampleData[blocks[x, y] + scanLineBaseIndex]);
                        unblocked.Add(this.SampleData[blocks[x, y] + scanLineBaseIndex + 1]);
                        unblocked.Add(this.SampleData[blocks[x, y] + scanLineBaseIndex + 2]);
                        unblocked.Add(this.SampleData[blocks[x, y] + scanLineBaseIndex + 3]);
                        unblocked.Add(this.SampleData[blocks[x, y] + scanLineBaseIndex + 4]);
                        unblocked.Add(this.SampleData[blocks[x, y] + scanLineBaseIndex + 5]);
                        unblocked.Add(this.SampleData[blocks[x, y] + scanLineBaseIndex + 6]);
                        unblocked.Add(this.SampleData[blocks[x, y] + scanLineBaseIndex + 7]);
                    }
                }

            this.SampleData = unblocked;
        }
        */
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

        ///// <summary>Performs an unshift by adding the shift level to </summary>
        ///// <param name="level">Sample bit precision</param>
        ///// <param name="data">List of samples to unshift</param>
        //protected abstract void UndoLevelShift(Int32 level);

        /// <summary>Gets the output sample data, in sample order</summary>
        /// <returns>a Byte array of sample data</returns>
        public abstract Primitive[] GetSampleData();
        #endregion
    }
}