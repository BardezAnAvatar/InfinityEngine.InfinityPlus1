using System;
using System.Text;

namespace Bardez.Projects.BasicStructures.Win32.Audio
{
    /// <summary>Managed representation of Win32 WAVEFORMATEX structure</summary>
    public class WaveFormatEx
    {
	    #region Members
		/// <summary>Integer identifier of the format</summary>
		protected UInt16 formatTag;

		/// <summary>Number of audio channels</summary>
		protected UInt16 numberChannels;

		/// <summary>Audio sample rate</summary>
		protected UInt32 samplesPerSec;

		/// <summary>Bytes per second (possibly approximate)</summary>
		protected UInt32 averageBytesPerSec;

		/// <summary>Size in bytes of a sample block (all channels)</summary>
		protected UInt16 blockAlignment;

		/// <summary>Size in bits of a single per-channel sample</summary>
		/// <remarks>16 bit is optimal for XAudio2, 32 bit following, then other formats.</remarks>
		protected UInt16 bitsPerSample;

		/// <summary>
		///		Bytes of extra data appended to this struct. Explains the length of bytes in addition to this struct,
		///		essentially the extra size of child instances.
		/// </summary>
		protected UInt16 size;
	    #endregion


	    #region Properties
		/// <summary>Integer identifier of the format</summary>
		public UInt16 FormatTag
		{
            get { return this.formatTag; }
            set { this.formatTag = value; }
		}

		/// <summary>Number of audio channels</summary>
		public UInt16 NumberChannels
		{
            get { return this.numberChannels; }
            set { this.numberChannels = value; }
		}

		/// <summary>Audio sample rate</summary>
		public UInt32 SamplesPerSec
		{
            get { return this.samplesPerSec; }
            set { this.samplesPerSec = value; }
		}

		/// <summary>Bytes per second (possibly approximate)</summary>
		public UInt32 AverageBytesPerSec
		{
            get { return this.averageBytesPerSec; }
            set { this.averageBytesPerSec = value; }
		}

		/// <summary>Size in bytes of a sample block (all channels)</summary>
		public UInt16 BlockAlignment
		{
            get { return this.blockAlignment; }
            set { this.blockAlignment = value; }
		}

		/// <summary>Size in bits of a single per-channel sample</summary>
		/// <remarks>16 bit is optimal for XAudio2, 32 bit following, then other formats.</remarks>
		public UInt16 BitsPerSample
		{
            get { return this.bitsPerSample; }
            set { this.bitsPerSample = value; }
		}

		/// <summary>Bytes of extra data appended to this struct</summary>
		public UInt16 Size
        {
            get { return this.size; }
            set { this.size = value; }
		}
	    #endregion


	    #region Construction
	    /// <summary>Default constructor</summary>
	    public WaveFormatEx() { }
	    #endregion


	    #region Methods
	    /// <summary>Generates a descriptive string (to be displayed to the end user)</summary>
	    /// <returns>A String representing the WaveFormatEx Object contents</returns>
        public virtual String ToDescriptionString()
        {
	        String desc = null;

	        StringBuilder builder = new StringBuilder();
	        builder.Append("\tFormat tag:                        ");
	        builder.Append(this.formatTag);
	        builder.AppendLine(String.Empty);
	        builder.Append("\tNumber of Channels:                ");
	        builder.Append(this.numberChannels);
	        builder.AppendLine(String.Empty);
	        builder.Append("\tSamples per Second:                ");
	        builder.Append(this.samplesPerSec);
	        builder.AppendLine(String.Empty);
	        builder.Append("\tAverage # of Bytes per Second:     ");
	        builder.Append(this.averageBytesPerSec);
	        builder.AppendLine(String.Empty);
	        builder.Append("\tBlock Alignment:                   ");
	        builder.Append(this.blockAlignment);
	        builder.AppendLine(String.Empty);
	        builder.Append("\tBits per Sample:                   ");
	        builder.Append(this.bitsPerSample);
	        builder.AppendLine(String.Empty);
	        builder.Append("\tSize of extended data:             ");
	        builder.Append(this.size);
	        builder.AppendLine(String.Empty);

	        desc = builder.ToString();
	
	        return desc;
        }
	    #endregion
    }
}
