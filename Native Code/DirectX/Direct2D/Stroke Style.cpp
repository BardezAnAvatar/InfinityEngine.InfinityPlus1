
#include "Stroke Style.h"

using namespace Bardez::Projects::DirectX::Direct2D;

#pragma region Properties
/// <summary>Exposes a pointer to a ID2D1StrokeStyle native interface</summary>
ID2D1StrokeStyle* StrokeStyle::StrokeStylePtr::get()
{
	return reinterpret_cast<ID2D1StrokeStyle*>(this->strokeStylePtr.ToPointer());
}

/// <summary>Exposes a pointer to a ID2D1StrokeStyle native interface</summary>
void StrokeStyle::StrokeStylePtr::set(ID2D1StrokeStyle* value)
{
	this->strokeStylePtr = System::IntPtr(value);
}

/// <summary>Exposes a value that specifies how the ends of each dash are drawn.</summary>
CapStyle StrokeStyle::DashCap::get()
{
	return (CapStyle)(this->StrokeStylePtr->GetDashCap());
}

/// <summary>Exposes the type of shape used at the end of a stroke.</summary>
CapStyle StrokeStyle::EndCap::get()
{
	return (CapStyle)(this->StrokeStylePtr->GetEndCap());
}

/// <summary>Exposes the type of shape used at the beginning of a stroke.</summary>
CapStyle StrokeStyle::StartCap::get()
{
	return (CapStyle)(this->StrokeStylePtr->GetStartCap());
}

/// <summary>Exposes the number of entries in the dashes array.</summary>
System::UInt32 StrokeStyle::DashesCount::get()
{
	return this->StrokeStylePtr->GetDashesCount();
}

/// <summary>Exposes a value that specifies how far in the dash sequence the stroke will start.</summary>
System::Single StrokeStyle::DashOffset::get()
{
	return this->StrokeStylePtr->GetDashOffset();
}

/// <summary>Exposes a value that describes the stroke's dash pattern.</summary>
Bardez::Projects::DirectX::Direct2D::DashStyle StrokeStyle::DashStyle::get()
{
	return (Bardez::Projects::DirectX::Direct2D::DashStyle)(this->StrokeStylePtr->GetDashStyle());
}

/// <summary>Exposes the type of joint used at the vertices of a shape's outline.</summary>
Bardez::Projects::DirectX::Direct2D::LineJoint StrokeStyle::LineJoint::get()
{
	return (Bardez::Projects::DirectX::Direct2D::LineJoint)(this->StrokeStylePtr->GetLineJoin());
}

/// <summary>Exposes the limit on the ratio of the miter length to half the stroke's thickness.</summary>
System::Single StrokeStyle::MiterLimit::get()
{
	return this->StrokeStylePtr->GetMiterLimit();
}
#pragma endregion


#pragma region Construction
/// <summary>Definition constructor</summary>
/// <param name="pointer">ID2D1StrokeStyle source pointer</param>
StrokeStyle::StrokeStyle(ID2D1StrokeStyle* pointer)
{
	this->StrokeStylePtr = pointer;
}
#pragma endregion


#pragma region Destruction
/// <summary>Destructor</summary>
/// <remarks>Dispose()</remarks>
StrokeStyle::~StrokeStyle()
{
	this->DisposeUnmanaged();
}

/// <summary>Destructor</summary>
/// <remarks>Finalize()</remarks>
StrokeStyle::!StrokeStyle()
{
	this->DisposeUnmanaged();
}

/// <summary>Destructor logic, disposes the object</summary>
void StrokeStyle::DisposeUnmanaged()
{
	// Dispose this Brush
	if (this->StrokeStylePtr)
	{
		this->StrokeStylePtr->Release();
		this->StrokeStylePtr = NULL;
	}
}
#pragma endregion


#pragma region Methods
/// <summary>Exposes the dash pattern to the specified array.</summary>
/// <param name="dashesCount">The number of dashes to copy. If this value is less than the number of dashes in the stroke style's dashes array, the returned dashes are truncated to dashesCount. If this value is greater than the number of dashes in the stroke style's dashes array, the extra dashes are set to 0.0f.</param>
/// <returns>A copied array of dash patterns.</returns>
array<System::Single>^ StrokeStyle::GetDashes(System::UInt32 dashesCount)
{
	// allocate both arrays
	array<System::Single>^ dashes = gcnew array<System::Single>(dashesCount);
	System::Single* nativeDashes = new System::Single[dashesCount];

	this->StrokeStylePtr->GetDashes(nativeDashes, dashesCount);

	//copy the results
	for (System::UInt32 index = 0; index < dashesCount; ++index)
		dashes[index] = nativeDashes[index];

	//memory cleanup
	delete [] nativeDashes;

	return dashes;
}
#pragma endregion
