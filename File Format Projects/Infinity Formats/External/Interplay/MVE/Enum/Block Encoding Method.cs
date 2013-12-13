using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum
{
    /// <summary>Represents the encoding method for an 8x8 block of pixels</summary>
    /// <remarks>
    ///     Curent frame = displayed frame (old); new frame = frame under construction.
    /// </remarks>
    public enum BlockEncodingMethod : byte /* Byte */
    {
        /// <summary>Copy from previous block to new block</summary>
        Copy = 0,

        /// <summary>Apparently this means unchanged from two frames ago. Either black or effectively encoding 0.</summary>
        /// <remarks>This implies a rotating frame buffer, i.e.: the buffer from last frame is not copied, but the value from two frames ago remains.</remarks>
        Unchanged = 1,

        /// <summary>Clone from a lower block in the new frame</summary>
        [Description("Copy from the bottom")]
        CopyBottom = 2,

        /// <summary>Clone from a higher block in the new frame</summary>
        [Description("Copy from the top")]
        CopyTop = 3,

        /// <summary>Clone from a block in the current frame</summary>
        [Description("Copy from the previous frame")]
        CopyPrevious = 4,

        /// <summary>Clone from a block in the current frame (larger area)</summary>
        [Description("Copy from the previous frame (larger area)")]
        CopyPreviousLarge = 5,

        /// <summary>Skips two blocks</summary>
        [Description("Skips two blocks"), Obsolete("Not used in IE MVE files.")]
        SkipTwoBlocks = 6,

        /// <summary>Two colors, either bitfield by row or bitfield by squares of 2 pixels</summary>
        [Description("Two-color bit field of rows or 4-pixel squares")]
        BitField2PixelRowOr2Square = 7,

        /// <summary>Four- or eight-color bitfield split by quadrants</summary>
        [Description("Four- or eight-color bit field")]
        BitFieldQuadrants4or8Pixel = 8,

        /// <summary>Four color bitfields with two bits per pixel</summary>
        [Description("Four-color bit field with two bits per pixel")]
        BitField2BitsPerPixel = 9,

        /// <summary>Four color bitfields split by quadrants or halves with two bits per pixel</summary>
        [Description("Four-color bit field split by quadrants or halves with two bits per pixel")]
        BitFieldQuadrantsOrHalves2BitsPerPixel = 0x0A,

        /// <summary>Each pixel is a palette index</summary>
        Raw = 0x0B,

        /// <summary>Each pixel is a palette index, but one value is shared with a 2x2 block of neighbors</summary>
        Raw2Square = 0x0C,

        /// <summary>Each pixel is a palette index, but one value is shared with a 4x4 block of neighbors</summary>
        Raw4Square = 0x0D,

        /// <summary>Each pixel is a palette index, but one value is shared with a 8x8 block of neighbors</summary>
        Raw8Square = 0x0E,

        /// <summary>Read two values, dithering between the two values</summary>
        Dithered = 0x0F,
    }
}