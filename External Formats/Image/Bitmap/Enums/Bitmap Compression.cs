using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Bitmap.Enums
{
    /// <summary>The type of compression for a compressed bottom-up bitmap (top-down DIBs cannot be compressed).</summary>
    public enum BitmapCompression : int /* Int32 */
    {
        /// <summary>An uncompressed format.</summary>
        Rgb         = 0,

        /// <summary>A run-length encoded (RLE) format for bitmaps with 8 bpp. The compression format is a 2-byte format consisting of a count byte followed by a byte containing a color index.</summary>
        Rle8,       //1

        /// <summary>An RLE format for bitmaps with 4 bpp. The compression format is a 2-byte format consisting of a count byte followed by two word-length color indexes.</summary>
        Rle4,       //2

        /// <summary>
        ///     Specifies that the bitmap is not compressed and that the color table consists of three
        ///     DWORD color masks that specify the red, green, and blue components, respectively, of each pixel.
        ///     This is valid when used with 16- and 32-bpp bitmaps.
        /// </summary>
        BitFields,  //3

        /// <summary>Indicates that the image is a JPEG image.</summary>
        Jpeg,       //4

        /// <summary>Indicates that the image is a PNG image.</summary>
        Png,        //5
    }
}
