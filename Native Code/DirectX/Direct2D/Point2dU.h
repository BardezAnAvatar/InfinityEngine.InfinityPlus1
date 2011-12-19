
#ifndef Bardez_Projects_DirectX_Direct2D_Point2dU
#define Bardez_Projects_DirectX_Direct2D_Point2dU

#include <d2dBaseTypes.h>

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a managed equivalent for the Direct2D D2D_POINT_2U structure</summary>
				public ref class Point2dU
				{
				#pragma region Fields
				protected:
					/// <summary>Represents the X coordinate of the point in question</summary>
					System::UInt32 x;

					/// <summary>Represents the Y coordinate of the point in question</summary>
					System::UInt32 y;
				#pragma endregion

				#pragma region Properties
				public:
					/// <summary>Represents the X coordinate of the point in question</summary>
					property System::UInt32 X
					{
						System::UInt32 get();
						void set(System::UInt32 value);
					}
					
					/// <summary>Represents the Y coordinate of the point in question</summary>
					property System::UInt32 Y
					{
						System::UInt32 get();
						void set(System::UInt32 value);
					}
				#pragma endregion

				#pragma region Construction
				public:
					/// <summary>Definition constructor</summary>
					/// <param name="x">X coordinate of the point, as an integer value</param>
					/// <param name="y">Y coordinate of the point, as an integer value</param>
					Point2dU(System::UInt32 x, System::UInt32 y);

					/// <summary>Definition constructor</summary>
					/// <param name="point">System.Drawing struct containing a two dimensional point</param>
					Point2dU(System::Drawing::Point point);

				internal:
					/// <summary>Unmanaged constructor</summary>
					/// <param name="point">D2D_POINT_2U source structure</param>
					Point2dU(D2D_POINT_2U point);
				#pragma endregion

				#pragma region Methods
				internal:
					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct equivalent</returns>
					D2D_POINT_2U ToUnmanaged();
				#pragma endregion
				};
			}
		}
	}
}

#endif
