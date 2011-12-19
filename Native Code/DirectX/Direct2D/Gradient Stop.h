
#ifndef Bardez_Projects_DirectX_Direct2D_GradientStop
#define Bardez_Projects_DirectX_Direct2D_GradientStop

#include <D2d1.h>
#include "ColorF.h"

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Contains the position and color of a gradient stop.</summary>
				public ref class GradientStop
				{
				#pragma region Fields
				protected:
					/// <summary>
					///		A value that indicates the relative position of the gradient stop in the brush.
					///		This value must be in the [0.0f, 1.0f] range if the gradient stop is to be seen explicitly.
					///	</summary>
					System::Single position;

					/// <summary>The color of the gradient stop.</summary>
					ColorF^ color;
				#pragma endregion


				#pragma region Properties
				public:
					/// <summary>A value that indicates the relative position of the gradient stop in the brush.</summary>
					/// <values>This value must be in the [0.0f, 1.0f] range if the gradient stop is to be seen explicitly.</values>
					virtual property System::Single Position
					{
						System::Single get();
						void set(System::Single value);
					}

					/// <summary>The color of the gradient stop.</summary>
					virtual property ColorF^ Color
					{
						ColorF^ get();
						void set(ColorF^ value);
					}
				#pragma endregion


				#pragma region Construction
				public:
					/// <summary>Definition constructor</summary>
					/// <param name="pos">Indicates the relative position of the gradient stop in the brush.</param>
					/// <param name="color">The color of the gradient stop.</param>
					GradientStop(System::Single pos, ColorF^ color);

				internal:
					/// <summary>Unmanaged constructor</summary>
					/// <param name="gradient">D2D1_GRADIENT_STOP source structure</param>
					GradientStop(D2D1_GRADIENT_STOP gradient);
				#pragma endregion


				#pragma region Methods
				internal:
					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct equivalent</returns>
					D2D1_GRADIENT_STOP ToUnmanaged();
				#pragma endregion
				};
			}
		}
	}
}

#endif
