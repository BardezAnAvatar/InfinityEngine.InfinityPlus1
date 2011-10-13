//    Quadric.cs
//    Created by Dave Kerr, 12/08/2003
//    Copyright (c) Dave Kerr 2003
//    http://www.codechamber.com


using System;
using System.ComponentModel;

namespace SharpGL.SceneGraph
{
	namespace Quadrics
	{

		/// <summary>
		/// Quadric is the base class for all SharpGL quadric objects.
		/// </summary>
		[Serializable()]
		public abstract class Quadric : SceneObject, IInteractable
		{
			public enum DrawStyle : uint
			{
				Point = OpenGL.GLU_POINT,
				Line = OpenGL.GLU_LINE,
				Silhouette = OpenGL.GLU_SILHOUETTE,
				Fill = OpenGL.GLU_FILL,
			}

			public enum Orientation : uint
			{
				Outside = OpenGL.GLU_OUTSIDE,
				Inside = OpenGL.GLU_INSIDE,
			}

			public enum Normals : uint 
			{
				None = OpenGL.GLU_NONE,
				Flat = OpenGL.GLU_FLAT,
				Smooth = OpenGL.GLU_SMOOTH,
			}


			public Quadric()
			{
				name = "Quadric";

				//	Create the underlying quadric object using our OpenGL reference.
				if(sceneOpenGL != null)
					Create(sceneOpenGL);
			}

			~Quadric()
			{
				//	Destroy the underlying quadric object.
				if(sceneOpenGL != null)
				{
					Destroy(sceneOpenGL);
					glQuadric = IntPtr.Zero;
				}
			}

			/// <summary>
			/// Create an OpenGL Quadric object to represent this quadric.
			/// </summary>
			/// <param name="gl"></param>
			public virtual void Create(OpenGL gl)
			{
				glQuadric = gl.NewQuadric();
			}

			/// <summary>
			/// Call this function to destroy the OpenGL quadric object that represents this
			/// quadric.
			/// </summary>
			/// <param name="gl"></param>
			public virtual void Destroy(OpenGL gl)
			{	
				gl.DeleteQuadric(glQuadric);
			}

			public override void Draw(OpenGL gl)
			{
				//	Set the quadric properties.
				gl.QuadricDrawStyle(glQuadric, (uint)drawStyle);
				gl.QuadricOrientation(glQuadric, (int)orientation);
				gl.QuadricNormals(glQuadric, (int)normals);
				gl.QuadricTexture(glQuadric, textureCoords ? 1 : 0);
			}


			/// <summary>
			/// This is the pointer to the opengl quadric object.
			/// </summary>
			protected IntPtr glQuadric = IntPtr.Zero;

			/// <summary>
			/// The draw style, can be filled, line, silouhette or points.
			/// </summary>
			protected DrawStyle drawStyle = DrawStyle.Fill;
			protected Orientation orientation = Orientation.Outside;
			protected Normals normals = Normals.Smooth;
			protected bool textureCoords = false;
			protected Texture tex = new Texture();

			/// <summary>
			/// This variable should be set to the Scenes OpenGL as soon as possible, or
			/// quadrics cannot be created.
			/// </summary>
			protected static OpenGL sceneOpenGL = null;

			[Description("Draw Style of the quadric."), Category("Quadric")]
			public DrawStyle QuadricDrawStyle
			{
				get {return drawStyle;}
				set {drawStyle = value; modified = true;}
			}
			[Description("What way normals face."), Category("Quadric")]
			public Orientation NormalOrientation
			{
				get {return orientation;}
				set {orientation = value; modified = true;}
			}
			[Description("How normals are generated."), Category("Quadric")]
			public Normals NormalGeneration
			{
				get {return normals;}
				set {normals = value; modified = true;}
			}
			[Description("Should OpenGL generate texture coordinates for the Quadric?"), Category("Quadric")]
			public bool TextureCoords
			{
				get {return textureCoords;}
				set {textureCoords = value; modified = true;}
			}
			[Description("The OpenGL object from the main scene."), Category("Quadric")]
			public static OpenGL SceneOpenGL
			{
				set {sceneOpenGL = value;}
			}
		}

		/// <summary>
		/// The Sphere class wraps the sphere quadric.
		/// </summary>
		[Serializable()]
		public class Sphere : Quadric
		{
			public Sphere()	{name = "Sphere";}
			
			//	Sphere data.
			double radius = 1.0;
			int slices = 16;
			int stacks = 20;

			override public void Draw(OpenGL gl)
			{
				//	Transform matrix etc.
				if(DoPreDraw(gl))
				{

					//	Draw a sphere with the current settings.
					base.Draw(gl);
					gl.Sphere(glQuadric, radius, slices, stacks);
				
					//	Restore matrix etc.
					DoPostDraw(gl);
				}
			}

			#region Properties

			[Description("Radius of the Sphere."), Category("Sphere")]
			public double Radius
			{
				get {return radius;}
				set {radius = value; modified = true;}
			}
			[Description("Number of slices."), Category("Sphere")]
			public int Slices
			{
				get {return slices;}
				set {slices = value; modified = true;}
			}
			[Description("Number of stacks."), Category("Sphere")]
			public int Stacks
			{
				get {return stacks;}
				set {stacks = value; modified = true;}
			}

			#endregion
		}

		/// <summary>
		/// The Cylinder class wraps the cylinder quadric.
		/// </summary>
		[Serializable()]
		public class Cylinder : Quadric
		{
			public Cylinder(){name = "Cylinder";}
			
			//	Sphere data.
			protected double baseRadius = 1.0;
			protected double topRadius = 0.0;
			protected double height = 1.0;
			protected int slices = 16;
			protected int stacks = 20;

			override public void Draw(OpenGL gl)
			{
				if(DoPreDraw(gl))
				{

					base.Draw(gl);
					gl.Cylinder(glQuadric, baseRadius, topRadius, height, slices, stacks);
				
					DoPostDraw(gl);
				}
			}

			#region Properties

			[Description("Radius of the base of the cylinder."), Category("Cylinder")]
			public double BaseRadius
			{
				get {return baseRadius;}
				set {baseRadius = value; modified = true;}
			}
			[Description("Radius of the top of the cylinder."), Category("Cylinder")]
			public double TopRadius
			{
				get {return topRadius;}
				set {topRadius = value; modified = true;}
			}
			[Description("Height of the cylinder."), Category("Cylinder")]
			public double Height
			{
				get {return height;}
				set {height = value; modified = true;}
			}
			[Description("Number of slices."), Category("Cylinder")]
			public int Slices
			{
				get {return slices;}
				set {slices = value; modified = true;}
			}
			[Description("Number of stacks."), Category("Cylinder")]
			public int Stacks
			{
				get {return stacks;}
				set {stacks = value; modified = true;}
			}

			#endregion
		}

		/// <summary>
		/// The Disk class wraps both the disk and partial disk quadrics.
		/// </summary>
		[Serializable()]
		public class Disk : Quadric
		{
			public Disk(){name = "Disk";}
			
			//	Sphere data.
			double innerRadius = 0.0;
			double outerRadius = 2.0;
			double startAngle = 0.0;
			double sweepAngle = 360.0;
			int slices = 16;
			int loops = 20;

			override public void Draw(OpenGL gl)
			{
				if(DoPreDraw(gl))
				{

					base.Draw(gl);
					gl.PartialDisk(glQuadric, innerRadius, outerRadius, 
						slices, loops, startAngle, sweepAngle);

					DoPostDraw(gl);
				}
			}

			#region Properties
		
			[Description("Radius of the hole on the inside of the disk."), Category("Disk")]
			public double InnerRadius
			{
				get {return innerRadius;}
				set {innerRadius = value; modified = true;}
			}
			[Description("Radius of the disk."), Category("Disk")]
			public double OuterRadius
			{
				get {return outerRadius;}
				set {outerRadius = value; modified = true;}
			}
			[Description("Start angle of the partial disk."), Category("Disk")]
			public double StartAngle
			{
				get {return startAngle;}
				set {startAngle = value; modified = true;}
			}
			[Description("Size of the partial disk."), Category("Disk")]
			public double SweepAngle
			{
				get {return sweepAngle;}
				set {sweepAngle = value; modified = true;}
			}
			[Description("Number of slices."), Category("Disk")]
			public int Slices
			{
				get {return slices;}
				set {slices = value; modified = true;}
			}
			[Description("Number of loops."), Category("Disk")]
			public int Loops
			{
				get {return loops;}
				set {loops = value; modified = true;}
			}

			#endregion
		}
	}
}
