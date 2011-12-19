

#ifndef Bardez_Projects_DirectX_Direct2D_DirectXVersion
#define Bardez_Projects_DirectX_Direct2D_DirectXVersion

#include <D2D1.h>

using namespace System::ComponentModel;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <remarks>See http://msdn.microsoft.com/en-us/library/windows/desktop/dd756628%28v=VS.85%29.aspx for more detail</remarks>
				public enum struct DirectXVersion : System::UInt32
				{
					/// <summary>Direct2D determines whether the video card provides adequate hardware rendering support.</summary>
					[DescriptionAttribute("Direct2D determines whether the video card provides adequate hardware rendering support.")]
					Default         = 0,

					/// <summary>The video card must support DirectX 9.</summary>
					[DescriptionAttribute("The video card must support DirectX 9.")]
					DirectX9		= D3D10_FEATURE_LEVEL_9_1,

					/// <summary>The video card must support DirectX 10.</summary>
					[DescriptionAttribute("The video card must support DirectX 10.")]
					DirectX10       = D3D10_FEATURE_LEVEL_10_0 ,
				};
			}
		}
	}
}


#endif
