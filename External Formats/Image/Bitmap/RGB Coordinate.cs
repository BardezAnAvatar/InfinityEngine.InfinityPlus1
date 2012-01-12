using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Bitmap
{
    /// <summary>Contains the x, y, and z coordinates of a color that correspond to either red, green, or blue.</summary>
    /// <remarks>
    ///     Maps to the CIEXYZ struct in WinGDI.h; it is defined as a struct of three ints.
    ///     Colorspace is foreign to me. It appears to be an RGB value.
    ///     See: http://en.wikipedia.org/wiki/CIE_1931_color_space for confusion.
    /// </remarks>
    public class RgbCoordinate
    {
        #region Properties
        /// <summary>The x coordinate</summary>
        public Int32 X { get; set; }

        /// <summary>The y coordinate</summary>
        public Int32 Y { get; set; }

        /// <summary>The z coordinate</summary>
        public Int32 Z { get; set; }
        #endregion

        #region Construction
        /// <summary>Default Constructor</summary>
        public RgbCoordinate() { }

        /// <summary>Default Constructor</summary>
        /// <param name="x">The x co-ordinate</param>
        /// <param name="y">The y co-ordinate</param>
        /// <param name="z">The z co-ordinate</param>
        public RgbCoordinate(Int32 x, Int32 y, Int32 z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        #endregion
    }
}