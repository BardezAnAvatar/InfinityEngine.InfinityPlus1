
#ifndef Bardez_Projects_DirectX_Direct2D_Point2dF
#define Bardez_Projects_DirectX_Direct2D_Point2dF

#include <d2dBaseTypes.h>

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a managed equivalent for the Direct2D D2D_POINT_2F structure</summary>
				public ref class Point2dF
				{
				#pragma region Fields
				protected:
					/// <summary>Represents the X coordinate of the point in question</summary>
					System::Single x;

					/// <summary>Represents the Y coordinate of the point in question</summary>
					System::Single y;
				#pragma endregion


				#pragma region Properties
				public:
					/// <summary>Represents the X coordinate of the point in question</summary>
					property System::Single X
					{
						System::Single get();
						void set(System::Single value);
					}

					/// <summary>Represents the Y coordinate of the point in question</summary>
					property System::Single Y
					{
						System::Single get();
						void set(System::Single value);
					}
				#pragma endregion


				#pragma region Construction
				public:
					/// <summary>Definition constructor</summary>
					/// <param name="x">X coordinate of the point, as a single-precision floating point value</param>
					/// <param name="y">Y coordinate of the point, as a single-precision floating point value</param>
					Point2dF(System::Single x, System::Single y);

					/// <summary>Definition constructor</summary>
					/// <param name="point">System.Drawing struct containing a two dimensional point</param>
					Point2dF(System::Drawing::PointF point);

				internal:
					/// <summary>Unmanaged constructor</summary>
					/// <param name="point">D2D_POINT_2F source structure</param>
					Point2dF(D2D_POINT_2F point);
				#pragma endregion


				#pragma region Methods
				internal:
					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct equivalent</returns>
					D2D_POINT_2F ToUnmanaged();
				#pragma endregion
				};
			}
		}
	}
}

#endif
