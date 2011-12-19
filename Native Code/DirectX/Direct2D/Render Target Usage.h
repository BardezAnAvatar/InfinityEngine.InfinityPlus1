

#ifndef Bardez_Projects_DirectX_Direct2D_RenderTargetUsage
#define Bardez_Projects_DirectX_Direct2D_RenderTargetUsage

using namespace System::ComponentModel;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <remarks>See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368157%28v=VS.85%29.aspx for more detail</remarks>
				public enum struct RenderTargetUsage : System::UInt32
				{
					/// <summary>
					//		The render target attempts to use Direct3D command-stream remoting and uses
					///		bitmap remoting if stream remoting fails. The render target is not GDI-compatible.
					///	</summary>
					[DescriptionAttribute("The render target attempts to use Direct3D command-stream remoting and uses bitmap remoting if stream remoting fails. The render target is not GDI-compatible.")]
					None					= 0,

					/// <summary>The render target renders content locally and sends it to the terminal services client as a bitmap.</summary>
					[DescriptionAttribute("The render target renders content locally and sends it to the terminal services client as a bitmap.")]
					ForceBitmapRemoting		= 1,

					/// <summary>The render target can be used efficiently with GDI.</summary>
					[DescriptionAttribute("The render target can be used efficiently with GDI.")]
					GdiCompatible			= 2,
				};
			}
		}
	}
}


#endif
