using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.Files.External.Image.Mathematics
{
    /// <summary>Represents a collection of static methods that perform the DCT and IDCT routines, and various implementations thereof.</summary>
    /// <remarks>
    ///     This is going into source control with the eight iterations to prove how much of a clusterfuck this whole thing was to my brain.
    ///     
    ///     The slow transforms are taken originally from the JPG specification, §A.3.3
    ///     The 'fast' modieifer Loeffler algorithms are taken from a number of sources:
    ///     *Loeffler's paper:
    ///         http://www3.matapp.unimib.it/corsi-2007-2008/matematica/istituzioni-di-analisi-numerica/jpeg/papers/11-multiplications.pdf
    ///     *DCT and IDCT Implementations on Different FPGA Technologies (Khurram Bukhari, Georgi Kuzmanov and Stamatis Vassiliadis):
    ///         http://ce.et.tudelft.nl/~george/publications/Conf/ProRISC02/DCT.pdf
    ///     *An 8-Point IDCT Computing Resource Implemented on a TriMedia/CPU64 Reconfigurable Functional Unit
    ///         (Mihai Simayz, Sorin Cotofanay, Jos T.J. van Eijndhovenz, Stamatis Vassiliadisy)
    ///         (this has the best, most readable diagram)
    ///         http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.157.9472&rep=rep1&type=pdf
    ///     *finally, to answer my descaling question (§2.2, page 2):
    ///     A COMPUTATIONALLY EFFICIENT HIGH-QUALITY CORDIC BASED DCT
    ///         (B. Heyne, C. C. Sun, J. Goetze and S. J. Ruan)
    ///         http://www.eurasip.org/Proceedings/Eusipco/Eusipco2006/papers/1568979966.pdf
    /// </remarks>
    public static class DiscreteCosineTransformation
    {
        #region constants
        private const Double rootHalf = 0.70710678118654757;    //Math.Sqrt(0.5);
        private const Double rootTwo =  1.4142135623730951;     //Math.Sqrt(2.0);

        //k * cos(n * pi / 16) or k * sin(n * pi / 16); k = root2; n=1,3,6; cos/sin and n represent the variable name
        private const Double rotateC6 = 0.54119610014619712;    //rootTwo * Math.Cos(6.0 * Math.PI / 16.0);
        private const Double rotateC1 = 1.3870398453221475;     //rootTwo * Math.Cos(1.0 * Math.PI / 16.0);
        private const Double rotateC3 = 1.1758756024193588;     //rootTwo * Math.Cos(3.0 * Math.PI / 16.0);
        private const Double rotateS6 = 1.3065629648763766;     //rootTwo * Math.Sin(6.0 * Math.PI / 16.0);
        private const Double rotateS1 = 0.275899379282943;      //rootTwo * Math.Sin(1.0 * Math.PI / 16.0);
        private const Double rotateS3 = 0.78569495838710213;    //rootTwo * Math.Sin(3.0 * Math.PI / 16.0);
        #endregion


        #region Inverse Discrete Cosine Transform
        #region Slow IDCTs
        /// <summary>Performs the inverse DCT on the list of FDCT values</summary>
        /// <param name="fDctList">List of integer Forward DCT values</param>
        /// <returns>A List of integer values of from the inverse DCT</returns>
        /// <remarks>See JPEG specification, §A.3.3</remarks>
        public static List<Double> InverseDiscreteCosineTransformSlowFloat(IList<Int32> fDctList)
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
        /// <param name="fDctList">List of integer Forward DCT values</param>
        /// <returns>A List of Double-precision values of from the inverse DCT</returns>
        /// <remarks>See JPEG specification, §A.3.3</remarks>
        public static List<Double> InverseDiscreteCosineTransformSlowFloat(IList<Double> fDctList)
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

        /// <summary>Performs the inverse DCT on the list of FDCT values</summary>
        /// <param name="fDctList">List of integer Forward DCT values</param>
        /// <returns>A List of integer values of from the inverse DCT</returns>
        /// <remarks>See JPEG specification, §A.3.3</remarks>
        public static List<Int32> InverseDiscreteCosineTransformSlowInteger(IList<Int32> fDctList)
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

        #region Fast IDCTs
        /// <summary>Performs the inverse DCT on the list of FDCT values</summary>
        /// <param name="fDctList">List of integer Forward DCT values</param>
        /// <returns>A List of integer values of from the inverse DCT</returns>
        /// <remarks>
        ///     See JPEG specification, §A.3.3
        ///     More to point, see http://ce.et.tudelft.nl/~george/publications/Conf/ProRISC02/DCT.pdf for my source of the 'fast' IDCT.
        ///     http://www.google.com/url?sa=t&rct=j&q=modieifed%20loeffler%20algorithm&source=web&cd=6&ved=0CFYQFjAF&url=http%3A%2F%2Fciteseerx.ist.psu.edu%2Fviewdoc%2Fdownload%3Fdoi%3D10.1.1.157.9472%26rep%3Drep1%26type%3Dpdf&ei=470JT87TAY7eggeTyfGbAg&usg=AFQjCNG_PsMSUdzUPkfno6rfa6UPnyc7vQ&cad=rja
        ///     Also, see http://www.cmlab.csie.ntu.edu.tw/cml/dsp/training/coding/transform/fft.html.
        ///     It loses accuracy, but processes much more quickly.
        /// </remarks>
        public static List<Int32> InverseDiscreteCosineTransformFastInteger(IList<Int32> fDctList)
        {
            List<Int32> idct = new List<Int32>();

            //loop through each block
            for (Int32 baseIndex = 0; baseIndex < fDctList.Count; baseIndex += 64)
            {
                Int32[] idctTemp = new Int32[64];
                //do a frst pass on the row, then a second on the columns
                for (Int32 row = 0; row < 8; ++row)
                {
                    Int32 rowBase = row * 8;
                    Int32 sourceBase = baseIndex + rowBase;
                    //if 0...7 are all 0, all the cross addition/multiplication comes out to 0 anyway.
                    if (fDctList[sourceBase] == 0 && fDctList[sourceBase + 1] == 0 && fDctList[sourceBase + 2] == 0 && fDctList[sourceBase + 3] == 0
                        && fDctList[sourceBase + 4] == 0 && fDctList[sourceBase + 5] == 0 && fDctList[sourceBase + 6] == 0 && fDctList[sourceBase + 7] == 0)
                        continue;   //idctTemp is already initialized to 0.
                    else
                    {
                        DiscreteCosineTransformation.InverseDiscreteCosineTransformFastRowInteger(
                            out idctTemp[rowBase], out idctTemp[rowBase + 1], out idctTemp[rowBase + 2], out idctTemp[rowBase + 3],
                            out idctTemp[rowBase + 4], out idctTemp[rowBase + 5], out idctTemp[rowBase + 6], out idctTemp[rowBase + 7],
                            fDctList[sourceBase], fDctList[sourceBase + 1], fDctList[sourceBase + 2], fDctList[sourceBase + 3],
                            fDctList[sourceBase + 4], fDctList[sourceBase + 5], fDctList[sourceBase + 6], fDctList[sourceBase + 7]);
                    }
                }

                //second pass on columns
                for (Int32 column = 0; column < 8; ++column)
                {
                    //if 0...7 are all 0, all the cross addition/multiplication comes out to 0 anyway.
                    if (idctTemp[column] == 0 && idctTemp[8 + column] == 0 && idctTemp[16 + column] == 0 && idctTemp[24 + column] == 0
                        && idctTemp[32 + column] == 0 && idctTemp[40 + column] == 0 && idctTemp[48 + column] == 0 && idctTemp[56 + column] == 0)
                        continue;   //idctTemp is already initialized to 0.
                    else
                    {
                        DiscreteCosineTransformation.InverseDiscreteCosineTransformFastRowInteger(
                            out idctTemp[column], out idctTemp[column + 8], out idctTemp[column + 16], out idctTemp[column + 24],
                            out idctTemp[column + 32], out idctTemp[column + 40], out idctTemp[column + 48], out idctTemp[column + 56],
                            idctTemp[column], idctTemp[column + 8], idctTemp[column + 16], idctTemp[column + 24],
                            idctTemp[column + 32], idctTemp[column + 40], idctTemp[column + 48], idctTemp[column + 56]);
                    }
                }

                //third pass; the values need to be de-scaled by 8.
                //man, this took a while to figure out.
                //I can't tell if this is commonly left out in mathematical papers as obvious, nor
                //can I tell if it i a needed division of 1/(2*root(2)) after each pass, or if it is
                //a needed division of N=8.
                for (Int32 descaleIndex = 0; descaleIndex < 64; ++descaleIndex)
                    idctTemp[descaleIndex] /= 8;

                idct.AddRange(idctTemp);
            }

            return idct;
        }

        /// <summary>Implementation of the 1-D modified Loeffler algoritm IDCT</summary>
        /// <param name="o0">Output parameter 0</param>
        /// <param name="o1">Output parameter 1</param>
        /// <param name="o2">Output parameter 2</param>
        /// <param name="o3">Output parameter 3</param>
        /// <param name="o4">Output parameter 4</param>
        /// <param name="o5">Output parameter 5</param>
        /// <param name="o6">Output parameter 6</param>
        /// <param name="o7">Output parameter 7</param>
        /// <param name="in0">Input parameter 0</param>
        /// <param name="in1">Input parameter 1</param>
        /// <param name="in2">Input parameter 2</param>
        /// <param name="in3">Input parameter 3</param>
        /// <param name="in4">Input parameter 4</param>
        /// <param name="in5">Input parameter 5</param>
        /// <param name="in6">Input parameter 6</param>
        /// <param name="in7">Input parameter 7</param>
        /// <remarks>A purty picture to help visualize can be found at: https://lh4.googleusercontent.com/W4XKZHNpsGlsGt6fHIYKvxcGEafiWujLIfuzilXxettuUifV4ndFFIL0AK6KTQP24TobRdLVXsfO3t5FMJTOMSuDBQ2GoY8qDY4vzZ1HYQ2nffCVyA </remarks>
        private static void InverseDiscreteCosineTransformFastRowInteger(
            out Int32 o0, out Int32 o1, out Int32 o2, out Int32 o3, out Int32 o4, out Int32 o5, out Int32 o6, out Int32 o7,
            Int32 in0, Int32 in1, Int32 in2, Int32 in3, Int32 in4, Int32 in5, Int32 in6, Int32 in7)
        {
            //temporary 'registers' to reorder the input
            Int32 reg0 = in0, reg1 = in4, reg2 = in2, reg3 = in6, reg4 = in7, reg5 = in3, reg6 = in5, reg7 = in1;

            /* phase 1, as some call it */
            o7 = Convert.ToInt32(reg7 * DiscreteCosineTransformation.rootHalf);
            o4 = Convert.ToInt32(reg4 * DiscreteCosineTransformation.rootHalf);
            DiscreteCosineTransformation.ButterflyInteger(out o7, out o4, o7, o4); //the butterfly right after is easy to miss in the Bukhari, Kuzmanov and Vassiliadis paper / PDF

            //butterfly
            DiscreteCosineTransformation.ButterflyInteger(out o0, out o1, reg0, reg1);

            //rotate 2, 6
            DiscreteCosineTransformation.RotateInteger(out o2, out o3, reg2, reg3, DiscreteCosineTransformation.rotateC6, DiscreteCosineTransformation.rotateS6);
            /* end phase 1 */


            /* 'phase' 2 */
            DiscreteCosineTransformation.ButterflyInteger(out o0, out o3, o0, o3);
            DiscreteCosineTransformation.ButterflyInteger(out o1, out o2, o1, o2);
            DiscreteCosineTransformation.ButterflyInteger(out o4, out o6, o4, reg6);
            DiscreteCosineTransformation.ButterflyInteger(out o7, out o5, o7, reg5);
            /* end 'phase' 2 */


            /* phase 3, rotation, could be part of phase 2, still, I guess */
            DiscreteCosineTransformation.RotateInteger(out o5, out o6, o5, o6, DiscreteCosineTransformation.rotateC1, DiscreteCosineTransformation.rotateS1);
            DiscreteCosineTransformation.RotateInteger(out o4, out o7, o4, o7, DiscreteCosineTransformation.rotateC3, DiscreteCosineTransformation.rotateS3);
            /* end phase 3 */


            /* phase 4, final butterfly */
            DiscreteCosineTransformation.ButterflyInteger(out o0, out o7, o0, o7);
            DiscreteCosineTransformation.ButterflyInteger(out o1, out o6, o1, o6);
            DiscreteCosineTransformation.ButterflyInteger(out o2, out o5, o2, o5);
            DiscreteCosineTransformation.ButterflyInteger(out o3, out o4, o3, o4);
            /* end phase 4*/
        }

        /// <summary>Implements the 'butterfly' operation on the modified Loeffler algorithm</summary>
        /// <param name="o0">Output top parameter</param>
        /// <param name="o1">Output bottom parameter</param>
        /// <param name="i0">Input top parameter</param>
        /// <param name="i1">Input bottom parameter</param>
        private static void ButterflyInteger(out Int32 o0, out Int32 o1, Int32 i0, Int32 i1)
        {
            o0 = i0 + i1;
            o1 = i0 - i1;
        }

        /// <summary>Implements the 'rotator' operation on the modified Loeffler algorithm</summary>
        /// <param name="o0">Output top parameter</param>
        /// <param name="o1">Output bottom parameter</param>
        /// <param name="i0">Input top parameter</param>
        /// <param name="i1">Input bottom parameter</param>
        /// <param name="cos">Appropriate cosine constant value</param>
        /// <param name="sin">Appropriate sin constant value</param>
        private static void RotateInteger(out Int32 o0, out Int32 o1, Int32 i0, Int32 i1, Double cos, Double sin)
        {
            o0 = Convert.ToInt32((i0 * cos) - (i1 * sin));
            o1 = Convert.ToInt32((i0 * sin) + (i1 * cos));
        }
        #endregion
        #endregion

        #region Forward Discrete Cosine Transform
        /// <summary>Implements the formal Inverse Discrete Cosine Transform from the JPEG specification, §A.3.3</summary>
        /// <param name="sampleList">List of input samples to transform</param>
        /// <returns>A List of Double-precision values of from the forward DCT operation</returns>
        /// <remarks>See JPEG specification, §A.3.3</remarks>
        public static List<Double> ForwardDiscreteCosineTransformSlow(IList<Int32> sampleList)
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
        public static List<Double> ForwardDiscreteCosineTransformSlow(IList<Double> sampleList)
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

        #region Code that does not implement the algorithm properly
        /// <summary>Performs the inverse DCT on the list of FDCT values</summary>
        /// <param name="fDctList">List of integer Forward DCT values</param>
        /// <remarks>See JPEG specification, §A.3.3</remarks>
        [Obsolete("Does not work")]
        private static void FailedInverseDiscreteCosineTransformSlow(IList<Int32> fDctList)
        {
            Double[] IDCT = new Double[64];
            Double denominatorRoot2 = 1.0 / Math.Sqrt(2.0);

            for (Int32 blockStart = 0; blockStart < fDctList.Count; blockStart += 64) //each 8x8 sample block
            {
                for (Int32 y = 0; y < 8; ++y)   //sample Y co-ordinate
                {
                    Double yBase = (Convert.ToDouble((2 * y) + 1) / 16.0) * Math.PI;

                    for (Int32 x = 0; x < 8; ++x)   //sample X co-ordinate
                    {
                        Double sampleIdct = 0;
                        Double xBase = (Convert.ToDouble((2 * x) + 1) / 16.0) * Math.PI;
                        Int32 dctValue = fDctList[blockStart + ((8 * y) + x)];

                        //summations
                        for (Int32 u = 0; u < 8; ++u)   // Horizontal frequency u
                        {
                            Double Cu = (u == 0) ? denominatorRoot2 : 1.0;
                            Double xVal = xBase * u;
                            Double cosX = Math.Cos(xVal);
                            Double dctCosX = dctValue * cosX;

                            for (Int32 v = 0; v < 8; ++v)   // Vertical frequency v
                            {
                                Double Cv = (v == 0) ? denominatorRoot2 : 1.0;
                                Double yVal = yBase * v;
                                Double cosY = Math.Cos(yVal);

                                sampleIdct += (Cv * dctCosX * cosY);
                            }

                            sampleIdct *= Cu;
                        }

                        sampleIdct /= 4.0;
                        IDCT[(8 * y) + x] = sampleIdct;
                    }
                }

                //copy the IDCT values
                for (Int32 index = 0; index < 64; ++index)
                    fDctList[blockStart + index] = Convert.ToInt32(IDCT[index]);
            }
        }

        /// <summary>Performs the inverse DCT on the list of FDCT values</summary>
        /// <param name="fDctList">List of integer Forward DCT values</param>
        /// <remarks>See JPEG specification, §A.3.3</remarks>
        [Obsolete("Does not work")]
        private static void FailedInverseDiscreteCosineTransformSlowRetry(IList<Int32> fDctList)
        {
            Double[] IDCT = new Double[64];
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
                        Int32 dctValue = fDctList[blockStart + ((8 * y) + x)];
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

                                sampleIdct += (Cu * Cv * dctValue * cosX * cosY);
                            }
                        }

                        sampleIdct /= 4.0;
                        IDCT[(8 * y) + x] = sampleIdct;
                    }
                }

                //copy the IDCT values
                for (Int32 index = 0; index < 64; ++index)
                    fDctList[blockStart + index] = Convert.ToInt32(IDCT[index]);
            }
        }

        /// <summary>Performs the inverse DCT on the list of FDCT values</summary>
        /// <param name="fDctList">List of integer Forward DCT values</param>
        /// <remarks>See JPEG specification, §A.3.3</remarks>
        [Obsolete("Does not work")]
        private static void FailedInverseDiscreteCosineTransformSlowThirdCharm(IList<Int32> fDctList)
        {
            Double[] IDCT = new Double[64];
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
                        Int32 dctValue = fDctList[blockStart + ((8 * y) + x)];
                        Double sampleIdct = 0;  //sum of sum

                        //summations

                        //let's try doing a row, then a column.
                        Int32 v, u;
                        Double Cv, Cu, xVal, yVal, cosX, cosY;

                        //set up static-ish V variables
                        v = y;
                        Cv = (v == 0) ? inverseRoot2 : 1.0;
                        yVal = yBase * v * sixteenthPi;
                        cosY = Math.Cos(yVal);
                        //loop trough U
                        for (u = 0; u < 8; ++u)
                        {
                            Cu = (u == 0) ? inverseRoot2 : 1.0;
                            xVal = xBase * u * sixteenthPi;
                            cosX = Math.Cos(xVal);

                            sampleIdct += (Cu * Cv * dctValue * cosX * cosY);
                        }

                        //set up static-ish U variables
                        u = x;
                        Cu = (u == 0) ? inverseRoot2 : 1.0;
                        xVal = xBase * u * sixteenthPi;
                        cosX = Math.Cos(xVal);
                        for (v = 0; v < 8; ++v)
                        {
                            Cv = (v == 0) ? inverseRoot2 : 1.0;
                            yVal = yBase * v * sixteenthPi;
                            cosY = Math.Cos(yVal);
                            sampleIdct += (Cu * Cv * dctValue * cosX * cosY);
                        }

                        sampleIdct /= 4.0;
                        IDCT[(8 * y) + x] = sampleIdct;
                    }
                }

                //copy the IDCT values
                for (Int32 index = 0; index < 64; ++index)
                    fDctList[blockStart + index] = Convert.ToInt32(IDCT[index]);
            }
        }

        /// <summary>Performs the inverse DCT on the list of FDCT values</summary>
        /// <param name="fDctList">List of integer Forward DCT values</param>
        /// <remarks>See JPEG specification, §A.3.3</remarks>
        [Obsolete("Does not work")]
        private static void FailedInverseDiscreteCosineTransformSlowWiki(IList<Int32> fDctList)
        {
            Double[] IDCT = new Double[64];
            Double alphaZero = Math.Sqrt(1.0 / 8.0);
            Double alphaOther = Math.Sqrt(2.0 / 8.0);
            Double eighthPi = Math.PI / 8.0;

            for (Int32 blockStart = 0; blockStart < fDctList.Count; blockStart += 64) //each 8x8 sample block
            {
                for (Int32 y = 0; y < 8; ++y)   //sample Y co-ordinate
                {
                    Double yBase = Convert.ToDouble(y + 0.5);

                    for (Int32 x = 0; x < 8; ++x)   //sample X co-ordinate
                    {
                        Double xBase = Convert.ToDouble(x + 0.5);
                        Int32 dctValue = fDctList[blockStart + ((8 * y) + x)];
                        Double sampleIdct = 0;  //sum of sum

                        //summations
                        for (Int32 u = 0; u < 8; ++u)   // Horizontal frequency u
                        {
                            Double Cu = (u == 0) ? alphaZero : alphaOther;
                            Double xVal = xBase * u * eighthPi;
                            Double cosX = Math.Cos(xVal);

                            for (Int32 v = 0; v < 8; ++v)   // Vertical frequency v
                            {
                                Double Cv = (v == 0) ? alphaZero : alphaOther;
                                Double yVal = yBase * v * eighthPi;
                                Double cosY = Math.Cos(yVal);

                                sampleIdct += (Cu * Cv * dctValue * cosX * cosY);
                            }
                        }

                        sampleIdct /= 4.0;
                        IDCT[(8 * y) + x] = sampleIdct;
                    }
                }

                //copy the IDCT values
                for (Int32 index = 0; index < 64; ++index)
                    fDctList[blockStart + index] = Convert.ToInt32(IDCT[index]);
            }
        }

        /// <summary>Performs the inverse DCT on the list of FDCT values</summary>
        /// <param name="fDctList">List of integer Forward DCT values</param>
        /// <remarks>See JPEG specification, §A.3.3</remarks>
        [Obsolete("Does not work")]
        private static void FailedInverseDiscreteCosineTransformSlowSubSum(IList<Int32> fDctList)
        {
            Double[] IDCT = new Double[64];
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
                        Int32 dctValue = fDctList[blockStart + ((8 * y) + x)];
                        Double sampleIdct = 0;  //sum of sum

                        //summations
                        for (Int32 u = 0; u < 8; ++u)   // Horizontal frequency u
                        {
                            Double Cu = (u == 0) ? inverseRoot2 : 1.0;
                            Double xVal = xBase * u * sixteenthPi;
                            Double cosX = Math.Cos(xVal);

                            Double innerSum = 0;
                            for (Int32 v = 0; v < 8; ++v)   // Vertical frequency v
                            {
                                Double Cv = (v == 0) ? inverseRoot2 : 1.0;
                                Double yVal = yBase * v * sixteenthPi;
                                Double cosY = Math.Cos(yVal);

                                innerSum += (Cv * dctValue * cosY);
                            }

                            sampleIdct += Cu * cosX * innerSum;
                        }

                        sampleIdct /= 4.0;
                        IDCT[(8 * y) + x] = sampleIdct;
                    }
                }

                //copy the IDCT values
                for (Int32 index = 0; index < 64; ++index)
                    fDctList[blockStart + index] = Convert.ToInt32(IDCT[index]);
            }
        }

        /// <summary>Performs the inverse DCT on the list of FDCT values</summary>
        /// <param name="fDctList">List of integer Forward DCT values</param>
        /// <remarks>See JPEG specification, §A.3.3</remarks>
        [Obsolete("Does not work")]
        private static void FailedInverseDiscreteCosineTransformSlowSwapXY(IList<Int32> fDctList)
        {
            Double[] IDCT = new Double[64];
            Double inverseRoot2 = 1.0 / Math.Sqrt(2.0);
            Double sixteenthPi = Math.PI / 16.0;

            for (Int32 blockStart = 0; blockStart < fDctList.Count; blockStart += 64) //each 8x8 sample block
            {
                for (Int32 y = 0; y < 8; ++y)   //sample Y co-ordinate
                {

                    for (Int32 x = 0; x < 8; ++x)   //sample X co-ordinate
                    {
                        Double yBase = Convert.ToDouble((2 * x) + 1);
                        Double xBase = Convert.ToDouble((2 * y) + 1);
                        Int32 dctValue = fDctList[blockStart + ((8 * x) + y)];
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

                                sampleIdct += (Cu * Cv * dctValue * cosX * cosY);
                            }
                        }

                        sampleIdct /= 4.0;
                        IDCT[(8 * x) + y] = sampleIdct;
                    }
                }

                //copy the IDCT values
                for (Int32 index = 0; index < 64; ++index)
                    fDctList[blockStart + index] = Convert.ToInt32(IDCT[index]);
            }
        }

        /// <summary>Performs the inverse DCT on the list of FDCT values</summary>
        /// <param name="fDctList">List of integer Forward DCT values</param>
        /// <remarks>See JPEG specification, §A.3.3</remarks>
        [Obsolete("Does not work")]
        private static void FailedInverseDiscreteCosineTransformSlowListingReimplementation(IList<Int32> fDctList)
        {
            Double[] IDCT = new Double[64];

            for (Int32 blockStart = 0; blockStart < fDctList.Count; blockStart += 64) //each 8x8 sample block
            {
                for (Int32 y = 0; y < 8; ++y)   //sample Y co-ordinate
                {
                    for (Int32 x = 0; x < 8; ++x)   //sample X co-ordinate
                    {
                        Double z = 0.0;

                        //summations
                        for (Int32 v = 0; v < 8; ++v)
                        {
                            for (Int32 u = 0; u < 8; ++u)   // Horizontal frequency u
                            {
                                Double Cu = (u == 0) ? 1.0 / Math.Sqrt(2.0) : 1.0;
                                Double Cv = (v == 0) ? 1.0 / Math.Sqrt(2.0) : 1.0;
                                Double dctValue = fDctList[blockStart + ((8 * y) + x)];
                                Double q = Cu * Cv * dctValue * Math.Cos(Convert.ToDouble((2 * x) + 1) * Convert.ToDouble(u) * Math.PI / 16.0)
                                    * Math.Cos(Convert.ToDouble((2 * y) + 1) * Convert.ToDouble(v) * Math.PI / 16.0);

                                z += q;
                            }
                        }

                        z /= 4.0;
                        IDCT[(8 * y) + x] = z;
                    }
                }

                //copy the IDCT values
                for (Int32 index = 0; index < 64; ++index)
                    fDctList[blockStart + index] = Convert.ToInt32(IDCT[index]);
            }
        }
        #endregion
    }
}