
#ifndef Bardez_Projects_DirectX_Direct2D_SizeF
#define Bardez_Projects_DirectX_Direct2D_SizeF

#include <d2dBaseTypes.h>

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a managed equivalent for the Direct2D D2D_SIZE_F structure</summary>
				public ref class SizeF
				{
				#pragma region Fields
				protected:
					/// <summary>Represents the width of the size in question</summary>
					System::Single width;

					/// <summary>Represents the height of the size in question</summary>
					System::Single height;
				#pragma endregion

				#pragma region Properties
					/// <summary>Represents the width of the size in question</summary>
					property System::Single Width
					{
						System::Single get();
						void set(System::Single value);
					}

					/// <summary>Represents the height of the size in question</summary>
					property System::Single Height
					{
						System::Single get();
						void set(System::Single value);
					}
				#pragma endregion

				#pragma region Construction
				public:
					/// <summary>Definition constructor</summary>
					/// <param name="width">Width of the size, as a single-precision floating point value</param>
					/// <param name="height">Height of the size, as a single-precision floating point value</param>
					SizeF(System::Single width, System::Single height);

					/// <summary>Definition constructor</summary>
					/// <param name="size">System.Drawing struct containing size</param>
					SizeF(System::Drawing::SizeF size);

				internal:
					/// <summary>Unmanaged constructor</summary>
					/// <param name="size">D2D_SIZE_F source structure</param>
					SizeF(D2D_SIZE_F size);
				#pragma endregion

				#pragma region Methods
				internal:
					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct euqivalent</returns>
					D2D_SIZE_F ToUnmanaged();
				#pragma endregion
				};
			}
		}
	}
}

#endif
