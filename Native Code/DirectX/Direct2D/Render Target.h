
#ifndef Bardez_Projects_DirectX_Direct2D_RenderTarget
#define Bardez_Projects_DirectX_Direct2D_RenderTarget

#include "Type Declarations.h"
#include "AntialiasMode.h"
#include "Bitmap Properties.h"
#include "Bitmap Interpolation Mode.h"
#include "Compatible Render Target Options.h"
#include "DPI Resolution.h"
#include "RectangleF.h"
#include "SizeU.h"

using namespace Bardez::Projects::Win32;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a render target for Direct2D</summary>
				public ref class RenderTarget
				{
				#pragma region Fields
				protected:
					/// <summary>Represents a pointer to the native render target interface object</summary>
					System::IntPtr renderPtr;
				#pragma endregion


				#pragma region Properties
				internal:
					/// <summary>Represents a pointer to the native render target interface object</summary>
					property ID2D1RenderTarget* RenderPtr
					{
						ID2D1RenderTarget* get();
						void set(ID2D1RenderTarget* value);
					}
				#pragma endregion


				#pragma region Construction
				internal:
					/// <summary>Definition constructor</summary>
					RenderTarget(ID2D1RenderTarget* handle);
				#pragma endregion


				#pragma region Interface Methods
				public:
					/// <summary>Begins drawing on the specified surface</summary>
					virtual void BeginDraw();

					/// <summary>Clears the drawing area to the specified color</summary>
					/// <param name="clearColor">Color with which to fill the area with</param>
					virtual void Clear(System::Drawing::Color clearColor);

					/// <summary>Clears the drawing area with transparent black color</summary>
					virtual void Clear();

					/// <summary>Creates an uninitialized Direct2D bitmap. </summary>
					/// <param name="size">The dimension of the bitmap to create in pixels.</param>
					/// <param name="properties">The pixel format and dots per inch (DPI) of the bitmap to create.</param>
					/// <param name="bitmap">When this method returns, contains a pointer to a pointer to the new bitmap. This parameter is passed uninitialized.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode CreateBitmap(System::Drawing::Size size, BitmapProperties^ properties, [System::Runtime::InteropServices::Out] Bitmap^ bitmap);

					/// <summary>Creates an uninitialized Direct2D bitmap. </summary>
					/// <param name="size">The dimension of the bitmap to create in pixels.</param>
					/// <param name="properties">The pixel format and dots per inch (DPI) of the bitmap to create.</param>
					/// <param name="bitmap">When this method returns, contains a pointer to a pointer to the new bitmap. This parameter is passed uninitialized.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode CreateBitmap(SizeU size, BitmapProperties^ properties, [System::Runtime::InteropServices::Out] Bitmap^ bitmap);

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
					virtual ResultCode CreateBitmap(System::Drawing::Size size, array<System::Byte>^ data, System::UInt32 pitch, BitmapProperties^ properties, [System::Runtime::InteropServices::Out] Bitmap^ bitmap);

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
					virtual ResultCode CreateBitmap(SizeU size, array<System::Byte>^ data, System::UInt32 pitch, BitmapProperties^ properties, [System::Runtime::InteropServices::Out] Bitmap^ bitmap);

					//Excluding CreateBitmapBrush(...) overloads for the time being
					//Excluding CreateBitmapFromWicBitmap(...) overloads for the time being

					/// <summay>
					///		Creates a new bitmap render target for use during intermediate offscreen drawing that is compatible
					///		with the current render target and has the same size, DPI, and pixel format (but not alpha mode)
					///		as the current render target.
					/// </summary>
					/// <param name="renderBuffer">Output reference to new RenderTarget.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					/// <remarks>The bitmap render target created by this method is not compatible with GDI and has an alpha mode of AlphaMode.Premultiplied.</remarks>
					virtual ResultCode CreateCompatibleRenderTarget([System::Runtime::InteropServices::Out] BitmapRenderTarget^ renderBuffer);
					
					/// <summay>
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
					virtual ResultCode CreateCompatibleRenderTarget(SizeF^ desiredSize, [System::Runtime::InteropServices::Out] BitmapRenderTarget^ renderBuffer);
					
					/// <summay>
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
					virtual ResultCode CreateCompatibleRenderTarget(SizeF^ desiredSize, SizeU^ desiredPixelSize, [System::Runtime::InteropServices::Out] BitmapRenderTarget^ renderBuffer);
					
					/// <summay>
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
					virtual ResultCode CreateCompatibleRenderTarget(SizeF^ desiredSize, SizeU^ desiredPixelSize, PixelFormat^ desiredFormat, [System::Runtime::InteropServices::Out] BitmapRenderTarget^ renderBuffer);

					/// <summay>
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
					virtual ResultCode CreateCompatibleRenderTarget(SizeF^ desiredSize, SizeU^ desiredPixelSize, PixelFormat^ desiredFormat, CompatibleRenderTargetOptions options, [System::Runtime::InteropServices::Out] BitmapRenderTarget^ renderBuffer);

					
					//Excluding CreateGradientStopCollection(...) overloads for the time being
					//Excluding CreateLayer(...) overloads for the time being
					//Excluding CreateLinearGradientBrush(...) overloads for the time being
					//Excluding CreateMesh(...) overloads for the time being
					//Excluding CreateRadialGradientBrush(...) overloads for the time being
					
					//Excluding CreateSharedBitmap(...) overloads for the time being //would be used for sharing with Direct3D
					//virtual ResultCode CreateSharedBitmap(System::Guid riid, array<System::Byte>^ data, BitmapProperties^ properties, Bitmap^ bitmap);

					//Excluding CreateSolidColorBrush(...) overloads for the time being

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
					virtual void DrawBitmap(Bitmap^ bitmap, RectangleF^ destinationRectangle, System::Single opacity, BitmapInterpolationMode interpolationMode, RectangleF sourceRectangle);
					
					//Excluding DrawEllipse(...) overloads for the time being
					//Excluding DrawGeometry(...) overloads for the time being
					//Excluding DrawGlyphRun(...) overloads for the time being
					//Excluding DrawLine(...) overloads for the time being
					//Excluding DrawRectangle(...) overloads for the time being
					//Excluding DrawRoundedRectangle(...) overloads for the time being
					//Excluding DrawText(...) overloads for the time being
					//Excluding DrawTextLayout(...) overloads for the time being

					/// <summary>Ends drawing operations on the render target and indicates the current error state and associated tags.</summary>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode EndDraw();

					/// <summary>Ends drawing operations on the render target and indicates the current error state and associated tags.</summary>
					/// <param name="tag1">When this method returns, contains the tag for drawing operations that caused errors or 0 if there were no errors. This parameter is passed uninitialized.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode EndDraw([System::Runtime::InteropServices::Out] System::UInt64 tag1);

					/// <summary>Ends drawing operations on the render target and indicates the current error state and associated tags.</summary>
					/// <param name="tag1">When this method returns, contains the tag for drawing operations that caused errors or 0 if there were no errors. This parameter is passed uninitialized.</param>
					/// <param name="tag2">When this method returns, contains the tag for drawing operations that caused errors or 0 if there were no errors. This parameter is passed uninitialized.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode EndDraw([System::Runtime::InteropServices::Out] System::UInt64 tag1, [System::Runtime::InteropServices::Out] System::UInt64 tag2);

					
					//Excluding FillEllipse(...) overloads for the time being
					//Excluding FillGeometry(...) overloads for the time being
					//Excluding FillMesh(...) overloads for the time being
					//Excluding FillOpacityMask(...) overloads for the time being
					//Excluding FillRectangle(...) overloads for the time being
					//Excluding FillRoundedRectangle(...) overloads for the time being
					
					/// <summary>Executes all pending drawing commands.</summary>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode Flush();

					/// <summary>Executes all pending drawing commands.</summary>
					/// <param name="tag1">When this method returns, contains the tag for drawing operations that caused errors or 0 if there were no errors. This parameter is passed uninitialized.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode Flush([System::Runtime::InteropServices::Out] System::UInt64 tag1);

					/// <summary>Executes all pending drawing commands.</summary>
					/// <param name="tag1">When this method returns, contains the tag for drawing operations that caused errors or 0 if there were no errors. This parameter is passed uninitialized.</param>
					/// <param name="tag2">When this method returns, contains the tag for drawing operations that caused errors or 0 if there were no errors. This parameter is passed uninitialized.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode Flush([System::Runtime::InteropServices::Out] System::UInt64 tag1, [System::Runtime::InteropServices::Out] System::UInt64 tag2);
					

/// <summary>Retrieves the current antialiasing mode for nontext drawing operations.</summary>
/// <returns>The current AntialiasMode.</returns>
virtual AntialiasMode GetAntialiasMode();

/// <summary>Retrieves the current antialiasing mode for nontext drawing operations.</summary>
/// <returns>The current render target DPI.</returns>
/// <remarks>
///		This method indicates the mapping from pixel space to device-independent space for the render target.
///		For ID2D1HwndRenderTarget, the DPI defaults to the most recently factory-read system DPI.
///		The default value for all other render targets is 96 DPI.
///</remarks>
virtual DpiResolution GetDpi();

/// <summary>Gets the maximum size, in device-dependent units (pixels), of any one bitmap dimension supported by the render target.</summary>
/// <returns>The maximum size, in pixels, of any one bitmap dimension supported by the render target.</returns>
virtual System::UInt32 GetMaximumBitmapSize();

/// <summary>Retrieves the pixel format and alpha mode of the render target.</summary>
/// <returns>The pixel format and alpha mode of the render target.</returns>
virtual PixelFormat^ GetPixelFormat();

/// <summary>Returns the size of the render target in device pixels.</summary>
/// <returns>The size of the render target in device pixels.</returns>
virtual SizeU^ GetPixelSize();

/// <summary>Returns the size of the render target in device-independent pixels.</summary>
/// <returns>The current size of the render target in device-independent pixels.</returns>
virtual SizeF^ GetSize();

					//Excluding GetTags(...) overloads for the time being
					//Excluding GetTextAntialiasMode(...) overloads for the time being
					//Excluding GetTextRenderingParams(...) overloads for the time being
					//Excluding GetTransform(...) overloads for the time being


				#pragma endregion
				};
			}
		}
	}
}

#endif
