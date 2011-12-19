
#ifndef Bardez_Projects_DirectX_Direct2D_CapStyle
#define Bardez_Projects_DirectX_Direct2D_CapStyle

using namespace System::ComponentModel;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Describes the shape at the end of a line or segment.</summary>
				/// <remarks>See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368079%28v=VS.85%29.aspx for more detail</remarks>
				public enum struct CapStyle : System::UInt32
				{
					/// <summary>A cap that does not extend past the last point of the line. Comparable to cap used for objects other than lines.</summary>
					[DescriptionAttribute("A cap that does not extend past the last point of the line. Comparable to cap used for objects other than lines.")]
					Flat			= 0,

					/// <summary>Half of a square that has a length equal to the line thickness.</summary>
					[DescriptionAttribute("Half of a square that has a length equal to the line thickness.")]
					Square			= 1,

					/// <summary>A semicircle that has a diameter equal to the line thickness.</summary>
					[DescriptionAttribute("A semicircle that has a diameter equal to the line thickness.")]
					Round			= 2,

					/// <summary>An isosceles right triangle whose hypotenuse is equal in length to the thickness of the line.</summary>
					[DescriptionAttribute("An isosceles right triangle whose hypotenuse is equal in length to the thickness of the line.")]
					Triangle		= 3,
				};
			}
		}
	}
}


#endif
