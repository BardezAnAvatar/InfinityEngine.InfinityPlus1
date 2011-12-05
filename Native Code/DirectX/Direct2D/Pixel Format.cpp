
#include "Pixel Format.h"

#pragma region Properties
/// <summary>Represents the pixel format of the binary data of the pixel(s)</summary>
Bardez::Projects::DirectX::Direct2D::DXGI_ChannelFormat Bardez::Projects::DirectX::Direct2D::PixelFormat::Format::get()
{
	return this->format;
}

/// <summary>Represents the pixel format of the binary data of the pixel(s)</summary>
void Bardez::Projects::DirectX::Direct2D::PixelFormat::Format::set(DXGI_ChannelFormat value)
{
	this->format = value;
}

/// <summary>Represents the alpha transparency format of the binary data of the pixel(s)</summary>
Bardez::Projects::DirectX::Direct2D::AlphaMode Bardez::Projects::DirectX::Direct2D::PixelFormat::AlphaType::get()
{
	return this->alphaType;
}

/// <summary>Represents the alpha transparency format of the binary data of the pixel(s)</summary>
void Bardez::Projects::DirectX::Direct2D::PixelFormat::AlphaType::set(AlphaMode value)
{
	this->alphaType = value;
}
#pragma endregion

#pragma region Construction
/// <summary>Definition constructor</summary>
/// <param name="format">Pixel format of the data</param>
/// <param name="alphaMode">Alpha transparency mode of the data</param>
Bardez::Projects::DirectX::Direct2D::PixelFormat::PixelFormat(Bardez::Projects::DirectX::Direct2D::DXGI_ChannelFormat format, Bardez::Projects::DirectX::Direct2D::AlphaMode alphaMode)
{
	this->format = format;
	this->alphaType = alphaMode;
}

/// <summary>Unmanaged constructor</summary>
/// <param name="size">D2D1_PIXEL_FORMAT source structure</param>
Bardez::Projects::DirectX::Direct2D::PixelFormat::PixelFormat(D2D1_PIXEL_FORMAT format)
{
	this->format = (DXGI_ChannelFormat)(format.format);
	this->alphaType = (AlphaMode)(format.alphaMode);
}
#pragma endregion


#pragma region Methods
/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
/// <returns>An unmanaged Direct2D struct euqivalent</returns>
D2D1_PIXEL_FORMAT Bardez::Projects::DirectX::Direct2D::PixelFormat::ToUnmanaged()
{
	D2D1_PIXEL_FORMAT PixelFormat;
	PixelFormat.format = (DXGI_FORMAT)(UINT32)(this->format);
	PixelFormat.alphaMode = (D2D1_ALPHA_MODE)(UINT32)(this->alphaType);
	
	return PixelFormat;
}
#pragma endregion
