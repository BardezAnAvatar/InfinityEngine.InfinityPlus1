
#ifndef Bardez_Projects_DirectX_Direct2D_PresentationOptions
#define Bardez_Projects_DirectX_Direct2D_PresentationOptions

using namespace System::ComponentModel;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Describes how a render target behaves when it presents its content. This enumeration allows a bitwise combination of its member values.</summary>
				/// <remarks>See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368144%28v=VS.85%29.aspx for more detail</remarks>
				public enum struct PresentationOptions : System::UInt32
				{
					/// <summary>The render target waits until the display refreshes to present and discards the frame upon presenting.</summary>
					[DescriptionAttribute("The render target waits until the display refreshes to present and discards the frame upon presenting.")]
					None            = 0,

					/// <summary>The render target does not discard the frame upon presenting.</summary>
					[DescriptionAttribute("The render target does not discard the frame upon presenting.")]
					RetainContents	= 1,

					/// <summary>The render target does not wait until the display refreshes to present.</summary>
					[DescriptionAttribute("The render target does not wait until the display refreshes to present.")]
					Immediately		= 2,
				};
			}
		}
	}
}


#endif
