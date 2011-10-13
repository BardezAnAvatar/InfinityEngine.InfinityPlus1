//    OptimisedPolygon.cs
//    Created by Dave Kerr, 15/09/2003
//    Copyright (c) Dave Kerr 2003
//    http://www.codechamber.com

//	SharpGL 1.7 : This file has been depreciated. All Polygons (and all sceneobjects)
//	are now automatiacally optimised into display lists. See SceneObject.cs for 
//	more info.

/*
using System;
using System.Collections;
using System.ComponentModel;

using SharpGL.SceneGraph.Collections;
using SharpGL.SceneGraph.Raytracing;

namespace SharpGL.SceneGraph
{
	/// <summary>
	/// An optimised polygon is one that is much, much faster. However, it must be created
	/// from an ordinary polygon.
	/// </summary>
	[Serializable]
	public class OptimisedPolygon : SceneObject, IInteractable
	{
		/// <summary>
		/// This is the main constructor. Pass it OpenGL and the polygon you want to
		/// make an optimised version of, and the fully optimised polygon will be 
		/// created.
		/// </summary>
		/// <param name="gl">OpenGL.</param>
		/// <param name="polygon">The polygon to optimise.</param>
		public OptimisedPolygon(OpenGL gl, Polygon polygon) 
		{
			//	Draw the polygon in list compile mode.
			displayList = gl.GenLists(1);
			gl.NewList(displayList, OpenGL.COMPILE);

			polygon.Draw(gl);

			//	End the list.
			gl.EndList();

			//	The polygon is safe to dispose now.
		}

		public override void Draw(OpenGL gl)
		{
			gl.CallList(displayList);
		}

		protected uint displayList = 0;
	}
}
*/