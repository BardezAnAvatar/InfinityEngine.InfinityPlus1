
#include "Layer Properties.h"

using namespace Bardez::Projects::DirectX::Direct2D;

#pragma region Properties
/// <summary>The content bounds of the layer. Content outside these bounds is not guaranteed to render</summary>
RectangleF^ LayerProperties::ContentBounds::get()
{
	return this->contentBounds;
}

/// <summary>The content bounds of the layer. Content outside these bounds is not guaranteed to render</summary>
void LayerProperties::ContentBounds::set(RectangleF^ value)
{
	this->contentBounds = value;
}

/// <summary>The geometric mask specifies the area of the layer that is composited into the render target.</summary>
Geometry^ LayerProperties::GeometricMask::get()
{
	return this->geometricMask;
}

/// <summary>The geometric mask specifies the area of the layer that is composited into the render target.</summary>
void LayerProperties::GeometricMask::set(Geometry^ value)
{
	this->geometricMask = value;
}

/// <summary>A value that specifies the antialiasing mode for the geometricMask.</summary>
AntialiasMode LayerProperties::MaskAntialiasMode::get()
{
	return this->maskAntialiasMode;
}

/// <summary>A value that specifies the antialiasing mode for the geometricMask.</summary>
void LayerProperties::MaskAntialiasMode::set(AntialiasMode value)
{
	this->maskAntialiasMode = value;
}

/// <summary>A value that specifies the transform that is applied to the geometric mask when composing the layer.</summary>
Matrix3x2F^ LayerProperties::MaskTransform::get()
{
	return this->maskTransform;
}

/// <summary>A value that specifies the transform that is applied to the geometric mask when composing the layer.</summary>
void LayerProperties::MaskTransform::set(Matrix3x2F^ value)
{
	this->maskTransform = value;
}

/// <summary>An opacity value that is applied uniformly to all resources in the layer when compositing to the target.</summary>
System::Single LayerProperties::Opacity::get()
{
	return this->opacity;
}

/// <summary>An opacity value that is applied uniformly to all resources in the layer when compositing to the target.</summary>
void LayerProperties::Opacity::set(System::Single value)
{
	this->opacity = value;
}

/// <summary>
///		A brush that is used to modify the opacity of the layer. The brush is mapped to the layer,
///		and the alpha channel of each mapped brush pixel is multiplied against the corresponding layer pixel.
/// </summary>
Brush^ LayerProperties::OpacityBrush::get()
{
	return this->opacityBrush;
}

/// <summary>
///		A brush that is used to modify the opacity of the layer. The brush is mapped to the layer,
///		and the alpha channel of each mapped brush pixel is multiplied against the corresponding layer pixel.
/// </summary>
void LayerProperties::OpacityBrush::set(Brush^ value)
{
	this->opacityBrush = value;
}

/// <summary>A value that specifies whether the layer intends to render text with ClearType antialiasing.</summary>
Bardez::Projects::DirectX::Direct2D::LayerOptions LayerProperties::LayerOptions::get()
{
	return this->layerOptions;
}

/// <summary>A value that specifies whether the layer intends to render text with ClearType antialiasing.</summary>
void LayerProperties::LayerOptions::set(Bardez::Projects::DirectX::Direct2D::LayerOptions value)
{
	this->layerOptions = value;
}
#pragma endregion


#pragma region Contruction
/// <summary>Default constructor</summary>
LayerProperties::LayerProperties()
{
	this->contentBounds = RectangleF::InfiniteRectangle;
	this->geometricMask = nullptr;
	this->maskAntialiasMode = AntialiasMode::PerPrimitive;
	this->maskTransform = Matrix3x2F::Identity();
	this->opacity = 1.0F;
	this->opacityBrush = nullptr;
	this->layerOptions = Bardez::Projects::DirectX::Direct2D::LayerOptions::None;
}

/// <summary>Definition constructor</summary>
/// <param name="current">The content bounds of the layer. Content outside these bounds is not guaranteed to render.</param>
/// <param name="geometryMask">A mask that specifies the area of the layer that is composited into the render target, or NULL.</param>
/// <param name="maskMode">A value that specifies the antialiasing mode for the geometric mask.</param>
/// <param name="transform">A value that specifies the transform that is applied to the geometric mask when composing the layer.</param>
/// <param name="alpha">An opacity that is applied uniformly to all resources in the layer when compositing to the target.</param>
/// <param name="alphaBrush">A brush that is used to alter the opacity of the layer. The brush is mapped to the layer, and the alpha channel of each mapped brush pixel is multiplied by the corresponding layer pixel.</param>
/// <param name="options">A value that specifies whether the layer intends to render text with ClearType antialiasing.</param>
LayerProperties::LayerProperties(RectangleF^ current, Geometry^ geometryMask, AntialiasMode maskMode, Matrix3x2F^ transform, System::Single alpha, Brush^ alphaBrush, Bardez::Projects::DirectX::Direct2D::LayerOptions options)
{
	this->contentBounds = current;
	this->geometricMask = geometryMask;
	this->maskAntialiasMode = maskMode;
	this->maskTransform = transform;
	this->opacity = alpha;
	this->opacityBrush = alphaBrush;
	this->layerOptions = options;
}

/// <summary>Definition constructor</summary>
/// <param name="current">The content bounds of the layer. Content outside these bounds is not guaranteed to render.</param>
/// <param name="geometryMask">A mask that specifies the area of the layer that is composited into the render target, or NULL.</param>
/// <param name="maskMode">A value that specifies the antialiasing mode for the geometric mask.</param>
/// <param name="transform">A value that specifies the transform that is applied to the geometric mask when composing the layer.</param>
/// <param name="alpha">An opacity that is applied uniformly to all resources in the layer when compositing to the target.</param>
/// <param name="alphaBrush">A brush that is used to alter the opacity of the layer. The brush is mapped to the layer, and the alpha channel of each mapped brush pixel is multiplied by the corresponding layer pixel.</param>
LayerProperties::LayerProperties(RectangleF^ current, Geometry^ geometryMask, AntialiasMode maskMode, Matrix3x2F^ transform, System::Single alpha, Brush^ alphaBrush)
{
	this->contentBounds = current;
	this->geometricMask = geometryMask;
	this->maskAntialiasMode = maskMode;
	this->maskTransform = transform;
	this->opacity = alpha;
	this->opacityBrush = alphaBrush;
	this->layerOptions = Bardez::Projects::DirectX::Direct2D::LayerOptions::None;
}

/// <summary>Definition constructor</summary>
/// <param name="current">The content bounds of the layer. Content outside these bounds is not guaranteed to render.</param>
/// <param name="geometryMask">A mask that specifies the area of the layer that is composited into the render target, or NULL.</param>
/// <param name="maskMode">A value that specifies the antialiasing mode for the geometric mask.</param>
/// <param name="transform">A value that specifies the transform that is applied to the geometric mask when composing the layer.</param>
/// <param name="alpha">An opacity that is applied uniformly to all resources in the layer when compositing to the target.</param>
LayerProperties::LayerProperties(RectangleF^ current, Geometry^ geometryMask, AntialiasMode maskMode, Matrix3x2F^ transform, System::Single alpha)
{
	this->contentBounds = current;
	this->geometricMask = geometryMask;
	this->maskAntialiasMode = maskMode;
	this->maskTransform = transform;
	this->opacity = alpha;
	this->opacityBrush = nullptr;
	this->layerOptions = Bardez::Projects::DirectX::Direct2D::LayerOptions::None;
}

/// <summary>Definition constructor</summary>
/// <param name="current">The content bounds of the layer. Content outside these bounds is not guaranteed to render.</param>
/// <param name="geometryMask">A mask that specifies the area of the layer that is composited into the render target, or NULL.</param>
/// <param name="maskMode">A value that specifies the antialiasing mode for the geometric mask.</param>
/// <param name="transform">A value that specifies the transform that is applied to the geometric mask when composing the layer.</param>
LayerProperties::LayerProperties(RectangleF^ current, Geometry^ geometryMask, AntialiasMode maskMode, Matrix3x2F^ transform)
{
	this->contentBounds = current;
	this->geometricMask = geometryMask;
	this->maskAntialiasMode = maskMode;
	this->maskTransform = transform;
	this->opacity = 1.0F;
	this->opacityBrush = nullptr;
	this->layerOptions = Bardez::Projects::DirectX::Direct2D::LayerOptions::None;
}

/// <summary>Definition constructor</summary>
/// <param name="current">The content bounds of the layer. Content outside these bounds is not guaranteed to render.</param>
/// <param name="geometryMask">A mask that specifies the area of the layer that is composited into the render target, or NULL.</param>
/// <param name="maskMode">A value that specifies the antialiasing mode for the geometric mask.</param>
LayerProperties::LayerProperties(RectangleF^ current, Geometry^ geometryMask, AntialiasMode maskMode)
{
	this->contentBounds = current;
	this->geometricMask = geometryMask;
	this->maskAntialiasMode = maskMode;
	this->maskTransform = Matrix3x2F::Identity();
	this->opacity = 1.0F;
	this->opacityBrush = nullptr;
	this->layerOptions = Bardez::Projects::DirectX::Direct2D::LayerOptions::None;
}

/// <summary>Definition constructor</summary>
/// <param name="current">The content bounds of the layer. Content outside these bounds is not guaranteed to render.</param>
/// <param name="geometryMask">A mask that specifies the area of the layer that is composited into the render target, or NULL.</param>
LayerProperties::LayerProperties(RectangleF^ current, Geometry^ geometryMask)
{
	this->contentBounds = current;
	this->geometricMask = geometryMask;
	this->maskAntialiasMode = AntialiasMode::PerPrimitive;
	this->maskTransform = Matrix3x2F::Identity();
	this->opacity = 1.0F;
	this->opacityBrush = nullptr;
	this->layerOptions = Bardez::Projects::DirectX::Direct2D::LayerOptions::None;
}

/// <summary>Definition constructor</summary>
/// <param name="current">The content bounds of the layer. Content outside these bounds is not guaranteed to render.</param>
LayerProperties::LayerProperties(RectangleF^ current)
{
	this->contentBounds = current;
	this->geometricMask = nullptr;
	this->maskAntialiasMode = AntialiasMode::PerPrimitive;
	this->maskTransform = Matrix3x2F::Identity();
	this->opacity = 1.0F;
	this->opacityBrush = nullptr;
	this->layerOptions = Bardez::Projects::DirectX::Direct2D::LayerOptions::None;
}

/// <summary>Definition constructor</summary>
/// <param name="current">The content bounds of the layer. Content outside these bounds is not guaranteed to render.</param>
/// <param name="maskMode">A value that specifies the antialiasing mode for the geometric mask.</param>
/// <param name="transform">A value that specifies the transform that is applied to the geometric mask when composing the layer.</param>
/// <param name="alpha">An opacity that is applied uniformly to all resources in the layer when compositing to the target.</param>
/// <param name="options">A value that specifies whether the layer intends to render text with ClearType antialiasing.</param>
LayerProperties::LayerProperties(RectangleF^ current, AntialiasMode maskMode, Matrix3x2F^ transform, System::Single alpha, Bardez::Projects::DirectX::Direct2D::LayerOptions options)
{
	this->contentBounds = current;
	this->geometricMask = nullptr;
	this->maskAntialiasMode = maskMode;
	this->maskTransform = transform;
	this->opacity = alpha;
	this->opacityBrush = nullptr;
	this->layerOptions = options;
}

/// <summary>Unmanaged constructor</summary>
/// <param name="parameters">D2D1_LAYER_PARAMETERS source structure</param>
LayerProperties::LayerProperties(D2D1_LAYER_PARAMETERS parameters)
{
	this->contentBounds = gcnew RectangleF(parameters.contentBounds);
	this->geometricMask = gcnew Geometry(parameters.geometricMask);
	this->maskAntialiasMode = (AntialiasMode)(parameters.maskAntialiasMode);
	this->maskTransform = gcnew Matrix3x2F(parameters.maskTransform);
	this->opacity = parameters.opacity;
	this->opacityBrush = gcnew Brush(parameters.opacityBrush);
	this->layerOptions = (Bardez::Projects::DirectX::Direct2D::LayerOptions)(parameters.layerOptions);
}
#pragma endregion


#pragma region Methods
/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
/// <returns>An unmanaged Direct2D struct equivalent</returns>
D2D1_LAYER_PARAMETERS LayerProperties::ToUnmanaged()
{
	D2D1_LAYER_PARAMETERS parameters;

	parameters.contentBounds = this->contentBounds->ToUnmanaged();
	parameters.geometricMask = NULL; //&(this->geometricMask->ToUnmanaged());
	parameters.maskAntialiasMode = (D2D1_ANTIALIAS_MODE)(this->maskAntialiasMode);
	parameters.maskTransform = this->maskTransform->ToUnmanaged();
	parameters.opacity = this->opacity;
	parameters.opacityBrush = NULL; //this->opacityBrush->ToUnmanaged();
	parameters.layerOptions = (D2D1_LAYER_OPTIONS)(this->layerOptions);

	return parameters;
}
#pragma endregion