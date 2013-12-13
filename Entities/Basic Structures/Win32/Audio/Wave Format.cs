using System;
using System.Text;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.BasicStructures.Win32.Audio
{
    /// <summary>Managed representation of Win32 waveformat_tag structure</summary>
    public class WaveFormat
    {
        #region Fields
        /// <summary>Integer identifier of the format</summary>
        public UInt16 FormatTag { get; set; }

        /// <summary>Number of audio channels</summary>
        public UInt16 NumberChannels { get; set; }

        /// <summary>Audio sample rate</summary>
        public UInt32 SamplesPerSec { get; set; }

        /// <summary>Bytes per second (possibly approximate)</summary>
        public UInt32 AverageBytesPerSec { get; set; }

        /// <summary>Size in bytes of a sample block (all channels)</summary>
        public UInt16 BlockAlignment { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes inferred channel mask based on the channel count.</summary>
        /// <remarks>A data field later down the inheritance hierarchy has an actual data slot for this</remarks>
        public virtual SpeakerPositions ChannelMask
        {
            get
            {
                SpeakerPositions pos = BasicStructures.Win32.Audio.SpeakerPositions.SPEAKER_ALL;

                if (this.NumberChannels == 1)
                    pos = BasicStructures.Win32.Audio.SpeakerPositions.SPEAKER_FRONT_CENTER;
                else if (this.NumberChannels == 2)
                    pos = BasicStructures.Win32.Audio.SpeakerPositions.SPEAKER_STEREO; //left & right
                else
                    pos = BasicStructures.Win32.Audio.SpeakerPositions.SPEAKER_ALL;

                return pos;
            }
            set
            {
                throw new NotSupportedException("No data storage exists for this property unless using the WaveFormatExtensible data structure.");
            }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public WaveFormat()
        {
            this.FormatTag = 1;             //1 means PCM, which is the most typical
            this.NumberChannels = 0;        //good default for invalid
            this.SamplesPerSec = 0;         //typically 11250 - 48000, but cannot be certain.
            this.AverageBytesPerSec = 0;
            this.BlockAlignment = 1;        //typically: blkalign = Number of channels * (Precision >> 3)
        }

        /// <summary>Definition constructor</summary>
        /// <param name="format">Integer identifier of the format</param>
        /// <param name="channels">Number of audio channels</param>
        /// <param name="sampleRate">Audio sample rate</param>
        /// <param name="avgBytesPerSec">Bytes per second (possibly approximate)</param>
        /// <param name="alignment">Size in bytes of a sample block (all channels)</param>
        public WaveFormat(UInt16 format, UInt16 channels, UInt32 sampleRate, UInt32 avgBytesPerSec, UInt16 alignment)
        {
            this.FormatTag = format;
            this.NumberChannels = channels;
            this.SamplesPerSec = sampleRate;
            this.AverageBytesPerSec = avgBytesPerSec;
            this.BlockAlignment = alignment;
        }
        #endregion


        #region ToString() methods
        /// <summary>Generates a descriptive string (to be displayed to the end user)</summary>
        /// <returns>A String representing the WaveFormatEx Object contents</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            this.WriteString(builder);

            return builder.ToString();
        }

        /// <summary>This method prints a human-readable representation to the given StringBuilder</summary>
        /// <param name="builder">StringBuilder to write to</param>
        public virtual void WriteString(StringBuilder builder)
        {
            StringFormat.ToStringAlignment("Format tag", builder);
            builder.Append(this.FormatTag);
            StringFormat.ToStringAlignment("Number of Channels", builder);
            builder.Append(this.NumberChannels);
            StringFormat.ToStringAlignment("Samples per Second", builder);
            builder.Append(this.SamplesPerSec);
            StringFormat.ToStringAlignment("Average # of Bytes per Second", builder);
            builder.Append(this.AverageBytesPerSec);
            StringFormat.ToStringAlignment("Block Alignment", builder);
            builder.Append(this.BlockAlignment);
        }
        #endregion
    }
}