//    StockDrawing.cs
//    Created by Dave Kerr, 12/08/2003
//    Copyright (c) Dave Kerr 2003
//    http://www.codechamber.com

using System;

namespace SharpGL.SceneGraph
{
	/// <summary>
	/// The StockObjects class contains a set of useful stock objects for you
	/// to draw. It is implemented via OpenGL display lists for speed.
	/// </summary>
	public class StockDrawing
	{
		public StockDrawing()
		{
		}

		/// <summary>
		/// This function creates the internal display lists.
		/// </summary>
		/// <param name="gl">OpenGL object.</param>
		public virtual void Create(OpenGL gl)
		{
			//	Create the circle. It circles around the Z axis, with a radius = 1.
			circle.Generate(gl);
			circle.New(gl, DisplayList.DisplayListMode.Compile);

			//	Start drawing a line.
			gl.Begin(OpenGL.LINE_LOOP);

			//	Make a circle of points.
			for(int i = 0; i < 12; i++)
			{
				double angle= 2 * Math.PI * i / 12;

				//	Draw the point.
				gl.Vertex(Math.Cos(angle), 0, Math.Sin(angle));
			}

			//	End the line drawing.
			gl.End();

			//	End the display list.
			circle.End(gl);

			//	Create the 3D Circle (it's not a sphere).
			circle3D.Generate(gl);
			circle3D.New(gl, DisplayList.DisplayListMode.Compile);

			//	Draw three circles.
			gl.PushMatrix();
			circle.Call(gl);
			gl.Rotate(90, 1, 0, 0);
			circle.Call(gl);
			gl.Rotate(90, 0, 0, 1);
			circle.Call(gl);
			gl.PopMatrix();

			//	End the circle 3D list.
			circle3D.End(gl);

			//	Create the arrow.
			arrow.Generate(gl);
			arrow.New(gl, DisplayList.DisplayListMode.Compile);

			//	Draw the 'line' of the arrow.
			gl.Begin(OpenGL.LINES);
			gl.Vertex(0, 0, 0);
			gl.Vertex(0, 0, 1);
			gl.End();

			//	Draw the arrowhead.
			gl.Begin(OpenGL.TRIANGLE_FAN);
			gl.Vertex(0, 0, 1);
			gl.Vertex(0.2f, 0.8f, 0.2f);
			gl.Vertex(0.2f, 0.8f, -0.2f);
			gl.Vertex(-0.2f, 0.8f, -0.2f);
			gl.Vertex(-0.2f, 0.8f, 0.2f);
			gl.End();

			//	End the arrow list.
			arrow.End(gl);

			//	Create the grid.
			grid.Generate(gl);
			grid.New(gl, DisplayList.DisplayListMode.Compile);
			
			gl.Begin(OpenGL.LINES);
			for(int i = -10; i <= 10; i++)
			{
				gl.Vertex(i, 0, -10);
				gl.Vertex(i, 0, 10);
				gl.Vertex(-10, 0, i);
				gl.Vertex(10, 0, i);
			}

			gl.End();
			grid.End(gl);

			//	Create the axies.
			axies.Generate(gl);
			axies.New(gl, DisplayList.DisplayListMode.Compile);
			gl.PushAttrib(OpenGL.ALL_ATTRIB_BITS);
			gl.Disable(OpenGL.LIGHTING);
			gl.DepthFunc(OpenGL.ALWAYS);
			gl.LineWidth(2.0f);

			gl.Begin(OpenGL.LINES);
			gl.Color(1, 0, 0, 1);
			gl.Vertex(0, 0, 0);
			gl.Vertex(3, 0, 0);
			gl.Color(0, 1, 0, 1);
			gl.Vertex(0, 0, 0);
			gl.Vertex(0, 3, 0);
			gl.Color(0, 0, 1, 1);
			gl.Vertex(0, 0, 0);
			gl.Vertex(0, 0, 3);
			gl.End();

			gl.PopAttrib();

			axies.End(gl);

			camera.Generate(gl);
			camera.New(gl, DisplayList.DisplayListMode.Compile);

			Polygon poly = new Polygon();
			poly.CreateCube();
			poly.Scale.Set(.2f ,0.2f, 0.2f);
			poly.Draw(gl);
			//poly.Dispose();

			camera.End(gl);
		}
		
		/// <summary>
		/// This is a circle, radius 1, that goes around the Z axis.
		/// </summary>
		protected DisplayList circle = new DisplayList();

		/// <summary>
		/// This is three circles, one ringing each axis.
		/// </summary>
		protected DisplayList circle3D = new DisplayList();

		/// <summary>
		/// This is an arrow, one unit long, from (0,0,0) to (0,1,0).
		/// </summary>
		protected DisplayList arrow = new DisplayList();

		/// <summary>
		/// This is a grid, 20 by 20 units.
		/// </summary>
		protected DisplayList grid = new DisplayList();

		/// <summary>
		/// This is a set of axis.
		/// </summary>
		protected DisplayList axies = new DisplayList();

		/// <summary>
		/// This is a camera object.
		/// </summary>
		protected DisplayList camera = new DisplayList();

		public DisplayList Circle
		{
			get {return circle;}
		}
		public DisplayList Circle3D
		{
			get {return circle3D;}
		}
		public DisplayList Arrow
		{
			get {return arrow;}
		}
		public DisplayList Grid
		{
			get {return grid;}
		}
		public DisplayList Axies
		{
			get {return axies;}
		}
		public DisplayList Camera
		{
			get {return camera;}
		}
	}
}
