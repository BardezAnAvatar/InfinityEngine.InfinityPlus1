

#ifndef Bardez_Projects_DirectX_Direct2D_ControlState
#define Bardez_Projects_DirectX_Direct2D_ControlState

using namespace System::ComponentModel;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Indicates the occluded state of the window.</summary>
				/// <remarks>See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368174%28v=VS.85%29.aspx for more detail</remarks>
				public enum struct ControlState : System::UInt32
				{
					/// <summary>The window is not occluded.</summary>
					[DescriptionAttribute("The window is not occluded.")]
					None			= 0,

					/// <summary>The window is occluded.</summary>
					[DescriptionAttribute("The window is occluded.")]
					Occluded		= 1,
				};
			}
		}
	}
}


#endif
