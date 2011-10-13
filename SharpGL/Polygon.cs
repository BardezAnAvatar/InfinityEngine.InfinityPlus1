//    Polygon.cs
//    Created by Dave Kerr, 12/08/2003
//    Copyright (c) Dave Kerr 2003
//    http://www.codechamber.com
//
//	This polygon code is my own, but the shadow casting code came from
//	a nehe (http://nehe.gamedev.net) tutorial by Brett Porter.


using System;
using System.Collections;
using System.ComponentModel;

using SharpGL.SceneGraph.Collections;
using SharpGL.Raytracing;

namespace SharpGL.SceneGraph
{
	/// <summary>
	/// A polygon contains a set of 'faces' which are indexes into a single list
	/// of vertices. The main thing about polygons is that they are easily editable
	/// by the user, depending on the Context they're in.
	/// </summary>
	[Serializable]
	public class Polygon : SceneObject, IInteractable
	{
		public Polygon() 
		{
			name = "Polygon";
		}

		~Polygon()
		{

		}

		/// <summary>
		/// This function makes a simple cube shape.
		/// </summary>
		public void CreateCube()
		{
			uvs.Add(new UV(0, 0));
			uvs.Add(new UV(0, 1));
			uvs.Add(new UV(1, 1));
			uvs.Add(new UV(1, 0));
	
			//	Add the vertices.
			vertices.Add(new Vertex(-1, -1, -1));
			vertices.Add(new Vertex( 1, -1, -1));
			vertices.Add(new Vertex( 1, -1,  1));
			vertices.Add(new Vertex(-1, -1,  1));
			vertices.Add(new Vertex(-1,  1, -1));
			vertices.Add(new Vertex( 1,  1, -1));
			vertices.Add(new Vertex( 1,  1,  1));
			vertices.Add(new Vertex(-1,  1,  1));

			//	Add the faces.
			Face face = new Face();	//	bottom
			face.Indices.Add(new Index(1, 0));
			face.Indices.Add(new Index(2, 1));
			face.Indices.Add(new Index(3, 2));
			face.Indices.Add(new Index(0, 3));
			faces.Add(face);

			face = new Face();		//	top
			face.Indices.Add(new Index(7, 0));
			face.Indices.Add(new Index(6, 1));
			face.Indices.Add(new Index(5, 2));
			face.Indices.Add(new Index(4, 3));
			faces.Add(face);

			face = new Face();		//	right
			face.Indices.Add(new Index(5, 0));
			face.Indices.Add(new Index(6, 1));
			face.Indices.Add(new Index(2, 2));
			face.Indices.Add(new Index(1, 3));
			faces.Add(face);
	
			face = new Face();		//	left
			face.Indices.Add(new Index(7, 0));
			face.Indices.Add(new Index(4, 1));
			face.Indices.Add(new Index(0, 2));
			face.Indices.Add(new Index(3, 3));
			faces.Add(face);

			face = new Face();		// front
			face.Indices.Add(new Index(4, 0));
			face.Indices.Add(new Index(5, 1));
			face.Indices.Add(new Index(1, 2));
			face.Indices.Add(new Index(0, 3));
			faces.Add(face);

			face = new Face();		//	back
			face.Indices.Add(new Index(6, 0));
			face.Indices.Add(new Index(7, 1));
			face.Indices.Add(new Index(3, 2));
			face.Indices.Add(new Index(2, 3));
			faces.Add(face);
		}

		/// <summary>
		/// This function is cool, just stick in a set of points, it'll add them to the
		/// array, and create a face. It will take account of duplicate vertices too!
		/// </summary>
		/// <param name="vertexData">A set of vertices to make into a face.</param>
		public virtual void AddFaceFromVertexData(Vertex[] vertexData)
		{
			//	Create a face.
			Face newFace = new Face();

			//	Go through the vertices...
			foreach(Vertex v in vertexData)
			{
				//	Do we have this vertex already?
				int at = vertices.Find(0, v, 0.01f);
				
				//	Add the vertex, and index it.
				newFace.Indices.Add(new Index( (at == -1) ? vertices.Add(v) : at));
			}

			//	Add the face.
			faces.Add(newFace);
		}

		/// <summary>
		/// Triangulate this polygon.
		/// </summary>
		public void Triangulate()
		{
			FaceCollection newFaces = new FaceCollection();

			//	Go through each face...
			foreach(Face face in faces)
			{
				//	Number of triangles = vertices - 2.
				int triangles = face.Indices.Count - 2;

				//	Is it a triangle already?...
				if(triangles == 1)
				{
					newFaces.Add(face);
					continue;
				}

				//	Add a set of triangles.
				for(int i=0; i<triangles; i++)
				{
					Face triangle = new Face();
					triangle.Indices.Add(new Index(face.Indices[0]));
					triangle.Indices.Add(new Index(face.Indices[i+1]));
					triangle.Indices.Add(new Index(face.Indices[i+2]));
					triangle.Indices.Add(new Index(face.Indices[i+2]));
					triangle.Indices.Add(new Index(face.Indices[i+1]));
					newFaces.Add(triangle);
				}
			}

			faces.Clear();
			faces = newFaces;
		}

		/// <summary>
		/// This function draws the polygon.
		/// </summary>
		/// <param name="gl">OpenGL.</param>
		public override void Draw(OpenGL gl)
		{
			if(DoPreDraw(gl))
			{
				polyAttributes.Set(gl);

				foreach(Face face in faces)
				{
					//	Begin drawing a polygon.
					if(face.Indices.Count == 2)
						gl.Begin(OpenGL.LINES);
					else
						gl.Begin(OpenGL.POLYGON);

					foreach(Index index in face.Indices)
					{
						//	Set a texture coord (if any).
						if(index.UV != -1)
							gl.TexCoord2f(uvs[index.UV]);

						//	Set a normal, or generate one.
						if(index.Normal != -1)
							gl.Normal(normals[index.Normal]);
						else
						{
							//	Do we have enough vertices for a normal?
							if(face.Indices.Count >= 3)
							{
								//	Create a normal.
								Vertex vNormal = face.GetSurfaceNormal(this);
								vNormal.UnitLength();

								//	Add it to the normals, setting the index for next time.
								index.Normal = normals.Add(new Normal(vNormal));

								gl.Normal(vNormal);
							}
						}
					
						//	Set the vertex.
						gl.Vertex(vertices[index.Vertex]);
					}
					
					gl.End();
				}

				//	If the current context is edit vertices, draw the vertices.
				if(currentContext == Context.EditVertices)
				{
					//	Push all the attributes.
					gl.PushAttrib(OpenGL.ALL_ATTRIB_BITS);

					//	Set the colour to red, large points, no lighting.
					gl.Color(1, 0, 0, 1);
					gl.PointSize(5);
					gl.Disable(OpenGL.LIGHTING);
				
					//	Draw the control points.
					for(int point = 0; point < vertices.Count; point++)
					{
						//	Name the point, then draw it.
						gl.PushName((uint)point);
                    
						gl.Begin(OpenGL.POINTS);
						gl.Vertex(vertices[point]);
						gl.End();
						gl.PopName();
					}

					//	Restore the attributes.
					gl.PopAttrib();
				}

				//	If the current context is edit normals, draw the normals.
				if(currentContext == Context.EditNormals)
				{
					//	We're going to disable lighting.
					Attributes.Lighting lighting = new Attributes.Lighting();
					lighting.Enable = false;
					lighting.Set(gl);
		
					gl.Color(1, 0, 0);
					gl.Begin(OpenGL.LINES);

					//	Draw the normals points.
					foreach(Face face in faces)
					{
						foreach(Index index in face.Indices)
						{
							if(index.Normal == -1)
								continue;
							//	Translate to that vertex.
							Vertex v = vertices[index.Vertex];
							gl.PushName((uint)index.Normal);
							gl.Vertex(v);
							gl.Vertex(v + normals[index.Normal]);
							gl.PopName();	
						}
					}

					gl.End();
				
					lighting.Restore(gl);
				}

				if(currentContext == Context.EditFaces)
				{
					for(int i = 0; i<Faces.Count; i++)
					{
						//	Push the face name.
						gl.PushName((uint)i);

						//	Set the parent poly.
						faces[i].parentpoly = this;

						//	Draw it.
						((IInteractable)faces[i]).DrawPick(gl);
					
						//	Pop the name.
						gl.PopName();
					}
				
				}

				//	Draw normals if we have to.
				if(drawNormals)
				{
					//	Set the colour to red.
					gl.PushAttrib(OpenGL.ALL_ATTRIB_BITS);
					gl.Color(1, 0, 0, 1);
					gl.Disable(OpenGL.LIGHTING);

					//	Go through each face.
					foreach(Face face in faces)
					{
						//	Go though each index.
						foreach(Index index in face.Indices)
						{
							//	Make sure it's got a normal, and a vertex.
							if(index.Normal != -1 && index.Vertex != -1)
							{
								//	Get the vertex.
								Vertex vertex = vertices[index.Vertex];

								//	Get the normal vertex.
								Normal normal = normals[index.Normal];
								Vertex vertex2 = vertex + normal;

								gl.Begin(OpenGL.LINES);
								gl.Vertex(vertex);
								gl.Vertex(vertex2);
								gl.End();
							}
						}
					}

					//	Restore the attributes.
					gl.PopAttrib();
				}

				polyAttributes.Restore(gl);

				DoPostDraw(gl);
			}
		}

		IInteractable IInteractable.GetObjectFromSelectNames(int[] names)
		{
			//	If it has another name, then it's either a UV, vertex or face,
			//	depending on the current context.
			if(names.Length == 1)
				return this;
			
			switch(currentContext)
			{
				case Context.EditVertices:
					return vertices[names[1]] as IInteractable;
				case Context.EditFaces:
					return faces[names[1]] as IInteractable;
				default: //	i.e context static.
					return this;
			}
		}

		void IInteractable.DrawPick(OpenGL gl)
		{
			Draw(gl);
		}

		/// <summary>
		/// This creates a polygon from a height map (any picture). Black is low, 
		/// and the colors are high (the lighter the color, the higher the surface).
		/// </summary>
		/// <param name="filename">Path of the image file.</param>
		/// <param name="xPoints">Number of points along X.</param>
		/// <param name="yPoints">Number of points along Y.</param>
		/// <returns>True if sucessful, false otherwise.</returns>
		public virtual bool CreateFromMap(string filename, int xPoints, int yPoints)
		{
			//	Try and load the image file.
			System.Drawing.Bitmap map = new System.Drawing.Bitmap(filename);
			if(map.Size.IsEmpty)
				return false;

			//	Set the descriptive name.
			Name = "Map created from '" + filename + "'";

			//	Get points.
			for(int y=0; y < yPoints; y++)
			{
				int yValue = (map.Height / yPoints) * y;

				for(int x=0; x < xPoints; x++)
				{
					int xValue = (map.Width / xPoints) * x;

					//	Get the pixel.
					System.Drawing.Color col = map.GetPixel(xValue, yValue);

					float xPos = (float)x / (float)xPoints;
					float yPos = (float)y / (float)yPoints;

					//	Create a control point from it.
					Vertex v = new Vertex(xPos, 0, yPos);

					//	Add the 'height', based on color.
					v.Y = (float)col.R / 255.0f + (float)col.G / 255.0f + 
						(float)col.B / 255.0f;
					
					//	Add this vertex to the vertices array.
					Vertices.Add(v);
				}
			}

			//	Create faces for the polygon.
			for(int y=0; y < (yPoints-1); y++)
			{
				for(int x=0; x < (xPoints-1); x++)
				{
					//	Create the face.
					Face face = new Face();

					//	Create vertex indicies.
					int nTopLeft = (y * xPoints) + x;
					int nBottomLeft = ((y + 1) * xPoints) + x;
					
					face.Indices.Add(new Index(nTopLeft));
					face.Indices.Add(new Index(nTopLeft + 1));
					face.Indices.Add(new Index(nBottomLeft + 1));
					face.Indices.Add(new Index(nBottomLeft));

					// Add the face.
					Faces.Add(face);
				}
			}

			return true;
		}

		/// <summary>
		/// This function performs lossless optimisation on the polygon, it should be 
		/// called when the geometry changes, and the polygon goes into static mode.
		/// </summary>
		/// <returns>The amount of optimisation (as a %).</returns>
		protected virtual float OptimisePolygon()
		{
			//	Check for any null faces.
			float facesBefore = faces.Count;

			for(int i=0; i<faces.Count; i++)
			{
				if(faces[i].Count == 0)
					faces.RemoveAt(i--);
			}
			float facesAfter = faces.Count;

			return (facesAfter / facesBefore) * 100;
		}

		/// <summary>
		/// Call this function as soon as you change the polygons geometry, it will
		/// re-generate normals, etc.
		/// </summary>
		/// <param name="regenerateNormals">Regenerate Normals.</param>
		public virtual void Validate(bool regenerateNormals)
		{
			if(regenerateNormals)
				normals.Clear();

			//	Go through each face.
			foreach(Face face in faces)
			{
				if(regenerateNormals)
				{
					//	Find a normal for the face.
					Normal normal = new Normal(face.GetSurfaceNormal(this));

					//	Does this normal already exist?
					int index = normals.Find(0, normal, 0.001f);

					if(index == -1)
						index = normals.Add(normal);

					//	Set the index normal.
					foreach(Index i in face.Indices)
						i.Normal = index;
				}
			}
		}

		/// <summary>
		/// This function calculates the neighbours in a face.
		/// </summary>
		protected virtual void SetConnectivity()
		{
			if(connectivitySet == true)
				return;

			//	Create space for edge indices.
			foreach(Face face in faces)
			{
				face.neighbourIndices = new int[face.Indices.Count];
				for(int i=0; i < face.neighbourIndices.Length; i++)
					face.neighbourIndices[i] = -2;		//	-2 = no neighbour calculated.
			}

			for(int faceAindex = 0; faceAindex < faces.Count; faceAindex++)
			{
				Face faceA = faces[faceAindex];

				for(int edgeA = 0; edgeA < faceA.neighbourIndices.Length; edgeA++)
				{
					if(faceA.neighbourIndices[edgeA] == -2)
					{
						//	We don't yet know the neighbor.
						bool found = false;
						for(int faceBindex = 0; faceBindex < faces.Count; faceBindex++)
						{
							if(faceBindex == faceAindex) continue;

							Face faceB = faces[faceBindex];
							

							for(int edgeB = 0; edgeB < faceB.neighbourIndices.Length; edgeB++)
							{
								int vA1 = faceA.Indices[edgeA].Vertex;
								int vA2 = faceA.Indices[(edgeA+1)%faceA.Indices.Count].Vertex;
								int vB1 = faceB.Indices[edgeB].Vertex;
								int vB2 = faceB.Indices[(edgeB+1)%faceB.Indices.Count].Vertex;

								//	Check if they're neighbours...
								if((vA1 == vB1 && vA2 == vB2) || (vA1 == vB2 && vA2 == vB1))
								{
									faceA.neighbourIndices[edgeA] = faceBindex;
									faceB.neighbourIndices[edgeB] = faceAindex;
									found = true;
									break;
								}
							}
							if(found)
								break;
						}
						if(found == false)
							faceA.neighbourIndices[edgeA] = -1;
					}
				}
			}

			connectivitySet = true;
		}

	
		/// <summary>
		/// Casts a real time 3D shadow.
		/// </summary>
		/// <param name="gl">The OpenGL object.</param>
		/// <param name="light">The source light.</param>
		public void CastShadow(OpenGL gl, Collections.LightCollection lights)
		{
			//	Set the connectivity, (calculate the neighbours of each face).
			SetConnectivity();

			//	Go through every light in the scene.
			foreach(Lights.Light light in lights)
			{
				//	Skip null lights.
				if(light == null)
					continue;

				//	Skip turned off lights and non shadow lights.
				if(light.On == false || light.CastShadow == false)
					continue;

				//	Every face will have a visibility setting.
				bool[] facesVisible = new bool[faces.Count];

				//	Get the light position relative to the polygon.
				Vertex lightPos = light.Translate;
				lightPos = lightPos - Translate;

				//	Go through every face, finding out whether it's visible to the light.
				for(int nFace = 0; nFace < faces.Count; nFace++)
				{
					//	Get a reference to the face.
					Face face = faces[nFace];

					//	Is this face facing the light?
					float[] planeEquation = face.GetPlaneEquation(this);
					float side = planeEquation[0] * lightPos.X + 
						planeEquation[1] * lightPos.Y + 
						planeEquation[2] * lightPos.Z + planeEquation[3];
					facesVisible[nFace] = (side > 0) ? true : false;
				}

				//	Save all the attributes.
				gl.PushAttrib(OpenGL.ALL_ATTRIB_BITS);
			
				//	Turn off lighting.
				gl.Disable(OpenGL.LIGHTING);

				//	Turn off writing to the depth mask.
				gl.DepthMask(0);
				gl.DepthFunc(OpenGL.LEQUAL);

				//	Turn on stencil buffer testing.
				gl.Enable(OpenGL.STENCIL_TEST);

				//	Translate our shadow volumes.
				gl.PushMatrix();
				Transform(gl);

				//	Don't draw to the color buffer.
				gl.ColorMask(0, 0, 0, 0);
				gl.StencilFunc(OpenGL.ALWAYS, 1, 0xFFFFFFFF);

				gl.Enable(OpenGL.CULL_FACE);
			
				//	First Pass. Increase Stencil Value In The Shadow
				gl.FrontFace(OpenGL.CCW);
				gl.StencilOp(OpenGL.KEEP, OpenGL.KEEP, OpenGL.INCR);
				DoShadowPass(gl, lightPos, facesVisible);

				//	Second Pass. Decrease Stencil Value In The Shadow
				gl.FrontFace(OpenGL.CW);
				gl.StencilOp(OpenGL.KEEP, OpenGL.KEEP, OpenGL.DECR);
				DoShadowPass(gl, lightPos, facesVisible);

				gl.FrontFace(OpenGL.CCW);

				gl.PopMatrix();
			
				//	Enable writing to the color buffer.
				gl.ColorMask(1, 1, 1, 1); 
            
				// Draw A Shadowing Rectangle Covering The Entire Screen
				gl.Color(light.ShadowColor);
				gl.Enable(OpenGL.BLEND);
				gl.BlendFunc(OpenGL.SRC_ALPHA, OpenGL.ONE_MINUS_SRC_ALPHA);
				gl.StencilFunc(OpenGL.NOTEQUAL, 0, 0xFFFFFFF);
				gl.StencilOp(OpenGL.KEEP, OpenGL.KEEP, OpenGL.KEEP);
				
				Quadrics.Sphere shadow = new Quadrics.Sphere();
				shadow.Scale.Set(shadowSize, shadowSize, shadowSize);
				shadow.Draw(gl);
	
				gl.PopAttrib();
			}
		}


		/// <summary>
		/// This is part of the shadow casting code, it does a shadowpass for the
		/// polygon using the specified light.
		/// </summary>
		/// <param name="gl">The OpenGL object.</param>
		/// <param name="light">The light casting the shadow.</param>
		/// <param name="visibleArray">An array of bools.</param>
		protected virtual void DoShadowPass(OpenGL gl, Vertex lightPos, bool[] visibleArray)
		{
			//	Go through each face.
			for(int i = 0; i < faces.Count; i++)
			{
				//	Get a reference to the face.
				Face face = faces[i];
				
				//	Is the face visible...
				if(visibleArray[i])
				{
					//	Go through each edge...
					for(int j = 0; j < face.Indices.Count; j++)
					{
						//	Get the neighbour of this edge.
						int neighbourIndex = face.neighbourIndices[j];

						//	If there's no neighbour, or the neighbour ain't visible, it's
						//	an edge...
						if(neighbourIndex == -1 || visibleArray[neighbourIndex] == false )
						{
							//	Get the edge vertices.
							Vertex v1 = vertices[face.Indices[j].Vertex];
							Vertex v2 = vertices[face.Indices[(j+1)%face.Indices.Count].Vertex];
						
							//	Create the two distant vertices.
							Vertex v3 = (v1 - lightPos) * 100;
							Vertex v4 = (v2 - lightPos) * 100;
							
							//	Draw the shadow volume.
							gl.Begin(OpenGL.TRIANGLE_STRIP);
							gl.Vertex(v1);
							gl.Vertex(v1 + v3);
							gl.Vertex(v2);
							gl.Vertex(v2 + v4);
							gl.End();
						}
					}
				}
			}			
		}

		/// <summary>
		/// This function tests to see if a ray interesects the polygon.
		/// </summary>
		/// <param name="ray">The ray you want to test.</param>
		/// <param name="pointOfIntersection">The actual point of intersection (if any).</param>
		/// <returns>The distance from the origin of the ray to the intersection, or -1 if there
		/// is no intersection.</returns>
		public Intersection TestIntersection(Ray ray)
		{
			Intersection intersect = new Intersection();

			//	This code came from jgt intersect_triangle code (search dogpile for it).
			foreach(Face face in faces)
			{
				//	Assert that it's a triangle.
				if(face.Count != 3)
					continue;

			
				//	Find the point of intersection upon the plane, as a point 't' along
				//	the ray.
				Vertex point1OnPlane = vertices[face.Indices[0].Vertex];
				Vertex point2OnPlane = vertices[face.Indices[1].Vertex];
				Vertex point3OnPlane = vertices[face.Indices[2].Vertex];
				Vertex midpointOpp1 = (point2OnPlane + point3OnPlane) / 2;
				Vertex midpointOpp2 = (point1OnPlane + point3OnPlane) / 2;
				Vertex midpointOpp3 = (point1OnPlane + point2OnPlane) / 2;
				
				Vertex planeNormal = face.GetSurfaceNormal(this);


				Vertex diff = point1OnPlane - ray.origin;
				float s1 = diff.ScalarProduct(planeNormal);
				float s2 =  ray.direction.ScalarProduct(planeNormal);

				if(s2 == 0)
					continue;
				float t = s1 / s2;
				if(t < 0)
					continue;
	
				float denomintor = planeNormal.ScalarProduct(ray.direction);
				if(denomintor < 0.00001f && denomintor > -0.00001f)
					continue;	//	doesn't intersect the plane.

			//	Vertex v = point1OnPlane - ray.origin;
			//	float t = (v.ScalarProduct(planeNormal)) / denomintor;

				//	Now we can get the point of intersection.
				Vertex vIntersect = ray.origin + (ray.direction * t);

				//	Do my cool test.
				Vertex vectorTo1 = vIntersect - point1OnPlane;
				Vertex vectorTo2 = vIntersect - point2OnPlane;
				Vertex vectorTo3 = vIntersect - point3OnPlane;
				Vertex vectorMidTo1 = midpointOpp1 - point1OnPlane;
				Vertex vectorMidTo2 = midpointOpp2 - point2OnPlane;
				Vertex vectorMidTo3 = midpointOpp3 - point3OnPlane;
				
				if(vectorTo1.Magnitude() > vectorMidTo1.Magnitude())
					continue;
				if(vectorTo2.Magnitude() > vectorMidTo2.Magnitude())
					continue;
				if(vectorTo3.Magnitude() > vectorMidTo3.Magnitude())
					continue;

				if(intersect.closeness == -1 || t < intersect.closeness) 
				{
					//	It's fucking intersection city man
					intersect.point = vIntersect;
					intersect.intersected = true;
					intersect.normal = planeNormal;
					intersect.closeness = t;
				}
					
				#region The Old Raytracing code					
					
                ////	Get the three vertices in the face.
                //Vertex v1 = vertices[face.Indices[0].Vertex];
                //Vertex v2 = vertices[face.Indices[1].Vertex];
                //Vertex v3 = vertices[face.Indices[2].Vertex];

                ////	Find the vectors for the two edges of the tri that share v1.
                //Vertex edge1 = v2 - v1;
                //Vertex edge2 = v3 - v1;

                ////	Begin calculating the determinant, also used to calculate U parameter.
                //Vertex pVector = ray.direction.VectorProduct(edge2);
                //float determinant = edge1.ScalarProduct(pVector);

                ////	If the determinant is less than zero, it's behind the triangle i.e.
                ////	the back face. If it's very close to zero (errorvalue) it on the plane.
                //float errorvalue = 0.000001f;
                //if(determinant < errorvalue && determinant > -errorvalue)
                //    continue;

                ////	Calculate the distance from vertex 1 to ray origin.
                //Vertex tVector = ray.origin - v1;

                ////	Calculate the U parameter and test bounds.
                //float u = tVector.ScalarProduct(pVector);
                //if(u < 0 || u > determinant)
                //    continue;

                ////	Prepare to test V parameter.
                //Vertex qVector = tVector.VectorProduct(edge1);

                ////	Calculate the v parameter and test bounds.
                //float v = ray.direction.ScalarProduct(qVector);
                //if(v < 0 || v > determinant)
                //    continue;

                ////	Calculate t, scale parameters.
                //float t = edge2.ScalarProduct(qVector);
                //float inverseDeterminant = 1.0f / determinant;
                //t *= inverseDeterminant;
                //u *= inverseDeterminant;
                //v *= inverseDeterminant;

                ////	Now we have a point of intersection. Yeah. Let's got some 
                ////	magnitude into the action.
                //Vertex pointOfIntersection = new Vertex(t, u, v);

                ////	OK! We've got an intersection, find how far away it it by simple
                ////	stuff: make a vector from the origin and the intersection, and
                ////	get it's magnitude.
                //Vertex vOriginToIntersection = pointOfIntersection - ray.origin;
                //float magnitude = (float)vOriginToIntersection.Magnitude();

                ////	Is this magnitude is smaller (closer) than any we've found so far?
                //if(intersect.closeness == -1 || magnitude < intersect.closeness) 
                //{
                //    //	This is the closest intersection yet, so fill in the 
                //    //	intersect structure.
                //    intersect.point.Set(t, u, v);
                //    intersect.intersected = true;
                //    intersect.normal = face.GetSurfaceNormal(this);
                //    intersect.closeness = magnitude;
                //}

				#endregion
			}

			return intersect;
		}

		public virtual Intersection Raytrace(Ray ray, Scene scene)
		{
			//	First we see if this ray intersects this polygon.
			Intersection intersect = TestIntersection(ray);

			//	If there wasn't an intersection, return.
			if(intersect.intersected == false)
				return intersect;

			//	There was an intersection, find the color of this point on the 
			//	polygon.
			foreach(SharpGL.SceneGraph.Lights.Light light in scene.Lights)
			{
				if(light.On)
				{
					//	Can we see this light? Cast a shadow ray.
					Ray shadowRay = new Ray();
					bool shadow = false;
					shadowRay.origin = intersect.point;
					shadowRay.direction = light.Translate - shadowRay.origin;

					//	Test it with every polygon.
					foreach(Polygon p in scene.Polygons)
					{
						if(p == this) continue;
						Intersection shadowIntersect = p.TestIntersection(shadowRay);
						if(shadowIntersect.intersected)
						{
							shadow = true;
							break;
						}
					}

					if(shadow == false)
					{
						//	Now find out what this light complements to our color.
						float angle = light.Direction.ScalarProduct(intersect.normal);
						ray.light += material.CalculateLighting(light, angle);
						ray.light.Clamp();
					}
				}
			}

			return intersect;
		}

		/// <summary>
		/// This function subdivides the faces of this polygon.
		/// </summary>
		/// <param name="smooth">If set to true the faces will be smoothed.</param>
		/// <returns>The number of faces in the new subdivided polygon.</returns>
		public int Subdivide()
		{
			FaceCollection newFaces = new FaceCollection();

			foreach(Face face in Faces)
			{
				//	Make sure the face is a triangle.
				if(face.Count != 3)
					continue;

				//	Now get the vertices of the face.
				Vertex v1 = Vertices[face.Indices[0].Vertex];
				Vertex v2 = Vertices[face.Indices[1].Vertex];
				Vertex v3 = Vertices[face.Indices[2].Vertex];

				//	Add the vertices to get a the midpoint of the edge formed by those
				//	vectors.
				Vertex vMidpoint = (v1 + v2 + v3) / 3;
				Index iMidpoint = new Index(Vertices.Add(vMidpoint));

				//	Now make three new faces from the old vertices and the midpoint.
				Face newFace = new Face();
				newFace.Indices.Add(new Index(face.Indices[0]));
				newFace.Indices.Add(new Index(face.Indices[1]));
				newFace.Indices.Add(iMidpoint);
				newFaces.Add(newFace);

				newFace = new Face();
				newFace.Indices.Add(new Index(face.Indices[1]));
				newFace.Indices.Add(new Index(face.Indices[2]));
				newFace.Indices.Add(iMidpoint);
				newFaces.Add(newFace);

				newFace = new Face();
				newFace.Indices.Add(new Index(face.Indices[2]));
				newFace.Indices.Add(new Index(face.Indices[0]));
				newFace.Indices.Add(iMidpoint);
				newFaces.Add(newFace);
			}

			faces = newFaces;

			return faces.Count;
		}

		#region Enumerations

		public enum Context
		{
			Static,			//	The polygon can't be edited, and is optimised for speed.
			EditVertices,	//	The polygons vertices are converted to control points.
			EditFaces,		//	The polygons faces can be edited.
			EditNormals,	//	The face normals can be edited.
			EditUVs,		//	The UV's can be edited.
		}

		#endregion

		#region Member Data
	
		/// <summary>
		/// Are the face neighbours calculated?
		/// </summary>
		private bool connectivitySet = false;

		/// <summary>
		/// The faces that make up the polygon.
		/// </summary>
		protected FaceCollection faces = new FaceCollection();

		/// <summary>
		/// The vertices that make up the polygon.
		/// </summary>
		protected VertexCollection vertices = new VertexCollection();

		/// <summary>
		/// The UV coordinates (texture coodinates) for the polygon.
		/// </summary>
		protected UVCollection uvs = new UVCollection();

		/// <summary>
		/// The normals of the polygon object.
		/// </summary>
		protected NormalCollection normals = new NormalCollection();

		/// <summary>
		/// The current 'context' of the polygon, e.g edit faces.
		/// </summary>
		protected Context currentContext = Context.Static;

		/// <summary>
		/// Does this polygon cast a real time shadow?
		/// </summary>
		protected bool castsShadow = false;
		
		/// <summary>
		/// The size of the shadow in each direction.
		/// </summary>
		protected float shadowSize = 10;

		/// <summary>
		/// Should the normals be drawn?
		/// </summary>
		protected bool drawNormals = false;

		protected Attributes.Polygon polyAttributes = new Attributes.Polygon();

		#endregion

		#region Properties

		[Description("The faces that make up the polygon."), Category("Polygon")]
		public FaceCollection Faces
		{
			get {return faces;}
			set {faces = value; modified = true;}
		}
		[Description("The vertices that make up the polygon."), Category("Polygon")]
		public VertexCollection Vertices
		{
			get {return vertices;}
			set {vertices = value; modified = true;}
		}
		[Description("The material coordinates."), Category("Polygon")]
		public UVCollection UVs
		{
			get {return uvs;}
			set {uvs = value; modified = true;}
		}
		[Description("The normals."), Category("Normals")]
		public NormalCollection Normals
		{
			get {return normals;}
			set {normals = value; modified = true;}
		}
		[Description("The current context of the polygon."), Category("Context")]
		public Context CurrentContext
		{
			get {return currentContext;}
			set {currentContext  = value; modified = true;}
		}
		[Description("Polygon Attributes"), Category("Attributes")]
		public Attributes.Polygon Attributes
		{
			get {return polyAttributes;}
			set {polyAttributes = value; modified = true;}
		}
		[Description("Should the object cast a real time shadow?"), Category("Shadow")]
		public bool CastsShadow
		{
			get {return castsShadow;}
			set {castsShadow = value; modified = true;}
		}
		[Description("How large should the shadow be (how far does it extend in each direction)."), Category("Shadow")]
		public float ShadowSize
		{
			get {return shadowSize;}
			set {shadowSize = value; modified = true;}
		}
		[Description("Should normals be drawn for each face?"), Category("Polygon")]
		public bool DrawNormals
		{
			get {return drawNormals;}
			set {drawNormals = value; modified = true;}
		}
		#endregion
	}

}
