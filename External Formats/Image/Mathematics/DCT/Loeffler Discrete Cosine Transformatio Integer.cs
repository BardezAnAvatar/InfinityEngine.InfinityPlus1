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
    public static class LoefflerDiscreteCosineTransformationInteger
    {
        #region Inverse Discrete Cosine Transform
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
        public static List<Int32> InverseDiscreteCosineTransformInteger(IList<Int32> fDctList)
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
                        LoefflerDiscreteCosineTransformationInteger.InverseDiscreteCosineTransformFastRowInteger(
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
                        LoefflerDiscreteCosineTransformationInteger.InverseDiscreteCosineTransformFastRowInteger(
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
        public static void InverseDiscreteCosineTransformInteger(Int32[,][] fDctList)
        {
            Int32 width = fDctList.GetLength(0), height = fDctList.GetLength(1);

            //loop through each block
            for (Int32 x = 0; x < width; ++x)
                for (Int32 y = 0; y < height; ++y)
                {
                    Int32[] fdct = fDctList[x, y];  //fewer de-referencing accessors?
                    Int32[] idctTemp = new Int32[64];
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
                            LoefflerDiscreteCosineTransformationInteger.InverseDiscreteCosineTransformFastRowInteger(
                                out idctTemp[rowBase], out idctTemp[rowBase + 1], out idctTemp[rowBase + 2], out idctTemp[rowBase + 3],
                                out idctTemp[rowBase + 4], out idctTemp[rowBase + 5], out idctTemp[rowBase + 6], out idctTemp[rowBase + 7],
                                fdct[rowBase], fdct[rowBase + 1], fdct[rowBase + 2], fdct[rowBase + 3],
                                fdct[rowBase + 4], fdct[rowBase + 5], fdct[rowBase + 6], fdct[rowBase + 7]);
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
                            LoefflerDiscreteCosineTransformationInteger.InverseDiscreteCosineTransformFastRowInteger(
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

                    fDctList[x, y] = idctTemp;
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
        private static void InverseDiscreteCosineTransformFastRowInteger(
            out Int32 o0, out Int32 o1, out Int32 o2, out Int32 o3, out Int32 o4, out Int32 o5, out Int32 o6, out Int32 o7,
            Int32 in0, Int32 in1, Int32 in2, Int32 in3, Int32 in4, Int32 in5, Int32 in6, Int32 in7)
        {
            //temporary 'registers' to reorder the input
            Int32 reg0 = in0, reg1 = in4, reg2 = in2, reg3 = in6, reg4 = in7, reg5 = in3, reg6 = in5, reg7 = in1;

            /* phase 1, as some call it */
            o7 = Convert.ToInt32(reg7 * Constants.RootHalf);
            o4 = Convert.ToInt32(reg4 * Constants.RootHalf);
            LoefflerDiscreteCosineTransformationInteger.ButterflyInteger(out o7, out o4, o7, o4); //the butterfly right after is easy to miss in the Bukhari, Kuzmanov and Vassiliadis paper / PDF

            //butterfly
            LoefflerDiscreteCosineTransformationInteger.ButterflyInteger(out o0, out o1, reg0, reg1);

            //rotate 2, 6
            LoefflerDiscreteCosineTransformationInteger.RotateInteger(out o2, out o3, reg2, reg3, Constants.RotateC6, Constants.RotateS6);
            /* end phase 1 */


            /* 'phase' 2 */
            LoefflerDiscreteCosineTransformationInteger.ButterflyInteger(out o0, out o3, o0, o3);
            LoefflerDiscreteCosineTransformationInteger.ButterflyInteger(out o1, out o2, o1, o2);
            LoefflerDiscreteCosineTransformationInteger.ButterflyInteger(out o4, out o6, o4, reg6);
            LoefflerDiscreteCosineTransformationInteger.ButterflyInteger(out o7, out o5, o7, reg5);
            /* end 'phase' 2 */


            /* phase 3, rotation, could be part of phase 2, still, I guess */
            LoefflerDiscreteCosineTransformationInteger.RotateInteger(out o5, out o6, o5, o6, Constants.RotateC1, Constants.RotateS1);
            LoefflerDiscreteCosineTransformationInteger.RotateInteger(out o4, out o7, o4, o7, Constants.RotateC3, Constants.RotateS3);
            /* end phase 3 */


            /* phase 4, final butterfly */
            LoefflerDiscreteCosineTransformationInteger.ButterflyInteger(out o0, out o7, o0, o7);
            LoefflerDiscreteCosineTransformationInteger.ButterflyInteger(out o1, out o6, o1, o6);
            LoefflerDiscreteCosineTransformationInteger.ButterflyInteger(out o2, out o5, o2, o5);
            LoefflerDiscreteCosineTransformationInteger.ButterflyInteger(out o3, out o4, o3, o4);
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
    }
}