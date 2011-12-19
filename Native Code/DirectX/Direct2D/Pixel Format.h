
#ifndef Bardez_Projects_DirectX_Direct2D_PixelFormat
#define Bardez_Projects_DirectX_Direct2D_PixelFormat

#include <d2d1.h>
#include "Alpha Mode.h"
#include "DXGI Channel Format.h"

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a managed equivalent for the Direct2D D2D1_PIXEL_FORMAT structure</summary>
				public ref class PixelFormat
				{
				#pragma region Fields
				protected:
					/// <summary>Represents the pixel format of the binary data of the pixel(s)</summary>
					DXGI_ChannelFormat format;

					/// <summary>Represents the alpha transparency format of the binary data of the pixel(s)</summary>
					AlphaMode alphaType;
				#pragma endregion


				#pragma region Properties
				public:
					/// <summary>Represents the pixel format of the binary data of the pixel(s)</summary>
					property DXGI_ChannelFormat Format
					{
						DXGI_ChannelFormat get();
						void set(DXGI_ChannelFormat value);
					}

					/// <summary>Represents the alpha transparency format of the binary data of the pixel(s)</summary>
					property AlphaMode AlphaType
					{
						AlphaMode get();
						void set(AlphaMode value);
					}
				#pragma endregion


				#pragma region Construction
				public:
					/// <summary>Definition constructor</summary>
					/// <param name="format">Pixel format of the data</param>
					/// <param name="alphaMode">Alpha transparency mode of the data</param>
					PixelFormat(DXGI_ChannelFormat format, AlphaMode alphaMode);

				internal:
					/// <summary>Unmanaged constructor</summary>
					/// <param name="format">D2D1_PIXEL_FORMAT source structure</param>
					PixelFormat(D2D1_PIXEL_FORMAT format);
				#pragma endregion

				#pragma region Methods
				internal:
					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct equivalent</returns>
					D2D1_PIXEL_FORMAT ToUnmanaged();
				#pragma endregion
				};
			}
		}
	}
}

#endif
