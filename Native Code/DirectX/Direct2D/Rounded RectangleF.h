
#ifndef Bardez_Projects_DirectX_Direct2D_RoundedRectangleF
#define Bardez_Projects_DirectX_Direct2D_RoundedRectangleF

#include <D2d1.h>
#include "RectangleF.h"

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Contains the dimensions and corner radii of a rounded rectangle. Represents a managed equivalent for the Direct2D D2D1_ROUNDED_RECT structure.</summary>
				public ref class RoundedRectangleF : RectangleF
				{
				#pragma region Fields
				protected:
					/// <summary>The x-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</summary>
					System::Single radiusX;

					/// <summary>The y-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</summary>
					System::Single radiusY;
				#pragma endregion


				#pragma region Properties
				public:
					/// <summary>The x-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</summary>
					virtual property System::Single RadiusX
					{
						System::Single get();
						void set(System::Single value);
					}

					/// <summary>The y-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</summary>
					virtual property System::Single RadiusY
					{
						System::Single get();
						void set(System::Single value);
					}
				#pragma endregion


				#pragma region Construction
				public:
					/// <summary>Definition constructor</summary>
					/// <param name="left">left X coordinate of the rectangle, as a single-precision floating point value</param>
					/// <param name="right">right X coordinate of the rectangle, as a single-precision floating point value</param>
					/// <param name="top">top Y coordinate of the rectangle, as a single-precision floating point value</param>
					/// <param name="bottom">bottom Y coordinate of the rectangle, as a single-precision floating point value</param>
					/// <param name="radiusX">The x-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</param>
					/// <param name="radiusY">The y-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</param>
					RoundedRectangleF(System::Single left, System::Single right, System::Single top, System::Single bottom, System::Single radiusX, System::Single radiusY);

					/// <summary>Definition constructor</summary>
					/// <param name="size">Size of the rectangle, origin of 0,0</param>
					/// <param name="radiusX">The x-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</param>
					/// <param name="radiusY">The y-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</param>
					RoundedRectangleF(SizeF^ size, System::Single radiusX, System::Single radiusY);

					/// <summary>Definition constructor</summary>
					/// <param name="rectangle">System.Drawing struct containing a rectangle</param>
					/// <param name="radiusX">The x-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</param>
					/// <param name="radiusY">The y-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</param>
					RoundedRectangleF(System::Drawing::RectangleF rectangle, System::Single radiusX, System::Single radiusY);

					/// <summary>Definition constructor</summary>
					/// <param name="rectangle">System.Drawing struct containing a rectangle</param>
					/// <param name="radiusX">The x-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</param>
					/// <param name="radiusY">The y-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</param>
					RoundedRectangleF(System::Drawing::Rectangle rectangle, System::Single radiusX, System::Single radiusY);

					/// <summary>Definition constructor</summary>
					/// <param name="left">left X coordinate of the rectangle, as a single-precision floating point value</param>
					/// <param name="right">right X coordinate of the rectangle, as a single-precision floating point value</param>
					/// <param name="top">top Y coordinate of the rectangle, as a single-precision floating point value</param>
					/// <param name="bottom">bottom Y coordinate of the rectangle, as a single-precision floating point value</param>
					RoundedRectangleF(System::Single left, System::Single right, System::Single top, System::Single bottom);

					/// <summary>Definition constructor</summary>
					/// <param name="size">Size of the rectangle, origin of 0,0</param>
					RoundedRectangleF(SizeF^ size);

					/// <summary>Definition constructor</summary>
					/// <param name="rectangle">System.Drawing struct containing a rectangle</param>
					RoundedRectangleF(System::Drawing::RectangleF rectangle);

					/// <summary>Definition constructor</summary>
					/// <param name="rectangle">System.Drawing struct containing a rectangle</param>
					RoundedRectangleF(System::Drawing::Rectangle rectangle);

				internal:
					/// <summary>Unmanaged constructor</summary>
					/// <param name="rectangle">Unmanaged source structure</param>
					RoundedRectangleF(D2D1_ROUNDED_RECT rectangle);

					/// <summary>Unmanaged constructor</summary>
					/// <param name="rectangle">D2D_RECT_F source structure</param>
					/// <param name="radiusX">The x-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</param>
					/// <param name="radiusY">The y-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</param>
					RoundedRectangleF(D2D_RECT_F rectangle, System::Single radiusX, System::Single radiusY);
				#pragma endregion

				#pragma region Methods
				internal:
					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct equivalent</returns>
					D2D1_ROUNDED_RECT ToUnmanaged();
				#pragma endregion
				};
			}
		}
	}
}

#endif
