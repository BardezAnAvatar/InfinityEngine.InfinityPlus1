

#ifndef Bardez_Projects_DirectX_Direct2D_AntialiasMode
#define Bardez_Projects_DirectX_Direct2D_AntialiasMode

using namespace System::ComponentModel;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Specifies how the edges of nontext primitives are rendered.</summary>
				/// <remarks>
				///		See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368061%28v=VS.85%29.aspx for more detail
				///	</remarks>
				public enum struct AntialiasMode : System::UInt32
				{
					[DescriptionAttribute("Edges are antialiased using the Direct2D per-primitive method of high-quality antialiasing.")]
					PerPrimitive	= 0,
						
					[DescriptionAttribute("Objects are aliased in most cases. Objects are antialiased only when they are drawn to a render target created by the CreateDxgiSurfaceRenderTarget method and Direct3D multisampling has been enabled on the backing DirectX Graphics Infrastructure (DXGI) surface.")]
					Aliased			= 1,
				};
			}
		}
	}
}


#endif
