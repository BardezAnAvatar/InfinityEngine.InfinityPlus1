using System;
using System.Collections;

using SharpGL.SceneGraph;

namespace SharpGL.SceneGraph.Interaction
{
	/// <summary>
	/// A Mouser is an object that allows you to build an object completely using the mouse.
	/// For example, when you click on the screen to make a Cuber- you click on three points
	/// to set the width, length and height.
	/// 
	/// A Mouser is a sceneobject, but it is only temporary.
	/// </summary>
	public abstract class Mouser : SceneObject
	{
		public Mouser()
		{
		}

		/// <summary>
		/// This is the core function of a Mouser. OnClick is called whenever the 
		/// user clicks, it allows you to repeatedly build up the detail of the object.
		/// When you return the finished SceneObject you are building, the Mouser is removed
		/// from the Scenes SceneObjects and the returned SceneObject is added. If you 
		/// return null it means the object is still being built- and needs more clicks.
		/// </summary>
		/// <param name="position">The mouse position.</param>
		/// <returns>Null if the object is stil being built, or otherwise the
		/// object that has been built.</returns>
		public virtual SceneObject OnClick(OpenGL gl, int x, int y)
		{
			//	Save the click.
			Click click = new Click(x, y);
			clicks.Add(click);

			return null;
		}

		public virtual void OnMouseMove(OpenGL gl, int x, int y)
		{
			modified = true;
		}

		/// <summary>
		/// This is a utility function that will turn a click
		/// on the control to a point on the floor.
		/// </summary>
		/// <param name="gl">OpenGL.</param>
		/// <param name="x">The X part of the point.</param>
		/// <param name="y">The Y part of the point.</param>
		/// <returns>A three-tuple representing the point on the floor.</returns>
		protected float[] PointToVertexOnFloor(OpenGL gl, int x, int y)
		{
			//	The first thing we do is turn the click into a vector that 
			//	is from the camera to the cursor.
			double[] point1 = gl.UnProject((double)x, (double)y, 0);
			double[] point2 = gl.UnProject((double)x, (double)y, 1);

			double[] direction = {point2[0] - point1[0], 
									 point2[1] - point1[1],
									 point2[2] - point1[2]};

			//	Now we find out the position when the line intersects the 
			//	ground level.
			if(direction[1] == 0)
				return null;

			double dist = -1.0 * point1[1] / direction[1];

			double px = point1[0] + dist*direction[0];
			double py = point1[1] + dist*direction[1];
			double pz = point1[2] + dist*direction[2];

			return new float[] {(float)px, (float)py, (float)pz};
		}

		/// <summary>
		/// You must override this function to do the actual building of
		/// the object. The lastClick parameter tells you which of the
		/// clicks has changed.
		/// </summary>
		/// <param name="lastClick">Which click has changed.</param>
		protected virtual void UpdateObject(int lastClick)
		{
		}


		#region Enumerations

		enum Axis
		{
			x,
			y,
			z,
		}

		internal class Click
		{
			public Click(int x, int y) {this.x = x; this.y = y;}

			public int x;
			public int y;
		}

		#endregion

		#region Member Data

		protected ArrayList clicks = new ArrayList();

		#endregion

		#region Properties
		#endregion
	}

	/// <summary>
	/// A Cuber is a Mouser that is used to build a cube. The first click defines the
	/// position of the bottom left corner, the second click defines the area, the third 
	/// click defines the height.
	/// </summary>
	public class Cuber : Mouser
	{
		public override SceneObject OnClick(OpenGL gl, int x, int y)
		{
			base.OnClick(gl, x, y);

			modified = true;

			float[] point = PointToVertexOnFloor(gl, x, y);

			//	If this is the first click, we need to create the starting point.
			if(clicks.Count == 1)
			{
				cube = new Polygon();
				cube.CreateCube();
				cube.Scale.Set(0, 0, 0);
				cube.Translate.Set(point[0], point[1], point[2]);
			}
			else if(clicks.Count == 2)
			{
				//	Set the x and y scale.
				float xscale = point[0] - cube.Translate.X;
				float zscale = point[2] - cube.Translate.Z;

				cube.Scale.X = xscale;
				cube.Scale.Z = zscale;
			}
			else if(clicks.Count == 3)
			{
				//	Set the z scale.
				float yscale = y - ((Click)clicks[0]).y;
				
				cube.Scale.Y = yscale / 100;

				return cube;
			}
			return null;
		}

		public override void OnMouseMove(OpenGL gl, int x, int y)
		{
			if(clicks.Count == 0)
				return;
			
			float[] point = PointToVertexOnFloor(gl, x, y);

			if(clicks.Count == 1)
			{
				//	Set the x and y scale.
				float xscale = point[0] - cube.Translate.X;
				float zscale = point[2] - cube.Translate.Z;

				cube.Scale.X = xscale;
				cube.Scale.Z = zscale;
			}
			else if(clicks.Count == 2)
			{
				//	Set the z scale.
				float yscale = y - ((Click)clicks[0]).y;
				
				cube.Scale.Y = yscale / 100;
			}

			cube.Modified = true;
			cube.Draw(gl);
			modified = true;
		}

		public override void Draw(OpenGL gl)
		{
			if(DoPreDraw(gl))
			{
				if(cube != null)
					cube.Draw(gl);

				DoPostDraw(gl);
			}
		}

		protected Polygon cube = null;
	}

	/// <summary>
	/// A Polygon2Der simply creates a 2D Polygon.
	/// </summary>
	public class Polygon2Der : Mouser
	{
		public Polygon2Der()
		{
			//	Create a new Polygon.
			polygon2D = new Polygon();

			//	Set it to outline mode.
			polygon2D.Attributes.PolygonDrawMode = SharpGL.SceneGraph.Attributes.Polygon.PolygonMode.Lines;
		}

		public override SceneObject OnClick(OpenGL gl, int x, int y)
		{
			base.OnClick(gl, x, y);

			modified = true;

			//	Get the point on the plane.
			gl.GDItoOpenGL(ref x, ref y);
			float[] point = PointToVertexOnFloor(gl, x, y);

			//	Add this to the vertices.
			polygon2D.Vertices.Add(new Vertex(point[0], point[1], point[2]));

			//	Make sure there is a face.
			if(polygon2D.Faces.Count == 0)
			{
				//	Add a face.
				polygon2D.Faces.Add(new Face());
			}

			//	Add this new vertex to the face.
			int nVertex = polygon2D.Vertices.Count - 1;
			polygon2D.Faces[0].Indices.Add( new Index(nVertex));

			polygon2D.ForceModifiedUpdate(gl);

			return null;
		}

		public override void OnMouseMove(OpenGL gl, int x, int y)
		{
			if(clicks.Count == 0)
				return;
		}

		public override void Draw(OpenGL gl)
		{
			if(DoPreDraw(gl))
			{
				if(polygon2D != null)
					polygon2D.Draw(gl);

				DoPostDraw(gl);
			}
		}

		protected Polygon polygon2D = null;
	}


	/// <summary>
	/// A Spherer is a Mouser that is used to build a Quadrics Sphere.
	/// Only two clicks are needed, the centre, and the radius.
	/// </summary>
	public class Spherer : Mouser
	{
		public override SceneObject OnClick(OpenGL gl, int x, int y)
		{
			base.OnClick(gl, x, y);

			modified = true;

			float[] point = PointToVertexOnFloor(gl, x, y);

			//	If this is the first click, we need to create the starting point.
			if(clicks.Count == 1)
			{
				cube = new Polygon();
				cube.CreateCube();
				cube.Scale.Set(0, 0, 0);
				cube.Translate.Set(point[0], point[1], point[2]);
			}
			else if(clicks.Count == 2)
			{
				//	Set the x and y scale.
				float xscale = point[0] - cube.Translate.X;
				float zscale = point[2] - cube.Translate.Z;

				cube.Scale.X = xscale;
				cube.Scale.Z = zscale;
			}
			else if(clicks.Count == 3)
			{
				//	Set the z scale.
				float yscale = y - ((Click)clicks[0]).y;
				
				cube.Scale.Y = yscale / 100;

				return cube;
			}
			return null;
		}

		public override void OnMouseMove(OpenGL gl, int x, int y)
		{
			if(clicks.Count == 0)
				return;
			
			float[] point = PointToVertexOnFloor(gl, x, y);

			if(clicks.Count == 1)
			{
				//	Set the x and y scale.
				float xscale = point[0] - cube.Translate.X;
				float zscale = point[2] - cube.Translate.Z;

				cube.Scale.X = xscale;
				cube.Scale.Z = zscale;
			}
			else if(clicks.Count == 2)
			{
				//	Set the z scale.
				float yscale = y - ((Click)clicks[0]).y;
				
				cube.Scale.Y = yscale / 100;
			}

			cube.Modified = true;
			cube.Draw(gl);
			modified = true;
		}

		public override void Draw(OpenGL gl)
		{
			if(DoPreDraw(gl))
			{
				if(cube != null)
					cube.Draw(gl);

				DoPostDraw(gl);
			}
		}

		protected Polygon cube = null;
	}
}
