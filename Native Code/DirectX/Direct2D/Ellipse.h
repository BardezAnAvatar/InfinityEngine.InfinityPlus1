
#ifndef Bardez_Projects_DirectX_Direct2D_Ellipse
#define Bardez_Projects_DirectX_Direct2D_Ellipse

#include <D2d1.h>
#include "Point2dF.h"

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a managed equivalent for the Direct2D D2D1_ELLIPSE structure. Contains the center point, x-radius, and y-radius of an ellipse.</summary>
				public ref class Ellipse
				{
				#pragma region Fields
				protected:
					/// <summary>The center point of the ellipse.</summary>
					Point2dF^ point;

					/// <summary>The X-radius of the ellipse.</summary>
					System::Single radiusX;

					/// <summary>The Y-radius of the ellipse.</summary>
					System::Single radiusY;
				#pragma endregion


				#pragma region Properties
				public:
					/// <summary>The center point of the ellipse.</summary>
					virtual property Point2dF^ Point
					{
						Point2dF^ get();
						void set(Point2dF^ value);
					}

					/// <summary>The X-radius of the ellipse.</summary>
					virtual property System::Single RadiusX
					{
						System::Single get();
						void set(System::Single value);
					}

					/// <summary>The Y-radius of the ellipse.</summary>
					virtual property System::Single RadiusY
					{
						System::Single get();
						void set(System::Single value);
					}
				#pragma endregion


				#pragma region Construction
				public:
					/// <summary>Definition constructor</summary>
					/// <param name="center">The center point of the ellipse.</param>
					/// <param name="radiusX">The X-radius of the ellipse.</param>
					/// <param name="radiusY">The Y-radius of the ellipse.</param>
					Ellipse(Point2dF^ center, System::Single radiusX, System::Single radiusY);

				internal:
					/// <summary>Unmanaged Constructor</summary>
					/// <param name="unmanaged">Unmanaged source structure</param>
					Ellipse(D2D1_ELLIPSE unmanaged);
				#pragma endregion


				#pragma region Methods
				internal:
					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct equivalent</returns>
					virtual D2D1_ELLIPSE ToUnmanaged();
				#pragma endregion
				};
			}
		}
	}
}

#endif
