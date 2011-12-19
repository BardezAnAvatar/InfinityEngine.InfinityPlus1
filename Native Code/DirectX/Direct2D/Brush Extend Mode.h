

#ifndef Bardez_Projects_DirectX_Direct2D_BrushExtendMode
#define Bardez_Projects_DirectX_Direct2D_BrushExtendMode

using namespace System::ComponentModel;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Specifies how a brush paints areas outside of its normal content area.</summary>
				/// <remarks>
				///		See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368100(v=VS.85).aspx for more detail
				///	</remarks>
				public enum struct BrushExtendMode : System::UInt32
				{
					/// <summary>Repeat the edge pixels of the brush's content for all regions outside the normal content area.</summary>
					[DescriptionAttribute("Repeat the edge pixels of the brush's content for all regions outside the normal content area.")]
					Clamp	= 0,

					/// <summary>Repeat the brush's content.</summary>
					[DescriptionAttribute("Repeat the brush's content.")]
					Wrap	= 1,

					/// <summary>The same as D2D1_EXTEND_MODE_WRAP, except that alternate tiles of the brush's content are flipped. (The brush's normal content is drawn untransformed.)</summary>
					[DescriptionAttribute("The same as D2D1_EXTEND_MODE_WRAP, except that alternate tiles of the brush's content are flipped. (The brush's normal content is drawn untransformed.)")]
					Miror	= 2,
				};
			}
		}
	}
}


#endif
