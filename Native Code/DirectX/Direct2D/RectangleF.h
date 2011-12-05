
#ifndef Bardez_Projects_DirectX_Direct2D_RectangleF
#define Bardez_Projects_DirectX_Direct2D_RectangleF

#include <d2dBaseTypes.h>

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a managed equivalent for the Direct2D D2D_RECT_F structure</summary>
				public ref class RectangleF
				{
				#pragma region Fields
				protected:
					/// <summary>Represents the left X coordinate of the rectangle in question</summary>
					System::Single left;

					/// <summary>Represents the right X coordinate of the rectangle in question</summary>
					System::Single right;

					/// <summary>Represents the top Y coordinate of the rectangle in question</summary>
					System::Single top;

					/// <summary>Represents the bottom Y coordinate of the rectangle in question</summary>
					System::Single bottom;
				#pragma endregion

				#pragma region Properties
					/// <summary>Represents the left X coordinate of the rectangle in question</summary>
					property System::Single Left
					{
						System::Single get();
						void set(System::Single value);
					}

					/// <summary>Represents the right X coordinate of the rectangle in question</summary>
					property System::Single Right
					{
						System::Single get();
						void set(System::Single value);
					}

					/// <summary>Represents the top Y coordinate of the rectangle in question</summary>
					property System::Single Top
					{
						System::Single get();
						void set(System::Single value);
					}

					/// <summary>Represents the bottom Y coordinate of the rectangle in question</summary>
					property System::Single Bottom
					{
						System::Single get();
						void set(System::Single value);
					}
				#pragma endregion

				#pragma region Construction
				public:
					/// <summary>Definition constructor</summary>
					/// <param name="left">left X coordinate of the rectangle, as a single-precision floating point value</param>
					/// <param name="right">right X coordinate of the rectangle, as a single-precision floating point value</param>
					/// <param name="top">top Y coordinate of the rectangle, as a single-precision floating point value</param>
					/// <param name="bottom">bottom Y coordinate of the rectangle, as a single-precision floating point value</param>
					RectangleF(System::Single left, System::Single right, System::Single top, System::Single bottom);

					/// <summary>Definition constructor</summary>
					/// <param name="rectangle">System.Drawing struct containing a rectangle</param>
					RectangleF(System::Drawing::RectangleF rectangle);

				internal:
					/// <summary>Unmanaged constructor</summary>
					/// <param name="point">D2D_RECT_F source structure</param>
					RectangleF(D2D_RECT_F rectangle);
				#pragma endregion

				#pragma region Methods
				internal:
					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct euqivalent</returns>
					D2D_RECT_F ToUnmanaged();
				#pragma endregion
				};
			}
		}
	}
}

#endif
