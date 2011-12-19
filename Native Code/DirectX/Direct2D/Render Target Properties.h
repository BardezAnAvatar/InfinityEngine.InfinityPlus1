
#ifndef Bardez_Projects_DirectX_Direct2D_RenderTargetProperties
#define Bardez_Projects_DirectX_Direct2D_RenderTargetProperties

#include <d2d1.h>
#include "DirectX Version.h"
#include "DPI Resolution.h"
#include "Pixel Format.h"
#include "Render Target Type.h"
#include "Render Target Usage.h"

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
				public ref class RenderTargetProperties
				{
				#pragma region Fields
				protected:
					/// <summary>
					///		A value that specifies whether the render target should force hardware or software rendering.
					///		A value of D2D1_RENDER_TARGET_TYPE_DEFAULT specifies that the render target should use hardware
					///		rendering if it is available; otherwise, it uses software rendering. Note that WIC bitmap render
					///		targets do not support hardware rendering.
					/// </summary>
					RenderTargetType type;
					
					/// <summary>
					///		The pixel format and alpha mode of the render target. You can use the D2D1::PixelFormat function to
					///		create a pixel format that specifies that Direct2D should select the pixel format and alpha mode for you.
					/// </summary>
					PixelFormat^ pixelFormat;

					/// <summary>Represents the DPI resolution. For default values, set both X and Y to 0.</summary>
					/// <remarks>The default DPI varies depending on the render target</remarks>
					DpiResolution resolution;

					/// <summary>A value that specifies how the render target is remoted and whether it should be GDI-compatible.</summary>
					/// <remarks>Set to RenderTargetUsage.None to create a render target that is not compatible with GDI and uses Direct3D command-stream remoting if it is available.</remarks>
					RenderTargetUsage usage;
					
					/// <summary>A value that specifies the minimum Direct3D feature level required for hardware rendering.</summary>
					/// <remarks>
					///		If the specified minimum level is not available, the render target uses software rendering if the type member
					///		is set to RenderTargetType.Default; if type is set to to RenderTargetType.Hardware, render target
					///		creation fails. A value of DirectXVerions.Default indicates that Direct2D should determine whether the
					///		Direct3D feature level of the device is adequate.
					///		This field is used only when creating HwndRenderTarget and DCRenderTarget objects.
					/// </remarks>
					DirectXVersion minVersion;
				#pragma endregion

					
				#pragma region Properties
				public:
					/// <summary>
					///		A value that specifies whether the render target should force hardware or software rendering.
					///		A value of D2D1_RENDER_TARGET_TYPE_DEFAULT specifies that the render target should use hardware
					///		rendering if it is available; otherwise, it uses software rendering. Note that WIC bitmap render
					///		targets do not support hardware rendering.
					/// </summary>
					property RenderTargetType Type
					{
						RenderTargetType get();
						void set(RenderTargetType value);
					}
					
					/// <summary>
					///		The pixel format and alpha mode of the render target. You can use the D2D1::PixelFormat function to
					///		create a pixel format that specifies that Direct2D should select the pixel format and alpha mode for you.
					/// </summary>
					property PixelFormat^ Pixel_Format
					{
						PixelFormat^ get();
						void set(PixelFormat^ value);
					}

					/// <summary>Represents the DPI resolution. For default values, set both X and Y to 0.</summary>
					/// <remarks>The default DPI varies depending on the render target</remarks>
					property DpiResolution Resolution
					{
						DpiResolution get();
						void set(DpiResolution value);
					}

					/// <summary>A value that specifies how the render target is remoted and whether it should be GDI-compatible.</summary>
					/// <remarks>Set to RenderTargetUsage.None to create a render target that is not compatible with GDI and uses Direct3D command-stream remoting if it is available.</remarks>
					property RenderTargetUsage Usage
					{
						RenderTargetUsage get();
						void set(RenderTargetUsage value);
					}
					
					/// <summary>A value that specifies the minimum Direct3D feature level required for hardware rendering.</summary>
					/// <remarks>
					///		If the specified minimum level is not available, the render target uses software rendering if the type member
					///		is set to RenderTargetType.Default; if type is set to to RenderTargetType.Hardware, render target
					///		creation fails. A value of DirectXVerions.Default indicates that Direct2D should determine whether the
					///		Direct3D feature level of the device is adequate.
					///		This field is used only when creating HwndRenderTarget and DCRenderTarget objects.
					/// </remarks>
					property DirectXVersion MinVersion
					{
						DirectXVersion get();
						void set(DirectXVersion value);
					}
				#pragma endregion


				#pragma region Construction
				public:
					/// <summary>Default Constructor</summary>
					RenderTargetProperties();
					
					/// <summary>Definition Constructor</summary>
					/// <param name="targetType">A value that specifies whether the render target should force hardware or software rendering.</param>
					/// <param name="format">The pixel format and alpha mode of the render target.</param>
					/// <param name="dpiX">The horizontal DPI of the render target.</param>
					/// <param name="dpiY">The vertical DPI of the render target.</param>
					/// <param name="usage">A value that specifies how the render target is remoted and whether it should be GDI-compatible.</param>
					/// <param name="minVersion">A value that specifies the minimum Direct3D feature level required for hardware rendering.</param>
					RenderTargetProperties(RenderTargetType targetType, PixelFormat^ format, System::Single dpiX, System::Single dpiY, RenderTargetUsage usage, DirectXVersion minVersion);
					
					/// <summary>Definition Constructor</summary>
					/// <param name="targetType">A value that specifies whether the render target should force hardware or software rendering.</param>
					/// <param name="format">The pixel format and alpha mode of the render target.</param>
					/// <param name="dpiResolution">The DPI resolution of the render target.</param>
					/// <param name="usage">A value that specifies how the render target is remoted and whether it should be GDI-compatible.</param>
					/// <param name="minVersion">A value that specifies the minimum Direct3D feature level required for hardware rendering.</param>
					RenderTargetProperties(RenderTargetType targetType, PixelFormat^ format, DpiResolution dpiResolution, RenderTargetUsage usage, DirectXVersion minVersion);

				internal:
					/// <summary>Unmanaged Constructor</summary>
					/// <param name="unmanaged">Unmanaged source structure</param>
					RenderTargetProperties(D2D1_RENDER_TARGET_PROPERTIES unmanaged);
				#pragma endregion


				#pragma region Methods
				internal:
					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct equivalent</returns>
					D2D1_RENDER_TARGET_PROPERTIES ToUnmanaged();
				#pragma endregion
				};
			}
		}
	}
}

#endif
