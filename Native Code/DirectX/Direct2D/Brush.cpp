

#include "Brush.h"

using namespace Bardez::Projects::DirectX::Direct2D;

#pragma region Properties
/// <summary>Exposes the native handle to the ID2D1Brush object</summary>
ID2D1Brush* Brush::BrushPtr::get()
{
	return reinterpret_cast<ID2D1Brush*>(this->brushPtr.ToPointer());
}

/// <summary>Exposes the native handle to the ID2D1Brush object</summary>
void Brush::BrushPtr::set(ID2D1Brush* value)
{
	this->brushPtr = System::IntPtr(value);
}

/// <summary>Exposes the opacity of this brush.</summary>
System::Single Brush::Opacity::get()
{
	return this->GetOpacity();
}

/// <summary>Exposes the opacity of this brush.</summary>
void Brush::Opacity::set(System::Single value)
{
	this->SetOpacity(value);
}

/// <summary>Exposes the transformation applied to the brush.</summary>
Matrix3x2F^ Brush::Transformation::get()
{
	return this->GetTransform();
}

/// <summary>Exposes the transformation applied to the brush.</summary>
void Brush::Transformation::set(Matrix3x2F^ value)
{
	this->SetTransform(value);
}
#pragma endregion


#pragma region Construction
/// <summary>Definition constructor</summary>
/// <param name="pointer">Pointer to the Direct2D brush</param>
Brush::Brush(ID2D1Brush* pointer)
{
	this->BrushPtr = pointer;
}
#pragma endregion


#pragma region Destruction
/// <summary>Destructor</summary>
/// <remarks>Dispose()</remarks>
Brush::~Brush()
{
	this->DisposeUnmanaged();
}

/// <summary>Destructor</summary>
/// <remarks>Finalize()</remarks>
Brush::!Brush()
{
	this->DisposeUnmanaged();
}

/// <summary>Destructor logic, disposes the object</summary>
void Brush::DisposeUnmanaged()
{
	// Dispose this Brush
	if (this->BrushPtr)
	{
		this->BrushPtr->Release();
		this->BrushPtr = NULL;
	}
}
#pragma endregion


#pragma region Methods
/// <summary>Gets the degree of opacity of this brush.</summary>
/// <returns>
///		A value between zero and 1 that indicates the opacity of the brush.
///		This value is a constant multiplier that linearly scales the alpha value of all pixels filled by the brush.
///		The opacity values are clamped in the range 0–1 before they are multipled together.
/// </returns>
System::Single Brush::GetOpacity()
{
	return this->BrushPtr->GetOpacity();
}

/// <summary>Gets the transform applied to this brush.</summary>
/// <returns>The transform applied to this brush.</returns>
Matrix3x2F^ Brush::GetTransform()
{
	D2D1_MATRIX_3X2_F transform;
	this->BrushPtr->GetTransform(&transform);

	return gcnew Matrix3x2F(transform);
}

/// <summary>Sets the degree of opacity of this brush.</summary>
/// <param name="opacity">
///		A value between zero and 1 that indicates the opacity of the brush.
///		This value is a constant multiplier that linearly scales the alpha value of all pixels filled by the brush.
///		The opacity values are clamped in the range 0–1 before they are multipled together. 
/// </param>
void Brush::SetOpacity(System::Single opacity)
{
	this->BrushPtr->SetOpacity(opacity);
}

/// <summary>Sets the transformation applied to the brush.</summary>
/// <param name="transform">The transformation to apply to this brush. </param>
void Brush::SetTransform(Matrix3x2F^ transform)
{
	this->BrushPtr->SetTransform(&(transform->ToUnmanaged()));
}
#pragma endregion
