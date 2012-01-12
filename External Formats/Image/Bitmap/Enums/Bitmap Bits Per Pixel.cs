using System;

namespace Bardez.Projects.InfinityPlus1.Files.External.Image.Bitmap.Enums
{
    /// <summary>The number of bits-per-pixel. determines the number of bits that define each pixel and the maximum number of colors in the bitmap.</summary>
    public enum BitsPerPixel : ushort /* UInt16 */
    {
        /// <summary>The number of bits-per-pixel is specified or is implied by the JPEG or PNG format.</summary>
        Bpp0    = 0,

        /// <summary>
        ///     The bitmap is monochrome, and the Palette member of Bitmap contains two entries. Each bit in the bitmap array represents a pixel.
        ///     If the bit is clear, the pixel is displayed with the color of the first entry in the Palette table; if the bit is set, the pixel has the color of the second entry in the table.
        /// </summary>
        Bpp1    = 1,

        /// <summary>
        ///     The bitmap is quad-colored, and the Palette member of Bitmap contains fouir entries. Every two bit in the bitmap array represents a pixel.
        /// </summary>
        /// <remarks>This, as fara as I know, was only used in Windows CE devices</remarks>
        Bpp2    = 2,

        /// <summary>
        ///     The bitmap has a maximum of 16 colors, and the Palette member of Bitmap contains up to 16 entries.
        ///     Each pixel in the bitmap is represented by a 4-bit index into the color table. For example, if the first byte in the bitmap is 0x1F, the byte represents two pixels.
        ///     The first pixel contains the color in the second table entry, and the second pixel contains the color in the sixteenth table entry.
        /// </summary>
        Bpp4    = 4,

        /// <summary>The bitmap has a maximum of 256 colors, and the Palette member of Bitmap contains up to 256 entries. In this case, each byte in the array represents a single pixel.</summary>
        Bpp8    = 8,

        /// <summary>
        ///     The bitmap has a maximum of 2^16 colors. If the Compression member of the BitmapInfoHeader_v3 is Rgb, the Palette member of Bitmap is null.
        ///     Each Int16 in the bitmap array represents a single pixel. The relative intensities of red, green, and blue are represented with five bits for each color component.
        ///     The value for blue is in the least significant five bits, followed by five bits each for green and red. The most significant bit is not used.
        ///     The Palette color table is used for optimizing colors used on palette-based devices, and must contain the number of entries specified by the ColorsUsed member of the BitmapInfoHeader_v3.
        /// </summary>
        /// <value>
        ///     If the Compression member of the BitmapInfoHeader_v3 is BitFields, the Colors member contains three UInt32 color masks that specify the red, green, and blue components, respectively, of each pixel.
        ///     Each UInt16 in the bitmap array represents a single pixel.
        /// </value>
        /// <remarks>
        ///     When the Compression member is BitFields, bits set in each UInt32 mask must be contiguous and should not overlap the bits of another mask.
        ///     All the bits in the pixel do not have to be used.
        /// </remarks>
        Bpp16   = 16,

        /// <summary>
        ///     The bitmap has a maximum of 2^24 colors, and the Palette member of Bitmap is null.
        ///     Each 3-byte triplet in the bitmap array represents the relative intensities of blue, green, and red, respectively, for a pixel.
        ///     The Palette color table is used for optimizing colors used on palette-based devices, and must contain the number of entries specified by the ColorsUsed member of the BitmapInfoHeader_v3.
        /// </summary>
        Bpp24   = 24,

        /// <summary>
        ///     The bitmap has a maximum of 2^32 colors. If the Compression member of the BitmapInfoHeader_v3 is Rgb, the Palette member of Bitmap is null.
        ///     Each UInt32 in the bitmap array represents the relative intensities of blue, green, and red, respectively, for a pixel.
        ///     The high byte in each UInt32 is not used.
        ///     The Palette color table is used for optimizing colors used on palette-based devices, and must contain the number of entries specified by the ColorsUsed member of the BitmapInfoHeader_v3.
        /// </summary>
        /// <value>
        ///     If the Compression member of the BitmapInfoHeader_v3 is BitFields, the Colors member contains three UInt32 color masks that specify the red, green, and blue components, respectively, of each pixel.
        ///     Each UInt32 in the bitmap array represents a single pixel.
        /// </value>
        /// <remarks>
        ///     When the Compression member is BitFields, bits set in each UInt32 mask must be contiguous and should not overlap the bits of another mask.
        ///     All the bits in the pixel do not have to be used.
        /// </remarks>
        Bpp32   = 32,
    }
}