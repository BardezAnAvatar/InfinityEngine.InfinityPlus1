
#include "Ellipse.h"

using namespace Bardez::Projects::DirectX::Direct2D;


#pragma region Properties
/// <summary>The center point of the ellipse.</summary>
Point2dF^ Ellipse::Point::get()
{
	return this->point;
}

/// <summary>The center point of the ellipse.</summary>
void Ellipse::Point::set(Point2dF^ value)
{
	this->point = value;
}

/// <summary>The X-radius of the ellipse.</summary>
System::Single Ellipse::RadiusX::get()
{
	return this->radiusX;
}

/// <summary>The X-radius of the ellipse.</summary>
void Ellipse::RadiusX::set(System::Single value)
{
	this->radiusX = value;
}

/// <summary>The Y-radius of the ellipse.</summary>
System::Single Ellipse::RadiusY::get()
{
	return this->radiusY;
}

/// <summary>The Y-radius of the ellipse.</summary>
void Ellipse::RadiusY::set(System::Single value)
{
	this->radiusY = value;
}
#pragma endregion


#pragma region Construction
/// <summary>Definition constructor</summary>
/// <param name="center">The center point of the ellipse.</param>
/// <param name="radiusX">The X-radius of the ellipse.</param>
/// <param name="radiusY">The Y-radius of the ellipse.</param>
Ellipse::Ellipse(Point2dF^ center, System::Single radiusX, System::Single radiusY)
{
	this->point = center;
	this->radiusX = radiusX;
	this->radiusY = radiusY;
}

/// <summary>Unmanaged Constructor</summary>
/// <param name="unmanaged">Unmanaged source structure</param>
Ellipse::Ellipse(D2D1_ELLIPSE unmanaged)
{
	this->point = gcnew Point2dF(unmanaged.point);
	this->radiusX = unmanaged.radiusX;
	this->radiusY = unmanaged.radiusY;
}
#pragma endregion


#pragma region Methods
/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
/// <returns>An unmanaged Direct2D struct equivalent</returns>
D2D1_ELLIPSE Ellipse::ToUnmanaged()
{
	D2D1_ELLIPSE ellipse;

	ellipse.point = this->point->ToUnmanaged();
	ellipse.radiusX = this->radiusX;
	ellipse.radiusY = this->radiusY;

	return ellipse;
}
#pragma endregion
