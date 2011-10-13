//    SceneObject.cs
//    Created by Dave Kerr, 12/08/2003
//    Copyright (c) Dave Kerr 2003
//    http://www.codechamber.com

//	SceneObject is the most important class (apart from Scene and OpenGL) it is the base
//	for all objects in a scene, lights, quadrics etc.

//	SharpGL 1.7 : Fundamental change! DoPreDraw now returns a bool. If it returns 
//	false you should just end the function, like this:
//
//	public void DrawMyObject(OpenGL gl)
//	{
//		if(DoPreDraw(gl))
//		{
//			..draw t'object
//			DoPostDraw(gl);
//		}
//	}
//	
//	This is essential, because DoPreDraw now automatically optimises objects to
//	use display lists. Because of this, every time you call draw, the vast amount
//	of the time all that code gets skipped and a display list is called instead!
//	This is a very powerful feature that speeds code up significantly.

//	SharpGL 1.7 : IMPORTANT
//	From now on any class you derive from SceneObject must always set the 
//	'modified' value to true whenever ANY change is made to it that will affect 
//	how it is drawn (i.e. material changes, interact mode changes etC).
//	This is the one thing you must remember, if you do, SharpGL will optimise
//	to displaylists for you!

//	Don't use the IOpenGLInteractable code yet, it is unfinished.

using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SharpGL.SceneGraph
{
	/// <summary>
	/// The mouseoperation enum descibes what the mouse is used for. Select has no effect
	/// on most classes, it selects objects. The other modes affect the select objects
	/// though.
	/// </summary>
	public enum MouseOperation
	{
		None,			//	The mouse will not affect the scene.
		Select,			//	The mouse selects objects.
		Translate,		//	The mouse moves objects.
		Scale,			//	The mouse scales objects.
		Rotate,			//	The mouse rotates objects.
	}

	/// <summary>
	/// The sceneobject class provides the base class for any object in a scene
	/// including lights, cameras, polygons, quadrics, nurbs etc. It provides common
	/// functionality, such as hit testing.
	/// </summary>
	[Serializable]
	public abstract class SceneObject : IInteractable
	{
		public SceneObject()
		{
			nCount++;
			Console.WriteLine("SceneObject.cs: Created scene object {0}",nCount);

		}

		~SceneObject()
		{
			nCount--;
			Console.WriteLine("SceneObject.cs: Destroy Called, {0} objects left.", nCount);
		}

		public virtual void Draw(OpenGL gl){}
		
		/// <summary>
		/// Classes derived from SceneObject can call this to perform common drawing
		/// operations, it pushes the modelview stack, set's attributes etc.
		/// </summary>
		/// <param name="gl"></param>
		/// <returns>True if the object must be drawn.</returns>
		protected internal virtual bool DoPreDraw(OpenGL gl)
		{
			//	DoPreDraw can go one of two ways.
			//	1. There is a valid displaylist, and 'modified' is false.
			//	This is ace, just call the display list. Fast as hell.
			//	2. There is no display list or 'modified' is true.
			//	This is OK, we do the slower version of the drawing while 
			//	compiling a display list at the same time. As long as the object
			//	doesn't get modified again, it'll just call the list next time.
			
			if(displayList != 0 && modified != true)
			{
				gl.CallList(displayList);
				return false;
			}

			//	If we have a list, destroy it. Then create a new one.
			if(displayList != 0)
				gl.DeleteLists(displayList, 1);
			displayList = gl.GenLists(1);

			//	From now on we are compiling the display list...
			gl.NewList(displayList, OpenGL.COMPILE_AND_EXECUTE);

			//	Push the matrix.
			gl.PushMatrix();

			//	Transform the matrix.
			Transform(gl);

			//	Set the material, after saving the current attributes.
			gl.PushAttrib(OpenGL.LIGHTING_BIT);
			material.Set(gl);

			return true;
		}

		/// <summary>
		/// This is always called after drawing, useful to restore the matrix stack etc.
		/// </summary>
		/// <param name="gl"></param>
		protected internal virtual void DoPostDraw(OpenGL gl)
		{
			//	Unset the material.
			gl.PopAttrib();

			//	Pop the matrix.
			gl.PopMatrix();

			//	End the display list (this function only ever gets called
			//	if DoPreDraw returns true).
			gl.EndList();
			modified = false;	//	Back in displaylist country until it changes again.
		}

		/// <summary>
		/// Transform calls the OpenGL transformation functions using the current
		/// values, normally in the order translate, scale, rotate.
		/// </summary>
		public virtual void Transform(OpenGL gl)
		{
			if(transformationOrder == TransformationOrder.TranslateRotateScale)
			{
				gl.Translate(translate.X, translate.Y, translate.Z);
				gl.Rotate(rotate.X, rotate.Y, rotate.Z);
			}
			else
			{
				gl.Rotate(rotate.X, rotate.Y, rotate.Z);
				gl.Translate(translate.X, translate.Y, translate.Z);
			}
			
			gl.Scale(scale.X, scale.Y, scale.Z);
		}

		/// <summary>
		/// This function forces the display list of an object to
		/// be recreated immediately.
		/// </summary>
		public virtual void ForceModifiedUpdate(OpenGL gl)
		{
			modified = true;
			DoPreDraw(gl);
			Draw(gl);
			DoPostDraw(gl);
		}

		/// <summary>
		/// ToString should just the name of the object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return name;
		}

		/// <summary>
		/// This function is complicated to explain, but in simple terms, it allows
		/// an object to subdivide itself into a heirarchy for selection. See derivatives
		/// for examples.
		/// </summary>
		/// <param name="names">A set of integers which are the names for this selection
		/// hit, normally one int, but for hierarchies it's more.</param>
		/// <returns>The object that the name(s) represents (usually just 'this').</returns>
		IInteractable IInteractable.GetObjectFromSelectNames(int[] names)
		{
			return this;
		}

		/// <summary>
		/// This function is called when the mouse is used to edit an object. You can override
		/// it to provide custom functionality.
		/// </summary>
		/// <param name="operation">The current mouse mode.</param>
		/// <param name="x">The amount dragged along X.</param>
		/// <param name="y">The amount dragged along X.</param>
		/// <param name="z">The amount dragged along X.</param>
		void IInteractable.DoMouseInteract(MouseOperation operation, float x, float y, float z)
		{
			switch(operation)
			{
				case MouseOperation.Translate:
					Translate.X += x;
					Translate.Y += y;
					Translate.Z += z;
					break;
				case MouseOperation.Rotate:
					Rotate.X += x;
					Rotate.Y += y;
					Rotate.Z += z;
					break;
				case MouseOperation.Scale:
					Scale.X += x;
					Scale.Y += y;
					Scale.Z += z;
					break;

			}

			//	This modifies the object.
			modified = true;
		}

		void IInteractable.DrawPick(OpenGL gl)
		{
			Draw(gl);
		}

		/// <summary>
		/// This enum describes the orders SharpGL can perform transformations in, when
		/// you call the 'Tranform' function in 'SceneObject'. Generally it is T>R>S.
		/// However, R>T>S lets you do different things, like 'pan' a camera or light.
		/// </summary>
		public enum TransformationOrder
		{
			TranslateRotateScale,
			RotateTranslateScale,
		}

		#region Member Data

		/// <summary>
		/// This represents the position of the object.
		/// </summary>
		protected Vertex translate = new Vertex(0, 0, 0);

		/// <summary>
		/// This represents the size (scale) of the object.
		/// </summary>
		protected Vertex scale = new Vertex(1, 1, 1);

		/// <summary>
		/// This represents rotation about the x, y and z axies respectfully.
		/// </summary>
		protected Vertex rotate = new Vertex(0, 0, 0);

		/// <summary>
		/// This is a descriptive name of the object.
		/// </summary>
		protected string name = "Unnamed Scene Object";

		/// <summary>
		/// The material used to draw the object.
		/// </summary>
		protected Material material = new Material();

		/// <summary>
		/// The order of matrix transformations.
		/// </summary>
		protected TransformationOrder transformationOrder = TransformationOrder.TranslateRotateScale;

		/// <summary>
		/// Any changes to the sceneobject must set 'modified' to true, to make
		/// sure the display list is correctly recreated.
		/// </summary>
		protected bool modified = false;

		/// <summary>
		/// This is the OpenGL displaylist code.
		/// </summary>
		protected uint displayList = 0;

		#endregion

		#region Properties

		[Description("The name of the scene object."), Category("Scene Object")]
		public string Name
		{
			get{return name;}
			set{name = value;}
		}
		[Description("Position of the object."), Category("Scene Object")]
		public Vertex Translate
		{
			get {return translate;}
			set {translate = value; modified = true;}
		}
		[Description("Size of the object."), Category("Scene Object")]
		public Vertex Scale
		{
			get {return scale;}
			set {scale = value; modified = true;}
		}
		[Description("Rotation of the object."), Category("Scene Object")]
		public Vertex Rotate
		{
			get {return rotate;}
			set {rotate = value; modified = true;}
		}
		[Description("The material used to draw the object."), Category("Scene Object")]
		public Material Material
		{
			get {return material;}
			set {material = value; modified = true;}
		}
		[Description("The order of matrix transformations."), Category("Scene Object")]
		public TransformationOrder TransformOrder
		{
			get {return transformationOrder;}
			set {transformationOrder = value; modified = true;}
		}
		[Description("Has the object been modified, and does the displaylist need to be redrawn?"), Category("Scene Object")]
		public bool Modified
		{
			get {return modified;}
			set {modified = value;}
		}
		[Description("The internal display list code."), Category("Scene Object")]
		public uint DisplayList
		{
			get {return displayList;}
		}

		#endregion

		static int nCount = 0;
	}
	

	/// <summary>
	/// Derive from this if you want mouse interaction from an object.
	/// </summary>
	public interface IInteractable
	{
		/// <summary>
		/// Draw this interactable class for picking (i.e with names).
		/// </summary>
		void DrawPick(OpenGL gl);

		/// <summary>
		/// From pick data, return the object (this allows for hierarchies).
		/// </summary>
		/// <param name="names">The select buffer names.</param>
		/// <returns>The object represented by the names.</returns>
		IInteractable GetObjectFromSelectNames(int[] names);

		/// <summary>
		/// This function allows the class to control what happens to it when the mouse
		/// is used to interact with it.
		/// </summary>
		/// <param name="operation">The mouse mode (rotate, scale etc).</param>
		/// <param name="x">The amount along X.</param>
		/// <param name="y">The amount along Y.</param>
		/// <param name="z">The amount along Z.</param>
		void DoMouseInteract(MouseOperation operation, float x, float y, float z);
	}
/*
	public interface IBuildable
	{
		/// <summary>
		/// This function is called when the user clicks on the OpenGL
		/// control. override it to handle clicks when building.
		/// </summary>
		/// <param name="gl">OpenGL.</param>
		/// <param name="e">The mouse parameters.</param>
		/// <returns>True if building is completed.</returns>
		bool OnMouseDown(OpenGL gl, MouseEventArgs e);

		/// <summary>
		/// This function is called when the user is building an object
		/// and moves the mouse.
		/// </summary>
		/// <param name="gl">OpenGL.</param>
		/// <param name="e">Mouse Parameters.</param>
		void OnMouseMove(OpenGL gl, MouseEventArgs e);

		/// <summary>
		/// This function is called when the user clicks on the OpenGL
		/// control. override it to handle clicks when building.
		/// </summary>
		/// <param name="gl">OpenGL.</param>
		/// <param name="e">The mouse parameters.</param>
		/// <returns>True if building is completed.</returns>
		bool OnMouseUp(OpenGL gl, MouseEventArgs e);
	}
*/
/*	/// <summary>
	///	Any class which depends heavily on OpenGL, for creation and destruction,
	///	such as quadrics, NURBS and materials should dervice from this interface,
	///	which helps to simplify the task of creating and destroying the underlying OpenGL
	///	objects. It also adds the functionality to migrate from one instance of OpenGL
	///	to another.
	/// </summary>
	public interface IOpenGLDependant
	{
		/// <summary>
		/// This function creates the underlying OpenGL object.
		/// </summary>
		/// <param name="gl">OpenGL.</param>
		void Create(OpenGL gl);

		/// <summary>
		/// This function destroys the underlying OpenGL object.
		/// </summary>
		/// <param name="gl">OpenGL.</param>
		void Destroy(OpenGL gl);

		/// <summary>
		/// This function copys the object into a new instance of OpenGL.
		/// </summary>
		/// <param name="gl"></param>
		void Migrate(OpenGL gl);
	}*/
}
