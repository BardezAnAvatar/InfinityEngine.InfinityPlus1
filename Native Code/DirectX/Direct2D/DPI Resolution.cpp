
#include "DPI Resolution.h"

#pragma region Properties
/// <summary>Horizontal DPI</summary>
System::Single Bardez::Projects::DirectX::Direct2D::DpiResolution::X::get()
{
	return this->x;
}

/// <summary>Horizontal DPI</summary>
void Bardez::Projects::DirectX::Direct2D::DpiResolution::X::set(System::Single value)
{
	this->x = value;
}

/// <summary>Vertical DPI</summary>
System::Single Bardez::Projects::DirectX::Direct2D::DpiResolution::Y::get()
{
	return this->y;
}

/// <summary>Vertical DPI</summary>
void Bardez::Projects::DirectX::Direct2D::DpiResolution::Y::set(System::Single value)
{
	this->y = value;
}
#pragma endregion


#pragma region Contruction
/// <summary>Definition constructor</summary>
/// <param name="dpiX">Horizontal DPI</param>
/// <param name="dpiY">Vertical DPI</param>
Bardez::Projects::DirectX::Direct2D::DpiResolution::DpiResolution(System::Single dpiX, System::Single dpiY)
{
	this->x = dpiX;
	this->y = dpiY;
}
#pragma endregion
