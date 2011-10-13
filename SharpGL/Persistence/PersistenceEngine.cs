//    PersistenceEngine.cs
//    Created by Dave Kerr, 12/08/2003
//    Copyright (c) Dave Kerr 2003
//    http://www.codechamber.com


using System;
using System.IO;
using System.Windows.Forms;

using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Collections;

namespace SharpGL.Persistence
{
	public class PersistenceEngine
	{
		public PersistenceEngine()
		{
			formats = new Format[] {new Formats.Caligari.CaligariFormatSCN(),
									new Formats.Caligari.CaligariFormatCOB(),
									new Formats.Discreet.MAXFormat(),
									new Formats.Standard.StandardImagesFormat(),
									new Formats.SGF.SGSFormat(),
									new Formats.SGF.SGPFormat(),
									new Formats.Simple3D.Simple3DFormatPOLY(),};
		}

		/// <summary>
		/// This function shows the power of the persistence engine. Pass it any type,
		/// material, scene, whatever, and it will display a file open dialog with
		/// the allowed formats and automatically load it.
		/// </summary>
		/// <param name="dataType">The type of data you want to load.</param>
		/// <returns>The loaded object.</returns>
		public virtual object UserLoad(Type dataType)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = FormatString(dataType);
			
			if(dlg.ShowDialog() == DialogResult.OK)
				return Load(dlg.FileName);

			return null;
		}

		/// <summary>
		/// This function shows the power of the persistence engine. Pass it any data,
		/// material, scene, whatever, and it will display a file save dialog with
		/// the allowed formats and automatically save it.
		/// </summary>
		/// <param name="data">The data you want to save.</param>
		/// <returns>True if successful.</returns>
		public virtual bool UserSave(object data)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.Filter = FormatString(data.GetType());
			
			if(dlg.ShowDialog() == DialogResult.OK)
				return Save(data, dlg.FileName);

			return false;
		}

		/// <summary>
		/// This function returns a format string, suitable for a file Open/Save
		/// dialog for the specified type.
		/// </summary>
		/// <param name="type">The type to format for.</param>
		/// <returns>The format string.</returns>
		public virtual string FormatString(Type type)
		{
			//	Create the string.
			string formatString = "";

			//	Go through every format.
			foreach(Format format in formats)
			{
				//	If this format is the correct type, add it to the string.
				foreach(Type dataType in format.DataTypes)
				{
					if(dataType == type)
					{
						formatString += "|" + format.Filter;
						break;
					}
				}
			}
			
			//	Return the string, minus the first '|'.
			if(formatString.Length == 0)
				return "";
			else
				return formatString.Substring(1, formatString.Length - 1);
		}

		public virtual object Load(string file)
		{
			//	Here we go through each of the Formats available to us,
			//	and use the one that's supported.

			foreach(Format format in formats)
			{
				if(format.IsValidFilename(file))
				{
					//	Load with this format.
					return format.Load(file);
				}
			}

			return null;
		}

		public virtual bool Save(object data, string file)
		{
			//	Here we go through each of the Formats available to us,
			//	and use the one that's supported.

			foreach(Format format in formats)
			{
				if(format.IsValidFilename(file))
				{
					//	Load with this format.
					return format.Save(data, file);
				}
			}

			return false;
		}

		protected Format[] formats;
	}
}