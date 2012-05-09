
#include "Radial Gradient Brush.h"

using namespace Bardez::Projects::DirectX::Direct2D;


#pragma region Properties
/// <summary>Exposes the native handle to the ID2D1BitmapBrush object</summary>
ID2D1RadialGradientBrush* RadialGradientBrush::RadialGradientBrushPtr::get()
{
	return reinterpret_cast<ID2D1RadialGradientBrush*>(this->brushPtr.ToPointer());
}

/// <summary>Exposes the native handle to the ID2D1BitmapBrush object</summary>
void RadialGradientBrush::RadialGradientBrushPtr::set(ID2D1RadialGradientBrush* value)
{
	this->brushPtr = System::IntPtr(value);
}

/// <summary>Exposes the center of the gradient ellipse.</summary>
Point2dF^ RadialGradientBrush::Center::get()
{
	return gcnew Point2dF(this->RadialGradientBrushPtr->GetCenter());
}

/// <summary>Exposes the center of the gradient ellipse.</summary>
void RadialGradientBrush::Center::set(Point2dF^ value)
{
	this->RadialGradientBrushPtr->SetCenter(value->ToUnmanaged());
}

/// <summary>Exposes the offset of the gradient origin relative to the gradient ellipse's center.</summary>
Point2dF^ RadialGradientBrush::GradientOriginOffset::get()
{
	return gcnew Point2dF(this->RadialGradientBrushPtr->GetGradientOriginOffset());
}

/// <summary>Exposes the offset of the gradient origin relative to the gradient ellipse's center.</summary>
void RadialGradientBrush::GradientOriginOffset::set(Point2dF^ value)
{
	this->RadialGradientBrushPtr->SetGradientOriginOffset(value->ToUnmanaged());
}

/// <summary>Exposes the x-radius of the gradient ellipse.</summary>
System::Single RadialGradientBrush::RadiusX::get()
{
	return this->RadialGradientBrushPtr->GetRadiusX();
}

/// <summary>Exposes the x-radius of the gradient ellipse.</summary>
void RadialGradientBrush::RadiusX::set(System::Single value)
{
	this->RadialGradientBrushPtr->SetRadiusX(value);
}

/// <summary>Exposes the y-radius of the gradient ellipse.</summary>
System::Single RadialGradientBrush::RadiusY::get()
{
	return this->RadialGradientBrushPtr->GetRadiusY();
}

/// <summary>Exposes the y-radius of the gradient ellipse.</summary>
void RadialGradientBrush::RadiusY::set(System::Single value)
{
	this->RadialGradientBrushPtr->SetRadiusY(value);
}

// <summary>Exposes the GradientStopCollection associated with this linear gradient brush.</summary>
GradientStopCollection^ RadialGradientBrush::GetGradientStops::get()
{
	ID2D1GradientStopCollection* gradients;
	this->RadialGradientBrushPtr->GetGradientStopCollection(&gradients);

	return gcnew GradientStopCollection(gradients);
}
#pragma endregion


#pragma region Construction
/// <summary>Definition constructor</summary>
/// <param name="pointer">Pointer to the Direct2D brush</param>
RadialGradientBrush::RadialGradientBrush(ID2D1RadialGradientBrush* pointer) : Brush(pointer) { }
#pragma endregion


#pragma region Destruction
/// <summary>Destructor</summary>
/// <remarks>Dispose()</remarks>
RadialGradientBrush::~RadialGradientBrush()
{
	this->DisposeUnmanaged();
}

/// <summary>Destructor</summary>
/// <remarks>Finalize()</remarks>
RadialGradientBrush::!RadialGradientBrush()
{
	this->DisposeUnmanaged();
}

/// <summary>Destructor logic, disposes the object</summary>
void RadialGradientBrush::DisposeUnmanaged()
{
	// Dispose this Brush
	if (this->RadialGradientBrushPtr)
	{
		this->RadialGradientBrushPtr->Release();
		this->RadialGradientBrushPtr = NULL;
	}
}
#pragma endregion