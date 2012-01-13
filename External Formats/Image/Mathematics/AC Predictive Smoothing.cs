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
        public static void SmoothingPrediction(Int32 blockCountVertical, Int32 blockCountHorizontal, IList<Double> coefficientData, Boolean performOnBorders = false)
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
        public static void SmoothingPrediction(Int32 blockCountVertical, Int32 blockCountHorizontal, IList<Int32> coefficientData, Boolean performOnBorders = false)
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
        /// <summary>Smooths the main body of the image</summary>
        /// <param name="blockCountVertical">Vertical block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        private static void SmoothingPredictionMainBody(Int32 blockCountVertical, Int32 blockCountHorizontal, Double[,][] coefficientData)
        {
            //these look at the neighboring 8 blocks for prediction. do the easy stuff first
            for (Int32 y = 1; y < (blockCountVertical - 1); ++y)
            {
                for (Int32 x = 1; x < (blockCountHorizontal - 1); ++x)
                {
                    Double dc1 = coefficientData[y - 1, x - 1][0];
                    Double dc2 = coefficientData[y - 1, x][0];
                    Double dc3 = coefficientData[y - 1, x + 1][0];
                    Double dc4 = coefficientData[y, x - 1][0];
                    Double dc6 = coefficientData[y, x + 1][0];
                    Double dc7 = coefficientData[y + 1, x - 1][0];
                    Double dc8 = coefficientData[y + 1, x][0];
                    Double dc9 = coefficientData[y + 1, x + 1][0];

                    //redefine ACs
                    SmoothZZ1(coefficientData[y, x], dc4, dc6);
                    SmoothZZ2(coefficientData[y, x], dc2, dc8);
                    SmoothZZ3(coefficientData[y, x], dc2, dc8);
                    SmoothZZ4(coefficientData[y, x], dc1, dc3, dc7, dc9);
                    SmoothZZ5(coefficientData[y, x], dc4, dc6);
                }
            }
        }

        /// <summary>Smooths the upper left corner of the image</summary>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionTopLeftCorner(Int32 blockCountHorizontal, Double[,][] coefficientData)
        {
            //top left; only 5, 6, 8, 9 exist
            Int32 x = 0, y = 0;
            Double dc6 = coefficientData[y, x + 1][0];
            Double dc8 = coefficientData[y + 1, x][0];

            //Weights 2 and 8 vs. 5
            SmoothZZ3(coefficientData[y, x], dc8, dc8);    //double 8 since there is an average of top and bottom, effectively
            SmoothZZ5(coefficientData[y, x], dc6, dc6);    //double 6 since there is an average of left and right, effectively
        }

        /// <summary>Smooths the upper right corner of the image</summary>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionTopRightCorner(Int32 blockCountHorizontal, Double[,][] coefficientData)
        {
            //top right; only 4, 5, 7, 8 exist
            Int32 x = blockCountHorizontal - 1, y = 0;

            Double dc4 = coefficientData[y, x - 1][0];
            Double dc8 = coefficientData[y + 1, x][0];

            //redefine ACs
            SmoothZZ3(coefficientData[y, x], dc8, dc8);    //double 8 since there is an average of top and bottom, effectively
            SmoothZZ5(coefficientData[y, x], dc4, dc4);    //double 4 since there is an average of left and right, effectively
        }

        /// <summary>Smooths the lower left corner of the image</summary>
        /// <param name="blockCountVertical">Vertical block count</param>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionBottomLeftCorner(Int32 blockCountVertical, Int32 blockCountHorizontal, Double[,][] coefficientData)
        {
            //bottom left; only 2, 3, 5, 6 exist
            Int32 x = 0, y = blockCountVertical - 1;

            Double dc2 = coefficientData[y - 1, x][0];
            Double dc6 = coefficientData[y, x + 1][0];

            //redefine ACs
            SmoothZZ3(coefficientData[y, x], dc2, dc2);    //double 2 since there is an average of top and bottom, effectively
            SmoothZZ5(coefficientData[y, x], dc6, dc6);    //double 6 since there is an average of left and right, effectively
        }

        /// <summary>Smooths the lower right corner of the image</summary>
        /// <param name="blockCountVertical">Vertical block count</param>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionBottomRightCorner(Int32 blockCountVertical, Int32 blockCountHorizontal, Double[,][] coefficientData)
        {
            //bottom right; only 1, 2, 4, 5 exist
            Int32 x = blockCountHorizontal - 1, y = blockCountVertical - 1;

            Double dc2 = coefficientData[y - 1, x][0];
            Double dc4 = coefficientData[y, x - 1][0];

            //redefine ACs
            SmoothZZ3(coefficientData[y, x], dc2, dc2);    //double 2 since there is an average of top and bottom, effectively
            SmoothZZ5(coefficientData[y, x], dc4, dc4);    //double 4 since there is an average of left and right, effectively
        }

        /// <summary>Smooths the top-most row of the image</summary>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionTopmostRow(Int32 blockCountHorizontal, Double[,][] coefficientData)
        {
            for (Int32 x = 1; x < (blockCountHorizontal - 1); ++x)
            {
                //set up indecies into existing component data
                Double dc4 = coefficientData[0, x - 1][0];
                Double dc6 = coefficientData[0, x + 1][0];
                Double dc8 = coefficientData[1, x][0];

                //redefine ACs
                SmoothZZ1(coefficientData[0, x], dc4, dc6);
                SmoothZZ3(coefficientData[0, x], dc8, dc8);    //double 8 since there is an average of top and bottom, effectively
                SmoothZZ5(coefficientData[0, x], dc4, dc6);
            }
        }

        /// <summary>Smooths the bottom-most row of the image</summary>
        /// <param name="blockCountVertical">Vertical block count</param>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionBottommostRow(Int32 blockCountVertical, Int32 blockCountHorizontal, Double[,][] coefficientData)
        {
            Int32 y = blockCountVertical - 1;

            for (Int32 x = 1; x < (blockCountHorizontal - 1); ++x)
            {
                //set up indecies into existing component data
                Double dc2 = coefficientData[y - 1, x][0];
                Double dc4 = coefficientData[y, x - 1][0];
                Double dc6 = coefficientData[y, x + 1][0];

                //redefine ACs
                SmoothZZ1(coefficientData[y, x], dc4, dc6);
                SmoothZZ3(coefficientData[y, x], dc2, dc2);    //double 2 since there is an average of top and bottom, effectively
                SmoothZZ5(coefficientData[y, x], dc4, dc6);
            }
        }

        /// <summary>Smooths the left-most column of the image</summary>
        /// <param name="blockCountVertical">Vertical block count</param>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionLeftmostColumn(Int32 blockCountVertical, Int32 blockCountHorizontal, Double[,][] coefficientData)
        {
            for (Int32 y = 1; y < (blockCountVertical - 1); ++y)
            {
                //set up indecies into existing component data
                Double dc2 = coefficientData[y - 1, 0][0];
                Double dc6 = coefficientData[y, 1][0];
                Double dc8 = coefficientData[y + 1, 0][0];

                //redefine ACs
                SmoothZZ2(coefficientData[y, 0], dc2, dc8);
                SmoothZZ3(coefficientData[y, 0], dc2, dc8);
                SmoothZZ5(coefficientData[y, 0], dc6, dc6);    //double 6 since there is an average of left and right, effectively
            }
        }

        /// <summary>Smooths the right-most column of the image</summary>
        /// <param name="blockCountVertical">Vertical block count</param>
        /// <param name="blockCountHorizontal">Horizontal block count</param>
        /// <param name="coefficientData">Double-precision floating-point data to adjust in-position</param>
        private static void SmoothingPredictionRightmostColumn(Int32 blockCountVertical, Int32 blockCountHorizontal, Double[,][] coefficientData)
        {
            Int32 x = blockCountHorizontal - 1;

            for (Int32 y = 1; y < (blockCountVertical - 1); ++y)
            {
                //set up indecies into existing component data
                Double dc2 = coefficientData[y - 1, x][0];
                Double dc4 = coefficientData[y, x - 1][0];
                Double dc8 = coefficientData[y + 1, x][0];

                //redefine ACs
                SmoothZZ2(coefficientData[y, x], dc2, dc8);
                SmoothZZ3(coefficientData[y, x], dc2, dc8);
                SmoothZZ5(coefficientData[y, x], dc4, dc4);    //double 4 since there is an average of left and right, effectively
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>Smooths AC1 in zig-zag order</summary>
        /// <param name="coefficientData">Data List to adjust</param>
        /// <param name="dc4">DC coefficient of the left block</param>
        /// <param name="dc6">DC coefficient of the right block</param>
        private static void SmoothZZ1(IList<Double> coefficientData, Double dc4, Double dc6)
        {
            if (coefficientData[1] == 0.0)   //AC 0,1
                coefficientData[1] = (1.13885 * (dc4 - dc6)) / 8.0;
        }

        /// <summary>Smooths AC2 in zig-zag order</summary>
        /// <param name="coefficientData">Data List to adjust</param>
        /// <param name="dc2">DC coefficient of the above block</param>
        /// <param name="dc8">DC coefficient of the below block</param>
        private static void SmoothZZ2(IList<Double> coefficientData, Double dc2, Double dc8)
        {
            if (coefficientData[8] == 0.0)   //AC 1,0
                coefficientData[8] = (1.13885 * (dc2 - dc8)) / 8.0;
        }

        /// <summary>Smooths AC3 in zig-zag order</summary>
        /// <param name="coefficientData">Data List to adjust</param>
        /// <param name="dc2">DC coefficient of the above block</param>
        /// <param name="dc8">DC coefficient of the below block</param>
        private static void SmoothZZ3(IList<Double> coefficientData, Double dc2, Double dc8)
        {
            if (coefficientData[16] == 0.0)  //AC 2,0
                coefficientData[16] = (0.27881 * ((dc2 + dc8) - (2 * coefficientData[0]))) / 8.0;
        }

        /// <summary>Smooths AC3 in zig-zag order</summary>
        /// <param name="coefficientData">Data List to adjust</param>
        /// <param name="dc1">DC coefficient of the upper left block</param>
        /// <param name="dc3">DC coefficient of the upper right block</param>
        /// <param name="dc7">DC coefficient of the lower left block</param>
        /// <param name="dc9">DC coefficient of the lower right block</param>
        private static void SmoothZZ4(IList<Double> coefficientData, Double dc1, Double dc3, Double dc7, Double dc9)
        {
            if (coefficientData[9] == 0.0)   //AC 1,1
                coefficientData[9] = (0.16213 * ((dc1 - dc3) - (dc7 - dc9))) / 8.0;
        }

        /// <summary>Smooths AC5 in zig-zag order</summary>
        /// <param name="coefficientData">Data List to adjust</param>
        /// <param name="dc4">DC coefficient of the left block</param>
        /// <param name="dc6">DC coefficient of the right block</param>
        private static void SmoothZZ5(IList<Double> coefficientData, Double dc4, Double dc6)
        {
            if (coefficientData[2] == 0.0)   //AC 0,2
                coefficientData[2] = (0.27881 * ((dc4 + dc6) - (2 * coefficientData[0]))) / 8.0;
        }
        #endregion
    }
}