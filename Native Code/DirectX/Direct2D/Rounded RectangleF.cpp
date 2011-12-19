
#include "Rounded RectangleF.h"

using namespace Bardez::Projects::DirectX::Direct2D;


#pragma region Properties
/// <summary>The x-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</summary>
System::Single RoundedRectangleF::RadiusX::get()
{
	return this->radiusX;
}

/// <summary>The x-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</summary>
void RoundedRectangleF::RadiusX::set(System::Single value)
{
	this->radiusX = value;
}

/// <summary>The y-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</summary>
System::Single RoundedRectangleF::RadiusY::get()
{
	return this->radiusY;
}

/// <summary>The y-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</summary>
void RoundedRectangleF::RadiusY::set(System::Single value)
{
	this->radiusY = value;
}
#pragma endregion


#pragma region Construction
/// <summary>Definition constructor</summary>
/// <param name="left">left X coordinate of the rectangle, as a single-precision floating point value</param>
/// <param name="right">right X coordinate of the rectangle, as a single-precision floating point value</param>
/// <param name="top">top Y coordinate of the rectangle, as a single-precision floating point value</param>
/// <param name="bottom">bottom Y coordinate of the rectangle, as a single-precision floating point value</param>
/// <param name="radiusX">The x-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</param>
/// <param name="radiusY">The y-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</param>
RoundedRectangleF::RoundedRectangleF(System::Single left, System::Single right, System::Single top, System::Single bottom, System::Single radiusX, System::Single radiusY)
	: RectangleF(left, right, top, bottom)
{
	this->radiusX = radiusX;
	this->radiusY = radiusY;
}

/// <summary>Definition constructor</summary>
/// <param name="size">Size of the rectangle, origin of 0,0</param>
/// <param name="radiusX">The x-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</param>
/// <param name="radiusY">The y-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</param>
RoundedRectangleF::RoundedRectangleF(SizeF^ size, System::Single radiusX, System::Single radiusY)
	: RectangleF(size)
{
	this->radiusX = radiusX;
	this->radiusY = radiusY;
}

/// <summary>Definition constructor</summary>
/// <param name="rectangle">System.Drawing struct containing a rectangle</param>
/// <param name="radiusX">The x-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</param>
/// <param name="radiusY">The y-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</param>
RoundedRectangleF::RoundedRectangleF(System::Drawing::RectangleF rectangle, System::Single radiusX, System::Single radiusY)
	: RectangleF(rectangle)
{
	this->radiusX = radiusX;
	this->radiusY = radiusY;
}

/// <summary>Definition constructor</summary>
/// <param name="rectangle">System.Drawing struct containing a rectangle</param>
/// <param name="radiusX">The x-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</param>
/// <param name="radiusY">The y-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</param>
RoundedRectangleF::RoundedRectangleF(System::Drawing::Rectangle rectangle, System::Single radiusX, System::Single radiusY)
	: RectangleF(rectangle)
{
	this->radiusX = radiusX;
	this->radiusY = radiusY;
}

/// <summary>Definition constructor</summary>
/// <param name="left">left X coordinate of the rectangle, as a single-precision floating point value</param>
/// <param name="right">right X coordinate of the rectangle, as a single-precision floating point value</param>
/// <param name="top">top Y coordinate of the rectangle, as a single-precision floating point value</param>
/// <param name="bottom">bottom Y coordinate of the rectangle, as a single-precision floating point value</param>
RoundedRectangleF::RoundedRectangleF(System::Single left, System::Single right, System::Single top, System::Single bottom) : RectangleF(left, right, top, bottom) { }

/// <summary>Definition constructor</summary>
/// <param name="size">Size of the rectangle, origin of 0,0</param>
RoundedRectangleF::RoundedRectangleF(SizeF^ size) : RectangleF(size) { }

/// <summary>Definition constructor</summary>
/// <param name="rectangle">System.Drawing struct containing a rectangle</param>
RoundedRectangleF::RoundedRectangleF(System::Drawing::RectangleF rectangle) : RectangleF(rectangle) { }

/// <summary>Definition constructor</summary>
/// <param name="rectangle">System.Drawing struct containing a rectangle</param>
RoundedRectangleF::RoundedRectangleF(System::Drawing::Rectangle rectangle) : RectangleF(rectangle) { }

/// <summary>Unmanaged constructor</summary>
/// <param name="rectangle">Unmanaged source structure</param>
RoundedRectangleF::RoundedRectangleF(D2D1_ROUNDED_RECT rectangle) : RectangleF(rectangle.rect)
{
	this->radiusX = rectangle.radiusX;
	this->radiusY = rectangle.radiusY;
}

/// <summary>Unmanaged constructor</summary>
/// <param name="rectangle">D2D_RECT_F source structure</param>
/// <param name="radiusX">The x-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</param>
/// <param name="radiusY">The y-radius for the quarter ellipse that is drawn to replace every corner of the rectangle.</param>
RoundedRectangleF::RoundedRectangleF(D2D_RECT_F rectangle, System::Single radiusX, System::Single radiusY) : RectangleF(rectangle)
{
	this->radiusX = radiusX;
	this->radiusY = radiusY;
}
#pragma endregion


#pragma region Methods
/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
/// <returns>An unmanaged Direct2D struct equivalent</returns>
D2D1_ROUNDED_RECT RoundedRectangleF::ToUnmanaged()
{
	D2D1_ROUNDED_RECT roundedRect;

	roundedRect.rect = RectangleF::ToUnmanaged();

	return roundedRect;
}
#pragma endregion
