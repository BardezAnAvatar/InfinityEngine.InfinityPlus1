using System;
using System.Text;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.BasicStructures.Win32.Audio
{
    /// <summary>Managed representation of Win32 waveformat_tag structure</summary>
    public class PcmWaveFormat : WaveFormat
    {
        #region Fields
        /// <summary>Size in bits of a single per-channel sample</summary>
        /// <remarks>16 bit is optimal for XAudio2, 32-bit float following, then other formats.</remarks>
        public UInt16 BitsPerSample { get; set; }
        #endregion
        

        #region Construction
        /// <summary>Default constructor</summary>
        public PcmWaveFormat() : base()
        {
            if (this.NumberChannels > 0)
                this.BitsPerSample = Convert.ToUInt16((this.AverageBytesPerSec / this.NumberChannels) * 8U);
            else
                this.BitsPerSample = 0;
        }

        /// <summary>Definition constructor</summary>
        /// <param name="format">Integer identifier of the format</param>
        /// <param name="channels">Number of audio channels</param>
        /// <param name="sampleRate">Audio sample rate</param>
        /// <param name="avgBytesPerSec">Bytes per second (possibly approximate)</param>
        /// <param name="alignment">Size in bytes of a sample block (all channels)</param>
        /// <param name="bitsPerSample">Size in bits of a single channel's sample</param>
        public PcmWaveFormat(UInt16 format, UInt16 channels, UInt32 sampleRate, UInt32 avgBytesPerSec, UInt16 alignment, UInt16 bitsPerSample)
            : base(format, channels, sampleRate, avgBytesPerSec, alignment)
        {
            this.BitsPerSample = bitsPerSample;
        }

        /// <summary>Derivation constructor</summary>
        /// <param name="format">More general WaveFormat structure to derive this instance from</param>
        public PcmWaveFormat(WaveFormat format)
            : base(format.FormatTag, format.NumberChannels, format.SamplesPerSec, format.AverageBytesPerSec, format.BlockAlignment)
        {
            if (format is PcmWaveFormat)
                this.BitsPerSample = (format as PcmWaveFormat).BitsPerSample;
            else if (this.NumberChannels > 0)
                this.BitsPerSample = Convert.ToUInt16((this.AverageBytesPerSec / this.NumberChannels) * 8U);
            else
                this.BitsPerSample = 0;
        }

        /// <summary>Derivation constructor</summary>
        /// <param name="format">More general WaveFormat structure to derive this instance from</param>
        /// <param name="bitsPerSample">Size in bits of a single channel's sample</param>
        public PcmWaveFormat(WaveFormat format, UInt16 bitsPerSample)
            : base(format.FormatTag, format.NumberChannels, format.SamplesPerSec, format.AverageBytesPerSec, format.BlockAlignment)
        {
            this.BitsPerSample = bitsPerSample;
        }
        #endregion


        #region ToString() methods
        /// <summary>This method prints a human-readable representation to the given StringBuilder</summary>
        /// <param name="builder">StringBuilder to write to</param>
        public override void WriteString(StringBuilder builder)
        {
            base.WriteString(builder);

            StringFormat.ToStringAlignment("Bits Per Sample", builder);
            builder.Append(this.BitsPerSample);
        }
        #endregion
    }
}