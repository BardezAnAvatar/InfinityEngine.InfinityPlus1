using System;
using System.Windows.Forms;
using System.Collections;

using SharpGL.SceneGraph;

namespace SharpGL.SceneGraph.Interaction
{
	public class Click
	{
		public Click(OpenGL gl, MouseEventArgs args)
		{
			this.args = args;
			GDIx = OpenGLx = args.X;
			GDIy = OpenGLy = args.Y;
			gl.GDItoOpenGL(ref OpenGLx, ref OpenGLy);
		}

		public int GDIx = 0;
		public int GDIy = 0;
		public int OpenGLx = 0;
		public int OpenGLy = 0;
		public Vertex vertex = null;

		public MouseEventArgs args;
	}

	public class ClickCollection : CollectionBase
	{
		public ClickCollection(){}

		public virtual void Add(Click value)
		{
			List.Add(value);
		}

		public virtual void AddRange(IList list)
		{
			foreach(Click click in list)
				List.Add(click);
		}

		public virtual Click this[int index]
		{
			get {return (Click)List[index];}
			set {List[index] = value;}
		}

		public virtual void Remove(Click value)
		{
			List.Remove(value);
		}
	}

	public class BuildingGrid
	{
		public BuildingGrid()
		{
			
		}

		public virtual void Clamp(Vertex v)
		{
			float diff = v.X % size;
			if(diff > size/2) diff -=size;
			v.X -= diff;

			diff = v.Y % size;
			if(diff > size/2) diff -=size;
			v.Y -= diff;

			diff = v.Z % size;
			if(diff > size/2) diff -=size;
			v.Z -= diff;

		}

		protected float size = 0.2f;
	}

	/// <summary>
	/// A Builder is an object that is used to build a SceneObject
	/// specifically using the mouse.
	/// </summary>
	public abstract class Builder
	{
		public Builder()
		{
		}

		/// <summary>
		/// This function is called when the user clicks
		/// in the window.
		/// </summary>
		/// <param name="e"></param>
		/// <param name="gl"></param>
		/// <returns>An object if the object is finished building.</returns>
		public virtual SceneObject OnMouseDown(OpenGL gl, MouseEventArgs e)
		{
			clicks.Add(new Click(gl, e));

			return null;
		}

		public virtual SceneObject OnMouseUp(OpenGL gl, MouseEventArgs e)
		{
			return null;
		}

		/// <summary>
		/// Update the mouse movement.
		/// </summary>
		/// <param name="gl"></param>
		/// <param name="e"></param>
		/// <returns>True if a redraw is needed.</returns>
		public virtual bool OnMouseMove(OpenGL gl, MouseEventArgs e)
		{
			return false;
		}

		public virtual void Draw(OpenGL gl)
		{
			if(buildingObject != null)
				buildingObject.Draw(gl);
		}

		/// <summary>
		/// This is a utility function that will turn a click
		/// on the control to a point on a specified plane.
		/// </summary>
		/// <param name="gl">OpenGL.</param>
		/// <param name="plane">The plane to project onto.</param>
		/// <param name="x">The X part of the point.</param>
		/// <param name="y">The Y part of the point.</param>
		/// <returns>A three-tuple representing the point on the floor.</returns>
		protected Vertex PointToVertexOnPlane(OpenGL gl, Plane plane, int x, int y)
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

			double dist = 0;
			switch(plane)
			{
				case Plane.xy:
					dist = -1.0 * point1[2] / direction[2];
					break;
				case Plane.xz:
					dist = -1.0 * point1[1] / direction[1];
					break;
				case Plane.yz:
					dist = -1.0 * point1[0] / direction[0];
					break;
			}

			double px = point1[0] + dist*direction[0];
			double py = point1[1] + dist*direction[1];
			double pz = point1[2] + dist*direction[2];

			return new Vertex((float)px, (float)py, (float)pz);
		}

		public enum Plane
		{
			xy,
			xz,
			yz,
		}

		#region Member Data

		/// <summary>
		/// This is the object that the builder is currently
		/// building.
		/// </summary>
		protected SceneObject buildingObject = null;

		/// <summary>
		/// This collection maintains data about where the user
		/// has clicked.
		/// </summary>
		protected ClickCollection clicks = new ClickCollection();

		#endregion
	}

	public class SphereBuilder : Builder
	{
		public SphereBuilder()
		{
		}

		public override SceneObject OnMouseDown(OpenGL gl, MouseEventArgs e)
		{
			base.OnMouseDown(gl, e);
			
			//	When the user presses the mouse down, we create the centre of the
			//	sphere.
			
			buildingObject = new Quadrics.Sphere();

			Vertex v = PointToVertexOnPlane(gl, Plane.xz, 
				clicks[0].OpenGLx, clicks[0].OpenGLy);

			BuildingGrid grid = new BuildingGrid();
			grid.Clamp(v);
				
			if(v != null)
			{
				clicks[0].vertex = v;
				buildingObject.Translate = v;
			}
			
			return null;
		}

		public override bool OnMouseMove(OpenGL gl, MouseEventArgs e)
		{
			//	We only update the radius if the left button is
			//	down, and the buildingObject isn't null.
			
			Click click = new Click(gl, e);

			if(e.Button == MouseButtons.Left)
			{

				//	If we have clicked once, then we are setting the radius.
				Vertex v = PointToVertexOnPlane(gl, Plane.xz, click.OpenGLx,
					click.OpenGLy);

				if(v != null)
				{
					Vertex v2 = clicks[0].vertex;
					BuildingGrid grid = new BuildingGrid();
					grid.Clamp(v);
					grid.Clamp(v2);

					

					Vertex between = v - v2;
					((Quadrics.Sphere)buildingObject).Radius = between.Magnitude();
				}

				buildingObject.Modified = true;

				return true;
			}

			return base.OnMouseMove (gl, e);
		}

		public override SceneObject OnMouseUp(OpenGL gl, MouseEventArgs e)
		{
			base.OnMouseUp(gl, e);

			OnMouseMove(gl, e);

			return buildingObject;
		}
	}

	public class PlaneBuilder : Builder
	{
		public PlaneBuilder()
		{
			Polygon plane = new Polygon();
			plane.Vertices.Add(new Vertex());
			plane.Vertices.Add(new Vertex());
			plane.Vertices.Add(new Vertex());
			plane.Vertices.Add(new Vertex());
						
			plane.UVs.Add(new UV(0, 0));
			plane.UVs.Add(new UV(0, 1));
			plane.UVs.Add(new UV(1, 1));
			plane.UVs.Add(new UV(1, 0));

			Face face = new Face();
			face.Indices.Add(new Index(2, 0));
			face.Indices.Add(new Index(3, 1));
			face.Indices.Add(new Index(1, 2));
			face.Indices.Add(new Index(0, 3));
			plane.Faces.Add(face);
			
			buildingObject = plane;
		}

		public override SceneObject OnMouseDown(OpenGL gl, MouseEventArgs e)
		{
			base.OnMouseDown(gl, e);

			Polygon plane = (Polygon)buildingObject;
			
			//	When the user presses the mouse down, we create the corner
			//	of the plane.

			Vertex v = PointToVertexOnPlane(gl, Plane.xz, 
				clicks[0].OpenGLx, clicks[0].OpenGLy);

			BuildingGrid grid = new BuildingGrid();
			grid.Clamp(v);
				
			if(v != null)
			{
				plane.Vertices[0] = v;
			}
			
			return null;
		}

		public override bool OnMouseMove(OpenGL gl, MouseEventArgs e)
		{
			//	We only update the radius if the left button is
			//	down, and the buildingObject isn't null.
			Polygon plane = (Polygon)buildingObject;
			Click click = new Click(gl, e);

			if(e.Button == MouseButtons.Left)
			{
				//	If we have clicked once, then we are setting the other corner.
				Vertex v = PointToVertexOnPlane(gl, Plane.xz, click.OpenGLx,
					click.OpenGLy);

				if(v != null)
				{
					BuildingGrid grid = new BuildingGrid();
					grid.Clamp(v);

					Vertex v0 = plane.Vertices[0];
					Vertex v3 = plane.Vertices[3] = v;

					plane.Vertices[1].Set(v3.X, 0, v0.Z);
					plane.Vertices[2].Set(v0.X, 0, v3.Z);
					
				}

				buildingObject.Modified = true;

				return true;
			}

			return base.OnMouseMove (gl, e);
		}

		public override SceneObject OnMouseUp(OpenGL gl, MouseEventArgs e)
		{
			base.OnMouseUp(gl, e);

			OnMouseMove(gl, e);

			return buildingObject;
		}
	}

	public class PolygonBuilder : Builder
	{
		public override SceneObject OnMouseDown(OpenGL gl, MouseEventArgs e)
		{
			base.OnMouseDown(gl, e);
			
			//	Every time the left mouse button is cliced, a new point
			//	is added. The first time the right button is clicked
			//	completes the polygon, the second time sweeps it.
			if(e.Button == MouseButtons.Left)
			{
				//	Add this click as a point.
				Click click = clicks[clicks.Count-1];
				click.vertex = PointToVertexOnPlane(gl, Plane.xz, click.OpenGLx,
					click.OpenGLy);
			}
			
			return null;
		}

		public override bool OnMouseMove(OpenGL gl, MouseEventArgs e)
		{
			Click click = new Click(gl, e);

			return base.OnMouseMove (gl, e);
		}

		public override void Draw(OpenGL gl)
		{
			//	Draw each point and the lines between them.

			base.Draw (gl);
		}

	}
}
