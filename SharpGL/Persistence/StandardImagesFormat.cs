//    StandardImagesFormat.cs
//    Created by Dave Kerr, 12/08/2003
//    Copyright (c) Dave Kerr 2003
//    http://www.codechamber.com


using System;
using System.IO;

using SharpGL.SceneGraph;

namespace SharpGL.Persistence.Formats.Standard
{
	/// <summary>
	/// 
	/// </summary>
	public class StandardImagesFormat : SharpGL.Persistence.Format
	{
		public StandardImagesFormat()
		{
		}

		protected override object LoadData(Stream stream)
		{
			//	We use a binary reader to load data.
			BinaryReader reader = new BinaryReader(stream, System.Text.Encoding.ASCII);

			//	Create a new material.
			Material material = new Material();
        
        //  TODO: Fix material.
		//	//	Create a bitmap object.
		//	System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(reader.BaseStream);
		//	material.Texture.LoadPixelData(bitmap);

			return material;
		}

		protected override bool SaveData(object data, Stream stream)
		{
			return false;
		}

		public override string[] FileTypes
		{
			get {return new string[] {"bmp", "jpg", "gif", "png", "tif"};}
		}

		public override string Filter
		{
			get {return "Image Files (*.bmp, *.jpg, *.gif, *.png, *.tif)|*.bmp;*.jpg;*.tga;*.png;*.tif";}
		}

		public override Type[] DataTypes
		{
			get {return new Type[] {typeof(Material)};}
		}
	}
}
