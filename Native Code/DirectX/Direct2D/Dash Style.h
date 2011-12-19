
#ifndef Bardez_Projects_DirectX_Direct2D_DashStyle
#define Bardez_Projects_DirectX_Direct2D_DashStyle

using namespace System::ComponentModel;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Describes the sequence of dashes and gaps in a stroke.</summary>
				/// <remarks>See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368087%28v=VS.85%29.aspx for more detail</remarks>
				public enum struct DashStyle : System::UInt32
				{
					/// <summary>A solid line with no breaks.</summary>
					[DescriptionAttribute("A solid line with no breaks.")]
					Solid			= 0,

					/// <summary>The equivalent dash array is {2, 2}.</summary>
					[DescriptionAttribute("The equivalent dash array is {2, 2}.")]
					Dash			= 1,

					/// <summary>The equivalent dash array is {0, 2}.</summary>
					[DescriptionAttribute("The equivalent dash array is {0, 2}.")]
					Dot				= 2,

					/// <summary>The equivalent dash array is {2, 2, 0, 2}.</summary>
					[DescriptionAttribute("The equivalent dash array is {2, 2, 0, 2}.")]
					DashDot			= 3,

					/// <summary>The equivalent dash array is {2, 2, 0, 2, 0, 2}.</summary>
					[DescriptionAttribute("The equivalent dash array is {2, 2, 0, 2, 0, 2}.")]
					DashDotDot		= 4,

					/// <summary>The dash pattern is specified by an array of floating-point values.</summary>
					[DescriptionAttribute("The dash pattern is specified by an array of floating-point values.")]
					Custom			= 5,
				};
			}
		}
	}
}

#endif
