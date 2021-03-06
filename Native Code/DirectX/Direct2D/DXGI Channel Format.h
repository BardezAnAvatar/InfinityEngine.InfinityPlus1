

#ifndef Bardez_Projects_DirectX_Direct2D_DXGIPixelFormat
#define Bardez_Projects_DirectX_Direct2D_DXGIPixelFormat

using namespace System::ComponentModel;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <remarks>See http://msdn.microsoft.com/en-us/library/bb173059%28VS.85%29.aspx for more detail</remarks>
				public enum struct DXGI_ChannelFormat : System::UInt32
				{
					/// <summary>The format is not known.</summary>
					[DescriptionAttribute("The format is not known.")]
					FORMAT_UNKNOWN                      = 0,

					/// <summary>A four-component, 128-bit typeless format that supports 32 bits per channel including alpha.</summary>
					[DescriptionAttribute("A four-component, 128-bit typeless format that supports 32 bits per channel including alpha.")]
					FORMAT_R32G32B32A32_TYPELESS        = 1,

					/// <summary>A four-component, 128-bit floating-point format that supports 32 bits per channel including alpha.</summary>
					[DescriptionAttribute("A four-component, 128-bit floating-point format that supports 32 bits per channel including alpha.")]
					FORMAT_R32G32B32A32_FLOAT           = 2,

					/// <summary>A four-component, 128-bit unsigned-integer format that supports 32 bits per channel including alpha.</summary>
					[DescriptionAttribute("A four-component, 128-bit unsigned-integer format that supports 32 bits per channel including alpha.")]
					FORMAT_R32G32B32A32_UINT            = 3,

					/// <summary>A four-component, 128-bit signed-integer format that supports 32 bits per channel including alpha.</summary>
					[DescriptionAttribute("A four-component, 128-bit signed-integer format that supports 32 bits per channel including alpha.")]
					FORMAT_R32G32B32A32_SINT            = 4,

					/// <summary>A three-component, 96-bit typeless format that supports 32 bits per color channel.</summary>
					[DescriptionAttribute("A three-component, 96-bit typeless format that supports 32 bits per color channel.")]
					FORMAT_R32G32B32_TYPELESS           = 5,

					/// <summary>A three-component, 96-bit floating-point format that supports 32 bits per color channel.</summary>
					[DescriptionAttribute("A three-component, 96-bit floating-point format that supports 32 bits per color channel.")]
					FORMAT_R32G32B32_FLOAT              = 6,

					/// <summary>A three-component, 96-bit unsigned-integer format that supports 32 bits per color channel.</summary>
					[DescriptionAttribute("A three-component, 96-bit unsigned-integer format that supports 32 bits per color channel.")]
					FORMAT_R32G32B32_UINT               = 7,

					/// <summary>A three-component, 96-bit signed-integer format that supports 32 bits per color channel.</summary>
					[DescriptionAttribute("A three-component, 96-bit signed-integer format that supports 32 bits per color channel.")]
					FORMAT_R32G32B32_SINT               = 8,

					/// <summary>A four-component, 64-bit typeless format that supports 16 bits per channel including alpha.</summary>
					[DescriptionAttribute("A four-component, 64-bit typeless format that supports 16 bits per channel including alpha.")]
					FORMAT_R16G16B16A16_TYPELESS        = 9,

					/// <summary>A four-component, 64-bit floating-point format that supports 16 bits per channel including alpha.</summary>
					[DescriptionAttribute("A four-component, 64-bit floating-point format that supports 16 bits per channel including alpha.")]
					FORMAT_R16G16B16A16_FLOAT           = 10,

					/// <summary>A four-component, 64-bit unsigned-normalized-integer format that supports 16 bits per channel including alpha.</summary>
					[DescriptionAttribute("A four-component, 64-bit unsigned-normalized-integer format that supports 16 bits per channel including alpha.")]
					FORMAT_R16G16B16A16_UNORM           = 11,

					/// <summary>A four-component, 64-bit unsigned-integer format that supports 16 bits per channel including alpha.</summary>
					[DescriptionAttribute("A four-component, 64-bit unsigned-integer format that supports 16 bits per channel including alpha.")]
					FORMAT_R16G16B16A16_UINT            = 12,

					/// <summary>A four-component, 64-bit signed-normalized-integer format that supports 16 bits per channel including alpha.</summary>
					[DescriptionAttribute("A four-component, 64-bit signed-normalized-integer format that supports 16 bits per channel including alpha.")]
					FORMAT_R16G16B16A16_SNORM           = 13,

					/// <summary>A four-component, 64-bit signed-integer format that supports 16 bits per channel including alpha.</summary>
					[DescriptionAttribute("A four-component, 64-bit signed-integer format that supports 16 bits per channel including alpha.")]
					FORMAT_R16G16B16A16_SINT            = 14,

					/// <summary>A two-component, 64-bit typeless format that supports 32 bits for the red channel and 32 bits for the green channel.</summary>
					[DescriptionAttribute("A two-component, 64-bit typeless format that supports 32 bits for the red channel and 32 bits for the green channel.")]
					FORMAT_R32G32_TYPELESS              = 15,

					/// <summary>A two-component, 64-bit floating-point format that supports 32 bits for the red channel and 32 bits for the green channel.</summary>
					[DescriptionAttribute("A two-component, 64-bit floating-point format that supports 32 bits for the red channel and 32 bits for the green channel.")]
					FORMAT_R32G32_FLOAT                 = 16,

					/// <summary>A two-component, 64-bit unsigned-integer format that supports 32 bits for the red channel and 32 bits for the green channel.</summary>
					[DescriptionAttribute("A two-component, 64-bit unsigned-integer format that supports 32 bits for the red channel and 32 bits for the green channel.")]
					FORMAT_R32G32_UINT                  = 17,

					/// <summary>A two-component, 64-bit signed-integer format that supports 32 bits for the red channel and 32 bits for the green channel.</summary>
					[DescriptionAttribute("A two-component, 64-bit signed-integer format that supports 32 bits for the red channel and 32 bits for the green channel.")]
					FORMAT_R32G32_SINT                  = 18,

					/// <summary>A two-component, 64-bit typeless format that supports 32 bits for the red channel, 8 bits for the green channel, and 24 bits are unused.</summary>
					[DescriptionAttribute("A two-component, 64-bit typeless format that supports 32 bits for the red channel, 8 bits for the green channel, and 24 bits are unused.")]
					FORMAT_R32G8X24_TYPELESS            = 19,

					/// <summary>A 32-bit floating-point component, and two unsigned-integer components (with an additional 32 bits). This format supports 32-bit depth, 8-bit stencil, and 24 bits are unused.</summary>
					[DescriptionAttribute("A 32-bit floating-point component, and two unsigned-integer components (with an additional 32 bits). This format supports 32-bit depth, 8-bit stencil, and 24 bits are unused.")]
					FORMAT_D32_FLOAT_S8X24_UINT         = 20,

					/// <summary>A 32-bit floating-point component, and two typeless components (with an additional 32 bits). This format supports 32-bit red channel, 8 bits are unused, and 24 bits are unused.</summary>
					[DescriptionAttribute("A 32-bit floating-point component, and two typeless components (with an additional 32 bits). This format supports 32-bit red channel, 8 bits are unused, and 24 bits are unused.")]
					FORMAT_R32_FLOAT_X8X24_TYPELESS     = 21,

					/// <summary>A 32-bit typeless component, and two unsigned-integer components (with an additional 32 bits). This format has 32 bits unused, 8 bits for green channel, and 24 bits are unused.</summary>
					[DescriptionAttribute("A 32-bit typeless component, and two unsigned-integer components (with an additional 32 bits). This format has 32 bits unused, 8 bits for green channel, and 24 bits are unused.")]
					FORMAT_X32_TYPELESS_G8X24_UINT      = 22,

					/// <summary>A four-component, 32-bit typeless format that supports 10 bits for each color and 2 bits for alpha.</summary>
					[DescriptionAttribute("A four-component, 32-bit typeless format that supports 10 bits for each color and 2 bits for alpha.")]
					FORMAT_R10G10B10A2_TYPELESS         = 23,

					/// <summary>A four-component, 32-bit unsigned-normalized-integer format that supports 10 bits for each color and 2 bits for alpha.</summary>
					[DescriptionAttribute("A four-component, 32-bit unsigned-normalized-integer format that supports 10 bits for each color and 2 bits for alpha.")]
					FORMAT_R10G10B10A2_UNORM            = 24,

					/// <summary>A four-component, 32-bit unsigned-integer format that supports 10 bits for each color and 2 bits for alpha.</summary>
					[DescriptionAttribute("A four-component, 32-bit unsigned-integer format that supports 10 bits for each color and 2 bits for alpha.")]
					FORMAT_R10G10B10A2_UINT             = 25,

					/// <summary>Three partial-precision floating-point numbers encoded into a single 32-bit value (a variant of s10e5, which is sign bit, 10-bit mantissa, and 5-bit biased (15) exponent). See reference page for more details.</summary>
					[DescriptionAttribute("Three partial-precision floating-point numbers encoded into a single 32-bit value (a variant of s10e5, which is sign bit, 10-bit mantissa, and 5-bit biased (15) exponent). See reference page for more details.")]
					FORMAT_R11G11B10_FLOAT              = 26,

					/// <summary>A four-component, 32-bit typeless format that supports 8 bits per channel including alpha.</summary>
					[DescriptionAttribute("A four-component, 32-bit typeless format that supports 8 bits per channel including alpha.")]
					FORMAT_R8G8B8A8_TYPELESS            = 27,

					/// <summary>A four-component, 32-bit unsigned-normalized-integer format that supports 8 bits per channel including alpha.</summary>
					[DescriptionAttribute("A four-component, 32-bit unsigned-normalized-integer format that supports 8 bits per channel including alpha.")]
					FORMAT_R8G8B8A8_UNORM               = 28,

					/// <summary>A four-component, 32-bit unsigned-normalized integer sRGB format that supports 8 bits per channel including alpha.</summary>
					[DescriptionAttribute("A four-component, 32-bit unsigned-normalized integer sRGB format that supports 8 bits per channel including alpha.")]
					FORMAT_R8G8B8A8_UNORM_SRGB          = 29,

					/// <summary>A four-component, 32-bit unsigned-integer format that supports 8 bits per channel including alpha.</summary>
					[DescriptionAttribute("A four-component, 32-bit unsigned-integer format that supports 8 bits per channel including alpha.")]
					FORMAT_R8G8B8A8_UINT                = 30,

					/// <summary>A four-component, 32-bit signed-normalized-integer format that supports 8 bits per channel including alpha.</summary>
					[DescriptionAttribute("A four-component, 32-bit signed-normalized-integer format that supports 8 bits per channel including alpha.")]
					FORMAT_R8G8B8A8_SNORM               = 31,

					/// <summary>A four-component, 32-bit signed-integer format that supports 8 bits per channel including alpha.</summary>
					[DescriptionAttribute("A four-component, 32-bit signed-integer format that supports 8 bits per channel including alpha.")]
					FORMAT_R8G8B8A8_SINT                = 32,

					/// <summary>A two-component, 32-bit typeless format that supports 16 bits for the red channel and 16 bits for the green channel.</summary>
					[DescriptionAttribute("A two-component, 32-bit typeless format that supports 16 bits for the red channel and 16 bits for the green channel.")]
					FORMAT_R16G16_TYPELESS              = 33,

					/// <summary>A two-component, 32-bit floating-point format that supports 16 bits for the red channel and 16 bits for the green channel.</summary>
					[DescriptionAttribute("A two-component, 32-bit floating-point format that supports 16 bits for the red channel and 16 bits for the green channel.")]
					FORMAT_R16G16_FLOAT                 = 34,

					/// <summary>A two-component, 32-bit unsigned-normalized-integer format that supports 16 bits each for the green and red channels.</summary>
					[DescriptionAttribute("A two-component, 32-bit unsigned-normalized-integer format that supports 16 bits each for the green and red channels.")]
					FORMAT_R16G16_UNORM                 = 35,

					/// <summary>A two-component, 32-bit unsigned-integer format that supports 16 bits for the red channel and 16 bits for the green channel.</summary>
					[DescriptionAttribute("A two-component, 32-bit unsigned-integer format that supports 16 bits for the red channel and 16 bits for the green channel.")]
					FORMAT_R16G16_UINT                  = 36,

					/// <summary>A two-component, 32-bit signed-normalized-integer format that supports 16 bits for the red channel and 16 bits for the green channel.</summary>
					[DescriptionAttribute("A two-component, 32-bit signed-normalized-integer format that supports 16 bits for the red channel and 16 bits for the green channel.")]
					FORMAT_R16G16_SNORM                 = 37,

					/// <summary>A two-component, 32-bit signed-integer format that supports 16 bits for the red channel and 16 bits for the green channel.</summary>
					[DescriptionAttribute("A two-component, 32-bit signed-integer format that supports 16 bits for the red channel and 16 bits for the green channel.")]
					FORMAT_R16G16_SINT                  = 38,

					/// <summary>A single-component, 32-bit typeless format that supports 32 bits for the red channel.</summary>
					[DescriptionAttribute("A single-component, 32-bit typeless format that supports 32 bits for the red channel.")]
					FORMAT_R32_TYPELESS                 = 39,

					/// <summary>A single-component, 32-bit floating-point format that supports 32 bits for depth.</summary>
					[DescriptionAttribute("A single-component, 32-bit floating-point format that supports 32 bits for depth.")]
					FORMAT_D32_FLOAT                    = 40,

					/// <summary>A single-component, 32-bit floating-point format that supports 32 bits for the red channel.</summary>
					[DescriptionAttribute("A single-component, 32-bit floating-point format that supports 32 bits for the red channel.")]
					FORMAT_R32_FLOAT                    = 41,

					/// <summary>A single-component, 32-bit unsigned-integer format that supports 32 bits for the red channel.</summary>
					[DescriptionAttribute("A single-component, 32-bit unsigned-integer format that supports 32 bits for the red channel.")]
					FORMAT_R32_UINT                     = 42,

					/// <summary>A single-component, 32-bit signed-integer format that supports 32 bits for the red channel.</summary>
					[DescriptionAttribute("A single-component, 32-bit signed-integer format that supports 32 bits for the red channel.")]
					FORMAT_R32_SINT                     = 43,

					/// <summary>A two-component, 32-bit typeless format that supports 24 bits for the red channel and 8 bits for the green channel.</summary>
					[DescriptionAttribute("A two-component, 32-bit typeless format that supports 24 bits for the red channel and 8 bits for the green channel.")]
					FORMAT_R24G8_TYPELESS               = 44,

					/// <summary>A 32-bit z-buffer format that supports 24 bits for depth and 8 bits for stencil.</summary>
					[DescriptionAttribute("A 32-bit z-buffer format that supports 24 bits for depth and 8 bits for stencil.")]
					FORMAT_D24_UNORM_S8_UINT            = 45,

					/// <summary>A 32-bit format, that contains a 24 bit, single-component, unsigned-normalized integer, with an additional typeless 8 bits. This format has 24 bits red channel and 8 bits unused.</summary>
					[DescriptionAttribute("A 32-bit format, that contains a 24 bit, single-component, unsigned-normalized integer, with an additional typeless 8 bits. This format has 24 bits red channel and 8 bits unused.")]
					FORMAT_R24_UNORM_X8_TYPELESS        = 46,

					/// <summary>A 32-bit format, that contains a 24 bit, single-component, typeless format, with an additional 8 bit unsigned integer component. This format has 24 bits unused and 8 bits green channel.</summary>
					[DescriptionAttribute("A 32-bit format, that contains a 24 bit, single-component, typeless format, with an additional 8 bit unsigned integer component. This format has 24 bits unused and 8 bits green channel.")]
					FORMAT_X24_TYPELESS_G8_UINT         = 47,

					/// <summary>A two-component, 16-bit typeless format that supports 8 bits for the red channel and 8 bits for the green channel.</summary>
					[DescriptionAttribute("A two-component, 16-bit typeless format that supports 8 bits for the red channel and 8 bits for the green channel.")]
					FORMAT_R8G8_TYPELESS                = 48,

					/// <summary>A two-component, 16-bit unsigned-normalized-integer format that supports 8 bits for the red channel and 8 bits for the green channel.</summary>
					[DescriptionAttribute("A two-component, 16-bit unsigned-normalized-integer format that supports 8 bits for the red channel and 8 bits for the green channel.")]
					FORMAT_R8G8_UNORM                   = 49,

					/// <summary>A two-component, 16-bit unsigned-integer format that supports 8 bits for the red channel and 8 bits for the green channel.</summary>
					[DescriptionAttribute("A two-component, 16-bit unsigned-integer format that supports 8 bits for the red channel and 8 bits for the green channel.")]
					FORMAT_R8G8_UINT                    = 50,

					/// <summary>A two-component, 16-bit signed-normalized-integer format that supports 8 bits for the red channel and 8 bits for the green channel.</summary>
					[DescriptionAttribute("A two-component, 16-bit signed-normalized-integer format that supports 8 bits for the red channel and 8 bits for the green channel.")]
					FORMAT_R8G8_SNORM                   = 51,

					/// <summary>A two-component, 16-bit signed-integer format that supports 8 bits for the red channel and 8 bits for the green channel.</summary>
					[DescriptionAttribute("A two-component, 16-bit signed-integer format that supports 8 bits for the red channel and 8 bits for the green channel.")]
					FORMAT_R8G8_SINT                    = 52,

					/// <summary>A single-component, 16-bit typeless format that supports 16 bits for the red channel.</summary>
					[DescriptionAttribute("A single-component, 16-bit typeless format that supports 16 bits for the red channel.")]
					FORMAT_R16_TYPELESS                 = 53,

					/// <summary>A single-component, 16-bit floating-point format that supports 16 bits for the red channel.</summary>
					[DescriptionAttribute("A single-component, 16-bit floating-point format that supports 16 bits for the red channel.")]
					FORMAT_R16_FLOAT                    = 54,

					/// <summary>A single-component, 16-bit unsigned-normalized-integer format that supports 16 bits for depth.</summary>
					[DescriptionAttribute("A single-component, 16-bit unsigned-normalized-integer format that supports 16 bits for depth.")]
					FORMAT_D16_UNORM                    = 55,

					/// <summary>A single-component, 16-bit unsigned-normalized-integer format that supports 16 bits for the red channel.</summary>
					[DescriptionAttribute("A single-component, 16-bit unsigned-normalized-integer format that supports 16 bits for the red channel.")]
					FORMAT_R16_UNORM                    = 56,

					/// <summary>A single-component, 16-bit unsigned-integer format that supports 16 bits for the red channel.</summary>
					[DescriptionAttribute("A single-component, 16-bit unsigned-integer format that supports 16 bits for the red channel.")]
					FORMAT_R16_UINT                     = 57,

					/// <summary>A single-component, 16-bit signed-normalized-integer format that supports 16 bits for the red channel.</summary>
					[DescriptionAttribute("A single-component, 16-bit signed-normalized-integer format that supports 16 bits for the red channel.")]
					FORMAT_R16_SNORM                    = 58,

					/// <summary>A single-component, 16-bit signed-integer format that supports 16 bits for the red channel.</summary>
					[DescriptionAttribute("A single-component, 16-bit signed-integer format that supports 16 bits for the red channel.")]
					FORMAT_R16_SINT                     = 59,

					/// <summary>A single-component, 8-bit typeless format that supports 8 bits for the red channel.</summary>
					[DescriptionAttribute("A single-component, 8-bit typeless format that supports 8 bits for the red channel.")]
					FORMAT_R8_TYPELESS                  = 60,

					/// <summary>A single-component, 8-bit unsigned-normalized-integer format that supports 8 bits for the red channel.</summary>
					[DescriptionAttribute("A single-component, 8-bit unsigned-normalized-integer format that supports 8 bits for the red channel.")]
					FORMAT_R8_UNORM                     = 61,

					/// <summary>A single-component, 8-bit unsigned-integer format that supports 8 bits for the red channel.</summary>
					[DescriptionAttribute("A single-component, 8-bit unsigned-integer format that supports 8 bits for the red channel.")]
					FORMAT_R8_UINT                      = 62,

					/// <summary>A single-component, 8-bit signed-normalized-integer format that supports 8 bits for the red channel.</summary>
					[DescriptionAttribute("A single-component, 8-bit signed-normalized-integer format that supports 8 bits for the red channel.")]
					FORMAT_R8_SNORM                     = 63,

					/// <summary>A single-component, 8-bit signed-integer format that supports 8 bits for the red channel.</summary>
					[DescriptionAttribute("A single-component, 8-bit signed-integer format that supports 8 bits for the red channel.")]
					FORMAT_R8_SINT                      = 64,

					/// <summary>A single-component, 8-bit unsigned-normalized-integer format for alpha only.</summary>
					[DescriptionAttribute("A single-component, 8-bit unsigned-normalized-integer format for alpha only.")]
					FORMAT_A8_UNORM                     = 65,

					/// <summary>A single-component, 1-bit unsigned-normalized integer format that supports 1 bit for the red channel.</summary>
					[DescriptionAttribute("A single-component, 1-bit unsigned-normalized integer format that supports 1 bit for the red channel.")]
					FORMAT_R1_UNORM                     = 66,

					/// <summary>Three partial-precision floating-point numbers encoded into a single 32-bit value all sharing the same 5-bit exponent (variant of s10e5, which is sign bit, 10-bit mantissa, and 5-bit biased (15) exponent). See reference page for more details.</summary>
					[DescriptionAttribute("Three partial-precision floating-point numbers encoded into a single 32-bit value all sharing the same 5-bit exponent (variant of s10e5, which is sign bit, 10-bit mantissa, and 5-bit biased (15) exponent). See reference page for more details.")]
					FORMAT_R9G9B9E5_SHAREDEXP           = 67,

					/// <summary>A four-component, 32-bit unsigned-normalized-integer format. This packed RGB format is analogous to the UYVY format. See reference page for more details.</summary>
					[DescriptionAttribute("A four-component, 32-bit unsigned-normalized-integer format. This packed RGB format is analogous to the UYVY format. See reference page for more details.")]
					FORMAT_R8G8_B8G8_UNORM              = 68,

					/// <summary>A four-component, 32-bit unsigned-normalized-integer format. This packed RGB format is analogous to the YUY2 format. See reference page for more details.</summary>
					[DescriptionAttribute("A four-component, 32-bit unsigned-normalized-integer format. This packed RGB format is analogous to the YUY2 format. See reference page for more details.")]
					FORMAT_G8R8_G8B8_UNORM              = 69,

					/// <summary>Four-component typeless block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11</summary>
					[DescriptionAttribute("Four-component typeless block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11")]
					FORMAT_BC1_TYPELESS                 = 70,

					/// <summary>Four-component block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.</summary>
					[DescriptionAttribute("Four-component block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.")]
					FORMAT_BC1_UNORM                    = 71,

					/// <summary>Four-component block-compression format for sRGB data. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.</summary>
					[DescriptionAttribute("Four-component block-compression format for sRGB data. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.")]
					FORMAT_BC1_UNORM_SRGB               = 72,

					/// <summary>Four-component typeless block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.</summary>
					[DescriptionAttribute("Four-component typeless block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.")]
					FORMAT_BC2_TYPELESS                 = 73,

					/// <summary>Four-component block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.</summary>
					[DescriptionAttribute("Four-component block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.")]
					FORMAT_BC2_UNORM                    = 74,

					/// <summary>Four-component block-compression format for sRGB data. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.</summary>
					[DescriptionAttribute("Four-component block-compression format for sRGB data. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.")]
					FORMAT_BC2_UNORM_SRGB               = 75,

					/// <summary>Four-component typeless block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.</summary>
					[DescriptionAttribute("Four-component typeless block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.")]
					FORMAT_BC3_TYPELESS                 = 76,

					/// <summary>Four-component block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.</summary>
					[DescriptionAttribute("Four-component block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.")]
					FORMAT_BC3_UNORM                    = 77,

					/// <summary>Four-component block-compression format for sRGB data. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.</summary>
					[DescriptionAttribute("Four-component block-compression format for sRGB data. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.")]
					FORMAT_BC3_UNORM_SRGB               = 78,

					/// <summary>One-component typeless block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.</summary>
					[DescriptionAttribute("One-component typeless block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.")]
					FORMAT_BC4_TYPELESS                 = 79,

					/// <summary>One-component block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.</summary>
					[DescriptionAttribute("One-component block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.")]
					FORMAT_BC4_UNORM                    = 80,

					/// <summary>One-component block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.</summary>
					[DescriptionAttribute("One-component block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.")]
					FORMAT_BC4_SNORM                    = 81,

					/// <summary>Two-component typeless block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.</summary>
					[DescriptionAttribute("Two-component typeless block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.")]
					FORMAT_BC5_TYPELESS                 = 82,

					/// <summary>Two-component block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.</summary>
					[DescriptionAttribute("Two-component block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.")]
					FORMAT_BC5_UNORM                    = 83,

					/// <summary>Two-component block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.</summary>
					[DescriptionAttribute("Two-component block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.")]
					FORMAT_BC5_SNORM                    = 84,

					/// <summary>A three-component, 16-bit unsigned-normalized-integer format that supports 5 bits for blue, 6 bits for green, and 5 bits for red.</summary>
					[DescriptionAttribute("A three-component, 16-bit unsigned-normalized-integer format that supports 5 bits for blue, 6 bits for green, and 5 bits for red.")]
					FORMAT_B5G6R5_UNORM                 = 85,

					/// <summary>A four-component, 16-bit unsigned-normalized-integer format that supports 5 bits for each color channel and 1-bit alpha.</summary>
					[DescriptionAttribute("A four-component, 16-bit unsigned-normalized-integer format that supports 5 bits for each color channel and 1-bit alpha.")]
					FORMAT_B5G5R5A1_UNORM               = 86,

					/// <summary>A four-component, 32-bit unsigned-normalized-integer format that supports 8 bits for each color channel and 8-bit alpha.</summary>
					[DescriptionAttribute("A four-component, 32-bit unsigned-normalized-integer format that supports 8 bits for each color channel and 8-bit alpha.")]
					FORMAT_B8G8R8A8_UNORM               = 87,

					/// <summary>A four-component, 32-bit unsigned-normalized-integer format that supports 8 bits for each color channel and 8 bits unused.</summary>
					[DescriptionAttribute("A four-component, 32-bit unsigned-normalized-integer format that supports 8 bits for each color channel and 8 bits unused.")]
					FORMAT_B8G8R8X8_UNORM               = 88,

					/// <summary>A four-component, 32-bit 2.8-biased fixed-point format that supports 10 bits for each color channel and 2-bit alpha.</summary>
					[DescriptionAttribute("A four-component, 32-bit 2.8-biased fixed-point format that supports 10 bits for each color channel and 2-bit alpha.")]
					FORMAT_R10G10B10_XR_BIAS_A2_UNORM   = 89,

					/// <summary>A four-component, 32-bit typeless format that supports 8 bits for each channel including alpha.</summary>
					[DescriptionAttribute("A four-component, 32-bit typeless format that supports 8 bits for each channel including alpha.")]
					FORMAT_B8G8R8A8_TYPELESS            = 90,

					/// <summary>A four-component, 32-bit unsigned-normalized standard RGB format that supports 8 bits for each channel including alpha.</summary>
					[DescriptionAttribute("A four-component, 32-bit unsigned-normalized standard RGB format that supports 8 bits for each channel including alpha.")]
					FORMAT_B8G8R8A8_UNORM_SRGB          = 91,

					/// <summary>A four-component, 32-bit typeless format that supports 8 bits for each color channel, and 8 bits are unused.</summary>
					[DescriptionAttribute("A four-component, 32-bit typeless format that supports 8 bits for each color channel, and 8 bits are unused.")]
					FORMAT_B8G8R8X8_TYPELESS            = 92,

					/// <summary>A four-component, 32-bit unsigned-normalized standard RGB format that supports 8 bits for each color channel, and 8 bits are unused.</summary>
					[DescriptionAttribute("A four-component, 32-bit unsigned-normalized standard RGB format that supports 8 bits for each color channel, and 8 bits are unused.")]
					FORMAT_B8G8R8X8_UNORM_SRGB          = 93,

					/// <summary>A typeless block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.</summary>
					[DescriptionAttribute("A typeless block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.")]
					FORMAT_BC6H_TYPELESS                = 94,

					/// <summary>A block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.</summary>
					[DescriptionAttribute("A block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.")]
					FORMAT_BC6H_UF16                    = 95,

					/// <summary>A block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.</summary>
					[DescriptionAttribute("A block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.")]
					FORMAT_BC6H_SF16                    = 96,

					/// <summary>A typeless block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.</summary>
					[DescriptionAttribute("A typeless block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.")]
					FORMAT_BC7_TYPELESS                 = 97,

					/// <summary>A block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.</summary>
					[DescriptionAttribute("A block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.")]
					FORMAT_BC7_UNORM                    = 98,

					/// <summary>A block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.</summary>
					[DescriptionAttribute("A block-compression format. For information about block-compression formats, see: Texture Block Compression in Direct3D 11.")]
					FORMAT_BC7_UNORM_SRGB               = 99,

					/// <summary>Most common YUV 4:4:4 video resource format. Valid view formats for this video resource format are DXGI_FORMAT_R8G8B8A8_UNORM and DXGI_FORMAT_R8G8B8A8_UINT.  See reference page for more details.</summary>
					[DescriptionAttribute("Most common YUV 4:4:4 video resource format. Valid view formats for this video resource format are DXGI_FORMAT_R8G8B8A8_UNORM and DXGI_FORMAT_R8G8B8A8_UINT.  See reference page for more details.")]
					FORMAT_AYUV                         = 100,

					/// <summary>10-bit per channel packed YUV 4:4:4 video resource format. Valid view formats for this video resource format are DXGI_FORMAT_R10G10B10A2_UNORM and DXGI_FORMAT_R10G10B10A2_UINT. See reference page for more details.</summary>
					[DescriptionAttribute("10-bit per channel packed YUV 4:4:4 video resource format. Valid view formats for this video resource format are DXGI_FORMAT_R10G10B10A2_UNORM and DXGI_FORMAT_R10G10B10A2_UINT. See reference page for more details.")]
					FORMAT_Y410                         = 101,

					/// <summary>16-bit per channel packed YUV 4:4:4 video resource format. Valid view formats for this video resource format are DXGI_FORMAT_R16G16B16A16_UNORM and DXGI_FORMAT_R16G16B16A16_UINT. See reference page for more details.</summary>
					[DescriptionAttribute("16-bit per channel packed YUV 4:4:4 video resource format. Valid view formats for this video resource format are DXGI_FORMAT_R16G16B16A16_UNORM and DXGI_FORMAT_R16G16B16A16_UINT. See reference page for more details.")]
					FORMAT_Y416                         = 102,

					/// <summary>Most common YUV 4:2:0 video resource format. Supported view types are SRV, RTV, and UAV. See reference page for more details.</summary>
					[DescriptionAttribute("Most common YUV 4:2:0 video resource format. Supported view types are SRV, RTV, and UAV. See reference page for more details.")]
					FORMAT_NV12                         = 103,

					/// <summary>10-bit per channel planar YUV 4:2:0 video resource format. Valid luminance data view formats for this video resource format are DXGI_FORMAT_R16_UNORM and DXGI_FORMAT_R16_UINT. See reference page for more details.</summary>
					[DescriptionAttribute("10-bit per channel planar YUV 4:2:0 video resource format. Valid luminance data view formats for this video resource format are DXGI_FORMAT_R16_UNORM and DXGI_FORMAT_R16_UINT. See reference page for more details.")]
					FORMAT_P010                         = 104,

					/// <summary>16-bit per channel planar YUV 4:2:0 video resource format. Valid luminance data view formats for this video resource format are DXGI_FORMAT_R16_UNORM and DXGI_FORMAT_R16_UINT. See reference page for more details.</summary>
					[DescriptionAttribute("16-bit per channel planar YUV 4:2:0 video resource format. Valid luminance data view formats for this video resource format are DXGI_FORMAT_R16_UNORM and DXGI_FORMAT_R16_UINT. See reference page for more details.")]
					FORMAT_P016                         = 105,

					/// <summary>8-bit per channel planar YUV 4:2:0 video resource format. See reference page for more details.</summary>
					[DescriptionAttribute("8-bit per channel planar YUV 4:2:0 video resource format. See reference page for more details.")]
					FORMAT_420_OPAQUE                   = 106,

					/// <summary>Most common YUV 4:2:2 video resource format. Valid view formats for this video resource format are DXGI_FORMAT_R8G8B8A8_UNORM and DXGI_FORMAT_R8G8B8A8_UINT. See reference page for more details.</summary>
					[DescriptionAttribute("Most common YUV 4:2:2 video resource format. Valid view formats for this video resource format are DXGI_FORMAT_R8G8B8A8_UNORM and DXGI_FORMAT_R8G8B8A8_UINT. See reference page for more details.")]
					FORMAT_YUY2                         = 107,

					/// <summary>10-bit per channel packed YUV 4:2:2 video resource format. Valid view formats for this video resource format are DXGI_FORMAT_R16G16B16A16_UNORM and DXGI_FORMAT_R16G16B16A16_UINT. See reference page for more details.</summary>
					[DescriptionAttribute("10-bit per channel packed YUV 4:2:2 video resource format. Valid view formats for this video resource format are DXGI_FORMAT_R16G16B16A16_UNORM and DXGI_FORMAT_R16G16B16A16_UINT. See reference page for more details.")]
					FORMAT_Y210                         = 108,

					/// <summary>16-bit per channel packed YUV 4:2:2 video resource format. Valid view formats for this video resource format are DXGI_FORMAT_R16G16B16A16_UNORM and DXGI_FORMAT_R16G16B16A16_UINT. See reference page for more details.</summary>
					[DescriptionAttribute("16-bit per channel packed YUV 4:2:2 video resource format. Valid view formats for this video resource format are DXGI_FORMAT_R16G16B16A16_UNORM and DXGI_FORMAT_R16G16B16A16_UINT. See reference page for more details.")]
					FORMAT_Y216                         = 109,

					/// <summary>Most common planar YUV 4:1:1 video resource format. Supported view types are SRV, RTV, and UAV. See reference page for more details.</summary>
					[DescriptionAttribute("Most common planar YUV 4:1:1 video resource format. Supported view types are SRV, RTV, and UAV. See reference page for more details.")]
					FORMAT_NV11                         = 110,

					/// <summary>4-bit palletized YUV format that is commonly used for DVD subpicture.</summary>
					[DescriptionAttribute("4-bit palletized YUV format that is commonly used for DVD subpicture.")]
					FORMAT_AI44                         = 111,

					/// <summary>4-bit palletized YUV format that is commonly used for DVD subpicture.</summary>
					[DescriptionAttribute("4-bit palletized YUV format that is commonly used for DVD subpicture.")]
					FORMAT_IA44                         = 112,

					/// <summary>8-bit palletized format that is used for palletized RGB data when the processor processes ISDB-T data and for palletized YUV data when the processor processes Blu-Ray data.</summary>
					[DescriptionAttribute("8-bit palletized format that is used for palletized RGB data when the processor processes ISDB-T data and for palletized YUV data when the processor processes Blu-Ray data.")]
					FORMAT_P8                           = 113,

					/// <summary>8-bit palletized format with 8 bits of alpha that is used for palletized YUV data when the processor processes Blu-Ray data.</summary>
					[DescriptionAttribute("8-bit palletized format with 8 bits of alpha that is used for palletized YUV data when the processor processes Blu-Ray data.")]
					FORMAT_A8P8                         = 114,

					/// <summary>A four-component, 16-bit unsigned-normalized integer format that supports 4 bits for each channel including alpha.</summary>
					[DescriptionAttribute("A four-component, 16-bit unsigned-normalized integer format that supports 4 bits for each channel including alpha.")]
					FORMAT_B4G4R4A4_UNORM               = 115,

					/// <summary>Forces this enumeration to compile to 32 bits in size. Without this value, some compilers would allow this enumeration to compile to a size other than 32 bits. This value is not used.</summary>
					[DescriptionAttribute("Forces this enumeration to compile to 32 bits in size. Without this value, some compilers would allow this enumeration to compile to a size other than 32 bits. This value is not used.")]
					FORMAT_FORCE_UINT                   = 0xFFFFFFFFU
				};
			}
		}
	}
}


#endif
