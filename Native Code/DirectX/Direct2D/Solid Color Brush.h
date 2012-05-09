
#ifndef Bardez_Projects_DirectX_Direct2D_SolidColorBrush
#define Bardez_Projects_DirectX_Direct2D_SolidColorBrush

#include <D2D1.h>
#include "Brush.h"
#include "ColorF.h"

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a SolidColorBrush rendering aid for Direct2D</summary>
				public ref class SolidColorBrush : Brush
				{
				#pragma region Properties
				internal:
					/// <summary>Exposes the native handle to the ID2D1SolidColorBrush object</summary>
					property ID2D1SolidColorBrush* SolidColorBrushPtr
					{
						ID2D1SolidColorBrush* get();
						void set(ID2D1SolidColorBrush* value);
					}

				public:
					/// <summary>The color of the solid color brush.</summary>
					virtual property ColorF^ Color
					{
						ColorF^ get();
						void set(ColorF^ value);
					}
				#pragma endregion


				#pragma region Construction
				internal:
					/// <summary>Definition constructor</summary>
					/// <param name="pointer">Pointer to the Direct2D brush</param>
					SolidColorBrush(ID2D1SolidColorBrush* pointer);
				#pragma endregion


				#pragma region Destruction
				public:
					/// <summary>Destructor</summary>
					/// <remarks>Dispose()</remarks>
					~SolidColorBrush();

					/// <summary>Destructor</summary>
					/// <remarks>Finalize()</remarks>
					!SolidColorBrush();

					/// <summary>Destructor logic, disposes the object</summary>
					virtual void DisposeUnmanaged() override;
				#pragma endregion
				};
			}
		}
	}
}

#endif
