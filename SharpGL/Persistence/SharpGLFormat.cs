//    SharpGLFormat.cs
//    Created by Dave Kerr, 12/08/2003
//    Copyright (c) Dave Kerr 2003
//    http://www.codechamber.com


using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using SharpGL.SceneGraph;
using SharpGL.Persistence;

namespace SharpGL.Persistence.Formats.SGF
{
	/// <summary>
	/// All SharpGL files, such as *.sgs and *.sgp persist in the same way, so the 
	/// loading and saving code is in this class, and all the other file types derive
	/// from it, just overriding the properties. Clever.
	/// </summary>
	public abstract class SharpGLFormatBase : Format
	{
		protected override object LoadData(Stream stream)
		{
			//	We use a binary formatter to load the data.
			IFormatter formatter = new BinaryFormatter();
			return formatter.Deserialize(stream);
		}

		protected override bool SaveData(object data, Stream stream)
		{
			//	We use a binary formatter to save the data.
			IFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, data);
	
			return true;
		}
	}

	/// <summary>
	/// The SGS format is for SGS files, i.e. SharpGL scenes.
	/// </summary>
	public class SGSFormat : SharpGLFormatBase
	{
		public override string[] FileTypes
		{
			get {return new string[] {"sgs"};}
		}

		public override string Filter
		{
			get {return "SharpGL Scenes (*.sgs)|*.sgs;}";}
		}

		public override Type[] DataTypes
		{
			get {return new Type[] {typeof(Scene)};}
		}
	}

	/// <summary>
	/// The SGP format is for SGP files, i.e. SharpGL Polygons.
	/// </summary>
	public class SGPFormat : SharpGLFormatBase
	{
		public override string[] FileTypes
		{
			get {return new string[] {"sgp"};}
		}

		public override string Filter
		{
			get {return "SharpGL Polygons (*.sgp)|*.sgp;}";}
		}

		public override Type[] DataTypes
		{
			get {return new Type[] {typeof(Polygon)};}
		}
	}

	/// <summary>
	/// The SGM format is for Materials.
	/// </summary>
	public class SGMFormat : SharpGLFormatBase
	{
		public override string[] FileTypes
		{
			get {return new string[] {"sgm"};}
		}

		public override string Filter
		{
			get {return "SharpGL Materials (*.sgm)|*.sgm;}";}
		}

		public override Type[] DataTypes
		{
			get {return new Type[] {typeof(Material)};}
		}
	}

	/// <summary>
	/// The SGML format is for Material Libraries.
	/// </summary>
	public class SGMLFormat : SharpGLFormatBase
	{
		public override string[] FileTypes
		{
			get {return new string[] {"sgml"};}
		}

		public override string Filter
		{
			get {return "SharpGL Material Libraries (*.sgml)|*.sgml;}";}
		}

		public override Type[] DataTypes
		{
			get {return new Type[] {typeof(SharpGL.SceneGraph.Collections.MaterialCollection)};}
		}
	}
}