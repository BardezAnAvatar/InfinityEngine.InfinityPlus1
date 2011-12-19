
#ifndef Bardez_Projects_DirectX_Direct2D_BrushProperties
#define Bardez_Projects_DirectX_Direct2D_BrushProperties

#include <d2d1.h>
#include "Matrix3x2F.h"

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Describes the opacity and transformation of a brush.</summary>
				public ref class BrushProperties
				{
				#pragma region Fields
				protected:
					/// <summary>A value between 0.0f and 1.0f, inclusive, that specifies the degree of opacity of the brush.</summary>
					System::Single opacity;
					
					/// <summary>The transformation that is applied to the brush.</summary>
					Matrix3x2F^ transform;
				#pragma endregion

					
				#pragma region Properties
				public:
					/// <summary>A value between 0.0f and 1.0f, inclusive, that specifies the degree of opacity of the brush.</summary>
					property System::Single Opacity
					{
						System::Single get();
						void set(System::Single value);
					}
					
					/// <summary>The transformation that is applied to the brush.</summary>
					property Matrix3x2F^ Transform
					{
						Matrix3x2F^ get();
						void set(Matrix3x2F^ value);
					}
				#pragma endregion


				#pragma region Construction
				public:
					/// <summary>Default Constructor</summary>
					BrushProperties();
					
					/// <summary>Definition Constructor</summary>
					/// <param name="opacity">The base opacity of the brush.</param>
					/// <param name="transform">The transformation to apply to the brush.</param>
					BrushProperties(System::Single opacity, Matrix3x2F^ transform);
					
					/// <summary>Definition Constructor</summary>
					/// <param name="opacity">The base opacity of the brush.</param>
					BrushProperties(System::Single opacity);

				internal:
					/// <summary>Unmanaged Constructor</summary>
					/// <param name="unmanaged">Unmanaged source structure</param>
					BrushProperties(D2D1_BRUSH_PROPERTIES unmanaged);
				#pragma endregion


				#pragma region Methods
				internal:
					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct equivalent</returns>
					D2D1_BRUSH_PROPERTIES ToUnmanaged();
				#pragma endregion
				};
			}
		}
	}
}

#endif
