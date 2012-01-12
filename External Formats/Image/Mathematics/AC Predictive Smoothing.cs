using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Mathematics
{
    /// <summary>Represents the mathematics represented in §K.2.8 of the JPEG specification</summary>
    public static class AmplitudeCoefficientPredictiveSmoothing
    {
        #region Public methods
        /// <summary>Attempts to implement JPEG specification §K.2.8.1</summary>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="blockCountVertical">Vertical block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        /// <param name="performOnBorders">Boolean flag indicating whether to perform the operation on corners and outer rows and columns</param>
        /// <remarks>
        ///     My thanks to the following, indicating another divide by 8 (see the IDCT fast implementation). I need to just start dividing by 8 when I get funny results.
        ///     Adaptive AC-Coefficient Prediction for Image Compression and Blind Watermarking
        ///     (K. Veeraswamy, S. Srinivas Kumar)
        ///     http://www.academypublisher.com/jmm/vol03/no01/jmm03011622.pdf
        /// </remarks>
        public static void SmoothingPrediction(Int32 blockCountVertical, Int32 blockCountHorizontal, List<Double> coefficientData, Boolean performOnBorders = false)
        {
            SmoothingPredictionMainBody(blockCountVertical, blockCountHorizontal, coefficientData);

            #region Outliers
            if (performOnBorders)
            {
                //now, do the four corners.
                if (blockCountHorizontal > 1 && blockCountVertical > 1)
                {
                    //top left
                    SmoothingPredictionTopLeftCorner(blockCountHorizontal, coefficientData);


                    //top right
                    SmoothingPredictionTopRightCorner(blockCountHorizontal, coefficientData);


                    //bottom left
                    SmoothingPredictionBottomLeftCorner(blockCountVertical, blockCountHorizontal, coefficientData);


                    //bottom right
                    SmoothingPredictionBottomRightCorner(blockCountVertical, blockCountHorizontal, coefficientData);
                }

                //now do top row
                SmoothingPredictionTopmostRow(blockCountHorizontal, coefficientData);

                //now do bottom row
                SmoothingPredictionBottommostRow(blockCountVertical, blockCountHorizontal, coefficientData);

                //now do left
                SmoothingPredictionLeftmostColumn(blockCountVertical, blockCountHorizontal, coefficientData);

                //now do right
                SmoothingPredictionRightmostColumn(blockCountVertical, blockCountHorizontal, coefficientData);
            }
            #endregion
        }

        /// <summary>Attempts to implement JPEG specification §K.2.8.1</summary>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="blockCountVertical">Vertical block count</param>
        /// <param name="coefficientData">Integer data to adjust in-position</param>
        /// <param name="performOnBorders">Boolean flag indicating whether to perform the operation on corners and outer rows and columns</param>
        /// <remarks>
        ///     My thanks to the following, indicating another divide by 8 (see the IDCT fast implementation). I need to just start dividing by 8 when I get funny results.
        ///     Adaptive AC-Coefficient Prediction for Image Compression and Blind Watermarking
        ///     (K. Veeraswamy, S. Srinivas Kumar)
        ///     http://www.academypublisher.com/jmm/vol03/no01/jmm03011622.pdf
        /// </remarks>
        public static void SmoothingPrediction(Int32 blockCountVertical, Int32 blockCountHorizontal, List<Int32> coefficientData, Boolean performOnBorders = false)
        {
            SmoothingPredictionMainBody(blockCountVertical, blockCountHorizontal, coefficientData);

            #region Outliers
            if (performOnBorders)
            {
                //now, do the four corners.
                if (blockCountHorizontal > 1 && blockCountVertical > 1)
                {
                    //top left
                    SmoothingPredictionTopLeftCorner(blockCountHorizontal, coefficientData);


                    //top right
                    SmoothingPredictionTopRightCorner(blockCountHorizontal, coefficientData);


                    //bottom left
                    SmoothingPredictionBottomLeftCorner(blockCountVertical, blockCountHorizontal, coefficientData);


                    //bottom right
                    SmoothingPredictionBottomRightCorner(blockCountVertical, blockCountHorizontal, coefficientData);
                }

                //now do top row
                SmoothingPredictionTopmostRow(blockCountHorizontal, coefficientData);

                //now do bottom row
                SmoothingPredictionBottommostRow(blockCountVertical, blockCountHorizontal, coefficientData);

                //now do left
                SmoothingPredictionLeftmostColumn(blockCountVertical, blockCountHorizontal, coefficientData);

                //now do right
                SmoothingPredictionRightmostColumn(blockCountVertical, blockCountHorizontal, coefficientData);
            }
            #endregion
        }
        #endregion

        #region Smoothing procedures
        #region Double-precision
        /// <summary>Smooths the main body of the image</summary>
        /// <param name="blockCountVertical">Vertical block count</param>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionMainBody(Int32 blockCountVertical, Int32 blockCountHorizontal, List<Double> coefficientData)
        {
            Int32 block1, block2, block3, block4, block5 /* this one */, block6, block7, block8, block9;
            Double dc1, dc2, dc3, dc4, dc5, dc6, dc7, dc8, dc9;
            Int32 x, y;

            //these look at the neighboring 8 blocks for prediction. do the easy stuff first
            for (y = 1; y < (blockCountVertical - 1); ++y)
            {
                for (x = 1; x < (blockCountHorizontal - 1); ++x)
                {
                    //set up indecies into existing component data
                    block1 = (((y - 1) * blockCountHorizontal) + (x - 1)) * 64;
                    block2 = (((y - 1) * blockCountHorizontal) + (x)) * 64;
                    block3 = (((y - 1) * blockCountHorizontal) + (x + 1)) * 64;
                    block4 = (((y) * blockCountHorizontal) + (x - 1)) * 64;
                    block5 = (((y) * blockCountHorizontal) + (x)) * 64;
                    block6 = (((y) * blockCountHorizontal) + (x + 1)) * 64;
                    block7 = (((y + 1) * blockCountHorizontal) + (x - 1)) * 64;
                    block8 = (((y + 1) * blockCountHorizontal) + (x)) * 64;
                    block9 = (((y + 1) * blockCountHorizontal) + (x + 1)) * 64;

                    dc1 = coefficientData[block1];
                    dc2 = coefficientData[block2];
                    dc3 = coefficientData[block3];
                    dc4 = coefficientData[block4];
                    dc5 = coefficientData[block5];
                    dc6 = coefficientData[block6];
                    dc7 = coefficientData[block7];
                    dc8 = coefficientData[block8];
                    dc9 = coefficientData[block9];

                    //redefine ACs
                    SmoothZZ1(coefficientData, block5, dc4, dc6);
                    SmoothZZ2(coefficientData, block5, dc2, dc8);
                    SmoothZZ3(coefficientData, block5, dc2, dc5, dc8);
                    SmoothZZ4(coefficientData, block5, dc1, dc3, dc7, dc9);
                    SmoothZZ5(coefficientData, block5, dc4, dc5, dc6);
                }
            }
        }

        /// <summary>Smooths the upper left corner of the image</summary>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionTopLeftCorner(Int32 blockCountHorizontal, List<Double> coefficientData)
        {
            //top left; only 5, 6, 8, 9 exist
            Int32 block5 /* this one */, block6, block8, block9;
            Double dc5, dc6, dc8, dc9;
            Int32 x, y;

            x = y = 0;
            block5 = (((y) * blockCountHorizontal) + (x)) * 64;
            block6 = (((y) * blockCountHorizontal) + (x + 1)) * 64;
            block8 = (((y + 1) * blockCountHorizontal) + (x)) * 64;
            block9 = (((y + 1) * blockCountHorizontal) + (x + 1)) * 64;
            dc5 = coefficientData[block5];
            dc6 = coefficientData[block6];
            dc8 = coefficientData[block8];
            dc9 = coefficientData[block9];

            //Weights 2 and 8 vs. 5
            SmoothZZ3(coefficientData, block5, dc8, dc5, dc8);    //double 8 since there is an average of top and bottom, effectively
            SmoothZZ5(coefficientData, block5, dc6, dc5, dc6);    //double 6 since there is an average of left and right, effectively
        }

        /// <summary>Smooths the upper right corner of the image</summary>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionTopRightCorner(Int32 blockCountHorizontal, List<Double> coefficientData)
        {
            //top right; only 4, 5, 7, 8 exist
            Int32 block4, block5 /* this one */, block7, block8;
            Double dc4, dc5, dc7, dc8;
            Int32 x, y;

            x = blockCountHorizontal - 1;
            y = 0;

            block4 = (((y) * blockCountHorizontal) + (x - 1)) * 64;
            block5 = (((y) * blockCountHorizontal) + (x)) * 64;
            block7 = (((y + 1) * blockCountHorizontal) + (x - 1)) * 64;
            block8 = (((y + 1) * blockCountHorizontal) + (x)) * 64;

            dc4 = coefficientData[block4];
            dc5 = coefficientData[block5];
            dc7 = coefficientData[block7];
            dc8 = coefficientData[block8];

            //redefine ACs
            SmoothZZ3(coefficientData, block5, dc8, dc5, dc8);    //double 8 since there is an average of top and bottom, effectively
            SmoothZZ5(coefficientData, block5, dc4, dc5, dc4);    //double 4 since there is an average of left and right, effectively
        }

        /// <summary>Smooths the lower left corner of the image</summary>
        /// <param name="blockCountVertical">Vertical block count</param>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionBottomLeftCorner(Int32 blockCountVertical, Int32 blockCountHorizontal, List<Double> coefficientData)
        {
            //bottom left; only 2, 3, 5, 6 exist
            Int32 block2, block3, block5 /* this one */, block6;
            Double dc2, dc3, dc5, dc6;
            Int32 x, y;

            x = 0;
            y = blockCountVertical - 1;

            block2 = (((y - 1) * blockCountHorizontal) + (x)) * 64;
            block3 = (((y - 1) * blockCountHorizontal) + (x + 1)) * 64;
            block5 = (((y) * blockCountHorizontal) + (x)) * 64;
            block6 = (((y) * blockCountHorizontal) + (x + 1)) * 64;

            dc2 = coefficientData[block2];
            dc3 = coefficientData[block3];
            dc5 = coefficientData[block5];
            dc6 = coefficientData[block6];

            //redefine ACs
            SmoothZZ3(coefficientData, block5, dc2, dc5, dc2);    //double 2 since there is an average of top and bottom, effectively
            SmoothZZ5(coefficientData, block5, dc6, dc5, dc6);    //double 6 since there is an average of left and right, effectively
        }

        /// <summary>Smooths the lower right corner of the image</summary>
        /// <param name="blockCountVertical">Vertical block count</param>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionBottomRightCorner(Int32 blockCountVertical, Int32 blockCountHorizontal, List<Double> coefficientData)
        {
            //bottom right; only 1, 2, 4, 5 exist
            Int32 block1, block2, block4, block5 /* this one */;
            Double dc1, dc2, dc4, dc5;
            Int32 x, y;

            x = blockCountHorizontal - 1;
            y = blockCountVertical - 1;

            block1 = (((y - 1) * blockCountHorizontal) + (x - 1)) * 64;
            block2 = (((y - 1) * blockCountHorizontal) + (x)) * 64;
            block4 = (((y) * blockCountHorizontal) + (x - 1)) * 64;
            block5 = (((y) * blockCountHorizontal) + (x)) * 64;

            dc1 = coefficientData[block1];
            dc2 = coefficientData[block2];
            dc4 = coefficientData[block4];
            dc5 = coefficientData[block5];

            //redefine ACs
            SmoothZZ3(coefficientData, block5, dc2, dc5, dc2);    //double 2 since there is an average of top and bottom, effectively
            SmoothZZ5(coefficientData, block5, dc4, dc5, dc4);    //double 4 since there is an average of left and right, effectively
        }

        /// <summary>Smooths the top-most row of the image</summary>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionTopmostRow(Int32 blockCountHorizontal, List<Double> coefficientData)
        {
            Int32 block4, block5 /* this one */, block6, block7, block8, block9;
            Double dc4, dc5, dc6, dc7, dc8, dc9;
            Int32 x;

            for (x = 1; x < (blockCountHorizontal - 1); ++x)
            {
                //set up indecies into existing component data
                block4 = (x - 1) * 64;
                block5 = (x) * 64;
                block6 = (x + 1) * 64;
                block7 = (x - 1) * 64;
                block8 = (x) * 64;
                block9 = (x + 1) * 64;

                dc4 = coefficientData[block4];
                dc5 = coefficientData[block5];
                dc6 = coefficientData[block6];
                dc7 = coefficientData[block7];
                dc8 = coefficientData[block8];
                dc9 = coefficientData[block9];

                //redefine ACs
                SmoothZZ1(coefficientData, block5, dc4, dc6);
                SmoothZZ3(coefficientData, block5, dc8, dc5, dc8);    //double 8 since there is an average of top and bottom, effectively
                SmoothZZ5(coefficientData, block5, dc4, dc5, dc6);
            }
        }

        /// <summary>Smooths the bottom-most row of the image</summary>
        /// <param name="blockCountVertical">Vertical block count</param>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionBottommostRow(Int32 blockCountVertical, Int32 blockCountHorizontal, List<Double> coefficientData)
        {
            Int32 block1, block2, block3, block4, block5 /* this one */, block6;
            Double dc1, dc2, dc3, dc4, dc5, dc6;
            Int32 x, y;

            y = blockCountVertical - 1;

            for (x = 1; x < (blockCountHorizontal - 1); ++x)
            {
                //set up indecies into existing component data
                block1 = (((y - 1) * blockCountHorizontal) + (x - 1)) * 64;
                block2 = (((y - 1) * blockCountHorizontal) + (x)) * 64;
                block3 = (((y - 1) * blockCountHorizontal) + (x + 1)) * 64;
                block4 = (((y) * blockCountHorizontal) + (x - 1)) * 64;
                block5 = (((y) * blockCountHorizontal) + (x)) * 64;
                block6 = (((y) * blockCountHorizontal) + (x + 1)) * 64;

                dc1 = coefficientData[block1];
                dc2 = coefficientData[block2];
                dc3 = coefficientData[block3];
                dc4 = coefficientData[block4];
                dc5 = coefficientData[block5];
                dc6 = coefficientData[block6];

                //redefine ACs
                SmoothZZ1(coefficientData, block5, dc4, dc6);
                SmoothZZ3(coefficientData, block5, dc2, dc5, dc2);    //double 2 since there is an average of top and bottom, effectively
                SmoothZZ5(coefficientData, block5, dc4, dc5, dc6);
            }
        }

        /// <summary>Smooths the left-most column of the image</summary>
        /// <param name="blockCountVertical">Vertical block count</param>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionLeftmostColumn(Int32 blockCountVertical, Int32 blockCountHorizontal, List<Double> coefficientData)
        {
            Int32 block2, block3, block5 /* this one */, block6, block8, block9;
            Double dc2, dc3, dc5, dc6, dc8, dc9;
            Int32 y;

            for (y = 1; y < (blockCountVertical - 1); ++y)
            {
                //set up indecies into existing component data
                block2 = ((y - 1) * blockCountHorizontal) * 64;
                block3 = ((y - 1) * blockCountHorizontal) * 64;
                block5 = (y * blockCountHorizontal) * 64;
                block6 = (y * blockCountHorizontal) * 64;
                block8 = ((y + 1) * blockCountHorizontal) * 64;
                block9 = ((y + 1) * blockCountHorizontal) * 64;

                dc2 = coefficientData[block2];
                dc3 = coefficientData[block3];
                dc5 = coefficientData[block5];
                dc6 = coefficientData[block6];
                dc8 = coefficientData[block8];
                dc9 = coefficientData[block9];

                //redefine ACs
                SmoothZZ2(coefficientData, block5, dc2, dc8);
                SmoothZZ3(coefficientData, block5, dc2, dc5, dc8);
                SmoothZZ5(coefficientData, block5, dc6, dc5, dc6);    //double 6 since there is an average of left and right, effectively
            }
        }

        /// <summary>Smooths the right-most column of the image</summary>
        /// <param name="blockCountVertical">Vertical block count</param>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionRightmostColumn(Int32 blockCountVertical, Int32 blockCountHorizontal, List<Double> coefficientData)
        {
            Int32 block1, block2, block4, block5 /* this one */, block7, block8;
            Double dc1, dc2, dc4, dc5, dc7, dc8;
            Int32 x, y;

            x = blockCountHorizontal - 1;

            for (y = 1; y < (blockCountVertical - 1); ++y)
            {
                //set up indecies into existing component data
                block1 = (((y - 1) * blockCountHorizontal) + (x - 1)) * 64;
                block2 = (((y - 1) * blockCountHorizontal) + (x)) * 64;
                block4 = (((y) * blockCountHorizontal) + (x - 1)) * 64;
                block5 = (((y) * blockCountHorizontal) + (x)) * 64;
                block7 = (((y + 1) * blockCountHorizontal) + (x - 1)) * 64;
                block8 = (((y + 1) * blockCountHorizontal) + (x)) * 64;

                dc1 = coefficientData[block1];
                dc2 = coefficientData[block2];
                dc4 = coefficientData[block4];
                dc5 = coefficientData[block5];
                dc7 = coefficientData[block7];
                dc8 = coefficientData[block8];

                //redefine ACs
                SmoothZZ2(coefficientData, block5, dc2, dc8);
                SmoothZZ3(coefficientData, block5, dc2, dc5, dc8);
                SmoothZZ5(coefficientData, block5, dc4, dc5, dc4);    //double 4 since there is an average of left and right, effectively
            }
        }
        #endregion

        #region Integer
        /// <summary>Smooths the main body of the image</summary>
        /// <param name="blockCountVertical">Vertical block count</param>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionMainBody(Int32 blockCountVertical, Int32 blockCountHorizontal, List<Int32> coefficientData)
        {
            Int32 block1, block2, block3, block4, block5 /* this one */, block6, block7, block8, block9;
            Int32 dc1, dc2, dc3, dc4, dc5, dc6, dc7, dc8, dc9;
            Int32 x, y;

            //these look at the neighboring 8 blocks for prediction. do the easy stuff first
            for (y = 1; y < (blockCountVertical - 1); ++y)
            {
                for (x = 1; x < (blockCountHorizontal - 1); ++x)
                {
                    //set up indecies into existing component data
                    block1 = (((y - 1) * blockCountHorizontal) + (x - 1)) * 64;
                    block2 = (((y - 1) * blockCountHorizontal) + (x)) * 64;
                    block3 = (((y - 1) * blockCountHorizontal) + (x + 1)) * 64;
                    block4 = (((y) * blockCountHorizontal) + (x - 1)) * 64;
                    block5 = (((y) * blockCountHorizontal) + (x)) * 64;
                    block6 = (((y) * blockCountHorizontal) + (x + 1)) * 64;
                    block7 = (((y + 1) * blockCountHorizontal) + (x - 1)) * 64;
                    block8 = (((y + 1) * blockCountHorizontal) + (x)) * 64;
                    block9 = (((y + 1) * blockCountHorizontal) + (x + 1)) * 64;

                    dc1 = coefficientData[block1];
                    dc2 = coefficientData[block2];
                    dc3 = coefficientData[block3];
                    dc4 = coefficientData[block4];
                    dc5 = coefficientData[block5];
                    dc6 = coefficientData[block6];
                    dc7 = coefficientData[block7];
                    dc8 = coefficientData[block8];
                    dc9 = coefficientData[block9];

                    //redefine ACs
                    SmoothZZ1(coefficientData, block5, dc4, dc6);
                    SmoothZZ2(coefficientData, block5, dc2, dc8);
                    SmoothZZ3(coefficientData, block5, dc2, dc5, dc8);
                    SmoothZZ4(coefficientData, block5, dc1, dc3, dc7, dc9);
                    SmoothZZ5(coefficientData, block5, dc4, dc5, dc6);
                }
            }
        }

        /// <summary>Smooths the upper left corner of the image</summary>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionTopLeftCorner(Int32 blockCountHorizontal, List<Int32> coefficientData)
        {
            //top left; only 5, 6, 8, 9 exist
            Int32 block5 /* this one */, block6, block8, block9;
            Int32 dc5, dc6, dc8, dc9;
            Int32 x, y;

            x = y = 0;
            block5 = (((y) * blockCountHorizontal) + (x)) * 64;
            block6 = (((y) * blockCountHorizontal) + (x + 1)) * 64;
            block8 = (((y + 1) * blockCountHorizontal) + (x)) * 64;
            block9 = (((y + 1) * blockCountHorizontal) + (x + 1)) * 64;
            dc5 = coefficientData[block5];
            dc6 = coefficientData[block6];
            dc8 = coefficientData[block8];
            dc9 = coefficientData[block9];

            //Weights 2 and 8 vs. 5
            SmoothZZ3(coefficientData, block5, dc8, dc5, dc8);    //double 8 since there is an average of top and bottom, effectively
            SmoothZZ5(coefficientData, block5, dc6, dc5, dc6);    //double 6 since there is an average of left and right, effectively
        }

        /// <summary>Smooths the upper right corner of the image</summary>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionTopRightCorner(Int32 blockCountHorizontal, List<Int32> coefficientData)
        {
            //top right; only 4, 5, 7, 8 exist
            Int32 block4, block5 /* this one */, block7, block8;
            Int32 dc4, dc5, dc7, dc8;
            Int32 x, y;

            x = blockCountHorizontal - 1;
            y = 0;

            block4 = (((y) * blockCountHorizontal) + (x - 1)) * 64;
            block5 = (((y) * blockCountHorizontal) + (x)) * 64;
            block7 = (((y + 1) * blockCountHorizontal) + (x - 1)) * 64;
            block8 = (((y + 1) * blockCountHorizontal) + (x)) * 64;

            dc4 = coefficientData[block4];
            dc5 = coefficientData[block5];
            dc7 = coefficientData[block7];
            dc8 = coefficientData[block8];

            //redefine ACs
            SmoothZZ3(coefficientData, block5, dc8, dc5, dc8);    //double 8 since there is an average of top and bottom, effectively
            SmoothZZ5(coefficientData, block5, dc4, dc5, dc4);    //double 4 since there is an average of left and right, effectively
        }

        /// <summary>Smooths the lower left corner of the image</summary>
        /// <param name="blockCountVertical">Vertical block count</param>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionBottomLeftCorner(Int32 blockCountVertical, Int32 blockCountHorizontal, List<Int32> coefficientData)
        {
            //bottom left; only 2, 3, 5, 6 exist
            Int32 block2, block3, block5 /* this one */, block6;
            Int32 dc2, dc3, dc5, dc6;
            Int32 x, y;

            x = 0;
            y = blockCountVertical - 1;

            block2 = (((y - 1) * blockCountHorizontal) + (x)) * 64;
            block3 = (((y - 1) * blockCountHorizontal) + (x + 1)) * 64;
            block5 = (((y) * blockCountHorizontal) + (x)) * 64;
            block6 = (((y) * blockCountHorizontal) + (x + 1)) * 64;

            dc2 = coefficientData[block2];
            dc3 = coefficientData[block3];
            dc5 = coefficientData[block5];
            dc6 = coefficientData[block6];

            //redefine ACs
            SmoothZZ3(coefficientData, block5, dc2, dc5, dc2);    //double 2 since there is an average of top and bottom, effectively
            SmoothZZ5(coefficientData, block5, dc6, dc5, dc6);    //double 6 since there is an average of left and right, effectively
        }

        /// <summary>Smooths the lower right corner of the image</summary>
        /// <param name="blockCountVertical">Vertical block count</param>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionBottomRightCorner(Int32 blockCountVertical, Int32 blockCountHorizontal, List<Int32> coefficientData)
        {
            //bottom right; only 1, 2, 4, 5 exist
            Int32 block1, block2, block4, block5 /* this one */;
            Int32 dc1, dc2, dc4, dc5;
            Int32 x, y;

            x = blockCountHorizontal - 1;
            y = blockCountVertical - 1;

            block1 = (((y - 1) * blockCountHorizontal) + (x - 1)) * 64;
            block2 = (((y - 1) * blockCountHorizontal) + (x)) * 64;
            block4 = (((y) * blockCountHorizontal) + (x - 1)) * 64;
            block5 = (((y) * blockCountHorizontal) + (x)) * 64;

            dc1 = coefficientData[block1];
            dc2 = coefficientData[block2];
            dc4 = coefficientData[block4];
            dc5 = coefficientData[block5];

            //redefine ACs
            SmoothZZ3(coefficientData, block5, dc2, dc5, dc2);    //double 2 since there is an average of top and bottom, effectively
            SmoothZZ5(coefficientData, block5, dc4, dc5, dc4);    //double 4 since there is an average of left and right, effectively
        }

        /// <summary>Smooths the top-most row of the image</summary>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionTopmostRow(Int32 blockCountHorizontal, List<Int32> coefficientData)
        {
            Int32 block4, block5 /* this one */, block6, block7, block8, block9;
            Int32 dc4, dc5, dc6, dc7, dc8, dc9;
            Int32 x;

            for (x = 1; x < (blockCountHorizontal - 1); ++x)
            {
                //set up indecies into existing component data
                block4 = (x - 1) * 64;
                block5 = (x) * 64;
                block6 = (x + 1) * 64;
                block7 = (x - 1) * 64;
                block8 = (x) * 64;
                block9 = (x + 1) * 64;

                dc4 = coefficientData[block4];
                dc5 = coefficientData[block5];
                dc6 = coefficientData[block6];
                dc7 = coefficientData[block7];
                dc8 = coefficientData[block8];
                dc9 = coefficientData[block9];

                //redefine ACs
                SmoothZZ1(coefficientData, block5, dc4, dc6);
                SmoothZZ3(coefficientData, block5, dc8, dc5, dc8);    //double 8 since there is an average of top and bottom, effectively
                SmoothZZ5(coefficientData, block5, dc4, dc5, dc6);
            }
        }

        /// <summary>Smooths the bottom-most row of the image</summary>
        /// <param name="blockCountVertical">Vertical block count</param>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionBottommostRow(Int32 blockCountVertical, Int32 blockCountHorizontal, List<Int32> coefficientData)
        {
            Int32 block1, block2, block3, block4, block5 /* this one */, block6;
            Int32 dc1, dc2, dc3, dc4, dc5, dc6;
            Int32 x, y;

            y = blockCountVertical - 1;

            for (x = 1; x < (blockCountHorizontal - 1); ++x)
            {
                //set up indecies into existing component data
                block1 = (((y - 1) * blockCountHorizontal) + (x - 1)) * 64;
                block2 = (((y - 1) * blockCountHorizontal) + (x)) * 64;
                block3 = (((y - 1) * blockCountHorizontal) + (x + 1)) * 64;
                block4 = (((y) * blockCountHorizontal) + (x - 1)) * 64;
                block5 = (((y) * blockCountHorizontal) + (x)) * 64;
                block6 = (((y) * blockCountHorizontal) + (x + 1)) * 64;

                dc1 = coefficientData[block1];
                dc2 = coefficientData[block2];
                dc3 = coefficientData[block3];
                dc4 = coefficientData[block4];
                dc5 = coefficientData[block5];
                dc6 = coefficientData[block6];

                //redefine ACs
                SmoothZZ1(coefficientData, block5, dc4, dc6);
                SmoothZZ3(coefficientData, block5, dc2, dc5, dc2);    //double 2 since there is an average of top and bottom, effectively
                SmoothZZ5(coefficientData, block5, dc4, dc5, dc6);
            }
        }

        /// <summary>Smooths the left-most column of the image</summary>
        /// <param name="blockCountVertical">Vertical block count</param>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionLeftmostColumn(Int32 blockCountVertical, Int32 blockCountHorizontal, List<Int32> coefficientData)
        {
            Int32 block2, block3, block5 /* this one */, block6, block8, block9;
            Int32 dc2, dc3, dc5, dc6, dc8, dc9;
            Int32 y;

            for (y = 1; y < (blockCountVertical - 1); ++y)
            {
                //set up indecies into existing component data
                block2 = ((y - 1) * blockCountHorizontal) * 64;
                block3 = ((y - 1) * blockCountHorizontal) * 64;
                block5 = (y * blockCountHorizontal) * 64;
                block6 = (y * blockCountHorizontal) * 64;
                block8 = ((y + 1) * blockCountHorizontal) * 64;
                block9 = ((y + 1) * blockCountHorizontal) * 64;

                dc2 = coefficientData[block2];
                dc3 = coefficientData[block3];
                dc5 = coefficientData[block5];
                dc6 = coefficientData[block6];
                dc8 = coefficientData[block8];
                dc9 = coefficientData[block9];

                //redefine ACs
                SmoothZZ2(coefficientData, block5, dc2, dc8);
                SmoothZZ3(coefficientData, block5, dc2, dc5, dc8);
                SmoothZZ5(coefficientData, block5, dc6, dc5, dc6);    //double 6 since there is an average of left and right, effectively
            }
        }

        /// <summary>Smooths the right-most column of the image</summary>
        /// <param name="blockCountVertical">Vertical block count</param>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionRightmostColumn(Int32 blockCountVertical, Int32 blockCountHorizontal, List<Int32> coefficientData)
        {
            Int32 block1, block2, block4, block5 /* this one */, block7, block8;
            Int32 dc1, dc2, dc4, dc5, dc7, dc8;
            Int32 x, y;

            x = blockCountHorizontal - 1;

            for (y = 1; y < (blockCountVertical - 1); ++y)
            {
                //set up indecies into existing component data
                block1 = (((y - 1) * blockCountHorizontal) + (x - 1)) * 64;
                block2 = (((y - 1) * blockCountHorizontal) + (x)) * 64;
                block4 = (((y) * blockCountHorizontal) + (x - 1)) * 64;
                block5 = (((y) * blockCountHorizontal) + (x)) * 64;
                block7 = (((y + 1) * blockCountHorizontal) + (x - 1)) * 64;
                block8 = (((y + 1) * blockCountHorizontal) + (x)) * 64;

                dc1 = coefficientData[block1];
                dc2 = coefficientData[block2];
                dc4 = coefficientData[block4];
                dc5 = coefficientData[block5];
                dc7 = coefficientData[block7];
                dc8 = coefficientData[block8];

                //redefine ACs
                SmoothZZ2(coefficientData, block5, dc2, dc8);
                SmoothZZ3(coefficientData, block5, dc2, dc5, dc8);
                SmoothZZ5(coefficientData, block5, dc4, dc5, dc4);    //double 4 since there is an average of left and right, effectively
            }
        }
        #endregion
        #endregion

        #region Helper Methods
        #region Double-Precision
        /// <summary>Smooths AC1 in zig-zag order</summary>
        /// <param name="coefficientData">Data List to adjust</param>
        /// <param name="currentBlockIndex">Index to the start of the current block</param>
        /// <param name="dc4">DC coefficient of the left block</param>
        /// <param name="dc6">DC coefficient of the right block</param>
        private static void SmoothZZ1(List<Double> coefficientData, Int32 currentBlockIndex, Double dc4, Double dc6)
        {
            if (coefficientData[currentBlockIndex + 1] == 0.0)   //AC 0,1
                coefficientData[currentBlockIndex + 1] = (1.13885 * (dc4 - dc6)) / 8.0;
        }

        /// <summary>Smooths AC2 in zig-zag order</summary>
        /// <param name="coefficientData">Data List to adjust</param>
        /// <param name="currentBlockIndex">Index to the start of the current block</param>
        /// <param name="dc2">DC coefficient of the above block</param>
        /// <param name="dc8">DC coefficient of the below block</param>
        private static void SmoothZZ2(List<Double> coefficientData, Int32 currentBlockIndex, Double dc2, Double dc8)
        {
            if (coefficientData[currentBlockIndex + 8] == 0.0)   //AC 1,0
                coefficientData[currentBlockIndex + 8] = (1.13885 * (dc2 - dc8)) / 8.0;
        }

        /// <summary>Smooths AC3 in zig-zag order</summary>
        /// <param name="coefficientData">Data List to adjust</param>
        /// <param name="currentBlockIndex">Index to the start of the current block</param>
        /// <param name="dc2">DC coefficient of the above block</param>
        /// <param name="dc5">DC coefficient of the current block</param>
        /// <param name="dc8">DC coefficient of the below block</param>
        private static void SmoothZZ3(List<Double> coefficientData, Int32 currentBlockIndex, Double dc2, Double dc5, Double dc8)
        {
            if (coefficientData[currentBlockIndex + 16] == 0.0)  //AC 2,0
                coefficientData[currentBlockIndex + 16] = (0.27881 * ((dc2 + dc8) - (2 * dc5))) / 8.0;
        }

        /// <summary>Smooths AC3 in zig-zag order</summary>
        /// <param name="coefficientData">Data List to adjust</param>
        /// <param name="currentBlockIndex">Index to the start of the current block</param>
        /// <param name="dc1">DC coefficient of the upper left block</param>
        /// <param name="dc3">DC coefficient of the upper right block</param>
        /// <param name="dc7">DC coefficient of the lower left block</param>
        /// <param name="dc9">DC coefficient of the lower right block</param>
        private static void SmoothZZ4(List<Double> coefficientData, Int32 currentBlockIndex, Double dc1, Double dc3, Double dc7, Double dc9)
        {
            if (coefficientData[currentBlockIndex + 9] == 0.0)   //AC 1,1
                coefficientData[currentBlockIndex + 9] = (0.16213 * ((dc1 - dc3) - (dc7 - dc9))) / 8.0;
        }

        /// <summary>Smooths AC5 in zig-zag order</summary>
        /// <param name="coefficientData">Data List to adjust</param>
        /// <param name="currentBlockIndex">Index to the start of the current block</param>
        /// <param name="dc4">DC coefficient of the left block</param>
        /// <param name="dc5">DC coefficient of the current block</param>
        /// <param name="dc6">DC coefficient of the right block</param>
        private static void SmoothZZ5(List<Double> coefficientData, Int32 currentBlockIndex, Double dc4, Double dc5, Double dc6)
        {
            if (coefficientData[currentBlockIndex + 2] == 0.0)   //AC 0,2
                coefficientData[currentBlockIndex + 2] = (0.27881 * ((dc4 + dc6) - (2 * dc5))) / 8.0;
        }
        #endregion

        #region Integer
        /// <summary>Smooths AC1 in zig-zag order</summary>
        /// <param name="coefficientData">Data List to adjust</param>
        /// <param name="currentBlockIndex">Index to the start of the current block</param>
        /// <param name="dc4">DC coefficient of the left block</param>
        /// <param name="dc6">DC coefficient of the right block</param>
        private static void SmoothZZ1(List<Int32> coefficientData, Int32 currentBlockIndex, Int32 dc4, Int32 dc6)
        {
            if (coefficientData[currentBlockIndex + 1] == 0)   //AC 0,1
                coefficientData[currentBlockIndex + 1] = Convert.ToInt32((1.13885 * (dc4 - dc6)) / 8.0);
        }

        /// <summary>Smooths AC2 in zig-zag order</summary>
        /// <param name="coefficientData">Data List to adjust</param>
        /// <param name="currentBlockIndex">Index to the start of the current block</param>
        /// <param name="dc2">DC coefficient of the above block</param>
        /// <param name="dc8">DC coefficient of the below block</param>
        private static void SmoothZZ2(List<Int32> coefficientData, Int32 currentBlockIndex, Int32 dc2, Int32 dc8)
        {
            if (coefficientData[currentBlockIndex + 8] == 0)   //AC 1,0
                coefficientData[currentBlockIndex + 8] = Convert.ToInt32((1.13885 * (dc2 - dc8)) / 8.0);
        }

        /// <summary>Smooths AC3 in zig-zag order</summary>
        /// <param name="coefficientData">Data List to adjust</param>
        /// <param name="currentBlockIndex">Index to the start of the current block</param>
        /// <param name="dc2">DC coefficient of the above block</param>
        /// <param name="dc5">DC coefficient of the current block</param>
        /// <param name="dc8">DC coefficient of the below block</param>
        private static void SmoothZZ3(List<Int32> coefficientData, Int32 currentBlockIndex, Int32 dc2, Int32 dc5, Int32 dc8)
        {
            if (coefficientData[currentBlockIndex + 16] == 0)  //AC 2,0
                coefficientData[currentBlockIndex + 16] = Convert.ToInt32((0.27881 * ((dc2 + dc8) - (2 * dc5))) / 8.0);
        }

        /// <summary>Smooths AC3 in zig-zag order</summary>
        /// <param name="coefficientData">Data List to adjust</param>
        /// <param name="currentBlockIndex">Index to the start of the current block</param>
        /// <param name="dc1">DC coefficient of the upper left block</param>
        /// <param name="dc3">DC coefficient of the upper right block</param>
        /// <param name="dc7">DC coefficient of the lower left block</param>
        /// <param name="dc9">DC coefficient of the lower right block</param>
        private static void SmoothZZ4(List<Int32> coefficientData, Int32 currentBlockIndex, Int32 dc1, Int32 dc3, Int32 dc7, Int32 dc9)
        {
            if (coefficientData[currentBlockIndex + 9] == 0)   //AC 1,1
                coefficientData[currentBlockIndex + 9] = Convert.ToInt32((0.16213 * ((dc1 - dc3) - (dc7 - dc9))) / 8.0);
        }

        /// <summary>Smooths AC5 in zig-zag order</summary>
        /// <param name="coefficientData">Data List to adjust</param>
        /// <param name="currentBlockIndex">Index to the start of the current block</param>
        /// <param name="dc4">DC coefficient of the left block</param>
        /// <param name="dc5">DC coefficient of the current block</param>
        /// <param name="dc6">DC coefficient of the right block</param>
        private static void SmoothZZ5(List<Int32> coefficientData, Int32 currentBlockIndex, Int32 dc4, Int32 dc5, Int32 dc6)
        {
            if (coefficientData[currentBlockIndex + 2] == 0)   //AC 0,2
                coefficientData[currentBlockIndex + 2] = Convert.ToInt32((0.27881 * ((dc4 + dc6) - (2 * dc5))) / 8.0);
        }
        #endregion
        #endregion
    }
}