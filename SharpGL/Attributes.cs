//    Attributes.cs
//    Created by Dave Kerr, 12/08/2003
//    Copyright (c) Dave Kerr 2003
//    http://www.codechamber.com

//	Currently I think the attribute code is a little bit kludgy, but it's one
//	of the hardest parts of OpenGL to wrap in an object-orientated way. This is
//	the current implementation of attributes, but it is likely to be revised soon.

using System;
using System.Drawing;
using System.ComponentModel;

using SharpGL.SceneGraph;

namespace SharpGL.SceneGraph.Attributes
{
	[Serializable()]
	public abstract class AttributeGroup
	{
		public AttributeGroup()
		{
		}

		~AttributeGroup()
		{
			//	If the group has been set, but then get's destroyed without
			//	being restored (i.e. a push without a corresponding pop)
			//	we output info.
			if(pushed)
				System.Diagnostics.Debugger.Break();
		}

		/// <summary>
		/// The AttributeBit is the OpenGL value that represents the
		/// attribute, e.g FOG_BIT
		/// </summary>
		public abstract uint AttributeBit
		{
			get;
		}

		/// <summary>
		/// Call this function to set the current values in the Group into OpenGL.
		/// It will automatically save the previous values for you. (So don't call
		/// gl.PushAttrib first!).
		/// </summary>
		public virtual void Set(OpenGL gl)
		{
			//	We're gonna push the group, so set 'push'.
			pushed = true;
		}

		/// <summary>
		/// Call this function to restore the previous values for this attribute
		/// group. You must only call it AFTER calling 'Set'.
		/// </summary>
		/// <param name="gl"></param>
		public virtual void Restore(OpenGL gl)
		{
			gl.PopAttrib();

			//	The group is being popped.
			pushed = false;
		}

		#region Member Data

		/// <summary>
		/// This variable keeps an eye on if the group is pushed or not, to
		/// guard againt stack overflows (when groups are not popped).
		/// </summary>
		private bool pushed = false;

		#endregion
	}

	/// <summary>
	/// This class has all the settings you can edit for fog.
	/// </summary>
	[TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	[Serializable()]
	public class Fog : AttributeGroup
	{ 
		public override uint AttributeBit
		{
			get {return OpenGL.FOG_BIT;}
		}

		public enum FogMode : uint
		{
			Linear = OpenGL.LINEAR,
			Exponent = OpenGL.EXP,
			ExponentSquared = OpenGL.EXP2,
		}

		public override void Set(OpenGL gl)
		{
			base.Set(gl);

			gl.PushAttrib(AttributeBit);

			if(enable)
			{
				gl.Enable(OpenGL.FOG);
				gl.Fog(OpenGL.FOG_MODE, (int)mode);
				gl.Fog(OpenGL.FOG_COLOR, color);
				gl.Fog(OpenGL.FOG_DENSITY, density);
				gl.Fog(OpenGL.FOG_START, start);
				gl.Fog(OpenGL.FOG_END, end);
			}
			else
				gl.Disable(OpenGL.FOG);
		}

		protected bool enable = false;
		protected FogMode mode = FogMode.Linear;
		protected GLColor color = new GLColor(0.2f, 0.6f, 0.2f, 1.0f);
		protected float density = 0.35f;
		protected float start = 1.0f;
		protected float end = 30.0f;

		[Description("Use OpenGL Fog."), Category("Fog")]
		public bool Enable
		{
			get {return enable;}
			set {enable = value;}
		}
		[Description("Fog mode (how the fog density is calculated)."), Category("Fog")]
		public FogMode Mode
		{
			get {return mode;}
			set {mode = value;}
		}
		[Description("Fog color."), Category("Fog")]
		public Color Color
		{
			get {return color;}
			set {color = value;}
		}
		[Description("Fog density (how thick the fog is)."), Category("Fog")]
		public float Density
		{
			get {return density;}
			set {density = value;}
		}
		[Description("How close to the camera the fog starts."), Category("Fog")]
		public float Start
		{
			get {return start;}
			set {start = value;}
		}
		[Description("How far away from the camera you get complete fog."), Category("Fog")]
		public float End
		{
			get {return end;}
			set {end = value;}
		}
	}

	/// <summary>
	/// This class has the light settings.
	/// </summary>
	[TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	[Serializable()]
	public class Lighting : AttributeGroup
	{
		public override uint AttributeBit
		{
			get {return OpenGL.LIGHTING_BIT;}
		}
	
		public override void Set(OpenGL gl)
		{
			base.Set(gl);

			gl.PushAttrib(AttributeBit);

			if(enable)
			{
				gl.Enable(OpenGL.LIGHTING);
				gl.LightModel(OpenGL.LIGHT_MODEL_AMBIENT, ambientLight);
				gl.LightModel(OpenGL.LIGHT_MODEL_LOCAL_VIEWER, localViewer == true ? 1 : 0);
				gl.LightModel(OpenGL.LIGHT_MODEL_TWO_SIDE, twoSided == true ? 1 : 0);
			}
			else
				gl.Disable(OpenGL.LIGHTING);
		}

		protected GLColor ambientLight = new GLColor(0.2f, 0.2f, 0.2f, 1);
		protected bool localViewer = false;
		protected bool twoSided = false;
		protected bool enable = true;
		
		[Description("The ambient light for the entire scene."), Category("Lighting")]
		public System.Drawing.Color AmbientLight
		{
			get {return ambientLight.ColorNET;}
			set {ambientLight.ColorNET = value;}
		}
		[Description("Does the scene get light depending on camera position?"), Category("Lighting")]
		public bool LocalViewer
		{
			get {return localViewer;}
			set {localViewer = value;}
		}
		[Description("Are both sides of a polygon lit?"), Category("Lighting")]
		public bool TwoSided
		{
			get {return twoSided;}
			set {twoSided = value;}
		}
		[Description("Is lighting enabled in the scene?"), Category("Lighting")]
		public bool Enable
		{
			get {return enable;}
			set {enable = value;}
		}
	}
		
	/// <summary>
	/// The line attributes.
	/// </summary>
	[TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	[Serializable()]
	public class Line : AttributeGroup
	{
		public override uint AttributeBit
		{
			get {return OpenGL.LINE_BIT;}
		}

		public override void Set(OpenGL gl)
		{
			base.Set(gl);

			//	Save the current attributes.
			gl.PushAttrib(AttributeBit);

			gl.LineWidth(width);
			gl.EnableIf(OpenGL.LINE_SMOOTH, smooth);
		}

		protected float width = 1.0f;
		protected bool smooth = true;

						#region Properties

		public float Width
		{
			get {return width;}
			set {width = value;}
		}
		public bool Smooth
		{
			get {return smooth;}
			set {smooth = value;}
		}

						#endregion
	}

	/// <summary>
	/// The point settings.
	/// </summary>
	[TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	[Serializable()]
	public class Point : AttributeGroup
	{
		public override uint AttributeBit
		{
			get {return OpenGL.POINT_BIT;}
		}

		public override void Set(OpenGL gl)
		{
			base.Set(gl);

			gl.PushAttrib(AttributeBit);
			gl.PointSize(size);
			gl.EnableIf(OpenGL.POINT_SMOOTH, smooth);
		}

		protected float size = 1.0f;
		protected bool smooth = true;

						#region Properties

		public float Size
		{
			get {return size;}
			set {size = value;}
		}
		public bool Smooth
		{
			get {return smooth;}
			set {smooth = value;}
		}

						#endregion
	}

		
	/// <summary>
	/// The polygon settings.
	/// </summary>
	[TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	[Serializable()]
	public class Polygon : AttributeGroup
	{
		public override uint AttributeBit
		{
			get {return OpenGL.POLYGON_BIT;}
		}

		public enum CullFaceMode : uint
		{
			Front = OpenGL.FRONT,
			FrontAndBack = OpenGL.FRONT_AND_BACK,
			Back = OpenGL.BACK,
		}

		public enum FrontFaceMode : uint
		{
			ClockWise = OpenGL.CW,
			CounterClockWise = OpenGL.CCW,
		}

		public enum PolygonMode : uint
		{
			Points = OpenGL.POINT,
			Lines = OpenGL.LINE,
			Filled = OpenGL.FILL,
		}

		public override void Set(OpenGL gl)
		{
			base.Set(gl);

			gl.PushAttrib(AttributeBit);

			gl.EnableIf(OpenGL.CULL_FACE, enableCullFace);
			gl.EnableIf(OpenGL.POLYGON_SMOOTH, enableSmooth);
			//	gl.glCullFace((uint)cullFaceMode);
			//	gl.glFrontFace((uint)frontFaceMode);
			gl.PolygonMode(OpenGL.FRONT_AND_BACK, (uint)polygonMode);
			//	gl.glPolygonOffset(offsetFactor, offsetBias);
			gl.EnableIf(OpenGL.POLYGON_OFFSET_POINT, enableOffsetPoint);
			gl.EnableIf(OpenGL.POLYGON_OFFSET_LINE, enableOffsetLine);
			gl.EnableIf(OpenGL.POLYGON_OFFSET_FILL, enableOffsetFill);
		}

		protected bool enableCullFace = false;
		protected bool enableSmooth = false;
		protected CullFaceMode cullFaceMode = CullFaceMode.Back;
		protected FrontFaceMode frontFaceMode = FrontFaceMode.CounterClockWise;
		protected PolygonMode polygonMode = PolygonMode.Filled;
		protected float offsetFactor = 0.0f;
		protected float offsetBias = 0.0f;
		protected bool enableOffsetPoint = false;
		protected bool enableOffsetLine = false;
		protected bool enableOffsetFill = false;
	
		#region Properties

		public bool EnableCullFace
		{
			get {return enableCullFace;} set{enableCullFace = value;}
		}
		public bool EnableSmooth
		{
			get {return enableSmooth;} set{enableSmooth = value;}
		}
		public CullFaceMode CullFaces
		{
			get {return cullFaceMode;} set{cullFaceMode = value;}
		}
		public FrontFaceMode FrontFaces
		{
			get {return frontFaceMode;} set{frontFaceMode = value;}
		}
		public PolygonMode PolygonDrawMode
		{
			get {return polygonMode;} set{polygonMode = value;}
		}
		public float OffsetFactor
		{
			get {return offsetFactor;} set{offsetFactor = value;}
		}
		public float OffsetBias
		{
			get {return offsetBias;} set{offsetBias = value;}
		}
		public bool EnableOffsetPoint
		{
			get {return enableOffsetPoint;} set{enableOffsetPoint = value;}
		}
		public bool EnableOffsetLine
		{
			get {return enableOffsetLine;} set{enableOffsetLine = value;}
		}
		public bool EnableOffsetFill
		{
			get {return enableOffsetFill;} set{enableOffsetFill = value;}
		}

					#endregion
	}
}
