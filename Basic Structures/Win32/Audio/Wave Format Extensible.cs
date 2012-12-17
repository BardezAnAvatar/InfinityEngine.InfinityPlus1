using System;
using System.Text;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.BasicStructures.Win32.Audio
{
    /// <summary>Managed representation of Win32 WAVEFORMATEXTENSIBLE structure, inheriting the WaveFormatEx structure</summary>
    public class WaveFormatExtensible : WaveFormatEx
    {
        #region Fields
        /// <summary>Union of three fields here</summary>
		protected UInt16 samples;
        #endregion


        #region Properties
		/// <summary>Union of three fields here: wValidBitsPerSample, wSamplesPerBlock, and wReserved</summary>
		public UInt16 Samples
		{
            get { return this.samples; }
			set { this.samples = value; }
		}
					
		/// <summary>Union of three fields here: wValidBitsPerSample, wSamplesPerBlock, and wReserved</summary>
		public UInt16 ValidBitsPerSample
		{
            get { return this.samples; }
			set { this.samples = value; }
		}
    
		/// <summary>Union of three fields here: wValidBitsPerSample, wSamplesPerBlock, and wReserved</summary>
		public UInt16 Reserved
		{
            get { return this.samples; }
			set { this.samples = value; }
		}

		/// <summary>Positions of the audio channels</summary>
        public SpeakerPositions ChannelMask { get; set; }

		/// <summary>Format identifier GUID</summary>
        public Guid SubFormat { get; set; }
        #endregion


		#region Construction
		/// <summary>Default constructor</summary>
		public WaveFormatExtensible() : base()
        {
            this.samples = this.BitsPerSample;                  //default assumption
            this.ChannelMask = SpeakerPositions.SPEAKER_ALL;    //default assumption
            this.SubFormat = Guid.Empty;
        }

        /// <summary>Definition constructor</summary>
        /// <param name="format">Integer identifier of the format</param>
        /// <param name="channels">Number of audio channels</param>
        /// <param name="sampleRate">Audio sample rate</param>
        /// <param name="avgBytesPerSec">Bytes per second (possibly approximate)</param>
        /// <param name="alignment">Size in bytes of a sample block (all channels)</param>
        /// <param name="bitsPerSample">Size in bits of a single channel's sample</param>
        /// <param name="size">Size (in Bytes) of data that follows past this structure, to indicate which sub-class to use</param>
        /// <param name="validBitsPerSample">Valid bits of precision (for 20-bit audio, for example)</param>
        /// <param name="channelMask">Which channels are present in the stream</param>
        /// <param name="subFormat">Guid defining a sub-format to use if additional information available</param>
        public WaveFormatExtensible(UInt16 format, UInt16 channels, UInt32 sampleRate, UInt32 avgBytesPerSec, UInt16 alignment, UInt16 bitsPerSample, UInt16 size, UInt16 validBitsPerSample, UInt32 channelMask, Guid subFormat)
            : this(format, channels, sampleRate, avgBytesPerSec, alignment, bitsPerSample, size, validBitsPerSample, (SpeakerPositions)channelMask, subFormat) { }

        /// <summary>Definition constructor</summary>
        /// <param name="format">Integer identifier of the format</param>
        /// <param name="channels">Number of audio channels</param>
        /// <param name="sampleRate">Audio sample rate</param>
        /// <param name="avgBytesPerSec">Bytes per second (possibly approximate)</param>
        /// <param name="alignment">Size in bytes of a sample block (all channels)</param>
        /// <param name="bitsPerSample">Size in bits of a single channel's sample</param>
        /// <param name="size">Size (in Bytes) of data that follows past this structure, to indicate which sub-class to use</param>
        /// <param name="validBitsPerSample">Valid bits of precision (for 20-bit audio, for example)</param>
        /// <param name="channelMask">Which channels are present in the stream</param>
        /// <param name="subFormat">Guid defining a sub-format to use if additional information available</param>
        public WaveFormatExtensible(UInt16 format, UInt16 channels, UInt32 sampleRate, UInt32 avgBytesPerSec, UInt16 alignment, UInt16 bitsPerSample, UInt16 size, UInt16 validBitsPerSample, SpeakerPositions channelMask, Guid subFormat)
            : base(format, channels, sampleRate, avgBytesPerSec, alignment, bitsPerSample, size)
        {
            this.Reserved = validBitsPerSample;
            this.ChannelMask = channelMask;
            this.SubFormat = subFormat;
        }

        /// <summary>Derivation constructor</summary>
        /// <param name="format">More general WaveFormat structure to derive this instance from</param>
        public WaveFormatExtensible(WaveFormat format) : base(format)
        {
            if (format is WaveFormatExtensible)
            {
                WaveFormatExtensible fmt = format as WaveFormatExtensible;

                this.Reserved = fmt.ValidBitsPerSample;
                this.ChannelMask = fmt.ChannelMask;
                this.SubFormat = fmt.SubFormat;
            }
            else
            {
                this.samples = this.BitsPerSample;                  //default assumption
                this.ChannelMask = SpeakerPositions.SPEAKER_ALL;    //default assumption
                this.SubFormat = Guid.Empty;
            }
        }

        /// <summary>Derivation constructor</summary>
        /// <param name="format">More general WaveFormat structure to derive this instance from</param>
        /// <param name="validBitsPerSample">Valid bits of precision (for 20-bit audio, for example)</param>
        /// <param name="channelMask">Which channels are present in the stream</param>
        /// <param name="subFormat">Guid defining a sub-format to use if additional information available</param>
        public WaveFormatExtensible(WaveFormatEx format, UInt16 validBitsPerSample, SpeakerPositions channelMask, Guid subFormat) : base(format)
        {
            this.Reserved = validBitsPerSample;
            this.ChannelMask = channelMask;
            this.SubFormat = subFormat;
        }
        #endregion


        #region ToString() methods
        /// <summary>This method prints a human-readable representation to the given StringBuilder</summary>
        /// <param name="builder">StringBuilder to write to</param>
        public override void WriteString(StringBuilder builder)
        {
            base.WriteString(builder);

            StringFormat.ToStringAlignment("Number of samples", builder);
            builder.Append(this.Samples);
            StringFormat.ToStringAlignment("Audio channel position(s)", builder);
            builder.Append(this.ChannelMask);
            StringFormat.ToStringAlignment("Format GUID identifier", builder);
            builder.Append(this.SubFormat.ToString());
        }
        #endregion

    }
}