

#include "Linear Gradient Brush.h"

using namespace Bardez::Projects::DirectX::Direct2D;


#pragma region Properties
/// <summary>Exposes the native handle to the ID2D1BitmapBrush object</summary>
ID2D1LinearGradientBrush* LinearGradientBrush::LinearGradientBrushPtr::get()
{
	return reinterpret_cast<ID2D1LinearGradientBrush*>(this->brushPtr.ToPointer());
}

/// <summary>Exposes the native handle to the ID2D1BitmapBrush object</summary>
void LinearGradientBrush::LinearGradientBrushPtr::set(ID2D1LinearGradientBrush* value)
{
	this->brushPtr = System::IntPtr(value);
}

/// <summary>Exposes the End point co-ordinate of the linear gradient</summary>
Point2dF^ LinearGradientBrush::EndPoint::get()
{
	return gcnew Point2dF(this->LinearGradientBrushPtr->GetEndPoint());
}

/// <summary>Exposes the End point co-ordinate of the linear gradient</summary>
void LinearGradientBrush::EndPoint::set(Point2dF^ value)
{
	this->LinearGradientBrushPtr->SetEndPoint(value->ToUnmanaged());
}

/// <summary>Exposes the Start point co-ordinate of the linear gradient</summary>
Point2dF^ LinearGradientBrush::StartPoint::get()
{
	return gcnew Point2dF(this->LinearGradientBrushPtr->GetStartPoint());
}

/// <summary>Exposes the Start point co-ordinate of the linear gradient</summary>
void LinearGradientBrush::StartPoint::set(Point2dF^ value)
{
	this->LinearGradientBrushPtr->SetStartPoint(value->ToUnmanaged());
}

// <summary>Exposes the GradientStopCollection associated with this linear gradient brush.</summary>
GradientStopCollection^ LinearGradientBrush::GetGradientStops::get()
{
	ID2D1GradientStopCollection* gradients;
	this->LinearGradientBrushPtr->GetGradientStopCollection(&gradients);

	return gcnew GradientStopCollection(gradients);
}
#pragma endregion


#pragma region Construction
/// <summary>Definition constructor</summary>
/// <param name="pointer">Pointer to the Direct2D brush</param>
LinearGradientBrush::LinearGradientBrush(ID2D1LinearGradientBrush* pointer) : Brush(pointer) { }
#pragma endregion


#pragma region Destruction
/// <summary>Destructor</summary>
/// <remarks>Dispose()</remarks>
LinearGradientBrush::~LinearGradientBrush()
{
	this->DisposeUnmanaged();
}

/// <summary>Destructor</summary>
/// <remarks>Finalize()</remarks>
LinearGradientBrush::!LinearGradientBrush()
{
	this->DisposeUnmanaged();
}

/// <summary>Destructor logic, disposes the object</summary>
void LinearGradientBrush::DisposeUnmanaged()
{
	// Dispose this Brush
	if (this->LinearGradientBrushPtr)
	{
		this->LinearGradientBrushPtr->Release();
		this->LinearGradientBrushPtr = NULL;
	}
}
#pragma endregion
