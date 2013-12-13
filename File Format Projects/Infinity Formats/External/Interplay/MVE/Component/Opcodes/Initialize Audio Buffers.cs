using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes
{
    /// <summary>Represents an Intialize Audio Buffers opcode</summary>
    public abstract class InitializeAudioBuffers : OpcodeData
    {
        #region Fields
        /// <summary>Represents the unknown leading two bytes</summary>
        public UInt16 Unknown { get; set; }

        /// <summary>Represents the flags of the audio buffers</summary>
        public UInt16 Flags { get; set; }

        /// <summary>Represents  the sample rate of the audio buffer</summary>
        public UInt16 SampleRate { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the intended buffer length</summary>
        public abstract UInt32 BufferLength { get; }

        /// <summary>Exposes the number of channels in the audio stream</summary>
        public Int32 Channels
        {
            get
            {
                Int32 channelFlag = (this.Flags & 0x01);
                return channelFlag + 1;
            }
        }

        /// <summary>Exposes the size of samples in the audio stream</summary>
        public Int32 SampleSize
        {
            get
            {
                Int32 sampleFlag = (this.Flags & 0x02);
                return (sampleFlag * 4) + 8;   //(0 + 8) or (2 * 4) + 8
            }
        }

        /// <summary>Exposes the size of samples in the audio stream</summary>
        public abstract Boolean Compressed { get; }
        #endregion


        #region ToString() methods
        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the contents.</returns>
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Unknown"));
            builder.Append(this.Unknown);
            builder.Append(StringFormat.ToStringAlignment("Flags"));
            builder.Append(this.Flags);
            builder.Append(this.GetFlagStringRepresentation());
            builder.Append(StringFormat.ToStringAlignment("Sample Rate"));
            builder.Append(this.SampleRate);
            builder.Append(StringFormat.ToStringAlignment("Buffer Length"));
            builder.Append(this.BufferLength);

            return builder.ToString();
        }

        /// <summary>This gets a human-readable representation of the flags.</summary>
        /// <returns>A string, formatted largely for console, that describes the flags' values.</returns>
        protected virtual String GetFlagStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Channels"));
            builder.Append(this.Channels);
            builder.Append(StringFormat.ToStringAlignment("Sample Size"));
            builder.Append(this.SampleSize);
            builder.Append(StringFormat.ToStringAlignment("Compressed"));
            builder.Append(this.Compressed);

            return builder.ToString();
        }
        #endregion


        #region Equality
        /// <summary>Overridden (value) equality method</summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>Boolean indicating equality</returns>
        public override Boolean Equals(Object obj)
        {
            Boolean equal = false;  //assume the worst

            try
            {
                if (obj != null && obj is InitializeAudioBuffers)
                {
                    InitializeAudioBuffers compare = obj as InitializeAudioBuffers;

                    Boolean structureEquality = base.Equals(compare);
                    structureEquality &= (this.Unknown == compare.Unknown);
                    structureEquality &= (this.Flags == compare.Flags);
                    structureEquality &= (this.SampleRate == compare.SampleRate);
                    structureEquality &= (this.BufferLength == compare.BufferLength);

                    //offsets are unimportant when it comes to data value equivalence/equality
                    equal = structureEquality;
                }
            }
            catch { equal = false; }    //per MSDN, must not throw exceptions

            return equal;
        }

        /// <summary>Override of GetHashCode</summary>
        /// <returns>Computed hash</returns>
        public override Int32 GetHashCode()
        {
            Int32 hash = base.GetHashCode();
            hash ^= this.Unknown.GetHashCode();
            hash ^= this.Flags.GetHashCode();
            hash ^= this.SampleRate.GetHashCode();
            hash ^= this.BufferLength.GetHashCode();

            return hash;
        }
        #endregion
    }
}