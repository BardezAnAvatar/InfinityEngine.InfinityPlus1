

#ifndef Bardez_Projects_DirectX_Direct2D_DebugLevel
#define Bardez_Projects_DirectX_Direct2D_DebugLevel

using namespace System::ComponentModel;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Indicates the type of information provided by the Direct2D Debug Layer.</summary>
				/// <remarks>See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368090%28v=VS.85%29.aspx for more detail</remarks>
				public enum struct DebugLevel : System::UInt32
				{
					/// <summary>Direct2D does not produce any debugging output.</summary>
					[DescriptionAttribute("Direct2D does not produce any debugging output.")]
					None            = 0,

					/// <summary>Direct2D sends error messages to the debug layer.</summary>
					[DescriptionAttribute("Direct2D sends error messages to the debug layer.")]
					Error			= 1,

					/// <summary>Direct2D sends error messages and warnings to the debug layer.</summary>
					[DescriptionAttribute("Direct2D sends error messages and warnings to the debug layer.")]
					Warning         = 2,

					/// <summary>Direct2D sends error messages, warnings, and additional diagnostic information that can help improve performance to the debug layer.</summary>
					[DescriptionAttribute("Direct2D sends error messages, warnings, and additional diagnostic information that can help improve performance to the debug layer.")]
					Information     = 3,
				};
			}
		}
	}
}


#endif
