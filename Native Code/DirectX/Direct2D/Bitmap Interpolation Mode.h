

#ifndef Bardez_Projects_DirectX_Direct2D_BitmapInterpolationMode
#define Bardez_Projects_DirectX_Direct2D_BitmapInterpolationMode

using namespace System::ComponentModel;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents the interpolation mode of a bitmap</summary>
				/// <remarks>
				///		To stretch an image, each pixel in the original image must be mapped to a group of pixels in the larger image.
				///		To shrink an image, groups of pixels in the original image must be mapped to single pixels in the smaller image.
				///		The effectiveness of the algorithms that perform these mappings determines the quality of a scaled image.
				///		Algorithms that produce higher-quality scaled images tend to require more processing time.
				///		BitmapInterpolationMode.NearestNeighbor provides faster but lower-quality interpolation,
				///		while BitmapInterpolationMode.Linear provides higher-quality interpolation. 
				///		See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368073%28v=VS.85%29.aspx for more detail
				///	</remarks>
				public enum struct BitmapInterpolationMode : System::UInt32
				{
					/// <summary>Use the exact color of the nearest bitmap pixel to the current rendering pixel.</summary>
					[DescriptionAttribute("Use the exact color of the nearest bitmap pixel to the current rendering pixel.")]
					NearestNeighbor	= 0,
						
					/// <summary>Interpolate a color from the four bitmap pixels that are the nearest to the rendering pixel.</summary>
					[DescriptionAttribute("Interpolate a color from the four bitmap pixels that are the nearest to the rendering pixel.")]
					Linear			= 1,
				};
			}
		}
	}
}


#endif
