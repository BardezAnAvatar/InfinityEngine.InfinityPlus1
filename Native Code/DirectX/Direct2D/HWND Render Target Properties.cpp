
#include "HWND Render Target Properties.h"

using namespace Bardez::Projects::DirectX::Direct2D;

					
#pragma region Properties
/// <summary>The WiNDow Handle to which the render target issues the output from its drawing commands.</summary>
System::IntPtr HwndRenderTargetProperties::WindowHandle::get()
{
	return this->windowHandle;
}

/// <summary>The WiNDow Handle to which the render target issues the output from its drawing commands.</summary>
void HwndRenderTargetProperties::WindowHandle::set(System::IntPtr value)
{
	this->windowHandle = value;
}

/// <summary>The size of the render target, in pixels.</summary>
/// <values>The default value is a SizeU that has a width and height of 0</values>
SizeU^ HwndRenderTargetProperties::PixelSize::get()
{
	return this->pixelSize;
}

/// <summary>The size of the render target, in pixels.</summary>
/// <values>The default value is a SizeU that has a width and height of 0</values>
void HwndRenderTargetProperties::PixelSize::set(SizeU^ value)
{
	this->pixelSize = value;
}

/// <summary>A value that specifies whether the render target retains the frame after it is presented and whether the render target waits for the device to refresh before presenting.</summary>
PresentationOptions HwndRenderTargetProperties::Options::get()
{
	return this->options;
}

/// <summary>A value that specifies whether the render target retains the frame after it is presented and whether the render target waits for the device to refresh before presenting.</summary>
void HwndRenderTargetProperties::Options::set(PresentationOptions value)
{
	this->options = value;
}
#pragma endregion


#pragma region Construction
/// <summary>Default Constructor</summary>
HwndRenderTargetProperties::HwndRenderTargetProperties()
{
	this->windowHandle = System::IntPtr(NULL);
}

/// <summary>Definition Constructor</summary>
/// <param name="windowHandle">Handle for the window being rendered to.</param>
HwndRenderTargetProperties::HwndRenderTargetProperties(System::IntPtr windowHandle)
{
	this->windowHandle = windowHandle;
	this->pixelSize = gcnew SizeU(0U, 0U);		//Microsoft documentation suggests this default value
	this->options = PresentationOptions::None;	//Microsoft documentation suggests this default value
}

/// <summary>Definition Constructor</summary>
/// <param name="windowHandle">Handle for the window being rendered to.</param>
/// <param name="size">Dimensions of the render target, in device dependent pixels.</param>
/// <param name="presentOptions">Specifies whether the render target retains the frame after it is presented and whether the render target waits for the device to refresh before presenting.</param>
HwndRenderTargetProperties::HwndRenderTargetProperties(System::IntPtr windowHandle, SizeU^ size, PresentationOptions presentOptions)
{
	this->windowHandle = windowHandle;
	this->pixelSize = size;
	this->options = presentOptions;
}

/// <summary>Definition Constructor</summary>
/// <param name="window">Handle for the window being rendered to.</param>
/// <param name="size">Dimensions of the render target, in device dependent pixels.</param>
/// <param name="presentOptions">Specifies whether the render target retains the frame after it is presented and whether the render target waits for the device to refresh before presenting.</param>
HwndRenderTargetProperties::HwndRenderTargetProperties(HWND window, SizeU^ size, PresentationOptions presentOptions)
{
	this->windowHandle = System::IntPtr(window);
	this->pixelSize = size;
	this->options = presentOptions;
}

/// <summary>Definition Constructor</summary>
/// <param name="window">Handle for the window being rendered to.</param>
HwndRenderTargetProperties::HwndRenderTargetProperties(HWND window)
{
	this->windowHandle = System::IntPtr(window);
	this->pixelSize = gcnew SizeU(0U, 0U);		//Microsoft documentation suggests this default value
	this->options = PresentationOptions::None;	//Microsoft documentation suggests this default value
}

/// <summary>Unmanaged Constructor</summary>
/// <param name="unmanaged">Unmanaged source structure</param>
HwndRenderTargetProperties::HwndRenderTargetProperties(D2D1_HWND_RENDER_TARGET_PROPERTIES unmanaged)
{
	this->windowHandle = System::IntPtr(unmanaged.hwnd);
	this->pixelSize = gcnew SizeU(unmanaged.pixelSize);
	this->options = (PresentationOptions)(unmanaged.presentOptions);
}
#pragma endregion


#pragma region Methods
/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
/// <returns>An unmanaged Direct2D struct equivalent</returns>
D2D1_HWND_RENDER_TARGET_PROPERTIES HwndRenderTargetProperties::ToUnmanaged()
{
	D2D1_HWND_RENDER_TARGET_PROPERTIES properties;
	
	properties.hwnd = (HWND)(this->windowHandle.ToPointer());
	properties.pixelSize = this->pixelSize->ToUnmanaged();
	properties.presentOptions = (D2D1_PRESENT_OPTIONS)(this->options);

	return properties;
}
#pragma endregion