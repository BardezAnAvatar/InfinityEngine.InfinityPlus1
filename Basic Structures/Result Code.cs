using System;

namespace Bardez.Projects.Win32
{
    /// <summary>Represents a standard HRESULT code common to Win32 API calls</summary>
    /// <remarks>See http://msdn.microsoft.com/en-us/library/cc704587%28v=prot.10%29.aspx for more HRESULT codes</remarks>
    public enum ResultCode : uint /* UInt32 */
    {
        /// <summary>An API call or one of its arguments was illegal</summary>
		XAUDIO2_E_INVALID_CALL							= 0x88960001,

        /// <summary>The XMA hardware suffered an unrecoverable error</summary>
		XAUDIO2_E_XMA_DECODER_ERROR						= 0x88960002,

        /// <summary>XAudio2 failed to initialize an XAPO effect</summary>
		XAUDIO2_E_XAPO_CREATION_FAILED					= 0x88960003,

        /// <summary>An audio device became unusable (unplugged, etc)</summary>
		XAUDIO2_E_DEVICE_INVALIDATED					= 0x88960004,

        /// <summary>Already initialized</summary>
		XAUDIO2_E_ALREADY_INITIALIZED					= 0xFFFFFFFE,	//not referenced?

        /// <summary>Operation successful</summary>
		S_OK											= 0x00000000,

        /// <summary>Not implemented</summary>
		E_NOTIMPL								        = 0x80004001,

        /// <summary>Class not registered</summary>
		E_CLASSNOTREG							        = 0x80040154,	//not installed

        /// <summary>Operation aborted</summary>
		E_ABORT								            = 0x80004004,

        /// <summary>General access denied error</summary>
		E_ACCESSDENIED									= 0x80070005,

        /// <summary>Failed to allocate necessary memory</summary>
        E_OUTOFMEMORY                                   = 0x8007000E,

        /// <summary>Unspecified failure</summary>
		E_FAIL										    = 0x80004005,

        /// <summary>Handle that is not valid</summary>
		E_HANDLE									    = 0x80070006,

        /// <summary>One or more arguments are not valid</summary>
		E_INVALIDARG								    = 0x80070057,

        /// <summary>No such interface supported</summary>
		E_INTERFACENOTSUPPORTED						    = 0x80004002,

        /// <summary>Pointer that is not valid</summary>
		E_POINTER								        = 0x80004003,

        /// <summary>Unexpected failure</summary>
        E_UNEXPECTED                                    = 0x8000FFFF,

        /// <summary>The data necessary to complete this operation is not yet available</summary>
		E_PENDING										= 0x8000000A,

        /// <summary>No such interface supported</summary>
		E_NOINTERFACE								    = 0x80000004,

        /// <summary>Invalid number.</summary>
		D2DERR_BAD_NUMBER							    = 0x88990011,

        /// <summary>The display format we need to render is not supported by the hardware device.</summary>
		D2DERR_DISPLAY_FORMAT_NOT_SUPPORTED			    = 0x88990009,

        /// <summary>A valid display state could not be determined.</summary>
		D2DERR_DISPLAY_STATE_INVALID					= 0x88990006,

        /// <summary>The requested size is larger than the guaranteed supported texture size.</summary>
		D2DERR_EXCEEDS_MAX_BITMAP_SIZE				    = 0x8899001D,

        /// <summary>The brush types are incompatible for the call.</summary>
		D2DERR_INCOMPATIBLE_BRUSH_TYPES				    = 0x88990018,

        /// <summary>The supplied buffer was too small to accomodate the data.</summary>
		D2DERR_INSUFFICIENT_BUFFER					    = 0x0000007A,	//ERROR_INSUFFICIENT_BUFFER (Windows error)

        /// <summary>The application should close this instance of D2D and should consider restarting its process.</summary>
		D2DERR_INTERNAL_ERROR						    = 0x88990008,

        /// <summary>A call to this method is invalid.</summary>
		D2DERR_INVALID_CALL							    = 0x8899000A,

        /// <summary>A layer resource can only be in use once at any point in time.</summary>
		D2DERR_LAYER_ALREADY_IN_USE					    = 0x88990013,

        /// <summary>Requested DX surface size exceeded maximum texture size.</summary>
		D2DERR_MAX_TEXTURE_SIZE_EXCEEDED			    = 0x8899000F,

        /// <summary>No HW rendering device is available for this operation.</summary>
		D2DERR_NO_HARDWARE_DEVICE						= 0x8899000B,

        /// <summary>The object has not yet been initialized.</summary>
		D2DERR_NOT_INITIALIZED						    = 0x88990002,

        /// <summary>The pop call did not match the corresponding push call.</summary>
		D2DERR_POP_CALL_DID_NOT_MATCH_PUSH				= 0x88990014,

        /// <summary>The push and pop calls were unbalanced.</summary>
		D2DERR_PUSH_POP_UNBALANCED					    = 0x88990016,

        /// <summary>
        ///     There has been a presentation error that may be recoverable. The caller
        ///     needs to recreate, rerender the entire frame, and reattempt present.
        /// </summary>
		D2DERR_RECREATE_TARGET						    = 0x8899000C,

        /// <summary>Attempt to copy from a render target while a layer or clip rect is applied.</summary>
		D2DERR_RENDER_TARGET_HAS_LAYER_OR_CLIPRECT	    = 0x88990017,

        /// <summary>The geomery scanner failed to process the data.</summary>
        D2DERR_SCANNER_FAILED						    = 0x88990004,

        /// <summary>D2D could not access the screen.</summary>
		D2DERR_SCREEN_ACCESS_DENIED					    = 0x88990005,

        /// <summary>Shader compilation failed.</summary>
		D2DERR_SHADER_COMPILE_FAILED					= 0x8899000E,

        /// <summary>The render target is not compatible with GDI.</summary>
		D2DERR_TARGET_NOT_GDI_COMPATIBLE				= 0x8899001A,

        /// <summary>A text client drawing effect object is of the wrong type.</summary>
		D2DERR_TEXT_EFFECT_IS_WRONG_TYPE				= 0x8899001B,

        /// <summary>
        ///     The application is holding a reference to the IDWriteTextRenderer interface
        ///     after the corresponding DrawText or DrawTextLayout call has returned. The
        ///     IDWriteTextRenderer instance will be zombied.
        /// </summary>
		D2DERR_TEXT_RENDERER_NOT_RELEASED				= 0x8899001C,

        /// <summary>Shader construction failed because it was too complex.</summary>
		D2DERR_TOO_MANY_SHADER_ELEMENTS				    = 0x8899000D,

        /// <summary>The requested opertion is not supported.</summary>
		D2DERR_UNSUPPORTED_OPERATION					= 0x88990003,

        /// <summary>The pixel format is not supported.</summary>
		D2DERR_UNSUPPORTED_PIXEL_FORMAT				    = 0x88982F80,	//WINCODEC_ERR_UNSUPPORTEDPIXELFORMAT (error in wincodec.h); (1UL << 31) | (0x898 UL << 16) | (0x2000 UL + 0xf80 UL)

        /// <summary>The requested D2D version is not supported.</summary>
        D2DERR_UNSUPPORTED_VERSION					    = 0x88990010,

        /// <summary>An unknown win32 failure occurred.</summary>
		D2DERR_WIN32_ERROR							    = 0x88990019,

        /// <summary>Objects used together must be created from the same factory instance.</summary>
		D2DERR_WRONG_FACTORY							= 0x88990012,

        /// <summary>The resource was realized on the wrong render target.</summary>
		D2DERR_WRONG_RESOURCE_DOMAIN					= 0x88990015,

        /// <summary>The object was not in the correct state to process the method.</summary>
		D2DERR_WRONG_STATE							    = 0x88990001,

        /// <summary>The supplied vector is vero.</summary>
		D2DERR_ZERO_VECTOR							    = 0x88990007,
    }
}