
#include "Factory.h"

using namespace Bardez::Projects::DirectX::Direct2D;

#pragma region Properties
/// <summary>Represents a pointer to the native Direct2D factory interface object</summary>
ID2D1Factory* Factory::FactoryPtr::get()
{
	return reinterpret_cast<ID2D1Factory*>(this->factoryPtr.ToPointer());
}

/// <summary>Represents a pointer to the native Direct2D factory interface object</summary>
void Factory::FactoryPtr::set(ID2D1Factory* value)
{
	this->factoryPtr = System::IntPtr(value);
}

/// <summary>A reference to the IID of ID2D1Factory that is obtained by using __uuidof(ID2D1Factory).</summary>
System::Guid Factory::RefGuid::get()
{
	GUID refiid = __uuidof(ID2D1Factory);

	System::Guid refGuid = System::Guid
		(refiid.Data1,	 refiid.Data2, refiid.Data3,
			refiid.Data4[0], refiid.Data4[1], refiid.Data4[2], refiid.Data4[3], 
			refiid.Data4[4], refiid.Data4[5], refiid.Data4[6], refiid.Data4[7]
		);
	
	return refGuid;
}
#pragma endregion


#pragma region Construction
/// <summary>Default constructor</summary>
Factory::Factory()
{
	void* factory;

	D2D1CreateFactory((D2D1_FACTORY_TYPE)(FactoryType::SingleThreaded), __uuidof(ID2D1Factory), &factory);
	this->FactoryPtr = reinterpret_cast<ID2D1Factory*>(factory);
}

/// <summary>Debug constructor</summary>
Factory::Factory(FactoryType factory, DebugLevel level)
{
	void* factory_pointer;

	D2D1_FACTORY_OPTIONS options;
	options.debugLevel = (D2D1_DEBUG_LEVEL)(level);

	D2D1CreateFactory((D2D1_FACTORY_TYPE)(factory), __uuidof(ID2D1Factory), &options, &factory_pointer);
	this->FactoryPtr = reinterpret_cast<ID2D1Factory*>(factory_pointer);
}
#pragma endregion
					

#pragma region Destruction
/// <summary>Destructor</summary>
/// <remarks>Dispose()</remarks>
Factory::~Factory()
{
	this->DisposeUnmanaged();
}

/// <summary>Destructor</summary>
/// <remarks>Finalize()</remarks>
Factory::!Factory()
{
	this->DisposeUnmanaged();
}

/// <summary>Destructor logic, disposes the object</summary>
void Factory::DisposeUnmanaged()
{
	// Dispose this factory
	if (this->FactoryPtr)
	{
		this->FactoryPtr->Release();
		this->FactoryPtr = NULL;
	}
}
#pragma endregion


#pragma region Interface Methods
/// <summary>Retrieves the current desktop dots per inch (DPI). To refresh this value, call <See cref="ReloadSystemMetrics" />.</summary>
/// <returns>the horizontal and vertical DPI of the desktop.</returns>
/// <remarks>Use this method to obtain the system DPI when setting physical pixel values, such as when you specify the size of a window.</remarks>
DpiResolution Factory::GetDesktopDpi()
{
	System::Single dpiX, dpiY;

	this->ReloadSystemMetrics();	//per Microsoft documentation, reload this.
	this->FactoryPtr->GetDesktopDpi(&dpiX, &dpiY);

	return DpiResolution(dpiX, dpiY);
}

/// <summary>Creates an HwndRenderTarget, a render target that renders to a window.</summary>
/// <param name="rtProperties">The rendering mode, pixel format, remoting options, DPI information, and the minimum DirectX support required for hardware rendering.</param>
/// <param name="hwndRtProperties">The window handle, initial size (in pixels), and present options.</param>
/// <param name="hwnd">When this method returns, contains the address of the pointer to the ID2D1HwndRenderTarget object created by this method.</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
/// <remarks>Use this method to obtain the system DPI when setting physical pixel values, such as when you specify the size of a window.</remarks>
ResultCode Factory::CreateHwndRenderTarget(RenderTargetProperties^ rtProperties, HwndRenderTargetProperties^ hwndRtProperties, [System::Runtime::InteropServices::Out] ControlRenderTarget^ %hwnd)
{
	ID2D1HwndRenderTarget* hwndRenderTarget;

	ResultCode result = (ResultCode)(this->FactoryPtr->CreateHwndRenderTarget(&(rtProperties->ToUnmanaged()), &(hwndRtProperties->ToUnmanaged()), &hwndRenderTarget));

	hwnd = gcnew ControlRenderTarget(hwndRenderTarget);

	return result;
}

/// <summary>Creates an StrokeStyle that describes start cap, dash pattern, and other features of a stroke.</summary>
/// <param name="strokeStyleProperties">Describes the stroke's line cap, dash offset, and other details of a stroke.</param>
/// <param name="dashes">Array whose elements are set to the length of each dash and space in the dash pattern. The first element sets the length of a dash, the second element sets the length of a space, the third element sets the length of a dash, and so on.</param>
/// <param name="strokeStyle">Output stroke style created by this method</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode Factory::CreateStrokeStyle(StrokeStyleProperties^ strokeStyleProperties, array<System::Single>^ dashes, [System::Runtime::InteropServices::Out] StrokeStyle^ %strokeStyle)
{
	ID2D1StrokeStyle* stkPtr;

	System::Single* nativeDashes = NULL;
	System::UInt32 dashesCount = 0;
	if (dashes != nullptr)
	{
		//pin the dashes array
		pin_ptr<System::Single> dashesPtr = &dashes[0];
		nativeDashes = dashesPtr;

		dashesCount = System::Convert::ToUInt32(dashes->Length);
	}

	ResultCode result = (ResultCode)(this->FactoryPtr->CreateStrokeStyle(strokeStyleProperties->ToUnmanaged(), nativeDashes, dashesCount, &stkPtr));

	return result;
}

/// <summary>Creates an StrokeStyle that describes start cap, dash pattern, and other features of a stroke.</summary>
/// <param name="strokeStyleProperties">Describes the stroke's line cap, dash offset, and other details of a stroke.</param>
/// <param name="strokeStyle">Output stroke style created by this method</param>
/// <returns>S_OK on success, otherwise an error code.</returns>
ResultCode Factory::CreateStrokeStyle(StrokeStyleProperties^ strokeStyleProperties, [System::Runtime::InteropServices::Out] StrokeStyle^ %strokeStyle)
{
	ID2D1StrokeStyle* stkPtr;

	System::Single* nativeDashes = NULL;
	System::UInt32 dashesCount = 0;

	ResultCode result = (ResultCode)(this->FactoryPtr->CreateStrokeStyle(strokeStyleProperties->ToUnmanaged(), nativeDashes, dashesCount, &stkPtr));

	return result;
}

/// <summary>Forces the factory to refresh any system defaults that it might have changed since factory creation.</summary>
/// <returns>S_OK on success, otherwise an error code.</returns>
/// <remarks>You should call this method before calling the GetDesktopDpi method, to ensure that the system DPI is current.</remarks>
ResultCode Factory::ReloadSystemMetrics()
{
	return (ResultCode)(this->FactoryPtr->ReloadSystemMetrics());
}
#pragma endregion