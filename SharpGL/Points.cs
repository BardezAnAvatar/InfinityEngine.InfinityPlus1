//    Points.cs
//    Created by Dave Kerr, 28/01/2004
//    Copyright (c) Dave Kerr 2004
//    http://www.codechamber.com

//	This class is a new type of primitive (specifically it's a SceneObject).
//	It demonstrates how to draw a large number of points very quickly, using the
//	same draw styles for each one. It would be added to the custom primitives in the 
//	scene. 

//	Every point has the same attributes, they are set at the beginning of the drawing
//	process. Edit the pointAttributes member and you change how the points are drawn.

//	The points are stored as a raw float array, not as an array of say, Point classes,
//	or an array of vertices. Points is designed to be very fast and memory efficient.
//	Although a raw float array is old fasioned, it is significantly faster. However, 
//	I've named it pointsRaw to make it explicit that it's NOT an array of vertices.

//	This class is interactable (the interactable code is *sweet*, by simply deriving
//	from the interactive interface you automatically have the functionaility to move, 
//	scale, rotate etc your points. A smoother way might be to override DrawInteractable
//	and draw a 'widgit' in the centre of the the field, i.e a handle to move it around 
//	with.

using System;
using System.Collections;
using System.ComponentModel;
using SharpGL.SceneGraph.Lights;


namespace SharpGL.SceneGraph
{
	/// <summary>
	/// Points is a class that holds a large array of vertices, each with an 
	/// intensity value, describing the brightness.
	/// </summary>
	[Serializable]
	public class Points : SceneObject, IInteractable
	{
		public Points()
		{
			//	Give this object a descriptive name, so you can recognise it
			//	in the scene graph.
			name = "Points";
		}

		/// <summary>
		/// Draw the points.
		/// </summary>
		public override void Draw(OpenGL gl)
		{
			if(pointsRaw == null)
				return;

			//	If you want you're custom object to be drawn, then override this function.

			//	The drawing of points is very simple, go through the point array, set
			//	the intensity, draw the point.

			//	This is the first function that must be called, DoPreDraw. It basicly
			//	sets up certain settings and organises the stack so that what you draw
			//	now doesn't interfere with what you will draw next. You actually 
			//	only need to draw it it returns true, when it returns false
			//	the object is actually drawn via a display list.
			if(DoPreDraw(gl))
			{
				//	This next code saves the current point attributes, then sets the point
				//	attributes for this set of points, so that the previous attributes 
				//	are not lost (we restore them later).
				pointAttributes.Set(gl);

				//	Begin drawing lines.
				gl.Begin(OpenGL.POINTS);
			
				//	Get the points count.
				int count = pointsRaw.Length / 4;
				for(int i=0, index=0; i < count; i++,index+=4)
				{
					//	Set the colour.
					float intensity = pointsRaw[index + 3];
					gl.Color(intensity, intensity, intensity, intensity);

					//	Draw the point.
					gl.Vertex(pointsRaw[index], pointsRaw[index+1], pointsRaw[index+2]);
				}

				//	End the drawing.
				gl.End();

				//	Restore the previous point settings.
				pointAttributes.Restore(gl);

				//	Finalise the drawing process.
				DoPostDraw(gl);
			}
		}

		/// <summary>
		/// These are all the opengl settings involved with points.
		/// </summary>
		protected Attributes.Point pointAttributes = new Attributes.Point();

		/// <summary>
		/// These are the points. See the top of the file for a detailed explanation.
		/// </summary>
		protected float[] pointsRaw = null;

		[Description("Point Attributes"), Category("Attributes")]
		public Attributes.Point Attributes
		{
			get {return pointAttributes;}
			set 
			{
				//	We're changing how the points are drawn, so we MUST set 
				//	'modified' to true.
				pointAttributes = value;
				modified = true;
			}
		}
		[Description("Raw Points Data"), Category("Points")]
		public float[] PointsRaw
		{
			get {return pointsRaw;}
			set 
			{
				pointsRaw = value;
				modified = true;
			}
		}
	}
}
