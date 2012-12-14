using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Bardez.Projects.Win32.Audio
{				
    /// <summary>Managed representation of Win32 WAVEFORMATEX structure</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WaveFormatEx
    {
        #region Fields
		/// <summary>Integer identifier of the format</summary>
        public UInt16 FormatTag;

		/// <summary>Number of audio channels</summary>
        public UInt16 NumberChannels;

		/// <summary>Audio sample rate</summary>
        public UInt32 SamplesPerSec;

		/// <summary>Bytes per second (possibly approximate)</summary>
        public UInt32 AverageBytesPerSec;

		/// <summary>Size in bytes of a sample block (all channels)</summary>
        public UInt16 BlockAlignment;

		/// <summary>Size in bits of a single per-channel sample</summary>
		/// <remarks>16 bit is optimal for XAudio2, 32 bit following, then other formats.</remarks>
        public UInt16 BitsPerSample;

        /// <summary>
        ///		Bytes of extra data appended to this struct. Explains the length of bytes in addition to this struct,
        ///		essentially the extra size of child instances.
        /// </summary>
        public UInt16 Size;
        #endregion


        #region ToString override
		/// <summary>Generates a descriptive string (to be displayed to the end user)</summary>
		/// <returns>A String representing the WaveFormatEx Object contents</returns>
        public override String ToString()
        {
	        StringBuilder builder = new StringBuilder();
	        builder.Append("\tFormat tag:                        ");
	        builder.Append(this.FormatTag);
	        builder.AppendLine(String.Empty);
	        builder.Append("\tNumber of Channels:                ");
	        builder.Append(this.NumberChannels);
	        builder.AppendLine(String.Empty);
	        builder.Append("\tSamples per Second:                ");
	        builder.Append(this.SamplesPerSec);
	        builder.AppendLine(String.Empty);
	        builder.Append("\tAverage # of Bytes per Second:     ");
	        builder.Append(this.AverageBytesPerSec);
	        builder.AppendLine(String.Empty);
	        builder.Append("\tBlock Alignment:                   ");
	        builder.Append(this.BlockAlignment);
	        builder.AppendLine(String.Empty);
	        builder.Append("\tBits per Sample:                   ");
	        builder.Append(this.BitsPerSample);
	        builder.AppendLine(String.Empty);
	        builder.Append("\tSize of extended data:             ");
	        builder.Append(this.Size);
	        builder.AppendLine(String.Empty);
	
	        return builder.ToString();
        }
        #endregion
    }
}