﻿using System;
using System.Collections.Generic;
using System.IO;


namespace Bardez.Projects.InfinityPlus1.Files.External.Image.JPEG
{
    /// <summary>Represents a scan component, entropy-decoded</summary>
    public class ScanComponentData
    {
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

        /// <summary>Indicates which table index to use for decoding the DC coefficient</summary>
        public Byte IndexDC { get; set; }

        /// <summary>Indicates which table index to use for decoding the AC coefficients</summary>
        public Byte IndexAC { get; set; }

        /// <summary>Exposes a reference to an ICoder for DC coefficient coding</summary>
        public ICoder DcCoder { get; set; }

        /// <summary>Exposes a reference to an ICoder for AC coefficient coding</summary>
        public ICoder AcCoder { get; set; }

        /// <summary>Previous DC coefficient, prediction from the previous block</summary>
        public Int32 DcPrediction { get; set; }

        /// <summary>Represents the buffer of entropy-decoded data into Bytes for lossless, DCT, or heirarchal decoding</summary>
        /// <value>Per §F.2.1.3 The coefficients are represented as two’s complement integers.</value>
        public List<Int32> ComponentData { get; set; }

        /// <summary>List of decoded component data as floating-precision values</summary>
        public List<Double> ComponentDecodedDataFloat { get; set; }

        /// <summary>List of decoded component data as integer values</summary>
        public List<Int32> ComponentDecodedDataInteger { get; set; }
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


        #region Construction
        /// <summary>Default constructor</summary>
        public ScanComponentData()
        {
            this.ComponentData = new List<Int32>();
            this.DcPrediction = 0;
        }
        #endregion


        #region Methods
        /// <summary>Populates the component data fields</summary>
        /// <param name="scanParam">Scan component parameter data</param>
        /// <param name="frameParam">Frame component parameter data</param>
        /// <param name="frameHeader">Frame header for original width, sampling factors, etc.</param>
        public void PopulateFields(ScanComponentParameter scanParam, FrameComponentParameter frameParam, JpegFrameHeader frameHeader)
        {
            Int32 hMax = frameHeader.MaxHorizontalSamplingFactor, vMax = frameHeader.MaxVerticalSamplingFactor;
            this.Identifier = scanParam.Identifier;
            this.HorizontalSamplingFactor = frameParam.HorizontalSamplingFactor;
            this.VerticalSamplingFactor = frameParam.VerticalSamplingFactor;
            this.QuantizationTableIndex = frameParam.QuantizationTableIndex;
            this.IndexAC = scanParam.IndexAC;
            this.IndexDC = scanParam.IndexDC;

            //width; JPEG spec. §A.1.1
            Decimal factor = Convert.ToDecimal(frameParam.HorizontalSamplingFactor) / Convert.ToDecimal(hMax);
            factor = Convert.ToDecimal(frameHeader.Width) * factor;
            this.Width = Convert.ToInt32(Math.Ceiling(factor));

            //height; JPEG spec. §A.1.1
            factor = Convert.ToDecimal(frameParam.VerticalSamplingFactor) / Convert.ToDecimal(vMax);
            factor = Convert.ToDecimal(frameHeader.Height) * factor;
            this.Height = Convert.ToInt32(Math.Ceiling(factor));
        }

        ///// <summary>Reads a byte from the appropriate decoder, based off of the current reading block position</summary>
        ///// <returns>The decoded Byte</returns>
        //public Byte DecodeByte()
        //{
        //    //ICoder coder = (this.BlockPosition == 0) ? this.DcCoder : this.AcCoder;
        //    ICoder coder = this.DcCoder;    //lossless (which uses samples/byte decoding) uses DC
        //    Byte decoded = coder.Decode();
        //    this.componentData.Add(decoded);
        //    ++this.BlockPosition;
        //    this.BlockPosition %= 64;   //reset to 0 if necessary

        //    return decoded;
        //}

        /// <summary>Decodes a quantized 8x8 block (sequential) or chunk thereof (progressive) of samples from the decoder(s).</summary>
        /// <param name="halt">
        ///     Reference flag set to false that, if set in this method, will percolate upwards indicating that the return value is possibly dirty,
        ///     and the stack to be traversed upward, returning work done so far, but ultimately terminating the scan
        /// </param>
        /// <returns>A quantized Block</returns>
        public Int32[] DecodeSequentialBlock(ref Boolean halt)
        {
            Int32[] zigZagChunk = new Int32[64];    //initialized to 0.

            Int32 pred = this.DcPrediction;
            Int32 DcDiff = this.DcCoder.DecodeDC(ref pred, ref halt);  //get DC coefficient
            if (halt)   //halt condition, just return
                return zigZagChunk;

            this.DcPrediction = pred;

            zigZagChunk[0] = DcDiff;

            this.AcCoder.DecodeSequentialACs(zigZagChunk, ref halt);

            return zigZagChunk;
        }

        /// <summary>Decodes a quantized 8x8 block (sequential) or chunk thereof (progressive) of samples from the decoder(s).</summary>
        /// <param name="halt">
        ///     Reference flag set to false that, if set in this method, will percolate upwards indicating that the return value is possibly dirty,
        ///     and the stack to be traversed upward, returning work done so far, but ultimately terminating the scan
        /// </param>
        /// <param name="startSelector">First coefficient to read</param>
        /// <param name="endSelector">Last coefficient to read</param>
        /// <param name="zeroRunCount">Number of 0 block to add after this one (already adjusted)</param>
        /// <returns>A quantized Block</returns>
        public Int32[] DecodeProgressiveBlock(ref Boolean halt, Int32 startSelector, Int32 endSelector, out Int32 zeroRunCount)
        {
            zeroRunCount = 0;
            Int32 readWidth = (endSelector - startSelector) + 1;

            Int32[] zigZagChunk = new Int32[readWidth];    //initialized to 0.

            if (startSelector == 0)
            {
                Int32 pred = this.DcPrediction;
                Int32 DcDiff = this.DcCoder.DecodeDC(ref pred, ref halt);  //get DC coefficient
                if (halt)   //halt condition, just return
                    return zigZagChunk;

                this.DcPrediction = pred;

                zigZagChunk[0] = DcDiff;
            }
            else
                zeroRunCount = this.AcCoder.DecodeProgressiveACs(zigZagChunk, ref halt, readWidth);

            return zigZagChunk;
        }
        #endregion
    }
}