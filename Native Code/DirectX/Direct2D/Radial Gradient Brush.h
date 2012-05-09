
#ifndef Bardez_Projects_DirectX_Direct2D_RadialGradientBrush
#define Bardez_Projects_DirectX_Direct2D_RadialGradientBrush

#include <D2D1.h>
#include "Brush.h"
#include "Gamma.h"
#include "Gradient Stop Collection.h"
#include "Point2dF.h"

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a RadialGradientBrush rendering aid for Direct2D</summary>
				public ref class RadialGradientBrush : Brush
				{
				#pragma region Properties
				internal:
					/// <summary>Exposes the native handle to the ID2D1BitmapBrush object</summary>
					property ID2D1RadialGradientBrush* RadialGradientBrushPtr
					{
						ID2D1RadialGradientBrush* get();
						void set(ID2D1RadialGradientBrush* value);
					}

				public:
					/// <summary>Exposes the center of the gradient ellipse.</summary>
					virtual property Point2dF^ Center
					{
						Point2dF^ get();
						void set(Point2dF^ value);
					}

					/// <summary>Exposes the offset of the gradient origin relative to the gradient ellipse's center.</summary>
					virtual property Point2dF^ GradientOriginOffset
					{
						Point2dF^ get();
						void set(Point2dF^ value);
					}

					/// <summary>Exposes the x-radius of the gradient ellipse.</summary>
					virtual property System::Single RadiusX
					{
						System::Single get();
						void set(System::Single value);
					}

					/// <summary>Exposes the y-radius of the gradient ellipse.</summary>
					virtual property System::Single RadiusY
					{
						System::Single get();
						void set(System::Single value);
					}

					// <summary>Exposes the GradientStopCollection associated with this linear gradient brush.</summary>
					virtual property GradientStopCollection^ GetGradientStops
					{
						GradientStopCollection^ get();
					}
				#pragma endregion


				#pragma region Construction
				internal:
					/// <summary>Definition constructor</summary>
					/// <param name="pointer">Pointer to the Direct2D brush</param>
					RadialGradientBrush(ID2D1RadialGradientBrush* pointer);
				#pragma endregion


				#pragma region Destruction
				public:
					/// <summary>Destructor</summary>
					/// <remarks>Dispose()</remarks>
					~RadialGradientBrush();

					/// <summary>Destructor</summary>
					/// <remarks>Finalize()</remarks>
					!RadialGradientBrush();

					/// <summary>Destructor logic, disposes the object</summary>
					virtual void DisposeUnmanaged() override;
				#pragma endregion
				};
			}
		}
	}
}

#endif
