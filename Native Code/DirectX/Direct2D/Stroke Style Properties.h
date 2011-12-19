

#ifndef Bardez_Projects_DirectX_Direct2D_StrokeStyleProperties
#define Bardez_Projects_DirectX_Direct2D_StrokeStyleProperties

#include <d2d1.h>
#include "Cap Style.h"
#include "Dash Style.h"
#include "Line Joint.h"

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Describes the stroke that outlines a shape.</summary>
				public ref class StrokeStyleProperties
				{
				#pragma region Fields
				protected:
					/// <summary>The cap applied to the start of all the open figures in a stroked geometry.</summary>
					CapStyle startCap;

					/// <summary>The cap applied to the end of all the open figures in a stroked geometry.</summary>
					CapStyle endCap;

					/// <summary>The shape at either end of each dash segment.</summary>
					CapStyle dashCap;

					/// <summary>A value that describes how segments are joined.</summary>
					/// <remarks>This value is ignored for a vertex if the segment flags specify that the segment should have a smooth join.</remarks>
					Bardez::Projects::DirectX::Direct2D::LineJoint lineJoint;

					/// <summary>The limit of the thickness of the join on a mitered corner.</summary>
					/// <remarks>This value is always treated as though it is greater than or equal to 1.0F.</remarks>
					System::Single miterLimit;

					/// <summary>A value that specifies whether the stroke has a dash pattern and, if so, the dash style.</summary>
					Bardez::Projects::DirectX::Direct2D::DashStyle dashStyle;

					/// <summary>A value that specifies an offset in the dash sequence.</summary>
					/// <value>
					///		A positive dash offset value shifts the dash pattern, in units of stroke width, toward the start of the stroked geometry.
					///		A negative dash offset value shifts the dash pattern, in units of stroke width, toward the end of the stroked geometry.
					///	</value>
					System::Single dashOffset;
				#pragma endregion

					
				#pragma region Properties
				public:
					/// <summary>Exposes the cap applied to the start of all the open figures in a stroked geometry.</summary>
					virtual property CapStyle StartCap
					{
						CapStyle get();
						void set(CapStyle value);
					}

					/// <summary>Exposes the cap applied to the end of all the open figures in a stroked geometry.</summary>
					virtual property CapStyle EndCap
					{
						CapStyle get();
						void set(CapStyle value);
					}

					/// <summary>Exposes the shape at either end of each dash segment.</summary>
					virtual property CapStyle DashCap
					{
						CapStyle get();
						void set(CapStyle value);
					}

					/// <summary>Exposes a value that describes how segments are joined.</summary>
					/// <remarks>This value is ignored for a vertex if the segment flags specify that the segment should have a smooth join.</remarks>
					virtual property Bardez::Projects::DirectX::Direct2D::LineJoint LineJoint
					{
						Bardez::Projects::DirectX::Direct2D::LineJoint get();
						void set(Bardez::Projects::DirectX::Direct2D::LineJoint value);
					}

					/// <summary>Exposes the limit of the thickness of the join on a mitered corner.</summary>
					/// <remarks>This value is always treated as though it is greater than or equal to 1.0F.</remarks>
					virtual property System::Single MiterLimit
					{
						System::Single get();
						void set(System::Single value);
					}

					/// <summary>Exposes a value that specifies whether the stroke has a dash pattern and, if so, the dash style.</summary>
					virtual property Bardez::Projects::DirectX::Direct2D::DashStyle DashStyle
					{
						Bardez::Projects::DirectX::Direct2D::DashStyle get();
						void set(Bardez::Projects::DirectX::Direct2D::DashStyle value);
					}

					/// <summary>Exposes a value that specifies an offset in the dash sequence.</summary>
					/// <value>
					///		A positive dash offset value shifts the dash pattern, in units of stroke width, toward the start of the stroked geometry.
					///		A negative dash offset value shifts the dash pattern, in units of stroke width, toward the end of the stroked geometry.
					///	</value>
					virtual property System::Single DashOffset
					{
						System::Single get();
						void set(System::Single value);
					}
				#pragma endregion


				#pragma region Construction
				public:
					/// <summary>Definition Constructor</summary>
					/// <param name="start">The cap applied to the start of all the open figures in a stroked geometry.</param>
					/// <param name="end">The cap applied to the end of all the open figures in a stroked geometry.</param>
					/// <param name="dash">The shape at either end of each dash segment.</param>
					/// <param name="lineJoin">A value that describes how segments are joined.</param>
					/// <param name="miterLimit">The limit of the thickness of the join on a mitered corner.</param>
					/// <param name="dashStyle">A value that specifies whether the stroke has a dash pattern and, if so, the dash style.</param>
					/// <param name="dashOffset">A value that specifies an offset in the dash sequence.</param>
					StrokeStyleProperties(CapStyle start, CapStyle end, CapStyle dash, Bardez::Projects::DirectX::Direct2D::LineJoint lineJoin, System::Single miterLimit, Bardez::Projects::DirectX::Direct2D::DashStyle dashStyle, System::Single dashOffset);

				internal:
					/// <summary>Unmanaged Constructor</summary>
					/// <param name="unmanaged">Unmanaged source structure</param>
					StrokeStyleProperties(D2D1_STROKE_STYLE_PROPERTIES unmanaged);
				#pragma endregion


				#pragma region Methods
				internal:
					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct equivalent</returns>
					D2D1_STROKE_STYLE_PROPERTIES ToUnmanaged();
				#pragma endregion
				};
			}
		}
	}
}

#endif
