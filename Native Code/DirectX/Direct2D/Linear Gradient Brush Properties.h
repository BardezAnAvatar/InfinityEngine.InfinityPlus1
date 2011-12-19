
#ifndef Bardez_Projects_DirectX_Direct2D_LinearGradientBrushProperties
#define Bardez_Projects_DirectX_Direct2D_LinearGradientBrushProperties

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
				public ref class LinearGradientBrushProperties
				{
				#pragma region Fields
				protected:
					/// <summary>In the brush's coordinate space, the starting point of the gradient axis.</summary>
					Point2dF^ startPoint;

					/// <summary>In the brush's coordinate space, the endpoint of the gradient axis.</summary>
					Point2dF^ endPoint;
				#pragma endregion

					
				#pragma region Properties
				public:
					/// <summary>In the brush's coordinate space, the starting point of the gradient axis.</summary>
					virtual property Point2dF^ StartPoint
					{
						Point2dF^ get();
						void set(Point2dF^ value);
					}

					/// <summary>In the brush's coordinate space, the endpoint of the gradient axis.</summary>
					virtual property Point2dF^ EndPoint
					{
						Point2dF^ get();
						void set(Point2dF^ value);
					}
				#pragma endregion


				#pragma region Construction
				public:
					/// <summary>Definition Constructor</summary>
					/// <param name="start">The start point, in the brush's coordinate space, of the gradient axis.</param>
					/// <param name="end">The end point, in the brush's coordinate space, of the gradient axis.</param>
					LinearGradientBrushProperties(Point2dF^ start, Point2dF^ end);

				internal:
					/// <summary>Unmanaged Constructor</summary>
					/// <param name="unmanaged">Unmanaged source structure</param>
					LinearGradientBrushProperties(D2D1_LINEAR_GRADIENT_BRUSH_PROPERTIES unmanaged);
				#pragma endregion


				#pragma region Methods
				internal:
					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct equivalent</returns>
					D2D1_LINEAR_GRADIENT_BRUSH_PROPERTIES ToUnmanaged();
				#pragma endregion
				};
			}
		}
	}
}

#endif
