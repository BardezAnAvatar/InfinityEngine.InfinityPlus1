
#ifndef Bardez_Projects_DirectX_Direct2D_GradientStopCollection
#define Bardez_Projects_DirectX_Direct2D_GradientStopCollection

#include <D2D1.h>
#include "Brush Extend Mode.h"
#include "Gamma.h"
#include "Gradient Stop.h"

using namespace Bardez::Projects::Win32;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents an collection of D2D1_GRADIENT_STOP objects for linear and radial gradient brushes.</summary>
				public ref class GradientStopCollection
				{
				#pragma region Fields
				protected:
					/// <summary>Represents a pointer to a ID2D1GradientStopCollection native interface</summary>
					System::IntPtr gradientStopCollectionPtr;
				#pragma endregion


				#pragma region Properties
				internal:
					/// <summary>Exposes a pointer to a ID2D1GradientStopCollection native interface</summary>
					virtual property ID2D1GradientStopCollection* GradientStopCollectionPtr
					{
						ID2D1GradientStopCollection* get();
						void set(ID2D1GradientStopCollection* value);
					}

				public:
					/// <summary>Exposes the gamma space in which the gradient stops are interpolated.</summary>
					virtual property Gamma ColorInterpolationGamma
					{
						Gamma get();
					}

					/// <summary>Exposes the behavior of the gradient outside the normalized gradient range.</summary>
					virtual property BrushExtendMode ExtendMode
					{
						BrushExtendMode get();
					}

					/// <summary>Exposes the number of gradient stops in the collection.</summary>
					virtual property System::UInt32 GradientStopCount
					{
						System::UInt32 get();
					}
				#pragma endregion


				#pragma region Construction
				internal:
					/// <summary>Definition constructor</summary>
					/// <param name="pointer">ID2D1GradientStopCollection source pointer</param>
					GradientStopCollection(ID2D1GradientStopCollection* pointer);
				#pragma endregion


				#pragma region Destruction
				public:
					/// <summary>Destructor</summary>
					/// <remarks>Dispose()</remarks>
					~GradientStopCollection();

					/// <summary>Destructor</summary>
					/// <remarks>Finalize()</remarks>
					!GradientStopCollection();

					/// <summary>Destructor logic, disposes the object</summary>
					virtual void DisposeUnmanaged();
				#pragma endregion


				#pragma region Methods
					/// <summary>Copies the gradient stops from the collection into an array of GradientStop objects.</summary>
					/// <param name="gradientStopsCount">A value indicating the number of gradient stops to copy</param>
					/// <returns>A one-dimensional array of GradientStop references containing copies of the collection's gradient stops.</returns>
					virtual array<GradientStop^>^ GetGradientStops(System::UInt32 gradientStopsCount);
				#pragma endregion
				};
			}
		}
	}
}

#endif
