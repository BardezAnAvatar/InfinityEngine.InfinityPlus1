
#include "Gradient Stop.h"

using namespace Bardez::Projects::DirectX::Direct2D;


#pragma region Properties
/// <summary>A value that indicates the relative position of the gradient stop in the brush.</summary>
/// <values>This value must be in the [0.0f, 1.0f] range if the gradient stop is to be seen explicitly.</values>
System::Single GradientStop::Position::get()
{
	return this->position;
}

/// <summary>A value that indicates the relative position of the gradient stop in the brush.</summary>
/// <values>This value must be in the [0.0f, 1.0f] range if the gradient stop is to be seen explicitly.</values>
void GradientStop::Position::set(System::Single value)
{
	this->position = value;
}

/// <summary>The color of the gradient stop.</summary>
ColorF^ GradientStop::Color::get()
{
	return this->color;
}

/// <summary>The color of the gradient stop.</summary>
void GradientStop::Color::set(ColorF^ value)
{
	this->color = value;
}
#pragma endregion


#pragma region Construction
/// <summary>Definition constructor</summary>
/// <param name="pos">Indicates the relative position of the gradient stop in the brush.</param>
/// <param name="color">The color of the gradient stop.</param>
GradientStop::GradientStop(System::Single pos, ColorF^ color)
{
	this->position = pos;
	this->color = color;
}

/// <summary>Unmanaged constructor</summary>
/// <param name="gradient">D2D1_GRADIENT_STOP source structure</param>
GradientStop::GradientStop(D2D1_GRADIENT_STOP gradient)
{
	this->position = gradient.position;
	this->color = gcnew ColorF(gradient.color);
}
#pragma endregion


#pragma region Methods
/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
/// <returns>An unmanaged Direct2D struct equivalent</returns>
D2D1_GRADIENT_STOP GradientStop::ToUnmanaged()
{
	D2D1_GRADIENT_STOP gradient;

	gradient.position = this->position;
	gradient.color = this->color->ToUnmanaged();

	return gradient;
}
#pragma endregion