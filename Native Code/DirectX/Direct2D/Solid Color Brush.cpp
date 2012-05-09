
#include "Solid Color Brush.h"

using namespace Bardez::Projects::DirectX::Direct2D;


#pragma region Properties
/// <summary>Exposes the native handle to the ID2D1SolidColorBrush object</summary>
ID2D1SolidColorBrush* SolidColorBrush::SolidColorBrushPtr::get()
{
	return reinterpret_cast<ID2D1SolidColorBrush*>(this->brushPtr.ToPointer());
}

/// <summary>Exposes the native handle to the ID2D1SolidColorBrush object</summary>
void SolidColorBrush::SolidColorBrushPtr::set(ID2D1SolidColorBrush* value)
{
	this->brushPtr = System::IntPtr(value);
}

/// <summary>The color of the solid color brush.</summary>
ColorF^ SolidColorBrush::Color::get()
{
	return gcnew ColorF(this->SolidColorBrushPtr->GetColor());
}

/// <summary>The color of the solid color brush.</summary>
void SolidColorBrush::Color::set(ColorF^ value)
{
	this->SolidColorBrushPtr->SetColor(value->ToUnmanaged());
}
#pragma endregion


#pragma region Construction
/// <summary>Definition constructor</summary>
/// <param name="pointer">Pointer to the Direct2D brush</param>
SolidColorBrush::SolidColorBrush(ID2D1SolidColorBrush* pointer) : Brush(pointer) { }
#pragma endregion


#pragma region Destruction
/// <summary>Destructor</summary>
/// <remarks>Dispose()</remarks>
SolidColorBrush::~SolidColorBrush()
{
	this->DisposeUnmanaged();
}

/// <summary>Destructor</summary>
/// <remarks>Finalize()</remarks>
SolidColorBrush::!SolidColorBrush()
{
	this->DisposeUnmanaged();
}

/// <summary>Destructor logic, disposes the object</summary>
void SolidColorBrush::DisposeUnmanaged()
{
	// Dispose this Brush
	if (this->SolidColorBrushPtr)
	{
		this->SolidColorBrushPtr->Release();
		this->SolidColorBrushPtr = NULL;
	}
}
#pragma endregion
