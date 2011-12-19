

#ifndef Bardez_Projects_DirectX_Direct2D_BitmapProperties
#define Bardez_Projects_DirectX_Direct2D_BitmapProperties

#include "DPI Resolution.h"
#include "Pixel Format.h"

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
				public ref class BitmapProperties
				{
				#pragma region Fields
				protected:
					/// <summary>Repesents the binary pixel data and alpha channel formats</summary>
					PixelFormat^ pixelFormat;

					/// <summary>Represents the DPI resolution of the bitmap.</summary>
					DpiResolution dpi;
				#pragma endregion


				#pragma region Properties
				protected:
					/// <summary>Repesents the binary pixel data and alpha channel formats</summary>
					property PixelFormat^ Pixel_Format
					{
						PixelFormat^ get();
						void set (PixelFormat^ value);
					}
					
					/// <summary>Represents the DPI resolution of the bitmap.</summary>
					property DpiResolution DPI
					{
						DpiResolution get();
						void set(DpiResolution value);
					}
				#pragma endregion


				#pragma region Construction
				public:
					/// <summary>Definition constructor</summary>
					/// <param name="format">Pixel format of the data</param>
					/// <param name="dpiHorizontal">Horizontal DPI of the bitmap</param>
					/// <param name="dpiVertical">Vertical DPI of the bitmap</param>
					BitmapProperties(PixelFormat^ format, System::Single dpiHorizontal, System::Single dpiVertical);

					/// <summary>Definition constructor</summary>
					/// <param name="format">Binary pixel format</param>
					/// <param name="alphaMode">Alpha transparency mode</param>
					/// <param name="dpiHorizontal">Horizontal DPI of the bitmap</param>
					/// <param name="dpiVertical">Vertical DPI of the bitmap</param>
					BitmapProperties(DXGI_ChannelFormat format, AlphaMode alphaMode, System::Single dpiHorizontal, System::Single dpiVertical);

					/// <summary>Definition constructor</summary>
					/// <param name="format">Pixel format of the data</param>
					/// <param name="DPI">DPI of the bitmap</param>
					BitmapProperties(PixelFormat^ format, DpiResolution DPI);

					/// <summary>Definition constructor</summary>
					/// <param name="format">Binary pixel format</param>
					/// <param name="alphaMode">Alpha transparency mode</param>
					/// <param name="DPI">DPI of the bitmap</param>
					BitmapProperties(DXGI_ChannelFormat format, AlphaMode alphaMode, DpiResolution DPI);
				
				internal:
					/// <summary>Unmanaged constructor</summary>
					/// <param name="properties">Unmanaged souce structure</param>
					BitmapProperties(D2D1_BITMAP_PROPERTIES properties);
				#pragma endregion


				#pragma region Methods
				internal:
					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct equivalent</returns>
					D2D1_BITMAP_PROPERTIES ToUnmanaged();
				#pragma endregion
				};
			}
		}
	}
}

#endif

