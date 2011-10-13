//	Simple3DFormat.cs
//	Created by Dave Kerr, 22/2/2004
//	Copyright (c) Dave Kerr.
//
//	Notes:
//	
//	The Simple3D format is used to save data from my C++ 3D library, in the 
//	library/3D/ folder. It is useful, especially for *.poly files, which are
//	used in other projects. This class provides a link between C++ apps 
//	and C# apps, you can transfer polygons between the two.


using System;
using System.IO;

using SharpGL.SceneGraph;
using SharpGL.Persistence;
using SharpGL.SceneGraph.Collections;

namespace SharpGL.Persistence.Formats.Simple3D
{
	public class Simple3DFormatPOLY : Format
	{
		protected override object LoadData(Stream stream)
		{
			//	We use a binary reader for this data.
			BinaryReader reader = new BinaryReader(stream, System.Text.Encoding.ASCII);
			
			//	Create a polygon.
			Polygon poly = new Polygon();

			//	Read the translate, scale, rotate.
			poly.Translate.X = reader.ReadSingle();
			poly.Translate.Y = reader.ReadSingle();
			poly.Translate.Z = reader.ReadSingle();
			poly.Scale.X = reader.ReadSingle();
			poly.Scale.Y = reader.ReadSingle();
			poly.Scale.Z = reader.ReadSingle();
			poly.Rotate.X = reader.ReadSingle();
			poly.Rotate.Y = reader.ReadSingle();
			poly.Rotate.Z = reader.ReadSingle();

			//	Read number of verticies.
			int verticesCount = reader.ReadInt32();
							
			//	Get them all 
			for(int i=0; i<verticesCount; i++)
			{
				//	Read a vertex.
				Vertex vertex = new Vertex(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

				//	Add it.
				poly.Vertices.Add(vertex);
			}
							
			//	Read UV count.
			int uvsCount = reader.ReadInt32();

			//	Read all of the UVs
			for(int i=0; i<uvsCount; i++)
				poly.UVs.Add(new UV(reader.ReadInt32(), reader.ReadInt32()));
							
			//	Read faces count.
			int faces = reader.ReadInt32();

			//	Read each face.
			for(int f= 0; f<faces; f++)
			{
				Face face = new Face();
				poly.Faces.Add(face);

				//	Read index count
				int indices = reader.ReadInt32();

				//	Now read the indices, a vertex index and a uv index and a color index.
				for(int j=0; j<indices; j++)
				{
					face.Indices.Add(new Index(reader.ReadInt32(), reader.ReadInt32()));
					int colour = reader.ReadInt32(); //not used.
				}
			}
			
			//	Finally, we update normals.
			poly.Validate(true);
				
			return poly;
		}

		protected override bool SaveData(object data, Stream stream)
		{
			//	We use a binary writer to write the data.
			BinaryWriter writer = new BinaryWriter(stream, System.Text.Encoding.ASCII);

			//	Cast the data to a polygon.
			Polygon poly = (Polygon)data;

			//	Write the translate, scale, rotate.
			writer.Write(poly.Translate.X);
			writer.Write(poly.Translate.Y);
			writer.Write(poly.Translate.Z);
			writer.Write(poly.Scale.X);
			writer.Write(poly.Scale.Y);
			writer.Write(poly.Scale.Z);
			writer.Write(poly.Rotate.X);
			writer.Write(poly.Rotate.Y);
			writer.Write(poly.Rotate.Z);

			//	Write the vertex count.
			writer.Write(poly.Vertices.Count);
			foreach(Vertex v in poly.Vertices)
			{
				writer.Write(v.X);
				writer.Write(v.Y);
				writer.Write(v.Z);
			}

			//	Write the uv count.
			writer.Write(poly.UVs.Count);
			foreach(UV uv in poly.UVs)
			{
				writer.Write(uv.u);
				writer.Write(uv.v);
			}

			//	Write face  count.
			writer.Write(poly.Faces.Count);
			foreach(Face face in poly.Faces)
			{
				//	Write the index count.
				writer.Write(face.Indices.Count);
				foreach(Index i in face.Indices)
				{
					writer.Write(i.Vertex);
					writer.Write(i.UV);
					writer.Write((int)0);	//	Color index, not used.
				}
			}

			return true;
		}


		public override string[] FileTypes
		{
			get {return new string[] {"poly"};}
		}

		public override string Filter
		{
			get {return "Simple3D Polygons (*.poly)|*.poly";}
		}

		public override Type[] DataTypes
		{
			get {return new Type[] {typeof(Polygon)};}
		}
	}
}