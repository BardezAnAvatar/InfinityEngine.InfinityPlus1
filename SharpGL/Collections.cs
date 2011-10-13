//    Collections.cs
//    Created by Dave Kerr, 12/08/2003
//    Copyright (c) Dave Kerr 2003
//    http://www.codechamber.com

//	This page shows something C# needs. templates.


using System;
using System.Collections;
using System.ComponentModel;

namespace SharpGL.SceneGraph
{
	/// <summary>
	/// The collections are a set of strongly typed collection classes, for use in
	/// SharpGL. They are strongly typed to keep the code efficient.
	/// </summary>
	namespace Collections
	{
		[Serializable()]
		public class SceneObjectCollection : CollectionBase
		{
			public SceneObjectCollection(){}

			public virtual void Add(SceneObject value)
			{
				List.Add(value);
			}

			public virtual SceneObject this[int index]
			{
				get {return (SceneObject)List[index];}
				set {List[index] = value;}
			}

			public virtual void Remove(SceneObject value)
			{
				List.Remove(value);
			}
		}

		[Serializable()]
		public class IndexCollection : CollectionBase
		{
			public IndexCollection(){}

			public virtual void Add(Index value)
			{
				List.Add(value);
			}

			public virtual Index this[int index]
			{
				get {return (Index)List[index];}
				set {List[index] = value;}
			}

			public virtual void Remove(Index value)
			{
				List.Remove(value);
			}
		}

		[Serializable()]
		public class FaceCollection : CollectionBase
		{
			public FaceCollection(){}

			public virtual void Add(Face value)
			{
				List.Add(value);
			}

			public virtual Face this[int index]
			{
				get {return (Face)List[index];}
				set {List[index] = value;}
			}

			public virtual void Remove(Face value)
			{
				List.Remove(value);
			}
		}

		[Serializable()]
		public class VertexCollection : CollectionBase
		{
			public VertexCollection(){}

			public virtual int Add(Vertex value)
			{
				return List.Add(value);
			}

			public virtual Vertex this[int index]
			{
				get {return (Vertex)List[index];}
				set {List[index] = value;}
			}

			public virtual void Remove(Vertex value)
			{
				List.Remove(value);
			}

			/// <summary>
			/// This function finds a vertex with a specified degree of accuracy.
			/// </summary>
			/// <param name="firstIndex">The place to start searching.</param>
			/// <param name="vertex">The Vertex to find.</param>
			/// <param name="accuracy">The furthest away a vertex can be, e.g. 0.0001.</param>
			/// <returns></returns>
			public virtual int Find(int firstIndex, Vertex vertex, float accuracy)
			{
				for(int i=firstIndex; i<Count; i++)
				{
					Vertex v = this[i];

					if((v.X > (vertex.X - accuracy) && v.X < (vertex.X + accuracy))
						&& (v.Y > (vertex.Y - accuracy) && v.Y < (vertex.Y + accuracy))
						&& (v.Z > (vertex.Z - accuracy) && v.Z < (vertex.Z + accuracy)))
						return i;
				}
				return -1;
			}

		}

		[Serializable()]
		public class NormalCollection : CollectionBase
		{
			public NormalCollection(){}

			public virtual int Add(Normal value)
			{
				return List.Add(value);
			}

			public virtual Normal this[int index]
			{
				get {return (Normal)List[index];}
				set {List[index] = value;}
			}

			public virtual void Remove(Normal value)
			{
				List.Remove(value);
			}

			/// <summary>
			/// This function finds a normal with a specified degree of accuracy.
			/// </summary>
			/// <param name="firstIndex">The place to start searching.</param>
			/// <param name="normal">The normal to find.</param>
			/// <param name="accuracy">The furthest away a normal can be, e.g. 0.0001.</param>
			/// <returns></returns>
			public virtual int Find(int firstIndex, Normal normal, float accuracy)
			{
				for(int i=firstIndex; i<Count; i++)
				{
					Normal n = this[i];

					if((n.X > (normal.X - accuracy) && n.X < (normal.X + accuracy))
						&& (n.Y > (normal.Y - accuracy) && n.Y < (normal.Y + accuracy))
						&& (n.Z > (normal.Z - accuracy) && n.Z < (normal.Z + accuracy)))
						return i;
				}
				return -1;
			}
		}

		[Serializable()]
		public class UVCollection : CollectionBase
		{
			public UVCollection(){}

			public virtual int Add(UV value)
			{
				return List.Add(value);
			}

			public virtual UV this[int index]
			{
				get {return (UV)List[index];}
				set {List[index] = value;}
			}

			public virtual void Remove(UV value)
			{
				List.Remove(value);
			}
		}

		[Serializable()]
		public class PolygonCollection : CollectionBase
		{
			public PolygonCollection(){}

			public virtual int Add(Polygon value)
			{
				return List.Add(value);
			}

			public virtual Polygon this[int index]
			{
				get {return (Polygon)List[index];}
				set {List[index] = value;}
			}

			public virtual void Remove(Polygon value)
			{
				List.Remove(value);
			}
		}

		[Serializable()]
		public class MetagonCollection : CollectionBase
		{
			public MetagonCollection(){}

			public virtual int Add(Metagon value)
			{
				return List.Add(value);
			}

			public virtual Metagon this[int index]
			{
				get {return (Metagon)List[index];}
				set {List[index] = value;}
			}

			public virtual void Remove(Metagon value)
			{
				List.Remove(value);
			}
		}

		[Editor(typeof(NETDesignSurface.Editors.QuadricCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]  
		[Serializable()]
		public class QuadricCollection : CollectionBase
		{
			public QuadricCollection(){}

			public virtual int Add(Quadrics.Quadric value)
			{
				return List.Add(value);
			}

			public virtual Quadrics.Quadric this[int index]
			{
				get {return (Quadrics.Quadric)List[index];}
				set {List[index] = value;}
			}

			public virtual void Remove(Quadrics.Quadric value)
			{
				List.Remove(value);
			}
		}

		/// <summary>
		/// This is one of the most important collections, in the NETDesignSurface theres
		/// a load of code that makes these collections more functional in the properties
		/// window.
		/// </summary>
		[TypeConverter(typeof(NETDesignSurface.Converters.MaterialCollectionConverter))]
		[Serializable()]
		public class MaterialCollection : CollectionBase
		{
			public MaterialCollection(){}

			public virtual int Add(Material value)
			{
				return List.Add(value);
			}

			public virtual Material this[int index]
			{
				get {return (Material)List[index];}
				set {List[index] = value;}
			}

			public virtual void Remove(Material value)
			{
				List.Remove(value);
			}
		}

		/// <summary>
		/// This collection is for cameras, and has a typeconverter for the different
		/// camera types.
		/// </summary>
		[Editor(typeof(NETDesignSurface.Editors.CameraCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]  
		[Serializable()]
		public class CameraCollection : CollectionBase
		{
			public CameraCollection(){}

			public virtual int Add(Cameras.Camera value)
			{
				return List.Add(value);
			}

			public virtual Cameras.Camera this[int index]
			{
				get {return (Cameras.Camera)List[index];}
				set {List[index] = value;}
			}

			public virtual void Remove(Cameras.Camera value)
			{
				List.Remove(value);
			}
		}

		[Serializable()]
		public class LightCollection : CollectionBase
		{
			public LightCollection(){}

			public virtual int Add(Lights.Light value)
			{
				return List.Add(value);
			}

			public virtual Lights.Light this[int index]
			{
				get {return (Lights.Light)List[index];}
				set {List[index] = value;}
			}

			public virtual void Remove(Lights.Light value)
			{
				List.Remove(value);
			}
		}

		[Editor(typeof(NETDesignSurface.Editors.EvaluatorCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]  
		[Serializable()]
		public class EvaluatorCollection : CollectionBase
		{
			public EvaluatorCollection(){}

			public virtual int Add(Evaluators.Evaluator value)
			{
				return List.Add(value);
			}

			public virtual Evaluators.Evaluator this[int index]
			{
				get {return (Evaluators.Evaluator)List[index];}
				set {List[index] = value;}
			}

			public virtual void Remove(Evaluators.Evaluator value)
			{
				List.Remove(value);
			}
		}

		/// <summary>
		/// This is a strongly typed collection of particles.
		/// </summary>
		[Serializable()]
		public class ParticleCollection : CollectionBase
		{
			public ParticleCollection(){}

			public virtual void Add(ParticleSystems.Particle value)
			{
				List.Add(value);
			}

			public virtual ParticleSystems.Particle this[int index]
			{
				get {return (ParticleSystems.Particle)List[index];}
				set {List[index] = value;}
			}

			public virtual void Remove(ParticleSystems.Particle value)
			{
				List.Remove(value);
			}
		}

		public class InteractableCollection : CollectionBase
		{
			public InteractableCollection(){}

			public virtual void Add(IInteractable value)
			{
				List.Add(value);
			}

			public virtual void AddRange(IList list)
			{
				foreach(IInteractable interact in list)
					List.Add(interact);
			}

			public virtual IInteractable this[int index]
			{
				get {return (IInteractable)List[index];}
				set {List[index] = value;}
			}

			public virtual void Remove(IInteractable value)
			{
				List.Remove(value);
			}
		}
	}
}
