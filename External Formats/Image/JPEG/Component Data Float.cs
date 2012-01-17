using System;
using System.Collections.Generic;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Mathematics;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Mathematics.DCT;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG
{
    /// <summary>Implementation of double-precision floating-point (Double) Component Data.</summary>
    public class ComponentDataFloat : ComponentData<Double>
    {
        #region Abstract method implementation
        /// <summary>Dequantizes the component data list into the sample data location</summary>
        /// <param name="componentData">List of component data to de-quantize</param>
        /// <param name="quantizationElements">Quantization table elements to dequantize against</param>
        /// <remarks>Also un-zig-zags the sample data while populating it</remarks>
        protected override void Dequantize(QuantizationTable qt)
        {
            this.SampleData = new Double[this.ContiguousBlockCountHorizontal, this.ContiguousBlockCountVertical][];

            for (Int32 x = 0; x < this.ContiguousBlockCountHorizontal; ++x)
                for (Int32 y = 0; y < this.ContiguousBlockCountVertical; ++y)
                {
                    Double[] temp = new Double[64];
                    Int32[] source = this.SourceData[x, y];
                    for (Int32 sample = 0; sample < 64; ++sample)
                        temp[ComponentDataFloat.UnZigZagReference[sample]] = source[sample] * qt.Elements[sample];
                    
                    this.SampleData[x, y] = temp;
                }
        }

        /// <summary>Attempts to implement JPEG specification §K.2.8.1</summary>
        /// <param name="component">Component to smooth</param>
        /// <remarks>
        ///     My thanks to the following, indicating another divide by 8 (see the IDCT fast implementation). I need to just start dividing by when I get funny results.
        ///     Adaptive AC-Coefficient Prediction for Image Compression and Blind Watermarking
        ///     (K. Veeraswamy, S. Srinivas Kumar)
        ///     http://www.academypublisher.com/jmm/vol03/no01/jmm03011622.pdf
        /// </remarks>
        protected override void SmoothingPrediction()
        {
            AmplitudeCoefficientPredictiveSmoothing.SmoothingPrediction(this.ContiguousBlockCountVertical, this.ContiguousBlockCountHorizontal, this.SampleData);
        }

        /// <summary>Performs the inverse DCT on the list of FDCT values</summary>
        /// <param name="fDctList">List of integer Forward DCT values</param>
        /// <remarks>
        ///     See JPEG specification §A.3.3
        ///     
        ///     Captures a floating-point list of data, to preserve data when generating RGB values from YCbCr data.
        /// </remarks>
        protected override void InverseDiscreteCosineTransform()
        {
            LoefflerDiscreteCosineTransformationFloat.InverseDiscreteCosineTransformFloat(this.SampleData);
        }

        /// <summary>Gets the output sample data, in sample order</summary>
        /// <returns>a Byte array of sample data</returns>
        public override Double[] GetSampleData()
        {
            Double[] sampleData = new Double[this.Width * this.Height]; //instantiate a new data array
            Int32 xCur = 0, yCur = 0;       //sampleData index calculation

            for (Int32 y = 0; y < this.ContiguousBlockCountVertical; ++y)
            {
                yCur = y * 8;

                for (Int32 blockScanline = 0; blockScanline < 8; ++blockScanline)
                {
                    if (yCur >= this.Height)
                        break;

                    for (Int32 x = 0; x < this.ContiguousBlockCountHorizontal; ++x)
                        for (Int32 blockX = 0; (blockX < 8) && (xCur < this.Width); ++blockX)
                        {
                            sampleData[yCur * this.Width + xCur] = this.SampleData[x, y][blockScanline * 8 + blockX] + 128;

                            ++xCur;
                            if (xCur == this.Width)
                            {
                                xCur = 0;
                                ++yCur;
                                goto NextScanLine;  //Curses! Need to break; twice, out of the X loop
                            }
                        }
                NextScanLine: ; //Curses, I say!
                }
            }

            return sampleData;
        }
        #endregion
    }
}