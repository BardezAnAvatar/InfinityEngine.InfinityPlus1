

#ifndef Bardez_Projects_DirectX_Direct2D_LayerOptions
#define Bardez_Projects_DirectX_Direct2D_LayerOptions

using namespace System::ComponentModel;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Specifies options that can be applied when a layer resource is applied to create a layer</summary>
				/// <remarks>See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368124%28v=VS.85%29.aspx for more detail</remarks>
				public enum struct LayerOptions : System::UInt32
				{
					/// <summary>The text in this layer does not use ClearType antialiasing.</summary>
					[DescriptionAttribute("The text in this layer does not use ClearType antialiasing.")]
					None			= 0,

					/// <summary>
					///		The layer renders correctly for ClearType text. If the render target is set to ClearType, the layer continues to render ClearType.
					///		If the render target is set to ClearType and this option is not specified, the render target will be set to render
					///		gray-scale until the layer is popped. The caller can override this default by calling SetTextAntialiasMode while within the layer.
					///		This flag is slightly slower than the default.
					///	</summary>
					[DescriptionAttribute("The layer renders correctly for ClearType text. If the render target is set to ClearType, the layer continues to render ClearType. If the render target is set to ClearType and this option is not specified, the render target will be set to render gray-scale until the layer is popped. The caller can override this default by calling SetTextAntialiasMode while within the layer. This flag is slightly slower than the default.")]
					Occluded   = 1,
				};
			}
		}
	}
}


#endif
