
#ifndef Bardez_Projects_DirectX_Direct2D_RenderTarget
#define Bardez_Projects_DirectX_Direct2D_RenderTarget

#include "Type Declarations.h"
#include "Antialias Mode.h"
#include "Bitmap Brush.h"
#include "Bitmap Brush Properties.h"
#include "Bitmap Interpolation Mode.h"
#include "Bitmap Properties.h"
#include "Brush Extend Mode.h"
#include "Brush Properties.h"
#include "ColorF.h"
#include "Compatible Render Target Options.h"
#include "DPI Resolution.h"
#include "Ellipse.h"
#include "Gamma.h"
#include "Gradient Stop.h"
#include "Gradient Stop Collection.h"
#include "Layer.h"
#include "Layer Properties.h"
#include "Linear Gradient Brush.h"
#include "Linear Gradient Brush Properties.h"
#include "Matrix3x2F.h"
#include "Pixel Format.h"
#include "RectangleF.h"
#include "Radial Gradient Brush.h"
#include "Radial Gradient Brush Properties.h"
#include "Render Target Properties.h"
#include "Rounded RectangleF.h"
#include "SizeF.h"
#include "SizeU.h"
#include "Solid Color Brush.h"
#include "Stroke Style.h"

using namespace System::Threading;
using namespace Bardez::Projects::BasicStructures::Win32;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a render target for Direct2D</summary>
				public ref class RenderTarget abstract
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

				public:
					/// <summary>The antialiasing mode for nontext drawing operations.</summary>
					virtual property AntialiasMode AntiAliasMode
					{
						AntialiasMode get();
						void set(AntialiasMode value);
					}
					
					/// <summary>The dots per inch (DPI) of the render target.</summary>
					virtual property DpiResolution DPI
					{
						DpiResolution get();
						void set(DpiResolution value);
					}

					/// <summary>The maximum size, in device-dependent units (pixels), of any one bitmap dimension supported by the render target.</summary>
					virtual property System::UInt32 MaximumBitmapSize
					{
						System::UInt32 get();
					}

					/// <summary>The pixel format and alpha mode of the render target.</summary>
					virtual property PixelFormat^ PixelFormat
					{
						Bardez::Projects::DirectX::Direct2D::PixelFormat^ get();
					}
					
					/// <summary>The size of the render target in device pixels.</summary>
					virtual property SizeU^ PixelSize
					{
						SizeU^ get();
					}
					
					/// <summary>The size of the render target in device-independent pixels.</summary>
					virtual property SizeF^ Size
					{
						SizeF^ get();
					}
					
					/// <summary>Exposes transform of the render target.</summary>
					virtual property Matrix3x2F^ Transform
					{
						Matrix3x2F^ get();
						void set(Matrix3x2F^ value);
					}
				#pragma endregion


				#pragma region Construction
				internal:
					/// <summary>Definition constructor</summary>
					RenderTarget(ID2D1RenderTarget* handle);
				#pragma endregion
					

				#pragma region Destruction
				public:
					/// <summary>Destructor</summary>
					/// <remarks>Dispose()</remarks>
					~RenderTarget();

					/// <summary>Destructor</summary>
					/// <remarks>Finalize()</remarks>
					!RenderTarget();

					/// <summary>Destructor logic, disposes the object</summary>
					virtual void DisposeUnmanaged();
				#pragma endregion


				#pragma region Interface Methods
				public:
					/// <summary>Begins drawing on the specified surface</summary>
					virtual void BeginDraw();

					/// <summary>Clears the drawing area to the specified color</summary>
					/// <param name="clearColor">Color with which to fill the area with</param>
					virtual void Clear(System::Drawing::Color clearColor);

					/// <summary>Clears the drawing area to the specified color</summary>
					/// <param name="clearColor">Color with which to fill the area with</param>
					virtual void Clear(ColorF^ clearColor);

					/// <summary>Clears the drawing area with transparent black color</summary>
					virtual void Clear();

					/// <summary>Creates an uninitialized Direct2D bitmap.</summary>
					/// <param name="size">The dimension of the bitmap to create in pixels.</param>
					/// <param name="properties">The pixel format and dots per inch (DPI) of the bitmap to create.</param>
					/// <param name="bitmap">When this method returns, contains a pointer to a pointer to the new bitmap. This parameter is passed uninitialized.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode CreateBitmap(System::Drawing::Size size, BitmapProperties^ properties, [System::Runtime::InteropServices::Out] Bitmap^ %bitmap);

					/// <summary>Creates an uninitialized Direct2D bitmap.</summary>
					/// <param name="size">The dimension of the bitmap to create in pixels.</param>
					/// <param name="properties">The pixel format and dots per inch (DPI) of the bitmap to create.</param>
					/// <param name="bitmap">When this method returns, contains a pointer to a pointer to the new bitmap. This parameter is passed uninitialized.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode CreateBitmap(SizeU^ size, BitmapProperties^ properties, [System::Runtime::InteropServices::Out] Bitmap^ %bitmap);

					/// <summary>Creates an uninitialized Direct2D bitmap.</summary>
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
					virtual ResultCode CreateBitmap(System::Drawing::Size size, array<System::Byte>^ data, System::UInt32 pitch, BitmapProperties^ properties, [System::Runtime::InteropServices::Out] Bitmap^ %bitmap);

					/// <summary>Creates an uninitialized Direct2D bitmap.</summary>
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
					virtual ResultCode CreateBitmap(SizeU^ size, array<System::Byte>^ data, System::UInt32 pitch, BitmapProperties^ properties, [System::Runtime::InteropServices::Out] Bitmap^ %bitmap);

					/// <summary>Creates an BitmapBrush from the specified bitmap</summary>
					/// <param name="bitmap">The bitmap contents of the new brush.</param>
					/// <param name="bitmapBrushProperties">The extend modes and interpolation mode of the new brush, or null.</param>
					/// <param name="brushProperties">A structure that contains the opacity and transform of the new brush, or null.</param>
					/// <param name="bitmapBrush">Output parameter containing a reference to the new brush</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode CreateBitmapBrush(Bitmap^ bitmap, BitmapBrushProperties^ bitmapBrushProperties, BrushProperties^ brushProperties, [System::Runtime::InteropServices::Out] BitmapBrush^ %bitmapBrush);
					
					/// <summary>Creates an BitmapBrush from the specified bitmap</summary>
					/// <param name="bitmap">The bitmap contents of the new brush.</param>
					/// <param name="bitmapBrushProperties">The extend modes and interpolation mode of the new brush, or null.</param>
					/// <param name="bitmapBrush">Output parameter containing a reference to the new brush</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode CreateBitmapBrush(Bitmap^ bitmap, BitmapBrushProperties^ bitmapBrushProperties, [System::Runtime::InteropServices::Out] BitmapBrush^ %bitmapBrush);

					/// <summary>Creates an BitmapBrush from the specified bitmap</summary>
					/// <param name="bitmap">The bitmap contents of the new brush.</param>
					/// <param name="bitmapBrush">Output parameter containing a reference to the new brush</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode CreateBitmapBrush(Bitmap^ bitmap, [System::Runtime::InteropServices::Out] BitmapBrush^ %bitmapBrush);
					
					/// <summary>
					///		Creates a new bitmap render target for use during intermediate offscreen drawing that is compatible
					///		with the current render target and has the same size, DPI, and pixel format (but not alpha mode)
					///		as the current render target.
					/// </summary>
					/// <param name="renderBuffer">Output reference to new RenderTarget.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					/// <remarks>The bitmap render target created by this method is not compatible with GDI and has an alpha mode of AlphaMode.Premultiplied.</remarks>
					virtual ResultCode CreateCompatibleRenderTarget([System::Runtime::InteropServices::Out] BitmapRenderTarget^ %renderBuffer);
					
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
					virtual ResultCode CreateCompatibleRenderTarget(SizeF^ desiredSize, [System::Runtime::InteropServices::Out] BitmapRenderTarget^ %renderBuffer);
					
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
					virtual ResultCode CreateCompatibleRenderTarget(SizeF^ desiredSize, SizeU^ desiredPixelSize, [System::Runtime::InteropServices::Out] BitmapRenderTarget^ %renderBuffer);
					
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
					virtual ResultCode CreateCompatibleRenderTarget(SizeF^ desiredSize, SizeU^ desiredPixelSize, Bardez::Projects::DirectX::Direct2D::PixelFormat^ desiredFormat, [System::Runtime::InteropServices::Out] BitmapRenderTarget^ %renderBuffer);

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
					virtual ResultCode CreateCompatibleRenderTarget(SizeF^ desiredSize, SizeU^ desiredPixelSize, Bardez::Projects::DirectX::Direct2D::PixelFormat^ desiredFormat, CompatibleRenderTargetOptions options, [System::Runtime::InteropServices::Out] BitmapRenderTarget^ %renderBuffer);

					/// <summary>Creates a GradientStopCollection from the specified gradient stops, color interpolation gamma, and extend mode.</summary>
					/// <param name="gradientStops">An array of GradientStop references</param>
					/// <param name="colorInterpolationGamma">The space in which color interpolation between the gradient stops is performed.</param>
					/// <param name="extendMode">The behavior of the gradient outside the [0,1] normalized range.</param>
					/// <param name="gradientStopCollection">Output reference to the new gradient stop collection.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode CreateGradientStopCollection(array<GradientStop^>^ gradientStops, Gamma colorInterpolationGamma, BrushExtendMode extendMode, [System::Runtime::InteropServices::Out] GradientStopCollection^ %gradientStopCollection);

					/// <summary>Creates a GradientStopCollection from the specified gradient stops, color interpolation gamma, and extend mode.</summary>
					/// <param name="gradientStops">An array of GradientStop references</param>
					/// <param name="gradientStopCollection">Output reference to the new gradient stop collection.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode CreateGradientStopCollection(array<GradientStop^>^ gradientStops, [System::Runtime::InteropServices::Out] GradientStopCollection^ %gradientStopCollection);

					/// <summary>Creates a LinearGradientBrush that contains the specified gradient stops and has the specified transform and base opacity.</summary>
					/// <param name="linearGradientBrushProperties">The start and end points of the gradient.</param>
					/// <param name="brushProperties">The transform and base opacity of the new brush, or NULL. If this value is NULL, the brush defaults to a base opacity of 1.0f and the identity matrix as its transformation.</param>
					/// <param name="gradientStopCollection">A collection of GradientStop objects that describe the colors in the brush's gradient and their locations along the gradient line.</param>
					/// <param name="linearGradientBrush">Output reference to the new brush</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode CreateLinearGradientBrush(LinearGradientBrushProperties^ linearGradientBrushProperties, BrushProperties^ brushProperties, GradientStopCollection^ gradientStopCollection, [System::Runtime::InteropServices::Out] LinearGradientBrush^ %linearGradientBrush);

					/// <summary>Creates a LinearGradientBrush that contains the specified gradient stops and has the specified transform and base opacity.</summary>
					/// <param name="linearGradientBrushProperties">The start and end points of the gradient.</param>
					/// <param name="gradientStopCollection">A collection of GradientStop objects that describe the colors in the brush's gradient and their locations along the gradient line.</param>
					/// <param name="linearGradientBrush">Output reference to the new brush</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode CreateLinearGradientBrush(LinearGradientBrushProperties^ linearGradientBrushProperties, GradientStopCollection^ gradientStopCollection, [System::Runtime::InteropServices::Out] LinearGradientBrush^ %linearGradientBrush);

					/// <summary>Creates a RadialGradientBrush that contains the specified gradient stops and has the specified transform and base opacity.</summary>
					/// <param name="radialGradientBrushProperties">The center, gradient origin offset, and x-radius and y-radius of the brush's gradient.</param>
					/// <param name="brushProperties">The transform and base opacity of the new brush, or NULL. If this value is NULL, the brush defaults to a base opacity of 1.0f and the identity matrix as its transformation.</param>
					/// <param name="gradientStopCollection">A collection of GradientStop references that describe the colors in the brush's gradient and their locations along the gradient.</param>
					/// <param name="radialGradientBrush">When this method returns, contains a pointer to a pointer to the new brush. This parameter is passed uninitialized.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode CreateRadialGradientBrush(RadialGradientBrushProperties^ radialGradientBrushProperties, BrushProperties^ brushProperties, GradientStopCollection^ gradientStopCollection, [System::Runtime::InteropServices::Out] RadialGradientBrush^ %radialGradientBrush);

					/// <summary>Creates a RadialGradientBrush that contains the specified gradient stops and has the specified transform and base opacity.</summary>
					/// <param name="radialGradientBrushProperties">The center, gradient origin offset, and x-radius and y-radius of the brush's gradient.</param>
					/// <param name="gradientStopCollection">A collection of GradientStop references that describe the colors in the brush's gradient and their locations along the gradient.</param>
					/// <param name="radialGradientBrush">When this method returns, contains a pointer to a pointer to the new brush. This parameter is passed uninitialized.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode CreateRadialGradientBrush(RadialGradientBrushProperties^ radialGradientBrushProperties, GradientStopCollection^ gradientStopCollection, [System::Runtime::InteropServices::Out] RadialGradientBrush^ %radialGradientBrush);

					/// <summary>Creates a new SolidColorBrush that has the specified color and opacity.</summary>
					/// <param name="color">The red, green, blue, and alpha values of the brush's color.</param>
					/// <param name="brushProperties">The base opacity of the brush.</param>
					/// <param name="solidColorBrush">When this method returns, contains a pointer to a pointer to the new brush. This parameter is passed uninitialized.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode CreateSolidColorBrush(ColorF^ color, BrushProperties^ brushProperties, [System::Runtime::InteropServices::Out] SolidColorBrush^ %solidColorBrush);

					/// <summary>Creates a new SolidColorBrush that has the specified color and opacity.</summary>
					/// <param name="color">The red, green, blue, and alpha values of the brush's color.</param>
					/// <param name="solidColorBrush">When this method returns, contains a pointer to a pointer to the new brush. This parameter is passed uninitialized.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode CreateSolidColorBrush(ColorF^ color, [System::Runtime::InteropServices::Out] SolidColorBrush^ %solidColorBrush);

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
					virtual void DrawBitmap(Bitmap^ bitmap, RectangleF^ destinationRectangle, System::Single opacity, BitmapInterpolationMode interpolationMode, RectangleF^ sourceRectangle);

					/// <summary>Draws the outline of the specified ellipse using the specified stroke style.</summary>
					/// <param name="ellipse">The position and radius of the ellipse to draw, in device-independent pixels.</param>
					/// <param name="brush">The brush used to paint the ellipse's outline.</param>
					/// <param name="strokeWidth">The thickness of the ellipse's stroke. The stroke is centered on the ellipse's outline.</param>
					/// <param name="strokeStyle">The style of stroke to apply to the ellipse's outline, or NULL to paint a solid stroke.</param>
					virtual void DrawEllipse(Ellipse^ ellipse, Brush^ brush, System::Single strokeWidth, StrokeStyle^ strokeStyle);

					/// <summary>Draws the outline of the specified ellipse using the specified stroke style.</summary>
					/// <param name="ellipse">The position and radius of the ellipse to draw, in device-independent pixels.</param>
					/// <param name="brush">The brush used to paint the ellipse's outline.</param>
					/// <param name="strokeWidth">The thickness of the ellipse's stroke. The stroke is centered on the ellipse's outline.</param>
					virtual void DrawEllipse(Ellipse^ ellipse, Brush^ brush, System::Single strokeWidth);

					/// <summary>Draws a line between the specified points using the specified stroke style.</summary>
					/// <param name="point0">The start point of the line, in device-independent pixels.</param>
					/// <param name="point1">The end point of the line, in device-independent pixels.</param>
					/// <param name="brush">The brush used to paint the line's stroke.</param>
					/// <param name="strokeWidth">A value greater than or equal to 0.0f that specifies the width of the stroke. The stroke is centered on the line.</param>
					/// <param name="strokeStyle">The style of stroke to paint, or null to paint a solid line.</param>
					virtual void DrawLine(Point2dF^ point0, Point2dF^ point1, Brush^ brush, System::Single strokeWidth, StrokeStyle^ strokeStyle);

					/// <summary>Draws a line between the specified points using the specified stroke style.</summary>
					/// <param name="point0">The start point of the line, in device-independent pixels.</param>
					/// <param name="point1">The end point of the line, in device-independent pixels.</param>
					/// <param name="brush">The brush used to paint the line's stroke.</param>
					/// <param name="strokeWidth">A value greater than or equal to 0.0f that specifies the width of the stroke. The stroke is centered on the line.</param>
					virtual void DrawLine(Point2dF^ point0, Point2dF^ point1, Brush^ brush, System::Single strokeWidth);

					/// <summary>Draws a line between the specified points using the specified stroke style.</summary>
					/// <param name="point0">The start point of the line, in device-independent pixels.</param>
					/// <param name="point1">The end point of the line, in device-independent pixels.</param>
					/// <param name="brush">The brush used to paint the line's stroke.</param>
					/// <param name="strokeStyle">The style of stroke to paint, or null to paint a solid line.</param>
					virtual void DrawLine(Point2dF^ point0, Point2dF^ point1, Brush^ brush, StrokeStyle^ strokeStyle);

					/// <summary>Draws a line between the specified points using the specified stroke style.</summary>
					/// <param name="point0">The start point of the line, in device-independent pixels.</param>
					/// <param name="point1">The end point of the line, in device-independent pixels.</param>
					/// <param name="brush">The brush used to paint the line's stroke.</param>
					virtual void DrawLine(Point2dF^ point0, Point2dF^ point1, Brush^ brush);

					/// <summary>Draws the outline of a rectangle that has the specified dimensions and stroke style.</summary>
					/// <param name="rect">The dimensions of the rectangle to draw, in device-independent pixels.</param>
					/// <param name="brush">The brush used to paint the rectangle's stroke.</param>
					/// <param name="strokeWidth">A value greater than or equal to 0.0f that specifies the width of the stroke. The stroke is centered on the line.</param>
					/// <param name="strokeStyle">The style of stroke to paint, or null to paint a solid stroke.</param>
					virtual void DrawRectangle(RectangleF^ rect, Brush^ brush, System::Single strokeWidth, StrokeStyle^ strokeStyle);

					/// <summary>Draws the outline of a rectangle that has the specified dimensions and stroke style.</summary>
					/// <param name="rect">The dimensions of the rectangle to draw, in device-independent pixels.</param>
					/// <param name="brush">The brush used to paint the rectangle's stroke.</param>
					/// <param name="strokeWidth">A value greater than or equal to 0.0f that specifies the width of the stroke. The stroke is centered on the line.</param>
					virtual void DrawRectangle(RectangleF^ rect, Brush^ brush, System::Single strokeWidth);

					/// <summary>Draws the outline of a rectangle that has the specified dimensions and stroke style.</summary>
					/// <param name="rect">The dimensions of the rectangle to draw, in device-independent pixels.</param>
					/// <param name="brush">The brush used to paint the line's stroke.</param>
					/// <param name="strokeStyle">The brush used to paint the rectangle's stroke.</param>
					virtual void DrawRectangle(RectangleF^ rect, Brush^ brush, StrokeStyle^ strokeStyle);

					/// <summary>Draws the outline of a rectangle that has the specified dimensions and stroke style.</summary>
					/// <param name="rect">The dimensions of the rectangle to draw, in device-independent pixels.</param>
					/// <param name="brush">The brush used to paint the rectangle's stroke.</param>
					virtual void DrawRectangle(RectangleF^ rect, Brush^ brush);

					/// <summary>Draws the outline of the specified rounded rectangle using the specified stroke style.</summary>
					/// <param name="roundedRect">The dimensions of the rectangle to draw, in device-independent pixels.</param>
					/// <param name="brush">The brush used to paint the rounded rectangle's outline.</param>
					/// <param name="strokeWidth">A value greater than or equal to 0.0f that specifies the width of the stroke. The stroke is centered on the line.</param>
					/// <param name="strokeStyle">The style of stroke to paint, or null to paint a solid stroke.</param>
					virtual void DrawRoundedRectangle(RoundedRectangleF^ roundedRect, Brush^ brush, System::Single strokeWidth, StrokeStyle^ strokeStyle);

					/// <summary>Draws the outline of the specified rounded rectangle using the specified stroke style.</summary>
					/// <param name="roundedRect">The dimensions of the rectangle to draw, in device-independent pixels.</param>
					/// <param name="brush">The brush used to paint the rounded rectangle's outline.</param>
					/// <param name="strokeWidth">A value greater than or equal to 0.0f that specifies the width of the stroke. The stroke is centered on the line.</param>
					virtual void DrawRoundedRectangle(RoundedRectangleF^ roundedRect, Brush^ brush, System::Single strokeWidth);

					/// <summary>Draws the outline of the specified rounded rectangle using the specified stroke style.</summary>
					/// <param name="roundedRect">The dimensions of the rectangle to draw, in device-independent pixels.</param>
					/// <param name="brush">The brush used to paint the rounded rectangle's outline.</param>
					/// <param name="strokeStyle">The style of stroke to paint, or null to paint a solid stroke.</param>
					virtual void DrawRoundedRectangle(RoundedRectangleF^ roundedRect, Brush^ brush, StrokeStyle^ strokeStyle);

					/// <summary>Draws the outline of the specified rounded rectangle using the specified stroke style.</summary>
					/// <param name="roundedRect">The dimensions of the rectangle to draw, in device-independent pixels.</param>
					/// <param name="brush">The brush used to paint the rounded rectangle's outline.</param>
					virtual void DrawRoundedRectangle(RoundedRectangleF^ roundedRect, Brush^ brush);

					/// <summary>Ends drawing operations on the render target and indicates the current error state and associated tags.</summary>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode EndDraw();

					/// <summary>Ends drawing operations on the render target and indicates the current error state and associated tags.</summary>
					/// <param name="tag1">When this method returns, contains the tag for drawing operations that caused errors or 0 if there were no errors. This parameter is passed uninitialized.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode EndDraw([System::Runtime::InteropServices::Out] System::UInt64 &tag1);

					/// <summary>Ends drawing operations on the render target and indicates the current error state and associated tags.</summary>
					/// <param name="tag1">When this method returns, contains the tag for drawing operations that caused errors or 0 if there were no errors. This parameter is passed uninitialized.</param>
					/// <param name="tag2">When this method returns, contains the tag for drawing operations that caused errors or 0 if there were no errors. This parameter is passed uninitialized.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode EndDraw([System::Runtime::InteropServices::Out] System::UInt64 &tag1, [System::Runtime::InteropServices::Out] System::UInt64 &tag2);
					
					/// <summary>Paints the interior of the specified ellipse.</summary>
					/// <param name="ellipse">The position and radius, in device-independent pixels, of the ellipse to paint.</param>
					/// <param name="brush">The brush used to paint the interior of the ellipse.</param>
					virtual void FillEllipse(Ellipse^ ellipse, Brush^ brush);

					/// <summary>Paints the interior of the specified rectangle.</summary>
					/// <param name="rectangle">The dimension of the rectangle to paint, in device-independent pixels.</param>
					/// <param name="brush">The brush used to paint the rectangle's interior.</param>
					virtual void FillRectangle(RectangleF^ rectangle, Brush^ brush);

					/// <summary>Paints the interior of the specified rounded rectangle.</summary>
					/// <param name="roundedRect">The dimensions of the rounded rectangle to paint, in device independent pixels.</param>
					/// <param name="brush">The brush used to paint the interior of the rounded rectangle.</param>
					virtual void FillRoundedRectangle(RoundedRectangleF^ roundedRect, Brush^ brush);

					/// <summary>Executes all pending drawing commands.</summary>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode Flush();

					/// <summary>Executes all pending drawing commands.</summary>
					/// <param name="tag1">When this method returns, contains the tag for drawing operations that caused errors or 0 if there were no errors. This parameter is passed uninitialized.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode Flush([System::Runtime::InteropServices::Out] System::UInt64 &tag1);

					/// <summary>Executes all pending drawing commands.</summary>
					/// <param name="tag1">When this method returns, contains the tag for drawing operations that caused errors or 0 if there were no errors. This parameter is passed uninitialized.</param>
					/// <param name="tag2">When this method returns, contains the tag for drawing operations that caused errors or 0 if there were no errors. This parameter is passed uninitialized.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode Flush([System::Runtime::InteropServices::Out] System::UInt64 &tag1, [System::Runtime::InteropServices::Out] System::UInt64 &tag2);

					/// <summary>Indicates whether the render target supports the specified properties.</summary>
					/// <param name="properties">The render target properties to test.</param>
					/// <returns>TRUE if the specified render target properties are supported by this render target; otherwise, FALSE.</returns>
					virtual System::Boolean IsSupported(RenderTargetProperties^ properties);

					/// <summary>Stops redirecting drawing operations to the layer that is specified by the last <see cref="PushLayer" /> call.</summary>
					/// <remarks>A PopLayer must match a previous PushLayer call.</remarks>
					virtual void PopLayer();
					
					/// <summary>Adds the specified layer to the render target so that it receives all subsequent drawing operations until <see cref="PopLayer" /> is called.</summary>
					/// <param name="properties">The content bounds, geometric mask, opacity, opacity mask, and antialiasing options for the layer.</param>
					/// <param name="layer">The layer that receives subsequent drawing operations.</param>
					virtual void PushLayer(LayerProperties^ properties, Layer^ layer);

					#pragma region Excluded methods
					//Excluding CreateBitmapFromWicBitmap(...) overloads for the time being
					//Excluding CreateLayer(...) overloads for the time being
					//Excluding CreateMesh(...) overloads for the time being
					//Excluding CreateSharedBitmap(...) overloads for the time being //would be used for sharing with Direct3D
					//virtual ResultCode CreateSharedBitmap(System::Guid riid, array<System::Byte>^ data, BitmapProperties^ properties, Bitmap^ bitmap);
					//Excluding DrawGeometry(...) overloads for the time being
					//Excluding DrawGlyphRun(...) overloads for the time being
					//Excluding DrawText(...) overloads for the time being
					//Excluding DrawTextLayout(...) overloads for the time being
					//Excluding FillGeometry(...) overloads for the time being
					//Excluding FillMesh(...) overloads for the time being
					//Excluding FillOpacityMask(...) overloads for the time being
					//Excluding GetTags(...) overloads for the time being
					//Excluding GetTextAntialiasMode(...) overloads for the time being
					//Excluding GetTextRenderingParams(...) overloads for the time being
					//Excluding PopAxisAlignedClip(...) overloads for the time being
					//Excluding RestoreDrawingState(...) overloads for the time being
					//Excluding SaveDrawingState(...) overloads for the time being
					//Excluding SetTags(...) overloads for the time being
					//Excluding SetTextAntialiasMode(...) overloads for the time being
					//Excluding SetTextRenderingParams(...) overloads for the time being
				#pragma endregion
				#pragma endregion
				};
			}
		}
	}
}

#endif
