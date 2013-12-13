using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Mathematics.DCT
{
    /// <summary>Represents a collection of static methods that perform the integer slow full summation DCT and IDCT routines.</summary>
    /// <remarks>The transformations were taken originally from the JPG specification, §A.3.3</remarks>
    public static class SummationDiscreteCosineTransformationInteger
    {
        #region Inverse Discrete Cosine Transform
        /// <summary>Performs the inverse DCT on the list of FDCT values</summary>
        /// <param name="fDctList">List of integer Forward DCT values</param>
        /// <returns>A List of integer values of from the inverse DCT</returns>
        /// <remarks>See JPEG specification, §A.3.3</remarks>
        public static List<Int32> InverseDiscreteCosineTransformInteger(IList<Int32> fDctList)
        {
            Int32[] idctTemp = new Int32[64];
            List<Int32> IDCT = new List<Int32>();
            Double inverseRoot2 = 1.0 / Math.Sqrt(2.0);
            Double sixteenthPi = Math.PI / 16.0;

            for (Int32 blockStart = 0; blockStart < fDctList.Count; blockStart += 64) //each 8x8 sample block
            {
                for (Int32 y = 0; y < 8; ++y)   //sample Y co-ordinate
                {
                    Double yBase = Convert.ToDouble((2 * y) + 1);

                    for (Int32 x = 0; x < 8; ++x)   //sample X co-ordinate
                    {
                        Double xBase = Convert.ToDouble((2 * x) + 1); ;
                        Double sampleIdct = 0;  //sum of sum

                        //summations
                        for (Int32 u = 0; u < 8; ++u)   // Horizontal frequency u
                        {
                            Double Cu = (u == 0) ? inverseRoot2 : 1.0;
                            Double xVal = xBase * u * sixteenthPi;
                            Double cosX = Math.Cos(xVal);

                            for (Int32 v = 0; v < 8; ++v)   // Vertical frequency v
                            {
                                Double Cv = (v == 0) ? inverseRoot2 : 1.0;
                                Double yVal = yBase * v * sixteenthPi;
                                Double cosY = Math.Cos(yVal);
                                Int32 dctValue = fDctList[blockStart + ((8 * v) + u)];

                                sampleIdct += (Cu * Cv * dctValue * cosX * cosY);
                            }
                        }

                        sampleIdct /= 4.0;
                        idctTemp[(8 * y) + x] = Convert.ToInt32(sampleIdct);
                    }
                }

                IDCT.AddRange(idctTemp);
            }

            return IDCT;
        }
        #endregion
    }
}