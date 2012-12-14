
#ifndef Bardez_Projects_DirectX_Direct2D_StrokeStyle
#define Bardez_Projects_DirectX_Direct2D_StrokeStyle

#include <D2D1.h>
#include "Cap Style.h"
#include "Dash Style.h"
#include "Line Joint.h"

using namespace Bardez::Projects::BasicStructures::Win32;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents an collection of ID2D1StrokeStyle objects for linear and radial gradient brushes.</summary>
				public ref class StrokeStyle
				{
				#pragma region Fields
				protected:
					/// <summary>Represents a pointer to a ID2D1StrokeStyle native interface</summary>
					System::IntPtr strokeStylePtr;
				#pragma endregion


				#pragma region Properties
				internal:
					/// <summary>Exposes a pointer to a ID2D1StrokeStyle native interface</summary>
					virtual property ID2D1StrokeStyle* StrokeStylePtr
					{
						ID2D1StrokeStyle* get();
						void set(ID2D1StrokeStyle* value);
					}

				public:
					/// <summary>Exposes a value that specifies how the ends of each dash are drawn.</summary>
					virtual property CapStyle DashCap
					{
						CapStyle get();
					}

					/// <summary>Exposes the type of shape used at the end of a stroke.</summary>
					virtual property CapStyle EndCap
					{
						CapStyle get();
					}

					/// <summary>Exposes the type of shape used at the beginning of a stroke.</summary>
					virtual property CapStyle StartCap
					{
						CapStyle get();
					}

					/// <summary>Exposes the number of entries in the dashes array.</summary>
					virtual property System::UInt32 DashesCount
					{
						System::UInt32 get();
					}

					/// <summary>Exposes a value that specifies how far in the dash sequence the stroke will start.</summary>
					virtual property System::Single DashOffset
					{
						System::Single get();
					}

					/// <summary>Exposes a value that describes the stroke's dash pattern.</summary>
					virtual property Bardez::Projects::DirectX::Direct2D::DashStyle DashStyle
					{
						Bardez::Projects::DirectX::Direct2D::DashStyle get();
					}

					/// <summary>Exposes the type of joint used at the vertices of a shape's outline.</summary>
					virtual property Bardez::Projects::DirectX::Direct2D::LineJoint LineJoint
					{
						Bardez::Projects::DirectX::Direct2D::LineJoint get();
					}

					/// <summary>Exposes the limit on the ratio of the miter length to half the stroke's thickness.</summary>
					virtual property System::Single MiterLimit
					{
						System::Single get();
					}
				#pragma endregion


				#pragma region Construction
				internal:
					/// <summary>Definition constructor</summary>
					/// <param name="pointer">ID2D1StrokeStyle source pointer</param>
					StrokeStyle(ID2D1StrokeStyle* pointer);
				#pragma endregion


				#pragma region Destruction
				public:
					/// <summary>Destructor</summary>
					/// <remarks>Dispose()</remarks>
					~StrokeStyle();

					/// <summary>Destructor</summary>
					/// <remarks>Finalize()</remarks>
					!StrokeStyle();

					/// <summary>Destructor logic, disposes the object</summary>
					virtual void DisposeUnmanaged();
				#pragma endregion


				#pragma region Methods
				public:
					/// <summary>Exposes the dash pattern to the specified array.</summary>
					/// <param name="dashesCount">The number of dashes to copy. If this value is less than the number of dashes in the stroke style's dashes array, the returned dashes are truncated to dashesCount. If this value is greater than the number of dashes in the stroke style's dashes array, the extra dashes are set to 0.0f.</param>
					/// <returns>A copied array of dash patterns.</returns>
					virtual array<System::Single>^ GetDashes(System::UInt32 dashesCount);
				#pragma endregion
				};
			}
		}
	}
}

#endif
