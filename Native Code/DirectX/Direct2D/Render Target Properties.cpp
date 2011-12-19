
#include "Render Target Properties.h"

#pragma region Properties
/// <summary>
///		A value that specifies whether the render target should force hardware or software rendering.
///		A value of D2D1_RENDER_TARGET_TYPE_DEFAULT specifies that the render target should use hardware
///		rendering if it is available; otherwise, it uses software rendering. Note that WIC bitmap render
///		targets do not support hardware rendering.
/// </summary>
Bardez::Projects::DirectX::Direct2D::RenderTargetType Bardez::Projects::DirectX::Direct2D::RenderTargetProperties::Type::get()
{
	return this->type;
}

/// <summary>
///		A value that specifies whether the render target should force hardware or software rendering.
///		A value of D2D1_RENDER_TARGET_TYPE_DEFAULT specifies that the render target should use hardware
///		rendering if it is available; otherwise, it uses software rendering. Note that WIC bitmap render
///		targets do not support hardware rendering.
/// </summary>
void Bardez::Projects::DirectX::Direct2D::RenderTargetProperties::Type::set(Bardez::Projects::DirectX::Direct2D::RenderTargetType value)
{
	this->type = value;
}

/// <summary>
///		The pixel format and alpha mode of the render target. You can use the D2D1::PixelFormat function to
///		create a pixel format that specifies that Direct2D should select the pixel format and alpha mode for you.
/// </summary>
Bardez::Projects::DirectX::Direct2D::PixelFormat^ Bardez::Projects::DirectX::Direct2D::RenderTargetProperties::Pixel_Format::get()
{
	return this->pixelFormat;
}

/// <summary>
///		The pixel format and alpha mode of the render target. You can use the D2D1::PixelFormat function to
///		create a pixel format that specifies that Direct2D should select the pixel format and alpha mode for you.
/// </summary>
void Bardez::Projects::DirectX::Direct2D::RenderTargetProperties::Pixel_Format::set(Bardez::Projects::DirectX::Direct2D::PixelFormat^ value)
{
	this->pixelFormat = value;
}

/// <summary>Represents the DPI resolution. For default values, set both X and Y to 0.</summary>
/// <remarks>The default DPI varies depending on the render target</remarks>
Bardez::Projects::DirectX::Direct2D::DpiResolution Bardez::Projects::DirectX::Direct2D::RenderTargetProperties::Resolution::get()
{
	return this->resolution;
}

/// <summary>Represents the DPI resolution. For default values, set both X and Y to 0.</summary>
/// <remarks>The default DPI varies depending on the render target</remarks>
void Bardez::Projects::DirectX::Direct2D::RenderTargetProperties::Resolution::set(Bardez::Projects::DirectX::Direct2D::DpiResolution value)
{
	this->resolution = value;
}

/// <summary>A value that specifies how the render target is remoted and whether it should be GDI-compatible.</summary>
/// <remarks>Set to RenderTargetUsage.None to create a render target that is not compatible with GDI and uses Direct3D command-stream remoting if it is available.</remarks>
Bardez::Projects::DirectX::Direct2D::RenderTargetUsage Bardez::Projects::DirectX::Direct2D::RenderTargetProperties::Usage::get()
{
	return this->usage;
}

/// <summary>A value that specifies how the render target is remoted and whether it should be GDI-compatible.</summary>
/// <remarks>Set to RenderTargetUsage.None to create a render target that is not compatible with GDI and uses Direct3D command-stream remoting if it is available.</remarks>
void Bardez::Projects::DirectX::Direct2D::RenderTargetProperties::Usage::set(Bardez::Projects::DirectX::Direct2D::RenderTargetUsage value)
{
	this->usage = value;
}

/// <summary>A value that specifies the minimum Direct3D feature level required for hardware rendering.</summary>
/// <remarks>
///		If the specified minimum level is not available, the render target uses software rendering if the type member
///		is set to RenderTargetType.Default; if type is set to to RenderTargetType.Hardware, render target
///		creation fails. A value of DirectXVerions.Default indicates that Direct2D should determine whether the
///		Direct3D feature level of the device is adequate.
///		This field is used only when creating HwndRenderTarget and DCRenderTarget objects.
/// </remarks>
Bardez::Projects::DirectX::Direct2D::DirectXVersion Bardez::Projects::DirectX::Direct2D::RenderTargetProperties::MinVersion::get()
{
	return this->minVersion;
}

/// <summary>A value that specifies the minimum Direct3D feature level required for hardware rendering.</summary>
/// <remarks>
///		If the specified minimum level is not available, the render target uses software rendering if the type member
///		is set to RenderTargetType.Default; if type is set to to RenderTargetType.Hardware, render target
///		creation fails. A value of DirectXVerions.Default indicates that Direct2D should determine whether the
///		Direct3D feature level of the device is adequate.
///		This field is used only when creating HwndRenderTarget and DCRenderTarget objects.
/// </remarks>
void Bardez::Projects::DirectX::Direct2D::RenderTargetProperties::MinVersion::set(Bardez::Projects::DirectX::Direct2D::DirectXVersion value)
{
	this->minVersion = value;
}
#pragma endregion


#pragma region Construction
/// <summary>Default Constructor</summary>
Bardez::Projects::DirectX::Direct2D::RenderTargetProperties::RenderTargetProperties()
{
	this->type = Bardez::Projects::DirectX::Direct2D::RenderTargetType::Default;
	this->Pixel_Format = gcnew Bardez::Projects::DirectX::Direct2D::PixelFormat(Bardez::Projects::DirectX::Direct2D::DXGI_ChannelFormat::FORMAT_UNKNOWN, Bardez::Projects::DirectX::Direct2D::AlphaMode::Unknown);
	this->resolution = Bardez::Projects::DirectX::Direct2D::DpiResolution(0.0F, 0.0F);
	this->usage = Bardez::Projects::DirectX::Direct2D::RenderTargetUsage::None;
	this->minVersion = Bardez::Projects::DirectX::Direct2D::DirectXVersion::Default;
}

/// <summary>Definition Constructor</summary>
/// <param name="targetType">A value that specifies whether the render target should force hardware or software rendering.</param>
/// <param name="format">The pixel format and alpha mode of the render target.</param>
/// <param name="dpiX">The horizontal DPI of the render target.</param>
/// <param name="dpiY">The vertical DPI of the render target.</param>
/// <param name="usage">A value that specifies how the render target is remoted and whether it should be GDI-compatible.</param>
/// <param name="minVersion">A value that specifies the minimum Direct3D feature level required for hardware rendering.</param>
Bardez::Projects::DirectX::Direct2D::RenderTargetProperties::RenderTargetProperties(Bardez::Projects::DirectX::Direct2D::RenderTargetType targetType, Bardez::Projects::DirectX::Direct2D::PixelFormat^ format, System::Single dpiX, System::Single dpiY, Bardez::Projects::DirectX::Direct2D::RenderTargetUsage usage, Bardez::Projects::DirectX::Direct2D::DirectXVersion minVersion)
{
	this->type = targetType;
	this->Pixel_Format = format;
	this->resolution = Bardez::Projects::DirectX::Direct2D::DpiResolution(dpiX, dpiY);
	this->usage = usage;
	this->minVersion = minVersion;
}

/// <summary>Definition Constructor</summary>
/// <param name="targetType">A value that specifies whether the render target should force hardware or software rendering.</param>
/// <param name="format">The pixel format and alpha mode of the render target.</param>
/// <param name="dpiResolution">The DPI resolution of the render target.</param>
/// <param name="usage">A value that specifies how the render target is remoted and whether it should be GDI-compatible.</param>
/// <param name="minVersion">A value that specifies the minimum Direct3D feature level required for hardware rendering.</param>
Bardez::Projects::DirectX::Direct2D::RenderTargetProperties::RenderTargetProperties(Bardez::Projects::DirectX::Direct2D::RenderTargetType targetType, Bardez::Projects::DirectX::Direct2D::PixelFormat^ format, Bardez::Projects::DirectX::Direct2D::DpiResolution dpiResolution, Bardez::Projects::DirectX::Direct2D::RenderTargetUsage usage, Bardez::Projects::DirectX::Direct2D::DirectXVersion minVersion)
{
	this->type = targetType;
	this->Pixel_Format = format;
	this->resolution = dpiResolution;
	this->usage = usage;
	this->minVersion = minVersion;
}

/// <summary>Unmanaged Constructor</summary>
/// <param name="unmanaged">Unmanaged source structure</param>
Bardez::Projects::DirectX::Direct2D::RenderTargetProperties::RenderTargetProperties(D2D1_RENDER_TARGET_PROPERTIES unmanaged)
{
	this->type = (Bardez::Projects::DirectX::Direct2D::RenderTargetType)(unmanaged.type);
	this->Pixel_Format = gcnew Bardez::Projects::DirectX::Direct2D::PixelFormat(unmanaged.pixelFormat);
	this->resolution = Bardez::Projects::DirectX::Direct2D::DpiResolution(unmanaged.dpiX, unmanaged.dpiY);
	this->usage = (Bardez::Projects::DirectX::Direct2D::RenderTargetUsage)(unmanaged.usage);
	this->minVersion = (Bardez::Projects::DirectX::Direct2D::DirectXVersion)(unmanaged.minLevel);
}
#pragma endregion


#pragma region Methods
/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
/// <returns>An unmanaged Direct2D struct equivalent</returns>
D2D1_RENDER_TARGET_PROPERTIES Bardez::Projects::DirectX::Direct2D::RenderTargetProperties::ToUnmanaged()
{
	D2D1_RENDER_TARGET_PROPERTIES properties;
	
	properties.type = (D2D1_RENDER_TARGET_TYPE)(System::UInt32)(this->type);
	properties.pixelFormat = this->Pixel_Format->ToUnmanaged();
	properties.dpiX = this->resolution.X;
	properties.dpiY = this->resolution.Y;
	properties.usage = (D2D1_RENDER_TARGET_USAGE)(System::UInt32)(this->usage);
	properties.minLevel = (D2D1_FEATURE_LEVEL)(System::UInt32)(this->minVersion);

	return properties;
}
#pragma endregion