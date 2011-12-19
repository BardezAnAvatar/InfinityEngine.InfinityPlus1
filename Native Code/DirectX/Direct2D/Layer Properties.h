
#ifndef Bardez_Projects_DirectX_Direct2D_LayerProperties
#define Bardez_Projects_DirectX_Direct2D_LayerProperties

#include <d2d1.h>
#include "Antialias Mode.h"
#include "Brush.h"
#include "Geometry.h"
#include "Layer Options.h"
#include "Matrix3x2F.h"
#include "RectangleF.h"

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Contains the content bounds, mask information, opacity settings, and other options for a layer resource.</summary>
				public ref class LayerProperties
				{
				#pragma region Fields
				protected:
					/// <summary>The content bounds of the layer. Content outside these bounds is not guaranteed to render</summary>
					/// <values>The default value is RectangleF::InfiniteRectangle</values>
					RectangleF^ contentBounds;

					/// <summary>The geometric mask specifies the area of the layer that is composited into the render target.</summary>
					/// <values>The default value is null</values>
					Geometry^ geometricMask;

					/// <summary>A value that specifies the antialiasing mode for the geometricMask.</summary>
					/// <values>The default value is AntialiasMode.PerPrimitive</values>
					AntialiasMode maskAntialiasMode;

					/// <summary>A value that specifies the transform that is applied to the geometric mask when composing the layer.</summary>
					/// <values>The default value is Matrix3x2F.Identity</values>
					Matrix3x2F^ maskTransform;

					/// <summary>An opacity value that is applied uniformly to all resources in the layer when compositing to the target.</summary>
					/// <values>The default value is 1.0F</values>
					System::Single opacity;

					/// <summary>
					///		A brush that is used to modify the opacity of the layer. The brush is mapped to the layer,
					///		and the alpha channel of each mapped brush pixel is multiplied against the corresponding layer pixel.
					/// </summary>
					/// <values>The default value is null</values>
					Brush^ opacityBrush;

					/// <summary>A value that specifies whether the layer intends to render text with ClearType antialiasing.</summary>
					/// <values>The default value is LayerOptions.None</values>
					Bardez::Projects::DirectX::Direct2D::LayerOptions layerOptions;
				#pragma endregion


				#pragma region Properties
				public:
					/// <summary>The content bounds of the layer. Content outside these bounds is not guaranteed to render</summary>
					property RectangleF^ ContentBounds
					{
						RectangleF^ get();
						void set(RectangleF^ value);
					}

					/// <summary>The geometric mask specifies the area of the layer that is composited into the render target.</summary>
					property Geometry^ GeometricMask
					{
						Geometry^ get();
						void set(Geometry^ value);
					}

					/// <summary>A value that specifies the antialiasing mode for the geometricMask.</summary>
					property AntialiasMode MaskAntialiasMode
					{
						AntialiasMode get();
						void set(AntialiasMode value);
					}

					/// <summary>A value that specifies the transform that is applied to the geometric mask when composing the layer.</summary>
					property Matrix3x2F^ MaskTransform
					{
						Matrix3x2F^ get();
						void set(Matrix3x2F^ value);
					}

					/// <summary>An opacity value that is applied uniformly to all resources in the layer when compositing to the target.</summary>
					property System::Single Opacity
					{
						System::Single get();
						void set(System::Single value);
					}

					/// <summary>
					///		A brush that is used to modify the opacity of the layer. The brush is mapped to the layer,
					///		and the alpha channel of each mapped brush pixel is multiplied against the corresponding layer pixel.
					/// </summary>
					property Brush^ OpacityBrush
					{
						Brush^ get();
						void set(Brush^ value);
					}

					/// <summary>A value that specifies whether the layer intends to render text with ClearType antialiasing.</summary>
					property Bardez::Projects::DirectX::Direct2D::LayerOptions LayerOptions
					{
						Bardez::Projects::DirectX::Direct2D::LayerOptions get();
						void set(Bardez::Projects::DirectX::Direct2D::LayerOptions value);
					}
				#pragma endregion


				#pragma region Contruction
				public:
					/// <summary>Default constructor</summary>
					LayerProperties();

					/// <summary>Definition constructor</summary>
					/// <param name="current">The content bounds of the layer. Content outside these bounds is not guaranteed to render.</param>
					/// <param name="geometryMask">A mask that specifies the area of the layer that is composited into the render target, or NULL.</param>
					/// <param name="maskMode">A value that specifies the antialiasing mode for the geometric mask.</param>
					/// <param name="transform">A value that specifies the transform that is applied to the geometric mask when composing the layer.</param>
					/// <param name="alpha">An opacity that is applied uniformly to all resources in the layer when compositing to the target.</param>
					/// <param name="alphaBrush">A brush that is used to alter the opacity of the layer. The brush is mapped to the layer, and the alpha channel of each mapped brush pixel is multiplied by the corresponding layer pixel.</param>
					/// <param name="options">A value that specifies whether the layer intends to render text with ClearType antialiasing.</param>
					LayerProperties(RectangleF^ current, Geometry^ geometryMask, AntialiasMode maskMode, Matrix3x2F^ transform, System::Single alpha, Brush^ alphaBrush, Bardez::Projects::DirectX::Direct2D::LayerOptions options);

					/// <summary>Definition constructor</summary>
					/// <param name="current">The content bounds of the layer. Content outside these bounds is not guaranteed to render.</param>
					/// <param name="geometryMask">A mask that specifies the area of the layer that is composited into the render target, or NULL.</param>
					/// <param name="maskMode">A value that specifies the antialiasing mode for the geometric mask.</param>
					/// <param name="transform">A value that specifies the transform that is applied to the geometric mask when composing the layer.</param>
					/// <param name="alpha">An opacity that is applied uniformly to all resources in the layer when compositing to the target.</param>
					/// <param name="alphaBrush">A brush that is used to alter the opacity of the layer. The brush is mapped to the layer, and the alpha channel of each mapped brush pixel is multiplied by the corresponding layer pixel.</param>
					LayerProperties(RectangleF^ current, Geometry^ geometryMask, AntialiasMode maskMode, Matrix3x2F^ transform, System::Single alpha, Brush^ alphaBrush);

					/// <summary>Definition constructor</summary>
					/// <param name="current">The content bounds of the layer. Content outside these bounds is not guaranteed to render.</param>
					/// <param name="geometryMask">A mask that specifies the area of the layer that is composited into the render target, or NULL.</param>
					/// <param name="maskMode">A value that specifies the antialiasing mode for the geometric mask.</param>
					/// <param name="transform">A value that specifies the transform that is applied to the geometric mask when composing the layer.</param>
					/// <param name="alpha">An opacity that is applied uniformly to all resources in the layer when compositing to the target.</param>
					LayerProperties(RectangleF^ current, Geometry^ geometryMask, AntialiasMode maskMode, Matrix3x2F^ transform, System::Single alpha);

					/// <summary>Definition constructor</summary>
					/// <param name="current">The content bounds of the layer. Content outside these bounds is not guaranteed to render.</param>
					/// <param name="geometryMask">A mask that specifies the area of the layer that is composited into the render target, or NULL.</param>
					/// <param name="maskMode">A value that specifies the antialiasing mode for the geometric mask.</param>
					/// <param name="transform">A value that specifies the transform that is applied to the geometric mask when composing the layer.</param>
					LayerProperties(RectangleF^ current, Geometry^ geometryMask, AntialiasMode maskMode, Matrix3x2F^ transform);

					/// <summary>Definition constructor</summary>
					/// <param name="current">The content bounds of the layer. Content outside these bounds is not guaranteed to render.</param>
					/// <param name="geometryMask">A mask that specifies the area of the layer that is composited into the render target, or NULL.</param>
					/// <param name="maskMode">A value that specifies the antialiasing mode for the geometric mask.</param>
					LayerProperties(RectangleF^ current, Geometry^ geometryMask, AntialiasMode maskMode);

					/// <summary>Definition constructor</summary>
					/// <param name="current">The content bounds of the layer. Content outside these bounds is not guaranteed to render.</param>
					/// <param name="geometryMask">A mask that specifies the area of the layer that is composited into the render target, or NULL.</param>
					LayerProperties(RectangleF^ current, Geometry^ geometryMask);

					/// <summary>Definition constructor</summary>
					/// <param name="current">The content bounds of the layer. Content outside these bounds is not guaranteed to render.</param>
					LayerProperties(RectangleF^ current);

					/// <summary>Definition constructor</summary>
					/// <param name="current">The content bounds of the layer. Content outside these bounds is not guaranteed to render.</param>
					/// <param name="maskMode">A value that specifies the antialiasing mode for the geometric mask.</param>
					/// <param name="transform">A value that specifies the transform that is applied to the geometric mask when composing the layer.</param>
					/// <param name="alpha">An opacity that is applied uniformly to all resources in the layer when compositing to the target.</param>
					/// <param name="options">A value that specifies whether the layer intends to render text with ClearType antialiasing.</param>
					LayerProperties(RectangleF^ current, AntialiasMode maskMode, Matrix3x2F^ transform, System::Single alpha, Bardez::Projects::DirectX::Direct2D::LayerOptions options);

				internal:
					/// <summary>Unmanaged constructor</summary>
					/// <param name="parameters">D2D1_LAYER_PARAMETERS source structure</param>
					LayerProperties(D2D1_LAYER_PARAMETERS parameters);
				#pragma endregion


				#pragma region Methods
				internal:
					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct equivalent</returns>
					D2D1_LAYER_PARAMETERS ToUnmanaged();
				#pragma endregion
				};
			}
		}
	}
}

#endif

