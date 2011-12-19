

#ifndef Bardez_Projects_DirectX_Direct2D_CompatibleRenderTargetOptions
#define Bardez_Projects_DirectX_Direct2D_CompatibleRenderTargetOptions

using namespace System::ComponentModel;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <remarks>See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368085%28v=VS.85%29.aspx for more detail</remarks>
				public enum struct CompatibleRenderTargetOptions : System::UInt32
				{
					///	<summary>The render target supports no additional features.</summary>
					[DescriptionAttribute("The render target supports no additional features.")]
					None			= 0,

					///	<summary>The render target supports interoperability with the Windows Graphics Device Interface (GDI).</summary>
					[DescriptionAttribute("The render target supports interoperability with the Windows Graphics Device Interface (GDI).")]
					GdiCompatible   = 1,
				};
			}
		}
	}
}


#endif
