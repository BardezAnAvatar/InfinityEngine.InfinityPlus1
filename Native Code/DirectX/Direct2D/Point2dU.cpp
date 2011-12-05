
#include "Point2dU.h"

#pragma region Properties
/// <summary>Represents the X coordinate of the point in question</summary>
System::UInt32 Bardez::Projects::DirectX::Direct2D::Point2dU::X::get()
{
	return this->x;
}

/// <summary>Represents the X coordinate of the point in question</summary>
void Bardez::Projects::DirectX::Direct2D::Point2dU::X::set(System::UInt32 value)
{
	this->x = value;
}

/// <summary>Represents the Y coordinate of the point in question</summary>
System::UInt32 Bardez::Projects::DirectX::Direct2D::Point2dU::Y::get()
{
	return this->y;
}

/// <summary>Represents the Y coordinate of the point in question</summary>
void Bardez::Projects::DirectX::Direct2D::Point2dU::Y::set(System::UInt32 value)
{
	this->y = value;
}
#pragma endregion

#pragma region Construction
/// <summary>Definition constructor</summary>
/// <param name="x">X coordinate of the point, as an integer value</param>
/// <param name="y">Y coordinate of the point, as an integer value</param>
Bardez::Projects::DirectX::Direct2D::Point2dU::Point2dU(System::UInt32 x, System::UInt32 y)
{
	this->x = x;
	this->y = y;
}

/// <summary>Definition constructor</summary>
/// <param name="point">System.Drawing struct containing a two dimensional point</param>
Bardez::Projects::DirectX::Direct2D::Point2dU::Point2dU(System::Drawing::Point point)
{
	this->x = point.X;
	this->y = point.Y;
}

/// <summary>Unmanaged constructor</summary>
/// <param name="point">D2D_SIZE_U source structure</param>
Bardez::Projects::DirectX::Direct2D::Point2dU::Point2dU(D2D_POINT_2U point)
{
	this->x = point.x;
	this->y = point.y;
}
#pragma endregion


#pragma region Methods
/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
/// <returns>An unmanaged Direct2D struct euqivalent</returns>
D2D_POINT_2U Bardez::Projects::DirectX::Direct2D::Point2dU::ToUnmanaged()
{
	D2D_POINT_2U point;
	point.x = this->x;
	point.y = this->y;
	
	return point;
}
#pragma endregion
