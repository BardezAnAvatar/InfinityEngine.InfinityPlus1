//    Scene.cs
//    Created by Dave Kerr, 12/08/2003
//    Copyright (c) Dave Kerr 2003
//    http://www.codechamber.com

//	This is the core scene graph code, check it out, it's well commented. ;-)


using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Collections;
using System.Diagnostics;
using System.Drawing;

using SharpGL;
using SharpGL.SceneGraph.Quadrics;
using SharpGL.SceneGraph.Cameras;
using SharpGL.SceneGraph.Evaluators;
using SharpGL.SceneGraph.Lights;
using SharpGL.SceneGraph.Collections;
using SharpGL.SceneGraph.Attributes;
using SharpGL.Raytracing;

namespace SharpGL.SceneGraph
{
	[TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	[Serializable()]
	public class Scene
	{
		public Scene() {}

		protected virtual void InternalInitialise(SceneType sceneType)
		{
			//	Add the default camera.
			currentCamera = new CameraPerspective();
			currentCamera.Translate = new Vertex(-5.0f, 8.0f, 10.0f);
			cameras.Add(currentCamera);
			
			//	Find out how many lights OpenGL can support.
			int [] umaxlights = new int[1];
			gl.GetInteger(OpenGL.MAX_LIGHTS, umaxlights);

			//	Now create as many lights as we can.
			for(uint u = 0; u < umaxlights[0]; u++)
			{
				Light light = new Light();
				light.GLCode = OpenGL.LIGHT0 + u;
				light.Name = "Light " + u.ToString();
				if(u == 0)
				{
					light.Ambient = new GLColor(1.0f, 1.0f, 1.0f, 0.0f);
					light.Diffuse  = new GLColor(1.0f, 1.0f, 1.0f, 0.0f);
					light.On = true;
				}
				lights.Add(light);
			}

			//	Set the scene type.
			SetSceneType(sceneType);
            
			//	Initialise stock drawing.
			gl.InitialiseStockDrawing();

			//	Set the Scene OpenGL for the quadrics and nurbs.
			Quadric.SceneOpenGL = OpenGL;
			NURBSBase.SceneOpenGL = OpenGL;
		}

		/// <summary>
		/// As soon as you have created the render context for the scene, call this
		/// function to initialise it, this is vital, to create underlying OpenGL
		/// objects.
		/// </summary>
		/// <param name="sceneType">The type of scene to create.</param>
		public virtual void Initialise(int width, int height, SceneType sceneType)
		{
			//	Create OpenGL.
			gl.Create(width, height);
	
			//	Finish initialisation.
			InternalInitialise(sceneType);			
		}

		/// <summary>
		/// Call this function to iniatilise a scene loaded from a file.
		/// </summary>
		/// <param name="ctrl">The OpenGLCtrl.</param>
		/// <param name="sceneType">The scene type.</param>
		public virtual void IntialiseFromSerial(int width, int height, SceneType sceneType)
		{
			//	Create OpenGL.
			gl = new OpenGL();
			gl.Create(width, height);

			//	Find out how many lights OpenGL can support.
			int [] umaxlights = new int[1];
			gl.GetInteger(OpenGL.MAX_LIGHTS, umaxlights);

			//	Re-create as many openGL lights as we can.
			for(uint u = 0; u < umaxlights[0]; u++)
			{
				if(u < Lights.Count)
				{
					Light light = lights[(int)u];
					light.GLCode = OpenGL.LIGHT0 + u;
				}
			}

			//	Re-create all the quadrics
			foreach(Quadric quadric in quadrics)
				quadric.Create(gl);

			//	Set the scene type.
			SetSceneType(sceneType);

			//	Initialise stock drawing.
			gl.InitialiseStockDrawing();

			//	Set the Scene OpenGL for the quadrics.
			Quadric.SceneOpenGL = OpenGL;
		}


		/// <summary>
		/// Like a backwards version of initialise, this function destroys the scene
		/// and the underlying render context.
		/// </summary>
		public virtual void Destroy()
		{
		}

		/// <summary>
		/// This function converts the entire scene into a triangle scene.
		/// </summary>
		public virtual void TriangulateScene()
		{
			/*	Feedback.Polygonator polygonator = new Feedback.Polygonator();

					//	Go through every polygon, and triangulate it.
					foreach(Polygon poly in polygons)
						poly.Triangulate();

					//	Go through the quadrics and triangulate them.
					foreach(Quadric quad in quadrics)
						polygons.Add(polygonator.CreatePolygon(quad, currentCamera));

					//	Go through the evaluators and triangulate them.
					foreach(Evaluator eval in evaluators)
						polygons.Add(polygonator.CreatePolygon(eval, currentCamera));
		
					quadrics.Clear();
					evaluators.Clear();*/
		}
				
		
		/// <summary>
		/// This function performs hit testing on the scene (the scene being the light
		/// array, the quadric array, and the polygon array etc).
		/// </summary>
		/// <param name="x">X coord of the point to hit test.</param>
		/// <param name="y">Y coord of the point to hit test.</param>
		/// <returns>An array containing all of the object hit by the point.</returns>
		public virtual InteractableCollection DoHitTest(int x, int y)
		{
			//	If we don't have a current camera, we cannot hit test.
			if(currentCamera == null)
			{
				System.Windows.Forms.MessageBox.Show("There is no current camera!");
				return new InteractableCollection();
			}

			//	Create an array that will be the viewport.
			int[] viewport = new int[4];
			
			//	Get the viewport, then convert the mouse point to an opengl point.
			gl.GetInteger(OpenGL.VIEWPORT, viewport);
			y = viewport[3] - y;

			//	Create a select buffer.
			uint[] selectBuffer = new uint[512];
			gl.SelectBuffer(512, selectBuffer);
			
			//	Enter select mode.
			gl.RenderMode(OpenGL.SELECT);

			//	Create a combined array.
			InteractableCollection combined = new InteractableCollection();
		//	Polygon nullPoly = new Polygon();
		//	combined.Add(nullPoly);
			combined.AddRange(quadrics);
			combined.AddRange(lights);
			combined.AddRange(polygons);
			combined.AddRange(evaluators);
			combined.AddRange(cameras);
			combined.AddRange(sceneObjects);

			//	Initialise the names, and add the first name.
			gl.InitNames();
			gl.PushName(0);
		
			//	Push matrix, set up projection, then load matrix.
			gl.MatrixMode(OpenGL.PROJECTION);
			gl.PushMatrix();
			gl.LoadIdentity();
			gl.PickMatrix(x, y, 4, 4, viewport);
			currentCamera.TransformProjectionMatrix(gl);
			currentCamera.LookAt(gl);
			gl.MatrixMode(OpenGL.MODELVIEW);
			gl.LoadIdentity();

			//	Draw everything in the combined array, with the index as it's name.
			for(int i = 0; i < combined.Count; i++)
			{
				SceneObject ob = (SceneObject)combined[i];
				gl.LoadName((uint)(i+1));
				((IInteractable)ob).DrawPick(gl);
			}
													
			//	Pop matrix and flush commands.
			gl.MatrixMode(OpenGL.PROJECTION);
			gl.PopMatrix();
			gl.MatrixMode(OpenGL.MODELVIEW);
			gl.Flush();

			//	End selection.
			int hits = gl.RenderMode(OpenGL.RENDER);
			uint posinarray = 0;
			InteractableCollection hitArray = new InteractableCollection();

			for(int hit = 0; hit < hits; hit++)
			{
				uint nameCount = selectBuffer[posinarray++];
				uint z1 = selectBuffer[posinarray++];
				uint z2 = selectBuffer[posinarray++];

				if(nameCount == 0)
					continue;
				
				//	Create an array that'll hold the names.
				int[] names = new int[nameCount];
					
				//	Add each name to the array.
				for(int name = 0; name < nameCount; name++)
					names[name] = (int)selectBuffer[posinarray++];

				//	Now ask the hit object what has been hit, then add it to the array.
				SceneObject hitObject = (SceneObject)combined[names[0] - 1];	//	names[0] is the object.
				hitArray.Add(((IInteractable)hitObject).GetObjectFromSelectNames(names));
			}

			return hitArray;
		}

		/// <summary>
		/// This is a really useful function. Pass any scene object of any type to it
		/// and it'll be 'jammed' into the correct array. The array it was inserted to
		/// is returned, in case extra stuff has to be done. Null is returned if the object
		/// is invalid, or doesn't belong in the scene. This is good if you need to add
		/// an object that is of an unknown type, but don't add known types like this,
		/// directly adding them to the arrays is much faster.
		/// </summary>
		/// <param name="sceneObject">The object to insert into the scene.</param>
		/// <returns>The list the object was inserted into.</returns>
		public virtual IList Jam(object sceneObject)
		{
			//	Get the object type.
			Type type = sceneObject.GetType();
			
			if(type == typeof(Polygon) || type.IsSubclassOf(typeof(Polygon)))
			{
				polygons.Add((Polygon)sceneObject);
				return polygons;
			}
			else if(type == typeof(Camera) || type.IsSubclassOf(typeof(Camera)))
			{
				cameras.Add((Camera)sceneObject);
				return cameras;
			}
			else if(type == typeof(Light) || type.IsSubclassOf(typeof(Light)))
			{
				lights.Add((Light)sceneObject);
				return lights;
			}
			else if(type == typeof(Evaluator) || type.IsSubclassOf(typeof(Evaluator)))
			{
				evaluators.Add((Evaluator)sceneObject);
				return evaluators;
			}
			else if(type == typeof(Material) || type.IsSubclassOf(typeof(Material)))
			{
				materials.Add((Material)sceneObject);
				return materials;
			}
			else if(type == typeof(Quadric) || type.IsSubclassOf(typeof(Quadric)))
			{
				quadrics.Add((Quadric)sceneObject);
				return quadrics;
			}
			else if(type == typeof(SceneObject) || type.IsSubclassOf(typeof(SceneObject)))
			{
				sceneObjects.Add((SceneObject)sceneObject);
				return sceneObjects;
			}

			return null;
		}

		/// <summary>
		/// This function does the opposite of jam. It finds the object in the scene,
		/// and then removes it from the correct list. It then returns the list the object
		/// was in.
		/// </summary>
		/// <param name="sceneObject">The object to remove.</param>
		/// <returns>The list the object was contained in.</returns>
		public virtual IList UnJam(object sceneObject)
		{
			//	Get the object type.
			Type type = sceneObject.GetType();
			
			if(type == typeof(Polygon) || type.IsSubclassOf(typeof(Polygon)))
			{
				polygons.Remove((Polygon)sceneObject);
				return polygons;
			}
			else if(type == typeof(Camera) || type.IsSubclassOf(typeof(Camera)))
			{
				cameras.Remove((Camera)sceneObject);
				return cameras;
			}
			else if(type == typeof(Light) || type.IsSubclassOf(typeof(Light)))
			{
				lights.Remove((Light)sceneObject);
				return lights;
			}
			else if(type == typeof(Evaluator) || type.IsSubclassOf(typeof(Evaluator)))
			{
				evaluators.Remove((Evaluator)sceneObject);
				return evaluators;
			}
			else if(type == typeof(Material) || type.IsSubclassOf(typeof(Material)))
			{
				materials.Remove((Material)sceneObject);
				return materials;
			}
			else if(type == typeof(Quadric) || type.IsSubclassOf(typeof(Quadric)))
			{
				quadrics.Remove((Quadric)sceneObject);
				return quadrics;
			}
			else if(type == typeof(SceneObject) || type.IsSubclassOf(typeof(SceneObject)))
			{
				sceneObjects.Remove((SceneObject)sceneObject);
				return sceneObjects;
			}

			return null;
		}

		/// <summary>
		/// This function creates an array with all the scene objects in it.
		/// </summary>
		/// <returns></returns>
		public virtual ArrayList CreateMergedArray()
		{
			//	Create a single array with everything in it.
			ArrayList merged = new ArrayList();
			merged.AddRange(sceneObjects);
			merged.AddRange(lights);
			merged.AddRange(quadrics);
			merged.AddRange(cameras);
			merged.AddRange(polygons);
			merged.AddRange(materials);
			merged.AddRange(evaluators);
			merged.AddRange(cameras);

			//	Return the array.
			return merged;
		}

		/// <summary>
		/// This function draws all of the objects in the scene (i.e. every quadric
		/// in the quadrics arraylist etc).
		/// </summary>
		public virtual void Draw()
		{
			//	Create a performance monitor.
			PerformanceMonitor performance = new PerformanceMonitor();

			performance.Monitor("Clearing buffers");

			//	Set the clear color.
			float[] clear = clearColour;
			gl.ClearColor(clear[0], clear[1], clear[2], clear[3]);

			//	Clear.
			gl.Clear(OpenGL.COLOR_BUFFER_BIT | OpenGL.DEPTH_BUFFER_BIT |
				OpenGL.STENCIL_BUFFER_BIT);

			gl.BindTexture(OpenGL.TEXTURE_2D, 0);

			if(drawExtras)
			{
				performance.Monitor("Drawing Stock Scene");
				gl.Color(gridColour);
				gl.StockDrawing.Grid.Call(gl);
				gl.StockDrawing.Axies.Call(gl);
			}

			performance.Monitor("Setting and drawing Lights");

			foreach(Light light in lights)
			{
				//	Set the lights properties into our OpenGL.
				light.Set(gl);
			
				//	Draw the light into OpenGL.
				if(drawExtras)
					light.Draw(gl);
			}

			performance.Monitor("Drawing Custom Objects");

			foreach(SceneObject ob in sceneObjects)
				ob.Draw(gl);

			performance.Monitor("Drawing Quadrics");

			foreach(Quadric quad in quadrics)
				quad.Draw(gl);

			performance.Monitor("Drawing Evaluators");

			foreach(Evaluator evaluator in evaluators)
				evaluator.Draw(gl);

			performance.Monitor("Drawing Cameras");

			foreach(Camera cam in cameras)
			{
				if(drawExtras)
					cam.Draw(gl);
			}

			performance.Monitor("Drawing Polygons");

			foreach(Polygon poly in polygons)
			{
				//	Draw the polygon into OpenGL.
				poly.Draw(gl);

				if(poly.CastsShadow)
					poly.CastShadow(gl, lights);
			}

			gl.Flush();

			performance.EndMonitor();

			if(drawPerformance)
				performance.Draw(gl.GDIGraphics);
		}

		internal class ScreenPixel
		{
			public GLColor color = new GLColor();
			public Ray ray = new Ray();
			public int x = 0;
			public int y = 0;
			public Vertex worldpos = new Vertex();
		}

		public virtual void Raytrace()
		{
			//	First, we need the matricies and viewport.
			double[] modelview = new double[16];
			double[] projection = new double[16];
			int[] viewport = new int[4];
			gl.GetDouble(SharpGL.OpenGL.MODELVIEW_MATRIX, modelview);
			gl.GetDouble(SharpGL.OpenGL.PROJECTION_MATRIX, projection);
			gl.GetInteger(SharpGL.OpenGL.VIEWPORT, viewport);
			int screenwidth = viewport[2];
			int screenheight = viewport[3];

			//	From frustum data, we make a screen origin, and s/t vectors.
			Vertex s = new Vertex(0, 0.03f, 0);
			Vertex t = new Vertex(0, 0, 0.05f);
			Vertex vScreenOrigin = new Vertex(0, 0, 5);

			//	Go through every pixel we have, and convert it into a screen pixel.
			ScreenPixel[] pixels = new ScreenPixel[viewport[2] * viewport[3]];


			for(int y = 0; y < screenheight; y++)
			{
				for(int x = 0; x < screenwidth; x++)
				{
					//	Get plane coordinates first of all.
					int planeX = x - (screenwidth / 2);
					int planeY = y - (screenwidth / 2);

					float worldX = vScreenOrigin.X + (planeX * t.X) + (planeY * s.X);
					float worldY = vScreenOrigin.Y + (planeX * t.Y) + (planeY * s.Y);
					float worldZ = vScreenOrigin.Z + (planeX * t.Z) + (planeY * s.Z);

					//	Finally, pack all that data into a ScreenPixel.
					ScreenPixel pixel = new ScreenPixel();
					pixel.x = x;
					pixel.y = y;
					pixel.worldpos = new Vertex(worldX, worldY, worldZ);
					pixel.ray.origin = currentCamera.Translate;
					pixel.ray.direction = pixel.worldpos - currentCamera.Translate;
					
					pixels[(y * viewport[2]) + x] = pixel;
				}
			}
			
			/*	for(int y = 0; y < viewport[3]; y++)
				{
					for(int x = 0; x < viewport[2]; x++)
					{
						//	Create a screenpixel.
						ScreenPixel pixel =new ScreenPixel();
						pixel = new ScreenPixel();
						pixel.x = x;
						pixel.y = y;

						//	Get the world coordinate.
						double[] wx = new Double[1];
						double[] wy = new Double[1];
						double[] wz = new Double[1];
						gl.UnProject(x, y, 0, modelview, projection, viewport,
							wx, wy, wz);
						pixel.worldpos.Set((float)wx[0], (float)wy[0], (float)wz[0]);

						//	Set the ray.
						pixel.ray.origin = currentCamera.Translate;
						pixel.ray.direction = pixel.worldpos - currentCamera.Translate;

						pixels[(y * viewport[2]) + x] = pixel;
					}
				}*/

			System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(viewport[2], viewport[3]);
			
			
			//	Now go through every ray and test for intersections.
			int pixelcounter = 0;
			int pixelcount = viewport[2] * viewport[3];
			foreach(ScreenPixel pix in pixels)
			{
				//	Raytrace the polygons.
				Intersection closest = new Intersection();
				foreach(Polygon poly in Polygons)
				{
					Intersection i = poly.Raytrace(pix.ray, this);
					if(i.intersected && (closest.intersected == false || i.closeness < closest.closeness))
						closest = i;
				}

				if(closest.intersected == true)
				{
					System.Console.WriteLine("i = {0}, only {1} left!\n", 
						closest.closeness, pixelcount - pixelcounter);		
				}
				bmp.SetPixel(pix.x, pix.y, pix.ray.light);
				pixelcounter++;
				
				
			}
			bmp.Save("C:\\raytraced.bmp");


			
		}

		/// <summary>
		/// Use this function to resize the scene window, and also to look through
		/// the current camera.
		/// </summary>
		/// <param name="width">Width of the screen.</param>
		/// <param name="height">Height of the screen.</param>
		public virtual void Resize(int width, int height)
		{
			if(width != -1 && height != -1)
			{
				//	Resize.
				gl.Viewport(0, 0, width, height);
				
				//	Then project.
				if(currentCamera != null)
					currentCamera.Project(gl);
			}
		}

		/// <summary>
		/// This function automatically makes an object look selected.
		/// </summary>
		/// <param name="ob"></param>
		public virtual void DrawObjectSelected(SceneObject ob)
		{
			gl.Enable(OpenGL.BLEND);
			gl.BlendFunc(OpenGL.SRC_ALPHA,OpenGL.ONE);

			gl.Color(1, 0, 0, 0.2f);
			gl.Disable(OpenGL.LIGHTING);
			gl.DepthFunc(OpenGL.ALWAYS);

			ob.Draw(gl);

			gl.Enable(OpenGL.LIGHTING);

			gl.Disable(OpenGL.BLEND);
			gl.DepthFunc(OpenGL.LESS);
		}

		/// <summary>
		/// This functions sets OpenGL and SharpGL parameters to match the scenetype
		/// passed to it.
		/// </summary>
		/// <param name="sceneType">The SceneType to set to.</param>
		public virtual void SetSceneType(SceneType sceneType)
		{
			switch(sceneType)
			{
				case SceneType.HighQuality:
					//	Enable depth testing and normals, lighting.
					gl.Enable(OpenGL.DEPTH_TEST);
					//	gl.Enable(OpenGL.NORMALIZE);
					gl.Enable(OpenGL.LIGHTING);
					gl.Enable(OpenGL.TEXTURE_2D);
					gl.LightModel(OpenGL.LIGHT_MODEL_TWO_SIDE, OpenGL.TRUE);
					break;

				case SceneType.HighSpeed:
					//	Just enable the depth test.
					gl.Enable(OpenGL.DEPTH_TEST);
					gl.LightModel(OpenGL.LIGHT_MODEL_TWO_SIDE, OpenGL.TRUE);
					break;

				case SceneType.Application:
					//	Enable depth test and extras.
					gl.Enable(OpenGL.DEPTH_TEST);
					gl.LightModel(OpenGL.LIGHT_MODEL_TWO_SIDE, OpenGL.TRUE);
					drawExtras = true;
					break;

				default:
					break;
			}
		}

		#region Enumerations

		protected enum MergedArrayType
		{
			All,
			Interactable,
			OpenGLDependant,
		}

		#endregion

		#region Member Data

		/// <summary>
		/// This is the OpenGL class, use it to call OpenGL functions.
		/// </summary>
		[NonSerializedAttribute()]
		protected OpenGL gl = new OpenGL();

		/// <summary>
		/// This class contains simply scene objects, if you want to add custom objects,
		/// you can add them here.
		/// </summary>
		protected SceneObjectCollection sceneObjects = new SceneObjectCollection();

		/// <summary>
		/// As soon as 'Create' is called, this array is filled with as many lights
		/// as opengl can support. Do not add or remove them, simply turn them
		/// on and off as you need to, or ignore them and use your own lighting code.
		/// </summary>
		protected LightCollection lights = new LightCollection();

		/// <summary>
		/// You can add quadrics to this list using the .NET editor if you want to
		/// as it is implemented as a property. The qudarics are drawn using the 
		/// Draw function, automatically.
		/// </summary>
		protected QuadricCollection quadrics = new QuadricCollection();

		/// <summary>
		/// This is an array of cameras. A single perspective camera is created by default.
		/// You are free to ignore cameras and use your own code, however, if you want to
		/// use the SharpGL functions such as DoHitTest, you will have to use cameras.
		/// </summary>
		protected CameraCollection cameras = new CameraCollection();

		/// <summary>
		/// This is an array of polygon objects, drawn automatically.
		/// </summary>
		protected PolygonCollection polygons = new PolygonCollection();

		/// <summary>
		/// The material library of the scene.
		/// </summary>
		protected MaterialCollection materials = new MaterialCollection();

		/// <summary>
		/// The evaluators in the scene.
		/// </summary>
		protected EvaluatorCollection evaluators = new EvaluatorCollection();

		/// <summary>
		/// This is the camera that is currently being used to view the scene.
		/// </summary>
		protected Camera currentCamera;

		/// <summary>
		/// This allows you to draw extras, such as lights and cameras and a grid.
		/// </summary>
		protected bool drawExtras = false;

		/// <summary>
		/// This allows the scene to draw the performance data.
		/// </summary>
		protected bool drawPerformance = false;

		/// <summary>
		/// This is the colour of the background of the scene.
		/// </summary>
		protected GLColor clearColour = new GLColor(0, 0, 0, 0);

		/// <summary>
		/// This is the colour of the grid.
		/// </summary>
		protected GLColor gridColour = new GLColor(1, 1, 1, 1);

		#endregion
		
		#region Properties for OpenGL.

		[Description("OpenGL API Wrapper Class"), Category("OpenGL/External")]
		public OpenGL OpenGL
		{
			get {return gl;}
			set {gl = value;}
		}
		[Description("The custom in the scene."), Category("Scene")]
		public SceneObjectCollection CustomObjects
		{
			get {return sceneObjects;}
			set {sceneObjects = value; Draw();}
		}
		[Description("The quadrics in the scene."), Category("Scene")]
		public QuadricCollection Quadrics
		{
			get {return quadrics;}
			set {quadrics = value; Draw();}
		}
		[Description("The lights in the scene."), Category("Scene")]
		public LightCollection Lights
		{
			get {return lights;}
			set {lights = value;}
		}
		[Description("The cameras in the scene."), Category("Scene")]
		public CameraCollection Cameras
		{
			get {return cameras;}
			set {cameras = value;}
		}
		[Description("The polygons in the scene."), Category("Scene")]
		public PolygonCollection Polygons
		{
			get {return polygons;}
			set {polygons = value;}
		}
		[Description("The materials in the scene."), Category("Scene")]
		public MaterialCollection Materials
		{
			get {return materials;}
			set {materials = value;}
		}
		[Description("The evaluators in the scene."), Category("Scene")]
		public EvaluatorCollection Evaluators
		{
			get {return evaluators;}
			set {evaluators = value;}
		}
		[Description("The current camera being used to view the scene."), Category("Scene")]
		public Camera CurrentCamera
		{
			get {return currentCamera;}
			set {currentCamera = value;}
		}
		[Description("Should extras (such as cameras and lights) be drawn?"), Category("Scene")]
		public bool DrawExtras
		{
			get {return drawExtras;}
			set {drawExtras = value;}
		}
		[Description("Should performace information be drawn?"), Category("Scene")]
		public bool DrawPerformance
		{
			get {return drawPerformance;}
			set {drawPerformance = value;}
		}
		[Description("The background colour."), Category("Scene")]
		public Color ClearColor
		{
			get {return clearColour;}
			set {clearColour = value;}
		}
		[Description("The grid colour."), Category("Scene")]
		public Color GridColor
		{
			get {return gridColour;}
			set {gridColour = value;}
		}
  			
		#endregion
	}

	/// <summary>
	/// The SceneType enum is used to choose what sort of scene you want to initialise
	/// with, to save you having to look up all the settings.
	/// </summary>
	public enum SceneType
	{
		/// <summary>
		/// The High-Quality setting is for detailed renders.
		/// </summary>
		HighQuality,

		/// <summary>
		/// The high-speed setting is for games, or faster renders. It uses
		/// various optimisations to speed up render times.
		/// </summary>
		HighSpeed,

		/// <summary>
		/// The application setting is for 3D applications, where you would
		/// want to see stock objects, and view in a wireframe mode.
		/// </summary>
		Application,
	}
}
