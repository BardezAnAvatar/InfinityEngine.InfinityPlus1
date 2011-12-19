
#include "ColorF.h"

using namespace Bardez::Projects::DirectX::Direct2D;


#pragma region Properties
/// <summary>Represents the red color composition of the current color</summary>
/// <value>Ranges from 0.0F to 1.0F, inclusive</value>
System::Single ColorF::Red::get()
{
	return this->red;
}

/// <summary>Represents the red color composition of the current color</summary>
/// <value>Ranges from 0.0F to 1.0F, inclusive</value>
void ColorF::Red::set(System::Single value)
{
	this->red = value;
}

/// <summary>Represents the green color composition of the current color</summary>
/// <value>Ranges from 0.0F to 1.0F, inclusive</value>
System::Single ColorF::Green::get()
{
	return this->green;
}

/// <summary>Represents the green color composition of the current color</summary>
/// <value>Ranges from 0.0F to 1.0F, inclusive</value>
void ColorF::Green::set(System::Single value)
{
	this->green = value;
}

/// <summary>Represents the blue color composition of the current color</summary>
/// <value>Ranges from 0.0F to 1.0F, inclusive</value>
System::Single ColorF::Blue::get()
{
	return this->blue;
}

/// <summary>Represents the blue color composition of the current color</summary>
/// <value>Ranges from 0.0F to 1.0F, inclusive</value>
void ColorF::Blue::set(System::Single value)
{
	this->blue = value;
}

/// <summary>Represents the alpha transparency composition of the current color</summary>
/// <value>Ranges from 0.0F to 1.0F, inclusive</value>
System::Single ColorF::Alpha::get()
{
	return this->alpha;
}

/// <summary>Represents the alpha transparency composition of the current color</summary>
/// <value>Ranges from 0.0F to 1.0F, inclusive</value>
void ColorF::Alpha::set(System::Single value)
{
	this->alpha = value;
}
#pragma endregion


#pragma region Construction
/// <summary>Definition constructor</summary>
/// <param name="r">Red component to the color, as a single-precision floating point value</param>
/// <param name="g">Green component to the color, as a single-precision floating point value</param>
/// <param name="b">Blue component to the color, as a single-precision floating point value</param>
/// <param name="a">Alpha transparency component to the color, as a single-precision floating point value</param>
ColorF::ColorF(System::Single r, System::Single g, System::Single b, System::Single a)
{
	this->red = r;
	this->green = g;
	this->blue = b;
	this->alpha = a;
}

/// <summary>Definition constructor</summary>
/// <param name="r">Red component to the color, as a single-precision floating point value</param>
/// <param name="g">Green component to the color, as a single-precision floating point value</param>
/// <param name="b">Blue component to the color, as a single-precision floating point value</param>
ColorF::ColorF(System::Single r, System::Single g, System::Single b)
{
	this->red = r;
	this->green = g;
	this->blue = b;
	this->alpha = 1.0;
}

/// <summary>Definition constructor</summary>
/// <param name="r">Red component to the color, as an integer value</param>
/// <param name="g">Green component to the color, as an integer value</param>
/// <param name="b">Blue component to the color, as an integer value</param>
/// <param name="a">Alpha transparency component to the color, as an integer value</param>
ColorF::ColorF(System::Byte r, System::Byte g, System::Byte b, System::Byte a)
{
	this->red = System::Convert::ToSingle(r) / (255.0F);
	this->green = System::Convert::ToSingle(g) / (255.0F);
	this->blue = System::Convert::ToSingle(b) / (255.0F);
	this->alpha = System::Convert::ToSingle(a) / (255.0F);
}

/// <summary>Definition constructor</summary>
/// <param name="r">Red component to the color, as an integer value</param>
/// <param name="g">Green component to the color, as an integer value</param>
/// <param name="b">Blue component to the color, as an integer value</param>
ColorF::ColorF(System::Byte r, System::Byte g, System::Byte b)
{
	this->red = System::Convert::ToSingle(r) / (255.0F);
	this->green = System::Convert::ToSingle(g) / (255.0F);
	this->blue = System::Convert::ToSingle(b) / (255.0F);
	this->alpha = 1.0F;
}

/// <summary>Definition constructor</summary>
/// <param name="r">Red component to the color, as an integer value</param>
/// <param name="g">Green component to the color, as an integer value</param>
/// <param name="b">Blue component to the color, as an integer value</param>
/// <param name="a">Alpha transparency component to the color, as an integer value</param>
ColorF::ColorF(System::Int32 r, System::Int32 g, System::Int32 b, System::Int32 a)
{
	if (r > 255 || r < 0)
		throw gcnew System::ArgumentOutOfRangeException(System::String::Format("The argument 'r' was not a value (inclusively) between 0 and 255. Value was {0}.", r));

	if (g > 255 || g < 0)
		throw gcnew System::ArgumentOutOfRangeException(System::String::Format("The argument 'g' was not a value (inclusively) between 0 and 255. Value was {0}.", g));

	if (b > 255 || b < 0)
		throw gcnew System::ArgumentOutOfRangeException(System::String::Format("The argument 'b' was not a value (inclusively) between 0 and 255. Value was {0}.", b));

	if (a > 255 || a < 0)
		throw gcnew System::ArgumentOutOfRangeException(System::String::Format("The argument 'a' was not a value (inclusively) between 0 and 255. Value was {0}.", a));

	this->red = System::Convert::ToSingle(r) / (255.0F);
	this->green = System::Convert::ToSingle(g) / (255.0F);
	this->blue = System::Convert::ToSingle(b) / (255.0F);
	this->alpha = System::Convert::ToSingle(a) / (255.0F);
}

/// <summary>Definition constructor</summary>
/// <param name="r">Red component to the color, as an integer value</param>
/// <param name="g">Green component to the color, as an integer value</param>
/// <param name="b">Blue component to the color, as an integer value</param>
ColorF::ColorF(System::Int32 r, System::Int32 g, System::Int32 b)
{
	if (r > 255 || r < 0)
		throw gcnew System::ArgumentOutOfRangeException(System::String::Format("The argument 'r' was not a value (inclusively) between 0 and 255. Value was {0}.", r));

	if (g > 255 || g < 0)
		throw gcnew System::ArgumentOutOfRangeException(System::String::Format("The argument 'g' was not a value (inclusively) between 0 and 255. Value was {0}.", g));

	if (b > 255 || b < 0)
		throw gcnew System::ArgumentOutOfRangeException(System::String::Format("The argument 'b' was not a value (inclusively) between 0 and 255. Value was {0}.", b));

	this->red = System::Convert::ToSingle(r) / (255.0F);
	this->green = System::Convert::ToSingle(g) / (255.0F);
	this->blue = System::Convert::ToSingle(b) / (255.0F);
	this->alpha = 1.0F;
}

/// <summary>Definition constructor</summary>
/// <param name="rgba">Entire color, as a single integer value</param>
ColorF::ColorF(System::UInt32 rgba)
{
	System::Byte r, g, b, a;
	r = 0x000000FFU & rgba;
	g = 0x0000FF00U & rgba;
	b = 0x00FF0000U & rgba;
	a = 0xFF000000U & rgba;

	this->red = System::Convert::ToSingle(r) / (255.0F);
	this->green = System::Convert::ToSingle(g) / (255.0F);
	this->blue = System::Convert::ToSingle(b) / (255.0F);
	this->alpha = System::Convert::ToSingle(a) / (255.0F);
}

/// <summary>Definition constructor</summary>
/// <param name="color">System.Drawing struct containing an integer color</param>
ColorF::ColorF(System::Drawing::Color color)
{
	this->red = System::Convert::ToSingle(color.R) / (255.0F);
	this->green = System::Convert::ToSingle(color.G) / (255.0F);
	this->blue = System::Convert::ToSingle(color.B) / (255.0F);
	this->alpha = System::Convert::ToSingle(color.A) / (255.0F);
}

/// <summary>Unmanaged constructor</summary>
/// <param name="color">D2D1_COLOR_F source structure</param>
ColorF::ColorF(D2D1_COLOR_F color)
{
	this->red = color.r;
	this->green = color.g;
	this->blue = color.b;
	this->alpha = color.a;
}
#pragma endregion


#pragma region Methods
/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
/// <returns>An unmanaged Direct2D struct equivalent</returns>
D2D1_COLOR_F ColorF::ToUnmanaged()
{
	D2D1_COLOR_F color;

	color.r = this->red;
	color.g = this->green;
	color.b = this->blue;
	color.a = this->alpha;

	return color;
}

/// <summary>Converts the managed class reference to a managed equivalent .NET framework <see cref="System::Drawing::Color" /> structure</summary>
/// <returns>A managed .NET framework struct equivalent</returns>
System::Drawing::Color ColorF::ToFramework()
{
	System::Byte r = (System::Byte)(this->red * 255.0F);
	System::Byte g = (System::Byte)(this->green * 255.0F);
	System::Byte b = (System::Byte)(this->blue * 255.0F);
	System::Byte a = (System::Byte)(this->alpha * 255.0F);

	System::Drawing::Color color = System::Drawing::Color::FromArgb(a, r, g, b);

	return color;
}
#pragma endregion