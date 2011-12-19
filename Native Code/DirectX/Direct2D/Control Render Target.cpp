
#include "Control Render Target.h"

using namespace Bardez::Projects::DirectX::Direct2D;

#pragma region Properties
/// <summary>Represents a pointer to the native HWND render target interface object</summary>
ID2D1HwndRenderTarget* ControlRenderTarget::HwndRenderPtr::get()
{
	return reinterpret_cast<ID2D1HwndRenderTarget*>(this->renderPtr.ToPointer());
}

/// <summary>Represents a pointer to the native HWND render target interface object</summary>
void ControlRenderTarget::HwndRenderPtr::set(ID2D1HwndRenderTarget* value)
{
	this->renderPtr = System::IntPtr(value);
}

/// <summary>Represents a pointer to the native HWND</summary>
HWND ControlRenderTarget::Hwnd::get()
{
	return this->HwndRenderPtr->GetHwnd();
}

/// <summary>Represents a pointer to the native HWND</summary>
System::IntPtr ControlRenderTarget::Handle::get()
{
	return System::IntPtr(this->Hwnd);
}
#pragma endregion


#pragma region Construction
/// <summary>Definition constructor</summary>
ControlRenderTarget::ControlRenderTarget(ID2D1HwndRenderTarget* handle) : RenderTarget(handle) { }
#pragma endregion


#pragma region Destruction
/// <summary>Destrutor</summary>
/// <remarks>Dispose()</remarks>
ControlRenderTarget::~ControlRenderTarget()
{
	this->DisposeUnmanaged();
}

/// <summary>Destrutor</summary>
/// <remarks>Finalize()</remarks>
ControlRenderTarget::!ControlRenderTarget()
{
	this->DisposeUnmanaged();
}

/// <summary>Destrutor logic, disposes the object</summary>
void ControlRenderTarget::DisposeUnmanaged()
{
	// Dispose this Control rendering target
	if (this->HwndRenderPtr)
	{
		this->HwndRenderPtr->Release();
		this->HwndRenderPtr = NULL;
	}
}
#pragma endregion


#pragma region Interface Methods
/// <summary>Indicates whether the control/HWND associated with this render target is occluded</summary>
/// <remarks>
///		If the window was occluded the last time that EndDraw was called, the next time that the render target calls
///		CheckControlState, it will return ControlState.Occluded regardless of the current window state.
/// </remarks>
ControlState ControlRenderTarget::CheckControlState()
{
	return (ControlState)(this->HwndRenderPtr->CheckWindowState());
}

/// <summary>Changes the size of the render target to the specified pixel size.</summary>
/// <param name="size">The new size of the render target in device pixels.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode ControlRenderTarget::Resize(SizeU^ size)
{
	return (ResultCode)(this->HwndRenderPtr->Resize(&(size->ToUnmanaged())));
}
#pragma endregion
