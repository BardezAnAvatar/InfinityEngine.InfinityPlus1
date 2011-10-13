//    Evaluators.cs
//    Created by Dave Kerr, 12/08/2003
//    Copyright (c) Dave Kerr 2003
//    http://www.codechamber.com


using System;
using System.Collections;
using System.ComponentModel;

using SharpGL.SceneGraph.Collections;

namespace SharpGL.SceneGraph.Evaluators
{

	/// <summary>
	/// This is the base class of all evaluators, 1D, 2D etc. It is also the base class
	/// for the NURBS, as they share alot of common code, such as the VertexGrid.
	/// </summary>
	[Serializable()]
	public abstract class Evaluator : SceneObject, IInteractable
	{
		public Evaluator()
		{
			name = "Evaluator";
		}

		IInteractable IInteractable.GetObjectFromSelectNames(int[] names)
		{
			//	If it's a single name, then it's just the evaluator.
			if(names.Length == 1)
				return this;

			//	If it has another name, then it's a control point.
			return (IInteractable)controlPoints.Vertices[names[1]];
		}

		#region Member Data

		protected VertexGrid controlPoints = new VertexGrid();
		protected bool drawPoints = true;
		protected bool drawLines = true;
		protected Attributes.Line lineSettings = new Attributes.Line();

		#endregion

		#region Properties

		public VertexCollection ControlPoints
		{
			get {return controlPoints.Vertices;}
			set {controlPoints.Vertices = value;  modified = true;}
		}
		public bool DrawControlPoints
		{
			get {return drawPoints;}
			set {drawPoints = value;  modified = true;}
		}
		public bool DrawControlGrid
		{
			get {return drawLines;}
			set {drawLines = value;  modified = true;}
		}
		public Attributes.Line LineSettings
		{
			get {return lineSettings;}
			set {lineSettings = value; modified = true;}
		}

			#endregion
	}


	/// <summary>
	/// This is a 1D evaluator, i.e a bezier curve.
	/// </summary>
	[Serializable()]
	public class Evaluator1D : Evaluator
	{
		public Evaluator1D()
		{
			//	Create a single line of points.
			controlPoints.CreateGrid(4, 1);

			Rotate.X = 180;

			name = "1D Evaluator (Bezier Curve)";
		}

		public Evaluator1D(int points)
		{
			//	Create a single line of points.
			controlPoints.CreateGrid(points, 1);
			name = "1D Evaluator (Bezier Curve)";
			Rotate.X = 180;
		}

		public override void Draw(OpenGL gl)
		{
			if(DoPreDraw(gl))
			{

				//	Set our line settings.
				lineSettings.Set(gl);
				
				//	Create the evaluator.
				gl.Map1(OpenGL.MAP1_VERTEX_3,//	Use and produce 3D points.
					0,								//	Low order value of 'u'.
					1,								//	High order value of 'u'.
					3,								//	Size (bytes) of a control point.
					controlPoints.Width,			//	Order (i.e degree plus one).
					controlPoints.ToFloatArray());	//	The control points.
				
				//	Enable the type of evaluator we wish to use.
				gl.Enable(OpenGL.MAP1_VERTEX_3);

				//	Beging drawing a line strip.
				gl.Begin(OpenGL.LINE_STRIP);

				//	Now draw it.
				for(int i = 0; i <= segments; i++)
					gl.EvalCoord1((float) i / segments);

				gl.End();

				//	Draw the control points.
				controlPoints.Draw(gl, drawPoints, drawLines);

				//	Restore the line attributes.
				lineSettings.Restore(gl);

				DoPostDraw(gl);
			}
		}

		protected int segments = 30;
		
		#region Properties

		public int Segments
		{
			get {return segments;}
			set {segments = value;  modified = true;}
		}

		#endregion
	}

	/// <summary>
	/// This is a 2D evaluator, i.e a bezier patch.
	/// </summary>
	[Serializable()]
	public class Evaluator2D : Evaluator
	{
		public Evaluator2D()
		{
			controlPoints.CreateGrid(4, 4);
			name = "2D Evaluator (Bezier Patch)";
			Rotate.X = 180;
		}

		public Evaluator2D(int u, int v)
		{
			controlPoints.CreateGrid(u, v);
			name = "2D Evaluator (Bezier Patch)";
			Rotate.X = 180;
		}

		public override void Draw(OpenGL gl)
		{
			if(DoPreDraw(gl))
			{
				//	Set our line settings.
				lineSettings.Set(gl);

				//	Create the evaluator.
				gl.Map2(OpenGL.MAP2_VERTEX_3,//	Use and produce 3D points.
					0,								//	Low order value of 'u'.
					1,								//	High order value of 'u'.
					3,								//	Size (bytes) of a control point.
					controlPoints.Width,			//	Order (i.e degree plus one).
					0,								//	Low order value of 'v'.
					1,								//	High order value of 'v'
					controlPoints.Width * 3,		//	Size in bytes of a 'row' of points.
					controlPoints.Height,			//	Order (i.e degree plus one).
					controlPoints.ToFloatArray());	//	The control points.
				
				gl.Enable(OpenGL.MAP2_VERTEX_3);
				gl.Enable(OpenGL.AUTO_NORMAL);
				gl.MapGrid2(20, 0, 1, 20, 0, 1);
			
				//	Now draw it.
				gl.EvalMesh2(OpenGL.FILL, 0, 20, 0, 20);

				//	Draw the control points.
				controlPoints.Draw(gl, drawPoints, drawLines);

				//	Restore the line attributes.
				lineSettings.Restore(gl);

				DoPostDraw(gl);
			}
		}

		protected int segments = 30;
	
		#region Properties

		public int Segments
		{
			get {return segments;}
			set {segments = value;  modified = true;}
		}

		#endregion
	}
}