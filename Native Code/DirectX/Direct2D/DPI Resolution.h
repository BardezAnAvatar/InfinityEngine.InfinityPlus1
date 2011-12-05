
#ifndef Bardez_Projects_DirectX_Direct2D_DPIResolution
#define Bardez_Projects_DirectX_Direct2D_DPIResolution

#include <d2dBaseTypes.h>

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents the resolution ratio of device-independent pixels per physical inch</summary>
				public value struct DpiResolution
				{
				#pragma region Fields
				private:
					/// <summary>Horizontal DPI</summary>
					System::Single x;
					
					/// <summary>Vertical DPI</summary>
					System::Single y;
				#pragma endregion


				#pragma region Properties
				public:
					/// <summary>Horizontal DPI</summary>
					property System::Single X
					{
						System::Single get();
						void set(System::Single value);
					}
					
					/// <summary>Vertical DPI</summary>
					property System::Single Y
					{
						System::Single get();
						void set(System::Single value);
					}
				#pragma endregion


				#pragma region Contruction
				public:
					/// <summary>Definition constructor</summary>
					/// <param name="dpiX">Horizontal DPI</param>
					/// <param name="dpiY">Vertical DPI</param>
					DpiResolution(System::Single dpiX, System::Single dpiY);
				#pragma endregion
				};
			}
		}
	}
}

#endif

