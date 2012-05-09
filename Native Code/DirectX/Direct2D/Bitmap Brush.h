
#ifndef Bardez_Projects_DirectX_Direct2D_BitmapBrush
#define Bardez_Projects_DirectX_Direct2D_BitmapBrush

#include "Bitmap.h"
#include "Bitmap Interpolation Mode.h"
#include "Brush.h"
#include "Brush Extend Mode.h"

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a BitmapBrush rendering aid for Direct2D</summary>
				public ref class BitmapBrush : Brush
				{
				#pragma region Fields
				protected:
					/// <summary>Reference to the Bitmap for this Brush</summary>
					Bardez::Projects::DirectX::Direct2D::Bitmap^ bitmap;
				#pragma endregion


				#pragma region Properties
				internal:
					/// <summary>Exposes the native handle to the ID2D1BitmapBrush object</summary>
					property ID2D1BitmapBrush* BitmapBrushPtr
					{
						ID2D1BitmapBrush* get();
						void set(ID2D1BitmapBrush* value);
					}

				public:
					/// <summary>Exposes the Bitmap reference being used for this Brush</summary>
					/// <remarks>This will not call the Direct2D API, since it could easily cause a memory access error if the Bitmap handle was already disposed.</remarks>
					virtual property Bardez::Projects::DirectX::Direct2D::Bitmap^ Bitmap
					{
						Bardez::Projects::DirectX::Direct2D::Bitmap^ get();
						void set(Bardez::Projects::DirectX::Direct2D::Bitmap^ value);
					}
					
					/// <summary>Exposes the method by which the brush horizontally tiles those areas that extend past its bitmap.</summary>
					virtual property BrushExtendMode ExtendModeX
					{
						BrushExtendMode get();
						void set(BrushExtendMode value);
					}
					
					/// <summary>Exposes the method by which the brush vertically tiles those areas that extend past its bitmap.</summary>
					virtual property BrushExtendMode ExtendModeY
					{
						BrushExtendMode get();
						void set(BrushExtendMode value);
					}

					/// <summary>Exposes the interpolation method used when the brush bitmap is scaled or rotated.</summary>
					virtual property BitmapInterpolationMode InterpolationMode
					{
						BitmapInterpolationMode get();
						void set(BitmapInterpolationMode value);
					}
				#pragma endregion


				#pragma region Construction
				internal:
					/// <summary>Definition constructor</summary>
					/// <param name="pointer">Pointer to the Direct2D brush</param>
					BitmapBrush(ID2D1BitmapBrush* pointer);
				#pragma endregion


				#pragma region Destruction
				public:
					/// <summary>Destructor</summary>
					/// <remarks>Dispose()</remarks>
					~BitmapBrush();

					/// <summary>Destructor</summary>
					/// <remarks>Finalize()</remarks>
					!BitmapBrush();

					/// <summary>Destructor logic, disposes the object</summary>
					virtual void DisposeUnmanaged() override;
				#pragma endregion
				};
			}
		}
	}
}

#endif
