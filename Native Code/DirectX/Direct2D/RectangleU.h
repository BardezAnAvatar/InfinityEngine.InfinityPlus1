
#ifndef Bardez_Projects_DirectX_Direct2D_RectangleU
#define Bardez_Projects_DirectX_Direct2D_RectangleU

#include <d2dBaseTypes.h>

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a managed equivalent for the Direct2D D2D_RECT_U structure</summary>
				public ref class RectangleU
				{
				#pragma region Fields
				protected:
					/// <summary>Represents the left X coordinate of the rectangle in question</summary>
					System::UInt32 left;

					/// <summary>Represents the right X coordinate of the rectangle in question</summary>
					System::UInt32 right;

					/// <summary>Represents the top Y coordinate of the rectangle in question</summary>
					System::UInt32 top;

					/// <summary>Represents the bottom Y coordinate of the rectangle in question</summary>
					System::UInt32 bottom;
				#pragma endregion

				#pragma region Properties
					/// <summary>Represents the left X coordinate of the rectangle in question</summary>
					property System::UInt32 Left
					{
						System::UInt32 get();
						void set(System::UInt32 value);
					}

					/// <summary>Represents the right X coordinate of the rectangle in question</summary>
					property System::UInt32 Right
					{
						System::UInt32 get();
						void set(System::UInt32 value);
					}

					/// <summary>Represents the top Y coordinate of the rectangle in question</summary>
					property System::UInt32 Top
					{
						System::UInt32 get();
						void set(System::UInt32 value);
					}

					/// <summary>Represents the bottom Y coordinate of the rectangle in question</summary>
					property System::UInt32 Bottom
					{
						System::UInt32 get();
						void set(System::UInt32 value);
					}
				#pragma endregion

				#pragma region Construction
				public:
					/// <summary>Definition constructor</summary>
					/// <param name="left">left X coordinate of the rectangle, as a single-precision floating point value</param>
					/// <param name="right">right X coordinate of the rectangle, as a single-precision floating point value</param>
					/// <param name="top">top Y coordinate of the rectangle, as a single-precision floating point value</param>
					/// <param name="bottom">bottom Y coordinate of the rectangle, as a single-precision floating point value</param>
					RectangleU(System::UInt32 left, System::UInt32 right, System::UInt32 top, System::UInt32 bottom);

					/// <summary>Definition constructor</summary>
					/// <param name="rectangle">System.Drawing struct containing a rectangle</param>
					RectangleU(System::Drawing::Rectangle rectangle);

				internal:
					/// <summary>Unmanaged constructor</summary>
					/// <param name="point">D2D_RECT_U source structure</param>
					RectangleU(D2D_RECT_U rectangle);
				#pragma endregion

				#pragma region Methods
				internal:
					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct euqivalent</returns>
					D2D_RECT_U ToUnmanaged();
				#pragma endregion
				};
			}
		}
	}
}

#endif
