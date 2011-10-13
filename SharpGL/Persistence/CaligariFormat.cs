//    CaligariFormat.cs
//    Created by Dave Kerr, 12/08/2003
//    Copyright (c) Dave Kerr 2003
//    http://www.codechamber.com


using System;
using System.IO;

using SharpGL.SceneGraph;
using SharpGL.Persistence;
using SharpGL.SceneGraph.Collections;

namespace SharpGL.Persistence.Formats.Caligari
{
	public class CaligariFormatSCN : Format
	{

		protected override object LoadData(Stream stream)
		{
			//	We use a binary reader for this data.
			BinaryReader reader = new BinaryReader(stream, System.Text.Encoding.ASCII);
			
			//	Create a scene.
			Scene scene = new Scene();

			//	First, read the header.
			CaligariFileHeader header = new CaligariFileHeader();
			header.Read(reader);

			//	Here we can make sure that it really is a Caligari File.
			if(header.id != "Caligari " || header.dataMode != 'B')
			{
				System.Diagnostics.Debugger.Log(1, "File I/O", "File is not internally compatible.\n");
				return false;
			}

			//	Now we go through the file, peeping at chunks.
			while(true)
			{
				//	Peep at the next chunk.
				string type = Peep(reader);
			
				//	Check for every type of chunk.
				if(type == "PolH")
				{
					//	Read a polygon into the scene.
					PolygonChunk polyChunk = new PolygonChunk();
					scene.Polygons.Add((Polygon)polyChunk.Read(reader));
				}
				else if(type == "END ")
				{
					//	It's the end of the file, so we may as well break.
					break;
				}
				else
				{
					//	Well we don't know what type it is, so just read the generic chunk.
					CaligariChunk chunk = new CaligariChunk();
					chunk.Read(reader);
				}

			}

			return scene;
		}

		protected override bool SaveData(object data, Stream stream)
		{
			//	We use a binary writer to write the data.
			BinaryWriter writer = new BinaryWriter(stream, System.Text.Encoding.ASCII);

			//	Cast the data to a scene.
			Scene scene = (Scene)data;

			//	Write a file header.
			CaligariFileHeader header = new CaligariFileHeader();
			header.Write(writer);

			//	I haven't yet written the code to write trueSpace stuff.
			return false;
		}

		protected virtual string Peep(BinaryReader reader)
		{
			//	Get the current position.
			long pos = reader.BaseStream.Position;

			//	Read a header.
			CaligariChunkHeader header = new CaligariChunkHeader();
			header.Read(reader);
			
			//	Move the file to it's original position.
			reader.BaseStream.Position = pos;

			//	Return the type.
			return header.chunkType;
		}

		public override string[] FileTypes
		{
			get {return new string[] {"scn"};}
		}

		public override string Filter
		{
			get {return "Caligari trueSpace Scenes (*.scn)|*.scn";}
		}

		public override Type[] DataTypes
		{
			get {return new Type[] {typeof(Scene)};}
		}
	}

	public class CaligariFormatCOB : CaligariFormatSCN
	{
		protected override object LoadData(Stream stream)
		{
			//	A COB file is layed out exactly the same way as a SCN file, this
			//	means we just need to load the scene, and return the object.
			
			//	Try to load the scene.
			object data = base.LoadData(stream);

			//	If it loaded, cast it to scene.
			if(data != null)
			{
				Scene scene = (Scene)data;

				//	Return the polygon.
				if(scene.Polygons.Count != 0)
					return scene.Polygons;
			}

			//	Return null, we failed to load.
			return null;
		}

		public override string[] FileTypes
		{
			get {return new string[] {"cob"};}
		}

		public override string Filter
		{
			get {return "Caligari trueSpace Objects (*.cob)|*.cob";}
		}

		public override Type[] DataTypes
		{
			get {return new Type[] {typeof(PolygonCollection)};}
		}
	}


	internal class CaligariFileHeader
	{
		public virtual void Read(BinaryReader stream)
		{
			//	Read the data.
			id = new string(stream.ReadChars(9));
			version = new string(stream.ReadChars(6));
			dataMode = stream.ReadChar();
			bitFormat = new string(stream.ReadChars(2));
                    
			//	Skip past the pad and newline, 14 chars.
			stream.ReadChars(14);
		}

		public virtual void Write(BinaryWriter writer)
		{
			//	Write the data.
			writer.Write(id.ToCharArray(), 0, id.Length);
			writer.Write(version.ToCharArray(), 0, version.Length);
			writer.Write(dataMode);
			writer.Write(bitFormat.ToCharArray(), 0, bitFormat.Length);
                    
			//	Skip past the pad and newline, 14 chars.
			writer.Write(new byte[14], 0, 14);
		}

		public string id = "Caligari ";
		public string version = "V04.00";
		public char dataMode = 'B';		//	'A' = ASCII, 'B' = Binary
		public string bitFormat = "HL";	//	'LH' = Little Endian, 'HL' = High Endian
	}

	internal class CaligariChunkHeader
	{
		public virtual void Read(BinaryReader stream)
		{
			//	Read the data.
			chunkType = new string(stream.ReadChars(4));
			majorVersion = stream.ReadInt16();
			minorVersion = stream.ReadInt16();
			chunkID = stream.ReadInt32();
			parentID = stream.ReadInt32();
			dataBytes = stream.ReadInt32();
		}

		public string chunkType;
		public short majorVersion;
		public short minorVersion;
		public long chunkID;
		public long parentID;
		public long dataBytes;
	}

	internal class CaligariChunk
	{
		/// <summary>
		/// This is the one chunk that reads no scene object, use it to skip
		/// past unknown chunks.
		/// </summary>
		/// <param name="reader">The Reader to read from.</param>
		/// <returns>The object that has been read.</returns>
		public virtual object Read(BinaryReader reader)
		{
			//	Read the header.
			header.Read(reader);

			//	Return the data.
			return ReadData(reader);
		}

		/// <summary>
		/// This function writes an object to the stream.
		/// </summary>
		/// <param name="writer">The writer to write to.</param>
		/// <param name="sceneObject">The object to write.</param>
		public virtual void Write(BinaryWriter writer, SceneObject sceneObject)
		{
			
		}

		/// <summary>
		/// This function reads the chunk header.
		/// </summary>
		/// <param name="reader">The Reader to read from.</param>
		protected virtual object ReadData(BinaryReader reader)
		{
			//	Skip the data, return nothing.
			data = reader.ReadBytes((int)header.dataBytes);
			return null;
		}

		protected virtual void WriteData(BinaryWriter writer, SceneObject sceneObject)
		{

		}

		public CaligariChunkHeader header = new CaligariChunkHeader();
		public byte[] data;
	}

	internal class PolygonChunk : CaligariChunk
	{
		/// <summary>
		/// This function reads a polygon.
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		protected override object ReadData(BinaryReader reader)
		{
			//	Create a polygon.
			Polygon poly = new Polygon();

			//	Read a name chunk.
			CaligariName name = new CaligariName();
			name.Read(reader);
			poly.Name = name.name;
						
			//	Read the local axies.
			CaligariAxies axies = new CaligariAxies();
			axies.Read(reader);
			poly.Translate = axies.centre;
		//	poly.Rotate = axies.rotate;
					
			//	Read the position matrix.
			CaligariPosition pos = new CaligariPosition();
			pos.Read(reader);

			//	Read number of verticies.
			int verticesCount = reader.ReadInt32();
							
			//	Get them all 
			for(int i=0; i<verticesCount; i++)
			{
				//	Read a vertex.
				Vertex vertex = new Vertex(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

				//	Multiply it by the position matrix.
				vertex = vertex * pos.matrix;

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

				//	Read face type flags.
				byte flags = reader.ReadByte();
							
				//	Read vertex count
				short verticesInFace = reader.ReadInt16();

				//	Do we read a material number?
				if((flags&0x08) == 0)	//	is it a 'hole' face?
					reader.ReadInt16();

				//	Now read the indices, a vertex index and a uv index.
				for(short j=0; j<verticesInFace; j++)
					face.Indices.Add(new Index(reader.ReadInt32(), reader.ReadInt32()));
			}

			//	Any extra stuff?
			if(header.minorVersion > 4)
			{
				//	read flags.
				reader.ReadChars(4);

				if((header.minorVersion > 5) && (header.minorVersion < 8))
					reader.ReadChars(2);
			}

			//	Now that we've loaded the polygon, we triangulate it.
			poly.Triangulate();

			//	Finally, we update normals.
			poly.Validate(true);

			return poly;
		}
	}

	internal class CaligariString
	{
		public virtual void Read(BinaryReader stream)
		{
			short length = stream.ReadInt16();
			name = new string(stream.ReadChars(length));
		}

		public string name;
	}

	internal class CaligariName
	{
		public virtual void Read(BinaryReader stream)
		{
			dupecount = stream.ReadInt16();
			CaligariString str = new CaligariString();
			str.Read(stream);
			name = str.name;
		
		}
				
		public short dupecount;
		public string name;
	}

	internal class CaligariAxies
	{
		public virtual void Read(BinaryReader stream)
		{
			centre = new Vertex(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());
			directionX = new Vertex(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());
			directionY = new Vertex(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());
			directionZ = new Vertex(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());

			//	The X axis in SharpGL is always (1, 0, 0).

			xAxis = new Vertex(1, 0, 0);
			float angleX = xAxis.ScalarProduct(directionX);
			yAxis = new Vertex(0, 1, 0);
			float angleY = yAxis.ScalarProduct(directionY);
			zAxis = new Vertex(0, 0, 1);
			float angleZ = zAxis.ScalarProduct(directionZ);
			angleX = (float)System.Math.Asin(angleX);
			angleY = (float)System.Math.Asin(angleY);
			angleZ = (float)System.Math.Asin(angleZ);

			angleX = (180 * angleX) / (float)Math.PI;
			angleY = (180 * angleY) / (float)Math.PI;
			angleZ = (180 * angleZ) / (float)Math.PI;

			rotate = new Vertex(-angleX, -angleY, -angleZ);

			xAxis = xAxisGL = directionX;
			yAxis = zAxisGL = directionY;
			zAxis = yAxisGL = directionZ;
			xAxisGL.X = -xAxisGL.X;

		}
		public Vertex centre;
		public Vertex rotate;

		public Vertex directionX;
		public Vertex directionY;
		public Vertex directionZ;

		public Vertex xAxis;
		public Vertex yAxis;
		public Vertex zAxis;
		public Vertex xAxisGL;
		public Vertex yAxisGL;
		public Vertex zAxisGL;
	}

	internal class CaligariPosition
	{
		public virtual void Read(BinaryReader stream)
		{
			row1 = new float[] {stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle()};
			row2 = new float[] {stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle()};
			row3 = new float[] {stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle()};
		
			//	Convert it to our own matrix.
			matrix.data[0] = row1;
			matrix.data[1] = row2;
			matrix.data[2] = row3;

			matrix.Transpose();
		}

		public float[] row1;
		public float[] row2;
		public float[] row3;

		
		public Matrix matrix = new Matrix();
	}
			
	
}