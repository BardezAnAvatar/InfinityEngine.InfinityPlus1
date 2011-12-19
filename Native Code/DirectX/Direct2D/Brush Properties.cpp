
#include "Brush Properties.h"

using namespace Bardez::Projects::DirectX::Direct2D;


#pragma region Properties
/// <summary>A value between 0.0f and 1.0f, inclusive, that specifies the degree of opacity of the brush.</summary>
System::Single BrushProperties::Opacity::get()
{
	return this->opacity;
}

/// <summary>A value between 0.0f and 1.0f, inclusive, that specifies the degree of opacity of the brush.</summary>
void BrushProperties::Opacity::set(System::Single value)
{
	this->opacity = value;
}

/// <summary>The transformation that is applied to the brush.</summary>
Matrix3x2F^ BrushProperties::Transform::get()
{
	return this->transform;
}

/// <summary>The transformation that is applied to the brush.</summary>
void BrushProperties::Transform::set(Matrix3x2F^ value)
{
	this->transform = value;
}
#pragma endregion


#pragma region Construction
/// <summary>Default Constructor</summary>
BrushProperties::BrushProperties()
{
	this->opacity = 1.0F;
	this->transform = Matrix3x2F::Identity();
}

/// <summary>Definition Constructor</summary>
/// <param name="opacity">The base opacity of the brush.</param>
/// <param name="transform">The transformation to apply to the brush.</param>
BrushProperties::BrushProperties(System::Single opacity, Matrix3x2F^ transform)
{
	this->opacity = opacity;
	this->transform = transform;
}

/// <summary>Definition Constructor</summary>
/// <param name="opacity">The base opacity of the brush.</param>
BrushProperties::BrushProperties(System::Single opacity)
{
	this->opacity = opacity;
	this->transform = Matrix3x2F::Identity();
}

/// <summary>Unmanaged Constructor</summary>
/// <param name="unmanaged">Unmanaged source structure</param>
BrushProperties::BrushProperties(D2D1_BRUSH_PROPERTIES unmanaged)
{
	this->opacity = unmanaged.opacity;
	this->transform = gcnew Matrix3x2F(unmanaged.transform);
}
#pragma endregion


#pragma region Methods
/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
/// <returns>An unmanaged Direct2D struct equivalent</returns>
D2D1_BRUSH_PROPERTIES BrushProperties::ToUnmanaged()
{
	D2D1_BRUSH_PROPERTIES properties;
	
	properties.opacity = this->opacity;
	properties.transform = this->transform->ToUnmanaged();

	return properties;
}
#pragma endregion

