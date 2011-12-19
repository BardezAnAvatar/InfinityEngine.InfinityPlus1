
#ifndef Bardez_Projects_DirectX_Direct2D_Bitmap
#define Bardez_Projects_DirectX_Direct2D_Bitmap

#include "Bitmap Properties.h"
#include "DPI Resolution.h"
#include "Point2dU.h"
#include "RectangleU.h"
#include "SizeF.h"
#include "SizeU.h"
#include "Type Declarations.h"

using namespace Bardez::Projects::Win32;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a Bitmap for Direct2D, logically loaded into the video memory</summary>
				/// <remarks>
				///		An ID2D1Bitmap is a device-dependent resource: your application should create bitmaps after it initializes
				///		the render target with which the bitmap will be used, and recreate the bitmap whenever the render target
				///		needs to be recreated.
				///	</remarks>
				public ref class Bitmap
				{
				#pragma region Fields
				protected:
					/// <summary>Represents a pointer to the native bitmap interface object</summary>
					System::IntPtr bitmapPtr;
				#pragma endregion


				#pragma region Properties
				internal:
					/// <summary>Accesses a pointer to the native bitmap interface object</summary>
					property ID2D1Bitmap* BitmapPtr
					{
						ID2D1Bitmap* get();
						void set(ID2D1Bitmap* value);
					}
				#pragma endregion


				#pragma region Construction
				internal:
					/// <summary>Definition constructor</summary>
					Bitmap(ID2D1Bitmap* handle);
				#pragma endregion


				#pragma region Destruction
					/// <summary>Destrutor</summary>
					/// <remarks>Dispose()</remarks>
					~Bitmap();

					/// <summary>Destrutor</summary>
					/// <remarks>Finalize()</remarks>
					!Bitmap();

					/// <summary>Destrutor logic, disposes the object</summary>
					virtual void DisposeUnmanaged();
				#pragma endregion


				#pragma region Methods
				public:
					/// <summary>Copies the specified region from the specified bitmap into the current bitmap.</summary>
					/// <param name="upperPoint">In the current bitmap, the upper-left corner of the area to which the data is copied. If null, [0,0] will be used</param>
					/// <param name="source">Source bitmap to read from. Cannot be null.</param>
					/// <param name="area">Area rectangle of the Bitmap to read from. If null, the full source/destination area will be copied/written to.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					/// <remarks>
					///		This method does not update the size of the current bitmap. If the contents of the source bitmap do not fit in the current bitmap,
					///		this method fails. Also, note that this method does not perform format conversion, and will fail if the bitmap formats do not match.
					///</remarks>
					virtual ResultCode CopyFromBitmap(Point2dU^ upperPoint, Bitmap^ source, RectangleU^ area);
					
					/// <summary>Copies the specified region from the specified bitmap into the current bitmap.</summary>
					/// <param name="upperPoint">In the current bitmap, the upper-left corner of the area to which the data is copied. If null, [0,0] will be used</param>
					/// <param name="source">Source bitmap to read from. Cannot be null.</param>
					/// <param name="area">Area rectangle of source to read from. If null, the full source/destination area will be copied/written to.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					/// <remarks>
					///		This method does not update the size of the current bitmap. If the contents of the source bitmap do not fit in the current bitmap,
					///		this method fails. Also, note that this method does not perform format conversion, and will fail if the bitmap formats do not match.
					///</remarks>
					virtual ResultCode CopyFromBitmap(System::Drawing::Point upperPoint, Bitmap^ source, System::Drawing::Rectangle area);

					/// <summary>Copies the specified region from the specified rendering target into the current bitmap.</summary>
					/// <param name="upperPoint">In the current bitmap, the upper-left corner of the area to which the data is copied. If null, [0,0] will be used</param>
					/// <param name="source">Source rendering target to read from. Cannot be null.</param>
					/// <param name="area">Area rectangle of source to read from. If null, the full source/destination area will be copied/written to.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					/// <remarks>
					///		This method does not update the size of the current bitmap. If the contents of the source bitmap do not fit in the current bitmap,
					///		this method fails. Also, note that this method does not perform format conversion, and will fail if the bitmap formats do not match.
					///</remarks>
					virtual ResultCode CopyFromRenderTarget(Point2dU^ upperPoint, RenderTarget^ source, RectangleU^ area);

					/// <summary>Copies the specified region from the specified rendering target into the current bitmap.</summary>
					/// <param name="upperPoint">In the current bitmap, the upper-left corner of the area to which the data is copied. If null, [0,0] will be used</param>
					/// <param name="source">Source rendering target to read from. Cannot be null.</param>
					/// <param name="area">Area rectangle of source to read from. If null, the full source/destination area will be copied/written to.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					/// <remarks>
					///		This method does not update the size of the current bitmap. If the contents of the source bitmap do not fit in the current bitmap,
					///		this method fails. Also, note that this method does not perform format conversion, and will fail if the bitmap formats do not match.
					///</remarks>
					virtual ResultCode CopyFromRenderTarget(System::Drawing::Point upperPoint, RenderTarget^ source, System::Drawing::Rectangle area);

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
					virtual ResultCode CopyFromMemory(RectangleU^ area, array<System::Byte>^ source, System::UInt32 pitch);
					
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
					virtual ResultCode CopyFromMemory(System::Drawing::Rectangle area, array<System::Byte>^ source, System::UInt32 pitch);

					/// <summary>Gets the dots per inch of the bitmap</summary>
					/// <returns>The DPI resolution of the bitmap.</returns>
					virtual DpiResolution GetDpi();
					
					/// <summary>Gets the pixel data format of this bitmap</summary>
					/// <returns>The pixel data format of this bitmap.</returns>
					virtual PixelFormat^ GetPixelFormat();
					
					/// <summary>Gets the integer size of this bitmap</summary>
					/// <returns>The integer size of this bitmap.</returns>
					virtual SizeU^ GetPixelSize();
					
					/// <summary>Gets the integer size of this bitmap</summary>
					/// <returns>The integer size of this bitmap.</returns>
					virtual System::Drawing::Size GetPixelFrameworkSize();
					
					/// <summary>Gets the single-precision floating point size, in device-independent pixels (DIPs), of this bitmap</summary>
					/// <returns>The single-precision floating point size of this bitmap.</returns>
					virtual SizeF^ GetSize();
					
					/// <summary>Gets the single-precision floating point size, in device-independent pixels (DIPs), of this bitmap</summary>
					/// <returns>The single-precision floating point size of this bitmap.</returns>
					virtual System::Drawing::SizeF GetFrameworkSize();
				#pragma endregion
				};
			}
		}
	}
}

#endif
