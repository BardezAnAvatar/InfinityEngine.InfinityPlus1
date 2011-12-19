
#include "SizeU.h"

#pragma region Properties
/// <summary>Represents the width of the size in question</summary>
System::UInt32 Bardez::Projects::DirectX::Direct2D::SizeU::Width::get()
{
	return this->width;
}

/// <summary>Represents the width of the size in question</summary>
void Bardez::Projects::DirectX::Direct2D::SizeU::Width::set(System::UInt32 value)
{
	this->width = value;
}

/// <summary>Represents the height of the size in question</summary>
System::UInt32 Bardez::Projects::DirectX::Direct2D::SizeU::Height::get()
{
	return this->height;
}

/// <summary>Represents the height of the size in question</summary>
void Bardez::Projects::DirectX::Direct2D::SizeU::Height::set(System::UInt32 value)
{
	this->height = value;
}
#pragma endregion


#pragma region Construction
/// <summary>Definition constructor</summary>
/// <param name="width">Width of the size, as an integer</param>
/// <param name="height">Height of the size, as an integer</param>
Bardez::Projects::DirectX::Direct2D::SizeU::SizeU(System::UInt32 width, System::UInt32 height)
{
	this->width = width;
	this->height = height;
}

/// <summary>Definition constructor</summary>
/// <param name="size">System.Drawing struct containing size</param>
Bardez::Projects::DirectX::Direct2D::SizeU::SizeU(System::Drawing::Size size)
{
	this->width = size.Width;
	this->height = size.Height;
}

/// <summary>Unmanaged constructor</summary>
/// <param name="size">D2D_SIZE_U source structure</param>
Bardez::Projects::DirectX::Direct2D::SizeU::SizeU(D2D_SIZE_U size)
{
	this->width = size.width;
	this->height = size.height;
}
#pragma endregion


#pragma region Methods
/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
/// <returns>An unmanaged Direct2D struct equivalent</returns>
D2D_SIZE_U Bardez::Projects::DirectX::Direct2D::SizeU::ToUnmanaged()
{
	D2D_SIZE_U sizeu;
	sizeu.width = this->width;
	sizeu.height = this->height;
	
	return sizeu;
}
#pragma endregion
