
#ifndef Bardez_Projects_DirectX_Direct2D_Layer
#define Bardez_Projects_DirectX_Direct2D_Layer

#include <D2D1.h>
#include "SizeF.h"

using namespace Bardez::Projects::Win32;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a render target's layer for Direct2D</summary>
				public ref class Layer
				{
				#pragma region Fields
				protected:
					/// <summary>Represents a pointer to the native render target layer interface object</summary>
					System::IntPtr layerPtr;
				#pragma endregion


				#pragma region Properties
				internal:
					/// <summary>Represents a pointer to the native render target layer interface object</summary>
					property ID2D1Layer* LayerPtr
					{
						ID2D1Layer* get();
						void set(ID2D1Layer* value);
					}
				#pragma endregion


				#pragma region Construction
				internal:
					/// <summary>Definition constructor</summary>
					Layer(ID2D1Layer* handle);
				#pragma endregion


				#pragma region Destruction
				public:
					/// <summary>Destrutor</summary>
					/// <remarks>Dispose()</remarks>
					~Layer();

					/// <summary>Destrutor</summary>
					/// <remarks>Finalize()</remarks>
					!Layer();

					/// <summary>Destrutor logic, disposes the object</summary>
					virtual void DisposeUnmanaged();
				#pragma endregion


				#pragma region Interface Methods
				public:
					/// <summary>Gets the size of the layer in device-independent pixels.</summary>
					/// <returns>The size of the layer in device-independent pixels.</returns>
					virtual SizeF^ GetSize();
				#pragma endregion
				};
			}
		}
	}
}

#endif
