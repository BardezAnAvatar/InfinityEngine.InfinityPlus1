
#ifndef Bardez_Projects_DirectX_Direct2D_ControlRenderTarget
#define Bardez_Projects_DirectX_Direct2D_ControlRenderTarget

#include "Render Target.h"
#include "Control State.h"

using namespace Bardez::Projects::Win32;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a Win32 window/control render target for Direct2D</summary>
				public ref class ControlRenderTarget : public RenderTarget
				{
				#pragma region Properties
				internal:
					/// <summary>Represents a pointer to the native HWND render target interface object</summary>
					property ID2D1HwndRenderTarget* HwndRenderPtr
					{
						ID2D1HwndRenderTarget* get();
						void set(ID2D1HwndRenderTarget* value);
					}

					/// <summary>Represents a pointer to the native HWND</summary>
					property HWND Hwnd
					{
						HWND get();
					}

				public:
					/// <summary>Represents a pointer to the native HWND</summary>
					property System::IntPtr Handle
					{
						System::IntPtr get();
					}
				#pragma endregion


				#pragma region Construction
				internal:
					/// <summary>Definition constructor</summary>
					ControlRenderTarget(ID2D1HwndRenderTarget* handle);
				#pragma endregion
					

				#pragma region Destruction
				public:
					/// <summary>Destructor</summary>
					/// <remarks>Dispose()</remarks>
					~ControlRenderTarget();

					/// <summary>Destructor</summary>
					/// <remarks>Finalize()</remarks>
					!ControlRenderTarget();

					/// <summary>Destructor logic, disposes the object</summary>
					virtual void DisposeUnmanaged() override;
				#pragma endregion


				#pragma region Interface Methods
				public:
					
					/// <summary>Indicates whether the control/HWND associated with this render target is occluded</summary>
					/// <remarks>
					///		If the window was occluded the last time that EndDraw was called, the next time that the render target calls
					///		CheckControlState, it will return ControlState.Occluded regardless of the current window state.
					/// </remarks>
					virtual ControlState CheckControlState();

					/// <summary>Changes the size of the render target to the specified pixel size.</summary>
					/// <param name="size">The new size of the render target in device pixels.</param>
					/// <returns>S_OK on success, otherwise an error code.</returns>
					virtual ResultCode Resize(SizeU^ size);
				#pragma endregion
				};
			}
		}
	}
}

#endif
