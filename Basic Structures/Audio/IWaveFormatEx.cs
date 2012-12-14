using System;

namespace Bardez.Projects.Win32.Audio
{
	/// <summary>Interface that declares a method to return a WaveFormatEx reference</summary>
	public interface IWaveFormatEx
    {
		/// <summary>Returns a WaveFormatEx instance from this header data</summary>
		/// <returns>A WaveFormatEx instance to submit to API calls</returns>
		WaveFormatEx GetWaveFormat();
    }
}