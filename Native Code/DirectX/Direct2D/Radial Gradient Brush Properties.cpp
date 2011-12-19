
#include "Radial Gradient Brush Properties.h"

using namespace Bardez::Projects::DirectX::Direct2D;


#pragma region Properties
/// <summary>In the brush's coordinate space, the center of the gradient ellipse.</summary>
Point2dF^ RadialGradientBrushProperties::Center::get()
{
	return this->center;
}

/// <summary>In the brush's coordinate space, the center of the gradient ellipse.</summary>
void RadialGradientBrushProperties::Center::set(Point2dF^ value)
{
	this->center = value;
}

/// <summary>In the brush's coordinate space, the offset of the gradient origin relative to the gradient ellipse's center.</summary>
Point2dF^ RadialGradientBrushProperties::GradientOriginOffset::get()
{
	return this->gradientOriginOffset;
}

/// <summary>In the brush's coordinate space, the offset of the gradient origin relative to the gradient ellipse's center.</summary>
void RadialGradientBrushProperties::GradientOriginOffset::set(Point2dF^ value)
{
	this->gradientOriginOffset = value;
}

/// <summary>In the brush's coordinate space, the x-radius of the gradient ellipse.</summary>
System::Single RadialGradientBrushProperties::RadiusX::get()
{
	return this->radiusX;
}

/// <summary>In the brush's coordinate space, the x-radius of the gradient ellipse.</summary>
void RadialGradientBrushProperties::RadiusX::set(System::Single value)
{
	this->radiusX = value;
}

/// <summary>In the brush's coordinate space, the y-radius of the gradient ellipse.</summary>
System::Single RadialGradientBrushProperties::RadiusY::get()
{
	return this->radiusY;
}

/// <summary>In the brush's coordinate space, the y-radius of the gradient ellipse.</summary>
void RadialGradientBrushProperties::RadiusY::set(System::Single value)
{
	this->radiusY = value;
}
#pragma endregion


#pragma region Construction
/// <summary>Definition Constructor</summary>
/// <param name="center">In the brush's coordinate space, the center of the gradient ellipse.</param>
/// <param name="origin">In the brush's coordinate space, the offset of the gradient origin relative to the gradient ellipse's center.</param>
/// <param name="radiusX">In the brush's coordinate space, the x-radius of the gradient ellipse.</param>
/// <param name="radiusY">In the brush's coordinate space, the y-radius of the gradient ellipse.</param>
RadialGradientBrushProperties::RadialGradientBrushProperties(Point2dF^ center, Point2dF^ origin, System::Single radiusX, System::Single radiusY)
{
	this->center = center;
	this->gradientOriginOffset = origin;
	this->radiusX = radiusX;
	this->radiusY = radiusY;
}

/// <summary>Unmanaged Constructor</summary>
/// <param name="unmanaged">Unmanaged source structure</param>
RadialGradientBrushProperties::RadialGradientBrushProperties(D2D1_RADIAL_GRADIENT_BRUSH_PROPERTIES unmanaged)
{
	this->center = gcnew Point2dF(unmanaged.center);
	this->gradientOriginOffset = gcnew Point2dF(unmanaged.gradientOriginOffset);
	this->radiusX = unmanaged.radiusX;
	this->radiusY = unmanaged.radiusY;
}
#pragma endregion


#pragma region Methods
/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
/// <returns>An unmanaged Direct2D struct equivalent</returns>
D2D1_RADIAL_GRADIENT_BRUSH_PROPERTIES RadialGradientBrushProperties::ToUnmanaged()
{
	D2D1_RADIAL_GRADIENT_BRUSH_PROPERTIES properties;
	
	properties.center = this->center->ToUnmanaged();
	properties.gradientOriginOffset = this->gradientOriginOffset->ToUnmanaged();
	properties.radiusX = this->radiusX;
	properties.radiusY = this->radiusY;

	return properties;
}
#pragma endregion