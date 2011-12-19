
#include "Bitmap Brush Properties.h"

using namespace Bardez::Projects::DirectX::Direct2D;

#pragma region Properties
/// <summary>A value that describes how the brush horizontally tiles those areas that extend past its bitmap.</summary>
BrushExtendMode BitmapBrushProperties::ExtendModeX::get()
{
	return this->extendModeX;
}

/// <summary>A value that describes how the brush horizontally tiles those areas that extend past its bitmap.</summary>
void BitmapBrushProperties::ExtendModeX::set(BrushExtendMode value)
{
	this->extendModeX = value;
}

/// <summary>A value that describes how the brush vertically tiles those areas that extend past its bitmap.</summary>
BrushExtendMode BitmapBrushProperties::ExtendModeY::get()
{
	return this->extendModeY;
}

/// <summary>A value that describes how the brush vertically tiles those areas that extend past its bitmap.</summary>
void BitmapBrushProperties::ExtendModeY::set(BrushExtendMode value)
{
	this->extendModeY = value;
}

/// <summary>A value that specifies how the bitmap is interpolated when it is scaled or rotated.</summary>
BitmapInterpolationMode BitmapBrushProperties::InterpolationMode::get()
{
	return this->interpolationMode;
}

/// <summary>A value that specifies how the bitmap is interpolated when it is scaled or rotated.</summary>
void BitmapBrushProperties::InterpolationMode::set(BitmapInterpolationMode value)
{
	this->interpolationMode = value;
}
#pragma endregion


#pragma region Construction
/// <summary>Default Constructor</summary>
BitmapBrushProperties::BitmapBrushProperties()
{
	this->extendModeX = BrushExtendMode::Clamp;
	this->extendModeY = BrushExtendMode::Clamp;
	this->interpolationMode = BitmapInterpolationMode::Linear;
}

/// <summary>Definition Constructor</summary>
/// <param name="extendX">A value that specifies how the brush horizontally tiles those areas that extend past its bitmap.</param>
/// <param name="extendY">A value that specifies how the brush vertically tiles those areas that extend past its bitmap.</param>
/// <param name="interpolationMode">A value that specifies the interpolation algorithm that is used when images are scaled or rotated.</param>
BitmapBrushProperties::BitmapBrushProperties(BrushExtendMode extendX, BrushExtendMode extendY, BitmapInterpolationMode interpolationMode)
{
	this->extendModeX = extendX;
	this->extendModeY = extendY;
	this->interpolationMode = interpolationMode;
}

/// <summary>Definition Constructor</summary>
/// <param name="extendX">A value that specifies how the brush horizontally tiles those areas that extend past its bitmap.</param>
/// <param name="extendY">A value that specifies how the brush vertically tiles those areas that extend past its bitmap.</param>
BitmapBrushProperties::BitmapBrushProperties(BrushExtendMode extendX, BrushExtendMode extendY)
{
	this->extendModeX = extendX;
	this->extendModeY = extendY;
	this->interpolationMode = BitmapInterpolationMode::Linear;
}

/// <summary>Definition Constructor</summary>
/// <param name="extendX">A value that specifies how the brush horizontally tiles those areas that extend past its bitmap.</param>
BitmapBrushProperties::BitmapBrushProperties(BrushExtendMode extendX)
{
	this->extendModeX = extendX;
	this->extendModeY = BrushExtendMode::Clamp;
	this->interpolationMode = BitmapInterpolationMode::Linear;
}

/// <summary>Unmanaged Constructor</summary>
/// <param name="unmanaged">Unmanaged source structure</param>
BitmapBrushProperties::BitmapBrushProperties(D2D1_BITMAP_BRUSH_PROPERTIES unmanaged)
{
	this->extendModeX = (BrushExtendMode)(unmanaged.extendModeX);
	this->extendModeY = (BrushExtendMode)(unmanaged.extendModeY);
	this->interpolationMode = (BitmapInterpolationMode)(unmanaged.interpolationMode);
}
#pragma endregion


#pragma region Methods
/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
/// <returns>An unmanaged Direct2D struct equivalent</returns>
D2D1_BITMAP_BRUSH_PROPERTIES BitmapBrushProperties::ToUnmanaged()
{
	D2D1_BITMAP_BRUSH_PROPERTIES properties;

	properties.extendModeX = (D2D1_EXTEND_MODE)(this->extendModeX);
	properties.extendModeY = (D2D1_EXTEND_MODE)(this->extendModeY);
	properties.interpolationMode = (D2D1_BITMAP_INTERPOLATION_MODE)(this->interpolationMode);

	return properties;
}
#pragma endregion
