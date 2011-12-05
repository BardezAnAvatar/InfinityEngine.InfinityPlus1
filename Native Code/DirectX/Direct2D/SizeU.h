
#ifndef Bardez_Projects_DirectX_Direct2D_SizeU
#define Bardez_Projects_DirectX_Direct2D_SizeU

#include <d2dBaseTypes.h>

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a managed equivalent for the Direct2D D2D_SIZE_U structure</summary>
				public ref class SizeU
				{
				#pragma region Fields
				protected:
					/// <summary>Represents the width of the size in question</summary>
					System::UInt32 width;

					/// <summary>Represents the height of the size in question</summary>
					System::UInt32 height;
				#pragma endregion

				#pragma region Properties
					/// <summary>Represents the width of the size in question</summary>
					property System::UInt32 Width
					{
						System::UInt32 get();
						void set(System::UInt32 value);
					}

					/// <summary>Represents the height of the size in question</summary>
					property System::UInt32 Height
					{
						System::UInt32 get();
						void set(System::UInt32 value);
					}
				#pragma endregion

				#pragma region Construction
				public:
					/// <summary>Definition constructor</summary>
					/// <param name="width">Width of the size, as an integer</param>
					/// <param name="height">Height of the size, as an integer</param>
					SizeU(System::UInt32 width, System::UInt32 height);

					/// <summary>Definition constructor</summary>
					/// <param name="size">System.Drawing struct containing size</param>
					SizeU(System::Drawing::Size size);

				internal:
					/// <summary>Unmanaged constructor</summary>
					/// <param name="size">D2D_SIZE_U source structure</param>
					SizeU(D2D_SIZE_U size);
				#pragma endregion

				#pragma region Methods
				internal:
					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct euqivalent</returns>
					D2D_SIZE_U ToUnmanaged();
				#pragma endregion
				};
			}
		}
	}
}

#endif
