
#include "Render Target.h"
#include "Bitmap.h" //for the constructor
#include "Bitmap Render Target.h" //for the constructor

using namespace Bardez::Projects::DirectX::Direct2D;

#pragma region Properties
/// <summary>Represents a pointer to the native render target interface object</summary>
ID2D1RenderTarget* RenderTarget::RenderPtr::get()
{
	return reinterpret_cast<ID2D1RenderTarget*>(this->renderPtr.ToPointer());
}

/// <summary>Represents a pointer to the native render target interface object</summary>
void RenderTarget::RenderPtr::set(ID2D1RenderTarget* value)
{
	this->renderPtr = System::IntPtr(value);
}

/// <summary>Retrieves the current antialiasing mode for nontext drawing operations.</summary>
/// <returns>The current AntialiasMode.</returns>
AntialiasMode RenderTarget::AntiAliasMode::get()
{
	return (AntialiasMode)(this->RenderPtr->GetAntialiasMode());
}

/// <summary>Sets the antialiasing mode of the render target. The antialiasing mode applies to all subsequent drawing operations, excluding text and glyph drawing operations.</summary>
/// <param name="value">The antialiasing mode for future drawing operations.</param>
void RenderTarget::AntiAliasMode::set(AntialiasMode value)
{
	this->RenderPtr->SetAntialiasMode((D2D1_ANTIALIAS_MODE)value);
}

/// <summary>Retrieves the current dots per inch (DPI) of the render target.</summary>
/// <returns>The current render target DPI.</returns>
/// <remarks>
///		This method indicates the mapping from pixel space to device-independent space for the render target.
///		For ID2D1HwndRenderTarget, the DPI defaults to the most recently factory-read system DPI.
///		The default value for all other render targets is 96 DPI.
/// </remarks>
DpiResolution RenderTarget::DPI::get()
{
	FLOAT dpiX, dpiY;
	this->RenderPtr->GetDpi(&dpiX, &dpiY);
	return DpiResolution(dpiX, dpiY);
}

/// <summary>Sets the dots per inch (DPI) of the render target.</summary>
/// <param name="value">The new render target DPI.</param>
/// <remarks>
///		This method specifies the mapping from pixel space to device-independent space for the render target.
///		If both dpiX and dpiY are 0, the factory-read system DPI is chosen.
///		If one parameter is zero and the other unspecified, the DPI is not changed.
/// </remarks>
void RenderTarget::DPI::set(DpiResolution value)
{
	this->RenderPtr->SetDpi(value.X, value.Y);
}

/// <summary>Gets the maximum size, in device-dependent units (pixels), of any one bitmap dimension supported by the render target.</summary>
/// <returns>The maximum size, in pixels, of any one bitmap dimension supported by the render target.</returns>
System::UInt32 RenderTarget::MaximumBitmapSize::get()
{
	return this->RenderPtr->GetMaximumBitmapSize();
}

/// <summary>Retrieves the pixel format and alpha mode of the render target.</summary>
/// <returns>The pixel format and alpha mode of the render target.</returns>
PixelFormat^ RenderTarget::PixelFormat::get()
{
	return gcnew Bardez::Projects::DirectX::Direct2D::PixelFormat(this->RenderPtr->GetPixelFormat());
}
					
/// <summary>Returns the size of the render target in device pixels.</summary>
/// <returns>The size of the render target in device pixels.</returns>
SizeU^ RenderTarget::PixelSize::get()
{
	return gcnew SizeU(this->RenderPtr->GetPixelSize());
}
					
/// <summary>Returns the size of the render target in device-independent pixels.</summary>
/// <returns>The current size of the render target in device-independent pixels.</returns>
SizeF^ RenderTarget::Size::get()
{
	return gcnew SizeF(this->RenderPtr->GetSize());
}
					
/// <summary>Exposes transform of the render target.</summary>
Matrix3x2F^ RenderTarget::Transform::get()
{
	D2D1_MATRIX_3X2_F* transform = NULL;
	this->RenderPtr->GetTransform(transform);

	Matrix3x2F^ result = gcnew Matrix3x2F((*transform));
	delete transform;

	return result;
}
					
/// <summary>Exposes transform of the render target.</summary>
void RenderTarget::Transform::set(Matrix3x2F^ value)
{
	this->RenderPtr->SetTransform(value->ToUnmanaged());
}
#pragma endregion


#pragma region Construction
/// <summary>Definition constructor</summary>
RenderTarget::RenderTarget(ID2D1RenderTarget* handle)
{
	this->RenderPtr = handle;
}
#pragma endregion


#pragma region Destruction
/// <summary>Destructor</summary>
/// <remarks>Dispose()</remarks>
RenderTarget::~RenderTarget()
{
	this->DisposeUnmanaged();
}

/// <summary>Destructor</summary>
/// <remarks>Finalize()</remarks>
RenderTarget::!RenderTarget()
{
	this->DisposeUnmanaged();
}

/// <summary>Destructor logic, disposes the object</summary>
void RenderTarget::DisposeUnmanaged()
{
	// Dispose this rendering target
	if (this->RenderPtr)
	{
		this->RenderPtr->Release();
		this->RenderPtr = NULL;
	}
}
#pragma endregion


#pragma region Interface Methods
/// <summary>Begins drawing on the specified surface</summary>
void RenderTarget::BeginDraw()
{
	this->RenderPtr->BeginDraw();
}

/// <summary>Clears the drawing area to the specified color</summary>
/// <param name="clearColor">Color with which to fill the area with</param>
void RenderTarget::Clear(System::Drawing::Color clearColor)
{
	this->Clear(gcnew ColorF(clearColor));
}

/// <summary>Clears the drawing area with transparent black color</summary>
void RenderTarget::Clear()
{
	this->RenderPtr->Clear();
}

/// <summary>Clears the drawing area to the specified color</summary>
/// <param name="clearColor">Color with which to fill the area with</param>
void RenderTarget::Clear(ColorF^ clearColor)
{
	this->RenderPtr->Clear(&(clearColor->ToUnmanaged()));
}

/// <summary>Creates an uninitialized Direct2D bitmap. </summary>
/// <param name="size">The dimension of the bitmap to create in pixels.</param>
/// <param name="properties">The pixel format and dots per inch (DPI) of the bitmap to create.</param>
/// <param name="bitmap">When this method returns, contains a pointer to a pointer to the new bitmap. This parameter is passed uninitialized.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode RenderTarget::CreateBitmap(System::Drawing::Size size, BitmapProperties^ properties, [System::Runtime::InteropServices::Out] Bitmap^ %bitmap)
{
	ID2D1Bitmap* bitmapPtr;
	ResultCode result = (ResultCode)(this->RenderPtr->CreateBitmap((gcnew SizeU(size))->ToUnmanaged(), properties->ToUnmanaged(), &bitmapPtr));
	bitmap = gcnew Bitmap(bitmapPtr);

	return result;
}

/// <summary>Creates an uninitialized Direct2D bitmap. </summary>
/// <param name="size">The dimension of the bitmap to create in pixels.</param>
/// <param name="properties">The pixel format and dots per inch (DPI) of the bitmap to create.</param>
/// <param name="bitmap">When this method returns, contains a pointer to a pointer to the new bitmap. This parameter is passed uninitialized.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode RenderTarget::CreateBitmap(SizeU^ size, BitmapProperties^ properties, [System::Runtime::InteropServices::Out] Bitmap^ %bitmap)
{
	ID2D1Bitmap* bitmapPtr;
	ResultCode result = (ResultCode)(this->RenderPtr->CreateBitmap(size->ToUnmanaged(), properties->ToUnmanaged(), &bitmapPtr));
	bitmap = gcnew Bitmap(bitmapPtr);

	return result;
}

/// <summary>Creates an uninitialized Direct2D bitmap. </summary>
/// <param name="size">The dimension of the bitmap to create in pixels.</param>
/// <param name="data">Source data to read from. Cannot be null.</param>
/// <param name="pitch">
///		The stride, or pitch, of the source bitmap stored in srcData. The stride is the byte count of
///		a scanline (one row of pixels in memory). The stride can be computed from the following formula:
///		pixel width * bytes per pixel + memory padding.
/// </param>
/// <param name="properties">The pixel format and dots per inch (DPI) of the bitmap to create.</param>
/// <param name="bitmap">When this method returns, contains a pointer to a pointer to the new bitmap. This parameter is passed uninitialized.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode RenderTarget::CreateBitmap(System::Drawing::Size size, array<System::Byte>^ data, System::UInt32 pitch, BitmapProperties^ properties, [System::Runtime::InteropServices::Out] Bitmap^ %bitmap)
{
	ID2D1Bitmap* bitmapPtr;
	pin_ptr<BYTE> binData = &data[0];
	BYTE* srcData = binData;
	ResultCode result = (ResultCode)(this->RenderPtr->CreateBitmap((gcnew SizeU(size))->ToUnmanaged(), srcData, pitch, &(properties->ToUnmanaged()), &bitmapPtr));
	bitmap = gcnew Bitmap(bitmapPtr);

	return result;
}

/// <summary>Creates an uninitialized Direct2D bitmap. </summary>
/// <param name="size">The dimension of the bitmap to create in pixels.</param>
/// <param name="data">Source data to read from. Cannot be null.</param>
/// <param name="pitch">
///		The stride, or pitch, of the source bitmap stored in srcData. The stride is the byte count of
///		a scanline (one row of pixels in memory). The stride can be computed from the following formula:
///		pixel width * bytes per pixel + memory padding.
/// </param>
/// <param name="properties">The pixel format and dots per inch (DPI) of the bitmap to create.</param>
/// <param name="bitmap">When this method returns, contains a pointer to a pointer to the new bitmap. This parameter is passed uninitialized.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode RenderTarget::CreateBitmap(SizeU^ size, array<System::Byte>^ data, System::UInt32 pitch, BitmapProperties^ properties, [System::Runtime::InteropServices::Out] Bitmap^ %bitmap)
{
	ID2D1Bitmap* bitmapPtr;
	pin_ptr<BYTE> binData = &data[0];
	BYTE* srcData = binData;
	ResultCode result = (ResultCode)(this->RenderPtr->CreateBitmap(size->ToUnmanaged(), srcData, pitch, &(properties->ToUnmanaged()), &bitmapPtr));
	bitmap = gcnew Bitmap(bitmapPtr);

	return result;
}

/// <summary>Creates an BitmapBrush from the specified bitmap</summary>
/// <param name="bitmap">The bitmap contents of the new brush.</param>
/// <param name="bitmapBrushProperties">The extend modes and interpolation mode of the new brush, or null.</param>
/// <param name="brushProperties">A structure that contains the opacity and transform of the new brush, or null.</param>
/// <param name="bitmapBrush">Output parameter containing a reference to the new brush</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode RenderTarget::CreateBitmapBrush(Bitmap^ bitmap, BitmapBrushProperties^ bitmapBrushProperties, BrushProperties^ brushProperties, [System::Runtime::InteropServices::Out] BitmapBrush^ %bitmapBrush)
{
	ID2D1BitmapBrush* bitmapBrushPtr;

	//bitmap brush properties pointer
	D2D1_BITMAP_BRUSH_PROPERTIES* bmpBrshProp;
	if (bitmapBrushProperties == nullptr)
		bmpBrshProp = NULL;
	else
		bmpBrshProp = &(bitmapBrushProperties->ToUnmanaged());

	//brush properties pointer
	D2D1_BRUSH_PROPERTIES* brshProp;
	if (brushProperties == nullptr)
		brshProp = NULL;
	else
		brshProp = &(brushProperties->ToUnmanaged());

	ResultCode result = (ResultCode)(this->RenderPtr->CreateBitmapBrush(bitmap->BitmapPtr, bmpBrshProp, brshProp, &bitmapBrushPtr));

	bitmapBrush = gcnew BitmapBrush(bitmapBrushPtr);

	return result;
}

/// <summary>Creates an BitmapBrush from the specified bitmap</summary>
/// <param name="bitmap">The bitmap contents of the new brush.</param>
/// <param name="bitmapBrushProperties">The extend modes and interpolation mode of the new brush, or null.</param>
/// <param name="bitmapBrush">Output parameter containing a reference to the new brush</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode RenderTarget::CreateBitmapBrush(Bitmap^ bitmap, BitmapBrushProperties^ bitmapBrushProperties, [System::Runtime::InteropServices::Out] BitmapBrush^ %bitmapBrush)
{
	ID2D1BitmapBrush* bitmapBrushPtr;
	ResultCode result = (ResultCode)(this->RenderPtr->CreateBitmapBrush(bitmap->BitmapPtr, bitmapBrushProperties->ToUnmanaged(), &bitmapBrushPtr));
	bitmapBrush = gcnew BitmapBrush(bitmapBrushPtr);

	return result;
}

/// <summary>Creates an BitmapBrush from the specified bitmap</summary>
/// <param name="bitmap">The bitmap contents of the new brush.</param>
/// <param name="bitmapBrush">Output parameter containing a reference to the new brush</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode RenderTarget::CreateBitmapBrush(Bitmap^ bitmap, [System::Runtime::InteropServices::Out] BitmapBrush^ %bitmapBrush)
{
	ID2D1BitmapBrush* bitmapBrushPtr;
	ResultCode result = (ResultCode)(this->RenderPtr->CreateBitmapBrush(bitmap->BitmapPtr, &bitmapBrushPtr));
	bitmapBrush = gcnew BitmapBrush(bitmapBrushPtr);

	return result;
}

/// <summary>
///		Creates a new bitmap render target for use during intermediate offscreen drawing that is compatible
///		with the current render target and has the same size, DPI, and pixel format (but not alpha mode)
///		as the current render target.
/// </summary>
/// <param name="renderBuffer">Output reference to new RenderTarget.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
/// <remarks>The bitmap render target created by this method is not compatible with GDI and has an alpha mode of AlphaMode.Premultiplied.</remarks>
ResultCode RenderTarget::CreateCompatibleRenderTarget([System::Runtime::InteropServices::Out] BitmapRenderTarget^ %renderBuffer)
{
	ID2D1BitmapRenderTarget* bitmapRenderTarget;
	ResultCode result = (ResultCode)(this->RenderPtr->CreateCompatibleRenderTarget(&bitmapRenderTarget));
	renderBuffer = gcnew BitmapRenderTarget(bitmapRenderTarget);

	return result;
}

/// <summary>
///		Creates a new bitmap render target for use during intermediate offscreen drawing that is compatible
///		with the current render target and has the same size, DPI, and pixel format (but not alpha mode)
///		as the current render target.
/// </summary>
/// <param name="desiredSize">The desired size of the new render target in device-independent pixels if it should be different from the original render target, or null.</param>
/// <param name="renderBuffer">When this method returns, contains the address of a pointer to a new bitmap render target. This parameter is passed uninitialized.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
/// <remarks>
///		If desiredSize maps to a fractional-pixel size, the pixel size is rounded up to the nearest integer
///		and the DPI for the compatible render target is slightly higher than the DPI of the parent render target.
///		The coordinate (desiredSize.width, desiredSize.height) maps to the lower-right corner of the compatible
///		render target.
/// </remarks>
ResultCode RenderTarget::CreateCompatibleRenderTarget(SizeF^ desiredSize, [System::Runtime::InteropServices::Out] BitmapRenderTarget^ %renderBuffer)
{
	ID2D1BitmapRenderTarget* bitmapRenderTarget;
	ResultCode result = (ResultCode)(this->RenderPtr->CreateCompatibleRenderTarget(desiredSize->ToUnmanaged(), &bitmapRenderTarget));
	renderBuffer = gcnew BitmapRenderTarget(bitmapRenderTarget);

	return result;
}

/// <summary>
///		Creates a new bitmap render target for use during intermediate offscreen drawing that is compatible
///		with the current render target and has the same size, DPI, and pixel format (but not alpha mode)
///		as the current render target.
/// </summary>
/// <param name="desiredSize">The desired size of the new render target in device-independent pixels if it should be different from the original render target, or null.</param>
/// <param name="desiredPixelSize">The desired size of the new render target in pixels if it should be different from the original render target, or null.</param>
/// <param name="renderBuffer">When this method returns, contains the address of a pointer to a new bitmap render target. This parameter is passed uninitialized.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
/// <remarks>
///			* If desiredSize is specified but desiredPixelSize is not, the pixel size is computed from the desired size using
///			the parent target DPI.
///			* If the desiredSize maps to a integer-pixel size, the DPI of the compatible render target is the
///			same as the DPI of the parent target.
///			* If desiredSize maps to a fractional-pixel size, the pixel size is rounded up to the nearest integer
///			and the DPI for the compatible render target is slightly higher than the DPI of the parent render target.
///			* In all cases, the coordinate (desiredSize.width, desiredSize.height) maps to the lower-right corner of the
///			compatible render target.
///			* If the desiredPixelSize is specified and desiredSize is not, the DPI of the new render target is the same as the original render target.
///			* If both desiredSize and desiredPixelSize are specified, the DPI of the new render target is computed to account for the difference in scale.
///			* If neither desiredSize nor desiredPixelSize is specified, the new render target size and DPI match the original render target. 
/// </remarks>
ResultCode RenderTarget::CreateCompatibleRenderTarget(SizeF^ desiredSize, SizeU^ desiredPixelSize, [System::Runtime::InteropServices::Out] BitmapRenderTarget^ %renderBuffer)
{
	ID2D1BitmapRenderTarget* bitmapRenderTarget;
	ResultCode result = (ResultCode)(this->RenderPtr->CreateCompatibleRenderTarget(desiredSize->ToUnmanaged(), desiredPixelSize->ToUnmanaged(), &bitmapRenderTarget));
	renderBuffer = gcnew BitmapRenderTarget(bitmapRenderTarget);

	return result;
}

/// <summary>
///		Creates a new bitmap render target for use during intermediate offscreen drawing that is compatible
///		with the current render target and has the same size, DPI, and pixel format (but not alpha mode)
///		as the current render target.
/// </summary>
/// <param name="desiredSize">The desired size of the new render target in device-independent pixels if it should be different from the original render target, or null.</param>
/// <param name="desiredPixelSize">The desired size of the new render target in pixels if it should be different from the original render target, or null.</param>
/// <param name="desiredFormat">
///		The desired pixel format and alpha mode of the new render target, or NULL. If the pixel format is set to
///		DXGI_ChannelFormat.FORMAT_UNKNOWN or if this parameter is null, the new render target uses the same pixel
///		format as the original render target. If the alpha mode is AlphaMode.Unknown or this parameter is null,
///		the alpha mode of the new render target defaults to AlphaMode.PreMultiplied.
/// </param>
/// <param name="renderBuffer">When this method returns, contains the address of a pointer to a new bitmap render target. This parameter is passed uninitialized.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
/// <remarks>
///			* If desiredSize is specified but desiredPixelSize is not, the pixel size is computed from the desired size using
///			the parent target DPI.
///			* If the desiredSize maps to a integer-pixel size, the DPI of the compatible render target is the
///			same as the DPI of the parent target.
///			* If desiredSize maps to a fractional-pixel size, the pixel size is rounded up to the nearest integer
///			and the DPI for the compatible render target is slightly higher than the DPI of the parent render target.
///			* In all cases, the coordinate (desiredSize.width, desiredSize.height) maps to the lower-right corner of the
///			compatible render target.
///			* If the desiredPixelSize is specified and desiredSize is not, the DPI of the new render target is the same as the original render target.
///			* If both desiredSize and desiredPixelSize are specified, the DPI of the new render target is computed to account for the difference in scale.
///			* If neither desiredSize nor desiredPixelSize is specified, the new render target size and DPI match the original render target. 
/// </remarks>
ResultCode RenderTarget::CreateCompatibleRenderTarget(SizeF^ desiredSize, SizeU^ desiredPixelSize, Bardez::Projects::DirectX::Direct2D::PixelFormat^ desiredFormat, [System::Runtime::InteropServices::Out] BitmapRenderTarget^ %renderBuffer)
{
	ID2D1BitmapRenderTarget* bitmapRenderTarget;
	ResultCode result = (ResultCode)(this->RenderPtr->CreateCompatibleRenderTarget(desiredSize->ToUnmanaged(), desiredPixelSize->ToUnmanaged(), desiredFormat->ToUnmanaged(), &bitmapRenderTarget));
	renderBuffer = gcnew BitmapRenderTarget(bitmapRenderTarget);

	return result;
}

/// <summary>
///		Creates a new bitmap render target for use during intermediate offscreen drawing that is compatible
///		with the current render target and has the same size, DPI, and pixel format (but not alpha mode)
///		as the current render target.
/// </summary>
/// <param name="desiredSize">The desired size of the new render target in device-independent pixels if it should be different from the original render target, or null.</param>
/// <param name="desiredPixelSize">The desired size of the new render target in pixels if it should be different from the original render target, or null.</param>
/// <param name="desiredFormat">
///		The desired pixel format and alpha mode of the new render target, or NULL. If the pixel format is set to
///		DXGI_ChannelFormat.FORMAT_UNKNOWN or if this parameter is null, the new render target uses the same pixel
///		format as the original render target. If the alpha mode is AlphaMode.Unknown or this parameter is null,
///		the alpha mode of the new render target defaults to AlphaMode.PreMultiplied.
/// </param>
/// <param name="options">A value that specifies whether the new render target must be compatible with GDI.</param>
/// <param name="renderBuffer">When this method returns, contains the address of a pointer to a new bitmap render target. This parameter is passed uninitialized.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
/// <remarks>
///			* If desiredSize is specified but desiredPixelSize is not, the pixel size is computed from the desired size using
///			the parent target DPI.
///			* If the desiredSize maps to a integer-pixel size, the DPI of the compatible render target is the
///			same as the DPI of the parent target.
///			* If desiredSize maps to a fractional-pixel size, the pixel size is rounded up to the nearest integer
///			and the DPI for the compatible render target is slightly higher than the DPI of the parent render target.
///			* In all cases, the coordinate (desiredSize.width, desiredSize.height) maps to the lower-right corner of the
///			compatible render target.
///			* If the desiredPixelSize is specified and desiredSize is not, the DPI of the new render target is the same as the original render target.
///			* If both desiredSize and desiredPixelSize are specified, the DPI of the new render target is computed to account for the difference in scale.
///			* If neither desiredSize nor desiredPixelSize is specified, the new render target size and DPI match the original render target. 
/// </remarks>
ResultCode RenderTarget::CreateCompatibleRenderTarget(SizeF^ desiredSize, SizeU^ desiredPixelSize, Bardez::Projects::DirectX::Direct2D::PixelFormat^ desiredFormat, CompatibleRenderTargetOptions options, [System::Runtime::InteropServices::Out] BitmapRenderTarget^ %renderBuffer)
{
	ID2D1BitmapRenderTarget* bitmapRenderTarget;
	ResultCode result = (ResultCode)(this->RenderPtr->CreateCompatibleRenderTarget(desiredSize->ToUnmanaged(), desiredPixelSize->ToUnmanaged(), desiredFormat->ToUnmanaged(), (D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS)options, &bitmapRenderTarget));
	renderBuffer = gcnew BitmapRenderTarget(bitmapRenderTarget);

	return result;
}

/// <summary>Creates a GradientStopCollection from the specified gradient stops, color interpolation gamma, and extend mode.</summary>
/// <param name="gradientStops">An array of GradientStop references</param>
/// <param name="colorInterpolationGamma">The space in which color interpolation between the gradient stops is performed.</param>
/// <param name="extendMode">The behavior of the gradient outside the [0,1] normalized range.</param>
/// <param name="gradientStopCollection">Output reference to the new gradient stop collection.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode RenderTarget::CreateGradientStopCollection(array<GradientStop^>^ gradientStops, Gamma colorInterpolationGamma, BrushExtendMode extendMode, [System::Runtime::InteropServices::Out] GradientStopCollection^ %gradientStopCollection)
{
	// copy the gradient stop array
	D2D1_GRADIENT_STOP* gradients = new D2D1_GRADIENT_STOP[gradientStops->Length];
	System::UInt32 len = System::Convert::ToUInt32(gradientStops->Length);
	for (System::UInt32 index = 0; index < len; ++index)
		gradients[index] = gradientStops[index]->ToUnmanaged();

	ID2D1GradientStopCollection* gsCollection;

	ResultCode result = (ResultCode)(this->RenderPtr->CreateGradientStopCollection(gradients, len, (D2D1_GAMMA)colorInterpolationGamma, (D2D1_EXTEND_MODE)extendMode, &gsCollection));

	gradientStopCollection = gcnew GradientStopCollection(gsCollection);

	return result;
}

/// <summary>Creates a GradientStopCollection from the specified gradient stops, color interpolation gamma, and extend mode.</summary>
/// <param name="gradientStops">An array of GradientStop references</param>
/// <param name="gradientStopCollection">Output reference to the new gradient stop collection.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode RenderTarget::CreateGradientStopCollection(array<GradientStop^>^ gradientStops, [System::Runtime::InteropServices::Out] GradientStopCollection^ %gradientStopCollection)
{
	// copy the gradient stop array
	D2D1_GRADIENT_STOP* gradients = new D2D1_GRADIENT_STOP[gradientStops->Length];
	System::UInt32 len = System::Convert::ToUInt32(gradientStops->Length);
	for (System::UInt32 index = 0; index < len; ++index)
		gradients[index] = gradientStops[index]->ToUnmanaged();

	ID2D1GradientStopCollection* gsCollection;

	ResultCode result = (ResultCode)(this->RenderPtr->CreateGradientStopCollection(gradients, len, &gsCollection));

	gradientStopCollection = gcnew GradientStopCollection(gsCollection);

	return result;
}

/// <summary>Creates a LinearGradientBrush that contains the specified gradient stops and has the specified transform and base opacity.</summary>
/// <param name="linearGradientBrushProperties">The start and end points of the gradient.</param>
/// <param name="brushProperties">The transform and base opacity of the new brush, or NULL. If this value is NULL, the brush defaults to a base opacity of 1.0f and the identity matrix as its transformation.</param>
/// <param name="gradientStopCollection">A collection of GradientStop objects that describe the colors in the brush's gradient and their locations along the gradient line.</param>
/// <param name="linearGradientBrush">Output reference to the new brush</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode RenderTarget::CreateLinearGradientBrush(LinearGradientBrushProperties^ linearGradientBrushProperties, BrushProperties^ brushProperties, GradientStopCollection^ gradientStopCollection, [System::Runtime::InteropServices::Out] LinearGradientBrush^ %linearGradientBrush)
{
	// brush properties pointer
	D2D1_BRUSH_PROPERTIES* brshPropPtr;
	if (brushProperties == nullptr)
		brshPropPtr = NULL;
	else
		brshPropPtr = &(brushProperties->ToUnmanaged());

	//output pointer
	ID2D1LinearGradientBrush* lnrGrdBrshPtr;
	ResultCode result = (ResultCode)(this->RenderPtr->CreateLinearGradientBrush(&(linearGradientBrushProperties->ToUnmanaged()), brshPropPtr, gradientStopCollection->GradientStopCollectionPtr, &lnrGrdBrshPtr));

	linearGradientBrush = gcnew LinearGradientBrush(lnrGrdBrshPtr);

	return result;
}

/// <summary>Creates a LinearGradientBrush that contains the specified gradient stops and has the specified transform and base opacity.</summary>
/// <param name="linearGradientBrushProperties">The start and end points of the gradient.</param>
/// <param name="gradientStopCollection">A collection of GradientStop objects that describe the colors in the brush's gradient and their locations along the gradient line.</param>
/// <param name="linearGradientBrush">Output reference to the new brush</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode RenderTarget::CreateLinearGradientBrush(LinearGradientBrushProperties^ linearGradientBrushProperties, GradientStopCollection^ gradientStopCollection, [System::Runtime::InteropServices::Out] LinearGradientBrush^ %linearGradientBrush)
{
	//output pointer
	ID2D1LinearGradientBrush* lnrGrdBrshPtr;
	ResultCode result = (ResultCode)(this->RenderPtr->CreateLinearGradientBrush(linearGradientBrushProperties->ToUnmanaged(), gradientStopCollection->GradientStopCollectionPtr, &lnrGrdBrshPtr));

	linearGradientBrush = gcnew LinearGradientBrush(lnrGrdBrshPtr);

	return result;
}

/// <summary>Creates a RadialGradientBrush that contains the specified gradient stops and has the specified transform and base opacity.</summary>
/// <param name="radialGradientBrushProperties">The center, gradient origin offset, and x-radius and y-radius of the brush's gradient.</param>
/// <param name="brushProperties">The transform and base opacity of the new brush, or NULL. If this value is NULL, the brush defaults to a base opacity of 1.0f and the identity matrix as its transformation.</param>
/// <param name="gradientStopCollection">A collection of GradientStop references that describe the colors in the brush's gradient and their locations along the gradient.</param>
/// <param name="radialGradientBrush">When this method returns, contains a pointer to a pointer to the new brush. This parameter is passed uninitialized.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode RenderTarget::CreateRadialGradientBrush(RadialGradientBrushProperties^ radialGradientBrushProperties, BrushProperties^ brushProperties, GradientStopCollection^ gradientStopCollection, [System::Runtime::InteropServices::Out] RadialGradientBrush^ %radialGradientBrush)
{
	// brush properties pointer
	D2D1_BRUSH_PROPERTIES* brshPropPtr;
	if (brushProperties == nullptr)
		brshPropPtr = NULL;
	else
		brshPropPtr = &(brushProperties->ToUnmanaged());

	//output pointer
	ID2D1RadialGradientBrush* rdlGrdBrshPtr;
	ResultCode result = (ResultCode)(this->RenderPtr->CreateRadialGradientBrush(&(radialGradientBrushProperties->ToUnmanaged()), brshPropPtr, gradientStopCollection->GradientStopCollectionPtr, &rdlGrdBrshPtr));

	radialGradientBrush = gcnew RadialGradientBrush(rdlGrdBrshPtr);

	return result;
}

/// <summary>Creates a RadialGradientBrush that contains the specified gradient stops and has the specified transform and base opacity.</summary>
/// <param name="radialGradientBrushProperties">The center, gradient origin offset, and x-radius and y-radius of the brush's gradient.</param>
/// <param name="gradientStopCollection">A collection of GradientStop references that describe the colors in the brush's gradient and their locations along the gradient.</param>
/// <param name="radialGradientBrush">When this method returns, contains a pointer to a pointer to the new brush. This parameter is passed uninitialized.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode RenderTarget::CreateRadialGradientBrush(RadialGradientBrushProperties^ radialGradientBrushProperties, GradientStopCollection^ gradientStopCollection, [System::Runtime::InteropServices::Out] RadialGradientBrush^ %radialGradientBrush)
{
	//output pointer
	ID2D1RadialGradientBrush* rdlGrdBrshPtr;
	ResultCode result = (ResultCode)(this->RenderPtr->CreateRadialGradientBrush(radialGradientBrushProperties->ToUnmanaged(), gradientStopCollection->GradientStopCollectionPtr, &rdlGrdBrshPtr));

	radialGradientBrush = gcnew RadialGradientBrush(rdlGrdBrshPtr);

	return result;
}

/// <summary>Creates a new SolidColorBrush that has the specified color and opacity.</summary>
/// <param name="color">The red, green, blue, and alpha values of the brush's color.</param>
/// <param name="brushProperties">The base opacity of the brush.</param>
/// <param name="solidColorBrush">When this method returns, contains a reference to the new brush. This parameter is passed uninitialized.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode RenderTarget::CreateSolidColorBrush(ColorF^ color, BrushProperties^ brushProperties, [System::Runtime::InteropServices::Out] SolidColorBrush^ %solidColorBrush)
{
	//output pointer
	ID2D1SolidColorBrush* sldBrshPtr;
	ResultCode result = (ResultCode)(this->RenderPtr->CreateSolidColorBrush(color->ToUnmanaged(), brushProperties->ToUnmanaged(), &sldBrshPtr));

	solidColorBrush = gcnew SolidColorBrush(sldBrshPtr);

	return result;
}

/// <summary>Creates a new SolidColorBrush that has the specified color and opacity.</summary>
/// <param name="color">The red, green, blue, and alpha values of the brush's color.</param>
/// <param name="solidColorBrush">When this method returns, contains a reference to the new brush. This parameter is passed uninitialized.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode RenderTarget::CreateSolidColorBrush(ColorF^ color, [System::Runtime::InteropServices::Out] SolidColorBrush^ %solidColorBrush)
{
	//output pointer
	ID2D1SolidColorBrush* sldBrshPtr;
	ResultCode result = (ResultCode)(this->RenderPtr->CreateSolidColorBrush(color->ToUnmanaged(), &sldBrshPtr));

	solidColorBrush = gcnew SolidColorBrush(sldBrshPtr);

	return result;
}

/// <summary>Draws the specified bitmap after scaling it to the size of the specified rectangle.</summary>
/// <param name="bitmap">The bitmap to render.</param>
/// <param name="destinationRectangle">
///		The size and position, in device-independent pixels in the render target's coordinate space,
///		of the area to which the bitmap is drawn; NULL to draw the selected portion of the bitmap at the origin
///		of the render target. If the rectangle is specified but not well-ordered, nothing is drawn,
///		but the render target does not enter an error state.
/// </param>
/// <param name="opacity">
///		A value between 0.0F and 1.0F, inclusive, that specifies an opacity value to apply to the bitmap;
///		this value is multiplied against the alpha values of the bitmap's contents. The default value is 1.0F.
///	</param>
/// <param name="interpolationMode">The interpolation mode to use if the bitmap is scaled or rotated by the drawing operation.</param>
/// <param name="sourceRectangle">
///		The size and position, in device-independent pixels in the bitmap's coordinate space,
///		of the area within the bitmap to be drawn; null to draw the entire bitmap.
///	</param>
void RenderTarget::DrawBitmap(Bitmap^ bitmap, RectangleF^ destinationRectangle, System::Single opacity, BitmapInterpolationMode interpolationMode, RectangleF^ sourceRectangle)
{
	this->RenderPtr->DrawBitmap(bitmap->BitmapPtr, &(destinationRectangle->ToUnmanaged()), opacity, (D2D1_BITMAP_INTERPOLATION_MODE)interpolationMode, &(sourceRectangle->ToUnmanaged()));
}

/// <summary>Draws the outline of the specified ellipse using the specified stroke style.</summary>
/// <param name="ellipse">The position and radius of the ellipse to draw, in device-independent pixels.</param>
/// <param name="brush">The brush used to paint the ellipse's outline.</param>
/// <param name="strokeWidth">The thickness of the ellipse's stroke. The stroke is centered on the ellipse's outline.</param>
/// <param name="strokeStyle">The style of stroke to apply to the ellipse's outline, or NULL to paint a solid stroke.</param>
void RenderTarget::DrawEllipse(Bardez::Projects::DirectX::Direct2D::Ellipse^ ellipse, Brush^ brush, System::Single strokeWidth, StrokeStyle^ strokeStyle)
{
	//nullable pointer
	ID2D1StrokeStyle* strokePtr;
	if (strokeStyle == nullptr)
		strokePtr = NULL;
	else
		strokePtr = strokeStyle->StrokeStylePtr;

	this->RenderPtr->DrawEllipse(ellipse->ToUnmanaged(), brush->BrushPtr, strokeWidth, strokePtr);
}

/// <summary>Draws the outline of the specified ellipse using the specified stroke style.</summary>
/// <param name="ellipse">The position and radius of the ellipse to draw, in device-independent pixels.</param>
/// <param name="brush">The brush used to paint the ellipse's outline.</param>
/// <param name="strokeWidth">The thickness of the ellipse's stroke. The stroke is centered on the ellipse's outline.</param>
void RenderTarget::DrawEllipse(Bardez::Projects::DirectX::Direct2D::Ellipse^ ellipse, Brush^ brush, System::Single strokeWidth)
{
	this->RenderPtr->DrawEllipse(ellipse->ToUnmanaged(), brush->BrushPtr, strokeWidth);
}

/// <summary>Draws a line between the specified points using the specified stroke style.</summary>
/// <param name="point0">The start point of the line, in device-independent pixels.</param>
/// <param name="point1">The end point of the line, in device-independent pixels.</param>
/// <param name="brush">The brush used to paint the line's stroke.</param>
/// <param name="strokeWidth">A value greater than or equal to 0.0f that specifies the width of the stroke. The stroke is centered on the line.</param>
/// <param name="strokeStyle">The style of stroke to paint, or null to paint a solid line.</param>
void RenderTarget::DrawLine(Point2dF^ point0, Point2dF^ point1, Brush^ brush, System::Single strokeWidth, StrokeStyle^ strokeStyle)
{
	ID2D1StrokeStyle* ptr;
	if (strokeStyle == nullptr)
		ptr = NULL;
	else
		ptr = strokeStyle->StrokeStylePtr;

	this->RenderPtr->DrawLine(point0->ToUnmanaged(), point1->ToUnmanaged(), brush->BrushPtr, strokeWidth, ptr);
}

/// <summary>Draws a line between the specified points using the specified stroke style.</summary>
/// <param name="point0">The start point of the line, in device-independent pixels.</param>
/// <param name="point1">The end point of the line, in device-independent pixels.</param>
/// <param name="brush">The brush used to paint the line's stroke.</param>
/// <param name="strokeWidth">A value greater than or equal to 0.0f that specifies the width of the stroke. The stroke is centered on the line.</param>
void RenderTarget::DrawLine(Point2dF^ point0, Point2dF^ point1, Brush^ brush, System::Single strokeWidth)
{
	this->RenderPtr->DrawLine(point0->ToUnmanaged(), point1->ToUnmanaged(), brush->BrushPtr, strokeWidth, NULL);
}

/// <summary>Draws a line between the specified points using the specified stroke style.</summary>
/// <param name="point0">The start point of the line, in device-independent pixels.</param>
/// <param name="point1">The end point of the line, in device-independent pixels.</param>
/// <param name="brush">The brush used to paint the line's stroke.</param>
/// <param name="strokeStyle">The style of stroke to paint, or null to paint a solid line.</param>
void RenderTarget::DrawLine(Point2dF^ point0, Point2dF^ point1, Brush^ brush, StrokeStyle^ strokeStyle)
{
	ID2D1StrokeStyle* ptr;
	if (strokeStyle == nullptr)
		ptr = NULL;
	else
		ptr = strokeStyle->StrokeStylePtr;

	this->RenderPtr->DrawLine(point0->ToUnmanaged(), point1->ToUnmanaged(), brush->BrushPtr, 1.0F, ptr);
}

/// <summary>Draws a line between the specified points using the specified stroke style.</summary>
/// <param name="point0">The start point of the line, in device-independent pixels.</param>
/// <param name="point1">The end point of the line, in device-independent pixels.</param>
/// <param name="brush">The brush used to paint the line's stroke.</param>
void RenderTarget::DrawLine(Point2dF^ point0, Point2dF^ point1, Brush^ brush)
{
	this->RenderPtr->DrawLine(point0->ToUnmanaged(), point1->ToUnmanaged(), brush->BrushPtr, 1.0F, NULL);
}

/// <summary>Draws the outline of a rectangle that has the specified dimensions and stroke style.</summary>
/// <param name="rect">The dimensions of the rectangle to draw, in device-independent pixels.</param>
/// <param name="brush">The brush used to paint the rectangle's stroke.</param>
/// <param name="strokeWidth">A value greater than or equal to 0.0f that specifies the width of the stroke. The stroke is centered on the line.</param>
/// <param name="strokeStyle">The style of stroke to paint, or null to paint a solid stroke.</param>
void RenderTarget::DrawRectangle(RectangleF^ rect, Brush^ brush, System::Single strokeWidth, StrokeStyle^ strokeStyle)
{
	ID2D1StrokeStyle* ptr;
	if (strokeStyle == nullptr)
		ptr = NULL;
	else
		ptr = strokeStyle->StrokeStylePtr;

	this->RenderPtr->DrawRectangle(rect->ToUnmanaged(), brush->BrushPtr, strokeWidth, ptr);
}

/// <summary>Draws the outline of a rectangle that has the specified dimensions and stroke style.</summary>
/// <param name="rect">The dimensions of the rectangle to draw, in device-independent pixels.</param>
/// <param name="brush">The brush used to paint the rectangle's stroke.</param>
/// <param name="strokeWidth">A value greater than or equal to 0.0f that specifies the width of the stroke. The stroke is centered on the line.</param>
void RenderTarget::DrawRectangle(RectangleF^ rect, Brush^ brush, System::Single strokeWidth)
{
	this->RenderPtr->DrawRectangle(rect->ToUnmanaged(), brush->BrushPtr, strokeWidth, NULL);
}

/// <summary>Draws the outline of a rectangle that has the specified dimensions and stroke style.</summary>
/// <param name="rect">The dimensions of the rectangle to draw, in device-independent pixels.</param>
/// <param name="brush">The brush used to paint the rectangle's stroke.</param>
/// <param name="strokeStyle">The style of stroke to paint, or null to paint a solid stroke.</param>
void RenderTarget::DrawRectangle(RectangleF^ rect, Brush^ brush, StrokeStyle^ strokeStyle)
{
	ID2D1StrokeStyle* ptr;
	if (strokeStyle == nullptr)
		ptr = NULL;
	else
		ptr = strokeStyle->StrokeStylePtr;

	this->RenderPtr->DrawRectangle(rect->ToUnmanaged(), brush->BrushPtr, 1.0F, ptr);
}

/// <summary>Draws the outline of a rectangle that has the specified dimensions and stroke style.</summary>
/// <param name="rect">The dimensions of the rectangle to draw, in device-independent pixels.</param>
/// <param name="brush">The brush used to paint the rectangle's stroke.</param>
void RenderTarget::DrawRectangle(RectangleF^ rect, Brush^ brush)
{
	this->RenderPtr->DrawRectangle(rect->ToUnmanaged(), brush->BrushPtr, 1.0F, NULL);
}

/// <summary>Draws the outline of the specified rounded rectangle using the specified stroke style.</summary>
/// <param name="roundedRect">The dimensions of the rectangle to draw, in device-independent pixels.</param>
/// <param name="brush">The brush used to paint the rounded rectangle's outline.</param>
/// <param name="strokeWidth">A value greater than or equal to 0.0f that specifies the width of the stroke. The stroke is centered on the line.</param>
/// <param name="strokeStyle">The style of stroke to paint, or null to paint a solid stroke.</param>
void RenderTarget::DrawRoundedRectangle(RoundedRectangleF^ roundedRect, Brush^ brush, System::Single strokeWidth, StrokeStyle^ strokeStyle)
{
	ID2D1StrokeStyle* ptr;
	if (strokeStyle == nullptr)
		ptr = NULL;
	else
		ptr = strokeStyle->StrokeStylePtr;

	this->RenderPtr->DrawRoundedRectangle(roundedRect->ToUnmanaged(), brush->BrushPtr, strokeWidth, ptr);
}

/// <summary>Draws the outline of the specified rounded rectangle using the specified stroke style.</summary>
/// <param name="roundedRect">The dimensions of the rectangle to draw, in device-independent pixels.</param>
/// <param name="brush">The brush used to paint the rounded rectangle's outline.</param>
/// <param name="strokeWidth">A value greater than or equal to 0.0f that specifies the width of the stroke. The stroke is centered on the line.</param>
void RenderTarget::DrawRoundedRectangle(RoundedRectangleF^ roundedRect, Brush^ brush, System::Single strokeWidth)
{
	this->RenderPtr->DrawRoundedRectangle(roundedRect->ToUnmanaged(), brush->BrushPtr, strokeWidth, NULL);
}

/// <summary>Draws the outline of the specified rounded rectangle using the specified stroke style.</summary>
/// <param name="roundedRect">The dimensions of the rectangle to draw, in device-independent pixels.</param>
/// <param name="brush">The brush used to paint the rounded rectangle's outline.</param>
/// <param name="strokeStyle">The style of stroke to paint, or null to paint a solid stroke.</param>
void RenderTarget::DrawRoundedRectangle(RoundedRectangleF^ roundedRect, Brush^ brush, StrokeStyle^ strokeStyle)
{
	ID2D1StrokeStyle* ptr = (strokeStyle == nullptr) ?  NULL : strokeStyle->StrokeStylePtr;

	this->RenderPtr->DrawRoundedRectangle(roundedRect->ToUnmanaged(), brush->BrushPtr, 1.0F, ptr);
}

/// <summary>Draws the outline of the specified rounded rectangle using the specified stroke style.</summary>
/// <param name="roundedRect">The dimensions of the rectangle to draw, in device-independent pixels.</param>
/// <param name="brush">The brush used to paint the rounded rectangle's outline.</param>
void RenderTarget::DrawRoundedRectangle(RoundedRectangleF^ roundedRect, Brush^ brush)
{
	this->RenderPtr->DrawRoundedRectangle(roundedRect->ToUnmanaged(), brush->BrushPtr, 1.0F, NULL);
}

/// <summary>Ends drawing operations on the render target and indicates the current error state and associated tags.</summary>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode RenderTarget::EndDraw()
{
	return (ResultCode)(this->RenderPtr->EndDraw());
}

/// <summary>Ends drawing operations on the render target and indicates the current error state and associated tags.</summary>
/// <param name="tag1">When this method returns, contains the tag for drawing operations that caused errors or 0 if there were no errors. This parameter is passed uninitialized.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode RenderTarget::EndDraw([System::Runtime::InteropServices::Out] System::UInt64 &tag1)
{
	return (ResultCode)(this->RenderPtr->EndDraw(&tag1));
}

/// <summary>Ends drawing operations on the render target and indicates the current error state and associated tags.</summary>
/// <param name="tag1">When this method returns, contains the tag for drawing operations that caused errors or 0 if there were no errors. This parameter is passed uninitialized.</param>
/// <param name="tag2">When this method returns, contains the tag for drawing operations that caused errors or 0 if there were no errors. This parameter is passed uninitialized.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode RenderTarget::EndDraw([System::Runtime::InteropServices::Out] System::UInt64 &tag1, [System::Runtime::InteropServices::Out] System::UInt64 &tag2)
{
	return (ResultCode)(this->RenderPtr->EndDraw(&tag1, &tag2));
}

/// <summary>Paints the interior of the specified ellipse.</summary>
/// <param name="ellipse">The position and radius, in device-independent pixels, of the ellipse to paint.</param>
/// <param name="brush">The brush used to paint the interior of the ellipse.</param>
void RenderTarget::FillEllipse(Bardez::Projects::DirectX::Direct2D::Ellipse^ ellipse, Brush^ brush)
{
	this->RenderPtr->FillEllipse(ellipse->ToUnmanaged(), brush->BrushPtr);
}

/// <summary>Paints the interior of the specified rectangle.</summary>
/// <param name="rectangle">The dimension of the rectangle to paint, in device-independent pixels.</param>
/// <param name="brush">The brush used to paint the rectangle's interior.</param>
void RenderTarget::FillRectangle(RectangleF^ rectangle, Brush^ brush)
{
	this->RenderPtr->FillRectangle(rectangle->ToUnmanaged(), brush->BrushPtr);
}

/// <summary>Paints the interior of the specified rounded rectangle.</summary>
/// <param name="roundedRect">The dimensions of the rounded rectangle to paint, in device independent pixels.</param>
/// <param name="brush">The brush used to paint the interior of the rounded rectangle.</param>
void RenderTarget::FillRoundedRectangle(RoundedRectangleF^ roundedRect, Brush^ brush)
{
	this->RenderPtr->FillRoundedRectangle(roundedRect->ToUnmanaged(), brush->BrushPtr);
}

/// <summary>Executes all pending drawing commands.</summary>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode RenderTarget::Flush()
{
	return (ResultCode)(this->RenderPtr->Flush());
}

/// <summary>Executes all pending drawing commands.</summary>
/// <param name="tag1">When this method returns, contains the tag for drawing operations that caused errors or 0 if there were no errors. This parameter is passed uninitialized.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode RenderTarget::Flush([System::Runtime::InteropServices::Out] System::UInt64 &tag1)
{
	return (ResultCode)(this->RenderPtr->Flush(&tag1));
}

/// <summary>Executes all pending drawing commands.</summary>
/// <param name="tag1">When this method returns, contains the tag for drawing operations that caused errors or 0 if there were no errors. This parameter is passed uninitialized.</param>
/// <param name="tag2">When this method returns, contains the tag for drawing operations that caused errors or 0 if there were no errors. This parameter is passed uninitialized.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode RenderTarget::Flush([System::Runtime::InteropServices::Out] System::UInt64 &tag1, [System::Runtime::InteropServices::Out] System::UInt64 &tag2)
{
	return (ResultCode)(this->RenderPtr->Flush(&tag1, &tag2));
}

/// <summary>Indicates whether the render target supports the specified properties.</summary>
/// <param name="properties">The render target properties to test.</param>
/// <returns>TRUE if the specified render target properties are supported by this render target; otherwise, FALSE.</returns>
System::Boolean RenderTarget::IsSupported(RenderTargetProperties^ properties)
{
	return System::Convert::ToBoolean(this->RenderPtr->IsSupported(&(properties->ToUnmanaged())));
}

/// <summary>Stops redirecting drawing operations to the layer that is specified by the last <see cref="PushLayer" /> call.</summary>
/// <remarks>A PopLayer must match a previous PushLayer call.</remarks>
void RenderTarget::PopLayer()
{
	this->RenderPtr->PopLayer();
}

/// <summary>Adds the specified layer to the render target so that it receives all subsequent drawing operations until <see cref="PopLayer" /> is called.</summary>
/// <param name="properties">The content bounds, geometric mask, opacity, opacity mask, and antialiasing options for the layer.</param>
/// <param name="layer">The layer that receives subsequent drawing operations.</param>
void RenderTarget::PushLayer(LayerProperties^ properties, Layer^ layer)
{
	this->RenderPtr->PushLayer(&(properties->ToUnmanaged()), layer->LayerPtr);
}

#pragma endregion
