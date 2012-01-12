using System;

namespace Bardez.Projects.InfinityPlus1.Files.External.Image.Bitmap
{
    /// <summary>Contains the x,y, and z coordinates of the three colors that correspond to the red, green, and blue.</summary>
    /// <remarks>
    ///     Maps to the CIEXYZTRIPLE in WinGDI.h; it is defined as a struct of three CIEXYZs.
    ///     Colorspace is foreign to me. It appears to be an RGB value.
    ///     See: http://en.wikipedia.org/wiki/CIE_1931_color_space for confusion.
    /// </remarks>
    public class RgbCoordinateTriplet
    {
        #region Properties
        /// <summary>The xyz coordinates of red</summary>
        public RgbCoordinate Red { get; set; }

        /// <summary>The xyz coordinates of blue</summary>
        public RgbCoordinate Blue { get; set; }

        /// <summary>The xyz coordinates of green</summary>
        public RgbCoordinate Green { get; set; }
        #endregion

        #region Construction
        /// <summary>Default Constructor</summary>
        public RgbCoordinateTriplet() { }

        /// <summary>Default Constructor</summary>
        /// <param name="red">The red xyz co-ordinate</param>
        /// <param name="green">The green xyz co-ordinate</param>
        /// <param name="blue">The blue xyz co-ordinate</param>
        public RgbCoordinateTriplet(RgbCoordinate red, RgbCoordinate green, RgbCoordinate blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }
        #endregion
    }
}
