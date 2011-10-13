//    NURBS.cs
//    Created by Dave Kerr, 08/09/2003
//    Copyright (c) Dave Kerr 2003
//    http://www.codechamber.com

//	NURBS are the latest addition to the library, a direct wrapper from the OpenGL code,
//	but (at least on my poor system) are awfully slow. Doubtless the code can be
//	optimised, but unfortunatly for at least this revision of SharpGL, they are almost
//	unusable. Also, I haven't gotten round to wrapping code for NURBS properties.

using System;
using System.ComponentModel;

using SharpGL.SceneGraph.Evaluators;

namespace SharpGL.SceneGraph.Evaluators
{
	/// <summary>
	/// The NURBS class is the base for NURBS objects, such as curves and surfaces.
	/// </summary>
	[Serializable()]
	public abstract class NURBSBase : Evaluator
	{
		public NURBSBase()
		{
			name = "NURBS Object";

			//	Create the underlying nurbs object using our OpenGL reference.
			if(sceneOpenGL != null)
				Create(sceneOpenGL);
		}

		~NURBSBase()
		{
			//	Destroy the underlying nurbs object.
			if(sceneOpenGL != null)
				Destroy(sceneOpenGL);
		}

		/// <summary>
		/// This function creates the nurbs object.
		/// </summary>
		/// <param name="gl">The instance of OpenGL to create in.</param>
		public virtual void Create(OpenGL gl)
		{
			//	Create the underlying NURBS renderer.
			nurbsRenderer = gl.NewNurbsRenderer();
		}

		/// <summary>
		/// This function deletes the underlying Nurbs object.
		/// </summary>
		/// <param name="gl">The instance of OpenGL.</param>
		public virtual void Destroy(OpenGL gl)
		{
			//	Delete the renderer if it exists.
			if(nurbsRenderer != IntPtr.Zero)
			{
				gl.DeleteNurbsRenderer(nurbsRenderer);
				nurbsRenderer = IntPtr.Zero;
			}
		}

		public override void Draw(OpenGL gl)
		{
			//	Set the properties.
			gl.NurbsProperty(nurbsRenderer, (int)OpenGL.GLU_DISPLAY_MODE, (float)displayMode);
		}

		public enum NurbsDisplayMode : uint
		{
			OutlinePatch = OpenGL.GLU_OUTLINE_PATCH,
			OutlinePolygon = OpenGL.GLU_OUTLINE_POLYGON,
			Fill = OpenGL.GLU_FILL,
		}

		/// <summary>
		/// This is the pointer to the underlying NURBS object.
		/// </summary>
		protected IntPtr nurbsRenderer = IntPtr.Zero;

		/// <summary>
		/// This variable should be set to the Scenes OpenGL as soon as possible, or
		/// quadrics cannot be created.
		/// </summary>
		protected static OpenGL sceneOpenGL = null;

		protected NurbsDisplayMode displayMode = NurbsDisplayMode.OutlinePolygon;

		[Description("The OpenGL object from the main scene."), Category("NURBS")]
		public static OpenGL SceneOpenGL
		{
			set {sceneOpenGL = value;}
		}
		[Description("The way that the NURBS object is rendered."), Category("NURBS")]
		public NurbsDisplayMode DisplayMode
		{
			get {return displayMode;}
			set {displayMode = value;}
		}
	}

	/// <summary>
	/// A NURBS Curve is a one dimensional non uniform B-Spline.
	/// </summary>
	[Serializable()]
	public class NURBSCurve : NURBSBase
	{
		public NURBSCurve()
		{
			name = "NURBS Curve";
			controlPoints.CreateGrid(4, 1);
		}

		public override void Draw(OpenGL gl)
		{
			//	Do pre drawing stuff.
			if(DoPreDraw(gl))
			{

				base.Draw(gl);

				//	Set our line settings.
				lineSettings.Set(gl);

				//	Begin drawing a NURBS curve.
				gl.BeginCurve(nurbsRenderer);

				//	Draw the curve.
				gl.NurbsCurve(nurbsRenderer,		//	The internal nurbs object.
					knots.Length,					//	Number of knots.
					knots,							//	The knots themselves.
					3,								//	The size of a vertex.
					controlPoints.ToFloatArray(),	//	The control points.
					controlPoints.Width,			//	The order, i.e degree + 1.
					OpenGL.MAP1_VERTEX_3);			//	Type of data to generate.

				//	End the curve.
				gl.EndCurve(nurbsRenderer);

				//	Draw the control points.
				controlPoints.Draw(gl, drawPoints, drawLines);

				//	Restore the line attributes.
				lineSettings.Restore(gl);

				//	Do post drawing stuff.
				DoPostDraw(gl);
			}
		}

		protected float[] knots = new float [] {0, 0, 0, 0, 1, 1, 1, 1};
	}

	/// <summary>
	/// A NURBS Surface is a two dimensional non uniform B-Spline.
	/// </summary>
	[Serializable()]
	public class NURBSSurface : NURBSBase
	{
		public NURBSSurface()
		{
			name = "NURBS Surface";
			controlPoints.CreateGrid(4, 4);
		}

		public override void Draw(OpenGL gl)
		{
			//	Do pre drawing stuff.
			if(DoPreDraw(gl))
			{

				base.Draw(gl);

				//	Set our line settings.
				lineSettings.Set(gl);

				//	Begin drawing a NURBS surface.
				gl.BeginSurface(nurbsRenderer);

				//	Draw the surface.
				gl.NurbsSurface(nurbsRenderer,		//	The internal nurbs object.
					sKnots.Length,					//	Number of s-knots.
					sKnots,							//	The s-knots themselves.
					tKnots.Length,					//	The number of t-knots.
					tKnots,							//	The t-knots themselves.
					controlPoints.Width * 3,		//	The size of a row of control points.
					3,								//	The size of a control points.
					controlPoints.ToFloatArray(),	//	The control points.
					controlPoints.Width,			//	The order, i.e degree + 1.
					controlPoints.Height,			//	The order, i.e degree + 1.
					OpenGL.MAP2_VERTEX_3);			//	Type of data to generate.

				//	End the surface.
				gl.EndSurface(nurbsRenderer);

				//	Draw the control points.
				controlPoints.Draw(gl, drawPoints, drawLines);

				//	Restore the line attributes.
				lineSettings.Restore(gl);

				//	Do post drawing stuff.
				DoPostDraw(gl);
			}
		}

		protected float[] sKnots = new float [] {0, 0, 0, 0, 1, 1, 1, 1};

		protected float[] tKnots = new float [] {0, 0, 0, 0, 1, 1, 1, 1};
	}
}
