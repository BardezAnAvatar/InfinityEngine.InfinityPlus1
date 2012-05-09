
#include "Layer.h"

using namespace Bardez::Projects::DirectX::Direct2D;

#pragma region Properties
/// <summary>Represents a pointer to the native render target layer interface object</summary>
ID2D1Layer* Layer::LayerPtr::get()
{
	return reinterpret_cast<ID2D1Layer*>(this->layerPtr.ToPointer());
}

/// <summary>Represents a pointer to the native render target layer interface object</summary>
void Layer::LayerPtr::set(ID2D1Layer* value)
{
	this->layerPtr = System::IntPtr(value);
}
#pragma endregion


#pragma region Construction
/// <summary>Definition constructor</summary>
Layer::Layer(ID2D1Layer* handle)
{
	this->LayerPtr = handle;
}
#pragma endregion


#pragma region Destruction
/// <summary>Destructor</summary>
/// <remarks>Dispose()</remarks>
Layer::~Layer()
{
	this->DisposeUnmanaged();
}

/// <summary>Destructor</summary>
/// <remarks>Finalize()</remarks>
Layer::!Layer()
{
	this->DisposeUnmanaged();
}

/// <summary>Destructor logic, disposes the object</summary>
void Layer::DisposeUnmanaged()
{
	// Dispose this factory
	if (this->LayerPtr)
	{
		this->LayerPtr->Release();
		this->LayerPtr = NULL;
	}
}
#pragma endregion


#pragma region Interface Methods
/// <summary>Gets the size of the layer in device-independent pixels.</summary>
/// <returns>The size of the layer in device-independent pixels.</returns>
SizeF^ Layer::GetSize()
{
	return gcnew SizeF(this->LayerPtr->GetSize());
}
#pragma endregion
