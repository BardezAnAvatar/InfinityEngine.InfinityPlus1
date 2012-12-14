
#ifndef Bardez_Projects_DirectX_Direct2D_BitmapRenderTarget
#define Bardez_Projects_DirectX_Direct2D_BitmapRenderTarget

#include "Render Target.h"
#include "Bitmap.h"

using namespace Bardez::Projects::BasicStructures::Win32;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a bitmap render target for Direct2D</summary>
				public ref class BitmapRenderTarget : public RenderTarget
				{
				#pragma region Properties
				internal:
					/// <summary>Represents a pointer to the native bitmap render target interface object</summary>
					property ID2D1BitmapRenderTarget* BitmapRenderPtr
					{
						ID2D1BitmapRenderTarget* get();
						void set(ID2D1BitmapRenderTarget* value);
					}
				#pragma endregion


				#pragma region Construction
				internal:
					/// <summary>Definition constructor</summary>
					BitmapRenderTarget(ID2D1BitmapRenderTarget* handle);
				#pragma endregion
					

				#pragma region Destruction
				public:
					/// <summary>Destructor</summary>
					/// <remarks>Dispose()</remarks>
					~BitmapRenderTarget();

					/// <summary>Destructor</summary>
					/// <remarks>Finalize()</remarks>
					!BitmapRenderTarget();

					/// <summary>Destructor logic, disposes the object</summary>
					virtual void DisposeUnmanaged() override;
				#pragma endregion


				#pragma region Interface Methods
				public:
					/// <summary>Creates an uninitialized Direct2D bitmap.</summary>
					/// <param name="bitmap">When this method returns, contains a pointer to a pointer to the new bitmap. This parameter is passed uninitialized.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode GetBitmap([System::Runtime::InteropServices::Out] Bitmap^ %bitmap);
				#pragma endregion
				};
			}
		}
	}
}

#endif
