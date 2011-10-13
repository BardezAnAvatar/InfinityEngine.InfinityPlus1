using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SharpGL
{
	public class DIBSection
	{
		#region Function Imports

		//	Unmanaged functions from LIBRARY_KERNEL (kernel32.dll).
		[DllImport("kernel32.dll")] 
		public static extern IntPtr LoadLibrary(string lpFileName);
		
		
		//	Unmanaged functions from the Win32 graphics library.
		[DllImport("gdi32.dll", SetLastError = true)] 
		public unsafe static extern int ChoosePixelFormat(IntPtr hDC, 
			[In, MarshalAs(UnmanagedType.LPStruct)] PIXELFORMATDESCRIPTOR ppfd);
		[DllImport("gdi32.dll", SetLastError = true)] 
		public unsafe static extern int SetPixelFormat(IntPtr hDC, int iPixelFormat, 
			[In, MarshalAs(UnmanagedType.LPStruct)] PIXELFORMATDESCRIPTOR ppfd );
		[DllImport("gdi32.dll")] 
		public static extern int SwapBuffers(IntPtr hDC);
		[DllImport("gdi32.dll", SetLastError = true)]
		protected static extern IntPtr CreateDIBSection(
			IntPtr hdc, 
			[In, MarshalAs(UnmanagedType.LPStruct)] BITMAPINFO pbmi, 
			uint iUsage, 
			out IntPtr ppvBits, 
			IntPtr hSection, 
			uint dwOffset);
		[DllImport("gdi32.dll")]
		protected static extern bool DeleteObject(IntPtr hObject);
		[DllImport("gdi32.dll", SetLastError = true)]
		protected static extern IntPtr CreateCompatibleDC(IntPtr hDC);
		[DllImport("gdi32.dll")]
		protected static extern bool DeleteDC(IntPtr hDC);
		[DllImport("gdi32.dll")]
		protected static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

		//	Unmanaged functions from user32.dll
		[DllImport("user32.dll")]
		public static extern IntPtr GetDC(IntPtr hWnd);
		[DllImport("user32.dll")] 
		public static extern int RelaseDC(IntPtr hWnd, IntPtr hDC);

		#endregion

		#region Pixel Format Descriptor

		[StructLayout(LayoutKind.Explicit)]
		public class PIXELFORMATDESCRIPTOR
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

		#region Bitmap Info

		[StructLayout(LayoutKind.Explicit)]
		public class BITMAPINFO
		{
			[FieldOffset(0)]
			public Int32 biSize;
			[FieldOffset(4)]
			public Int32 biWidth;
			[FieldOffset(8)]
			public Int32 biHeight;
			[FieldOffset(12)]
			public Int16 biPlanes;
			[FieldOffset(14)]
			public Int16 biBitCount;
			[FieldOffset(16)]
			public Int32 biCompression;
			[FieldOffset(20)]
			public Int32 biSizeImage;
			[FieldOffset(24)]
			public Int32 biXPelsPerMeter;
			[FieldOffset(28)]
			public Int32 biYPelsPerMeter;
			[FieldOffset(32)]
			public Int32 biClrUsed;
			[FieldOffset(36)]
			public Int32 biClrImportant;
			[FieldOffset(40)]
			public Int32 colors;
		}

		protected static uint BI_RGB = 0;
		protected static uint BI_RLE8 = 1;
		protected static uint BI_RLE4 = 2;
		protected static uint BI_BITFIELDS = 3;
		protected static uint BI_JPEG = 4;
		protected static uint BI_PNG = 5;
	
		protected static uint DIB_RGB_COLORS = 0;
		protected static uint DIB_PAL_COLORS = 1;

		#endregion

		public DIBSection()
		{
			//	Create a blank DC.
			IntPtr screenDC = GetDC(IntPtr.Zero);
			hDC = CreateCompatibleDC(screenDC);
		}

		~DIBSection()
		{
			//	Destroy the surface.
			Destroy();

			//	Destroy the DC.
			if(hDC != IntPtr.Zero)
			{
				DeleteDC(hDC);
				hDC = IntPtr.Zero;
			}
		}

		public virtual unsafe bool Create(int width, int height, uint bitCount)
		{
			this.width = width;
			this.height = height;

			//	Destroy existing objects.
			Destroy();
			
			//	Create a bitmap info structure.
			BITMAPINFO info = new BITMAPINFO();

			//	Set the data.
			info.biSize = 40;
			info.biBitCount = (Int16)bitCount;
			info.biPlanes = 1;
			info.biWidth = width;
			info.biHeight = height;

			//	Create the bitmap.
			IntPtr ppvBits;
			hBitmap = CreateDIBSection(hDC, info, DIB_RGB_COLORS,
				out ppvBits, IntPtr.Zero, 0);

			SelectObject(hDC, hBitmap);
			
			//	Set the OpenGL pixel format.
			SetPixelFormat(bitCount);

			return true;
		}

		/// <summary>
		/// This function sets the pixel format of the underlying bitmap.
		/// </summary>
		/// <param name="bitCount">The bitcount.</param>
		protected virtual bool SetPixelFormat(uint bitCount)
		{
			//	Create the big lame pixel format majoo.
			PIXELFORMATDESCRIPTOR pixelFormat = new PIXELFORMATDESCRIPTOR();

			//	Set the values for the pixel format.
			pixelFormat.nSize = 40;
			pixelFormat.nVersion  = 1;
			pixelFormat.dwFlags = (PFD_DRAW_TO_BITMAP | PFD_SUPPORT_OPENGL | PFD_SUPPORT_GDI);
			pixelFormat.iPixelType = PFD_TYPE_RGBA;
			pixelFormat.cColorBits = (byte)bitCount;
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
			pixelFormat.cDepthBits = 32;
			pixelFormat.cStencilBits = 0;
			pixelFormat.cAuxBuffers = 0;
			pixelFormat.iLayerType = PFD_MAIN_PLANE;
			pixelFormat.bReserved = 0;
			pixelFormat.dwLayerMask = 0;
			pixelFormat.dwVisibleMask = 0;
			pixelFormat.dwDamageMask = 0;

			//	Match an appropriate pixel format 
			int iPixelformat;
			if((iPixelformat = ChoosePixelFormat(hDC, pixelFormat)) == 0 )
				return false;

			//	Sets the pixel format
			if(SetPixelFormat(hDC, iPixelformat, pixelFormat) == 0)
			{
				int lastError = Marshal.GetLastWin32Error();
				return false;
			}

			return true;
		}

		public virtual void Destroy()
		{
			//	Destroy the bitmap.
			if(hBitmap != IntPtr.Zero)
			{
				DeleteObject(hBitmap);
				hBitmap = IntPtr.Zero;
			}
		}

		protected IntPtr hDC = IntPtr.Zero;
		protected IntPtr hBitmap = IntPtr.Zero;
		protected int width = 0;
		protected int height = 0;
		
		public IntPtr HDC
		{
			get {return hDC;}
		}
		public IntPtr HBitmap
		{
			get {return hBitmap;}
		}
		public int Width
		{
			get {return width;}
		}
		public int Height
		{
			get {return height;}
		}
	}
}