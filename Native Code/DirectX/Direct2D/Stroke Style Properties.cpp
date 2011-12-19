
#include "Stroke Style Properties.h"

using namespace Bardez::Projects::DirectX::Direct2D;


#pragma region Properties
/// <summary>Exposes the cap applied to the start of all the open figures in a stroked geometry.</summary>
CapStyle StrokeStyleProperties::StartCap::get()
{
	return this->startCap;
}

/// <summary>Exposes the cap applied to the start of all the open figures in a stroked geometry.</summary>
void StrokeStyleProperties::StartCap::set(CapStyle value)
{
	this->startCap = value;
}

/// <summary>Exposes the cap applied to the end of all the open figures in a stroked geometry.</summary>
CapStyle StrokeStyleProperties::EndCap::get()
{
	return this->endCap;
}

/// <summary>Exposes the cap applied to the end of all the open figures in a stroked geometry.</summary>
void StrokeStyleProperties::EndCap::set(CapStyle value)
{
	this->endCap = value;
}

/// <summary>Exposes the shape at either end of each dash segment.</summary>
CapStyle StrokeStyleProperties::DashCap::get()
{
	return this->dashCap;
}

/// <summary>Exposes the shape at either end of each dash segment.</summary>
void StrokeStyleProperties::DashCap::set(CapStyle value)
{
	this->dashCap = value;
}

/// <summary>Exposes a value that describes how segments are joined.</summary>
/// <remarks>This value is ignored for a vertex if the segment flags specify that the segment should have a smooth join.</remarks>
Bardez::Projects::DirectX::Direct2D::LineJoint StrokeStyleProperties::LineJoint::get()
{
	return this->lineJoint;
}

/// <summary>Exposes a value that describes how segments are joined.</summary>
/// <remarks>This value is ignored for a vertex if the segment flags specify that the segment should have a smooth join.</remarks>
void StrokeStyleProperties::LineJoint::set(Bardez::Projects::DirectX::Direct2D::LineJoint value)
{
	this->lineJoint = value;
}

/// <summary>Exposes the limit of the thickness of the join on a mitered corner.</summary>
/// <remarks>This value is always treated as though it is greater than or equal to 1.0F.</remarks>
System::Single StrokeStyleProperties::MiterLimit::get()
{
	return this->miterLimit;
}

/// <summary>Exposes the limit of the thickness of the join on a mitered corner.</summary>
/// <remarks>This value is always treated as though it is greater than or equal to 1.0F.</remarks>
void StrokeStyleProperties::MiterLimit::set(System::Single value)
{
	this->miterLimit = value;
}

/// <summary>Exposes a value that specifies whether the stroke has a dash pattern and, if so, the dash style.</summary>
Bardez::Projects::DirectX::Direct2D::DashStyle StrokeStyleProperties::DashStyle::get()
{
	return this->dashStyle;
}

/// <summary>Exposes a value that specifies whether the stroke has a dash pattern and, if so, the dash style.</summary>
void StrokeStyleProperties::DashStyle::set(Bardez::Projects::DirectX::Direct2D::DashStyle value)
{
	this->dashStyle = value;
}

/// <summary>Exposes a value that specifies an offset in the dash sequence.</summary>
/// <value>
///		A positive dash offset value shifts the dash pattern, in units of stroke width, toward the start of the stroked geometry.
///		A negative dash offset value shifts the dash pattern, in units of stroke width, toward the end of the stroked geometry.
///	</value>
System::Single StrokeStyleProperties::DashOffset::get()
{
	return this->dashOffset;
}

/// <summary>Exposes a value that specifies an offset in the dash sequence.</summary>
/// <value>
///		A positive dash offset value shifts the dash pattern, in units of stroke width, toward the start of the stroked geometry.
///		A negative dash offset value shifts the dash pattern, in units of stroke width, toward the end of the stroked geometry.
///	</value>
void StrokeStyleProperties::DashOffset::set(System::Single value)
{
	this->dashOffset = value;
}
#pragma endregion


#pragma region Construction
/// <summary>Definition Constructor</summary>
/// <param name="start">The cap applied to the start of all the open figures in a stroked geometry.</param>
/// <param name="end">The cap applied to the end of all the open figures in a stroked geometry.</param>
/// <param name="dash">The shape at either end of each dash segment.</param>
/// <param name="lineJoin">A value that describes how segments are joined.</param>
/// <param name="miterLimit">The limit of the thickness of the join on a mitered corner.</param>
/// <param name="dashStyle">A value that specifies whether the stroke has a dash pattern and, if so, the dash style.</param>
/// <param name="dashOffset">A value that specifies an offset in the dash sequence.</param>
StrokeStyleProperties::StrokeStyleProperties(CapStyle start, CapStyle end, CapStyle dash, Bardez::Projects::DirectX::Direct2D::LineJoint lineJoin, System::Single miterLimit, Bardez::Projects::DirectX::Direct2D::DashStyle dashStyle, System::Single dashOffset)
{
	this->startCap = start;
	this->endCap = end;
	this->dashCap = dash;
	this->lineJoint = lineJoin;
	this->miterLimit = miterLimit;
	this->dashStyle = dashStyle;
	this->dashOffset = dashOffset;
}

/// <summary>Unmanaged Constructor</summary>
/// <param name="unmanaged">Unmanaged source structure</param>
StrokeStyleProperties::StrokeStyleProperties(D2D1_STROKE_STYLE_PROPERTIES unmanaged)
{
	this->startCap = (CapStyle)(unmanaged.startCap);
	this->endCap = (CapStyle)(unmanaged.endCap);
	this->dashCap = (CapStyle)(unmanaged.dashCap);
	this->lineJoint = (Bardez::Projects::DirectX::Direct2D::LineJoint)(unmanaged.lineJoin);
	this->miterLimit = unmanaged.miterLimit;
	this->dashStyle = (Bardez::Projects::DirectX::Direct2D::DashStyle)(unmanaged.dashStyle);
	this->dashOffset = unmanaged.dashOffset;
}
#pragma endregion


#pragma region Methods
/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
/// <returns>An unmanaged Direct2D struct equivalent</returns>
D2D1_STROKE_STYLE_PROPERTIES StrokeStyleProperties::ToUnmanaged()
{
	D2D1_STROKE_STYLE_PROPERTIES properties;

	properties.startCap = (D2D1_CAP_STYLE)(this->startCap);
	properties.endCap = (D2D1_CAP_STYLE)(this->endCap);
	properties.dashCap = (D2D1_CAP_STYLE)(this->dashCap);
	properties.lineJoin = (D2D1_LINE_JOIN)(this->lineJoint);
	properties.miterLimit = this->miterLimit;
	properties.dashStyle = (D2D1_DASH_STYLE)(this->dashStyle);
	properties.dashOffset = this->dashOffset;

	return properties;
}
#pragma endregion