using System;
using System.Text;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.BasicStructures.Win32.Audio
{
    /// <summary>Managed representation of Win32 WAVEFORMATEX structure</summary>
    public class WaveFormatEx : PcmWaveFormat
    {
	    #region Fields
		/// <summary>
		///		Size, in Bytes, of extra data appended to this struct.
        ///		Explains which struct to use if larger (WAVEFORMATEXTENSIBLE or so on)
		/// </summary>
        public UInt16 Size { get; set; }
	    #endregion


	    #region Construction
	    /// <summary>Default constructor</summary>
	    public WaveFormatEx() : base()
        {
            this.Size = 0;
        }

        /// <summary>Definition constructor</summary>
        /// <param name="format">Integer identifier of the format</param>
        /// <param name="channels">Number of audio channels</param>
        /// <param name="sampleRate">Audio sample rate</param>
        /// <param name="avgBytesPerSec">Bytes per second (possibly approximate)</param>
        /// <param name="alignment">Size in bytes of a sample block (all channels)</param>
        /// <param name="bitsPerSample">Size in bits of a single channel's sample</param>
        /// <param name="size">Size (in Bytes) of data that follows past this structure, to indicate which sub-class to use</param>
        public WaveFormatEx(UInt16 format, UInt16 channels, UInt32 sampleRate, UInt32 avgBytesPerSec, UInt16 alignment, UInt16 bitsPerSample, UInt16 size)
            : base(format, channels, sampleRate, avgBytesPerSec, alignment, bitsPerSample)
        {
            this.Size = size;
        }

        /// <summary>Derivation constructor</summary>
        /// <param name="format">More general WaveFormat structure to derive this instance from</param>
        public WaveFormatEx(WaveFormat format) : base (format)
        {
            if (format is WaveFormatEx)
                this.Size = (format as WaveFormatEx).Size;
            else
                this.Size = 0;
        }

        /// <summary>Derivation constructor</summary>
        /// <param name="format">More general WaveFormat structure to derive this instance from</param>
        /// <param name="size">Size (in Bytes) of data that follows past this structure, to indicate which sub-class to use</param>
        public WaveFormatEx(PcmWaveFormat format, UInt16 size) : base(format)
        {
            this.Size = size;
        }
	    #endregion


        #region ToString() methods
        /// <summary>This method prints a human-readable representation to the given StringBuilder</summary>
        /// <param name="builder">StringBuilder to write to</param>
        public override void WriteString(StringBuilder builder)
        {
            base.WriteString(builder);

            StringFormat.ToStringAlignment("Size of extended data", builder);
            builder.Append(this.Size);
        }
        #endregion
    }
}