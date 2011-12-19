
#include "Point2dF.h"

#pragma region Properties
/// <summary>Represents the X coordinate of the point in question</summary>
System::Single Bardez::Projects::DirectX::Direct2D::Point2dF::X::get()
{
	return this->x;
}

/// <summary>Represents the X coordinate of the point in question</summary>
void Bardez::Projects::DirectX::Direct2D::Point2dF::X::set(System::Single value)
{
	this->x = value;
}

/// <summary>Represents the Y coordinate of the point in question</summary>
System::Single Bardez::Projects::DirectX::Direct2D::Point2dF::Y::get()
{
	return this->y;
}

/// <summary>Represents the Y coordinate of the point in question</summary>
void Bardez::Projects::DirectX::Direct2D::Point2dF::Y::set(System::Single value)
{
	this->y = value;
}
#pragma endregion

#pragma region Construction
/// <summary>Definition constructor</summary>
/// <param name="x">X coordinate of the point, as a single-precision floating point value</param>
/// <param name="y">Y coordinate of the point, as a single-precision floating point value</param>
Bardez::Projects::DirectX::Direct2D::Point2dF::Point2dF(System::Single x, System::Single y)
{
	this->x = x;
	this->y = y;
}

/// <summary>Definition constructor</summary>
/// <param name="point">System.Drawing struct containing a two dimensional point</param>
Bardez::Projects::DirectX::Direct2D::Point2dF::Point2dF(System::Drawing::PointF point)
{
	this->x = point.X;
	this->y = point.Y;
}

/// <summary>Unmanaged constructor</summary>
/// <param name="point">D2D_SIZE_U source structure</param>
Bardez::Projects::DirectX::Direct2D::Point2dF::Point2dF(D2D_POINT_2F point)
{
	this->x = point.x;
	this->y = point.y;
}
#pragma endregion


#pragma region Methods
/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
/// <returns>An unmanaged Direct2D struct equivalent</returns>
D2D_POINT_2F Bardez::Projects::DirectX::Direct2D::Point2dF::ToUnmanaged()
{
	D2D_POINT_2F point;
	point.x = this->x;
	point.y = this->y;
	
	return point;
}
#pragma endregion
