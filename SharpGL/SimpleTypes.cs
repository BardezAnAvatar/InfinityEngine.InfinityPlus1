//    SimpleTypes.cs
//    Created by Dave Kerr, 12/08/2003
//    Copyright (c) Dave Kerr 2003
//    http://www.codechamber.com

//	These are objects like normals and vertices.

using System;
using System.ComponentModel;
using System.Collections;

using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Collections;
using SharpGL.SceneGraph.NETDesignSurface.Converters;

namespace SharpGL
{
	/// <summary>
	/// The vertex class represents a 3D point in space.
	/// </summary>
	[TypeConverter(typeof(VertexConverter))]
	[Serializable()]
	public class Vertex : IInteractable
	{
		public Vertex(){}

		public Vertex(float x, float y, float z)
		{
			this.x = x;	 this.y = y; this.z = z;
		}

		public void Set(float x, float y, float z)
		{
			this.x = x; this.y = y; this.z = z;
		}

		public void Push(float x, float y, float z) {this.x += x; this.y += y; this.z += z;}

		public override string ToString()
		{
			return "(" + x + ", " + y + ", " + z + ")";
		}

		public static Vertex operator + (Vertex lhs, Vertex rhs)
		{
			return new Vertex(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
		}

		public static Vertex operator + (Vertex lhs, Normal rhs)
		{
			return new Vertex(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
		}

		public static Vertex operator - (Vertex lhs, Vertex rhs)
		{
			return new Vertex(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
		}

		public static Vertex operator * (Vertex lhs, Vertex rhs)
		{
			return new Vertex(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
		}

		public static Vertex operator * (Vertex lhs, float rhs)
		{
			return new Vertex(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
		}

		public static Vertex operator * (Vertex lhs, Matrix rhs)
		{
			float x = lhs.x * rhs.data[0][0] + lhs.y * rhs.data[1][0] + lhs.z * rhs.data[2][0];
			float y = lhs.x * rhs.data[0][1] + lhs.y * rhs.data[1][1] + lhs.z * rhs.data[2][1];
			float z = lhs.x * rhs.data[0][2] + lhs.y * rhs.data[1][2] + lhs.z * rhs.data[2][2];

			return new Vertex(x, y, z);
		}

		public static Vertex operator / (Vertex lhs, float rhs)
		{
			return new Vertex(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
		}

		/// <summary>
		/// This finds the Scalar Product (Dot Product) of two vectors.
		/// </summary>
		/// <param name="rhs">The right hand side of the equation.</param>
		/// <returns>A Scalar Representing the Dot-Product.</returns>
		public float ScalarProduct(Vertex rhs)
		{
			return x*rhs.x + y*rhs.y + z*rhs.z;
		}

		/// <summary>
		/// Find the Vector product (cross product) of two vectors.
		/// </summary>
		/// <param name="rhs">The right hand side of the equation.</param>
		/// <returns>The Cross Product.</returns>
		public Vertex VectorProduct(Vertex rhs)
		{
			return new Vertex( (y *  rhs.z) - (z * rhs.y) , (z *  rhs.x) - (x * rhs.z),
				(x *  rhs.y) - (y * rhs.x));
		}

		/// <summary>
		/// If you use this as a Vector, then call this function to get the vector
		/// magnitude.
		/// </summary>
		/// <returns></returns>
		public double Magnitude()
		{
			return System.Math.Sqrt(x*x + y*y + z*z);
		}

		/// <summary>
		/// Make this vector unit length.
		/// </summary>
		public void UnitLength()
		{
			float f = x*x + y*y + z*z;
			float frt = (float)Math.Sqrt(f);
			x /= frt;
			y /= frt;
			z /= frt;
		}

		public static implicit operator float[](Vertex rhs)
		{
			return new float[] {rhs.x, rhs.y, rhs.z};
		}

		/// <summary>
		/// This function is called on interactable vertices, to draw them.
		/// </summary>
		void IInteractable.DrawPick(OpenGL gl)
		{
			//	Here we just draw a point.
			gl.Begin(OpenGL.POINTS);
			gl.Vertex(this);
			gl.End();
		}

		/// <summary>
		/// Convert select names to object.
		/// </summary>
		IInteractable IInteractable.GetObjectFromSelectNames(int[] names)
		{
			return this;
		}

		/// <summary>
		/// Interact.
		/// </summary>
		void IInteractable.DoMouseInteract(MouseOperation operation, float x, float y, float z)
		{
			if(operation == MouseOperation.Translate)
				Set(this.x += x, this.y += y, this.z += z);
		}

		protected float x = 0;
		protected float y = 0;
		protected float z = 0;

		public float X
		{
			get {return x;}
			set {x = value;}
		}
		public float Y
		{
			get {return y;}
			set {y = value;}
		}
		public float Z
		{
			get {return z;}
			set {z = value;}
		}
	}

	/// <summary>
	/// This class represent's a grid of points, just like you'd get on a NURBS
	/// surface, or a patch.
	/// </summary>
	[Serializable()]
	public class VertexGrid
	{
		public virtual void CreateGrid(int x, int y)
		{
			//	Clear the current array.
			vertices.Clear();
			
			//	Add a new set of control points.
			for(int yvals = 0; yvals < y; yvals++)
			{
				for(int xvals = 0; xvals < x; xvals++)
                    vertices.Add( new Vertex(xvals, 1.0f, yvals));
			}

			this.x = x;
			this.y = y;
		}

		/// <summary>
		/// Use this to draw the vertex grid.
		/// </summary>
		/// <param name="gl">OpenGL object.</param>
		/// <param name="points">Draw each individual vertex (with selection names).</param>
		/// <param name="lines">Draw the lines connecting the points.</param>
		public virtual void Draw(OpenGL gl, bool points, bool lines)
		{
			//	Save the attributes.
			gl.PushAttrib(OpenGL.ALL_ATTRIB_BITS);
			gl.Disable(OpenGL.LIGHTING);
			gl.Color(1, 0, 0, 1);

			if(points)
			{
                int name = 0;

				gl.PointSize(5);

				//	Add a new name (the vertex name).
				gl.PushName(0);

				foreach(Vertex v in vertices)
				{
					//	Set the name, draw the vertex.
					gl.LoadName((uint)name++);
					((IInteractable)v).DrawPick(gl);
				}

				//	Pop the name.
				gl.PopName();
			}

			if(lines)
			{
				//	Draw lines along each row, then along each column.
				gl.DepthFunc(OpenGL.ALWAYS);

				gl.LineWidth(1);
				gl.Disable(OpenGL.LINE_SMOOTH);

				for(int col=0; col < y; col++)
				{
					for(int row=0; row < x; row++)
					{
						//	Create vertex indicies.
						int nTopLeft = (col * x) + row;
						int nBottomLeft = ((col + 1) * x) + row;

						gl.Begin(OpenGL.LINES);
						if(row < (x-1))
						{
							gl.Vertex(vertices[nTopLeft]);
							gl.Vertex(vertices[nTopLeft + 1]);
						}
						if(col < (y-1))
						{
							gl.Vertex(vertices[nTopLeft]);
							gl.Vertex(vertices[nBottomLeft]);
						}
						gl.End();
					}
				}
				gl.DepthFunc(OpenGL.LESS);
			}

			gl.PopAttrib();
		}

		/// <summary>
		/// This function returns all of the control points as a float array, which 
		/// is in the format [0] = vertex 1 X, [1] = vertex 1 Y, [2] = vertex 1 Z, 
		/// [3] = vertex 2 X etc etc... This array is suitable for OpenGL functions
		/// for evaluators and NURBS.
		/// </summary>
		/// <returns>An array of floats.</returns>
		public float[] ToFloatArray()
		{
			float[] floats = new float[Vertices.Count * 3];
			int index = 0;
			foreach(Vertex pt in Vertices)
			{
				floats[index++] = pt.X;
				floats[index++] = pt.Y;
				floats[index++] = pt.Z;
			}

			return floats;
		}
		
		protected VertexCollection vertices = new VertexCollection();
		protected int x = 0;
		protected int y = 0;

		public VertexCollection Vertices
		{
			get {return vertices;}
			set {vertices = value;}
		}
		public int Width
		{
			get {return x;}
			set {x = value;}
		}
		public int Height
		{
			get {return x;}
			set {y = value;}
		}
	}

	/// <summary>
	/// The normal object is almost exactly the same as a vertex, except that it's drawing
	/// code is slightly different, as is it's interaction code.
	/// </summary>
	[Serializable()]
	public class Normal : Vertex, IInteractable
	{
		public Normal(Vertex v) {x=v.X;y=v.Y;z=v.Z;}
		/// <summary>
		/// This function simply draws the normal as an arrow.
		/// </summary>
		void IInteractable.DrawPick(OpenGL gl)
		{
			//	We set some point styles here.
			SharpGL.SceneGraph.Attributes.Point point = new SharpGL.SceneGraph.Attributes.Point();
			point.Size = 5;
			point.Set(gl);
				
			//	Here we just draw a point.
			gl.Begin(OpenGL.LINES);
			gl.Vertex(0, 0, 0);
			gl.Vertex(this);
			gl.End();

			//	Restore old styles.
			point.Restore(gl);
		}

		/// <summary>
		/// Convert select names to object.
		/// </summary>
		IInteractable IInteractable.GetObjectFromSelectNames(int[] names)
		{
			return this;
		}

		/// <summary>
		/// This moves the direction of the normal.
		/// </summary>
		void IInteractable.DoMouseInteract(MouseOperation operation, float x, float y, float z)
		{
			if(operation == MouseOperation.Translate)
				Set(this.x += x, this.y += y, this.z += z);
		}
	}

	[Serializable()]
	public class Plane
	{
		public Plane() {}

		/// <summary>
		/// This finds out if a point is in front of, behind, or on this plane.
		/// </summary>
		/// <param name="point">The point to classify.</param>
		/// <returns>Less than 0 if behind, 0 if on, Greater than 0 if in front.</returns>
		public float ClassifyPoint(Vertex point)
		{
			//	(X-P)*N = 0. Where, X is a point to test, P is a point
			//	on the plane, and N is the normal to the plane.

			return normal.ScalarProduct(point);
		}

	/*	public Vertex Intersection(Vertex lineStart, Vertex lineEnd)
		{
			//	Get the vector of this line.
			Vertex ray = lineEnd - lineStart;

			//	Classify both points.
			float fStart = ClassifyPoint(lineStart);
			float fEnd = ClassifyPoint(lineEnd);

			//	If both are in front, or both are behind, its NOT an intersection.
			if((fStart < 0 && fEnd < 0) || (fStart > 0 && fEnd > 0))
				return null;

			//	Now find the actual intersection point POTENTIAL DIVIDE BY ZERO!
			float t = - (normal.ScalarProduct(lineStart) + position) /
				normal.ScalarProduct(ray);
        
			Vertex intersection = lineStart + (ray * t);
		}*/

		public Vertex position = new Vertex(0, 0, 0);
		public Vertex normal = new Vertex(0, 0, 0);

		public float[] equation; // ax + by + cz + d = 0.
	}

	[Serializable()]
	public class UV
	{
		public UV(){}
		public UV(float u, float v) {this.u = u; this.v = v;}

		public float u = 0;
		public float v = 0;

		public override string ToString()
		{
			return "(" + u + ", " + v + ")";
		}
		
		public static implicit operator float[](UV rhs)
		{
			return new float[] {rhs.u, rhs.v};
		}

		public float U
		{
			get {return u;}
			set {u = value;}
		}
		public float V
		{
			get {return v;}
			set {v = value;}
		}
	}

	
	/// <summary>
	/// An index into a set of arrays.
	/// </summary>
	[Serializable()]
	public class Index
	{
		//	Constructors.
		public Index(Index index) {vertex = index.vertex; uv = index.uv;}
		public Index(int vertex) {this.vertex = vertex;}
		public Index(int vertex, int uv) {this.vertex = vertex; this.uv = uv;}
		public Index(int vertex, int uv, int normal) {this.vertex = vertex; this.uv = uv; this.normal = normal;}

		#region Member Data

		/// <summary>
		/// This is the vertex in the polygon vertex array that the index refers to.
		/// </summary>
		protected int vertex = 0;

		/// <summary>
		/// This is the material coord in the polygon UV array that the index refers to.
		/// </summary>
		protected int uv = -1;

		/// <summary>
		/// This is the index into the normal array for this vertex. A value of -1 will
		/// generate a normal on the fly.
		/// </summary>
		protected int normal = -1;

		#endregion

		#region Properties

		[Description("The vertex index from the Polygons Vertex Array."), Category("Index")]
		public int Vertex
		{
			get {return vertex;}
			set {vertex = value;}
		}
		[Description("The UV index from the polygons UV Array."), Category("Index")]
		public int UV
		{
			get {return uv;}
			set {uv = value;}
		}
		[Description("The normal index, -1 means a normal will be generated on the fly."), Category("Index")]
		public int Normal
		{
			get {return normal;}
			set {normal = value;}
		}

		#endregion
	}


	/// <summary>
	/// A Face is a set of indices to vertices.
	/// </summary>
	[Serializable()]
	public class Face : IInteractable
	{
		public Face() {}

		/// <summary>
		/// Returns the plane equation (ax + by + cz + d = 0) of the face.
		/// </summary>
		/// <param name="parent">The parent polygon.</param>
		/// <returns>An array of four coefficients a,b,c,d.</returns>
		public float[] GetPlaneEquation(Polygon parent)
		{
			//	Get refs to vertices.
			Vertex v1 = parent.Vertices[indices[0].Vertex];
			Vertex v2 = parent.Vertices[indices[1].Vertex];
			Vertex v3 = parent.Vertices[indices[2].Vertex];

			float a = v1.Y*(v2.Z-v3.Z) + v2.Y*(v3.Z-v1.Z) + v3.Y*(v1.Z-v2.Z);
			float b = v1.Z*(v2.X-v3.X) + v2.Z*(v3.X-v1.X) + v3.Z*(v1.X-v2.X);
			float c = v1.X*(v2.Y-v3.Y) + v2.X*(v3.Y-v1.Y) + v3.X*(v1.Y-v2.Y);
			float d = -( v1.X*( v2.Y*v3.Z - v3.Y*v2.Z ) +
				v2.X*(v3.Y*v1.Z - v1.Y*v3.Z) +
				v3.X*(v1.Y*v2.Z - v2.Y*v1.Z) );

			return new float[] {a, b, c, d};
		}

		public Vertex GetSurfaceNormal(Polygon parent)
		{
			//	Do we have enough vertices for a normal?
			if(indices.Count < 3)
				return new Vertex(0, 0, 0);

			Vertex v1 = parent.Vertices[indices[0].Vertex];
			Vertex v2 = parent.Vertices[indices[1].Vertex];
			Vertex v3 = parent.Vertices[indices[2].Vertex];
			Vertex va = v1-v2;
			Vertex vb = v2-v3;
			return va.VectorProduct(vb);
		}

		/// <summary>
		/// This function reverses the order of the indices, i.e changes which direction
		/// this face faces in.
		/// </summary>
		/// <param name="parent">The parent polygon.</param>
		public void Reorder(Polygon parent)
		{
			//	Create a new index collection.
			IndexCollection newIndices = new IndexCollection();

			//	Go through every old index and add it.
			for(int i = 0; i < indices.Count; i++)
				newIndices.Add(indices[indices.Count - (i+1)]);

			//	Set the new index array.
			indices = newIndices;

			//	Recreate each normal.
			GenerateNormals(parent);
		}

		/// <summary>
		/// This function generates normals for every vertex.
		/// </summary>
		/// <param name="parent">The parent polygon.</param>
		public void GenerateNormals(Polygon parent)
		{
			if(Indices.Count >= 3)
			{
				foreach(Index index in Indices)
				{
					//	Do we have enough vertices for a normal?
					if(Indices.Count >= 3)
					{
						//	Create a normal.
						Vertex vNormal = GetSurfaceNormal(parent);
						vNormal.UnitLength();

						//	Add it to the normals, setting the index for next time.
						if(index.Normal != -1)
							parent.Normals.RemoveAt(index.Normal);
						index.Normal = parent.Normals.Add(new Normal(vNormal));
					}
				}
			}
		}

		public override string ToString()
		{
			return "Face, " + indices.Count + " indices";
		}


		#region IInteractable Code
        
		/// <summary>
		/// This is called when a face is interactable, so highlight it.
		/// </summary>
		void IInteractable.DrawPick(OpenGL gl)
		{
			//	Save all the attributes.
			gl.PushAttrib(OpenGL.ALL_ATTRIB_BITS);

			//	Disable lighting, set colour to red etc.
			gl.Disable(OpenGL.LIGHTING);
			gl.DepthFunc(OpenGL.LEQUAL);

			//	Now draw it with a faint red.
			gl.Enable(OpenGL.BLEND);
			gl.BlendFunc(OpenGL.SRC_ALPHA, OpenGL.ONE_MINUS_SRC_ALPHA);
			gl.Color(1, 0, 0, 0.2f);

			//	Draw the face.
			gl.Begin(OpenGL.POLYGON);
			foreach(Index index in indices)
				gl.Vertex(parentpoly.Vertices[index.Vertex]);
			gl.End();

			gl.Disable(OpenGL.BLEND);

			//	Draw the face.
			gl.Begin(OpenGL.LINE_LOOP);
			for(int i = 0; i < indices.Count - 1; i++)
				gl.Vertex(parentpoly.Vertices[indices[i].Vertex]);
			gl.End();

			

			gl.PopAttrib();
		}

		/// <summary>
		/// Convert select names to object.
		/// </summary>
		IInteractable IInteractable.GetObjectFromSelectNames(int[] names)
		{
			return this;
		}

		/// <summary>
		/// Interact.
		/// </summary>
		void IInteractable.DoMouseInteract(MouseOperation operation, float x, float y, float z)
		{
			if(operation == MouseOperation.Translate)
			{
				//	push the face.
				foreach(Index i in indices)
					parentpoly.Vertices[i.Vertex].Push(x, y, z);
			}
		}

		#endregion

		protected IndexCollection indices = new IndexCollection();
		public Polygon parentpoly;
		public int[] neighbourIndices;

		#region Properties
		public int Count
		{
			get {return indices.Count;}
		}
		public IndexCollection Indices
		{
			get {return indices;}
			set {indices = value;}
		}
		#endregion
	}

	[Serializable()]
	public class Matrix
	{
		public Matrix() {}

		public float[][] data = new float[][]
			{
				new float[] {1, 0, 0, 0},
				new float[] {0, 1, 0, 0},
				new float[] {0, 0, 1, 0},
				new float[] {0, 0, 0, 1},
		};

		public virtual void Transpose()
		{
			//	Make a new matrix data array the same size.
			float[][] newdata = new float[data.Length][];
			for(int i=0; i<data.Length; i++)
				newdata[i] = new float[data[i].Length];

			for(int i=0; i<data.Length; i++)
			{
				float[] row = data[i];
				for(int j=0; j<row.Length; j++)
					newdata[j][i] = data[i][j];
			}

			data = newdata;
		}
	}

	namespace Raytracing
	{
	
		public class Ray
		{
			public GLColor light = new GLColor(0, 0, 0, 0);
			public Vertex origin = new Vertex();
			public Vertex direction = new Vertex();
		}

		public class Intersection
		{
			public bool intersected = false;
			public Vertex normal = new Vertex();
			public Vertex point = new Vertex();
			public float closeness = -1;
		}

	}
}