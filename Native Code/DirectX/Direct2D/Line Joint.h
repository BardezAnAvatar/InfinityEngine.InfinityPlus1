
#ifndef Bardez_Projects_DirectX_Direct2D_LineJoint
#define Bardez_Projects_DirectX_Direct2D_LineJoint

using namespace System::ComponentModel;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Describes the shape that joins two lines or segments.</summary>
				/// <remarks>See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368130%28v=VS.85%29.aspx for more detail</remarks>
				public enum struct LineJoint : System::UInt32
				{
					/// <summary>Regular angular vertices.</summary>
					[DescriptionAttribute("Regular angular vertices.")]
					Miter			= 0,

					/// <summary>Beveled vertices.</summary>
					[DescriptionAttribute("Beveled vertices.")]
					Bevel			= 1,

					/// <summary>Rounded vertices.</summary>
					[DescriptionAttribute("Rounded vertices.")]
					Round			= 2,

					/// <summary>Regular angular vertices unless the join would extend beyond the miter limit; otherwise, beveled vertices.</summary>
					[DescriptionAttribute("Regular angular vertices unless the join would extend beyond the miter limit; otherwise, beveled vertices.")]
					MiterOrBevel	= 3,
				};
			}
		}
	}
}

#endif
