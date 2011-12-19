
#include "Linear Gradient Brush Properties.h"

using namespace Bardez::Projects::DirectX::Direct2D;

	
#pragma region Properties
/// <summary>In the brush's coordinate space, the starting point of the gradient axis.</summary>
Point2dF^ LinearGradientBrushProperties::StartPoint::get()
{
	return this->startPoint;
}

/// <summary>In the brush's coordinate space, the starting point of the gradient axis.</summary>
void LinearGradientBrushProperties::StartPoint::set(Point2dF^ value)
{
	this->startPoint = value;
}

/// <summary>In the brush's coordinate space, the endpoint of the gradient axis.</summary>
Point2dF^ LinearGradientBrushProperties::EndPoint::get()
{
	return this->endPoint;
}

/// <summary>In the brush's coordinate space, the endpoint of the gradient axis.</summary>
void LinearGradientBrushProperties::EndPoint::set(Point2dF^ value)
{
	this->endPoint = value;
}
#pragma endregion


#pragma region Construction
/// <summary>Definition Constructor</summary>
/// <param name="start">The start point, in the brush's coordinate space, of the gradient axis.</param>
/// <param name="end">The end point, in the brush's coordinate space, of the gradient axis.</param>
LinearGradientBrushProperties::LinearGradientBrushProperties(Point2dF^ start, Point2dF^ end)
{
	this->startPoint = start;
	this->endPoint = end;
}

/// <summary>Unmanaged Constructor</summary>
/// <param name="unmanaged">Unmanaged source structure</param>
LinearGradientBrushProperties::LinearGradientBrushProperties(D2D1_LINEAR_GRADIENT_BRUSH_PROPERTIES unmanaged)
{
	this->startPoint = gcnew Point2dF(unmanaged.startPoint);
	this->endPoint = gcnew Point2dF(unmanaged.endPoint);
}
#pragma endregion


#pragma region Methods
/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
/// <returns>An unmanaged Direct2D struct equivalent</returns>
D2D1_LINEAR_GRADIENT_BRUSH_PROPERTIES LinearGradientBrushProperties::ToUnmanaged()
{
	D2D1_LINEAR_GRADIENT_BRUSH_PROPERTIES properties;
	
	properties.startPoint = this->startPoint->ToUnmanaged();
	properties.endPoint = this->endPoint->ToUnmanaged();

	return properties;
}
#pragma endregion
