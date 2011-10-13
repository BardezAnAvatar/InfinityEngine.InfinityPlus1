//    Imports.cs
//    Created by Dave Kerr, 12/08/2003
//    Copyright (c) Dave Kerr 2003
//    http://www.codechamber.com

//	This is the class which OpenGL derives from. It imports all necessary functions.
//	Not all of the OpenGL functions are yet implementated in the derived class!

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Drawing;

using SharpGL.SceneGraph.Cameras;

namespace SharpGL
{
	/// <summary>
	/// Imports is simply a class that imports all of the opengl functions and constant
	/// definitions, as well as the GLU ones. The GL class derives from it.
	/// Use it to call 'native' OpenGL code (i.e no scene stuff).
	/// It also has a couple of functions for creating render contexts.
	/// </summary>
	public class Imports
	{
		#region Library strings and information.
		
		public const string LIBRARY_KERNEL = "kernel32.dll";
		public const string LIBRARY_OPENGL = "opengl32.dll";
		public const string LIBRARY_GLU = "Glu32.dll";
		public const string LIBRARY_GDI = "gdi32.dll";
		public const string LIBRARY_USER = "user32.dll";
		
		#endregion
		#region PixelFormatDescriptor structure and flags.
		
		#region PixelFormatDescriptor Structure.

		[StructLayout(LayoutKind.Explicit)]
			public class PIXELFORMATDESC
		{
			[FieldOffset(0)]
			public UInt16 nSize;
			[FieldOffset(2)]
			public UInt16 nVersion;
			[FieldOffset(4)]
			public UInt32 dwFlags;
			[FieldOffset(8)]
			public Byte iPixelType;
			[FieldOffset(9)]
			public Byte cColorBits;
			[FieldOffset(10)]
			public Byte cRedBits;
			[FieldOffset(11)]
			public Byte cRedShift;
			[FieldOffset(12)]
			public Byte cGreenBits;
			[FieldOffset(13)]
			public Byte cGreenShift;
			[FieldOffset(14)]
			public Byte cBlueBits;
			[FieldOffset(15)]
			public Byte cBlueShift;
			[FieldOffset(16)]
			public Byte cAlphaBits;
			[FieldOffset(17)]
			public Byte cAlphaShift;
			[FieldOffset(18)]
			public Byte cAccumBits;
			[FieldOffset(19)]
			public Byte cAccumRedBits;
			[FieldOffset(20)]
			public Byte cAccumGreenBits;
			[FieldOffset(21)]
			public Byte cAccumBlueBits;
			[FieldOffset(22)]
			public Byte cAccumAlphaBits;
			[FieldOffset(23)]
			public Byte cDepthBits;
			[FieldOffset(24)]
			public Byte cStencilBits;
			[FieldOffset(25)]
			public Byte cAuxBuffers;
			[FieldOffset(26)]
			public SByte iLayerType;
			[FieldOffset(27)]
			public Byte bReserved;
			[FieldOffset(28)]
			public UInt32 dwLayerMask;
			[FieldOffset(32)]
			public UInt32 dwVisibleMask;
			[FieldOffset(36)]
			public UInt32 dwDamageMask;
		}

		public struct PixelFormatDescriptor
		{
			public ushort nSize;
			public ushort nVersion;
			public uint   dwFlags;
			public byte   iPixelType;
			public byte   cColorBits;
			public byte   cRedBits;
			public byte   cRedShift;
			public byte   cGreenBits;
			public byte   cGreenShift;
			public byte   cBlueBits;
			public byte   cBlueShift;
			public byte   cAlphaBits;
			public byte   cAlphaShift;
			public byte   cAccumBits;
			public byte   cAccumRedBits;
			public byte   cAccumGreenBits;
			public byte   cAccumBlueBits;
			public byte   cAccumAlphaBits;
			public byte   cDepthBits;
			public byte   cStencilBits;
			public byte   cAuxBuffers;
			public sbyte  iLayerType;
			public byte   bReserved;
			public uint   dwLayerMask;
			public uint   dwVisibleMask;
			public uint   dwDamageMask;
		}

		#endregion

		#region PixelFormatDescriptor Flags

		protected const byte PFD_TYPE_RGBA			= 0;
		protected const byte PFD_TYPE_COLORINDEX		= 1;

		protected const uint PFD_DOUBLEBUFFER			= 1;
		protected const uint PFD_STEREO				= 2;
		protected const uint PFD_DRAW_TO_WINDOW		= 4;
		protected const uint PFD_DRAW_TO_BITMAP		= 8;
		protected const uint PFD_SUPPORT_GDI			= 16;
		protected const uint PFD_SUPPORT_OPENGL		= 32;
		protected const uint PFD_GENERIC_FORMAT		= 64;
		protected const uint PFD_NEED_PALETTE			= 128;
		protected const uint PFD_NEED_SYSTEM_PALETTE	= 256;
		protected const uint PFD_SWAP_EXCHANGE		= 512;
		protected const uint PFD_SWAP_COPY			= 1024;
		protected const uint PFD_SWAP_LAYER_BUFFERS	= 2048;
		protected const uint PFD_GENERIC_ACCELERATED	= 4096;
		protected const uint PFD_SUPPORT_DIRECTDRAW	= 8192;

		protected const sbyte PFD_MAIN_PLANE			= 0;
		protected const sbyte PFD_OVERLAY_PLANE		= 1;
		protected const sbyte PFD_UNDERLAY_PLANE		= -1;

		
		#endregion

		#endregion
		#region Win32 Function Definitions.

		//	Unmanaged functions from LIBRARY_KERNEL (kernel32.dll).
		[DllImport(LIBRARY_KERNEL)] public static extern IntPtr LoadLibrary(string lpFileName);
		
		//	Unmanaged functions from the OpenSharpGL library.
		[DllImport(LIBRARY_OPENGL)] public static extern IntPtr wglGetCurrentContext();
		[DllImport(LIBRARY_OPENGL)] public static extern int wglMakeCurrent( IntPtr hdc, IntPtr hrc );
		[DllImport(LIBRARY_OPENGL)] public static extern IntPtr wglCreateContext( IntPtr hdc );
		[DllImport(LIBRARY_OPENGL)] public static extern int  wglDeleteContext( IntPtr hrc );
		
		//	Unmanaged functions from the Win32 graphics library.
		[DllImport(LIBRARY_GDI, SetLastError = true)] 
		public unsafe static extern int ChoosePixelFormat(IntPtr hDC, 
			[In, MarshalAs(UnmanagedType.LPStruct)] PIXELFORMATDESC ppfd);
		[DllImport(LIBRARY_GDI, SetLastError = true)] 
		public unsafe static extern int SetPixelFormat(IntPtr hDC, int iPixelFormat, 
			[In, MarshalAs(UnmanagedType.LPStruct)] PIXELFORMATDESC ppfd );
		[DllImport(LIBRARY_GDI)] 
		public static extern int SwapBuffers(IntPtr hDC);
		[DllImport(LIBRARY_GDI)] 
		public static extern bool BitBlt(IntPtr hDC, int x, int y, int width, 
			int height, IntPtr hDCSource, int sourceX, int sourceY, uint type);

		//	Unmanaged functions from user32.dll
		[DllImport(LIBRARY_USER)] public static extern IntPtr GetDC(IntPtr hWnd);
		[DllImport(LIBRARY_USER)] public static extern int RelaseDC(IntPtr hWnd, IntPtr hDC);

		#endregion
		#region BitBlt Modes
	
		protected const uint SRCCOPY		= 0x00CC0020;	// dest = source                   
		protected const uint SRCPAINT		= 0x00EE0086;	// dest = source OR dest           
		protected const uint SRCAND		= 0x008800C6;	// dest = source AND dest          
		protected const uint SRCINVERT	= 0x00660046;	// dest = source XOR dest          
		protected const uint SRCERASE		= 0x00440328;	// dest = source AND (NOT dest )   
		protected const uint NOTSRCCOPY	= 0x00330008;	// dest = (NOT source)             
		protected const uint NOTSRCERASE	= 0x001100A6;	// dest = (NOT src) AND (NOT dest) 
		protected const uint MERGECOPY	= 0x00C000CA;	// dest = (source AND pattern)     
		protected const uint MERGEPAINT	= 0x00BB0226;	// dest = (NOT source) OR dest     
		protected const uint PATCOPY		= 0x00F00021;	// dest = pattern                  
		protected const uint PATPAINT		= 0x00FB0A09;	// dest = DPSnoo                   
		protected const uint PATINVERT	= 0x005A0049;	// dest = pattern XOR dest         
		protected const uint DSTINVERT	= 0x00550009;	// dest = (NOT dest)               
		protected const uint BLACKNESS	= 0x00000042;	// dest = BLACK                    
		protected const uint WHITENESS	= 0x00FF0062;	// dest = WHITE                    

		#endregion

		public Imports()
		{
			handleModuleOpenGL = LoadLibrary(LIBRARY_OPENGL);
		}

		~Imports()
		{

		}

		/// <summary>
		/// This function creates OpenGL from a handle to a device context.
		/// </summary>
		/// <param name="hDC">The handle to the device context.</param>
		/// <returns>True if creation was successful.</returns>
		protected virtual unsafe bool Create(IntPtr hDC)
		{
			//	Create the big lame pixel format majoo.
			PIXELFORMATDESC pixelFormat = new PIXELFORMATDESC();

			//	Set the values for the pixel format.
			pixelFormat.nSize = 40;
			pixelFormat.nVersion  = 1;
			pixelFormat.dwFlags = (PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER);
			pixelFormat.iPixelType = PFD_TYPE_RGBA;
			pixelFormat.cColorBits = 24;
			pixelFormat.cRedBits = 0;
			pixelFormat.cRedShift = 0;
			pixelFormat.cGreenBits = 0;
			pixelFormat.cGreenShift = 0;
			pixelFormat.cBlueBits = 0;
			pixelFormat.cBlueShift = 0;
			pixelFormat.cAlphaBits = 0;
			pixelFormat.cAlphaShift = 0;
			pixelFormat.cAccumBits = 0;
			pixelFormat.cAccumRedBits = 0;
			pixelFormat.cAccumGreenBits = 0;
			pixelFormat.cAccumBlueBits = 0;
			pixelFormat.cAccumAlphaBits = 0;
			pixelFormat.cDepthBits = 16;
			pixelFormat.cStencilBits = 0;
			pixelFormat.cAuxBuffers = 0;
			pixelFormat.iLayerType = PFD_MAIN_PLANE;
			pixelFormat.bReserved = 0;
			pixelFormat.dwLayerMask = 0;
			pixelFormat.dwVisibleMask = 0;
			pixelFormat.dwDamageMask = 0;

			//	Match an appropriate pixel format 
			int iPixelformat = ChoosePixelFormat(hDC, pixelFormat);
			if(iPixelformat == 0 )
				return false;

			//	Sets the pixel format
			if(SetPixelFormat(hDC, iPixelformat, pixelFormat) == 0)
				return false;

			//	Create a new OpenSharpGL rendering contex
			handleRenderContext = wglCreateContext(hDC);

			//	Make the OpenSharpGL rendering context the current rendering context
			wglMakeCurrent(hDC, handleRenderContext);
		
			return true;
		}

		/// <summary>
		/// Call this function to create an OpenSharpGL instance for a certain window.
		/// </summary>
		/// <param name="ctrl">A window, control etc.</param>
		/// <returns>True if OpenSharpGL sucessfully initialised, otherwise, false.</returns>
		public virtual /*unsafe*/ bool Create(System.Windows.Forms.Control ctrl)
		{
			//	Load the OpenGL DLL.
			//	handleModuleOpenGL = LoadLibrary(LIBRARY_OPENGL);

			//	Get a device context (the old, Win32 way).
			handleDeviceContext = GetDC(ctrl.Handle);

			return Create(handleDeviceContext);
		}
	
		/// <summary>
		/// Make this render context current.
		/// </summary>
		public void MakeCurrent()
		{
			if(handleRenderContext != IntPtr.Zero)
				wglMakeCurrent(handleDeviceContext, handleRenderContext);
		}

	
		/// <summary>
		/// Swap the buffers, for double buffered drawing.
		/// </summary>
		public void SwapBuffers()
		{
			//	Swap the buffers.
			SwapBuffers(handleDeviceContext);
		}

		public virtual unsafe bool Create(int width, int height)
		{	
			if(width == 0 || height == 0)
				return false;

			//	First of all create the bitmap.
			dib.Create(width, height, 24);

			//	Create a new OpenSharpGL rendering contex and set the DC.
			if(handleRenderContext == IntPtr.Zero)
				handleRenderContext = wglCreateContext(dib.HDC);
			handleDeviceContext = dib.HDC;

			//	Make the OpenSharpGL rendering context the current rendering context
			wglMakeCurrent(dib.HDC, handleRenderContext);

			//	Destroy the old GDI graphics and the old GDI bitmap.
			if(bitmapGDI != null)
				bitmapGDI.Dispose();
			if(graphicsGDI != null)
				graphicsGDI.Dispose();

			//	Now we create a GDI bitmap.
			bitmapGDI = new Bitmap(width, height);
			graphicsGDI = Graphics.FromImage(bitmapGDI);

			return true;
		}

		/// <summary>
		/// Call this function to draw the GDI bitmap onto the OpenGL bitmap.
		/// </summary>
		public virtual void BlitGDI()
		{
			if(graphicsGDI == null || dib.HDC == IntPtr.Zero)
				return;

			IntPtr hDCSource = graphicsGDI.GetHdc();
			BitBlt(dib.HDC, 0, 0, dib.Width, dib.Height, hDCSource, 0, 0, SRCCOPY);
			graphicsGDI.ReleaseHdc(hDCSource);
		}

		/// <summary>
		/// This is the internal DIB Section that OpenGL renders to.
		/// </summary>
		protected DIBSection dib = new DIBSection();

		/// <summary>
		/// This is the graphics object used to draw to the GDI Bitmap.
		/// </summary>
		protected Graphics graphicsGDI = null;

		/// <summary>
		/// This is the bitmap object used to for GDI drawing.
		/// </summary>
		protected Bitmap bitmapGDI = null;

		protected IntPtr handleDeviceContext = IntPtr.Zero;	//	Display device context
		protected IntPtr handleRenderContext = IntPtr.Zero;	//	OpenSharpGL rendering context
		protected IntPtr handleModuleOpenGL = IntPtr.Zero;		//	Handle to OPENGL32.DLL
		
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Description("This is the graphics object used to draw to the internal GDI bitmap"), Category("OpenGL")]
		public Graphics GDIGraphics
		{
			get {return graphicsGDI;}
		}
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Description("This is the graphics object used to draw to the internal GDI bitmap"), Category("OpenGL")]
		public Bitmap GDIBitmap
		{
			get {return bitmapGDI;}
		}
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Description("This is the graphics object used to draw to the internal GDI bitmap"), Category("OpenGL")]
		public Bitmap OpenGLBitmap
		{
			get {return Bitmap.FromHbitmap(dib.HBitmap);}
		}
	}

	namespace Delegates
	{
		namespace Tesselators
		{
			//	The functions.
			public delegate void Begin(uint mode);
			public delegate void EdgeFlag(bool flag);
			public delegate void Vertex(IntPtr data);
			public delegate void End();
			public delegate void Combine(double[] coords, IntPtr vertexData,	float[] weight, double[] outData);
			public delegate void Error(uint error);

			//	The functions, with user data.
			public delegate void BeginData(uint mode, IntPtr userData);
			public delegate void EdgeFlagData(bool flag, IntPtr userData);
			public delegate void VertexData(IntPtr data, IntPtr userData);
			public delegate void EndData(IntPtr userData);
			public delegate void CombineData(double[] coords, IntPtr vertexData,	float[] weight, double[] outData, IntPtr userData);
			public delegate void ErrorData(uint error, IntPtr userData);
		}
	}

}
