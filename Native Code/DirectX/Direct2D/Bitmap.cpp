

#include "Bitmap.h"
#include "Render Target.h"


using namespace Bardez::Projects::BasicStructures::Win32;


#pragma region Properties
/// <summary>Accesses a pointer to the native bitmap interface object</summary>
ID2D1Bitmap* Bardez::Projects::DirectX::Direct2D::Bitmap::BitmapPtr::get()
{
	return reinterpret_cast<ID2D1Bitmap*>(this->bitmapPtr.ToPointer());
}

/// <summary>Accesses a pointer to the native bitmap interface object</summary>
void Bardez::Projects::DirectX::Direct2D::Bitmap::BitmapPtr::set(ID2D1Bitmap* value)
{
	this->bitmapPtr = System::IntPtr(value);
}
#pragma endregion


#pragma region Construction
/// <summary>Definition constructor</summary>
Bardez::Projects::DirectX::Direct2D::Bitmap::Bitmap(ID2D1Bitmap* handle)
{
	this->BitmapPtr = handle;
}
#pragma endregion


#pragma region Destruction
/// <summary>Destructor</summary>
/// <remarks>Dispose()</remarks>
Bardez::Projects::DirectX::Direct2D::Bitmap::~Bitmap()
{
	this->DisposeUnmanaged();
}

/// <summary>Destructor</summary>
/// <remarks>Finalize()</remarks>
Bardez::Projects::DirectX::Direct2D::Bitmap::!Bitmap()
{
	this->DisposeUnmanaged();
}

/// <summary>Destructor logic, disposes the object</summary>
void Bardez::Projects::DirectX::Direct2D::Bitmap::DisposeUnmanaged()
{
	// Dispose this rendering target
	if (this->BitmapPtr)
	{
		this->BitmapPtr->Release();
		this->BitmapPtr = NULL;
	}
}
#pragma endregion


#pragma region Methods
/// <summary>Copies the specified region from the specified bitmap into the current bitmap.</summary>
/// <param name="upperPoint">In the current bitmap, the upper-left corner of the area to which the data is copied. If null, [0,0] will be used</param>
/// <param name="source">Source bitmap to read from. Cannot be null.</param>
/// <param name="area">Area rectangle of the Bitmap to read from. If null, the full source/destination area will be copied/written to.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
/// <remarks>
///		This method does not update the size of the current bitmap. If the contents of the source bitmap do not fit in the current bitmap,
///		this method fails. Also, note that this method does not perform format conversion, and will fail if the bitmap formats do not match.
///</remarks>
ResultCode Bardez::Projects::DirectX::Direct2D::Bitmap::CopyFromBitmap(Bardez::Projects::DirectX::Direct2D::Point2dU^ upperPoint, Bardez::Projects::DirectX::Direct2D::Bitmap^ source, Bardez::Projects::DirectX::Direct2D::RectangleU^ area)
{
	if (source == nullptr)
		throw gcnew System::ArgumentNullException("Source Bitmap cannot be null.");

	D2D_POINT_2U* ptrPoint = (upperPoint == nullptr ? NULL : &(upperPoint->ToUnmanaged()));
	D2D_RECT_U* ptrRect = (area == nullptr ? NULL : &(area->ToUnmanaged()));

	return (ResultCode) this->BitmapPtr->CopyFromBitmap(ptrPoint, source->BitmapPtr, ptrRect);
}

/// <summary>Copies the specified region from the specified bitmap into the current bitmap.</summary>
/// <param name="upperPoint">In the current bitmap, the upper-left corner of the area to which the data is copied. If null, [0,0] will be used</param>
/// <param name="source">Source bitmap to read from. Cannot be null.</param>
/// <param name="area">Area rectangle of source to read from. If null, the full source/destination area will be copied/written to.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
/// <remarks>
///		This method does not update the size of the current bitmap. If the contents of the source bitmap do not fit in the current bitmap,
///		this method fails. Also, note that this method does not perform format conversion, and will fail if the bitmap formats do not match.
///</remarks>
ResultCode Bardez::Projects::DirectX::Direct2D::Bitmap::CopyFromBitmap(System::Drawing::Point upperPoint, Bitmap^ source, System::Drawing::Rectangle area)
{
	return this->CopyFromBitmap(gcnew Bardez::Projects::DirectX::Direct2D::Point2dU(upperPoint), source, gcnew Bardez::Projects::DirectX::Direct2D::RectangleU(area));
}

/// <summary>Copies the specified region from the specified rendering target into the current bitmap.</summary>
/// <param name="upperPoint">In the current bitmap, the upper-left corner of the area to which the data is copied. If null, [0,0] will be used</param>
/// <param name="source">Source rendering target to read from. Cannot be null.</param>
/// <param name="area">Area rectangle of source to read from. If null, the full source/destination area will be copied/written to.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
/// <remarks>
///		This method does not update the size of the current bitmap. If the contents of the source bitmap do not fit in the current bitmap,
///		this method fails. Also, note that this method does not perform format conversion, and will fail if the bitmap formats do not match.
///</remarks>
ResultCode Bardez::Projects::DirectX::Direct2D::Bitmap::CopyFromRenderTarget(Bardez::Projects::DirectX::Direct2D::Point2dU^ upperPoint, Bardez::Projects::DirectX::Direct2D::RenderTarget^ source, Bardez::Projects::DirectX::Direct2D::RectangleU^ area)
{
	if (source == nullptr)
		throw gcnew System::ArgumentNullException("Source RenderTarget cannot be null.");

	D2D_POINT_2U* ptrPoint = (upperPoint == nullptr ? NULL : &(upperPoint->ToUnmanaged()));
	D2D_RECT_U* ptrRect = (area == nullptr ? NULL : &(area->ToUnmanaged()));

	return (ResultCode) this->BitmapPtr->CopyFromRenderTarget(ptrPoint, source->RenderPtr, ptrRect);
}

/// <summary>Copies the specified region from the specified rendering target into the current bitmap.</summary>
/// <param name="upperPoint">In the current bitmap, the upper-left corner of the area to which the data is copied. If null, [0,0] will be used</param>
/// <param name="source">Source rendering target to read from. Cannot be null.</param>
/// <param name="area">Area rectangle of source to read from. If null, the full source/destination area will be copied/written to.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
/// <remarks>
///		This method does not update the size of the current bitmap. If the contents of the source bitmap do not fit in the current bitmap,
///		this method fails. Also, note that this method does not perform format conversion, and will fail if the bitmap formats do not match.
///</remarks>
ResultCode Bardez::Projects::DirectX::Direct2D::Bitmap::CopyFromRenderTarget(System::Drawing::Point upperPoint, Bardez::Projects::DirectX::Direct2D::RenderTarget^ source, System::Drawing::Rectangle area)
{
	return this->CopyFromRenderTarget(gcnew Bardez::Projects::DirectX::Direct2D::Point2dU(upperPoint), source, gcnew Bardez::Projects::DirectX::Direct2D::RectangleU(area));
}

/// <summary>Copies the specified region from memory into the current bitmap.</summary>
/// <param name="area">Area rectangle of the Bitmap to read from. If null, the full source/destination area will be copied/written to.</param>
/// <param name="source">Source data to read from. Cannot be null.</param>
/// <param name="pitch">
///		The stride, or pitch, of the source bitmap stored in srcData. The stride is the byte count of
///		a scanline (one row of pixels in memory). The stride can be computed from the following formula:
///		pixel width * bytes per pixel + memory padding.
/// </param>
/// <returns>S_OK on success, otherwise an error code.</returns>
/// <remarks>
///		This method does not update the size of the current bitmap. If the contents of the source bitmap do not fit in the current bitmap,
///		this method fails. Also, note that this method does not perform format conversion, and will fail if the bitmap formats do not match.
///</remarks>
ResultCode Bardez::Projects::DirectX::Direct2D::Bitmap::CopyFromMemory(Bardez::Projects::DirectX::Direct2D::RectangleU^ area, array<System::Byte>^ source, System::UInt32 pitch)
{
	if (source == nullptr)
		throw gcnew System::ArgumentNullException("Source data cannot be null.");

	D2D_RECT_U* ptrRect = (area == nullptr ? NULL : &(area->ToUnmanaged()));
	pin_ptr<BYTE> dataPtr = &source[0];
	BYTE* data = dataPtr;

	return (ResultCode) this->BitmapPtr->CopyFromMemory(ptrRect, dataPtr, pitch);
}

/// <summary>Copies the specified region from memory into the current bitmap.</summary>
/// <param name="area">Area rectangle of the Bitmap to read from. If null, the full source/destination area will be copied/written to.</param>
/// <param name="source">Pointer to source data to read from. Cannot be Zero.</param>
/// <param name="length">Length of the data pointed to.</param>
/// <param name="pitch">
///		The stride, or pitch, of the source bitmap stored in srcData. The stride is the byte count of
///		a scanline (one row of pixels in memory). The stride can be computed from the following formula:
///		pixel width * bytes per pixel + memory padding.
/// </param>
/// <returns>S_OK on success, otherwise an error code.</returns>
/// <remarks>
///		This method does not update the size of the current bitmap. If the contents of the source bitmap do not fit in the current bitmap,
///		this method fails. Also, note that this method does not perform format conversion, and will fail if the bitmap formats do not match.
///</remarks>
ResultCode Bardez::Projects::DirectX::Direct2D::Bitmap::CopyFromMemory(RectangleU^ area, System::IntPtr source, System::Int32 length, System::UInt32 pitch)
{
	if (source == System::IntPtr::Zero)
		throw gcnew System::ArgumentNullException("Source data cannot be IntPtr.Zero.");

	D2D_RECT_U* ptrRect = (area == nullptr ? NULL : &(area->ToUnmanaged()));
	BYTE* data = reinterpret_cast<BYTE*>(source.ToPointer());

	return (ResultCode) this->BitmapPtr->CopyFromMemory(ptrRect, data, pitch);
}

/// <summary>Copies the specified region from memory into the current bitmap.</summary>
/// <param name="area">Area rectangle of the Bitmap to read from. If null, the full source/destination area will be copied/written to.</param>
/// <param name="source">Source data to read from. Cannot be null.</param>
/// <param name="pitch">
///		The stride, or pitch, of the source bitmap stored in srcData. The stride is the byte count of
///		a scanline (one row of pixels in memory). The stride can be computed from the following formula:
///		pixel width * bytes per pixel + memory padding.
/// </param>
/// <returns>S_OK on success, otherwise an error code.</returns>
/// <remarks>
///		This method does not update the size of the current bitmap. If the contents of the source bitmap do not fit in the current bitmap,
///		this method fails. Also, note that this method does not perform format conversion, and will fail if the bitmap formats do not match.
///</remarks>
ResultCode Bardez::Projects::DirectX::Direct2D::Bitmap::CopyFromMemory(System::Drawing::Rectangle area, array<System::Byte>^ source, System::UInt32 pitch)
{
	return this->CopyFromMemory(gcnew Bardez::Projects::DirectX::Direct2D::RectangleU(area), source, pitch);
}

/// <summary>Copies the specified region from memory into the current bitmap.</summary>
/// <param name="area">Area rectangle of the Bitmap to read from. If null, the full source/destination area will be copied/written to.</param>
/// <param name="source">Pointer to source data to read from. Cannot be Zero.</param>
/// <param name="length">Length of the data pointed to.</param>
/// <param name="pitch">
///		The stride, or pitch, of the source bitmap stored in srcData. The stride is the byte count of
///		a scanline (one row of pixels in memory). The stride can be computed from the following formula:
///		pixel width * bytes per pixel + memory padding.
/// </param>
/// <returns>S_OK on success, otherwise an error code.</returns>
/// <remarks>
///		This method does not update the size of the current bitmap. If the contents of the source bitmap do not fit in the current bitmap,
///		this method fails. Also, note that this method does not perform format conversion, and will fail if the bitmap formats do not match.
///</remarks>
ResultCode Bardez::Projects::DirectX::Direct2D::Bitmap::CopyFromMemory(System::Drawing::Rectangle area, System::IntPtr source, System::Int32 length, System::UInt32 pitch)
{
	return this->CopyFromMemory(gcnew Bardez::Projects::DirectX::Direct2D::RectangleU(area), source, length, pitch);
}

/// <summary>Gets the dots per inch of the bitmap</summary>
/// <returns>The DPI resolution of the bitmap.</returns>
Bardez::Projects::DirectX::Direct2D::DpiResolution Bardez::Projects::DirectX::Direct2D::Bitmap::GetDpi()
{
	FLOAT x_dpi, y_dpi;
	this->BitmapPtr->GetDpi(&x_dpi, &y_dpi);
	return Bardez::Projects::DirectX::Direct2D::DpiResolution(x_dpi, y_dpi);
}

/// <summary>Gets the pixel data format of this bitmap</summary>
/// <returns>The pixel data format of this bitmap.</returns>
Bardez::Projects::DirectX::Direct2D::PixelFormat^ Bardez::Projects::DirectX::Direct2D::Bitmap::GetPixelFormat()
{
	return gcnew Bardez::Projects::DirectX::Direct2D::PixelFormat(this->BitmapPtr->GetPixelFormat());
}

/// <summary>Gets the integer size of this bitmap</summary>
/// <returns>The integer size of this bitmap.</returns>
Bardez::Projects::DirectX::Direct2D::SizeU^ Bardez::Projects::DirectX::Direct2D::Bitmap::GetPixelSize()
{
	return gcnew Bardez::Projects::DirectX::Direct2D::SizeU(this->BitmapPtr->GetPixelSize());
}

/// <summary>Gets the integer size of this bitmap</summary>
/// <returns>The integer size of this bitmap.</returns>
System::Drawing::Size Bardez::Projects::DirectX::Direct2D::Bitmap::GetPixelFrameworkSize()
{
	System::Drawing::Size size;

	D2D_SIZE_U nativeSize = this->BitmapPtr->GetPixelSize();
	size.Height = nativeSize.height;
	size.Width = nativeSize.width;

	return size;
}

/// <summary>Gets the single-precision floating point size, in device-independent pixels (DIPs), of this bitmap</summary>
/// <returns>The single-precision floating point size of this bitmap.</returns>
Bardez::Projects::DirectX::Direct2D::SizeF^ Bardez::Projects::DirectX::Direct2D::Bitmap::GetSize()
{
	return gcnew Bardez::Projects::DirectX::Direct2D::SizeF(this->BitmapPtr->GetSize());
}

/// <summary>Gets the single-precision floating point size, in device-independent pixels (DIPs), of this bitmap</summary>
/// <returns>The single-precision floating point size of this bitmap.</returns>
System::Drawing::SizeF Bardez::Projects::DirectX::Direct2D::Bitmap::GetFrameworkSize()
{
	System::Drawing::SizeF size;

	D2D_SIZE_F nativeSize = this->BitmapPtr->GetSize();
	size.Height = nativeSize.height;
	size.Width = nativeSize.width;

	return size;
}
#pragma endregion

