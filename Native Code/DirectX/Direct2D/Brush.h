
#ifndef Bardez_Projects_DirectX_Direct2D_Brush
#define Bardez_Projects_DirectX_Direct2D_Brush

#include "Matrix3x2F.h"

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a Brush rendering aid for Direct2D</summary>
				public ref class Brush //abstract //abstract gets in the way of the COM interop object model
				{
				#pragma region Fields
				protected:
					/// <summary>Represents the native handle to the ID2D1Brush object</summary>
					System::IntPtr brushPtr;
				#pragma endregion


				#pragma region Properties
				internal:
					/// <summary>Exposes the native handle to the ID2D1Brush object</summary>
					property ID2D1Brush* BrushPtr
					{
						ID2D1Brush* get();
						void set(ID2D1Brush* value);
					}

				public:
					/// <summary>Exposes the opacity of this brush.</summary>
					property System::Single Opacity
					{
						System::Single get();
						void set(System::Single value);
					}
					
					/// <summary>Exposes the transformation applied to the brush.</summary>
					property Matrix3x2F^ Transformation
					{
						Matrix3x2F^ get();
						void set(Matrix3x2F^ value);
					}
				#pragma endregion


				#pragma region Construction
				internal:
					/// <summary>Definition constructor</summary>
					/// <param name="pointer">Pointer to the Direct2D brush</param>
					Brush(ID2D1Brush* pointer);
				#pragma endregion


				#pragma region Destruction
				public:
					/// <summary>Destructor</summary>
					/// <remarks>Dispose()</remarks>
					~Brush();

					/// <summary>Destructor</summary>
					/// <remarks>Finalize()</remarks>
					!Brush();

					/// <summary>Destructor logic, disposes the object</summary>
					virtual void DisposeUnmanaged();
				#pragma endregion


				#pragma region Methods
				protected:
					/// <summary>Gets the degree of opacity of this brush.</summary>
					/// <returns>
					///		A value between zero and 1 that indicates the opacity of the brush.
					///		This value is a constant multiplier that linearly scales the alpha value of all pixels filled by the brush.
					///		The opacity values are clamped in the range 0–1 before they are multipled together.
					/// </returns>
					virtual System::Single GetOpacity();

					/// <summary>Gets the transform applied to this brush.</summary>
					/// <returns>The transform applied to this brush.</returns>
					virtual Matrix3x2F^ GetTransform();

					/// <summary>Sets the degree of opacity of this brush.</summary>
					/// <param name="opacity">
					///		A value between zero and 1 that indicates the opacity of the brush.
					///		This value is a constant multiplier that linearly scales the alpha value of all pixels filled by the brush.
					///		The opacity values are clamped in the range 0–1 before they are multipled together. 
					/// </param>
					virtual void SetOpacity(System::Single opacity);

					/// <summary>Sets the transformation applied to the brush.</summary>
					/// <param name="transform">The transformation to apply to this brush. </param>
					virtual void SetTransform(Matrix3x2F^ transform);
				#pragma endregion
				};
			}
		}
	}
}

#endif
