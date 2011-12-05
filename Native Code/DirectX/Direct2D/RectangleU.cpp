
#include "RectangleU.h"

#pragma region Properties
/// <summary>Represents the left X coordinate of the rectangle in question</summary>
System::UInt32 Bardez::Projects::DirectX::Direct2D::RectangleU::Left::get()
{
	return this->left;
}

/// <summary>Represents the left X coordinate of the rectangle in question</summary>
void Bardez::Projects::DirectX::Direct2D::RectangleU::Left::set(System::UInt32 value)
{
	this->left = value;
}

/// <summary>Represents the right X coordinate of the rectangle in question</summary>
System::UInt32 Bardez::Projects::DirectX::Direct2D::RectangleU::Right::get()
{
	return this->right;
}

/// <summary>Represents the right X coordinate of the rectangle in question</summary>
void Bardez::Projects::DirectX::Direct2D::RectangleU::Right::set(System::UInt32 value)
{
	this->right = value;
}

/// <summary>Represents the top Y coordinate of the rectangle in question</summary>
System::UInt32 Bardez::Projects::DirectX::Direct2D::RectangleU::Top::get()
{
	return this->top;
}

/// <summary>Represents the top Y coordinate of the rectangle in question</summary>
void Bardez::Projects::DirectX::Direct2D::RectangleU::Top::set(System::UInt32 value)
{
	this->top = value;
}

/// <summary>Represents the bottom Y coordinate of the rectangle in question</summary>
System::UInt32 Bardez::Projects::DirectX::Direct2D::RectangleU::Bottom::get()
{
	return this->bottom;
}

/// <summary>Represents the bottom Y coordinate of the rectangle in question</summary>
void Bardez::Projects::DirectX::Direct2D::RectangleU::Bottom::set(System::UInt32 value)
{
	this->bottom = value;
}
#pragma endregion


#pragma region Construction
/// <summary>Definition constructor</summary>
/// <param name="left">left X coordinate of the rectangle, as a single-precision floating point value</param>
/// <param name="right">right X coordinate of the rectangle, as a single-precision floating point value</param>
/// <param name="top">top Y coordinate of the rectangle, as a single-precision floating point value</param>
/// <param name="bottom">bottom Y coordinate of the rectangle, as a single-precision floating point value</param>
Bardez::Projects::DirectX::Direct2D::RectangleU::RectangleU(System::UInt32 left, System::UInt32 right, System::UInt32 top, System::UInt32 bottom)
{
	this->left = left;
	this->right = right;
	this->top = top;
	this->bottom = bottom;
}

/// <summary>Definition constructor</summary>
/// <param name="rectangle">System.Drawing struct containing a rectangle</param>
Bardez::Projects::DirectX::Direct2D::RectangleU::RectangleU(System::Drawing::Rectangle rectangle)
{
	this->left = rectangle.Left;
	this->right = rectangle.Right;
	this->top = rectangle.Top;
	this->bottom = rectangle.Bottom;
}

/// <summary>Unmanaged constructor</summary>
/// <param name="point">D2D_RECT_U source structure</param>
Bardez::Projects::DirectX::Direct2D::RectangleU::RectangleU(D2D_RECT_U rectangle)
{
	this->left = rectangle.left;
	this->right = rectangle.right;
	this->top = rectangle.top;
	this->bottom = rectangle.bottom;
}
#pragma endregion


#pragma region Methods
/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
/// <returns>An unmanaged Direct2D struct euqivalent</returns>
D2D_RECT_U Bardez::Projects::DirectX::Direct2D::RectangleU::ToUnmanaged()
{
	D2D_RECT_U rectangle;
	
	rectangle.left = this->left;
	rectangle.right = this->right;
	rectangle.top = this->top;
	rectangle.bottom = this->bottom;
	
	return rectangle;
}
#pragma endregion
