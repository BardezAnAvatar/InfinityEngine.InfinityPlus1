

#ifndef Bardez_Projects_DirectX_Direct2D_AlphaMode
#define Bardez_Projects_DirectX_Direct2D_AlphaMode

using namespace System::ComponentModel;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <remarks>See http://msdn.microsoft.com/en-us/library/hh404496(v=VS.85).aspx for more detail</remarks>
				public enum struct AlphaMode : System::UInt32
				{
					/// <summary>The alpha value might not be meaningful.</summary>
					[DescriptionAttribute("The alpha value might not be meaningful.")]
					Unknown         = 0,
						
					/// <summary>
					///		The alpha value has been premultiplied. Each color is first scaled by the alpha value.
					///		The alpha value itself is the same in both straight and premultiplied alpha.
					///		Typically, no color channel value is greater than the alpha channel value.
					///		If a color channel value in a premultiplied format is greater than the alpha channel, the standard source-over blending math results in an additive blend.
					///	</summary>
					[DescriptionAttribute("The alpha value has been premultiplied. Each color is first scaled by the alpha value. The alpha value itself is the same in both straight and premultiplied alpha. Typically, no color channel value is greater than the alpha channel value. If a color channel value in a premultiplied format is greater than the alpha channel, the standard source-over blending math results in an additive blend.")]
					PreMultiplied   = 1,
						
					/// <summary>The alpha value has not been premultiplied. The alpha channel indicates the transparency of the color.</summary>
					[DescriptionAttribute("The alpha value has not been premultiplied. The alpha channel indicates the transparency of the color.")]
					Straight        = 2,
						
					/// <summary>The alpha value is ignored.</summary>
					[DescriptionAttribute("The alpha value is ignored.")]
					Ignore          = 3,
				};
			}
		}
	}
}


#endif
