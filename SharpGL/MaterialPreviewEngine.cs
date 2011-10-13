//    MaterialPreviewEngine.cs
//    Created by Dave Kerr, 08/09/2003
//    Copyright (c) Dave Kerr 2003
//    http://www.codechamber.com

//	This class is a good idea, but it fails in one respect. You cannot as of yet
//	create an instance of OpenGL based on an memory bitmap, it must be a window.
//	This will be revised as soon as possible, allowing scenes to be created that 
//	render directly to memory, i.e for previews of materials.

using System;
using System.Drawing;
using System.Collections;

using SharpGL.SceneGraph.Collections;

namespace SharpGL.SceneGraph.NET
{
	/// <summary>
	/// This class can be used to create prviews of the materials in a scene. The preview
	/// is a small bitmap render of a sphere with the material on it. The sphere is the
	/// default object, any object can be used. Some of the operations in this class
	/// are computationally intensive, and those functions are used as infrequently 
	/// as possible.
	/// </summary>
	public class MaterialPreviewEngine
	{
		/// <summary>
		/// This function constructs the preview engine with the default preview size,
		/// 64 by 64 pixels.
		/// </summary>
		public MaterialPreviewEngine()
		{
			//	Create the scene and the drawing objects.
			Create();
		}

		/// <summary>
		/// This function constucts the preview engine with a specific with and height,
		/// this is useful if you need particularly large previews.
		/// </summary>
		/// <param name="previewWidth">The width of the preview images.</param>
		/// <param name="previewHeight">The height of the preview images.</param>
		public MaterialPreviewEngine(int previewWidth, int previewHeight)
		{
			//	Set the width and height.
			this.previewWidth = previewWidth;
			this.previewHeight = previewHeight;

			//	Create the scene and the underlying graphics objects.
			Create();
		}

		/// <summary>
		/// This function creates the internal scene and graphics object.
		/// </summary>
		/// <returns></returns>
		protected bool Create()
		{
			//	Create a scene from the offscreen graphics.
			previewScene.Initialise(previewWidth, previewHeight, SceneType.HighQuality);

			//	Create a sphere.
			Quadrics.Sphere sphere = new Quadrics.Sphere();
			sphere.TextureCoords = true;

			//	Set it as the preview object. Do this via the property, because it
			//	automatically updates the scene.
			PreviewObject = sphere;

			return true;
		}

		/// <summary>
		/// This function generates a material preview for the materials, or updates
		/// the existing ones.
		/// </summary>
		/// <param name="materials">The materials to generate.</param>
		public virtual void GeneratePreview(MaterialCollection materials)
		{
			//	Generate each preview.
			foreach(Material material in materials)
				GeneratePreview(material);
		}

		/// <summary>
		/// This function generates a material preview for the material, or updates
		/// the existing one.
		/// </summary>
		/// <param name="material">The material to generate.</param>
		public virtual void GeneratePreview(Material material)
		{
			//	Check the validity of the preview object.
			if(previewObject == null) 
				return;

			//	Set the material of the preview object.
			previewObject.Material = material;

			//	Draw the scene.
			previewScene.Draw();

			//	Get a preview image.
			Bitmap preview = new Bitmap(offscreenBitmap);

			//	Search for the preview item, and update it.
			foreach(PreviewItem item in previews)
			{
				if(item.Material == material)
				{
					item.Preview = preview;
					return;
				}
			}

			//	At this point, the item doesn't exist, so add it.
			previews.Add(new PreviewItem(material, preview));
		}

		/// <summary>
		/// This function gets the preview bitmap of the material. If it doesn't exist,
		/// it generates one. This means that the first time you call this function, it 
		/// will be slow (unless you previously generated all of the materials) but from
		/// then on it will be extremely fast.
		/// </summary>
		/// <param name="material">The material to get a preview of.</param>
		/// <returns>The preview.</returns>
		public virtual Bitmap GetPreview(Material material)
		{
			//	Check the generated previews.
			foreach(PreviewItem item in previews)
			{
				if(item.Material == material)
					return item.Preview;
			}

			//	Generate the preview.
			GeneratePreview(material);

			//	Return the newly generated bitmap.
			return previews[previews.Count - 1].Preview;
		}
        
		/// <summary>
		/// This is the scene used to preview the material.
		/// </summary>
		protected Scene previewScene = new Scene();

		/// <summary>
		/// This is the width of the preview images.
		/// </summary>
		protected int previewWidth = 64;

		/// <summary>
		/// This is the height of the preview images.
		/// </summary>
		protected int previewHeight = 64;

		/// <summary>
		/// This is the bitmap all the rendering is done to.
		/// </summary>
		protected Bitmap offscreenBitmap = null;

		/// <summary>
		/// This the graphics, based on the internal bitmap, which the scene draws with.
		/// </summary>
		protected Graphics offscreenGraphics = null;

		/// <summary>
		/// This is the object used to preview the Materials.
		/// </summary>
		protected SceneObject previewObject = null;

		/// <summary>
		/// This is the collection of material preview pairs.
		/// </summary>
		internal PreviewCollection previews = new PreviewCollection();

		public SceneObject PreviewObject
		{
			get {return previewObject;}
			set
			{
				//	Unjam the existing object.
				if(previewObject != null)
					previewScene.UnJam(previewObject);

				//	Jam the new object.
				previewScene.Jam(value);

				previewObject = value;
			}
		}
	}

	internal class PreviewItem
	{
		public PreviewItem(Material material, Bitmap preview)
		{
			this.material = material;
			this.preview = preview;
		}

		protected Material material = null;
		protected Bitmap preview = null;

		public Material Material
		{
			get {return material;}
			set {material = value;}
		}
		public Bitmap Preview
		{
			get {return preview;}
			set {preview = value;}
		}
	}

	internal class PreviewCollection : CollectionBase
	{
		public PreviewCollection(){}

		public virtual int Add(PreviewItem value)
		{
			return List.Add(value);
		}

		public virtual PreviewItem this[int index]
		{
			get {return (PreviewItem)List[index];}
			set {List[index] = value;}
		}

		public virtual void Remove(PreviewItem value)
		{
			List.Remove(value);
		}
	}
}
