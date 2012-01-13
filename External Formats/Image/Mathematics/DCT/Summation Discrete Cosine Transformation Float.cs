using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Mathematics.DCT
{
    /// <summary>Represents a collection of static methods that perform the double-precision floating-point slow full summation DCT and IDCT routines.</summary>
    /// <remarks>The transformations were taken originally from the JPG specification, §A.3.3</remarks>
    public static class SummationDiscreteCosineTransformationFloat
    {
        #region Inverse Discrete Cosine Transform
        /// <summary>Performs the inverse DCT on the list of FDCT values</summary>
        /// <param name="fDctList">List of integer Forward DCT values</param>
        /// <returns>A List of double-precision floating-point values of from the inverse DCT</returns>
        /// <remarks>See JPEG specification, §A.3.3</remarks>
        public static List<Double> InverseDiscreteCosineTransformFloat(IList<Int32> fDctList)
        {
            Double[] idctTemp = new Double[64];
            List<Double> IDCT = new List<Double>();
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
                        idctTemp[(8 * y) + x] = sampleIdct;
                    }
                }

                IDCT.AddRange(idctTemp);
            }

            return IDCT;
        }

        /// <summary>Performs the inverse DCT on the list of FDCT values</summary>
        /// <param name="fDctList">List of double-precision floating-point Forward DCT values</param>
        /// <returns>A List of double-precision floating-point values of from the inverse DCT</returns>
        /// <remarks>See JPEG specification, §A.3.3</remarks>
        public static List<Double> InverseDiscreteCosineTransformFloat(IList<Double> fDctList)
        {
            Double[] idctTemp = new Double[64];
            List<Double> IDCT = new List<Double>();
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
                                Double dctValue = fDctList[blockStart + ((8 * v) + u)];

                                sampleIdct += (Cu * Cv * dctValue * cosX * cosY);
                            }
                        }

                        sampleIdct /= 4.0;
                        idctTemp[(8 * y) + x] = sampleIdct;
                    }
                }

                IDCT.AddRange(idctTemp);
            }

            return IDCT;
        }
        #endregion


        #region Forward Discrete Cosine Transform
        /// <summary>Implements the formal Inverse Discrete Cosine Transform from the JPEG specification, §A.3.3</summary>
        /// <param name="sampleList">List of input samples to transform</param>
        /// <returns>A List of double-precision floating-point values of from the forward DCT operation</returns>
        /// <remarks>See JPEG specification, §A.3.3</remarks>
        public static List<Double> ForwardDiscreteCosineTransformFloat(IList<Int32> sampleList)
        {
            Double[] fdctTemp = new Double[64];
            List<Double> FDCT = new List<Double>();

            for (Int32 blockStart = 0; blockStart < sampleList.Count; blockStart += 64) //each 8x8 sample block
            {
                for (Int32 v = 0; v < 8; ++v)
                {
                    for (Int32 u = 0; u < 8; ++u)
                    {
                        Double coefficient = 0.0;

                        for (Int32 x = 0; x < 8; ++x)
                        {
                            for (Int32 y = 0; y < 8; ++y)
                            {
                                Int32 sample = sampleList[blockStart + ((y * 8) + x)];
                                coefficient += sample * Math.Cos(((2 * x) + 1) * u * Math.PI / 16) * Math.Cos(((2 * y) + 1) * v * Math.PI / 16);
                            }
                        }

                        coefficient /= 4.0;
                        Double Cu = (u == 0) ? 1.0 / Math.Sqrt(2.0) : 1.0;
                        Double Cv = (v == 0) ? 1.0 / Math.Sqrt(2.0) : 1.0;

                        coefficient *= (Cu * Cv);
                        fdctTemp[(v * 8) + u] = coefficient;
                    }
                }

                FDCT.AddRange(fdctTemp);
            }

            return FDCT;
        }

        /// <summary>Implements the formal Inverse Discrete Cosine Transform from the JPEG specification, §A.3.3</summary>
        /// <param name="sampleList">List of input samples to transform</param>
        /// <returns>A List of Double-precision values of from the forward DCT operation</returns>
        /// <remarks>See JPEG specification, §A.3.3</remarks>
        public static List<Double> ForwardDiscreteCosineTransformFloat(IList<Double> sampleList)
        {
            Double[] fdctTemp = new Double[64];
            List<Double> FDCT = new List<Double>();

            for (Int32 blockStart = 0; blockStart < sampleList.Count; blockStart += 64) //each 8x8 sample block
            {
                for (Int32 v = 0; v < 8; ++v)
                {
                    for (Int32 u = 0; u < 8; ++u)
                    {
                        Double coefficient = 0.0;

                        for (Int32 x = 0; x < 8; ++x)
                        {
                            for (Int32 y = 0; y < 8; ++y)
                            {
                                Double sample = sampleList[blockStart + ((y * 8) + x)];
                                coefficient += sample * Math.Cos(((2 * x) + 1) * u * Math.PI / 16) * Math.Cos(((2 * y) + 1) * v * Math.PI / 16);
                            }
                        }

                        coefficient /= 4.0;
                        Double Cu = (u == 0) ? 1.0 / Math.Sqrt(2.0) : 1.0;
                        Double Cv = (v == 0) ? 1.0 / Math.Sqrt(2.0) : 1.0;

                        coefficient *= (Cu * Cv);
                        fdctTemp[(v * 8) + u] = coefficient;
                    }
                }

                FDCT.AddRange(fdctTemp);
            }

            return FDCT;
        }
        #endregion


        #region Proof-only helper methods
        /// <summary>Helper function that gets the DCT operation's cosine values based on the X and U (or Y and V, respecively) values</summary>
        /// <param name="x">Pixel x co-ordinate</param>
        /// <param name="u">Horizontal sampling frequency</param>
        /// <returns>The cosine of the value for this part of the DCT operation</returns>
        /// <remarks>Proof method, too slow to realisticaly use</remarks>
        private static Double DctCosine(Int32 x, Int32 u)
        {
            Double param = ((2 * x) + 1) * u * Math.PI / 16.0;
            return Math.Cos(param);
        }

        /// <summary>Gets the DCT scaling factor for either Cu or Cv</summary>
        /// <param name="u">The U (or V) frequency</param>
        /// <returns>the inverse of root 2 or 1</returns>
        /// <remarks>Proof method, too slow to realisticaly use</remarks>
        private static Double DctScalingFactor(Int32 u)
        {
            return u == 0 ? (1.0 / Math.Sqrt(2.0)) : 1.0;
        }
        #endregion
    }
}