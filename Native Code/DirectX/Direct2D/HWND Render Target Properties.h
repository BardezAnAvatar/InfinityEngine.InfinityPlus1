
#ifndef Bardez_Projects_DirectX_Direct2D_HwndRenderTargetProperties
#define Bardez_Projects_DirectX_Direct2D_HwndRenderTargetProperties

#include <d2d1.h>
#include "Presentation Options.h"
#include "SizeU.h"

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>
				///		Contains rendering options (hardware or software), pixel format, DPI information, remoting options,
				///		and Direct3D support requirements for a render target.
				/// </summary>
				public ref class HwndRenderTargetProperties
				{
				#pragma region Fields
				protected:
					/// <summary>The WiNDow Handle to which the render target issues the output from its drawing commands.</summary>
					System::IntPtr windowHandle;

					/// <summary>The size of the render target, in pixels.</summary>
					/// <values>The default value is a SizeU that has a width and height of 0</values>
					SizeU^ pixelSize;

					/// <summary>A value that specifies whether the render target retains the frame after it is presented and whether the render target waits for the device to refresh before presenting.</summary>
					/// <values>The default value is PresentationOptions.None</values>
					PresentationOptions options;
				#pragma endregion

					
				#pragma region Properties
				public:
					/// <summary>The WiNDow Handle to which the render target issues the output from its drawing commands.</summary>
					property System::IntPtr WindowHandle
					{
						System::IntPtr get();
						void set(System::IntPtr value);
					}

					/// <summary>The size of the render target, in pixels.</summary>
					/// <values>The default value is a SizeU that has a width and height of 0</values>
					property SizeU^ PixelSize
					{
						SizeU^ get();
						void set(SizeU^ value);
					}
					
					/// <summary>A value that specifies whether the render target retains the frame after it is presented and whether the render target waits for the device to refresh before presenting.</summary>
					property PresentationOptions Options
					{
						PresentationOptions get();
						void set(PresentationOptions value);
					}
				#pragma endregion


				#pragma region Construction
				public:
					/// <summary>Default Constructor</summary>
					HwndRenderTargetProperties();

					/// <summary>Definition Constructor</summary>
					/// <param name="windowHandle">Handle for the window being rendered to.</param>
					HwndRenderTargetProperties(System::IntPtr windowHandle);

					/// <summary>Definition Constructor</summary>
					/// <param name="windowHandle">Handle for the window being rendered to.</param>
					/// <param name="size">Dimensions of the render target, in device dependent pixels.</param>
					/// <param name="presentOptions">Specifies whether the render target retains the frame after it is presented and whether the render target waits for the device to refresh before presenting.</param>
					HwndRenderTargetProperties(System::IntPtr windowHandle, SizeU^ size, PresentationOptions presentOptions);
					
				internal:
					/// <summary>Definition Constructor</summary>
					/// <param name="window">Handle for the window being rendered to.</param>
					HwndRenderTargetProperties(HWND window);

					/// <summary>Definition Constructor</summary>
					/// <param name="window">Handle for the window being rendered to.</param>
					/// <param name="size">Dimensions of the render target, in device dependent pixels.</param>
					/// <param name="presentOptions">Specifies whether the render target retains the frame after it is presented and whether the render target waits for the device to refresh before presenting.</param>
					HwndRenderTargetProperties(HWND window, SizeU^ size, PresentationOptions presentOptions);

					/// <summary>Unmanaged Constructor</summary>
					/// <param name="unmanaged">Unmanaged source structure</param>
					HwndRenderTargetProperties(D2D1_HWND_RENDER_TARGET_PROPERTIES unmanaged);
				#pragma endregion


				#pragma region Methods
				internal:
					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct equivalent</returns>
					D2D1_HWND_RENDER_TARGET_PROPERTIES ToUnmanaged();
				#pragma endregion
				};
			}
		}
	}
}

#endif
