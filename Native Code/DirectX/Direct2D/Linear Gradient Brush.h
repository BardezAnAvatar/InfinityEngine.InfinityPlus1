
#ifndef Bardez_Projects_DirectX_Direct2D_LinearGradientBrush
#define Bardez_Projects_DirectX_Direct2D_LinearGradientBrush

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
				/// <summary>Represents a LinearGradientBrush rendering aid for Direct2D</summary>
				public ref class LinearGradientBrush : Brush
				{
				#pragma region Properties
				internal:
					/// <summary>Exposes the native handle to the ID2D1BitmapBrush object</summary>
					property ID2D1LinearGradientBrush* LinearGradientBrushPtr
					{
						ID2D1LinearGradientBrush* get();
						void set(ID2D1LinearGradientBrush* value);
					}

				public:
					/// <summary>Exposes the End point co-ordinate of the linear gradient</summary>
					virtual property Point2dF^ EndPoint
					{
						Point2dF^ get();
						void set(Point2dF^ value);
					}

					/// <summary>Exposes the Start point co-ordinate of the linear gradient</summary>
					virtual property Point2dF^ StartPoint
					{
						Point2dF^ get();
						void set(Point2dF^ value);
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
					LinearGradientBrush(ID2D1LinearGradientBrush * pointer);
				#pragma endregion


				#pragma region Destruction
				public:
					/// <summary>Destructor</summary>
					/// <remarks>Dispose()</remarks>
					~LinearGradientBrush();

					/// <summary>Destructor</summary>
					/// <remarks>Finalize()</remarks>
					!LinearGradientBrush();

					/// <summary>Destructor logic, disposes the object</summary>
					virtual void DisposeUnmanaged() override;
				#pragma endregion
				};
			}
		}
	}
}

#endif
