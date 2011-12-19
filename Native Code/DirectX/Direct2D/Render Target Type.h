

#ifndef Bardez_Projects_DirectX_Direct2D_RenderTargetType
#define Bardez_Projects_DirectX_Direct2D_RenderTargetType

using namespace System::ComponentModel;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <remarks>See http://msdn.microsoft.com/en-us/library/windows/desktop/dd756630%28v=VS.85%29.aspx for more detail</remarks>
				public enum struct RenderTargetType : System::UInt32
				{
					/// <summary>The render target uses hardware rendering, if available; otherwise, it uses software rendering.</summary>
					[DescriptionAttribute("The render target uses hardware rendering, if available; otherwise, it uses software rendering.")]
					Default         = 0,

					/// <summary>The render target uses software rendering only.</summary>
					[DescriptionAttribute("The render target uses software rendering only.")]
					Software		= 1,

					/// <summary>The render target uses hardware rendering only.</summary>
					[DescriptionAttribute("The render target uses hardware rendering only.")]
					Hardware        = 2,
				};
			}
		}
	}
}


#endif
