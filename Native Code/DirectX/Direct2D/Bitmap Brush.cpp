
#include "Bitmap Brush.h"

using namespace Bardez::Projects::DirectX::Direct2D;


#pragma region Properties
/// <summary>Exposes the native handle to the ID2D1BitmapBrush object</summary>
ID2D1BitmapBrush* BitmapBrush::BitmapBrushPtr::get()
{
	return reinterpret_cast<ID2D1BitmapBrush*>(this->brushPtr.ToPointer());
}

/// <summary>Exposes the native handle to the ID2D1BitmapBrush object</summary>
void BitmapBrush::BitmapBrushPtr::set(ID2D1BitmapBrush* value)
{
	this->brushPtr = System::IntPtr(value);
}

/// <summary>Exposes the Bitmap reference being used for this Brush</summary>
Bardez::Projects::DirectX::Direct2D::Bitmap^ BitmapBrush::Bitmap::get()
{
	return this->bitmap;
}

/// <summary>Exposes the Bitmap reference being used for this Brush</summary>
void BitmapBrush::Bitmap::set(Bardez::Projects::DirectX::Direct2D::Bitmap^ value)
{
	this->bitmap = value;
}

/// <summary>Exposes the method by which the brush horizontally tiles those areas that extend past its bitmap.</summary>
BrushExtendMode BitmapBrush::ExtendModeX::get()
{
	return (BrushExtendMode)this->BitmapBrushPtr->GetExtendModeX();
}

/// <summary>Exposes the method by which the brush horizontally tiles those areas that extend past its bitmap.</summary>
void BitmapBrush::ExtendModeX::set(BrushExtendMode value)
{
	this->BitmapBrushPtr->SetExtendModeX((D2D1_EXTEND_MODE)value);
}

/// <summary>Exposes the method by which the brush vertically tiles those areas that extend past its bitmap.</summary>
BrushExtendMode BitmapBrush::ExtendModeY::get()
{
	return (BrushExtendMode)this->BitmapBrushPtr->GetExtendModeY();
}

/// <summary>Exposes the method by which the brush vertically tiles those areas that extend past its bitmap.</summary>
void BitmapBrush::ExtendModeY::set(BrushExtendMode value)
{
	this->BitmapBrushPtr->SetExtendModeY((D2D1_EXTEND_MODE)value);
}

/// <summary>Exposes the interpolation method used when the brush bitmap is scaled or rotated.</summary>
BitmapInterpolationMode BitmapBrush::InterpolationMode::get()
{
	return (BitmapInterpolationMode)this->BitmapBrushPtr->GetInterpolationMode();
}

/// <summary>Exposes the interpolation method used when the brush bitmap is scaled or rotated.</summary>
void BitmapBrush::InterpolationMode::set(BitmapInterpolationMode value)
{
	this->BitmapBrushPtr->SetInterpolationMode((D2D1_BITMAP_INTERPOLATION_MODE)value);
}
#pragma endregion


#pragma region Construction
/// <summary>Definition constructor</summary>
/// <param name="pointer">Pointer to the Direct2D brush</param>
BitmapBrush::BitmapBrush(ID2D1BitmapBrush* pointer) : Brush(pointer) { }
#pragma endregion


#pragma region Destruction
/// <summary>Destrutor</summary>
/// <remarks>Dispose()</remarks>
BitmapBrush::~BitmapBrush()
{
	this->DisposeUnmanaged();
}

/// <summary>Destrutor</summary>
/// <remarks>Finalize()</remarks>
BitmapBrush::!BitmapBrush()
{
	this->DisposeUnmanaged();
}

/// <summary>Destrutor logic, disposes the object</summary>
void BitmapBrush::DisposeUnmanaged()
{
	// Dispose this Brush
	if (this->BitmapBrushPtr)
	{
		this->BitmapBrushPtr->Release();
		this->BitmapBrushPtr = NULL;
	}
}
#pragma endregion