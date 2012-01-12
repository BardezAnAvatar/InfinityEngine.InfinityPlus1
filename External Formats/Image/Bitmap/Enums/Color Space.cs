using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Bitmap.Enums
{
    /// <summary>Represents the colorspace of a Bitmap file</summary>
    public enum ColorSpace : int /* Int32 */
    {
        CalibratedRgb       = 0,
        sRGB                = 0x42475273,   //#define LCS_sRGB 'sRGB'
        WindowsColorSpace   = 0x206E6957,   //#define LCS_WINDOWS_COLOR_SPACE 'Win '  // Windows default color space
        ProfileLinked       = 0x4B4E494C,   //#define PROFILE_LINKED 'LINK'
        ProfileEmbedded     = 0x4445424D,   //#define PROFILE_EMBEDDED 'MBED'
    }
}