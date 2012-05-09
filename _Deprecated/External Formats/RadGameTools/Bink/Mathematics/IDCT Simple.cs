using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RadGameTools.Bink.Mathematics
{
    public static class IDCT
    {
        #region Inverse Discrete Cosine Transform
        /// <summary>Performs the inverse DCT on the list of FDCT values</summary>
        /// <param name="fDctList">List of integer Forward DCT values</param>
        /// <returns>A List of integer values of from the inverse DCT</returns>
        /// <remarks>See JPEG specification, §A.3.3</remarks>
        public static List<Int32> InverseDiscreteCosineTransformInteger(Int32[] fDctList, Int32 n)
        {
            Int32[] idctTemp = new Int32[n*n];
            List<Int32> IDCT = new List<Int32>();
            Double inverseRoot2 = 1.0 / Math.Sqrt(2.0);
            Double sixteenthPi = Math.PI / 16.0;

            for (Int32 blockStart = 0; blockStart < fDctList.Length; blockStart += (n*n)) //each N x N sample block
            {
                for (Int32 y = 0; y < n; ++y)   //sample Y co-ordinate
                {
                    Double yBase = Convert.ToDouble((2 * y) + 1);

                    for (Int32 x = 0; x < n; ++x)   //sample X co-ordinate
                    {
                        Double xBase = Convert.ToDouble((2 * x) + 1); ;
                        Double sampleIdct = 0;  //sum of sum

                        //summations
                        for (Int32 u = 0; u < n; ++u)   // Horizontal frequency u
                        {
                            Double Cu = (u == 0) ? inverseRoot2 : 1.0;
                            Double xVal = xBase * u * sixteenthPi;
                            Double cosX = Math.Cos(xVal);

                            for (Int32 v = 0; v < n; ++v)   // Vertical frequency v
                            {
                                Double Cv = (v == 0) ? inverseRoot2 : 1.0;
                                Double yVal = yBase * v * sixteenthPi;
                                Double cosY = Math.Cos(yVal);
                                Int32 dctValue = fDctList[blockStart + ((n * v) + u)];

                                sampleIdct += (Cu * Cv * dctValue * cosX * cosY);
                            }
                        }

                        sampleIdct /= 4.0;
                        idctTemp[(n * y) + x] = Convert.ToInt32(sampleIdct);
                    }
                }

                IDCT.AddRange(idctTemp);
            }

            return IDCT;
        }
        #endregion
    }
}