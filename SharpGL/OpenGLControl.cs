//    OpenGLCtrl.cs
//    Created by Dave Kerr, 12/08/2003
//    Copyright (c) Dave Kerr 2003
//    http://www.codechamber.com

//	This is the core control, check it out...
//	By the way, ANYONE who can help with my persistent problem of having flickering
//	GDI code, even though I render to an offscreen DC then blit it please email me
//	(focus_business@hotmail.com) cause I really want this problem sorted, the GDI
//	and OpenGL drawing working together is a really powerful feature, that is weakened
//	by the annoying flickering!


using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using SharpGL.SceneGraph.Interaction;
using SharpGL.SceneGraph.Collections;

namespace SharpGL
{
   	/// <summary>
	/// This is the basic OpenGL control object, it gives all of the basic OpenGL functionality.
	/// </summary>
	public class OpenGLCtrl : System.Windows.Forms.UserControl
	{	
		private System.ComponentModel.IContainer components;

        public OpenGLCtrl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            //  Set the user draw styles.
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);

            //	Create OpenGL.
            gl.Create(Width, Height);
		
            //  Set the most basic OpenGL styles.
            gl.ShadeModel(OpenGL.SMOOTH);
            gl.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            gl.ClearDepth(1.0f);
            gl.Enable(OpenGL.DEPTH_TEST);
            gl.DepthFunc(OpenGL.LEQUAL);
            gl.Hint(OpenGL.PERSPECTIVE_CORRECTION_HINT, OpenGL.NICEST);
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.timerDrawing = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timerDrawing
            // 
            this.timerDrawing.Enabled = true;
            this.timerDrawing.Tick += new System.EventHandler(this.timerDrawing_Tick);
            // 
            // OpenGLCtrl
            // 
            this.Name = "OpenGLCtrl";
            this.ResumeLayout(false);

		}
		#endregion

		protected override void OnPaint(PaintEventArgs e)
		{
			//	If we're doing GDI drawing, clear it now.
			if(gdiEnabled)
				OpenGL.GDIGraphics.Clear(Color.Transparent);

			//	Get the time (for speed checking reasons).
			System.DateTime timeBefore = System.DateTime.Now;

			//	Make sure it's our instance of openSharpGL that's active.
			OpenGL.MakeCurrent();

			//	If there is a draw handler, then call it.
			if(OpenGLDraw != null)
				OpenGLDraw(this, e);				
				
			//	Swap the buffers, i.e draw.
			OpenGL.SwapBuffers();

			System.DateTime timeAfter = System.DateTime.Now;

			//	If's there's a GDI draw handler, then call it.
			if(GDIDraw != null)
				GDIDraw(this, e);

			//	Draw the render time.
			if(drawRenderTime)
			{
				System.TimeSpan span = new System.TimeSpan(timeAfter.Ticks - timeBefore.Ticks);
				OpenGL.GDIGraphics.DrawString("Draw Time : " + span.Milliseconds + " milliseconds",
					new System.Drawing.Font(FontFamily.GenericSerif, 10), Brushes.White, new PointF(1, 1));
			}

			//	Blit our offscreen bitmap.
			e.Graphics.DrawImageUnscaled(OpenGL.OpenGLBitmap, 0, 0);
			
			//	If the client wants GDI drawing, blit it on now.
			if(gdiEnabled)
				e.Graphics.DrawImageUnscaled(OpenGL.GDIBitmap, 0, 0);
		}


		protected override void OnPaintBackground(PaintEventArgs e)
		{
			//	We override this, and don't call the base, i.e we don't paint
			//	the background.
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			//	Resize the DIB Surface.
            OpenGL.Create(Width, Height);

            //	Set the viewport.
            gl.Viewport(0, 0, Width, Height);

            //  If we have a project handler, call it.
            if(Width != -1 && Height != -1)
            {
                //  Otherwise we do our own projection.
                gl.MatrixMode(OpenGL.PROJECTION);
                gl.LoadIdentity();

                // Calculate The Aspect Ratio Of The Window
                gl.Perspective(45.0f, (float)Width / (float)Height, 0.1f, 100.0f);

                gl.MatrixMode(OpenGL.MODELVIEW);
                gl.LoadIdentity();	
               
            }

			Invalidate();
		}

        private void timerDrawing_Tick(object sender, EventArgs e)
        {
            //  The timer for drawing simply invalidates the control at a regular interval.
            Invalidate();
        }

		#region Events

		[Description("Called whenever OpenGL drawing can should occur."), Category("SharpGL")]
		public event PaintEventHandler OpenGLDraw;

        [Description("Called at the point in the render cycle when GDI drawing can occur."), Category("SharpGL")]
		public event PaintEventHandler GDIDraw;

		#endregion

		#region Member Data

		/// <summary>
		/// When set to true, the draw time will be displayed in the render.
		/// </summary>
		protected bool drawRenderTime = false;

		/// <summary>
		/// When set to true, GDI drawing can occur.
		/// </summary>
		protected bool gdiEnabled = false;

        /// <summary>
        /// The timer used for drawing the control.
        /// </summary>
        protected Timer timerDrawing;

        /// <summary>
        /// The OpenGL object for the control.
        /// </summary>
        protected OpenGL gl = new OpenGL();

		#endregion

		#region Properties

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public OpenGL OpenGL
		{
			get {return gl;}
		}
		[Description("Should the draw time be shown?"), Category("Drawing")]
		public bool DrawRenderTime
		{
			get {return drawRenderTime;}
			set {drawRenderTime = value;}
		}
        [Description("Is GDI drawing enabled (greatly slows performance)."), Category("Drawing")]
		public bool GDIEnabled
		{
			get {return gdiEnabled;}
			set {gdiEnabled = value;}
        }
        [Description("The rate at which the control should be re-drawn, in Hertz."), Category("Drawing")]
        public float FrameRate
        {
            get 
            { 
                //  If the timers not running, the frame rate is zero.
                if (timerDrawing.Enabled == false)
                    return 0;
                else
                    return 1000.0f / (float)timerDrawing.Interval;
            }
            set 
            {
                //  If the frame rate is zero, stop the timer.
                if (value == 0)
                {
                    //  Stop and reset the timer.
                    timerDrawing.Enabled = false;
                    timerDrawing.Interval = 100;
                }
                else
                {
                    //  Enable the timer and set the rate in Hertz.
                    timerDrawing.Enabled = true;
                    timerDrawing.Interval = (int)(1000.0f / value);
                }
            }
        }

		#endregion
	}
}
