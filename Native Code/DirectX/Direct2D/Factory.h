
#ifndef Bardez_Projects_DirectX_Direct2D_Factory
#define Bardez_Projects_DirectX_Direct2D_Factory

#include "Render Target.h"
#include "Bitmap.h"
#include "Bitmap Render Target.h"
#include "Control Render Target.h"
#include "Debug Level.h"
#include "DPI Resolution.h"
#include "HWND Render Target Properties.h"
#include "Factory Type.h"
#include "Presentation Options.h"
#include "Render Target Properties.h"
#include "Stroke Style.h"
#include "Stroke Style Properties.h"

//used to force the linker to link to the correct library
#pragma comment(lib, "D2D1.lib") //for C extern calls

using namespace Bardez::Projects::Win32;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a Direct2D Factory</summary>
				public ref class Factory
				{
				#pragma region Properties
				protected:
					/// <summary>Represents a pointer to the native Direct2D factory interface object</summary>
					System::IntPtr factoryPtr;
				#pragma endregion


				#pragma region Properties
				internal:
					/// <summary>Represents a pointer to the native Direct2D factory interface object</summary>
					property ID2D1Factory* FactoryPtr
					{
						ID2D1Factory* get();
						void set(ID2D1Factory* value);
					}

				public:
					/// <summary>A reference to the IID of ID2D1Factory that is obtained by using __uuidof(ID2D1Factory).</summary>
					static property System::Guid RefGuid
					{
						System::Guid get();
					}
				#pragma endregion


				#pragma region Construction
				public:
					/// <summary>Default constructor</summary>
					Factory();

					/// <summary>Debug constructor</summary>
					Factory(FactoryType factory, DebugLevel level);
				#pragma endregion
					

				#pragma region Destruction
				public:
					/// <summary>Destructor</summary>
					/// <remarks>Dispose()</remarks>
					~Factory();

					/// <summary>Destructor</summary>
					/// <remarks>Finalize()</remarks>
					!Factory();

					/// <summary>Destructor logic, disposes the object</summary>
					void DisposeUnmanaged();
				#pragma endregion


				#pragma region Interface Methods
				public:
					/// <summary>Retrieves the current desktop dots per inch (DPI). To refresh this value, call <See cref="ReloadSystemMetrics" />.</summary>
					/// <returns>the horizontal and vertical DPI of the desktop.</returns>
					/// <remarks>Use this method to obtain the system DPI when setting physical pixel values, such as when you specify the size of a window.</remarks>
					virtual DpiResolution GetDesktopDpi();

					/// <summary>Creates an HwndRenderTarget, a render target that renders to a window.</summary>
					/// <param name="rtProperties">The rendering mode, pixel format, remoting options, DPI information, and the minimum DirectX support required for hardware rendering.</param>
					/// <param name="hwndRtProperties">The window handle, initial size (in pixels), and present options.</param>
					/// <param name="hwnd">When this method returns, contains the address of the pointer to the ID2D1HwndRenderTarget object created by this method.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					/// <remarks>Use this method to obtain the system DPI when setting physical pixel values, such as when you specify the size of a window.</remarks>
					virtual ResultCode CreateHwndRenderTarget(RenderTargetProperties^ rtProperties, HwndRenderTargetProperties^ hwndRtProperties, [System::Runtime::InteropServices::Out] ControlRenderTarget^ %hwnd);

					/// <summary>Creates an StrokeStyle that describes start cap, dash pattern, and other features of a stroke.</summary>
					/// <param name="strokeStyleProperties">Describes the stroke's line cap, dash offset, and other details of a stroke.</param>
					/// <param name="dashes">Array whose elements are set to the length of each dash and space in the dash pattern. The first element sets the length of a dash, the second element sets the length of a space, the third element sets the length of a dash, and so on.</param>
					/// <param name="strokeStyle">Output stroke style created by this method</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode CreateStrokeStyle(StrokeStyleProperties^ strokeStyleProperties, array<System::Single>^ dashes, [System::Runtime::InteropServices::Out] StrokeStyle^ %strokeStyle);

					/// <summary>Creates an StrokeStyle that describes start cap, dash pattern, and other features of a stroke.</summary>
					/// <param name="strokeStyleProperties">Describes the stroke's line cap, dash offset, and other details of a stroke.</param>
					/// <param name="strokeStyle">Output stroke style created by this method</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode CreateStrokeStyle(StrokeStyleProperties^ strokeStyleProperties, [System::Runtime::InteropServices::Out] StrokeStyle^ %strokeStyle);

					/// <summary>Forces the factory to refresh any system defaults that it might have changed since factory creation.</summary>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					/// <remarks>You should call this method before calling the GetDesktopDpi method, to ensure that the system DPI is current.</remarks>
					virtual ResultCode ReloadSystemMetrics();
				#pragma endregion
				};
			}
		}
	}
}

#endif
