
#ifndef Bardez_Projects_DirectX_Direct2D_RadialGradientBrushProperties
#define Bardez_Projects_DirectX_Direct2D_RadialGradientBrushProperties

#include <d2d1.h>
#include "Point2dF.h"

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Contains the starting point and endpoint of the gradient axis for a brush.</summary>
				public ref class RadialGradientBrushProperties
				{
				#pragma region Fields
				protected:
					/// <summary>In the brush's coordinate space, the center of the gradient ellipse.</summary>
					Point2dF^ center;

					/// <summary>In the brush's coordinate space, the offset of the gradient origin relative to the gradient ellipse's center.</summary>
					Point2dF^ gradientOriginOffset;

					/// <summary>In the brush's coordinate space, the x-radius of the gradient ellipse.</summary>
					System::Single radiusX;

					/// <summary>In the brush's coordinate space, the y-radius of the gradient ellipse.</summary>
					System::Single radiusY;
				#pragma endregion

					
				#pragma region Properties
				public:
					/// <summary>In the brush's coordinate space, the center of the gradient ellipse.</summary>
					virtual property Point2dF^ Center
					{
						Point2dF^ get();
						void set(Point2dF^ value);
					}

					/// <summary>In the brush's coordinate space, the offset of the gradient origin relative to the gradient ellipse's center.</summary>
					virtual property Point2dF^ GradientOriginOffset
					{
						Point2dF^ get();
						void set(Point2dF^ value);
					}

					/// <summary>In the brush's coordinate space, the x-radius of the gradient ellipse.</summary>
					virtual property System::Single RadiusX
					{
						System::Single get();
						void set(System::Single value);
					}

					/// <summary>In the brush's coordinate space, the y-radius of the gradient ellipse.</summary>
					virtual property System::Single RadiusY
					{
						System::Single get();
						void set(System::Single value);
					}
				#pragma endregion


				#pragma region Construction
				public:
					/// <summary>Definition Constructor</summary>
					/// <param name="center">In the brush's coordinate space, the center of the gradient ellipse.</param>
					/// <param name="origin">In the brush's coordinate space, the offset of the gradient origin relative to the gradient ellipse's center.</param>
					/// <param name="radiusX">In the brush's coordinate space, the x-radius of the gradient ellipse.</param>
					/// <param name="radiusY">In the brush's coordinate space, the y-radius of the gradient ellipse.</param>
					RadialGradientBrushProperties(Point2dF^ center, Point2dF^ origin, System::Single radiusX, System::Single radiusY);

				internal:
					/// <summary>Unmanaged Constructor</summary>
					/// <param name="unmanaged">Unmanaged source structure</param>
					RadialGradientBrushProperties(D2D1_RADIAL_GRADIENT_BRUSH_PROPERTIES unmanaged);
				#pragma endregion


				#pragma region Methods
				internal:
					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct equivalent</returns>
					D2D1_RADIAL_GRADIENT_BRUSH_PROPERTIES ToUnmanaged();
				#pragma endregion
				};
			}
		}
	}
}

#endif
