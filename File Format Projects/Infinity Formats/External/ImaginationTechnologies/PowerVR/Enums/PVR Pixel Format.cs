using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.ImaginationTechnologies.PowerVR.Enums
{
    /// <summary>Possible enumerated pixel format</summary>
    public enum PvrPixelFormat : uint /* UInt32 */
    {
        PVRTC_2bpp_RGB              = 0,
        PVRTC_2bpp_RGBA             = 1,
        PVRTC_4bpp_RGB              = 2,
        PVRTC_4bpp_RGBA             = 3,
        PVRTC_II_2bpp               = 4,
        PVRTC_II_4bpp               = 5,
        ETC1                        = 6,
        DXT1                        = 7,
        DXT2                        = 8,
        DXT3                        = 9,
        DXT4                        = 10,
        DXT5                        = 11,
        BC1                         = 7,
        BC2                         = 9,
        BC3                         = 11,
        BC4                         = 12,
        BC5                         = 13,
        BC6                         = 14,
        BC7                         = 15,
        UYVY                        = 16,
        YUY2                        = 17,
        BW1bpp                      = 18,
        R9G9B9E5_Shared_Exponent    = 19,
        RGBG8888                    = 20,
        GRGB8888                    = 21,
        ETC2_RGB                    = 22,
        ETC2_RGBA                   = 23,
        ETC2_RGB_A1                 = 24,
        EAC_R11_Unsigned            = 25,
        EAC_R11_Signed              = 26,
        EAC_RG11_Unsigned           = 27,
        EAC_RG11_Signed             = 28,
    }
}