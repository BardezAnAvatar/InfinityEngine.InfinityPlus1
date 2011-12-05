
#include "Bitmap Properties.h"

#pragma region Properties
/// <summary>Repesents the binary pixel data and alpha channel formats</summary>
Bardez::Projects::DirectX::Direct2D::PixelFormat^ Bardez::Projects::DirectX::Direct2D::BitmapProperties::Pixel_Format::get()
{
	return this->pixelFormat;
}

/// <summary>Repesents the binary pixel data and alpha channel formats</summary>
void Bardez::Projects::DirectX::Direct2D::BitmapProperties::Pixel_Format::set(Bardez::Projects::DirectX::Direct2D::PixelFormat^ value)
{
	this->pixelFormat = value;
}

/// <summary>Represents the DPI resolution of the bitmap.</summary>
Bardez::Projects::DirectX::Direct2D::DpiResolution Bardez::Projects::DirectX::Direct2D::BitmapProperties::DPI::get()
{
	return this->dpi;
}

/// <summary>Represents the DPI resolution of the bitmap.</summary>
void Bardez::Projects::DirectX::Direct2D::BitmapProperties::DPI::set(Bardez::Projects::DirectX::Direct2D::DpiResolution value)
{
	this->dpi = value;
}
#pragma endregion


#pragma region Construction
/// <summary>Definition constructor</summary>
/// <param name="format">Pixel format of the data</param>
/// <param name="dpiHorizontal">Horizontal DPI of the bitmap</param>
/// <param name="dpiVertical">Vertical DPI of the bitmap</param>
Bardez::Projects::DirectX::Direct2D::BitmapProperties::BitmapProperties(Bardez::Projects::DirectX::Direct2D::PixelFormat^ format, System::Single dpiHorizontal, System::Single dpiVertical)
{
	this->pixelFormat = format;
	this->dpi = Bardez::Projects::DirectX::Direct2D::DpiResolution(dpiHorizontal, dpiVertical);
}

/// <summary>Definition constructor</summary>
/// <param name="format">Binary pixel format</param>
/// <param name="alphaMode">Alpha transparency mode</param>
/// <param name="dpiHorizontal">Horizontal DPI of the bitmap</param>
/// <param name="dpiVertical">Vertical DPI of the bitmap</param>
Bardez::Projects::DirectX::Direct2D::BitmapProperties::BitmapProperties(Bardez::Projects::DirectX::Direct2D::DXGI_ChannelFormat format, Bardez::Projects::DirectX::Direct2D::AlphaMode alphaMode, System::Single dpiHorizontal, System::Single dpiVertical)
{
	this->pixelFormat = gcnew Bardez::Projects::DirectX::Direct2D::PixelFormat(format, alphaMode);
	this->dpi = Bardez::Projects::DirectX::Direct2D::DpiResolution(dpiHorizontal, dpiVertical);
}

/// <summary>Unmanaged constructor</summary>
/// <param name="properties">Unmanaged souce structure</param>
Bardez::Projects::DirectX::Direct2D::BitmapProperties::BitmapProperties(D2D1_BITMAP_PROPERTIES properties)
{
	this->pixelFormat = gcnew Bardez::Projects::DirectX::Direct2D::PixelFormat(properties.pixelFormat);
	this->dpi = Bardez::Projects::DirectX::Direct2D::DpiResolution(properties.dpiX, properties.dpiY);
}
#pragma endregion


#pragma region Methods
/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
/// <returns>An unmanaged Direct2D struct euqivalent</returns>
D2D1_BITMAP_PROPERTIES Bardez::Projects::DirectX::Direct2D::BitmapProperties::ToUnmanaged()
{
	D2D1_BITMAP_PROPERTIES properties;

	properties.pixelFormat = this->pixelFormat->ToUnmanaged();
	properties.dpiX = this->dpi.X;
	properties.dpiY = this->dpi.Y;

	return properties;
}
#pragma endregion

