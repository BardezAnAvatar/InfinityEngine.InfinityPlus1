
#include "RectangleF.h"

#pragma region Properties
/// <summary>Represents the left X coordinate of the rectangle in question</summary>
System::Single Bardez::Projects::DirectX::Direct2D::RectangleF::Left::get()
{
	return this->left;
}

/// <summary>Represents the left X coordinate of the rectangle in question</summary>
void Bardez::Projects::DirectX::Direct2D::RectangleF::Left::set(System::Single value)
{
	this->left = value;
}

/// <summary>Represents the right X coordinate of the rectangle in question</summary>
System::Single Bardez::Projects::DirectX::Direct2D::RectangleF::Right::get()
{
	return this->right;
}

/// <summary>Represents the right X coordinate of the rectangle in question</summary>
void Bardez::Projects::DirectX::Direct2D::RectangleF::Right::set(System::Single value)
{
	this->right = value;
}

/// <summary>Represents the top Y coordinate of the rectangle in question</summary>
System::Single Bardez::Projects::DirectX::Direct2D::RectangleF::Top::get()
{
	return this->top;
}

/// <summary>Represents the top Y coordinate of the rectangle in question</summary>
void Bardez::Projects::DirectX::Direct2D::RectangleF::Top::set(System::Single value)
{
	this->top = value;
}

/// <summary>Represents the bottom Y coordinate of the rectangle in question</summary>
System::Single Bardez::Projects::DirectX::Direct2D::RectangleF::Bottom::get()
{
	return this->bottom;
}

/// <summary>Represents the bottom Y coordinate of the rectangle in question</summary>
void Bardez::Projects::DirectX::Direct2D::RectangleF::Bottom::set(System::Single value)
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
Bardez::Projects::DirectX::Direct2D::RectangleF::RectangleF(System::Single left, System::Single right, System::Single top, System::Single bottom)
{
	this->left = left;
	this->right = right;
	this->top = top;
	this->bottom = bottom;
}

/// <summary>Definition constructor</summary>
/// <param name="rectangle">System.Drawing struct containing a rectangle</param>
Bardez::Projects::DirectX::Direct2D::RectangleF::RectangleF(System::Drawing::RectangleF rectangle)
{
	this->left = rectangle.Left;
	this->right = rectangle.Right;
	this->top = rectangle.Top;
	this->bottom = rectangle.Bottom;
}

/// <summary>Unmanaged constructor</summary>
/// <param name="point">D2D_RECT_F source structure</param>
Bardez::Projects::DirectX::Direct2D::RectangleF::RectangleF(D2D_RECT_F rectangle)
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
D2D_RECT_F Bardez::Projects::DirectX::Direct2D::RectangleF::ToUnmanaged()
{
	D2D_RECT_F rectangle;
	
	rectangle.left = this->left;
	rectangle.right = this->right;
	rectangle.top = this->top;
	rectangle.bottom = this->bottom;
	
	return rectangle;
}
#pragma endregion
