using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Bitmap.Enums
{
    /// <summary>Represents the intended rendering device for the image (print, screen, etc., but in a more abstract sense; Absolute (math), relative(business), etc.)</summary>
    public enum RenderingIntent : uint /* UInt32 */
    {
        /// <summary>Maintains the white point. Matches the colors to their nearest color in the destination gamut.</summary>
        AbsoluteColorimetric    = 8,    //LCS_GM_ABS_COLORIMETRIC

        /// <summary>Maintains saturation. Used for business charts and other situations in which undithered colors are required.</summary>
        Saturation              = 1,    //LCS_GM_BUSINESS

        /// <summary>Maintains colorimetric match. Used for graphic designs and named colors.</summary>
        RelativeColorimetric    = 2,    //LCS_GM_GRAPHICS

        /// <summary>Maintains contrast. Used for photographs and natural images.</summary>
        Perceptual              = 4     //LCS_GM_IMAGES
    }
}