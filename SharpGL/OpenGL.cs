//    OpenGL.cs
//    Created by Dave Kerr, 12/08/2003
//    Copyright (c) Dave Kerr 2003
//    http://www.codechamber.com

//	Rules for using OpenGL!
//	There are 600 or so OpenGL functions imported into this class. If you want to use
//	a function, first check it's been wrapped. If it hasn't, wrap it (call PreErrorCheck,
//	call the glFunction, call ErrorCheck) and comment the function fully (i.e with
//	parameters and return values). 
//	Doing this all at once would take hours, so wrap functions as you need them, and comment
//	them well.
//	One last thing, remove all postfixes to the function, i.e glVertex3fv should be wrapped
//	simply as Vertex.

//	After wrapping a GLU function, move it to the wrapped opengl functions region and 
//	put in the right alphabetical order.

//	ErrorCheck and PreErrorCheck do nothing at the moment, add your own code if you want
//	but it tends to slow things down. In the next revision, the error checking will
//	exist for debug mode only.

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

using SharpGL;
using SharpGL.SceneGraph;

namespace SharpGL
{
	/// <summary>
	/// The OpenGL class wraps Suns OpenGL 3D library.
	/// </summary>
	public class OpenGL : Imports
	{
		public OpenGL() {}

		#region The OpenGL constant definitions.

	#region OpenGL Version Identifier
		public  const uint VERSION_1_1 = 1;
		#endregion

	#region AccumOp
		public  uint ACCUM                          = 0x0100;
		public  uint LOAD                           = 0x0101;
		public  uint RETURN                         = 0x0102;
		public  uint MULT                           = 0x0103;
		public  const uint ADD                            = 0x0104;
	#endregion
	#region AlphaFunction
		public  const uint NEVER                          = 0x0200;
		public  const uint LESS                           = 0x0201;
		public  const uint EQUAL                          = 0x0202;
		public  const uint LEQUAL                         = 0x0203;
		public  const uint GREATER                        = 0x0204;
		public  const uint NOTEQUAL                       = 0x0205;
		public  const uint GEQUAL                         = 0x0206;
		public  const uint ALWAYS                         = 0x0207;
	#endregion
	#region AttribMask
		public  const uint CURRENT_BIT                    = 0x00000001;
		public  const uint POINT_BIT                      = 0x00000002;
		public  const uint LINE_BIT                       = 0x00000004;
		public  const uint POLYGON_BIT                    = 0x00000008;
		public  const uint POLYGON_STIPPLE_BIT            = 0x00000010;
		public  const uint PIXEL_MODE_BIT                 = 0x00000020;
		public  const uint LIGHTING_BIT                   = 0x00000040;
		public  const uint FOG_BIT                        = 0x00000080;
		public  const uint DEPTH_BUFFER_BIT               = 0x00000100;
		public  const uint ACCUM_BUFFER_BIT               = 0x00000200;
		public  const uint STENCIL_BUFFER_BIT             = 0x00000400;
		public  const uint VIEWPORT_BIT                   = 0x00000800;
		public  const uint TRANSFORM_BIT                  = 0x00001000;
		public  const uint ENABLE_BIT                     = 0x00002000;
		public  const uint COLOR_BUFFER_BIT               = 0x00004000;
		public  const uint HINT_BIT                       = 0x00008000;
		public  const uint EVAL_BIT                       = 0x00010000;
		public  const uint LIST_BIT                       = 0x00020000;
		public  const uint TEXTURE_BIT                    = 0x00040000;
		public  const uint SCISSOR_BIT                    = 0x00080000;
		public  const uint ALL_ATTRIB_BITS                = 0x000fffff;
	#endregion
	#region BeginMode
		public  const uint POINTS                         = 0x0000;
		public  const uint LINES                          = 0x0001;
		public  const uint LINE_LOOP                      = 0x0002;
		public  const uint LINE_STRIP                     = 0x0003;
		public  const uint TRIANGLES                      = 0x0004;
		public  const uint TRIANGLE_STRIP                 = 0x0005;
		public  const uint TRIANGLE_FAN                   = 0x0006;
		public  const uint QUADS                          = 0x0007;
		public  const uint QUAD_STRIP                     = 0x0008;
		public  const uint POLYGON                        = 0x0009;
	#endregion
	#region BlendingFactorDest
		public  const uint ZERO                           = 0;
		public  const uint ONE                            = 1;
		public  const uint SRC_COLOR                      = 0x0300;
		public  const uint ONE_MINUS_SRC_COLOR            = 0x0301;
		public  const uint SRC_ALPHA                      = 0x0302;
		public  const uint ONE_MINUS_SRC_ALPHA            = 0x0303;
		public  const uint DST_ALPHA                      = 0x0304;
		public  const uint ONE_MINUS_DST_ALPHA            = 0x0305;
	#endregion
	#region BlendingFactorSrc
		public  const uint DST_COLOR                      = 0x0306;
		public  const uint ONE_MINUS_DST_COLOR            = 0x0307;
		public  const uint SRC_ALPHA_SATURATE             = 0x0308;
		#endregion
	#region Boolean
		public  const uint TRUE                           = 1;
		public  const uint FALSE                          = 0;
		#endregion     
	#region ClipPlaneName
		public  const uint CLIP_PLANE0                    = 0x3000;
		public  const uint CLIP_PLANE1                    = 0x3001;
		public  const uint CLIP_PLANE2                    = 0x3002;
		public  const uint CLIP_PLANE3                    = 0x3003;
		public  const uint CLIP_PLANE4                    = 0x3004;
		public  const uint CLIP_PLANE5                    = 0x3005;
	#endregion
	#region DataType
		public  const uint BYTE                           = 0x1400;
		public  const uint UNSIGNED_BYTE                  = 0x1401;
		public  const uint SHORT                          = 0x1402;
		public  const uint UNSIGNED_SHORT                 = 0x1403;
		public  const uint INT                            = 0x1404;
		public  const uint UNSIGNED_INT                   = 0x1405;
		public  const uint FLOAT                          = 0x1406;
		public  const uint GL_2_BYTES                        = 0x1407;
		public  const uint GL_3_BYTES                        = 0x1408;
		public  const uint GL_4_BYTES                        = 0x1409;
		public  const uint DOUBLE                         = 0x140A;
	#endregion
	#region DrawBufferMode
		public  const uint NONE                           = 0;
		public  const uint FRONT_LEFT                     = 0x0400;
		public  const uint FRONT_RIGHT                    = 0x0401;
		public  const uint BACK_LEFT                      = 0x0402;
		public  const uint BACK_RIGHT                     = 0x0403;
		public  const uint FRONT                          = 0x0404;
		public  const uint BACK                           = 0x0405;
		public  const uint LEFT                           = 0x0406;
		public  const uint RIGHT                          = 0x0407;
		public  const uint FRONT_AND_BACK                 = 0x0408;
		public  const uint AUX0                           = 0x0409;
		public  const uint AUX1                           = 0x040A;
		public  const uint AUX2                           = 0x040B;
		public  const uint AUX3                           = 0x040C;
	#endregion
	#region ErrorCode
		public  const uint NO_ERROR                   = 0;
		public  const uint INVALID_ENUM                   = 0x0500;
		public  const uint INVALID_VALUE                  = 0x0501;
		public  const uint INVALID_OPERATION              = 0x0502;
		public  const uint STACK_OVERFLOW                 = 0x0503;
		public  const uint STACK_UNDERFLOW                = 0x0504;
		public  const uint OUT_OF_MEMORY                  = 0x0505;
	#endregion
	#region FeedBackMode
		public  const uint GL_2D                             = 0x0600;
		public  const uint GL_3D                             = 0x0601;
		public  const uint GL_4D_COLOR                       = 0x0602;
		public  const uint GL_3D_COLOR_TEXTURE               = 0x0603;
		public  const uint GL_4D_COLOR_TEXTURE               = 0x0604;
	#endregion
	#region FeedBackToken
		public  const uint PASS_THROUGH_TOKEN             = 0x0700;
		public  const uint POINT_TOKEN                    = 0x0701;
		public  const uint LINE_TOKEN                     = 0x0702;
		public  const uint POLYGON_TOKEN                  = 0x0703;
		public  const uint BITMAP_TOKEN                   = 0x0704;
		public  const uint DRAW_PIXEL_TOKEN               = 0x0705;
		public  const uint COPY_PIXEL_TOKEN               = 0x0706;
		public  const uint LINE_RESET_TOKEN               = 0x0707;
	#endregion
	#region FogMode
		public  const uint EXP                            = 0x0800;
		public  const uint EXP2                           = 0x0801;
	#endregion
	#region FrontFaceDirection
		public  const uint CW                             = 0x0900;
		public  const uint CCW                            = 0x0901;
	#endregion
	#region  GetMapTarget 
		public  const uint COEFF                          = 0x0A00;
		public  const uint ORDER                          = 0x0A01;
		public  const uint DOMAIN                         = 0x0A02;
	#endregion
	#region GetTarget
		public  const uint CURRENT_COLOR                  = 0x0B00;
		public  const uint CURRENT_INDEX                  = 0x0B01;
		public  const uint CURRENT_NORMAL                 = 0x0B02;
		public  const uint CURRENT_TEXTURE_COORDS         = 0x0B03;
		public  const uint CURRENT_RASTER_COLOR           = 0x0B04;
		public  const uint CURRENT_RASTER_INDEX           = 0x0B05;
		public  const uint CURRENT_RASTER_TEXTURE_COORDS  = 0x0B06;
		public  const uint CURRENT_RASTER_POSITION        = 0x0B07;
		public  const uint CURRENT_RASTER_POSITION_VALID  = 0x0B08;
		public  const uint CURRENT_RASTER_DISTANCE        = 0x0B09;
		public  const uint POINT_SMOOTH                   = 0x0B10;
		public  const uint POINT_SIZE                     = 0x0B11;
		public  const uint POINT_SIZE_RANGE               = 0x0B12;
		public  const uint POINT_SIZE_GRANULARITY         = 0x0B13;
		public  const uint LINE_SMOOTH                    = 0x0B20;
		public  const uint LINE_WIDTH                     = 0x0B21;
		public  const uint LINE_WIDTH_RANGE               = 0x0B22;
		public  const uint LINE_WIDTH_GRANULARITY         = 0x0B23;
		public  const uint LINE_STIPPLE                   = 0x0B24;
		public  const uint LINE_STIPPLE_PATTERN           = 0x0B25;
		public  const uint LINE_STIPPLE_REPEAT            = 0x0B26;
		public  const uint LIST_MODE                      = 0x0B30;
		public  const uint MAX_LIST_NESTING               = 0x0B31;
		public  const uint LIST_BASE                      = 0x0B32;
		public  const uint LIST_INDEX                     = 0x0B33;
		public  const uint POLYGON_MODE                   = 0x0B40;
		public  const uint POLYGON_SMOOTH                 = 0x0B41;
		public  const uint POLYGON_STIPPLE                = 0x0B42;
		public  const uint EDGE_FLAG                      = 0x0B43;
		public  const uint CULL_FACE                      = 0x0B44;
		public  const uint CULL_FACE_MODE                 = 0x0B45;
		public  const uint FRONT_FACE                     = 0x0B46;
		public  const uint LIGHTING                       = 0x0B50;
		public  const uint LIGHT_MODEL_LOCAL_VIEWER       = 0x0B51;
		public  const uint LIGHT_MODEL_TWO_SIDE           = 0x0B52;
		public  const uint LIGHT_MODEL_AMBIENT            = 0x0B53;
		public  const uint SHADE_MODEL                    = 0x0B54;
		public  const uint COLOR_MATERIAL_FACE            = 0x0B55;
		public  const uint COLOR_MATERIAL_PARAMETER       = 0x0B56;
		public  const uint COLOR_MATERIAL                 = 0x0B57;
		public  const uint FOG                            = 0x0B60;
		public  const uint FOG_INDEX                      = 0x0B61;
		public  const uint FOG_DENSITY                    = 0x0B62;
		public  const uint FOG_START                      = 0x0B63;
		public  const uint FOG_END                        = 0x0B64;
		public  const uint FOG_MODE                       = 0x0B65;
		public  const uint FOG_COLOR                      = 0x0B66;
		public  const uint DEPTH_RANGE                    = 0x0B70;
		public  const uint DEPTH_TEST                     = 0x0B71;
		public  const uint DEPTH_WRITEMASK                = 0x0B72;
		public  const uint DEPTH_CLEAR_VALUE              = 0x0B73;
		public  const uint DEPTH_FUNC                     = 0x0B74;
		public  const uint ACCUM_CLEAR_VALUE              = 0x0B80;
		public  const uint STENCIL_TEST                   = 0x0B90;
		public  const uint STENCIL_CLEAR_VALUE            = 0x0B91;
		public  const uint STENCIL_FUNC                   = 0x0B92;
		public  const uint STENCIL_VALUE_MASK             = 0x0B93;
		public  const uint STENCIL_FAIL                   = 0x0B94;
		public  const uint STENCIL_PASS_DEPTH_FAIL        = 0x0B95;
		public  const uint STENCIL_PASS_DEPTH_PASS        = 0x0B96;
		public  const uint STENCIL_REF                    = 0x0B97;
		public  const uint STENCIL_WRITEMASK              = 0x0B98;
		public  const uint MATRIX_MODE                    = 0x0BA0;
		public  const uint NORMALIZE                      = 0x0BA1;
		public  const uint VIEWPORT                       = 0x0BA2;
		public  const uint MODELVIEW_STACK_DEPTH          = 0x0BA3;
		public  const uint PROJECTION_STACK_DEPTH         = 0x0BA4;
		public  const uint TEXTURE_STACK_DEPTH            = 0x0BA5;
		public  const uint MODELVIEW_MATRIX               = 0x0BA6;
		public  const uint PROJECTION_MATRIX              = 0x0BA7;
		public  const uint TEXTURE_MATRIX                 = 0x0BA8;
		public  const uint ATTRIB_STACK_DEPTH             = 0x0BB0;
		public  const uint CLIENT_ATTRIB_STACK_DEPTH      = 0x0BB1;
		public  const uint ALPHA_TEST                     = 0x0BC0;
		public  const uint ALPHA_TEST_FUNC                = 0x0BC1;
		public  const uint ALPHA_TEST_REF                 = 0x0BC2;
		public  const uint DITHER                         = 0x0BD0;
		public  const uint BLEND_DST                      = 0x0BE0;
		public  const uint BLEND_SRC                      = 0x0BE1;
		public  const uint BLEND                          = 0x0BE2;
		public  const uint LOGIC_OP_MODE                  = 0x0BF0;
		public  const uint INDEX_LOGIC_OP                 = 0x0BF1;
		public  const uint COLOR_LOGIC_OP                 = 0x0BF2;
		public  const uint AUX_BUFFERS                    = 0x0C00;
		public  const uint DRAW_BUFFER                    = 0x0C01;
		public  const uint READ_BUFFER                    = 0x0C02;
		public  const uint SCISSOR_BOX                    = 0x0C10;
		public  const uint SCISSOR_TEST                   = 0x0C11;
		public  const uint INDEX_CLEAR_VALUE              = 0x0C20;
		public  const uint INDEX_WRITEMASK                = 0x0C21;
		public  const uint COLOR_CLEAR_VALUE              = 0x0C22;
		public  const uint COLOR_WRITEMASK                = 0x0C23;
		public  const uint INDEX_MODE                     = 0x0C30;
		public  const uint RGBA_MODE                      = 0x0C31;
		public  const uint DOUBLEBUFFER                   = 0x0C32;
		public  const uint STEREO                         = 0x0C33;
		public  const uint RENDER_MODE                    = 0x0C40;
		public  const uint PERSPECTIVE_CORRECTION_HINT    = 0x0C50;
		public  const uint POINT_SMOOTH_HINT              = 0x0C51;
		public  const uint LINE_SMOOTH_HINT               = 0x0C52;
		public  const uint POLYGON_SMOOTH_HINT            = 0x0C53;
		public  const uint FOG_HINT                       = 0x0C54;
		public  const uint TEXTURE_GEN_S                  = 0x0C60;
		public  const uint TEXTURE_GEN_T                  = 0x0C61;
		public  const uint TEXTURE_GEN_R                  = 0x0C62;
		public  const uint TEXTURE_GEN_Q                  = 0x0C63;
		public  const uint PIXEL_MAP_I_TO_I               = 0x0C70;
		public  const uint PIXEL_MAP_S_TO_S               = 0x0C71;
		public  const uint PIXEL_MAP_I_TO_R               = 0x0C72;
		public  const uint PIXEL_MAP_I_TO_G               = 0x0C73;
		public  const uint PIXEL_MAP_I_TO_B               = 0x0C74;
		public  const uint PIXEL_MAP_I_TO_A               = 0x0C75;
		public  const uint PIXEL_MAP_R_TO_R               = 0x0C76;
		public  const uint PIXEL_MAP_G_TO_G               = 0x0C77;
		public  const uint PIXEL_MAP_B_TO_B               = 0x0C78;
		public  const uint PIXEL_MAP_A_TO_A               = 0x0C79;
		public  const uint PIXEL_MAP_I_TO_I_SIZE          = 0x0CB0;
		public  const uint PIXEL_MAP_S_TO_S_SIZE          = 0x0CB1;
		public  const uint PIXEL_MAP_I_TO_R_SIZE          = 0x0CB2;
		public  const uint PIXEL_MAP_I_TO_G_SIZE          = 0x0CB3;
		public  const uint PIXEL_MAP_I_TO_B_SIZE          = 0x0CB4;
		public  const uint PIXEL_MAP_I_TO_A_SIZE          = 0x0CB5;
		public  const uint PIXEL_MAP_R_TO_R_SIZE          = 0x0CB6;
		public  const uint PIXEL_MAP_G_TO_G_SIZE          = 0x0CB7;
		public  const uint PIXEL_MAP_B_TO_B_SIZE          = 0x0CB8;
		public  const uint PIXEL_MAP_A_TO_A_SIZE          = 0x0CB9;
		public  const uint UNPACK_SWAP_BYTES              = 0x0CF0;
		public  const uint UNPACK_LSB_FIRST               = 0x0CF1;
		public  const uint UNPACK_ROW_LENGTH              = 0x0CF2;
		public  const uint UNPACK_SKIP_ROWS               = 0x0CF3;
		public  const uint UNPACK_SKIP_PIXELS             = 0x0CF4;
		public  const uint UNPACK_ALIGNMENT               = 0x0CF5;
		public  const uint PACK_SWAP_BYTES                = 0x0D00;
		public  const uint PACK_LSB_FIRST                 = 0x0D01;
		public  const uint PACK_ROW_LENGTH                = 0x0D02;
		public  const uint PACK_SKIP_ROWS                 = 0x0D03;
		public  const uint PACK_SKIP_PIXELS               = 0x0D04;
		public  const uint PACK_ALIGNMENT                 = 0x0D05;
		public  const uint MAP_COLOR                      = 0x0D10;
		public  const uint MAP_STENCIL                    = 0x0D11;
		public  const uint INDEX_SHIFT                    = 0x0D12;
		public  const uint INDEX_OFFSET                   = 0x0D13;
		public  const uint RED_SCALE                      = 0x0D14;
		public  const uint RED_BIAS                       = 0x0D15;
		public  const uint ZOOM_X                         = 0x0D16;
		public  const uint ZOOM_Y                         = 0x0D17;
		public  const uint GREEN_SCALE                    = 0x0D18;
		public  const uint GREEN_BIAS                     = 0x0D19;
		public  const uint BLUE_SCALE                     = 0x0D1A;
		public  const uint BLUE_BIAS                      = 0x0D1B;
		public  const uint ALPHA_SCALE                    = 0x0D1C;
		public  const uint ALPHA_BIAS                     = 0x0D1D;
		public  const uint DEPTH_SCALE                    = 0x0D1E;
		public  const uint DEPTH_BIAS                     = 0x0D1F;
		public  const uint MAX_EVAL_ORDER                 = 0x0D30;
		public  const uint MAX_LIGHTS                     = 0x0D31;
		public  const uint MAX_CLIP_PLANES                = 0x0D32;
		public  const uint MAX_TEXTURE_SIZE               = 0x0D33;
		public  const uint MAX_PIXEL_MAP_TABLE            = 0x0D34;
		public  const uint MAX_ATTRIB_STACK_DEPTH         = 0x0D35;
		public  const uint MAX_MODELVIEW_STACK_DEPTH      = 0x0D36;
		public  const uint MAX_NAME_STACK_DEPTH           = 0x0D37;
		public  const uint MAX_PROJECTION_STACK_DEPTH     = 0x0D38;
		public  const uint MAX_TEXTURE_STACK_DEPTH        = 0x0D39;
		public  const uint MAX_VIEWPORT_DIMS              = 0x0D3A;
		public  const uint MAX_CLIENT_ATTRIB_STACK_DEPTH  = 0x0D3B;
		public  const uint SUBPIXEL_BITS                  = 0x0D50;
		public  const uint INDEX_BITS                     = 0x0D51;
		public  const uint RED_BITS                       = 0x0D52;
		public  const uint GREEN_BITS                     = 0x0D53;
		public  const uint BLUE_BITS                      = 0x0D54;
		public  const uint ALPHA_BITS                     = 0x0D55;
		public  const uint DEPTH_BITS                     = 0x0D56;
		public  const uint STENCIL_BITS                   = 0x0D57;
		public  const uint ACCUM_RED_BITS                 = 0x0D58;
		public  const uint ACCUM_GREEN_BITS               = 0x0D59;
		public  const uint ACCUM_BLUE_BITS                = 0x0D5A;
		public  const uint ACCUM_ALPHA_BITS               = 0x0D5B;
		public  const uint NAME_STACK_DEPTH               = 0x0D70;
		public  const uint AUTO_NORMAL                    = 0x0D80;
		public  const uint MAP1_COLOR_4                   = 0x0D90;
		public  const uint MAP1_INDEX                     = 0x0D91;
		public  const uint MAP1_NORMAL                    = 0x0D92;
		public  const uint MAP1_TEXTURE_COORD_1           = 0x0D93;
		public  const uint MAP1_TEXTURE_COORD_2           = 0x0D94;
		public  const uint MAP1_TEXTURE_COORD_3           = 0x0D95;
		public  const uint MAP1_TEXTURE_COORD_4           = 0x0D96;
		public  const uint MAP1_VERTEX_3                  = 0x0D97;
		public  const uint MAP1_VERTEX_4                  = 0x0D98;
		public  const uint MAP2_COLOR_4                   = 0x0DB0;
		public  const uint MAP2_INDEX                     = 0x0DB1;
		public  const uint MAP2_NORMAL                    = 0x0DB2;
		public  const uint MAP2_TEXTURE_COORD_1           = 0x0DB3;
		public  const uint MAP2_TEXTURE_COORD_2           = 0x0DB4;
		public  const uint MAP2_TEXTURE_COORD_3           = 0x0DB5;
		public  const uint MAP2_TEXTURE_COORD_4           = 0x0DB6;
		public  const uint MAP2_VERTEX_3                  = 0x0DB7;
		public  const uint MAP2_VERTEX_4                  = 0x0DB8;
		public  const uint MAP1_GRID_DOMAIN               = 0x0DD0;
		public  const uint MAP1_GRID_SEGMENTS             = 0x0DD1;
		public  const uint MAP2_GRID_DOMAIN               = 0x0DD2;
		public  const uint MAP2_GRID_SEGMENTS             = 0x0DD3;
		public  const uint TEXTURE_1D                     = 0x0DE0;
		public  const uint TEXTURE_2D                     = 0x0DE1;
		public  const uint FEEDBACK_BUFFER_POINTER        = 0x0DF0;
		public  const uint FEEDBACK_BUFFER_SIZE           = 0x0DF1;
		public  const uint FEEDBACK_BUFFER_TYPE           = 0x0DF2;
		public  const uint SELECTION_BUFFER_POINTER       = 0x0DF3;
		public  const uint SELECTION_BUFFER_SIZE          = 0x0DF4;
	#endregion
	#region GetTextureParameter
		public  const uint TEXTURE_WIDTH                  = 0x1000;
		public  const uint TEXTURE_HEIGHT                 = 0x1001;
		public  const uint TEXTURE_INTERNAL_FORMAT        = 0x1003;
		public  const uint TEXTURE_BORDER_COLOR           = 0x1004;
		public  const uint TEXTURE_BORDER                 = 0x1005;
	#endregion
	#region HintMode
		public  const uint DONT_CARE                      = 0x1100;
		public  const uint FASTEST                        = 0x1101;
		public  const uint NICEST                         = 0x1102;
	#endregion
	#region LightName
		public  const uint LIGHT0                         = 0x4000;
		public  const uint LIGHT1                         = 0x4001;
		public  const uint LIGHT2                         = 0x4002;
		public  const uint LIGHT3                         = 0x4003;
		public  const uint LIGHT4                         = 0x4004;
		public  const uint LIGHT5                         = 0x4005;
		public  const uint LIGHT6                         = 0x4006;
		public  const uint LIGHT7                         = 0x4007;
	#endregion
	#region LightParameter
		public  const uint AMBIENT                        = 0x1200;
		public  const uint DIFFUSE                        = 0x1201;
		public  const uint SPECULAR                       = 0x1202;
		public  const uint POSITION                       = 0x1203;
		public  const uint SPOT_DIRECTION                 = 0x1204;
		public  const uint SPOT_EXPONENT                  = 0x1205;
		public  const uint SPOT_CUTOFF                    = 0x1206;
		public  const uint CONSTANT_ATTENUATION           = 0x1207;
		public  const uint LINEAR_ATTENUATION             = 0x1208;
		public  const uint QUADRATIC_ATTENUATION          = 0x1209;
	#endregion
	#region ListMode
		public  const uint COMPILE                        = 0x1300;
		public  const uint COMPILE_AND_EXECUTE            = 0x1301;
	#endregion
	#region LogicOp
		public  const uint CLEAR                          = 0x1500;
		public  const uint AND                            = 0x1501;
		public  const uint AND_REVERSE                    = 0x1502;
		public  const uint COPY                           = 0x1503;
		public  const uint AND_INVERTED                   = 0x1504;
		public  const uint NOOP                           = 0x1505;
		public  const uint XOR                            = 0x1506;
		public  const uint OR                             = 0x1507;
		public  const uint NOR                            = 0x1508;
		public  const uint EQUIV                          = 0x1509;
		public  const uint INVERT                         = 0x150A;
		public  const uint OR_REVERSE                     = 0x150B;
		public  const uint COPY_INVERTED                  = 0x150C;
		public  const uint OR_INVERTED                    = 0x150D;
		public  const uint NAND                           = 0x150E;
		public  const uint SET                            = 0x150F;
	#endregion
	#region MaterialParameter
		public  const uint EMISSION                       = 0x1600;
		public  const uint SHININESS                      = 0x1601;
		public  const uint AMBIENT_AND_DIFFUSE            = 0x1602;
		public  const uint COLOR_INDEXES                  = 0x1603;
	#endregion
	#region MatrixMode
		public  const uint MODELVIEW                      = 0x1700;
		public  const uint PROJECTION                     = 0x1701;
		public  const uint TEXTURE                        = 0x1702;
	#endregion
	#region PixelCopyType
		public  const uint COLOR                          = 0x1800;
		public  const uint DEPTH                          = 0x1801;
		public  const uint STENCIL                        = 0x1802;
	#endregion
	#region PixelFormat
		public  const uint COLOR_INDEX                    = 0x1900;
		public  const uint STENCIL_INDEX                  = 0x1901;
		public  const uint DEPTH_COMPONENT                = 0x1902;
		public  const uint RED                            = 0x1903;
		public  const uint GREEN                          = 0x1904;
		public  const uint BLUE                           = 0x1905;
		public  const uint ALPHA                          = 0x1906;
		public  const uint RGB                            = 0x1907;
		public  const uint RGBA                           = 0x1908;
		public  const uint LUMINANCE                      = 0x1909;
		public  const uint LUMINANCE_ALPHA                = 0x190A;
	#endregion
	#region PixelType
		public  const uint BITMAP                     = 0x1A00;
		#endregion
	#region PolygonMode
		public  const uint POINT                          = 0x1B00;
		public  const uint LINE                           = 0x1B01;
		public  const uint FILL                           = 0x1B02;
	#endregion
	#region RenderingMode 
		public  const uint RENDER                         = 0x1C00;
		public  const uint FEEDBACK                       = 0x1C01;
		public  const uint SELECT                         = 0x1C02;
	#endregion
	#region ShadingModel
		public  const uint FLAT                           = 0x1D00;
		public  const uint SMOOTH                         = 0x1D01;
	#endregion
	#region StencilOp	
		public  const uint KEEP                           = 0x1E00;
		public  const uint REPLACE                        = 0x1E01;
		public  const uint INCR                           = 0x1E02;
		public  const uint DECR                           = 0x1E03;
	#endregion
	#region StringName
		public  const uint VENDOR                         = 0x1F00;
		public  const uint RENDERER                       = 0x1F01;
		public  const uint VERSION                        = 0x1F02;
		public  const uint EXTENSIONS                     = 0x1F03;
	#endregion
	#region TextureCoordName
		public  const uint S                              = 0x2000;
		public  const uint T                              = 0x2001;
		public  const uint R                              = 0x2002;
		public  const uint Q                              = 0x2003;
	#endregion
	#region TextureEnvMode
		public  const uint MODULATE                       = 0x2100;
		public  const uint DECAL                          = 0x2101;
	#endregion
	#region TextureEnvParameter
		public  const uint TEXTURE_ENV_MODE               = 0x2200;
		public  const uint TEXTURE_ENV_COLOR              = 0x2201;
	#endregion
	#region TextureEnvTarget
		public  const uint TEXTURE_ENV                    = 0x2300;
	#endregion
	#region TextureGenMode 
		public  const uint EYE_LINEAR                     = 0x2400;
		public  const uint OBJECT_LINEAR                  = 0x2401;
		public  const uint SPHERE_MAP                     = 0x2402;
	#endregion
	#region TextureGenParameter
		public  const uint TEXTURE_GEN_MODE               = 0x2500;
		public  const uint OBJECT_PLANE                   = 0x2501;
		public  const uint EYE_PLANE                      = 0x2502;
	#endregion
	#region TextureMagFilter
		public  const uint NEAREST                        = 0x2600;
		public  const uint LINEAR                         = 0x2601;
	#endregion
	#region TextureMinFilter 
	
		public  const uint NEAREST_MIPMAP_NEAREST         = 0x2700;
		public  const uint LINEAR_MIPMAP_NEAREST          = 0x2701;
		public  const uint NEAREST_MIPMAP_LINEAR          = 0x2702;
		public  const uint LINEAR_MIPMAP_LINEAR           = 0x2703;
	#endregion
	#region TextureParameterName
		public  const uint TEXTURE_MAG_FILTER             = 0x2800;
		public  const uint TEXTURE_MIN_FILTER             = 0x2801;
		public  const uint TEXTURE_WRAP_S                 = 0x2802;
		public  const uint TEXTURE_WRAP_T                 = 0x2803;
	#endregion
	#region TextureWrapMode
		public  const uint CLAMP                          = 0x2900;
		public  const uint REPEAT                         = 0x2901;
	#endregion
	#region ClientAttribMask
		public  const uint CLIENT_PIXEL_STORE_BIT         = 0x00000001;
		public  const uint CLIENT_VERTEX_ARRAY_BIT        = 0x00000002;
		public  const uint CLIENT_ALL_ATTRIB_BITS         = 0xffffffff;
	#endregion
	#region Polygon Offset

		public  const uint POLYGON_OFFSET_FACTOR          = 0x8038;
		public  const uint POLYGON_OFFSET_UNITS           = 0x2A00;
		public  const uint POLYGON_OFFSET_POINT           = 0x2A01;
		public  const uint POLYGON_OFFSET_LINE            = 0x2A02;
		public  const uint POLYGON_OFFSET_FILL            = 0x8037;
	#endregion
	#region Texture 
		public  const uint ALPHA4                         = 0x803B;
		public  const uint ALPHA8                         = 0x803C;
		public  const uint ALPHA12                        = 0x803D;
		public  const uint ALPHA16                        = 0x803E;
		public  const uint LUMINANCE4                     = 0x803F;
		public  const uint LUMINANCE8                     = 0x8040;
		public  const uint LUMINANCE12                    = 0x8041;
		public  const uint LUMINANCE16                    = 0x8042;
		public  const uint LUMINANCE4_ALPHA4              = 0x8043;
		public  const uint LUMINANCE6_ALPHA2              = 0x8044;
		public  const uint LUMINANCE8_ALPHA8              = 0x8045;
		public  const uint LUMINANCE12_ALPHA4             = 0x8046;
		public  const uint LUMINANCE12_ALPHA12            = 0x8047;
		public  const uint LUMINANCE16_ALPHA16            = 0x8048;
		public  const uint INTENSITY                      = 0x8049;
		public  const uint INTENSITY4                     = 0x804A;
		public  const uint INTENSITY8                     = 0x804B;
		public  const uint INTENSITY12                    = 0x804C;
		public  const uint INTENSITY16                    = 0x804D;
		public  const uint R3_G3_B2                       = 0x2A10;
		public  const uint RGB4                           = 0x804F;
		public  const uint RGB5                           = 0x8050;
		public  const uint RGB8                           = 0x8051;
		public  const uint RGB10                          = 0x8052;
		public  const uint RGB12                          = 0x8053;
		public  const uint RGB16                          = 0x8054;
		public  const uint RGBA2                          = 0x8055;
		public  const uint RGBA4                          = 0x8056;
		public  const uint RGB5_A1                        = 0x8057;
		public  const uint RGBA8                          = 0x8058;
		public  const uint RGB10_A2                       = 0x8059;
		public  const uint RGBA12                         = 0x805A;
		public  const uint RGBA16                         = 0x805B;
		public  const uint TEXTURE_RED_SIZE               = 0x805C;
		public  const uint TEXTURE_GREEN_SIZE             = 0x805D;
		public  const uint TEXTURE_BLUE_SIZE              = 0x805E;
		public  const uint TEXTURE_ALPHA_SIZE             = 0x805F;
		public  const uint TEXTURE_LUMINANCE_SIZE         = 0x8060;
		public  const uint TEXTURE_INTENSITY_SIZE         = 0x8061;
		public  const uint PROXY_TEXTURE_1D               = 0x8063;
		public  const uint PROXY_TEXTURE_2D               = 0x8064;
	#endregion
	#region Texture object
		public  const uint TEXTURE_PRIORITY               = 0x8066;
		public  const uint TEXTURE_RESIDENT               = 0x8067;
		public  const uint TEXTURE_BINDING_1D             = 0x8068;
		public  const uint TEXTURE_BINDING_2D             = 0x8069;
	#endregion
	#region Vertex array
		public  const uint VERTEX_ARRAY                   = 0x8074;
		public  const uint NORMAL_ARRAY                   = 0x8075;
		public  const uint COLOR_ARRAY                    = 0x8076;
		public  const uint INDEX_ARRAY                    = 0x8077;
		public  const uint TEXTURE_COORD_ARRAY            = 0x8078;
		public  const uint EDGE_FLAG_ARRAY                = 0x8079;
		public  const uint VERTEX_ARRAY_SIZE              = 0x807A;
		public  const uint VERTEX_ARRAY_TYPE              = 0x807B;
		public  const uint VERTEX_ARRAY_STRIDE            = 0x807C;
		public  const uint NORMAL_ARRAY_TYPE              = 0x807E;
		public  const uint NORMAL_ARRAY_STRIDE            = 0x807F;
		public  const uint COLOR_ARRAY_SIZE               = 0x8081;
		public  const uint COLOR_ARRAY_TYPE               = 0x8082;
		public  const uint COLOR_ARRAY_STRIDE             = 0x8083;
		public  const uint INDEX_ARRAY_TYPE               = 0x8085;
		public  const uint INDEX_ARRAY_STRIDE             = 0x8086;
		public  const uint TEXTURE_COORD_ARRAY_SIZE       = 0x8088;
		public  const uint TEXTURE_COORD_ARRAY_TYPE       = 0x8089;
		public  const uint TEXTURE_COORD_ARRAY_STRIDE     = 0x808A;
		public  const uint EDGE_FLAG_ARRAY_STRIDE         = 0x808C;
		public  const uint VERTEX_ARRAY_POINTER           = 0x808E;
		public  const uint NORMAL_ARRAY_POINTER           = 0x808F;
		public  const uint COLOR_ARRAY_POINTER            = 0x8090;
		public  const uint INDEX_ARRAY_POINTER            = 0x8091;
		public  const uint TEXTURE_COORD_ARRAY_POINTER    = 0x8092;
		public  const uint EDGE_FLAG_ARRAY_POINTER        = 0x8093;
		public  const uint V2F                            = 0x2A20;
		public  const uint V3F                            = 0x2A21;
		public  const uint C4UB_V2F                       = 0x2A22;
		public  const uint C4UB_V3F                       = 0x2A23;
		public  const uint C3F_V3F                        = 0x2A24;
		public  const uint N3F_V3F                        = 0x2A25;
		public  const uint C4F_N3F_V3F                    = 0x2A26;
		public  const uint T2F_V3F                        = 0x2A27;
		public  const uint T4F_V4F                        = 0x2A28;
		public  const uint T2F_C4UB_V3F                   = 0x2A29;
		public  const uint T2F_C3F_V3F                    = 0x2A2A;
		public  const uint T2F_N3F_V3F                    = 0x2A2B;
		public  const uint T2F_C4F_N3F_V3F                = 0x2A2C;
		public  const uint T4F_C4F_N3F_V4F                = 0x2A2D;
	#endregion
	#region Extensions
		public  const uint EXT_vertex_array               = 1;
		public  const uint EXT_bgra                       = 1;
		public  const uint EXT_paletted_texture           = 1;
		public  const uint WIN_swap_hint                  = 1;
		public  const uint WIN_draw_range_elements        = 1;
		#endregion
	#region EXT_vertex_array 
		public  const uint VERTEX_ARRAY_EXT               = 0x8074;
		public  const uint NORMAL_ARRAY_EXT               = 0x8075;
		public  const uint COLOR_ARRAY_EXT                = 0x8076;
		public  const uint INDEX_ARRAY_EXT                = 0x8077;
		public  const uint TEXTURE_COORD_ARRAY_EXT        = 0x8078;
		public  const uint EDGE_FLAG_ARRAY_EXT            = 0x8079;
		public  const uint VERTEX_ARRAY_SIZE_EXT          = 0x807A;
		public  const uint VERTEX_ARRAY_TYPE_EXT          = 0x807B;
		public  const uint VERTEX_ARRAY_STRIDE_EXT        = 0x807C;
		public  const uint VERTEX_ARRAY_COUNT_EXT         = 0x807D;
		public  const uint NORMAL_ARRAY_TYPE_EXT          = 0x807E;
		public  const uint NORMAL_ARRAY_STRIDE_EXT        = 0x807F;
		public  const uint NORMAL_ARRAY_COUNT_EXT         = 0x8080;
		public  const uint COLOR_ARRAY_SIZE_EXT           = 0x8081;
		public  const uint COLOR_ARRAY_TYPE_EXT           = 0x8082;
		public  const uint COLOR_ARRAY_STRIDE_EXT         = 0x8083;
		public  const uint COLOR_ARRAY_COUNT_EXT          = 0x8084;
		public  const uint INDEX_ARRAY_TYPE_EXT           = 0x8085;
		public  const uint INDEX_ARRAY_STRIDE_EXT         = 0x8086;
		public  const uint INDEX_ARRAY_COUNT_EXT          = 0x8087;
		public  const uint TEXTURE_COORD_ARRAY_SIZE_EXT   = 0x8088;
		public  const uint TEXTURE_COORD_ARRAY_TYPE_EXT   = 0x8089;
		public  const uint TEXTURE_COORD_ARRAY_STRIDE_EXT = 0x808A;
		public  const uint TEXTURE_COORD_ARRAY_COUNT_EXT  = 0x808B;
		public  const uint EDGE_FLAG_ARRAY_STRIDE_EXT     = 0x808C;
		public  const uint EDGE_FLAG_ARRAY_COUNT_EXT      = 0x808D;
		public  const uint VERTEX_ARRAY_POINTER_EXT       = 0x808E;
		public  const uint NORMAL_ARRAY_POINTER_EXT       = 0x808F;
		public  const uint COLOR_ARRAY_POINTER_EXT        = 0x8090;
		public  const uint INDEX_ARRAY_POINTER_EXT        = 0x8091;
		public  const uint TEXTURE_COORD_ARRAY_POINTER_EXT = 0x8092;
		public  const uint EDGE_FLAG_ARRAY_POINTER_EXT    = 0x8093;
		public  const uint DOUBLE_EXT                     =1;/*DOUBLE*/
	#endregion
	#region EXT_bgra
		public  const uint BGR_EXT                        = 0x80E0;
		public  const uint BGRA_EXT                       = 0x80E1;
	#endregion
	#region EXT_paletted_texture

		public  const uint COLOR_TABLE_FORMAT_EXT         = 0x80D8;
		public  const uint COLOR_TABLE_WIDTH_EXT          = 0x80D9;
		public  const uint COLOR_TABLE_RED_SIZE_EXT       = 0x80DA;
		public  const uint COLOR_TABLE_GREEN_SIZE_EXT     = 0x80DB;
		public  const uint COLOR_TABLE_BLUE_SIZE_EXT      = 0x80DC;
		public  const uint COLOR_TABLE_ALPHA_SIZE_EXT     = 0x80DD;
		public  const uint COLOR_TABLE_LUMINANCE_SIZE_EXT = 0x80DE;
		public  const uint COLOR_TABLE_INTENSITY_SIZE_EXT = 0x80DF;
		public  const uint COLOR_INDEX1_EXT               = 0x80E2;
		public  const uint COLOR_INDEX2_EXT               = 0x80E3;
		public  const uint COLOR_INDEX4_EXT               = 0x80E4;
		public  const uint COLOR_INDEX8_EXT               = 0x80E5;
		public  const uint COLOR_INDEX12_EXT              = 0x80E6;
		public  const uint COLOR_INDEX16_EXT              = 0x80E7;
	#endregion
	#region WIN_draw_range_elements
		public  const uint MAX_ELEMENTS_VERTICES_WIN      = 0x80E8;
		public  const uint MAX_ELEMENTS_INDICES_WIN       = 0x80E9;
	#endregion
	#region WIN_phong_shading
		public  const uint PHONG_WIN                      = 0x80EA;
		public  const uint PHONG_HINT_WIN                 = 0x80EB; 
	#endregion

	#region WIN_specular_fog 
		public  uint FOG_SPECULAR_TEXTURE_WIN       = 0x80EC;
		#endregion

	#endregion
		#region The OpenGL DLL Functions (Exactly the same naming).

		[DllImport(LIBRARY_OPENGL)] protected static extern void glAccum (uint op, float value);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glAlphaFunc (uint func, float ref_notkeword);
		[DllImport(LIBRARY_OPENGL)] protected static extern byte glAreTexturesResident (int n,  uint []textures, byte []residences);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glArrayElement (int i);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glBegin (uint mode);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glBindTexture (uint target, uint texture);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glBitmap (int width, int height, float xorig, float yorig, float xmove, float ymove,  byte []bitmap);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glBlendFunc (uint sfactor, uint dfactor);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glCallList (uint list);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glCallLists (int n, uint type,  int[] lists);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glClear (uint mask);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glClearAccum (float red, float green, float blue, float alpha);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glClearColor (float red, float green, float blue, float alpha);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glClearDepth (double depth);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glClearIndex (float c);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glClearStencil (int s);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glClipPlane (uint plane,  double []equation);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor3b (byte red, byte green, byte blue);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor3bv ( byte []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor3d (double red, double green, double blue);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor3dv ( double []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor3f (float red, float green, float blue);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor3fv ( float []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor3i (int red, int green, int blue);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor3iv ( int []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor3s (short red, short green, short blue);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor3sv ( short []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor3ub (byte red, byte green, byte blue);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor3ubv ( byte []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor3ui (uint red, uint green, uint blue);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor3uiv ( uint []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor3us (ushort red, ushort green, ushort blue);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor3usv ( ushort []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor4b (byte red, byte green, byte blue, byte alpha);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor4bv ( byte []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor4d (double red, double green, double blue, double alpha);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor4dv ( double []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor4f (float red, float green, float blue, float alpha);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor4fv ( float []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor4i (int red, int green, int blue, int alpha);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor4iv ( int []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor4s (short red, short green, short blue, short alpha);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor4sv ( short []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor4ub (byte red, byte green, byte blue, byte alpha);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor4ubv ( byte []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor4ui (uint red, uint green, uint blue, uint alpha);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor4uiv ( uint []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor4us (ushort red, ushort green, ushort blue, ushort alpha);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColor4usv ( ushort []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColorMask (byte red, byte green, byte blue, byte alpha);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColorMaterial (uint face, uint mode);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glColorPointer (int size, uint type, int stride,  int[] pointer);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glCopyPixels (int x, int y, int width, int height, uint type);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glCopyTexImage1D (uint target, int level, uint internalFormat, int x, int y, int width, int border);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glCopyTexImage2D (uint target, int level, uint internalFormat, int x, int y, int width, int height, int border);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glCopyTexSubImage1D (uint target, int level, int xoffset, int x, int y, int width);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glCopyTexSubImage2D (uint target, int level, int xoffset, int yoffset, int x, int y, int width, int height);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glCullFace (uint mode);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glDeleteLists (uint list, int range);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glDeleteTextures (int n,  uint []textures);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glDepthFunc (uint func);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glDepthMask (byte flag);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glDepthRange (double zNear, double zFar);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glDisable (uint cap);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glDisableClientState (uint array);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glDrawArrays (uint mode, int first, int count);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glDrawBuffer (uint mode);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glDrawElements (uint mode, int count, uint type,  int[] indices);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glDrawPixels(int width, int height, uint format, uint type,  float[] pixels);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glEdgeFlag (byte flag);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glEdgeFlagPointer (int stride,  int[] pointer);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glEdgeFlagv ( byte []flag);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glEnable (uint cap);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glEnableClientState (uint array);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glEnd ();
		[DllImport(LIBRARY_OPENGL)] protected static extern void glEndList ();
		[DllImport(LIBRARY_OPENGL)] protected static extern void glEvalCoord1d (double u);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glEvalCoord1dv ( double []u);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glEvalCoord1f (float u);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glEvalCoord1fv ( float []u);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glEvalCoord2d (double u, double v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glEvalCoord2dv ( double []u);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glEvalCoord2f (float u, float v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glEvalCoord2fv ( float []u);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glEvalMesh1 (uint mode, int i1, int i2);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glEvalMesh2 (uint mode, int i1, int i2, int j1, int j2);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glEvalPoint1 (int i);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glEvalPoint2 (int i, int j);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glFeedbackBuffer (int size, uint type, float []buffer);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glFinish ();
		[DllImport(LIBRARY_OPENGL)] protected static extern void glFlush ();
		[DllImport(LIBRARY_OPENGL)] protected static extern void glFogf (uint pname, float param);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glFogfv (uint pname,  float []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glFogi (uint pname, int param);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glFogiv (uint pname,  int []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glFrontFace (uint mode);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glFrustum (double left, double right, double bottom, double top, double zNear, double zFar);
		[DllImport(LIBRARY_OPENGL)] protected static extern uint glGenLists (int range);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGenTextures (int n, uint []textures);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetBooleanv (uint pname, byte []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetClipPlane (uint plane, double []equation);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetDoublev (uint pname, double []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern uint glGetError ();
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetFloatv (uint pname, float []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetIntegerv (uint pname, int []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetLightfv (uint light, uint pname, float []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetLightiv (uint light, uint pname, int []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetMapdv (uint target, uint query, double []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetMapfv (uint target, uint query, float []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetMapiv (uint target, uint query, int []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetMaterialfv (uint face, uint pname, float []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetMaterialiv (uint face, uint pname, int []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetPixelMapfv (uint map, float []values);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetPixelMapuiv (uint map, uint []values);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetPixelMapusv (uint map, ushort []values);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetPointerv (uint pname, int[] params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetPolygonStipple (byte []mask);
		[DllImport(LIBRARY_OPENGL)] protected unsafe static extern sbyte* glGetString (uint name);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetTexEnvfv (uint target, uint pname, float []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetTexEnviv (uint target, uint pname, int []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetTexGendv (uint coord, uint pname, double []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetTexGenfv (uint coord, uint pname, float []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetTexGeniv (uint coord, uint pname, int []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetTexImage (uint target, int level, uint format, uint type, int []pixels);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetTexLevelParameterfv (uint target, int level, uint pname, float []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetTexLevelParameteriv (uint target, int level, uint pname, int []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetTexParameterfv (uint target, uint pname, float []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glGetTexParameteriv (uint target, uint pname, int []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glHint (uint target, uint mode);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glIndexMask (uint mask);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glIndexPointer (uint type, int stride,  int[] pointer);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glIndexd (double c);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glIndexdv ( double []c);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glIndexf (float c);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glIndexfv ( float []c);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glIndexi (int c);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glIndexiv ( int []c);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glIndexs (short c);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glIndexsv ( short []c);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glIndexub (byte c);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glIndexubv ( byte []c);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glInitNames ();
		[DllImport(LIBRARY_OPENGL)] protected static extern void glInterleavedArrays (uint format, int stride,  int[] pointer);
		[DllImport(LIBRARY_OPENGL)] protected static extern byte glIsEnabled (uint cap);
		[DllImport(LIBRARY_OPENGL)] protected static extern byte glIsList (uint list);
		[DllImport(LIBRARY_OPENGL)] protected static extern byte glIsTexture (uint texture);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glLightModelf (uint pname, float param);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glLightModelfv (uint pname,  float []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glLightModeli (uint pname, int param);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glLightModeliv (uint pname,  int []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glLightf (uint light, uint pname, float param);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glLightfv (uint light, uint pname,  float []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glLighti (uint light, uint pname, int param);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glLightiv (uint light, uint pname,  int []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glLineStipple (int factor, ushort pattern);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glLineWidth (float width);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glListBase (uint base_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glLoadIdentity ();
		[DllImport(LIBRARY_OPENGL)] protected static extern void glLoadMatrixd ( double []m);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glLoadMatrixf ( float []m);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glLoadName (uint name);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glLogicOp (uint opcode);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glMap1d (uint target, double u1, double u2, int stride, int order,  double []points);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glMap1f (uint target, float u1, float u2, int stride, int order,  float []points);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glMap2d (uint target, double u1, double u2, int ustride, int uorder, double v1, double v2, int vstride, int vorder,  double []points);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glMap2f (uint target, float u1, float u2, int ustride, int uorder, float v1, float v2, int vstride, int vorder,  float []points);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glMapGrid1d (int un, double u1, double u2);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glMapGrid1f (int un, float u1, float u2);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glMapGrid2d (int un, double u1, double u2, int vn, double v1, double v2);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glMapGrid2f (int un, float u1, float u2, int vn, float v1, float v2);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glMaterialf (uint face, uint pname, float param);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glMaterialfv (uint face, uint pname,  float []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glMateriali (uint face, uint pname, int param);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glMaterialiv (uint face, uint pname,  int []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glMatrixMode (uint mode);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glMultMatrixd ( double []m);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glMultMatrixf ( float []m);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glNewList (uint list, uint mode);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glNormal3b (byte nx, byte ny, byte nz);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glNormal3bv ( byte []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glNormal3d (double nx, double ny, double nz);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glNormal3dv ( double []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glNormal3f (float nx, float ny, float nz);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glNormal3fv ( float []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glNormal3i (int nx, int ny, int nz);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glNormal3iv ( int []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glNormal3s (short nx, short ny, short nz);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glNormal3sv ( short []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glNormalPointer (uint type, int stride, float[] pointer);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glOrtho (double left, double right, double bottom, double top, double zNear, double zFar);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPassThrough (float token);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPixelMapfv (uint map, int mapsize,  float []values);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPixelMapuiv (uint map, int mapsize,  uint []values);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPixelMapusv (uint map, int mapsize,  ushort []values);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPixelStoref (uint pname, float param);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPixelStorei (uint pname, int param);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPixelTransferf (uint pname, float param);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPixelTransferi (uint pname, int param);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPixelZoom (float xfactor, float yfactor);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPointSize (float size);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPolygonMode (uint face, uint mode);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPolygonOffset (float factor, float units);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPolygonStipple ( byte []mask);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPopAttrib ();
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPopClientAttrib ();
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPopMatrix ();
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPopName ();
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPrioritizeTextures (int n,  uint []textures,  float []priorities);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPushAttrib (uint mask);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPushClientAttrib (uint mask);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPushMatrix ();
		[DllImport(LIBRARY_OPENGL)] protected static extern void glPushName (uint name);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos2d (double x, double y);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos2dv ( double []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos2f (float x, float y);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos2fv ( float []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos2i (int x, int y);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos2iv ( int []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos2s (short x, short y);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos2sv ( short []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos3d (double x, double y, double z);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos3dv ( double []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos3f (float x, float y, float z);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos3fv ( float []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos3i (int x, int y, int z);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos3iv ( int []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos3s (short x, short y, short z);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos3sv ( short []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos4d (double x, double y, double z, double w);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos4dv ( double []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos4f (float x, float y, float z, float w);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos4fv ( float []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos4i (int x, int y, int z, int w);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos4iv ( int []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos4s (short x, short y, short z, short w);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRasterPos4sv ( short []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glReadBuffer (uint mode);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glReadPixels(int x, int y, int width, int height, uint format, uint type, float[] pixels);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRectd (double x1, double y1, double x2, double y2);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRectdv ( double []v1,  double []v2);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRectf (float x1, float y1, float x2, float y2);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRectfv ( float []v1,  float []v2);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRecti (int x1, int y1, int x2, int y2);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRectiv ( int []v1,  int []v2);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRects (short x1, short y1, short x2, short y2);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRectsv ( short []v1,  short []v2);
		[DllImport(LIBRARY_OPENGL)] protected static extern int glRenderMode (uint mode);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRotated (double angle, double x, double y, double z);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glRotatef (float angle, float x, float y, float z);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glScaled (double x, double y, double z);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glScalef (float x, float y, float z);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glScissor (int x, int y, int width, int height);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glSelectBuffer (int size, uint []buffer);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glShadeModel (uint mode);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glStencilFunc (uint func, int ref_notkeword, uint mask);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glStencilMask (uint mask);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glStencilOp (uint fail, uint zfail, uint zpass);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord1d (double s);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord1dv ( double []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord1f (float s);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord1fv ( float []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord1i (int s);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord1iv ( int []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord1s (short s);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord1sv ( short []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord2d (double s, double t);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord2dv ( double []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord2f (float s, float t);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord2fv ( float []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord2i (int s, int t);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord2iv ( int []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord2s (short s, short t);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord2sv ( short []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord3d (double s, double t, double r);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord3dv ( double []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord3f (float s, float t, float r);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord3fv ( float []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord3i (int s, int t, int r);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord3iv ( int []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord3s (short s, short t, short r);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord3sv ( short []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord4d (double s, double t, double r, double q);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord4dv ( double []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord4f (float s, float t, float r, float q);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord4fv ( float []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord4i (int s, int t, int r, int q);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord4iv ( int []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord4s (short s, short t, short r, short q);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoord4sv ( short []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexCoordPointer (int size, uint type, int stride,  float[] pointer);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexEnvf (uint target, uint pname, float param);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexEnvfv (uint target, uint pname,  float []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexEnvi (uint target, uint pname, int param);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexEnviv (uint target, uint pname,  int []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexGend (uint coord, uint pname, double param);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexGendv (uint coord, uint pname,  double []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexGenf (uint coord, uint pname, float param);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexGenfv (uint coord, uint pname,  float []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexGeni (uint coord, uint pname, int param);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexGeniv (uint coord, uint pname,  int []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexImage1D (uint target, int level, int internalformat, int width, int border, uint format, uint type,  byte[] pixels);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexImage2D (uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, byte[] pixels);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexImage2D (uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, IntPtr pixels);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexParameterf (uint target, uint pname, float param);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexParameterfv (uint target, uint pname,  float []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexParameteri (uint target, uint pname, int param);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexParameteriv (uint target, uint pname,  int []params_notkeyword);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexSubImage1D (uint target, int level, int xoffset, int width, uint format, uint type,  int[] pixels);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTexSubImage2D (uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type,  int[] pixels);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTranslated (double x, double y, double z);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glTranslatef (float x, float y, float z);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex2d (double x, double y);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex2dv ( double []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex2f (float x, float y);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex2fv ( float []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex2i (int x, int y);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex2iv ( int []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex2s (short x, short y);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex2sv ( short []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex3d (double x, double y, double z);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex3dv ( double []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex3f (float x, float y, float z);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex3fv ( float []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex3i (int x, int y, int z);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex3iv ( int []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex3s (short x, short y, short z);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex3sv ( short []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex4d (double x, double y, double z, double w);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex4dv ( double []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex4f (float x, float y, float z, float w);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex4fv ( float []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex4i (int x, int y, int z, int w);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex4iv ( int []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex4s (short x, short y, short z, short w);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertex4sv ( short []v);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glVertexPointer (int size, uint type, int stride, float[] pointer);
		[DllImport(LIBRARY_OPENGL)] protected static extern void glViewport (int x, int y, int width, int height);
	#endregion
		#region The GLU DLL Functions (Exactly the same naming).

		[DllImport(LIBRARY_GLU)] protected static extern string gluErrorString(int errCode);
		[DllImport(LIBRARY_GLU)] protected static extern string gluGetString(int name);
		[DllImport(LIBRARY_GLU)] protected static extern void gluOrtho2D(double left, double right, double bottom, double top);
		[DllImport(LIBRARY_GLU)] protected static extern void gluPerspective (double fovy, double aspect, double zNear, double zFar);
		[DllImport(LIBRARY_GLU)] protected static extern void gluPickMatrix ( double x, double y, double width, double height, int[] viewport);
		[DllImport(LIBRARY_GLU)] protected static extern void gluLookAt ( double eyex, double eyey, double eyez, double centerx, double centery, double centerz, double upx, double upy, double upz);
		[DllImport(LIBRARY_GLU)] protected static extern void gluProject (double objx, double        objy, double        objz,   double[]  modelMatrix,  double[]  projMatrix,  int[] viewport, double [] winx, double        []winy, double        []winz);
		[DllImport(LIBRARY_GLU)] protected static extern void gluUnProject (double       winx, double       winy, double       winz,  double[] modelMatrix,  double[] projMatrix,  int[] viewport, double [] objx, double       []objy, double       []objz);
		[DllImport(LIBRARY_GLU)] protected static extern void gluScaleImage ( int      format, int       widthin, int       heightin,  int      typein,  int  []datain, int       widthout, int       heightout, int      typeout, int[] dataout);
		[DllImport(LIBRARY_GLU)] protected static extern void gluBuild1DMipmaps (int      target, int       components, int       width, int      format, int      type,  int[] data);
		[DllImport(LIBRARY_GLU)] protected static extern void gluBuild2DMipmaps (int      target, int       components, int       width, int       height, int      format, int      type,  int[] data);
		[DllImport(LIBRARY_GLU)] protected static extern IntPtr gluNewQuadric();
		[DllImport(LIBRARY_GLU)] protected static extern void gluDeleteQuadric (IntPtr state);
		[DllImport(LIBRARY_GLU)] protected static extern void gluQuadricNormals (IntPtr quadObject, int normals);
		[DllImport(LIBRARY_GLU)] protected static extern void gluQuadricTexture (IntPtr quadObject, int textureCoords);
		[DllImport(LIBRARY_GLU)] protected static extern void gluQuadricOrientation (IntPtr quadObject, int orientation);
		[DllImport(LIBRARY_GLU)] protected static extern void gluQuadricDrawStyle (IntPtr quadObject, uint drawStyle);
		[DllImport(LIBRARY_GLU)] protected static extern void gluCylinder(IntPtr           qobj,double            baseRadius, double topRadius, double height,int slices,int stacks);
		[DllImport(LIBRARY_GLU)] protected static extern void gluDisk(IntPtr qobj, double innerRadius,double outerRadius,int slices, int loops);
		[DllImport(LIBRARY_GLU)] protected static extern void gluPartialDisk(IntPtr qobj,double innerRadius,double outerRadius, int slices, int loops, double startAngle, double sweepAngle);
		[DllImport(LIBRARY_GLU)] protected static extern void gluSphere(IntPtr qobj, double radius, int slices, int stacks);
		[DllImport(LIBRARY_GLU)] protected static extern IntPtr gluNewTess();
		[DllImport(LIBRARY_GLU)] protected static extern void  gluDeleteTess(IntPtr tess);
		[DllImport(LIBRARY_GLU)] protected static extern void  gluTessBeginPolygon(IntPtr tess, IntPtr polygonData);
		[DllImport(LIBRARY_GLU)] protected static extern void  gluTessBeginContour(IntPtr tess);
		[DllImport(LIBRARY_GLU)] protected static extern void  gluTessVertex(IntPtr tess,double[] coords, double[] data );
		[DllImport(LIBRARY_GLU)] protected static extern void  gluTessEndContour(   IntPtr        tess );
		[DllImport(LIBRARY_GLU)] protected static extern void  gluTessEndPolygon(   IntPtr        tess );
		[DllImport(LIBRARY_GLU)] protected static extern void  gluTessProperty(     IntPtr        tess,int              which, double            value );
		[DllImport(LIBRARY_GLU)] protected static extern void  gluTessNormal(       IntPtr        tess, double            x,double            y, double            z );
//		[DllImport(LIBRARY_GLU)] protected static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.Begin callback);
//		[DllImport(LIBRARY_GLU)] protected static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.BeginData callback);
//		[DllImport(LIBRARY_GLU)] protected static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.Combine callback);
//		[DllImport(LIBRARY_GLU)] protected static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.CombineData callback);
//		[DllImport(LIBRARY_GLU)] protected static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.EdgeFlag callback);
//		[DllImport(LIBRARY_GLU)] protected static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.EdgeFlagData callback);
//		[DllImport(LIBRARY_GLU)] protected static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.End callback);
//		[DllImport(LIBRARY_GLU)] protected static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.EndData callback);
//		[DllImport(LIBRARY_GLU)] protected static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.Error callback);
//		[DllImport(LIBRARY_GLU)] protected static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.ErrorData callback);
//		[DllImport(LIBRARY_GLU)] protected static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.Vertex callback);
//		[DllImport(LIBRARY_GLU)] protected static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.VertexData callback);
		[DllImport(LIBRARY_GLU)] protected static extern void  gluGetTessProperty(  IntPtr        tess,int              which, double            value );
		[DllImport(LIBRARY_GLU)] protected static extern IntPtr gluNewNurbsRenderer ();
		[DllImport(LIBRARY_GLU)] protected static extern void gluDeleteNurbsRenderer (IntPtr            nobj);
		[DllImport(LIBRARY_GLU)] protected static extern void gluBeginSurface (IntPtr            nobj);
		[DllImport(LIBRARY_GLU)] protected static extern void gluBeginCurve (IntPtr            nobj);
		[DllImport(LIBRARY_GLU)] protected static extern void gluEndCurve (IntPtr            nobj);
		[DllImport(LIBRARY_GLU)] protected static extern void gluEndSurface (IntPtr            nobj);
		[DllImport(LIBRARY_GLU)] protected static extern void gluBeginTrim (IntPtr            nobj);
		[DllImport(LIBRARY_GLU)] protected static extern void gluEndTrim (IntPtr            nobj);
		[DllImport(LIBRARY_GLU)] protected static extern void gluPwlCurve (IntPtr            nobj, int               count, float             array, int stride, uint type);
		[DllImport(LIBRARY_GLU)] protected static extern void gluNurbsCurve(IntPtr nobj, int nknots, float[] knot, int               stride, float[] ctlarray, int               order, uint type);
		[DllImport(LIBRARY_GLU)] protected static extern void gluNurbsSurface(IntPtr nobj, int sknot_count, float[] sknot, int tknot_count, float[]             tknot, int               s_stride, int               t_stride, float[] ctlarray, int sorder, int               torder, uint              type);
		[DllImport(LIBRARY_GLU)] protected static extern void gluLoadSamplingMatrices (IntPtr            nobj,  float[] modelMatrix,  float[] projMatrix, int[] viewport);
		[DllImport(LIBRARY_GLU)] protected static extern void gluNurbsProperty(IntPtr nobj, int property, float value);
		[DllImport(LIBRARY_GLU)] protected static extern void gluGetNurbsProperty (IntPtr            nobj, int              property, float             value );
		[DllImport(LIBRARY_GLU)] protected static extern void IntPtrCallback(IntPtr            nobj, int              which, IntPtr Callback );

		#endregion
		#region The GLU DLL Constant Definitions.

		
		/* Version */
		public const uint GLU_VERSION_1_1                 = 1;
		public const uint GLU_VERSION_1_2                 = 1;

		/* Errors: (return value 0 = no error) */
		public const uint GLU_INVALID_ENUM        = 100900;
		public const uint GLU_INVALID_VALUE       = 100901;
		public const uint GLU_OUT_OF_MEMORY       = 100902;
		public const uint GLU_INCOMPATIBLE_GL_VERSION    = 100903;

		/* StringName */
		public const uint GLU_VERSION             = 100800;
		public const uint GLU_EXTENSIONS          = 100801;

		/* Boolean */
		public const uint GLU_TRUE                = 1;
		public const uint GLU_FALSE               = 0;


		/****           Quadric constants               ****/

		/* QuadricNormal */
		public const uint GLU_SMOOTH              = 100000;
		public const uint GLU_FLAT                = 100001;
		public const uint GLU_NONE                = 100002;

		/* QuadricDrawStyle */
		public const uint GLU_POINT               = 100010;
		public const uint GLU_LINE                = 100011;
		public const uint GLU_FILL                = 100012;
		public const uint GLU_SILHOUETTE          = 100013;

		/* QuadricOrientation */
		public const uint GLU_OUTSIDE             = 100020;
		public const uint GLU_INSIDE              = 100021;

		/****           Tesselation constants           ****/

		public const double GLU_TESS_MAX_COORD             = 1.0e150;

		/* TessProperty */
		public const uint GLU_TESS_WINDING_RULE           =100140;
		public const uint GLU_TESS_BOUNDARY_ONLY          =100141;
		public const uint GLU_TESS_TOLERANCE              =100142;

		/* TessWinding */
		public const uint GLU_TESS_WINDING_ODD            =100130;
		public const uint GLU_TESS_WINDING_NONZERO        =100131;
		public const uint GLU_TESS_WINDING_POSITIVE       =100132;
		public const uint GLU_TESS_WINDING_NEGATIVE       =100133;
		public const uint GLU_TESS_WINDING_ABS_GEQ_TWO    =100134;

		/* TessCallback */
		public const uint GLU_TESS_BEGIN          =100100;  /* void (CALLBACK*)(GLenum    type)  */
		public const uint GLU_TESS_VERTEX         =100101;  /* void (CALLBACK*)(void      *data) */
		public const uint GLU_TESS_END            =100102;  /* void (CALLBACK*)(void)            */
		public const uint GLU_TESS_ERROR          =100103;  /* void (CALLBACK*)(GLenum    errno) */
		public const uint GLU_TESS_EDGE_FLAG      =100104;  /* void (CALLBACK*)(GLboolean boundaryEdge)  */
		public const uint GLU_TESS_COMBINE        =100105;  /* void (CALLBACK*)(GLdouble  coords[3],
		void      *data[4],
			GLfloat   weight[4],
		void      **dataOut)     */
		public const uint GLU_TESS_BEGIN_DATA     =100106;  /* void (CALLBACK*)(GLenum    type,  
		void      *polygon_data) */
		public const uint GLU_TESS_VERTEX_DATA    =100107;  /* void (CALLBACK*)(void      *data, 
		void      *polygon_data) */
		public const uint GLU_TESS_END_DATA       =100108;/* void (CALLBACK*)(void      *polygon_data) */
		public const uint GLU_TESS_ERROR_DATA     =100109; /* void (CALLBACK*)(GLenum    errno, 
		void      *polygon_data) */
		public const uint GLU_TESS_EDGE_FLAG_DATA =100110;  /* void (CALLBACK*)(GLboolean boundaryEdge,
		void      *polygon_data) */
		public const uint GLU_TESS_COMBINE_DATA   =100111;  /* void (CALLBACK*)(GLdouble  coords[3],
		void      *data[4],
			GLfloat   weight[4],
		void      **dataOut,
			void      *polygon_data) */

		/* TessError */
		public const uint GLU_TESS_ERROR1     =100151;
		public const uint GLU_TESS_ERROR2     =100152;
		public const uint GLU_TESS_ERROR3     =100153;
		public const uint GLU_TESS_ERROR4     =100154;
		public const uint GLU_TESS_ERROR5     =100155;
		public const uint GLU_TESS_ERROR6     =100156;
		public const uint GLU_TESS_ERROR7     =100157;
		public const uint GLU_TESS_ERROR8     =100158;

		public const uint GLU_TESS_MISSING_BEGIN_POLYGON  =100151;
		public const uint GLU_TESS_MISSING_BEGIN_CONTOUR  =100152;
		public const uint GLU_TESS_MISSING_END_POLYGON    =100153;
		public const uint GLU_TESS_MISSING_END_CONTOUR    =100154;
		public const uint GLU_TESS_COORD_TOO_LARGE        =100155;
		public const uint GLU_TESS_NEED_COMBINE_CALLBACK  =100156;

		/****           NURBS constants                 ****/

		/* NurbsProperty */
		public const uint GLU_AUTO_LOAD_MATRIX    =100200;
		public const uint GLU_CULLING             =100201;
		public const uint GLU_SAMPLING_TOLERANCE  =100203;
		public const uint GLU_DISPLAY_MODE        =100204;
		public const uint GLU_PARAMETRIC_TOLERANCE        =100202;
		public const uint GLU_SAMPLING_METHOD             =100205;
		public const uint GLU_U_STEP                      =100206;
		public const uint GLU_V_STEP                      =100207;

		/* NurbsSampling */
		public const uint GLU_PATH_LENGTH                 =100215;
		public const uint GLU_PARAMETRIC_ERROR            =100216;
		public const uint GLU_DOMAIN_DISTANCE             =100217;


		/* NurbsTrim */
		public const uint GLU_MAP1_TRIM_2         =100210;
		public const uint GLU_MAP1_TRIM_3         =100211;

		/* NurbsDisplay */
		/*      GLU_FILL                100012 */
		public const uint GLU_OUTLINE_POLYGON     =100240;
		public const uint GLU_OUTLINE_PATCH       =100241;

		/* NurbsCallback */
		/*      GLU_ERROR               100103 */

		/* NurbsErrors */
		public const uint GLU_NURBS_ERROR1        =100251;
		public const uint GLU_NURBS_ERROR2        =100252;
		public const uint GLU_NURBS_ERROR3        =100253;
		public const uint GLU_NURBS_ERROR4        =100254;
		public const uint GLU_NURBS_ERROR5        =100255;
		public const uint GLU_NURBS_ERROR6        =100256;
		public const uint GLU_NURBS_ERROR7        =100257;
		public const uint GLU_NURBS_ERROR8        =100258;
		public const uint GLU_NURBS_ERROR9        =100259;
		public const uint GLU_NURBS_ERROR10       =100260;
		public const uint GLU_NURBS_ERROR11       =100261;
		public const uint GLU_NURBS_ERROR12       =100262;
		public const uint GLU_NURBS_ERROR13       =100263;
		public const uint GLU_NURBS_ERROR14       =100264;
		public const uint GLU_NURBS_ERROR15       =100265;
		public const uint GLU_NURBS_ERROR16       =100266;
		public const uint GLU_NURBS_ERROR17       =100267;
		public const uint GLU_NURBS_ERROR18       =100268;
		public const uint GLU_NURBS_ERROR19       =100269;
		public const uint GLU_NURBS_ERROR20       =100270;
		public const uint GLU_NURBS_ERROR21       =100271;
		public const uint GLU_NURBS_ERROR22       =100272;
		public const uint GLU_NURBS_ERROR23       =100273;
		public const uint GLU_NURBS_ERROR24       =100274;
		public const uint GLU_NURBS_ERROR25       =100275;
		public const uint GLU_NURBS_ERROR26       =100276;
		public const uint GLU_NURBS_ERROR27       =100277;
		public const uint GLU_NURBS_ERROR28       =100278;
		public const uint GLU_NURBS_ERROR29       =100279;
		public const uint GLU_NURBS_ERROR30       =100280;
		public const uint GLU_NURBS_ERROR31       =100281;
		public const uint GLU_NURBS_ERROR32       =100282;
		public const uint GLU_NURBS_ERROR33       =100283;
		public const uint GLU_NURBS_ERROR34       =100284;
		public const uint GLU_NURBS_ERROR35       =100285;
		public const uint GLU_NURBS_ERROR36       =100286;
		public const uint GLU_NURBS_ERROR37       =100287;

		#endregion

		#region Wrapped OpenGL Functions

		/// <summary>
		/// Set the Accumulation Buffer operation.
		/// </summary>
		/// <param name="op">Operation of the buffer.</param>
		/// <param name="value">Reference value.</param>
		public void Accum(uint op, float value)
		{
			PreErrorCheck();
			glAccum(op, value);
			ErrorCheck();
		}
		public void AlphaFunc(uint func, float ref_notkeword){}
		public byte AreTexturesResident(int n,  uint []textures, byte []residences){return 0;}
		public void ArrayElement (int i){}
		/// <summary>
		/// Begin drawing geometry in the specified mode.
		/// </summary>
		/// <param name="mode">The mode to draw in, e.g. OpenGL.POLYGONS.</param>
		public void Begin(uint mode)
		{
			PreErrorCheck();
			glBegin(mode);
			ErrorCheck();
		}
		/// <summary>
		/// This function begins drawing a NURBS curve.
		/// </summary>
		/// <param name="nurbsObject">The NURBS object.</param>
        public void BeginCurve(IntPtr nurbsObject)
		{
			PreErrorCheck();
			gluBeginCurve(nurbsObject);
			ErrorCheck();
		}
		/// <summary>
		/// This function begins drawing a NURBS surface.
		/// </summary>
		/// <param name="nurbsObject">The NURBS object.</param>
		public void BeginSurface(IntPtr nurbsObject)
		{
			PreErrorCheck();
			gluBeginSurface(nurbsObject);
			ErrorCheck();
		}
		/// <summary>
		/// Call this function after creating a texture to finalise creation of it, 
		/// or to make an existing texture current.
		/// </summary>
		/// <param name="target">The target type, e.g TEXTURE_2D.</param>
		/// <param name="texture">The OpenGL texture object.</param>
		public void BindTexture(uint target, uint texture)
		{
			PreErrorCheck();
			glBindTexture(target, texture);
			ErrorCheck();
		}
		public void Bitmap (int width, int height, float xorig, float yorig, float xmove, float ymove,  byte []bitmap){}
		/// <summary>
		/// This function sets the current blending function.
		/// </summary>
		/// <param name="sfactor">Source factor.</param>
		/// <param name="dfactor">Destination factor.</param>
		public void BlendFunc(uint sfactor, uint dfactor)
		{
			PreErrorCheck();
			glBlendFunc(sfactor,dfactor);
			ErrorCheck();
		}
		/// <summary>
		/// This function calls a certain display list.
		/// </summary>
		/// <param name="list">The display list to call.</param>
		public void CallList(uint list)
		{
			PreErrorCheck();
			glCallList(list);
			ErrorCheck();
		}
		public void CallLists (int n, uint type,  int[] lists){}
		/// <summary>
		/// This function clears the buffers specified by mask.
		/// </summary>
		/// <param name="mask">Which buffers to clear.</param>
		public void Clear(uint mask)
		{
			PreErrorCheck();
			glClear(mask);
			ErrorCheck();
		}
		public void ClearAccum (float red, float green, float blue, float alpha){}
		/// <summary>
		/// This function sets the color that the drawing buffer is 'cleared' to.
		/// </summary>
		/// <param name="red">Red component of the color (between 0 and 1).</param>
		/// <param name="green">Green component of the color (between 0 and 1).</param>
		/// <param name="blue">Blue component of the color (between 0 and 1)./<param>
		/// <param name="alpha">Alpha component of the color (between 0 and 1).</param>
		public void ClearColor (float red, float green, float blue, float alpha)
		{
			PreErrorCheck();
			glClearColor(red, green, blue, alpha);
			ErrorCheck();
		}
		public void ClearDepth (double depth){}
		public void ClearIndex (float c){}
		public void ClearStencil (int s){}
		public void ClipPlane (uint plane,  double []equation){}
		/// <summary>
		/// Sets the current color.
		/// </summary>
		/// <param name="red">Red color component (between 0 and 1).</param>
		/// <param name="green">Green color component (between 0 and 1).</param>
		/// <param name="blue">Blue color component (between 0 and 1).</param>
		public void Color(byte red, byte green, byte blue)
		{
			PreErrorCheck();
			glColor3b(red, green, blue);
			ErrorCheck();
		}
		public void Color3bv ( byte []v){}
		/// <summary>
		/// Sets the current color.
		/// </summary>
		/// <param name="red">Red color component (between 0 and 1).</param>
		/// <param name="green">Green color component (between 0 and 1).</param>
		/// <param name="blue">Blue color component (between 0 and 1).</param>
		public void Color(double red, double green, double blue)
		{
			PreErrorCheck();
			glColor3d(red, green, blue);
			ErrorCheck();
		}
		public void Color3dv ( double []v){}
		/// <summary>
		/// Sets the current color.
		/// </summary>
		/// <param name="red">Red color component (between 0 and 1).</param>
		/// <param name="green">Green color component (between 0 and 1).</param>
		/// <param name="blue">Blue color component (between 0 and 1).</param>
		public void Color(float red, float green, float blue)
		{
			PreErrorCheck();
			glColor3f(red, green, blue);
			ErrorCheck();
		}
		/// <summary>
		/// Sets the current color to 'v'.
		/// </summary>
		/// <param name="v">An array of either 3 or 4 float values.</param>
		public void Color(float[] v)
		{
			PreErrorCheck();
			if(v.Length == 3)
				glColor3fv(v);
			else if(v.Length == 4)
				glColor4fv(v);
			ErrorCheck();
		}
		/// <summary>
		/// Sets the current color.
		/// </summary>
		/// <param name="red">Red color component (between 0 and 1).</param>
		/// <param name="green">Green color component (between 0 and 1).</param>
		/// <param name="blue">Blue color component (between 0 and 1).</param>
		public void Color(int red, int green, int blue)
		{
			PreErrorCheck();
			glColor3i(red, green, blue);
			ErrorCheck();
		}
		public void Color3iv ( int []v){}
		/// <summary>
		/// Sets the current color.
		/// </summary>
		/// <param name="red">Red color component (between 0 and 1).</param>
		/// <param name="green">Green color component (between 0 and 1).</param>
		/// <param name="blue">Blue color component (between 0 and 1).</param>
		public void Color(short red, short green, short blue)
		{
			PreErrorCheck();
			glColor3s(red, green, blue);
			ErrorCheck();
		}
		public void Color3sv ( short []v){}
	/*	/// <summary>
		/// Sets the current color.
		/// </summary>
		/// <param name="red">Red color component (between 0 and 1).</param>
		/// <param name="green">Green color component (between 0 and 1).</param>
		/// <param name="blue">Blue color component (between 0 and 1).</param>
		public void Color(byte red, byte green, byte blue)
		{
			PreErrorCheck();
			glColor3ub(red, green, blue);
			ErrorCheck();
		}*/
		public void Color3ubv ( byte []v){}
		/// <summary>
		/// Sets the current color.
		/// </summary>
		/// <param name="red">Red color component (between 0 and 1).</param>
		/// <param name="green">Green color component (between 0 and 1).</param>
		/// <param name="blue">Blue color component (between 0 and 1).</param>
		public void Color(uint red, uint green, uint blue)
		{
			PreErrorCheck();
			glColor3ui(red, green, blue);
			ErrorCheck();
		}
		public void Color3uiv ( uint []v){}
		/// <summary>
		/// Sets the current color.
		/// </summary>
		/// <param name="red">Red color component (between 0 and 1).</param>
		/// <param name="green">Green color component (between 0 and 1).</param>
		/// <param name="blue">Blue color component (between 0 and 1).</param>
		public void Color(ushort red, ushort green, ushort blue)
		{
			PreErrorCheck();
			glColor3us(red, green, blue);
			ErrorCheck();
		}
		public void Color3usv ( ushort []v){}
		public void Color4b (byte red, byte green, byte blue, byte alpha){}
		public void Color4bv ( byte []v){}
		public void Color4d (double red, double green, double blue, double alpha){}
		public void Color4dv ( double []v){}
		/// <summary>
		/// Sets the current color.
		/// </summary>
		/// <param name="red">Red color component (between 0 and 1).</param>
		/// <param name="green">Green color component (between 0 and 1).</param>
		/// <param name="blue">Blue color component (between 0 and 1).</param>
		/// <param name="alpha">Alpha color component (between 0 and 1).</param>
		public void Color(float red, float green, float blue, float alpha)
		{
			PreErrorCheck();
			glColor4f(red, green, blue, alpha);
			ErrorCheck();
		}
		public void Color4i (int red, int green, int blue, int alpha){}
		public void Color4iv ( int []v){}
		public void Color4s (short red, short green, short blue, short alpha){}
		public void Color4sv ( short []v){}
		public void Color4ub (byte red, byte green, byte blue, byte alpha){}
		public void Color4ubv ( byte []v){}
		public void Color4ui (uint red, uint green, uint blue, uint alpha){}
		public void Color4uiv ( uint []v){}
		public void Color4us (ushort red, ushort green, ushort blue, ushort alpha){}
		public void Color4usv ( ushort []v){}
		/// <summary>
		/// This function sets the current colour mask.
		/// </summary>
		/// <param name="red">Red component mask.</param>
		/// <param name="green">Green component mask.</param>
		/// <param name="blue">Blue component mask.</param>
		/// <param name="alpha">Alpha component mask.</param>
		public void ColorMask(byte red, byte green, byte blue, byte alpha)
		{
			PreErrorCheck();
			glColorMask(red, green, blue, alpha);
			ErrorCheck();
		}
		public void ColorMaterial (uint face, uint mode){}
		public void ColorPointer (int size, uint type, int stride,  int[] pointer){}
		public void CopyPixels (int x, int y, int width, int height, uint type){}
		public void CopyTexImage1D (uint target, int level, uint internalFormat, int x, int y, int width, int border){}
		public void CopyTexImage2D (uint target, int level, uint internalFormat, int x, int y, int width, int height, int border){}
		public void CopyTexSubImage1D (uint target, int level, int xoffset, int x, int y, int width){}
		public void CopyTexSubImage2D (uint target, int level, int xoffset, int yoffset, int x, int y, int width, int height){}
		public void CullFace (uint mode){}

		/// <summary>
		/// This function draws a sphere from the quadric object.
		/// </summary>
		/// <param name="qobj">The quadric object.</param>
		/// <param name="baseRadius">Radius at the base.</param>
		/// <param name="topRadius">Radius at the top.</param>
		/// <param name="height">Height of cylinder.</param>
		/// <param name="slices">Cylinder slices.</param>
		/// <param name="stacks">Cylinder stacks.</param>
		public void Cylinder(IntPtr qobj, double baseRadius, double topRadius, double height,int slices, int stacks)
		{
			PreErrorCheck();
			gluCylinder(qobj, baseRadius, topRadius, height, slices, stacks);
			ErrorCheck();
		}
		/// <summary>
		/// This function deletes a list, or a range of lists.
		/// </summary>
		/// <param name="list">The list to delete.</param>
		/// <param name="range">The range of lists (often just 1).</param>
		public void DeleteLists(uint list, int range)
		{
			PreErrorCheck();
			glDeleteLists(list, range);
			ErrorCheck();
		}
		/// <summary>
		/// This function deletes the underlying glu nurbs renderer.
		/// </summary>
		/// <param name="nurbsObject">The pointer to the nurbs object.</param>
		public void DeleteNurbsRenderer(IntPtr nurbsObject)
		{
			PreErrorCheck();
			gluDeleteNurbsRenderer(nurbsObject);
			ErrorCheck();
		}
		/// <summary>
		/// This function deletes a set of Texture objects.
		/// </summary>
		/// <param name="n">Number of textures to delete.</param>
		/// <param name="textures">The array containing the names of the textures to delete.</param>
		public void DeleteTextures (int n,  uint []textures)
		{
			PreErrorCheck();
			glDeleteTextures(n, textures);
			ErrorCheck();
		}
		/// <summary>
		/// Call this function to delete an OpenGL Quadric object.
		/// </summary>
		/// <param name="state"></param>
		public void DeleteQuadric(IntPtr quadric)
		{
			PreErrorCheck();
			gluDeleteQuadric(quadric);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets the current depth buffer comparison function, the default it LESS.
		/// </summary>
		/// <param name="func">The comparison function to set.</param>
		public void DepthFunc(uint func)
		{
			PreErrorCheck();
			glDepthFunc(func);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets the depth mask.
		/// </summary>
		/// <param name="flag">The depth mask flag, normally 1.</param>
		public void DepthMask(byte flag)
		{
			PreErrorCheck();
			glDepthMask(flag);
			ErrorCheck();
		}
		public void DepthRange (double zNear, double zFar){}
		/// <summary>
		/// Call this function to disable an OpenGL capability.
		/// </summary>
		/// <param name="cap">The capability to disable.</param>
		public void Disable(uint cap)
		{
			PreErrorCheck();
			glDisable(cap);
			ErrorCheck();
		}
		/// <summary>
		/// This function disables a client state array, such as a vertex array.
		/// </summary>
		/// <param name="array">The array to disable.</param>
		public void DisableClientState (uint array)
		{
			PreErrorCheck();
			glDisableClientState(array);
			ErrorCheck();
		}
		public void DrawArrays (uint mode, int first, int count){}
		public void DrawBuffer (uint mode){}
		public void DrawElements (uint mode, int count, uint type,  int[] indices){}
		/// <summary>
		/// Draws a rectangle of pixel data at the current raster position.
		/// </summary>
		/// <param name="width">Width of pixel data.</param>
		/// <param name="height">Height of pixel data.</param>
		/// <param name="format">Format of pixel data.</param>
		/// <param name="type">Type of pixel data.</param>
		/// <param name="pixels">Pixel data buffer.</param>
		public void DrawPixels(int width, int height, uint format, uint type,  float[] pixels)
		{
			PreErrorCheck();
			glDrawPixels(width, height, format, type, pixels);
			ErrorCheck();
		}
		public void EdgeFlag (byte flag){}
		public void EdgeFlagPointer (int stride,  int[] pointer){}
		public void EdgeFlagv ( byte []flag){}
		/// <summary>
		/// Call this function to enable an OpenGL capability.
		/// </summary>
		/// <param name="cap">The capability you wish to enable.</param>
		public void Enable(uint cap)
		{
			PreErrorCheck();
			glEnable(cap);
			ErrorCheck();
		}
		/// <summary>
		/// This function enables one of the client state arrays, such as a vertex array.
		/// </summary>
		/// <param name="array">The array to enable.</param>
		public void EnableClientState(uint array)
		{
			PreErrorCheck();
			glEnableClientState(array);
			ErrorCheck();
		}
		/// <summary>
		/// This is not an imported OpenGL function, but very useful. If 'test' is
		/// true, cap is enabled, otherwise, it's disable.
		/// </summary>
		/// <param name="cap">The capability you want to enable.</param>
		/// <param name="test">The logical comparison.</param>
		public void EnableIf(uint cap, bool test)
		{
			if(test)	Enable(cap);
			else		Disable(cap);
		}
		/// <summary>
		/// Signals the End of drawing.
		/// </summary>
		public void End()
		{
			PreErrorCheck();
			glEnd();
			ErrorCheck();
		}
		/// <summary>
		/// This function ends the drawing of a NURBS curve.
		/// </summary>
		/// <param name="nobj">The NURBS object.</param>
		public void EndCurve(IntPtr nurbsObject)
		{
			PreErrorCheck();
			gluEndCurve(nurbsObject);
			ErrorCheck();
		}
		/// <summary>
		/// Ends the current display list compilation.
		/// </summary>
		public void EndList()
		{
			PreErrorCheck();
			glEndList();
			ErrorCheck();
		}
		/// <summary>
		/// This function ends the drawing of a NURBS surface.
		/// </summary>
		/// <param name="nobj">The NURBS object.</param>
		public void EndSurface(IntPtr nurbsObject)
		{
			PreErrorCheck();
			gluEndSurface(nurbsObject);
			ErrorCheck();
		}
		
		/// <summary>
		/// Evaluate from the current evaluator.
		/// </summary>
		/// <param name="u">Domain coordinate.</param>
		public void EvalCoord1(double u)
		{
			PreErrorCheck();
			glEvalCoord1d(u);
			ErrorCheck();
		}
		public void EvalCoord1dv ( double []u){}
		/// <summary>
		/// Evaluate from the current evaluator.
		/// </summary>
		/// <param name="u">Domain coordinate.</param>
		public void EvalCoord1(float u)
		{
			PreErrorCheck();
			glEvalCoord1f(u);
			ErrorCheck();
		}
		public void EvalCoord1fv ( float []u){}
		public void EvalCoord2d (double u, double v){}
		public void EvalCoord2dv ( double []u){}
		public void EvalCoord2f (float u, float v){}
		public void EvalCoord2fv ( float []u){}
		/// <summary>
		/// Evaluates a 'mesh' from the current evaluators.
		/// </summary>
		/// <param name="mode">Drawing mode, can be POINT or LINE.</param>
		/// <param name="i1">Beginning of range.<param>
		/// <param name="i2">End of range.</param>
		public void EvalMesh1 (uint mode, int i1, int i2)
		{
			PreErrorCheck();
			glEvalMesh1(mode, i1, i2);
			ErrorCheck();
		}
		/// <summary>
		/// Evaluates a 'mesh' from the current evaluators.
		/// </summary>
		/// <param name="mode">Drawing mode, fill, point or line.</param>
		/// <param name="i1">Beginning of range.</param>
		/// <param name="i2">End of range.</param>
		/// <param name="j1">Beginning of range.</param>
		/// <param name="j2">End of range.</param>
		public void EvalMesh2 (uint mode, int i1, int i2, int j1, int j2)
		{
			PreErrorCheck();
			glEvalMesh2(mode, i1, i2, j1, j2);
			ErrorCheck();
		}
		public void EvalPoint1 (int i){}
		public void EvalPoint2 (int i, int j){}
		/// <summary>
		/// This function sets the feedback buffer, that will receive feedback data.
		/// </summary>
		/// <param name="size">Size of the buffer.</param>
		/// <param name="type">Type of data in the buffer.</param>
		/// <param name="buffer">The buffer itself.</param>
		public void FeedbackBuffer(int size, uint type, float []buffer)
		{
			PreErrorCheck();
			glFeedbackBuffer(size, type, buffer);
			ErrorCheck();
		}
		/// <summary>
		/// This function is similar to flush, but in a sense does it more, as it
		/// executes all commands aon both the client and the server.
		/// </summary>
		public void Finish()
		{
			PreErrorCheck();
			glFinish();
			ErrorCheck();
		}
		/// <summary>
		/// This forces OpenGL to execute any commands you have given it.
		/// </summary>
		public void Flush()
		{
			PreErrorCheck();
			glFlush();
			ErrorCheck();
		}
		/// <summary>
		/// Sets a fog parameter.
		/// </summary>
		/// <param name="pname">The parameter to set.</param>
		/// <param name="param">The value to set it to.</param>
		public void Fog(uint pname, float param)
		{
			PreErrorCheck();
			glFogf(pname, param);
			ErrorCheck();
		}
		/// <summary>
		/// Sets a fog parameter.
		/// </summary>
		/// <param name="pname">The parameter to set.</param>
		/// <param name="parameters">The values to set it to.</param>
		public void Fog(uint pname,  float[] parameters)
		{
			PreErrorCheck();
			glFogfv(pname, parameters);
			ErrorCheck();
		}
		/// <summary>
		/// Sets a fog parameter.
		/// </summary>
		/// <param name="pname">The parameter to set.</param>
		/// <param name="param">The value to set it to.</param>
		public void Fog(uint pname, int param)
		{
			PreErrorCheck();
			glFogi(pname, param);
			ErrorCheck();
		}
		/// <summary>
		/// Sets a fog parameter.
		/// </summary>
		/// <param name="pname">The parameter to set.</param>
		/// <param name="parameters">The values to set it to.</param>
		public void Fog(uint pname,  int[] parameters)
		{
			PreErrorCheck();
			glFogiv(pname, parameters);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets what defines a front face.
		/// </summary>
		/// <param name="mode">Winding mode, counter clockwise by default.</param>
		public void FrontFace(uint mode)
		{
			PreErrorCheck();
			glFrontFace(mode);
			ErrorCheck();
		}
		/// <summary>
		/// This function creates a frustrum transformation and mulitplies it to the current
		/// matrix (which in most cases should be the projection matrix).
		/// </summary>
		/// <param name="left">Left clip position.</param>
		/// <param name="right">Right clip position.</param>
		/// <param name="bottom">Bottom clip position.</param>
		/// <param name="top">Top clip position.</param>
		/// <param name="zNear">Near clip position.</param>
		/// <param name="zFar">Far clip position.</param>
		public void Frustum(double left, double right, double bottom, 
			double top, double zNear, double zFar)
		{
			PreErrorCheck();
			glFrustum(left, right, bottom, top, zNear, zFar);
			ErrorCheck();
		}
		/// <summary>
		/// This function generates 'range' number of contiguos display list indices.
		/// </summary>
		/// <param name="range">The number of lists to generate.</param>
		/// <returns>The first list.</returns>
		public uint GenLists(int range)
		{
			PreErrorCheck();
			uint list = glGenLists(range);
			ErrorCheck();

			return list;
		}
		/// <summary>
		/// Create a set of unique texture names.
		/// </summary>
		/// <param name="n">Number of names to create.</param>
		/// <param name="textures">Array to store the texture names.</param>
		public void GenTextures(int n, uint []textures)
		{
			PreErrorCheck();
			glGenTextures(n, textures);
			ErrorCheck();
		}
		/// <summary>
		/// This function queries OpenGL for data, and puts it in the buffer supplied.
		/// </summary>
		/// <param name="pname">The parameter to query.</param>
		/// <param name="parameters"></param>
		public void GetBooleanv (uint pname, byte[] parameters)
		{
			PreErrorCheck();
			glGetBooleanv(pname, parameters);
			ErrorCheck();
		}
		public void GetClipPlane (uint plane, double []equation){}
		/// <summary>
		/// This function queries OpenGL for data, and puts it in the buffer supplied.
		/// </summary>
		/// <param name="pname">The parameter to query.</param>
		/// <param name="parameters">The buffer to put that data into.</param>
		public void GetDouble(uint pname, double []parameters)
		{
			PreErrorCheck();
			glGetDoublev(pname, parameters);
			ErrorCheck();
		}
		/// <summary>
		/// Get the current OpenGL error code.
		/// </summary>
		/// <returns>The current OpenGL error code.</returns>
		public uint GetError()
		{
			return glGetError();
		}
		/// <summary>
		/// This this function to query OpenGL values.
		/// </summary>
		/// <param name="pname">The parameter to query.</param>
		/// <param name="parameters">The parameters</param>
		public void GetFloat(uint pname, float[] parameters)
		{
			PreErrorCheck();
			glGetFloatv(pname, parameters);
			ErrorCheck();
		}
		/// <summary>
		/// Use this function to query OpenGL parameter values.
		/// </summary>
		/// <param name="pname">The Parameter to query</param>
		/// <param name="parameters">An array to put the values into.</param>
		public void GetInteger(uint pname, int[] parameters)
		{
			PreErrorCheck();
			glGetIntegerv(pname, parameters);
			ErrorCheck();
		}
		public void GetLightfv (uint light, uint pname, float []params_notkeyword){}
		public void GetLightiv (uint light, uint pname, int []params_notkeyword){}
		public void GetMapdv (uint target, uint query, double []v){}
		public void GetMapfv (uint target, uint query, float []v){}
		public void GetMapiv (uint target, uint query, int []v){}
		public void GetMaterialfv (uint face, uint pname, float []params_notkeyword){}
		public void GetMaterialiv (uint face, uint pname, int []params_notkeyword){}
		public void GetPixelMapfv (uint map, float []values){}
		public void GetPixelMapuiv (uint map, uint []values){}
		public void GetPixelMapusv (uint map, ushort []values){}
		public void GetPointerv (uint pname, int[] params_notkeyword){}
		public void GetPolygonStipple (byte []mask){}
		public unsafe string GetString (uint name)
		{
			sbyte* pStr = glGetString(name);
			string str = new string(pStr);
			return str;
		}
		public void GetTexEnvfv (uint target, uint pname, float []params_notkeyword){}
		public void GetTexEnviv (uint target, uint pname, int []params_notkeyword){}
		public void GetTexGendv (uint coord, uint pname, double []params_notkeyword){}
		public void GetTexGenfv (uint coord, uint pname, float []params_notkeyword){}
		public void GetTexGeniv (uint coord, uint pname, int []params_notkeyword){}
		public void GetTexImage (uint target, int level, uint format, uint type, int []pixels){}
		public void GetTexLevelParameterfv (uint target, int level, uint pname, float []params_notkeyword){}
		public void GetTexLevelParameteriv (uint target, int level, uint pname, int []params_notkeyword){}
		public void GetTexParameterfv (uint target, uint pname, float []params_notkeyword){}
		public void GetTexParameteriv (uint target, uint pname, int []params_notkeyword){}
		public void Hint (uint target, uint mode){}
		public void IndexMask (uint mask){}
		public void IndexPointer (uint type, int stride,  int[] pointer){}
		public void Indexd (double c){}
		public void Indexdv ( double []c){}
		public void Indexf (float c){}
		public void Indexfv ( float []c){}
		public void Indexi (int c){}
		public void Indexiv ( int []c){}
		public void Indexs (short c){}
		public void Indexsv ( short []c){}
		public void Indexub (byte c){}
		public void Indexubv ( byte []c){}
		/// <summary>
		/// This function initialises the select buffer names.
		/// </summary>
		public void InitNames()
		{
			PreErrorCheck();
			glInitNames();
			ErrorCheck();
		}
		public void InterleavedArrays (uint format, int stride,  int[] pointer){}
		/// <summary>
		/// Use this function to query if a certain OpenGL function is enabled or not.
		/// </summary>
		/// <param name="cap">The capability to test.</param>
		/// <returns>True if the capability is enabled, otherwise, false.</returns>
		public bool IsEnabled (uint cap)
		{
			PreErrorCheck();
			byte e = glIsEnabled(cap);
			ErrorCheck();

			return e == 0 ? false : true;
		}
		/// <summary>
		/// This function determines whether a specified value is a display list.
		/// </summary>
		/// <param name="list">The value to test.</param>
		/// <returns>TRUE if it is a list, FALSE otherwise.</returns>
		public byte IsList(uint list)
		{
			PreErrorCheck();
			byte islist = glIsList(list);
			ErrorCheck();

			return islist;
		}
		public byte IsTexture (uint texture){return 0;}
		/// <summary>
		/// This function sets a parameter of the lighting model.
		/// </summary>
		/// <param name="pname">The name of the parameter.</param>
		/// <param name="param">The parameter to set it to.</param>
		public void LightModel(uint pname, float param)
		{
			PreErrorCheck();
			glLightModelf(pname, param);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets a parameter of the lighting model.
		/// </summary>
		/// <param name="pname">The name of the parameter.</param>
		/// <param name="parameters">The parameter to set it to.</param>
		public void LightModel(uint pname,  float[] parameters)
		{
			PreErrorCheck();
			glLightModelfv(pname, parameters);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets a parameter of the lighting model.
		/// </summary>
		/// <param name="pname">The name of the parameter.</param>
		/// <param name="param">The parameter to set it to.</param>
		public void LightModel(uint pname, int param)
		{
			PreErrorCheck();
			glLightModeli(pname, param);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets a parameter of the lighting model.
		/// </summary>
		/// <param name="pname">The name of the parameter.</param>
		/// <param name="parameters">The parameter to set it to.</param>
		public void LightModel (uint pname, int[] parameters)
		{
			PreErrorCheck();
			glLightModeliv(pname, parameters);
			ErrorCheck();
		}
		/// <summary>
		/// Set the parameter (pname) of the light 'light'.
		/// </summary>
		/// <param name="light">The light you wish to set parameters for.</param>
		/// <param name="pname">The parameter you want to set.</param>
		/// <param name="param">The value that you want to set the parameter to.</param>
		public void Light(uint light, uint pname, float param)
		{
            PreErrorCheck();
			glLightf(light, pname, param);
			ErrorCheck();
		}
		/// <summary>
		/// Set the parameter (pname) of the light 'light'.
		/// </summary>
		/// <param name="light">The light you wish to set parameters for.</param>
		/// <param name="pname">The parameter you want to set.</param>
		/// <param name="param">The value that you want to set the parameter to.</param>
		public void Light(uint light, uint pname,  float []parameters)
		{
			PreErrorCheck();
			glLightfv(light, pname, parameters);
			ErrorCheck();
		}
		/// <summary>
		/// Set the parameter (pname) of the light 'light'.
		/// </summary>
		/// <param name="light">The light you wish to set parameters for.</param>
		/// <param name="pname">The parameter you want to set.</param>
		/// <param name="param">The value that you want to set the parameter to.</param>
		public void Light(uint light, uint pname, int param)
		{
			PreErrorCheck();
			glLighti(light, pname, param);
			ErrorCheck();
		}
		/// <summary>
		/// Set the parameter (pname) of the light 'light'.
		/// </summary>
		/// <param name="light">The light you wish to set parameters for.</param>
		/// <param name="pname">The parameter you want to set.</param>
		/// <param name="param">The value that you want to set the parameter to.</param>
		public void Light(uint light, uint pname,  int []parameters)
		{
			PreErrorCheck();
			glLightiv(light, pname, parameters);
			ErrorCheck();
		}
		public void LineStipple (int factor, ushort pattern){}
		/// <summary>
		/// Set's the current width of lines.
		/// </summary>
		/// <param name="width">New line width to set.</param>
		public void LineWidth(float width)
		{
			PreErrorCheck();
			glLineWidth(width);
			ErrorCheck();
		}
		public void ListBase (uint base_notkeyword){}
		/// <summary>
		/// Call this function to load the identity matrix into the current matrix stack.
		/// </summary>
		public void LoadIdentity()
		{
			PreErrorCheck();
			glLoadIdentity();
			ErrorCheck();
		}
		public void LoadMatrixd ( double []m){}
		public void LoadMatrixf ( float []m){}
		/// <summary>
		/// This function replaces the name at the top of the selection names stack
		/// with 'name'.
		/// </summary>
		/// <param name="name">The name to replace it with.</param>
		public void LoadName (uint name)
		{
			PreErrorCheck();
			glLoadName(name);
			ErrorCheck();
		}
		public void LogicOp (uint opcode){}
		/// <summary>
		/// This function transforms the projection matrix so that it looks at a certain
		/// point, from a certain point.
		/// </summary>
		/// <param name="eyex">Position of the eye.</param>
		/// <param name="eyey">Position of the eye.</param>
		/// <param name="eyez">Position of the eye.</param>
		/// <param name="centerx">Point to look at.</param>
		/// <param name="centery">Point to look at.</param>
		/// <param name="centerz">Point to look at.</param>
		/// <param name="upx">'Up' Vector X Component.</param>
		/// <param name="upy">'Up' Vector Y Component.</param>
		/// <param name="upz">'Up' Vector Z Component.</param>
		public void LookAt(double eyex, double eyey, double eyez, 
			double centerx, double centery, double centerz, 
			double upx, double upy, double upz)
		{
			PreErrorCheck();
			gluLookAt(eyex, eyey, eyez, centerx, centery, centerz, upx, upy, upz);
			ErrorCheck();
		}
		/// <summary>
		/// Defines a 1D evaluator.
		/// </summary>
		/// <param name="target">What the control points represent (e.g. MAP1_VERTEX_3).</param>
		/// <param name="u1">Range of the variable 'u'.</param>
		/// <param name="u2">Range of the variable 'u'.</param>
		/// <param name="stride">Offset between beginning of one control point, and beginning of next.</param>
		/// <param name="order">The degree plus one, should agree with the number of control points.</param>
		/// <param name="points">The data for the points.</param>
		public void Map1(uint target, double u1, double u2, int stride, int order,  double []points)
		{
			PreErrorCheck();
			glMap1d(target, u1, u2, stride, order, points);
			ErrorCheck();
		}
		/// <summary>
		/// Defines a 1D evaluator.
		/// </summary>
		/// <param name="target">What the control points represent (e.g. MAP1_VERTEX_3).</param>
		/// <param name="u1">Range of the variable 'u'.</param>
		/// <param name="u2">Range of the variable 'u'.</param>
		/// <param name="stride">Offset between beginning of one control point, and beginning of next.</param>
		/// <param name="order">The degree plus one, should agree with the number of control points.</param>
		/// <param name="points">The data for the points.</param>
		public void Map1(uint target, float u1, float u2, int stride, int order,  float []points)
		{
			PreErrorCheck();
			glMap1f(target, u1, u2, stride, order, points);
			ErrorCheck();
		}
		/// <summary>
		/// Defines a 2D evaluator.
		/// </summary>
		/// <param name="target">What the control points represent (e.g. MAP2_VERTEX_3).</param>
		/// <param name="u1">Range of the variable 'u'.</param>
		/// <param name="u2">Range of the variable 'u.</param>
		/// <param name="ustride">Offset between beginning of one control point and the next.</param>
		/// <param name="uorder">The degree plus one.</param>
		/// <param name="v1">Range of the variable 'v'.</param>
		/// <param name="v2">Range of the variable 'v'.</param>
		/// <param name="vstride">Offset between beginning of one control point and the next.</param>
		/// <param name="vorder">The degree plus one.</param>
		/// <param name="points">The data for the points.</param>
		public void Map2(uint target, double u1, double u2, int ustride, int uorder, double v1, double v2, int vstride, int vorder,  double []points)
		{
			PreErrorCheck();
			glMap2d(target, u1, u2, ustride, uorder, v1, v2, vstride, vorder, points);
			ErrorCheck();
		}
		/// <summary>
		/// Defines a 2D evaluator.
		/// </summary>
		/// <param name="target">What the control points represent (e.g. MAP2_VERTEX_3).</param>
		/// <param name="u1">Range of the variable 'u'.</param>
		/// <param name="u2">Range of the variable 'u.</param>
		/// <param name="ustride">Offset between beginning of one control point and the next.</param>
		/// <param name="uorder">The degree plus one.</param>
		/// <param name="v1">Range of the variable 'v'.</param>
		/// <param name="v2">Range of the variable 'v'.</param>
		/// <param name="vstride">Offset between beginning of one control point and the next.</param>
		/// <param name="vorder">The degree plus one.</param>
		/// <param name="points">The data for the points.</param>
		public void Map2(uint target, float u1, float u2, int ustride, int uorder, float v1, float v2, int vstride, int vorder,  float []points)
		{
			PreErrorCheck();
			glMap2f(target, u1, u2, ustride, uorder, v1, v2, vstride, vorder, points);
			ErrorCheck();
		}
		/// <summary>
		/// This function defines a grid that goes from u1 to u1 in n steps, evenly spaced.
		/// </summary>
		/// <param name="un">Number of steps.</param>
		/// <param name="u1">Range of variable 'u'.</param>
		/// <param name="u2">Range of variable 'u'.</param>
		public void MapGrid1(int un, double u1, double u2)
		{
			PreErrorCheck();
			glMapGrid1d(un, u1, u2);
			ErrorCheck();
		}
		/// <summary>
		/// This function defines a grid that goes from u1 to u1 in n steps, evenly spaced.
		/// </summary>
		/// <param name="un">Number of steps.</param>
		/// <param name="u1">Range of variable 'u'.</param>
		/// <param name="u2">Range of variable 'u'.</param>
		public void MapGrid1(int un, float u1, float u2)
		{
			PreErrorCheck();
			glMapGrid1d(un, u1, u2);
			ErrorCheck();
		}
		/// <summary>
		/// This function defines a grid that goes from u1 to u1 in n steps, evenly spaced,
		/// and the same for v.
		/// </summary>
		/// <param name="un">Number of steps.</param>
		/// <param name="u1">Range of variable 'u'.</param>
		/// <param name="u2">Range of variable 'u'.</param>
		/// <param name="vn">Number of steps.</param>
		/// <param name="v1">Range of variable 'v'.</param>
		/// <param name="v2">Range of variable 'v'.</param>
		public void MapGrid2(int un, double u1, double u2, int vn, double v1, double v2)
		{
			PreErrorCheck();
			glMapGrid2d(un, u1, u2, vn, v1, v2);
			ErrorCheck();
		}
		/// <summary>
		/// This function defines a grid that goes from u1 to u1 in n steps, evenly spaced,
		/// and the same for v.
		/// </summary>
		/// <param name="un">Number of steps.</param>
		/// <param name="u1">Range of variable 'u'.</param>
		/// <param name="u2">Range of variable 'u'.</param>
		/// <param name="vn">Number of steps.</param>
		/// <param name="v1">Range of variable 'v'.</param>
		/// <param name="v2">Range of variable 'v'.</param>
		public void MapGrid2(int un, float u1, float u2, int vn, float v1, float v2)
		{
			PreErrorCheck();
			glMapGrid2f(un, u1, u2, vn, v1, v2);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets a material parameter.
		/// </summary>
		/// <param name="face">What faces is this parameter for (i.e front/back etc).</param>
		/// <param name="pname">What parameter you want to set.</param>
		/// <param name="param">The value to set 'pname' to.</param>
		public void Material(uint face, uint pname, float param)
		{
			PreErrorCheck();
			glMaterialf(face, pname, param);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets a material parameter.
		/// </summary>
		/// <param name="face">What faces is this parameter for (i.e front/back etc).</param>
		/// <param name="pname">What parameter you want to set.</param>
		/// <param name="parameters">The value to set 'pname' to.</param>
		public void Material(uint face, uint pname,  float[] parameters)
		{
			PreErrorCheck();
			glMaterialfv(face, pname, parameters);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets a material parameter.
		/// </summary>
		/// <param name="face">What faces is this parameter for (i.e front/back etc).</param>
		/// <param name="pname">What parameter you want to set.</param>
		/// <param name="param">The value to set 'pname' to.</param>
		public void Material(uint face, uint pname, int param)
		{
			PreErrorCheck();
			glMateriali(face, pname, param);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets a material parameter.
		/// </summary>
		/// <param name="face">What faces is this parameter for (i.e front/back etc).</param>
		/// <param name="pname">What parameter you want to set.</param>
		/// <param name="parameters">The value to set 'pname' to.</param>
		public void Material(uint face, uint pname,  int[] parameters)
		{
			PreErrorCheck();
			glMaterialiv(face, pname, parameters);
			ErrorCheck();
		}
		/// <summary>
		/// Set the current matrix mode (the matrix that matrix operations will be 
		/// performed on).
		/// </summary>
		/// <param name="mode">The mode, normally PROJECTION or MODELVIEW.</param>
		public void MatrixMode (uint mode)
		{
			PreErrorCheck();
			glMatrixMode(mode);
			ErrorCheck();
		}
		public void MultMatrixd ( double []m){}
		public void MultMatrixf ( float []m){}
		/// <summary>
		/// This function starts compiling a new display list.
		/// </summary>
		/// <param name="list">The list to compile.</param>
		/// <param name="mode">Either COMPILE or COMPILE_AND_EXECUTE.</param>
		public void NewList(uint list, uint mode)
		{
			PreErrorCheck();
			glNewList(list, mode);
			ErrorCheck();
		}
		/// <summary>
		/// This function creates a new glu NURBS renderer object.
		/// </summary>
		/// <returns>A Pointer to the NURBS renderer.</returns>
		public IntPtr NewNurbsRenderer()
		{
			PreErrorCheck();
			IntPtr nurbs = gluNewNurbsRenderer();
			ErrorCheck();

			return nurbs;
		}
		/// <summary>
		/// This function creates a new OpenGL Quadric Object.
		/// </summary>
		/// <returns>The pointer to the Quadric Object.</returns>
		public IntPtr NewQuadric()
		{
			PreErrorCheck();
			IntPtr quad = gluNewQuadric();
			ErrorCheck();

			return quad;
		}
		public void Normal3b (byte nx, byte ny, byte nz){}
		public void Normal3bv ( byte []v){}
		public void Normal3d (double nx, double ny, double nz){}
		public void Normal3dv ( double []v){}
		public void Normal3f (float nx, float ny, float nz){}
		/// <summary>
		/// This function sets the current normal.
		/// </summary>
		/// <param name="v">The normal.</param>
		public void Normal(float[] v)
		{
			PreErrorCheck();
			glNormal3fv(v);
			ErrorCheck();
		}
		public void Normal3i (int nx, int ny, int nz){}
		public void Normal3iv ( int []v){}
		public void Normal3s (short nx, short ny, short nz){}
		public void Normal3sv ( short []v){}
		/// <summary>
		/// Set's the pointer to the normal array.
		/// </summary>
		/// <param name="type">The type of data.</param>
		/// <param name="stride">The space in bytes between each normal.</param>
		/// <param name="pointer">The normals.</param>
		public void NormalPointer(uint type, int stride, float[] pointer)
		{
			PreErrorCheck();
			glNormalPointer(type, stride, pointer);
			ErrorCheck();
		}
		/// <summary>
		/// This function defines a NURBS Curve.
		/// </summary>
		/// <param name="nurbsObject">The NURBS object.</param>
		/// <param name="knotsCount">The number of knots.</param>
		/// <param name="knots">The knots themselves.</param>
		/// <param name="stride">The stride, i.e. distance between vertices in the 
		/// control points array.</param>
		/// <param name="controlPointsArray">The array of control points.</param>
		/// <param name="order">The order of the polynomial.</param>
		/// <param name="type">The type of data to generate.</param>
		public void NurbsCurve(IntPtr nurbsObject, int knotsCount, float[] knots, 
			int stride, float[] controlPointsArray, int order, uint type)
		{
			PreErrorCheck();
			gluNurbsCurve(nurbsObject, knotsCount, knots, stride, controlPointsArray,
				order, type);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets a NURBS property.
		/// </summary>
		/// <param name="nurbsObject">The object to set the property for.</param>
		/// <param name="property">The property to set.</param>
		/// <param name="value">The new value of the property.</param>
		public void NurbsProperty(IntPtr nurbsObject, int property, float value)
		{
			PreErrorCheck();
			gluNurbsProperty(nurbsObject, property, value);
			ErrorCheck();
		}
		/// <summary>
		/// This function defines a NURBS surface.
		/// </summary>
		/// <param name="nurbsObject">The NURBS object.</param>
		/// <param name="sknotscount">The number of s-knots.</param>
		/// <param name="sknots">The s-knots.</param>
		/// <param name="tknotsCount">The number of t-knots.</param>
		/// <param name="tknots">The t-knots.</param>
		/// <param name="sStride">The distance between s vertices.</param>
		/// <param name="tStride">The distance between t vertices.</param>
		/// <param name="controlPointsArray">The control points.</param>
		/// <param name="sOrder">The order of the s polynomial.</param>
		/// <param name="tOrder">The order of the t polynomial.</param>
		/// <param name="type">The type of data to generate.</param>
		public void NurbsSurface(IntPtr nurbsObject, int sknotsCount, float[] sknots, 
			int tknotsCount, float[] tknots, int sStride, int tStride, 
			float[] controlPointsArray, int sOrder, int tOrder, uint type)
		{
			PreErrorCheck();
			gluNurbsSurface(nurbsObject, sknotsCount, sknots, tknotsCount, tknots,
				sStride, tStride, controlPointsArray, sOrder, tOrder, type);
			ErrorCheck();
		}
		
		/// <summary>
		/// This function creates an orthographic projection matrix (i.e one with no 
		/// perspective) and multiplies it to the current matrix stack, which would
		/// normally be 'PROJECTION'.
		/// </summary>
		/// <param name="left">Left clipping plane.</param>
		/// <param name="right">Right clipping plane.</param>
		/// <param name="bottom">Bottom clipping plane.</param>
		/// <param name="top">Top clipping plane.</param>
		/// <param name="zNear">Near clipping plane.</param>
		/// <param name="zFar">Far clipping plane.</param>
		public void Ortho(double left, double right, double bottom, 
			double top, double zNear, double zFar)
		{
			PreErrorCheck();
			glOrtho(left, right, bottom, top, zNear, zFar);
			ErrorCheck();
		}
		/// <summary>
		/// This function creates an orthographic project based on a screen size.
		/// </summary>
		/// <param name="left">Left of the screen. (Normally 0).</param>
		/// <param name="right">Right of the screen.(Normally width).</param>
		/// <param name="bottom">Bottom of the screen (normally 0).</param>
		/// <param name="top">Top of the screen (normally height).</param>
		public void Ortho2D(double left, double right, double bottom, double top)
		{
			PreErrorCheck();
			gluOrtho2D(left, right, bottom, top);
			ErrorCheck();
		}
		/// <summary>
		/// This function draws a partial disk from the quadric object.
		/// </summary>
		/// <param name="qobj">The Quadric objec.t</param>
		/// <param name="innerRadius">Radius of the inside of the disk.</param>
		/// <param name="outerRadius">Radius of the outside of the disk.</param>
		/// <param name="slices">The slices.</param>
		/// <param name="loops">The loops.</param>
		/// <param name="startAngle">Starting angle.</param>
		/// <param name="sweepAngle">Sweep angle.</param>
		public void PartialDisk(IntPtr qobj,double innerRadius,double outerRadius, int slices, int loops, double startAngle, double sweepAngle)
		{
			PreErrorCheck();
			gluPartialDisk(qobj, innerRadius, outerRadius, slices, loops, startAngle, sweepAngle);
			ErrorCheck();
		}
		public void PassThrough (float token){}
		/// <summary>
		/// This function creates a perspective matrix and multiplies it to the current
		/// matrix stack (which in most cases should be 'PROJECTION').
		/// </summary>
		/// <param name="fovy">Field of view angle (human eye = 60 Degrees).</param>
		/// <param name="aspect">Apsect Ratio (width of screen divided by height of screen).</param>
		/// <param name="zNear">Near clipping plane (normally 1).</param>
		/// <param name="zFar">Far clipping plane.</param>
		public void Perspective(double fovy, double aspect, double zNear, double zFar)
		{
			PreErrorCheck();
			gluPerspective(fovy, aspect, zNear, zFar);
			ErrorCheck();
		}
		/// <summary>
		/// This function creates a 'pick matrix' normally used for selecting objects that
		/// are at a certain point on the screen.
		/// </summary>
		/// <param name="x">X Point.</param>
		/// <param name="y">Y Point.</param>
		/// <param name="width">Width of point to test (4 is normal).</param>
		/// <param name="height">Height of point to test (4 is normal).</param>
		/// <param name="viewport">The current viewport.</param>
		public void PickMatrix(double x, double y, double width, double height, int[] viewport)
		{
			PreErrorCheck();
			gluPickMatrix(x, y, width, height, viewport);
			ErrorCheck();
		}
		public void PixelMapfv (uint map, int mapsize,  float []values){}
		public void PixelMapuiv (uint map, int mapsize,  uint []values){}
		public void PixelMapusv (uint map, int mapsize,  ushort []values){}
		public void PixelStoref (uint pname, float param){}
		public void PixelStorei (uint pname, int param){}
		public void PixelTransferf (uint pname, float param){}
		public void PixelTransferi (uint pname, int param){}
		public void PixelZoom (float xfactor, float yfactor){}
		/// <summary>
		/// The size of points to be rasterised.
		/// </summary>
		/// <param name="size">Size in pixels.</param>
		public void PointSize(float size)
		{
			PreErrorCheck();
			glPointSize(size);
			ErrorCheck();
		}
		/// <summary>
		/// This sets the current drawing mode of polygons (points, lines, filled).
		/// </summary>
		/// <param name="face">The faces this applies to (front, back or both).</param>
		/// <param name="mode">The mode to set to (points, lines, or filled).</param>
		public void PolygonMode(uint face, uint mode)
		{
			PreErrorCheck();
			glPolygonMode(face, mode);
			ErrorCheck();
		}
		public void PolygonOffset (float factor, float units){}
		public void PolygonStipple ( byte []mask){}
		/// <summary>
		/// This function restores the attribute stack to the state it was when
		/// PushAttrib was called.
		/// </summary>
		public void PopAttrib()
		{
			PreErrorCheck();
			glPopAttrib();
			ErrorCheck();
		}
		public void PopClientAttrib (){}
		/// <summary>
		/// Restore the previously saved state of the current matrix stack.
		/// </summary>
		public void PopMatrix()
		{
			PreErrorCheck();
			glPopMatrix();
			ErrorCheck();
		}
		/// <summary>
		/// This takes the top name off the selection names stack.
		/// </summary>
		public void PopName()
		{
			PreErrorCheck();
			glPopName();
			ErrorCheck();
		}
		public void PrioritizeTextures (int n,  uint []textures,  float []priorities){}
		/// <summary>
		/// This function Maps the specified object coordinates into window coordinates.
		/// </summary>
		/// <param name="objx">The object's x coord.</param>
		/// <param name="objy">The object's y coord.</param>
		/// <param name="objz">The object's z coord.</param>
		/// <param name="modelMatrix">The modelview matrix.</param>
		/// <param name="projMatrix">The projection matrix.</param>
		/// <param name="viewport">The viewport.</param>
		/// <param name="winx">The window x coord.</param>
		/// <param name="winy">The Window y coord.</param>
		/// <param name="winz">The Window z coord.</param>
		public void Project(double objx, double objy, double objz, double[] modelMatrix, double[] projMatrix, int[] viewport, double[] winx, double[] winy, double[] winz)
		{
			PreErrorCheck();
			gluProject(objx, objy, objz, modelMatrix, projMatrix, viewport, winx, winy, winz);
			ErrorCheck();
		}
		/// <summary>
		/// This is a SharpGL helper version, that projects the vertex passed, using the
		/// current matrixes.
		/// </summary>
		/// <param name="vertex">The object coordinates.</param>
		/// <returns>The screen coords.</returns>
		public Vertex Project(Vertex vertex)
		{
			PreErrorCheck();
			
			//	THIS CODE MUST BE TESTED
			double[] modelview = new double[16];
			double[] projection = new double[16];
			int[] viewport = new int[4];
			GetDouble(MODELVIEW_MATRIX, modelview);
			GetDouble(PROJECTION_MATRIX, projection);
			GetInteger(VIEWPORT, viewport);
			double[] x = new double[1];	//	kludgy
			double[] y = new double[1];
			double[] z = new double[1];
			gluProject(vertex.X, vertex.Y, vertex.Z, 
				modelview, projection, viewport, x, y, z);

			ErrorCheck();

			return new Vertex((float)x[0], (float)y[0], (float)z[0]);
		}
		/// <summary>
		/// Save the current state of the attribute groups specified by 'mask'.
		/// </summary>
		/// <param name="mask">The attibute groups to save.</param>
		public void PushAttrib(uint mask)
		{
			PreErrorCheck();
			glPushAttrib(mask);
			ErrorCheck();
		}
		public void PushClientAttrib (uint mask){}
		/// <summary>
		/// Save the current state of the current matrix stack.
		/// </summary>
		public void PushMatrix()
		{
			PreErrorCheck();
			glPushMatrix();
			ErrorCheck();
		}
		/// <summary>
		/// This function adds a new name to the selection buffer.
		/// </summary>
		/// <param name="name">The name to add.</param>
		public void PushName(uint name)
		{
			PreErrorCheck();
			glPushName(name);
			ErrorCheck();
		}
		/// <summary>
		/// This set's the Generate Normals propery of the specified Quadric object.
		/// </summary>
		/// <param name="quadricObject">The quadric object.</param>
		/// <param name="normals">The type of normals to generate.</param>
		public void QuadricNormals(IntPtr quadricObject, int normals)
		{
			PreErrorCheck();
			gluQuadricNormals(quadricObject, normals);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets the type of texture coordinates being generated by
		/// the specified quadric object.
		/// </summary>
		/// <param name="quadricObject">The quadric object.</param>
		/// <param name="textureCoords">The type of coordinates to generate.</param>
		public void QuadricTexture(IntPtr quadricObject, int textureCoords)
		{
			PreErrorCheck();
			gluQuadricTexture(quadricObject, textureCoords);
			ErrorCheck();
		}
		/// <summary>
		/// This sets the orientation for the quadric object.
		/// </summary>
		/// <param name="quadricObject">The quadric object.</param>
		/// <param name="orientation">The orientation.</param>
		public void QuadricOrientation(IntPtr quadricObject, int orientation)
		{
			PreErrorCheck();
			gluQuadricOrientation(quadricObject, orientation);
			ErrorCheck();
		}
		/// <summary>
		/// This sets the current drawstyle for the Quadric Object.
		/// </summary>
		/// <param name="quadObject">The quadric object.</param>
		/// <param name="drawStyle">The draw style.</param>
		public void QuadricDrawStyle (IntPtr quadObject, uint drawStyle)
		{
			PreErrorCheck();
			gluQuadricDrawStyle(quadObject, drawStyle);
			ErrorCheck();
		}
		public void RasterPos2d (double x, double y){}
		public void RasterPos2dv ( double []v){}
		public void RasterPos2f (float x, float y){}
		public void RasterPos2fv ( float []v){}
		/// <summary>
		/// This function sets the current raster position.
		/// </summary>
		/// <param name="x">X coordinate.</param>
		/// <param name="y">Y coordinate.</param>
		public void RasterPos(int x, int y)
		{
			PreErrorCheck();
			glRasterPos2i(x, y);
			ErrorCheck();
		}
		public void RasterPos2iv ( int []v){}
		public void RasterPos2s (short x, short y){}
		public void RasterPos2sv ( short []v){}
		public void RasterPos3d (double x, double y, double z){}
		public void RasterPos3dv ( double []v){}
		public void RasterPos3f (float x, float y, float z){}
		public void RasterPos3fv ( float []v){}
		public void RasterPos3i (int x, int y, int z){}
		public void RasterPos3iv ( int []v){}
		public void RasterPos3s (short x, short y, short z){}
		public void RasterPos3sv ( short []v){}
		public void RasterPos4d (double x, double y, double z, double w){}
		public void RasterPos4dv ( double []v){}
		public void RasterPos4f (float x, float y, float z, float w){}
		public void RasterPos4fv ( float []v){}
		public void RasterPos4i (int x, int y, int z, int w){}
		public void RasterPos4iv ( int []v){}
		public void RasterPos4s (short x, short y, short z, short w){}
		public void RasterPos4sv ( short []v){}
		public void ReadBuffer (uint mode){}
		public void ReadPixels(int x, int y, int width, int height, uint format, uint type, float[] pixels){}
		/// <summary>
		/// Draw a rectangle from two coordinates (top-left and bottom-right).
		/// </summary>
		/// <param name="x1">Top-Left X value.</param>
		/// <param name="y1">Top-Left Y value.</param>
		/// <param name="x2">Bottom-Right X Value.</param>
		/// <param name="y2">Bottom-Right Y Value.</param>
		public void Rect(double x1, double y1, double x2, double y2)
		{
			PreErrorCheck();
			glRectd(x1, y1, x2, y2);
			ErrorCheck();
		}
		/// <summary>
		/// Draw a rectangle from two coordinates, expressed as arrays, e.g
		/// Rect(new float[] {0, 0}, new float[] {10, 10});
		/// </summary>
		/// <param name="v1">Top-Left point.</param>
		/// <param name="v2">Bottom-Right point.</param>
		public void Rect( double []v1,  double []v2)
		{
			PreErrorCheck();
			glRectdv(v1, v2);
			ErrorCheck();
		}
		/// <summary>
		/// Draw a rectangle from two coordinates (top-left and bottom-right).
		/// </summary>
		/// <param name="x1">Top-Left X value.</param>
		/// <param name="y1">Top-Left Y value.</param>
		/// <param name="x2">Bottom-Right X Value.</param>
		/// <param name="y2">Bottom-Right Y Value.</param>
		public void Rect(float x1, float y1, float x2, float y2)
		{
			PreErrorCheck();
			glRectd(x1, y1, x2, y2);
			ErrorCheck();
		}
		/// <summary>
		/// Draw a rectangle from two coordinates, expressed as arrays, e.g
		/// Rect(new float[] {0, 0}, new float[] {10, 10});
		/// </summary>
		/// <param name="v1">Top-Left point.</param>
		/// <param name="v2">Bottom-Right point.</param>
		public void Rect(float []v1,  float []v2)
		{
			PreErrorCheck();
			glRectfv(v1, v2);
			ErrorCheck();
		}
		/// <summary>
		/// Draw a rectangle from two coordinates (top-left and bottom-right).
		/// </summary>
		/// <param name="x1">Top-Left X value.</param>
		/// <param name="y1">Top-Left Y value.</param>
		/// <param name="x2">Bottom-Right X Value.</param>
		/// <param name="y2">Bottom-Right Y Value.</param>
		public void Rect(int x1, int y1, int x2, int y2)
		{
			PreErrorCheck();
			glRecti(x1, y1, x2, y2);
			ErrorCheck();
		}
		/// <summary>
		/// Draw a rectangle from two coordinates, expressed as arrays, e.g
		/// Rect(new float[] {0, 0}, new float[] {10, 10});
		/// </summary>
		/// <param name="v1">Top-Left point.</param>
		/// <param name="v2">Bottom-Right point.</param>
		public void Rect( int []v1,  int []v2)
		{
			PreErrorCheck();
			glRectiv(v1, v2);
			ErrorCheck();
		}
		/// <summary>
		/// Draw a rectangle from two coordinates (top-left and bottom-right).
		/// </summary>
		/// <param name="x1">Top-Left X value.</param>
		/// <param name="y1">Top-Left Y value.</param>
		/// <param name="x2">Bottom-Right X Value.</param>
		/// <param name="y2">Bottom-Right Y Value.</param>
		public void Rect(short x1, short y1, short x2, short y2)
		{
			PreErrorCheck();
			glRects(x1, y1, x2, y2);
			ErrorCheck();
		}
		/// <summary>
		/// Draw a rectangle from two coordinates, expressed as arrays, e.g
		/// Rect(new float[] {0, 0}, new float[] {10, 10});
		/// </summary>
		/// <param name="v1">Top-Left point.</param>
		/// <param name="v2">Bottom-Right point.</param>
		public void Rect(short []v1, short []v2)
		{
			PreErrorCheck();
			glRectsv(v1, v2);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets the current render mode (render, feedback or select).
		/// </summary>
		/// <param name="mode">The Render mode (RENDER, SELECT or FEEDBACK).</param>
		/// <returns>The hits that selection or feedback caused..</returns>
		public int RenderMode(uint mode)
		{
			PreErrorCheck();
			int hits = glRenderMode(mode);
			ErrorCheck();
			return hits;
		}
		/// <summary>
		/// This function applies a rotation transformation to the current matrix.
		/// </summary>
		/// <param name="angle">The angle to rotate.</param>
		/// <param name="x">Amount along x.</param>
		/// <param name="y">Amount along y.</param>
		/// <param name="z">Amount along z.</param>
		public void Rotate(double angle, double x, double y, double z)
		{
			PreErrorCheck();
			glRotated(angle, x, y, z);
			ErrorCheck();
		}
		/// <summary>
		/// This function applies a rotation transformation to the current matrix.
		/// </summary>
		/// <param name="angle">The angle to rotate.</param>
		/// <param name="x">Amount along x.</param>
		/// <param name="y">Amount along y.</param>
		/// <param name="z">Amount along z.</param>
		public void Rotate(float angle, float x, float y, float z)
		{
			PreErrorCheck();
			glRotatef(angle, x, y, z);
			ErrorCheck();
		}
		/// <summary>
		/// This function quickly does three rotations, one about each axis, with the
		/// given angles (it's not an OpenGL function, but very useful).
		/// </summary>
		/// <param name="anglex">The angle to rotate about x.</param>
		/// <param name="angley">The angle to rotate about y.</param>
		/// <param name="anglez">The angle to rotate about z.</param>
		public void Rotate(float anglex, float angley, float anglez)
		{
			PreErrorCheck();
			glRotatef(anglex, 1, 0, 0);
			glRotatef(angley, 0, 1, 0);
			glRotatef(anglez, 0, 0, 1);
			ErrorCheck();
		}
		/// <summary>
		/// This function applies a scale transformation to the current matrix.
		/// </summary>
		/// <param name="x">The amount to scale along x.</param>
		/// <param name="y">The amount to scale along y.</param>
		/// <param name="z">The amount to scale along z.</param>
		public void Scale(double x, double y, double z)
		{
			PreErrorCheck();
			glScaled(x, y, z);
			ErrorCheck();
		}
		/// <summary>
		/// This function applies a scale transformation to the current matrix.
		/// </summary>
		/// <param name="x">The amount to scale along x.</param>
		/// <param name="y">The amount to scale along y.</param>
		/// <param name="z">The amount to scale along z.</param>
		public void Scale(float x, float y, float z)
		{
			PreErrorCheck();
			glScalef(x, y, z);
			ErrorCheck();
		}
		public void Scissor (int x, int y, int width, int height){}
		/// <summary>
		/// This function sets the current select buffer.
		/// </summary>
		/// <param name="size">The size of the buffer you are passing.</param>
		/// <param name="buffer">The buffer itself.</param>
		public void SelectBuffer(int size, uint[] buffer)
		{
			PreErrorCheck();
			glSelectBuffer(size, buffer);
			ErrorCheck();
		}
		public void ShadeModel (uint mode){}
		/// <summary>
		/// This function draws a sphere from a Quadric Object.
		/// </summary>
		/// <param name="qobj">The quadric object.</param>
		/// <param name="radius">Sphere radius.</param>
		/// <param name="slices">Slices of the sphere.</param>
		/// <param name="stacks">Stakcs of the sphere.</param>
		public void Sphere(IntPtr qobj, double radius, int slices, int stacks)
		{
			PreErrorCheck();
			gluSphere(qobj, radius, slices, stacks);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets the current stencil buffer function.
		/// </summary>
		/// <param name="func">The function type.</param>
		/// <param name="reference">The function reference.</param>
		/// <param name="mask">The function mask.</param>
		public void StencilFunc(uint func, int reference, uint mask)
		{
			PreErrorCheck();
			glStencilFunc(func, reference, mask);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets the stencil buffer mask.
		/// </summary>
		/// <param name="mask">The mask.</param>
		public void StencilMask(uint mask)
		{
			PreErrorCheck();
			glStencilMask(mask);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets the stencil buffer operation.
		/// </summary>
		/// <param name="fail">Fail operation.</param>
		/// <param name="zfail">Depth fail component.</param>
		/// <param name="zpass">Depth pass component.</param>
		public void StencilOp(uint fail, uint zfail, uint zpass)
		{
			PreErrorCheck();
			glStencilOp(fail, zfail, zpass);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets the current texture coordinates.
		/// </summary>
		/// <param name="s">Texture Coordinate.</param>
		public void TexCoord(double s)
		{
			PreErrorCheck();
			glTexCoord1d(s);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets the current texture coordinates.
		/// </summary>
		/// <param name="v">Array of 1,2,3 or 4 Texture Coordinates.</param>
		public void TexCoord(double []v)
		{
			PreErrorCheck();
			if(v.Length == 1)
				glTexCoord1dv(v);
			else if(v.Length == 2)
				glTexCoord2dv(v);
			else if(v.Length == 3)
				glTexCoord3dv(v);
			else if(v.Length == 4)
				glTexCoord4dv(v);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets the current texture coordinates.
		/// </summary>
		/// <param name="s">Texture Coordinate.</param>
		public void TexCoord(float s)
		{
			PreErrorCheck();
			glTexCoord1f(s);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets the current texture coordinates. WARNING: if you
		/// can call something more explicit, like TexCoord2f then call that, it's
		/// much faster.
		/// </summary>
		/// <param name="v">Array of 1,2,3 or 4 Texture Coordinates.</param>
		public void TexCoord(float[] v)
		{
			PreErrorCheck();
			if(v.Length == 1)
				glTexCoord1fv(v);
			else if(v.Length == 2)
				glTexCoord2fv(v);
			else if(v.Length == 3)
				glTexCoord3fv(v);
			else if(v.Length == 4)
				glTexCoord4fv(v);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets the current texture coordinates, explicitly for 
		/// uv textures (i.e two floats) if you can use this explicitly, do it
		/// it is significantly faster than the Texcoord function.
		/// </summary>
		/// <param name="v">Two floats.</param>
		public void TexCoord2f(float[] v)
		{
			PreErrorCheck();
			glTexCoord2fv(v);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets the current texture coordinates.
		/// </summary>
		/// <param name="s">Texture Coordinate.</param>
		public void TexCoord(int s)
		{
			PreErrorCheck();
			glTexCoord1i(s);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets the current texture coordinates.
		/// </summary>
		/// <param name="v">Array of 1,2,3 or 4 Texture Coordinates.</param>
		public void TexCoord(int[] v)
		{
			PreErrorCheck();
			if(v.Length == 1)
				glTexCoord1iv(v);
			else if(v.Length == 2)
				glTexCoord2iv(v);
			else if(v.Length == 3)
				glTexCoord3iv(v);
			else if(v.Length == 4)
				glTexCoord4iv(v);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets the current texture coordinates.
		/// </summary>
		/// <param name="s">Texture Coordinate.</param>
		public void TexCoord(short s)
		{
			PreErrorCheck();
			glTexCoord1s(s);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets the current texture coordinates.
		/// </summary>
		/// <param name="v">Array of 1,2,3 or 4 Texture Coordinates.</param>
		public void TexCoord(short[] v)
		{
			PreErrorCheck();
			if(v.Length == 1)
				glTexCoord1sv(v);
			else if(v.Length == 2)
				glTexCoord2sv(v);
			else if(v.Length == 3)
				glTexCoord3sv(v);
			else if(v.Length == 4)
				glTexCoord4sv(v);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets the current texture coordinates.
		/// </summary>
		/// <param name="s">Texture Coordinate.</param>
		/// <param name="t">Texture Coordinate.</param>
		public void TexCoord(double s, double t)
		{
			PreErrorCheck();
			glTexCoord2d(s, t);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets the current texture coordinates.
		/// </summary>
		/// <param name="s">Texture Coordinate.</param>
		/// <param name="t">Texture Coordinate.</param>
		public void TexCoord2f(float s, float t)
		{
			PreErrorCheck();
			glTexCoord2f(s, t);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets the current texture coordinates.
		/// </summary>
		/// <param name="s">Texture Coordinate.</param>
		/// <param name="t">Texture Coordinate.</param>
		public void TexCoord(int s, int t)
		{
			PreErrorCheck();
			glTexCoord2i(s, t);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets the current texture coordinates.
		/// </summary>
		/// <param name="s">Texture Coordinate.</param>
		/// <param name="t">Texture Coordinate.</param>
		public void TexCoord(short s, short t)
		{
			PreErrorCheck();
			glTexCoord2s(s, t);
			ErrorCheck();
		}
		public void TexCoord3d (double s, double t, double r){}
		public void TexCoord3f (float s, float t, float r){}
		public void TexCoord3i (int s, int t, int r){}
		public void TexCoord3s (short s, short t, short r){}
		public void TexCoord4d (double s, double t, double r, double q){}
		public void TexCoord4f (float s, float t, float r, float q){}
		public void TexCoord4i (int s, int t, int r, int q){}
		public void TexCoord4s (short s, short t, short r, short q){}
		/// <summary>
		/// This function sets the texture coord array.
		/// </summary>
		/// <param name="size">The number of coords per set.</param>
		/// <param name="type">The type of data.</param>
		/// <param name="stride">The number of bytes between coords.</param>
		/// <param name="pointer">The coords.</param>
		public void TexCoordPointer(int size, uint type, int stride, float[] pointer)
		{
			PreErrorCheck();
			glTexCoordPointer(size, type, stride, pointer);
			ErrorCheck();
		}
		public void TexEnvf (uint target, uint pname, float param){}
		public void TexEnvfv (uint target, uint pname,  float []params_notkeyword){}
		public void TexEnvi (uint target, uint pname, int param){}
		public void TexEnviv (uint target, uint pname,  int []params_notkeyword){}
		public void TexGend (uint coord, uint pname, double param){}
		public void TexGendv (uint coord, uint pname,  double []params_notkeyword){}
		public void TexGenf (uint coord, uint pname, float param){}
		public void TexGenfv (uint coord, uint pname,  float []params_notkeyword){}
		public void TexGeni (uint coord, uint pname, int param){}
		public void TexGeniv (uint coord, uint pname,  int []params_notkeyword){}
		/// <summary>
		/// This function sets the image for the currently binded texture.
		/// </summary>
		/// <param name="target">The type of texture, TEXTURE_2D or PROXY_TEXTURE_2D.</param>
		/// <param name="level">For mip-map textures, ordinary textures should be '0'.</param>
		/// <param name="internalformat">The format of the data you are want OpenGL to create, e.g  RGB16.</param>
		/// <param name="width">The width of the texture image (must be a power of 2, e.g 64).</param>
		/// <param name="border">The width of the border (0 or 1).</param>
		/// <param name="format">The format of the data you are passing, e.g. RGBA.</param>
		/// <param name="type">The type of data you are passing, e.g GL_BYTE.</param>
		/// <param name="pixels">The actual pixel data.</param>
		public void TexImage1D(uint target, int level, int internalformat, int width, int border, uint format, uint type,  byte[] pixels)
		{
			PreErrorCheck();
			glTexImage1D(target, level, internalformat, width, border, format, type, pixels);
			ErrorCheck();
		}
		/// <summary>
		/// This function sets the image for the currently binded texture.
		/// </summary>
		/// <param name="target">The type of texture, TEXTURE_2D or PROXY_TEXTURE_2D.</param>
		/// <param name="level">For mip-map textures, ordinary textures should be '0'.</param>
		/// <param name="internalformat">The format of the data you are want OpenGL to create, e.g  RGB16.</param>
		/// <param name="width">The width of the texture image (must be a power of 2, e.g 64).</param>
		/// <param name="height">The height of the texture image (must be a power of 2, e.g 32).</param>
		/// <param name="border">The width of the border (0 or 1).</param>
		/// <param name="format">The format of the data you are passing, e.g. RGBA.</param>
		/// <param name="type">The type of data you are passing, e.g GL_BYTE.</param>
		/// <param name="pixels">The actual pixel data.</param>
		public void TexImage2D(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, byte[] pixels)
		{
			PreErrorCheck();
			glTexImage2D(target, level, internalformat, width, height, border, format, type, pixels);
			ErrorCheck();
		}
        /// <summary>
        /// This function sets the image for the currently binded texture.
        /// </summary>
        /// <param name="target">The type of texture, TEXTURE_2D or PROXY_TEXTURE_2D.</param>
        /// <param name="level">For mip-map textures, ordinary textures should be '0'.</param>
        /// <param name="internalformat">The format of the data you are want OpenGL to create, e.g  RGB16.</param>
        /// <param name="width">The width of the texture image (must be a power of 2, e.g 64).</param>
        /// <param name="height">The height of the texture image (must be a power of 2, e.g 32).</param>
        /// <param name="border">The width of the border (0 or 1).</param>
        /// <param name="format">The format of the data you are passing, e.g. RGBA.</param>
        /// <param name="type">The type of data you are passing, e.g GL_BYTE.</param>
        /// <param name="pixels">The actual pixel data.</param>
        public void TexImage2D(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, IntPtr pixels)
        {
            PreErrorCheck();
            glTexImage2D(target, level, internalformat, width, height, border, format, type, pixels);
            ErrorCheck();
        }
		/// <summary>
		///	This function sets the parameters for the currently binded texture object.
		/// </summary>
		/// <param name="target">The type of texture you are setting the parameter to, e.g. TEXTURE_2D</param>
		/// <param name="pname">The parameter to set.</param>
		/// <param name="param">The value to set it to.</param>
		public void TexParameter(uint target, uint pname, float param)
		{
			PreErrorCheck();
			glTexParameterf(target, pname, param);
			ErrorCheck();
		}
		/// <summary>
		///	This function sets the parameters for the currently binded texture object.
		/// </summary>
		/// <param name="target">The type of texture you are setting the parameter to, e.g. TEXTURE_2D</param>
		/// <param name="pname">The parameter to set.</param>
		/// <param name="parameters">The value to set it to.</param>
		public void TexParameter(uint target, uint pname,  float[] parameters)
		{
			PreErrorCheck();
			glTexParameterfv(target, pname, parameters);
			ErrorCheck();
		}
		/// <summary>
		///	This function sets the parameters for the currently binded texture object.
		/// </summary>
		/// <param name="target">The type of texture you are setting the parameter to, e.g. TEXTURE_2D</param>
		/// <param name="pname">The parameter to set.</param>
		/// <param name="param">The value to set it to.</param>
		public void TexParameter(uint target, uint pname, int param)
		{
			PreErrorCheck();
			glTexParameteri(target, pname, param);
			ErrorCheck();
		}
		/// <summary>
		///	This function sets the parameters for the currently binded texture object.
		/// </summary>
		/// <param name="target">The type of texture you are setting the parameter to, e.g. TEXTURE_2D</param>
		/// <param name="pname">The parameter to set.</param>
		/// <param name="parameters">The value to set it to.</param>
		public void TexParameter(uint target, uint pname,  int[] parameters)
		{
			PreErrorCheck();
			glTexParameteriv(target, pname, parameters);
			ErrorCheck();
		}
		public void TexSubImage1D (uint target, int level, int xoffset, int width, uint format, uint type,  int[] pixels){}
		public void TexSubImage2D (uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type,  int[] pixels){}
		/// <summary>
		/// This function applies a translation transformation to the current matrix.
		/// </summary>
		/// <param name="x">The amount to translate along the x axis.</param>
		/// <param name="y">The amount to translate along the y axis.</param>
		/// <param name="z">The amount to translate along the z axis.</param>
		public void Translate(double x, double y, double z)
		{
			PreErrorCheck();
			glTranslated(x, y, z);
			ErrorCheck();
		}
		/// <summary>
		/// This function applies a translation transformation to the current matrix.
		/// </summary>
		/// <param name="x">The amount to translate along the x axis.</param>
		/// <param name="y">The amount to translate along the y axis.</param>
		/// <param name="z">The amount to translate along the z axis.</param>
		public void Translate(float x, float y, float z)
		{
			PreErrorCheck();
			glTranslatef(x, y, z);
			ErrorCheck();
		}
		/// <summary>
		/// This function turns a screen Coordinate into a world coordinate.
		/// </summary>
		/// <param name="winx">Screen Coordinate.</param>
		/// <param name="winy">Screen Coordinate.</param>
		/// <param name="winz">Screen Coordinate.</param>
		/// <param name="modelMatrix">Current ModelView matrix.</param>
		/// <param name="projMatrix">Current Projection matrix.</param>
		/// <param name="viewport">Current Viewport.</param>
		/// <param name="objx">The world coordinate.</param>
		/// <param name="objy">The world coordinate.</param>
		/// <param name="objz">The world coordinate.</param>
		public void UnProject(double winx, double winy, double winz, 
			double[] modelMatrix, double[] projMatrix, int[] viewport, 
			double[] objx, double[] objy, double[] objz)
		{
			PreErrorCheck();
			gluUnProject(winx, winy, winz, modelMatrix, projMatrix, viewport,
				objx, objy, objz);
			ErrorCheck();
		}
		/// <summary>
		/// This is a convenience function. It calls UnProject with the current 
		/// viewport, modelview and persective matricies, saving you from getting them.
		/// To use you own matricies, all the other version of UnProject.
		/// </summary>
		/// <param name="winx">X Coordinate (Screen Coordinate).</param>
		/// <param name="winy">Y Coordinate (Screen Coordinate).</param>
		/// <param name="winz">Z Coordinate (Screen Coordinate).</param>
		/// <returns>The world coordinate.</returns>
		public double[] UnProject(double winx, double winy, double winz)
		{
			PreErrorCheck();
			//	THIS CODE MUST BE TESTED
			double[] modelview = new double[16];
			double[] projection = new double[16];
			int[] viewport = new int[4];
			GetDouble(MODELVIEW_MATRIX, modelview);
			GetDouble(PROJECTION_MATRIX, projection);
			GetInteger(VIEWPORT, viewport);
			double[] x = new double[1];	//	kludgy
			double[] y = new double[1];
			double[] z = new double[1];
			gluUnProject(winx, winy, winz, modelview, projection, viewport, x, y, z);

			ErrorCheck();

			return new double[] {x[0], y[0], z[0]};
		}
		/// <summary>
		/// Set the current vertex (must be called between 'Begin' and 'End').
		/// </summary>
		/// <param name="x">X Value.</param>
		/// <param name="y">Y Value.</param>
		public void Vertex(double x, double y)
		{
			PreErrorCheck();
			glVertex2d(x, y);
			ErrorCheck();
		}
		public void Vertex2dv ( double []v){}
		public void Vertex2f (float x, float y){}
		public void Vertex2i (int x, int y){}
		public void Vertex2iv ( int []v){}
		public void Vertex2s (short x, short y){}
		public void Vertex2sv ( short []v){}
		/// <summary>
		/// Set the current vertex (must be called between 'Begin' and 'End').
		/// </summary>
		/// <param name="x">X Value.</param>
		/// <param name="y">Y Value.</param>
		/// <param name="z">Z Value.</param>
		public void Vertex(double x, double y, double z)
		{
			PreErrorCheck();
			glVertex3d(x, y, z);
			ErrorCheck();
		}
		public void Vertex3dv ( double []v){}
		/// <summary>
		/// Set the current vertex (must be called between 'Begin' and 'End').
		/// </summary>
		/// <param name="x">X Value.</param>
		/// <param name="y">Y Value.</param>
		/// <param name="z">Z Value.</param>
		public void Vertex(float x, float y, float z)
		{
			PreErrorCheck();
			glVertex3f(x, y, z);
			ErrorCheck();
		}
		/// <summary>
		/// Sets the current vertex (must be called between 'Begin' and 'End').
		/// </summary>
		/// <param name="v">An array of 2, 3 or 4 floats.</param>
		public void Vertex(float []v)
		{
			PreErrorCheck();
			if(v.Length == 2)
				glVertex2fv(v);
			else if(v.Length == 3)
				glVertex3fv(v);
			else if(v.Length == 4)
				glVertex4fv(v);
			ErrorCheck();
		}
		public void Vertex3i (int x, int y, int z){}
		public void Vertex3iv ( int []v){}
		public void Vertex3s (short x, short y, short z){}
		public void Vertex3sv ( short []v){}
		public void Vertex4d (double x, double y, double z, double w){}
		public void Vertex4dv ( double []v){}
		public void Vertex4f (float x, float y, float z, float w){}
		public void Vertex4i (int x, int y, int z, int w){}
		public void Vertex4iv ( int []v){}
		public void Vertex4s (short x, short y, short z, short w){}
		public void Vertex4sv ( short []v){}
		/// <summary>
		/// This function sets the address of the vertex pointer array.
		/// </summary>
		/// <param name="size">The number of coords per vertex.</param>
		/// <param name="type">The data type.</param>
		/// <param name="stride">The byte offset between vertices.</param>
		/// <param name="pointer">The array.</param>
		public void VertexPointer(int size, uint type, int stride, float[] pointer)
		{
			PreErrorCheck();
			glVertexPointer(size, type, stride, pointer);
			ErrorCheck();
		}
		/// <summary>
		/// This sets the viewport of the current Render Context. Normally x and y are 0
		/// and the width and height are just those of the control/graphics you are drawing
		/// to.
		/// </summary>
		/// <param name="x">Top-Left point of the viewport.</param>
		/// <param name="y">Top-Left point of the viewport.</param>
		/// <param name="width">Width of the viewport.</param>
		/// <param name="height">Height of the viewport.</param>
		public void Viewport (int x, int y, int width, int height)
		{
			PreErrorCheck();
			glViewport(x, y, width, height);
			ErrorCheck();
		}
		
		#endregion

		#region The Wrapped GLU functions (Eventually should be moved to 'wrapped functions'

		public string ErrorString(int errCode){return "";}
		public string GetString(int name){return "";}
		public void ScaleImage ( int      format, int       widthin, int       heightin,  int      typein,  int  []datain, int       widthout, int       heightout, int      typeout, int[] dataout){}
		public void Build1DMipmaps (int      target, int       components, int       width, int      format, int      type,  int[] data){}
		public void Build2DMipmaps (int      target, int       components, int       width, int       height, int      format, int      type,  int[] data){}
		public void Disk(IntPtr qobj, double innerRadius,double outerRadius,int slices, int loops){}
		public IntPtr NewTess(){return IntPtr.Zero;}
		public void DeleteTess(IntPtr tess){}
		public void TessBeginPolygon(IntPtr tess, IntPtr polygonData){}
		public void TessBeginContour(IntPtr tess){}
		public void TessVertex(IntPtr tess,double[] coords, double[] data ){}
		public void TessEndContour(   IntPtr        tess ){}
		public void TessEndPolygon(   IntPtr        tess ){}
		public void TessProperty(     IntPtr        tess,int              which, double            value ){}
		public void TessNormal(       IntPtr        tess, double            x,double            y, double            z ){}
		public void GetTessProperty(  IntPtr        tess,int              which, double            value ){}
		public void BeginTrim (IntPtr            nobj){}
		public void EndTrim (IntPtr            nobj){}
		public void PwlCurve (IntPtr            nobj, int               count, float             array, int               stride, int              type){}
		public void LoadSamplingMatrices (IntPtr            nobj,  float[] modelMatrix,  float[] projMatrix, int[] viewport){}
		public void GetNurbsProperty (IntPtr            nobj, int              property, float             value ){}
		
		#endregion

		#region Error Checking

		/// <summary>
		/// Call this function before error checking a function.
		/// </summary>
		protected virtual void PreErrorCheck()
		{
			//	Get the current error (to clear it).
		//	GetError();
		}

		/// <summary>
		/// Call this after the function you want to check.
		/// </summary>
		/// <returns>True if there are no errors, false otherwise.</returns>
		protected virtual bool ErrorCheck()
		{
			//	This error check is very useful, as you can break anytime 
			//	an OpenGL error occurs, going through a program with this on
			//	can rid it of bugs. It's VERY slow though, as every call is monitored.

		/*	uint errorCode = GetError();
			

			//	What error is it?
			if(errorCode == OpenGL.NO_ERROR)
				return true;
			string errorMessage;
			//	Set this to true for any error you consider is important
			//	enough to warrant a break.
			bool breakDebugger = false;
			switch(errorCode)
			{
				case OpenGL.INVALID_ENUM:
					errorMessage = "A GLenum argument was out of range.";
					breakDebugger = true;
					break;
				case OpenGL.INVALID_VALUE:
					errorMessage = "A numeric argument was out of range.";
					breakDebugger = true;
					break;
				case OpenGL.INVALID_OPERATION:
					errorMessage = "Operation illeagal in current state.";
					break;
				case OpenGL.STACK_OVERFLOW:
					errorMessage = "Command would cause a stack overflow.";
					breakDebugger = true;
					break;
				case OpenGL.STACK_UNDERFLOW:
					errorMessage = "Command would cause a stack underflow.";
					breakDebugger = true;
					break;
				case OpenGL.OUT_OF_MEMORY:
					errorMessage = "Not enough memory left to execute command.";
					break;
				default:
					errorMessage = "Unknown Error";
					break;
			}

			System.Diagnostics.Debugger.Log(1, "OpenGL Error", errorMessage);
			if(breakDebugger)
				System.Diagnostics.Debugger.Break();
			
			return false;*/
			return true;
		}

		#endregion

		#region Stock Drawing

		protected StockDrawing stockDrawing = new StockDrawing();

		public StockDrawing StockDrawing
		{
			get {return stockDrawing;}
		}

		public void InitialiseStockDrawing()
		{
			stockDrawing.Create(this);
		}

		#endregion

		#region Utility Functions

		/// <summary>
		/// This function transforms a windows point into an OpenGL point,
		/// which is measured from the bottom left of the screen.
		/// </summary>
		/// <param name="x">The x coord.</param>
		/// <param name="y">The y coord.</param>
		public void GDItoOpenGL(ref int x, ref int y)
		{
			//	Create an array that will be the viewport.
			int[] viewport = new int[4];
			
			//	Get the viewport, then convert the mouse point to an opengl point.
			GetInteger(OpenGL.VIEWPORT, viewport);
			y = viewport[3] - y;
		}

		#endregion
	}
}
