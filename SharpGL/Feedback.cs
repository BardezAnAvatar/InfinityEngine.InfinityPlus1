//    Feedback.cs
//    Created by Dave Kerr, 12/08/2003
//    Copyright (c) Dave Kerr 2003
//    http://www.codechamber.com


using System;
using System.Collections;

namespace SharpGL.SceneGraph
{
	namespace Feedback
	{
		/// <summary>
		/// The feedback class handles feedback easily and well. It is 100% dependant on
		/// OpenGL so use it with care!
		/// </summary>
		public class Feedback
		{
			public Feedback()
			{
			}

			/// <summary>
			/// This function begins feedback, recording the scene etc. End finishes
			/// feedback, and parses the feedback buffer.
			/// </summary>
			public virtual void Begin(OpenGL gl)
			{
				//	Set the feedback buffer.
				gl.FeedbackBuffer(feedbackBuffer.Length, OpenGL.GL_3D_COLOR_TEXTURE, feedbackBuffer);
		
				//	Set the rendermode to feedback.
				gl.RenderMode(OpenGL.FEEDBACK);
			}

			/// <summary>
			/// This function stops the collection of feedback data.
			/// </summary>
			/// <returns>The feedback array.</returns>
			public virtual float[] End(OpenGL gl)
			{
				//	End feedback mode.
				int values = gl.RenderMode(OpenGL.RENDER);

				//	Check for buffer size.
				if(values == -1)
				{
					System.Windows.Forms.MessageBox.Show("The scene contained too much data! The data buffer has been doubled in size now, please try again.");
					feedbackBuffer = new float [feedbackBuffer.Length * 2];
					return new float[] {-1};
				}

				//	Parse the data.
				ParseData(gl, values);

				return feedbackBuffer;
			}

			/// <summary>
			/// Override this function to do custom functions on the data.
			/// </summary>
			/// <param name="values">Number of values.</param>
			protected virtual void ParseData(OpenGL gl, int values){}

			protected float[] feedbackBuffer = new float[40960];
		
			public string FeedbackBufferSize
			{
				get {return (feedbackBuffer.Length * 4) + " bytes" ;}
			}
		}

		public class Triangulator : Feedback
		{
			/// <summary>
			/// This takes the feedback data and turns it into triangles.
			/// </summary>
			/// <param name="values">The number of triangles.</param>
			protected override void ParseData(OpenGL gl, int values)
			{
				int count = values;

				//	Create the triangle.
				triangle = new Polygon();
				triangle.Name = "Triangulated Polygon";
			
				//	For every value in the buffer...
				while(count != 0)
				{
					//	Get the token.
					float token = feedbackBuffer[values - count];
			
					//	Decrement count (move to the next token).
					count--;
				
					//	Check the type of the token.
					switch((int)token)
					{
						case (int)OpenGL.PASS_THROUGH_TOKEN:
							count--;
							break;
						case (int)OpenGL.POINT_TOKEN:
							//	We use only polygons, skip this single vertex (11 floats).
							count -= 11;
							break;
						case (int)OpenGL.LINE_TOKEN:
							//	We use only polygons, skip this vertex pair (22 floats).
							count -= 22;
							break;
						case (int)OpenGL.LINE_RESET_TOKEN:
							//	We use only polygons, skip this vertex pair (22 floats).
							count -= 22;
							break;
						case (int)OpenGL.POLYGON_TOKEN:

							//	Get the number of vertices.
							int vertexCount = (int)feedbackBuffer[values - count--];
						
							//	Create an array of vertices.
							Vertex[] vertices = new Vertex[vertexCount];

							//	Parse them.
							for(int i=0; i<vertexCount; i++)
							{
								vertices[i] = new Vertex();
								double x = (double)feedbackBuffer[values - count--];
								double y = (double)feedbackBuffer[values - count--];
								double z = (double)feedbackBuffer[values - count--];
								double[] coords = gl.UnProject(x, y, z);
								vertices[i].X = (float)coords[0];
								vertices[i].Y = (float)coords[1];
								vertices[i].Z = (float)coords[2];

								//	Ignore the four r,g,b,a values and four material coords.
                                count -= 8;
							}
	
							//	Add a new face to the current polygon.
							triangle.AddFaceFromVertexData(vertices);
						
							break;
					}
				}

				//	Triangulate it.
				triangle.Triangulate();
			}

			protected Polygon triangle;

			#region Properties
			public Polygon Triangle
			{
				get {return triangle;}
				set {triangle = value;}
			}
			#endregion
		}

		/// <summary>
		/// Pass this class any SceneObject and it'll send you back a polygon.
		/// </summary>
		public class Polygonator : Triangulator
		{
			/// <summary>
			/// This is the main function of the class, it'll create a triangulated polygon
			/// from and SceneObject.
			/// </summary>
			/// <param name="sourceObject">The object to convert.</param>
			/// <param name="guarenteedView">A camera that can see the whole object.</param>
			/// <returns>A polygon created from 'sourceObject'.</returns>
			public Polygon CreatePolygon(OpenGL gl, SceneObject sourceObject, Cameras.Camera guarenteedView)
			{
				//	Save the current camera data.
				gl.MatrixMode(OpenGL.PROJECTION);
				gl.PushMatrix();

				//	Look through the camera that can see the object.
				guarenteedView.Project(gl);

				//	Start triangulation.
				Begin(gl);

				//	Draw the object.
				sourceObject.Draw(gl);

				//	End triangulation.
				End(gl);

				Polygon newPoly = Triangle;
				newPoly.Name = sourceObject.Name + " (Triangulated Poly)";
				return newPoly;
			}
		}

	}
}
