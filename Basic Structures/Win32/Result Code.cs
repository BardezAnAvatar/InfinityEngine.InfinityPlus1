using System;

namespace Bardez.Projects.BasicStructures.Win32
{
    /// <summary>Enum that is a managed representation for HRESULT codes</summary>
    /// <remarks>See http://msdn.microsoft.com/en-us/library/cc704587%28v=prot.10%29.aspx for more HRESULT codes</remarks>
    public enum ResultCode : uint /* UInt32 */
    {
		XAudio2_Error_INVALID_CALL							= 0x88960001,
		XAudio2_Error_XMA_DECODER_ERROR						= 0x88960002,
		XAudio2_Error_XAPO_CREATION_FAILED					= 0x88960003,
		XAudio2_Error_DEVICE_INVALIDATED					= 0x88960004,
		XAudio2_Error_ALREADY_INITIALIZED					= 0xFFFFFFFE,	//not referenced?
		Success_OK											= 0x00000000,
		Error_NotImplemented								= 0x80004001,
		Error_ClassNotRegistered							= 0x80040154,	//not installed
		Error_OperationAborted								= 0x80004004,
		Error_AccessDenied									= 0x80070005,
		Error_Failure										= 0x80004005,
		Error_InvalidHandle									= 0x80070006,
		Error_InvalidArgument								= 0x80070057,
		Error_NoSuchInterfaceSupported						= 0x80004002,
		Error_OutOfMemory									= 0x8007000E,
		Error_InvalidPointer								= 0x80004003,
		Error_UnexpectedFailure								= 0x8000FFFF,
		Error_Pending										= 0x8000000A,
		Error_NoSuchInterface								= 0x80000004,
		Direct2D_Error_BadNumber							= 0x88990011,
		Direct2D_Error_DisplayFormatNotSupported			= 0x88990009,
		Direct2D_Error_DisplayStateInvalid					= 0x88990006,
		Direct2D_Error_ExceedsMaximumBitmapSize				= 0x8899001D,
		Direct2D_Error_IncompatibleBrushTypes				= 0x88990018,
		Direct2D_Error_InsufficientBuffer					= 0x0000007A,	//ERROR_INSUFFICIENT_BUFFER (Windows error)
		Direct2D_Error_InternalError						= 0x88990008,
		Direct2D_Error_InvalidCall							= 0x8899000A,
		Direct2D_Error_LayerAlreadyInUse					= 0x88990013,
		Direct2D_Error_MaximumTextureSizeExceeded			= 0x8899000F,
		Direct2D_Error_NoHardwareDevice						= 0x8899000B,
		Direct2D_Error_NotInitialized						= 0x88990002,
		Direct2D_Error_PopCallDidNotMatchPush				= 0x88990014,
		Direct2D_Error_PushPopUnbalanced					= 0x88990016,
		Direct2D_Error_RecreateTarget						= 0x8899000C,
		Direct2D_Error_RenderTagetHasLayerOrClipRectangle	= 0x88990017,
		Direct2D_Error_ScannerFailed						= 0x88990004,
		Direct2D_Error_ScreenAccessDenied					= 0x88990005,
		Direct2D_Error_ShaderCompileFailed					= 0x8899000E,
		Direct2D_Error_TargetNotGdiCompatible				= 0x8899001A,
		Direct2D_Error_TextEffectIsWrongType				= 0x8899001B,
		Direct2D_Error_TextRendererNotREleased				= 0x8899001C,
		Direct2D_Error_TooManyShaderElements				= 0x8899000D,
		Direct2D_Error_UnsupportedOperation					= 0x88990003,
		Direct2D_Error_UnsupportedPixelFormat				= 0x88982F80,	//WINCODEC_ERR_UNSUPPORTEDPIXELFORMAT (error in wincodec.h); (1UL << 31) | (0x898 UL << 16) | (0x2000 UL + 0xf80 UL)
		Direct2D_Error_UnsuportedVersion					= 0x88990010,
		Direct2D_Error_Win32Error							= 0x88990019,
		Direct2D_Error_WrongFactory							= 0x88990012,
		Direct2D_Error_WrongResourceDomain					= 0x88990015,
		Direct2D_Error_WrongState							= 0x88990001,
		Direct2D_Error_ZeroVector							= 0x88990007,
    }
}