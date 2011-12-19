
#include "SizeF.h"

#pragma region Properties
/// <summary>Represents the width of the size in question</summary>
System::Single Bardez::Projects::DirectX::Direct2D::SizeF::Width::get()
{
	return this->width;
}

/// <summary>Represents the width of the size in question</summary>
void Bardez::Projects::DirectX::Direct2D::SizeF::Width::set(System::Single value)
{
	this->width = value;
}

/// <summary>Represents the height of the size in question</summary>
System::Single Bardez::Projects::DirectX::Direct2D::SizeF::Height::get()
{
	return this->height;
}

/// <summary>Represents the height of the size in question</summary>
void Bardez::Projects::DirectX::Direct2D::SizeF::Height::set(System::Single value)
{
	this->height = value;
}
#pragma endregion

#pragma region Construction
/// <summary>Definition constructor</summary>
/// <param name="width">Width of the size, as a single-precision floating point value</param>
/// <param name="height">Height of the size, as a single-precision floating point value</param>
Bardez::Projects::DirectX::Direct2D::SizeF::SizeF(System::Single width, System::Single height)
{
	this->width = width;
	this->height = height;
}

/// <summary>Definition constructor</summary>
/// <param name="size">System.Drawing struct containing size</param>
Bardez::Projects::DirectX::Direct2D::SizeF::SizeF(System::Drawing::SizeF size)
{
	this->width = size.Width;
	this->height = size.Height;
}

/// <summary>Unmanaged constructor</summary>
/// <param name="size">D2D_SIZE_U source structure</param>
Bardez::Projects::DirectX::Direct2D::SizeF::SizeF(D2D_SIZE_F size)
{
	this->width = size.width;
	this->height = size.height;
}
#pragma endregion


#pragma region Methods
/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
/// <returns>An unmanaged Direct2D struct equivalent</returns>
D2D_SIZE_F Bardez::Projects::DirectX::Direct2D::SizeF::ToUnmanaged()
{
	D2D_SIZE_F SizeF;
	SizeF.width = this->width;
	SizeF.height = this->height;
	
	return SizeF;
}
#pragma endregion
