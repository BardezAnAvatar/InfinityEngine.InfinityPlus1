
#ifndef Bardez_Projects_DirectX_Direct2D_ColorF
#define Bardez_Projects_DirectX_Direct2D_ColorF

#include <D2d1.h>

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a managed equivalent for the Direct2D D2D1_COLOR_F structure</summary>
				public ref class ColorF
				{
				#pragma region Fields
				protected:
					/// <summary>Represents the red color composition of the current color</summary>
					/// <value>Ranges from 0.0F to 1.0F, inclusive</value>
					System::Single red;

					/// <summary>Represents the green color composition of the current color</summary>
					/// <value>Ranges from 0.0F to 1.0F, inclusive</value>
					System::Single green;

					/// <summary>Represents the blue color composition of the current color</summary>
					/// <value>Ranges from 0.0F to 1.0F, inclusive</value>
					System::Single blue;

					/// <summary>Represents the alpha transparency composition of the current color</summary>
					/// <value>Ranges from 0.0F to 1.0F, inclusive</value>
					System::Single alpha;
				#pragma endregion


				#pragma region Properties
				public:
					/// <summary>Represents the red color composition of the current color</summary>
					/// <value>Ranges from 0.0F to 1.0F, inclusive</value>
					property System::Single Red
					{
						System::Single get();
						void set(System::Single value);
					}

					/// <summary>Represents the green color composition of the current color</summary>
					/// <value>Ranges from 0.0F to 1.0F, inclusive</value>
					property System::Single Green
					{
						System::Single get();
						void set(System::Single value);
					}

					/// <summary>Represents the blue color composition of the current color</summary>
					/// <value>Ranges from 0.0F to 1.0F, inclusive</value>
					property System::Single Blue
					{
						System::Single get();
						void set(System::Single value);
					}

					/// <summary>Represents the alpha transparency composition of the current color</summary>
					/// <value>Ranges from 0.0F to 1.0F, inclusive</value>
					property System::Single Alpha
					{
						System::Single get();
						void set(System::Single value);
					}
				#pragma endregion


				#pragma region Construction
				public:
					/// <summary>Definition constructor</summary>
					/// <param name="r">Red component to the color, as a single-precision floating point value</param>
					/// <param name="g">Green component to the color, as a single-precision floating point value</param>
					/// <param name="b">Blue component to the color, as a single-precision floating point value</param>
					/// <param name="a">Alpha transparency component to the color, as a single-precision floating point value</param>
					ColorF(System::Single r, System::Single g, System::Single b, System::Single a);

					/// <summary>Definition constructor</summary>
					/// <param name="r">Red component to the color, as a single-precision floating point value</param>
					/// <param name="g">Green component to the color, as a single-precision floating point value</param>
					/// <param name="b">Blue component to the color, as a single-precision floating point value</param>
					ColorF(System::Single r, System::Single g, System::Single b);

					/// <summary>Definition constructor</summary>
					/// <param name="r">Red component to the color, as an integer value</param>
					/// <param name="g">Green component to the color, as an integer value</param>
					/// <param name="b">Blue component to the color, as an integer value</param>
					/// <param name="a">Alpha transparency component to the color, as an integer value</param>
					ColorF(System::Byte r, System::Byte g, System::Byte b, System::Byte a);

					/// <summary>Definition constructor</summary>
					/// <param name="r">Red component to the color, as an integer value</param>
					/// <param name="g">Green component to the color, as an integer value</param>
					/// <param name="b">Blue component to the color, as an integer value</param>
					ColorF(System::Byte r, System::Byte g, System::Byte b);

					/// <summary>Definition constructor</summary>
					/// <param name="r">Red component to the color, as an integer value</param>
					/// <param name="g">Green component to the color, as an integer value</param>
					/// <param name="b">Blue component to the color, as an integer value</param>
					/// <param name="a">Alpha transparency component to the color, as an integer value</param>
					ColorF(System::Int32 r, System::Int32 g, System::Int32 b, System::Int32 a);

					/// <summary>Definition constructor</summary>
					/// <param name="r">Red component to the color, as an integer value</param>
					/// <param name="g">Green component to the color, as an integer value</param>
					/// <param name="b">Blue component to the color, as an integer value</param>
					ColorF(System::Int32 r, System::Int32 g, System::Int32 b);

					/// <summary>Definition constructor</summary>
					/// <param name="rgba">Entire color, as a single integer value</param>
					ColorF(System::UInt32 rgba);

					/// <summary>Definition constructor</summary>
					/// <param name="color">System.Drawing struct containing an integer color</param>
					ColorF(System::Drawing::Color color);

				internal:
					/// <summary>Unmanaged constructor</summary>
					/// <param name="color">D2D1_COLOR_F source structure</param>
					ColorF(D2D1_COLOR_F color);
				#pragma endregion

				#pragma region Methods
				internal:
					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct equivalent</returns>
					D2D1_COLOR_F ToUnmanaged();
				
				public:
					/// <summary>Converts the managed class reference to a managed equivalent .NET framework <see cref="System::Drawing::Color" /> structure</summary>
					/// <returns>A managed .NET framework struct equivalent</returns>
					System::Drawing::Color ToFramework();
				#pragma endregion
				};
			}
		}
	}
}

#endif
