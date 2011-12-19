
#include "Gradient Stop Collection.h"

using namespace Bardez::Projects::DirectX::Direct2D;


#pragma region Properties
/// <summary>Exposes a pointer to a ID2D1GradientStopCollection native interface</summary>
ID2D1GradientStopCollection* GradientStopCollection::GradientStopCollectionPtr::get()
{
	return reinterpret_cast<ID2D1GradientStopCollection*>(this->gradientStopCollectionPtr.ToPointer());
}

/// <summary>Exposes a pointer to a ID2D1GradientStopCollection native interface</summary>
void GradientStopCollection::GradientStopCollectionPtr::set(ID2D1GradientStopCollection* value)
{
	this->gradientStopCollectionPtr = System::IntPtr(value);
}

/// <summary>Exposes the gamma space in which the gradient stops are interpolated.</summary>
Gamma GradientStopCollection::ColorInterpolationGamma::get()
{
	return (Gamma)this->GradientStopCollectionPtr->GetColorInterpolationGamma();
}

/// <summary>Exposes the behavior of the gradient outside the normalized gradient range.</summary>
BrushExtendMode GradientStopCollection::ExtendMode::get()
{
	return (BrushExtendMode)this->GradientStopCollectionPtr->GetExtendMode();
}

/// <summary>Exposes the number of gradient stops in the collection.</summary>
System::UInt32 GradientStopCollection::GradientStopCount::get()
{
	return this->GradientStopCollectionPtr->GetGradientStopCount();
}
#pragma endregion


#pragma region Construction
/// <summary>Definition constructor</summary>
/// <param name="pointer">ID2D1GradientStopCollection source pointer</param>
GradientStopCollection::GradientStopCollection(ID2D1GradientStopCollection* pointer)
{
	this->GradientStopCollectionPtr = pointer;
}
#pragma endregion


#pragma region Destruction
/// <summary>Destrutor</summary>
/// <remarks>Dispose()</remarks>
GradientStopCollection::~GradientStopCollection()
{
	this->DisposeUnmanaged();
}

/// <summary>Destrutor</summary>
/// <remarks>Finalize()</remarks>
GradientStopCollection::!GradientStopCollection()
{
	this->DisposeUnmanaged();
}

/// <summary>Destrutor logic, disposes the object</summary>
void GradientStopCollection::DisposeUnmanaged()
{
	// Dispose this Brush
	if (this->GradientStopCollectionPtr)
	{
		this->GradientStopCollectionPtr->Release();
		this->GradientStopCollectionPtr = NULL;
	}
}
#pragma endregion


#pragma region Methods
/// <summary>Copies the gradient stops from the collection into an array of GradientStop objects.</summary>
/// <param name="gradientStopsCount">A value indicating the number of gradient stops to copy</param>
/// <returns>A one-dimensional array of GradientStop references containing copies of the collection's gradient stops.</returns>
array<GradientStop^>^ GradientStopCollection::GetGradientStops(System::UInt32 gradientStopsCount)
{
	//pre-allocated array
	D2D1_GRADIENT_STOP* gradients = new D2D1_GRADIENT_STOP[gradientStopsCount];
	
	this->GradientStopCollectionPtr->GetGradientStops(gradients, gradientStopsCount);

	array<GradientStop^>^ wrappedGradients = gcnew array<GradientStop^>(gradientStopsCount);

	// instantiate a new gradient stop wrapping the native stop
	for (System::UInt32 i = 0; i < gradientStopsCount; ++i)
		wrappedGradients[i] = gcnew GradientStop(gradients[i]);

	//memory cleanup
	delete[] gradients;

	return wrappedGradients;
}
#pragma endregion