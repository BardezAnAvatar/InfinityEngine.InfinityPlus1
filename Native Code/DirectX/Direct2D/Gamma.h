	

#ifndef Bardez_Projects_DirectX_Direct2D_Gamma
#define Bardez_Projects_DirectX_Direct2D_Gamma

using namespace System::ComponentModel;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Specifies which gamma is used for interpolation.</summary>
				/// <remarks>See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368113%28v=VS.85%29.aspx for more detail</remarks>
				public enum struct Gamma : System::UInt32
				{
					/// <summary>Interpolation is performed in the standard RGB (sRGB) gamma.</summary>
					[DescriptionAttribute("Interpolation is performed in the standard RGB (sRGB) gamma.")]
					Gamma2_2		= 0,

					/// <summary>Interpolation is performed in the linear-gamma color space.</summary>
					[DescriptionAttribute("Interpolation is performed in the linear-gamma color space.")]
					Gamma1_0		= 1,
				};
			}
		}
	}
}


#endif
