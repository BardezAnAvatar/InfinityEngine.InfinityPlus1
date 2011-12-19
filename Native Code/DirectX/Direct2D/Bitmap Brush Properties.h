
#ifndef Bardez_Projects_DirectX_Direct2D_BitmapBrushProperties
#define Bardez_Projects_DirectX_Direct2D_BitmapBrushProperties

#include <d2d1.h>
#include "Brush Extend Mode.h"
#include "Bitmap Interpolation Mode.h"

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Describes the extend modes and the interpolation mode of  a brush.</summary>
				public ref class BitmapBrushProperties
				{
				#pragma region Fields
				protected:
					/// <summary>A value that describes how the brush horizontally tiles those areas that extend past its bitmap.</summary>
					BrushExtendMode extendModeX;

					/// <summary>A value that describes how the brush vertically tiles those areas that extend past its bitmap.</summary>
					BrushExtendMode extendModeY;

					/// <summary>A value that specifies how the bitmap is interpolated when it is scaled or rotated.</summary>
					BitmapInterpolationMode interpolationMode;
				#pragma endregion

					
				#pragma region Properties
				public:
					/// <summary>A value that describes how the brush horizontally tiles those areas that extend past its bitmap.</summary>
					virtual property BrushExtendMode ExtendModeX
					{
						BrushExtendMode get();
						void set(BrushExtendMode value);
					}

					/// <summary>A value that describes how the brush vertically tiles those areas that extend past its bitmap.</summary>
					virtual property BrushExtendMode ExtendModeY
					{
						BrushExtendMode get();
						void set(BrushExtendMode value);
					}

					/// <summary>A value that specifies how the bitmap is interpolated when it is scaled or rotated.</summary>
					virtual property BitmapInterpolationMode InterpolationMode
					{
						BitmapInterpolationMode get();
						void set(BitmapInterpolationMode value);
					}
				#pragma endregion


				#pragma region Construction
				public:
					/// <summary>Default Constructor</summary>
					BitmapBrushProperties();
					
					/// <summary>Definition Constructor</summary>
					/// <param name="extendX">A value that specifies how the brush horizontally tiles those areas that extend past its bitmap.</param>
					/// <param name="extendY">A value that specifies how the brush vertically tiles those areas that extend past its bitmap.</param>
					/// <param name="interpolationMode">A value that specifies the interpolation algorithm that is used when images are scaled or rotated.</param>
					BitmapBrushProperties(BrushExtendMode extendX, BrushExtendMode extendY, BitmapInterpolationMode interpolationMode);
					
					/// <summary>Definition Constructor</summary>
					/// <param name="extendX">A value that specifies how the brush horizontally tiles those areas that extend past its bitmap.</param>
					/// <param name="extendY">A value that specifies how the brush vertically tiles those areas that extend past its bitmap.</param>
					BitmapBrushProperties(BrushExtendMode extendX, BrushExtendMode extendY);
					
					/// <summary>Definition Constructor</summary>
					/// <param name="extendX">A value that specifies how the brush horizontally tiles those areas that extend past its bitmap.</param>
					BitmapBrushProperties(BrushExtendMode extendX);

				internal:
					/// <summary>Unmanaged Constructor</summary>
					/// <param name="unmanaged">Unmanaged source structure</param>
					BitmapBrushProperties(D2D1_BITMAP_BRUSH_PROPERTIES unmanaged);
				#pragma endregion


				#pragma region Methods
				internal:
					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct equivalent</returns>
					D2D1_BITMAP_BRUSH_PROPERTIES ToUnmanaged();
				#pragma endregion
				};
			}
		}
	}
}

#endif
