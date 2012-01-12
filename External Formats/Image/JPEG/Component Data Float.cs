using System;
using System.Collections.Generic;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Mathematics;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG
{
    /// <summary>Implementation of double-precision floating-point (Double) Component Data.</summary>
    public class ComponentDataFloat : ComponentData<Double>
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public ComponentDataFloat()
        {
            this.SourceData = new List<Int32>();
            this.SampleData = new List<Double>();
        }
        #endregion


        #region Abstract method implementation
        /// <summary>Dequantizes the component data list into the sample data location</summary>
        /// <param name="componentData">List of component data to de-quantize</param>
        /// <param name="quantizationElements">Quantization table elements to dequantize against</param>
        protected override void Dequantize(QuantizationTable qt)
        {
            for (Int32 qDctIndex = 0; qDctIndex < this.SourceData.Count; ++qDctIndex)
            {
                Int32 dct = this.SourceData[qDctIndex] * qt.Elements[qDctIndex % 64];
                this.SampleData.Add(dct);
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
            this.SampleData = DiscreteCosineTransformation.InverseDiscreteCosineTransformFastFloat(this.SampleData);
        }

        /// <summary>Performs an unshift by adding the shift level to </summary>
        /// <param name="level">Sample bit precision</param>
        /// <param name="data">List of samples to unshift</param>
        protected override void UndoLevelShift(Int32 level)
        {
            Int32 shift;
            switch (level)
            {
                case 8:
                    shift = 128;
                    break;
                case 12:
                    shift = 2048;
                    break;
                default:
                    throw new ApplicationException(String.Format("Unexpected level shift size of {0}.", level));
            }

            for (Int32 index = 0; index < this.SampleData.Count; ++index)
                this.SampleData[index] += shift;
        }
        #endregion
    }
}