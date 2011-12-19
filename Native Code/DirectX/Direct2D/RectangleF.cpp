
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

/// <summary>Exposes a rectangle that has its upper-left corner set to (negative infinity, negative infinity) and its lower-right corner set to (infinity, infinity).</summary>
/// <remarks>System::Single::NegativeInfinity and System::Single::PositiveInfinity yield c4756 overflow in constant arithmetic warnings</remarks>
Bardez::Projects::DirectX::Direct2D::RectangleF^ Bardez::Projects::DirectX::Direct2D::RectangleF::InfiniteRectangle::get()
{
	System::Single zero = 0.0F;
	System::Single pos = 1.0F / zero;
	System::Single neg = -1.0F / zero;
	return gcnew RectangleF(neg, neg, pos, pos);
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
/// <param name="size">Size of the rectangle, origin of 0,0</param>
Bardez::Projects::DirectX::Direct2D::RectangleF::RectangleF(Bardez::Projects::DirectX::Direct2D::SizeF^ size)
{
	this->left = 0.0F;
	this->right = size->Width > 0.0F ? size->Width - 1.0F : 0.0F;
	this->top = 0.0F;
	this->bottom = size->Height > 0.0F ? size->Height - 1.0F : 0.0F;
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

/// <summary>Definition constructor</summary>
/// <param name="rectangle">System.Drawing struct containing a rectangle</param>
Bardez::Projects::DirectX::Direct2D::RectangleF::RectangleF(System::Drawing::Rectangle rectangle)
{
	this->left = System::Convert::ToSingle(rectangle.Left);
	this->right = System::Convert::ToSingle(rectangle.Right);
	this->top = System::Convert::ToSingle(rectangle.Top);
	this->bottom = System::Convert::ToSingle(rectangle.Bottom);
}

/// <summary>Unmanaged constructor</summary>
/// <param name="rectangle">D2D_RECT_F source structure</param>
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
/// <returns>An unmanaged Direct2D struct equivalent</returns>
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
