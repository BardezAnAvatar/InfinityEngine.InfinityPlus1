
#include "Bitmap Render Target.h"

using namespace Bardez::Projects::DirectX::Direct2D;

#pragma region Properties
/// <summary>Represents a pointer to the native bitmap render target interface object</summary>
ID2D1BitmapRenderTarget* BitmapRenderTarget::BitmapRenderPtr::get()
{
	return reinterpret_cast<ID2D1BitmapRenderTarget*>(this->renderPtr.ToPointer());
}

/// <summary>Represents a pointer to the native bitmap render target interface object</summary>
void BitmapRenderTarget::BitmapRenderPtr::set(ID2D1BitmapRenderTarget* value)
{
	this->renderPtr = System::IntPtr(value);
}
#pragma endregion


#pragma region Construction
/// <summary>Definition constructor</summary>
/// <remarks>
///		It was not enough to recreate the assignment logic. I had to call the base constructor from the derived one.
///		Self-FYI.
/// </remarks>
BitmapRenderTarget::BitmapRenderTarget(ID2D1BitmapRenderTarget* handle) : RenderTarget(handle) { }
#pragma endregion


#pragma region Destruction
/// <summary>Destructor</summary>
/// <remarks>Dispose()</remarks>
BitmapRenderTarget::~BitmapRenderTarget()
{
	this->DisposeUnmanaged();
}

/// <summary>Destructor</summary>
/// <remarks>Finalize()</remarks>
BitmapRenderTarget::!BitmapRenderTarget()
{
	this->DisposeUnmanaged();
}

/// <summary>Destructor logic, disposes the object</summary>
void BitmapRenderTarget::DisposeUnmanaged()
{
	// Dispose this Bitmap rendering target
	if (this->BitmapRenderPtr)
	{
		this->BitmapRenderPtr->Release();
		this->BitmapRenderPtr = NULL;
	}
}
#pragma endregion


#pragma region Interface Methods
/// <summary>Creates an uninitialized Direct2D bitmap.</summary>
/// <param name="bitmap">When this method returns, contains a pointer to a pointer to the new bitmap. This parameter is passed uninitialized.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode BitmapRenderTarget::GetBitmap([System::Runtime::InteropServices::Out] Bitmap^ %bitmap)
{
	ID2D1Bitmap * bitmapHandle;
	ResultCode result = (ResultCode)(this->BitmapRenderPtr->GetBitmap(&bitmapHandle));
	bitmap = gcnew Bitmap(bitmapHandle);

	return result;
}
#pragma endregion
