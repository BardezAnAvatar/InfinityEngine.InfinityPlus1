using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Mathematics.DCT
{
    /// <summary>Represents a collection of static methods that perform the fast Loeffler DCT and IDCT routines.</summary>
    /// <remarks>
    ///     The 'fast' Loeffler algorithms are taken from a number of sources:
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
    public static class LoefflerDiscreteCosineTransformationFloat
    {
        #region Inverse Discrete Cosine Transform
        /// <summary>Performs the inverse DCT on the list of FDCT values</summary>
        /// <param name="fDctList">List of double-precision floating-point Forward DCT values</param>
        /// <returns>A List of double-precision floating-point values of from the inverse DCT</returns>
        /// <remarks>
        ///     See JPEG specification, §A.3.3
        ///     More to point, see http://ce.et.tudelft.nl/~george/publications/Conf/ProRISC02/DCT.pdf for my source of the 'fast' IDCT.
        ///     http://www.google.com/url?sa=t&rct=j&q=modieifed%20loeffler%20algorithm&source=web&cd=6&ved=0CFYQFjAF&url=http%3A%2F%2Fciteseerx.ist.psu.edu%2Fviewdoc%2Fdownload%3Fdoi%3D10.1.1.157.9472%26rep%3Drep1%26type%3Dpdf&ei=470JT87TAY7eggeTyfGbAg&usg=AFQjCNG_PsMSUdzUPkfno6rfa6UPnyc7vQ&cad=rja
        ///     Also, see http://www.cmlab.csie.ntu.edu.tw/cml/dsp/training/coding/transform/fft.html.
        ///     It loses accuracy, but processes much more quickly.
        /// </remarks>
        public static List<Double> InverseDiscreteCosineTransformFloat(IList<Double> fDctList)
        {
            List<Double> idct = new List<Double>();

            //loop through each block
            for (Int32 baseIndex = 0; baseIndex < fDctList.Count; baseIndex += 64)
            {
                Double[] idctTemp = new Double[64];
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
                        LoefflerDiscreteCosineTransformationFloat.InverseDiscreteCosineTransformFastRowFloat(
                            out idctTemp[rowBase], out idctTemp[rowBase + 1], out idctTemp[rowBase + 2], out idctTemp[rowBase + 3],
                            out idctTemp[rowBase + 4], out idctTemp[rowBase + 5], out idctTemp[rowBase + 6], out idctTemp[rowBase + 7],
                            fDctList[sourceBase], fDctList[sourceBase + 1], fDctList[sourceBase + 2], fDctList[sourceBase + 3],
                            fDctList[sourceBase + 4], fDctList[sourceBase + 5], fDctList[sourceBase + 6], fDctList[sourceBase + 7]);
                    }
                }

                //second pass on columns
                for (Int32 column = 0; column < 8; ++column)
                {
                    Int32 row2 = column + 8, row3 = column + 16, row4 = column + 24, row5 = column + 32, row6 = column + 40, row7 = column + 48, row8 = column + 56;

                    //if 0...7 are all 0, all the cross addition/multiplication comes out to 0 anyway.
                    if (idctTemp[column] == 0 && idctTemp[row2] == 0 && idctTemp[row3] == 0 && idctTemp[row4] == 0
                        && idctTemp[row5] == 0 && idctTemp[row6] == 0 && idctTemp[row7] == 0 && idctTemp[row8] == 0)
                        continue;   //idctTemp is already initialized to 0.
                    else
                    {
                        LoefflerDiscreteCosineTransformationFloat.InverseDiscreteCosineTransformFastRowFloat(
                            out idctTemp[column], out idctTemp[row2], out idctTemp[row3], out idctTemp[row4],
                            out idctTemp[row5], out idctTemp[row6], out idctTemp[row7], out idctTemp[row8],
                            idctTemp[column], idctTemp[row2], idctTemp[row3], idctTemp[row4],
                            idctTemp[row5], idctTemp[row6], idctTemp[row7], idctTemp[row8]);
                    }
                }

                //third pass; the values need to be de-scaled by 8.
                //man, this took a while to figure out.
                //I can't tell if this is commonly left out in mathematical papers as obvious, nor
                //can I tell if it i a needed division of 1/(2*root(2)) after each pass, or if it is
                //a needed division of N=8.
                // Note: Later found out it is 1 / (2 root 2).
                for (Int32 descaleIndex = 0; descaleIndex < 64; ++descaleIndex)
                    idct.Add(idctTemp[descaleIndex] / 8);
            }

            return idct;
        }


        /// <summary>Performs the inverse DCT on the list of FDCT values</summary>
        /// <param name="fDctList">List of double-precision floating-point Forward DCT values</param>
        /// <returns>A List of double-precision floating-point values of from the inverse DCT</returns>
        /// <remarks>
        ///     See JPEG specification, §A.3.3
        ///     More to point, see http://ce.et.tudelft.nl/~george/publications/Conf/ProRISC02/DCT.pdf for my source of the 'fast' IDCT.
        ///     http://www.google.com/url?sa=t&rct=j&q=modieifed%20loeffler%20algorithm&source=web&cd=6&ved=0CFYQFjAF&url=http%3A%2F%2Fciteseerx.ist.psu.edu%2Fviewdoc%2Fdownload%3Fdoi%3D10.1.1.157.9472%26rep%3Drep1%26type%3Dpdf&ei=470JT87TAY7eggeTyfGbAg&usg=AFQjCNG_PsMSUdzUPkfno6rfa6UPnyc7vQ&cad=rja
        ///     Also, see http://www.cmlab.csie.ntu.edu.tw/cml/dsp/training/coding/transform/fft.html.
        ///     It loses accuracy, but processes much more quickly.
        /// </remarks>
        public static void InverseDiscreteCosineTransformFloat(Double[,][] fDctList)
        {
            Int32 width = fDctList.GetLength(0), height = fDctList.GetLength(1);

            //loop through each block
            for (Int32 x = 0; x < width; ++x)
                for (Int32 y = 0; y < height; ++y)
                {
                    Double[] fdct = fDctList[x, y];  //fewer de-referencing accessors?
                    Double[] idctTemp = new Double[64];
                    //do a frst pass on the row, then a second on the columns
                    for (Int32 row = 0; row < 8; ++row)
                    {
                        Int32 rowBase = row * 8;
                        //if 0...7 are all 0, all the cross addition/multiplication comes out to 0 anyway.
                        if (fdct[rowBase] == 0 && fdct[rowBase + 1] == 0 && fdct[rowBase + 2] == 0 && fdct[rowBase + 3] == 0
                            && fdct[rowBase + 4] == 0 && fdct[rowBase + 5] == 0 && fdct[rowBase + 6] == 0 && fdct[rowBase + 7] == 0)
                            continue;   //idctTemp is already initialized to 0.
                        else
                        {
                            LoefflerDiscreteCosineTransformationFloat.InverseDiscreteCosineTransformFastRowFloat(
                                out idctTemp[rowBase], out idctTemp[rowBase + 1], out idctTemp[rowBase + 2], out idctTemp[rowBase + 3],
                                out idctTemp[rowBase + 4], out idctTemp[rowBase + 5], out idctTemp[rowBase + 6], out idctTemp[rowBase + 7],
                                fdct[rowBase], fdct[rowBase + 1], fdct[rowBase + 2], fdct[rowBase + 3],
                                fdct[rowBase + 4], fdct[rowBase + 5], fdct[rowBase + 6], fdct[rowBase + 7]);
                        }
                    }

                    //second pass on columns
                    for (Int32 column = 0; column < 8; ++column)
                    {
                        Int32 row2 = column + 8, row3 = column + 16, row4 = column + 24, row5 = column + 32, row6 = column + 40, row7 = column + 48, row8 = column + 56;

                        //if 0...7 are all 0, all the cross addition/multiplication comes out to 0 anyway.
                        if (idctTemp[column] == 0 && idctTemp[row2] == 0 && idctTemp[row3] == 0 && idctTemp[row4] == 0
                            && idctTemp[row5] == 0 && idctTemp[row6] == 0 && idctTemp[row7] == 0 && idctTemp[row8] == 0)
                            continue;   //idctTemp is already initialized to 0.
                        else
                        {
                            LoefflerDiscreteCosineTransformationFloat.InverseDiscreteCosineTransformFastRowFloat(
                                out idctTemp[column], out idctTemp[row2], out idctTemp[row3], out idctTemp[row4],
                                out idctTemp[row5], out idctTemp[row6], out idctTemp[row7], out idctTemp[row8],
                                idctTemp[column], idctTemp[row2], idctTemp[row3], idctTemp[row4],
                                idctTemp[row5], idctTemp[row6], idctTemp[row7], idctTemp[row8]);
                        }
                    }

                    //third pass; the values need to be de-scaled by 8.
                    //man, this took a while to figure out.
                    //I can't tell if this is commonly left out in mathematical papers as obvious, nor
                    //can I tell if it i a needed division of 1/(2*root(2)) after each pass, or if it is
                    //a needed division of N=8.
                    // Note: Later found out it is 1 / (2 root 2).
                    for (Int32 descaleIndex = 0; descaleIndex < 64; ++descaleIndex)
                        idctTemp[descaleIndex] /= 8.0;

                    fDctList[x, y] = idctTemp;  //fewer de-referencing accessors?
                }
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
        private static void InverseDiscreteCosineTransformFastRowFloat(
            out Double o0, out Double o1, out Double o2, out Double o3, out Double o4, out Double o5, out Double o6, out Double o7,
            Double in0, Double in1, Double in2, Double in3, Double in4, Double in5, Double in6, Double in7)
        {
            //temporary 'registers' to reorder the input
            Double reg0 = in0, reg1 = in4, reg2 = in2, reg3 = in6, reg4 = in7, reg5 = in3, reg6 = in5, reg7 = in1;

            /* phase 1, as some call it */
            o7 = reg7 * Constants.RootHalf;
            o4 = reg4 * Constants.RootHalf;
            LoefflerDiscreteCosineTransformationFloat.ButterflyFloat(out o7, out o4, o7, o4); //the butterfly right after is easy to miss in the Bukhari, Kuzmanov and Vassiliadis paper / PDF

            //butterfly
            LoefflerDiscreteCosineTransformationFloat.ButterflyFloat(out o0, out o1, reg0, reg1);

            //rotate 2, 6
            LoefflerDiscreteCosineTransformationFloat.RotateFloat(out o2, out o3, reg2, reg3, Constants.RotateC6D, Constants.RotateS6D);
            /* end phase 1 */


            /* 'phase' 2 */
            LoefflerDiscreteCosineTransformationFloat.ButterflyFloat(out o0, out o3, o0, o3);
            LoefflerDiscreteCosineTransformationFloat.ButterflyFloat(out o1, out o2, o1, o2);
            LoefflerDiscreteCosineTransformationFloat.ButterflyFloat(out o4, out o6, o4, reg6);
            LoefflerDiscreteCosineTransformationFloat.ButterflyFloat(out o7, out o5, o7, reg5);
            /* end 'phase' 2 */


            /* phase 3, rotation, could be part of phase 2, still, I guess */
            LoefflerDiscreteCosineTransformationFloat.RotateFloat(out o5, out o6, o5, o6, Constants.RotateC1D, Constants.RotateS1D);
            LoefflerDiscreteCosineTransformationFloat.RotateFloat(out o4, out o7, o4, o7, Constants.RotateC3D, Constants.RotateS3D);
            /* end phase 3 */


            /* phase 4, final butterfly */
            LoefflerDiscreteCosineTransformationFloat.ButterflyFloat(out o0, out o7, o0, o7);
            LoefflerDiscreteCosineTransformationFloat.ButterflyFloat(out o1, out o6, o1, o6);
            LoefflerDiscreteCosineTransformationFloat.ButterflyFloat(out o2, out o5, o2, o5);
            LoefflerDiscreteCosineTransformationFloat.ButterflyFloat(out o3, out o4, o3, o4);
            /* end phase 4*/
        }

        /// <summary>Implements the 'butterfly' operation on the modified Loeffler algorithm</summary>
        /// <param name="o0">Output top parameter</param>
        /// <param name="o1">Output bottom parameter</param>
        /// <param name="i0">Input top parameter</param>
        /// <param name="i1">Input bottom parameter</param>
        private static void ButterflyFloat(out Double o0, out Double o1, Double i0, Double i1)
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
        private static void RotateFloat(out Double o0, out Double o1, Double i0, Double i1, Double cos, Double sin)
        {
            o0 = (i0 * cos) - (i1 * sin);
            o1 = (i0 * sin) + (i1 * cos);
        }
        #endregion
    }
}