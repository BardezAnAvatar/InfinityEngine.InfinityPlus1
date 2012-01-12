using System;
using System.IO;
using System.Text;

using Bardez.Projects.ReusableCode;
using Bardez.Projects.Win32.Audio;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.ACM
{
    /// <summary>Interplay ACM file header</summary>
    /// <remarks>
    ///     When the definitions say that the bits are packed, they are read from right to left. That is, with a binary representation of 0x07, 0x01 read as a UInt16 is 0x0107; the first 4 bits would be 0x7.
    /// </remarks>
    public class AcmHeader : IWaveFormatEx
    {
        #region Members
        /// <summary>Leading four bytes of the header file</summary>
        /// <remarks>
        ///     Read as two separate reads, one the first three bytes, the second the fourth.
        ///     <see cref="http://falloutmods.wikia.com/wiki/ACM_File_Format" />
        /// </remarks>
        protected UInt32 signature;

        /// <summary>Number of samples in this format</summary>
        protected UInt32 samplesCount;

        /// <summary>Number of channels</summary>
        /// <remarks>Supposedly bogus; does this mean intended as output (i.e.: mono, outputting stereo)?</remarks>
        protected UInt16 channelsCount;

        /// <summary>Number of samples per second</summary>
        protected UInt16 sampleRate;

        /// <summary>Packing attributes of this ACM file</summary>
        protected UInt16 packingAttributes;
        #endregion

        #region Properties
        /// <summary>Leading four bytes of the header file</summary>
        /// <remarks>Read as two separate reads, one the first three bytes, the second the fourth</remarks>
        /// <see cref="http://falloutmods.wikia.com/wiki/ACM_File_Format" />
        public UInt32 Signature
        {
            get { return this.signature; }
            set { this.signature = value; }
        }

        /// <summary>Leading three bytes of the header file</summary>
        /// <see cref="http://falloutmods.wikia.com/wiki/ACM_File_Format" />
        public UInt32 SignatureACM
        {
            get { return this.signature & 0x00FFFFFF; }
            set
            {
                UInt32 val = value & 0x00FFFFFF;
                UInt32 existing = this.signature & 0xFF000000;
                this.signature = val | existing;
            }
        }

        /// <summary>Leading three bytes of the header file</summary>
        /// <see cref="http://falloutmods.wikia.com/wiki/ACM_File_Format" />
        public UInt32 VersionACM
        {
            get { return (this.signature & 0xFF000000) >> 24; }
            set
            {
                UInt32 val = (value & 0x000000FF) << 24;
                UInt32 existing = this.signature & 0x00FFFFFF;
                this.signature = val | existing;
            }
        }

        /// <summary>Number of samples in this format</summary>
        public UInt32 SamplesCount
        {
            get { return this.samplesCount; }
            set { this.samplesCount = value; }
        }

        /// <summary>Number of channels</summary>
        /// <remarks>
        ///     Supposedly bogus; does this mean intended as output (i.e.: mono, outputting stereo)?
        ///     Also, BerliOS seems to default to 2 channels if mono, oddly.
        /// </remarks>
        public UInt16 ChannelsCount
        {
            get
            {
                //return this.channelsCount;
                return unchecked((UInt16)((this.channelsCount < 2U) ? 2U : this.channelsCount));
            }
            set { this.channelsCount = value; }
        }

        /// <summary>Number of samples per second</summary>
        public UInt16 SampleRate
        {
            get { return this.sampleRate; }
            set { this.sampleRate = value; }
        }

        /// <summary>Packing attributes of this ACM file</summary>
        public UInt16 PackingAttributes
        {
            get { return this.packingAttributes; }
            set { this.packingAttributes = value; }
        }

        /// <summary>Exponent packing attribute of this ACM file</summary>
        /// <see cref="http://falloutmods.wikia.com/wiki/ACM_File_Format" />
        /// <remarks>When this value is 0, no unpacking actually occurs.</remarks>
        public UInt16 PackingLevels
        {
            get { return Convert.ToUInt16(this.packingAttributes & 0x000F); }
            set
            {
                Int32 existing = 0xFFF0 & this.packingAttributes;
                Int32 val = value & 0x000F;
                this.packingAttributes = Convert.ToUInt16(val | existing);
            }
        }

        /// <summary>Exponent packing attribute of this ACM file</summary>
        /// <see cref="http://falloutmods.wikia.com/wiki/ACM_File_Format" />
        /// <remarks>When this value is 0, no unpacking actually occurs.</remarks>
        public UInt32 PackingColumns
        {
            get
            {
                Int32 val = this.packingAttributes & 0x000F;
                return 1U << val;
            }
            set
            {
                Int32 shift = 0;

                //get the bitval
                for (shift = 0; shift < 32; ++shift)
                {
                    UInt32 shifted = (value >> shift);
                    if ((shifted & 1U) == 1U && shifted > 1)
                        throw new ArgumentOutOfRangeException(String.Format("'value' is not a power of 2: {0}", value));
                    else if ((shifted & 1U) == 1U)
                        break;
                }

                if (shift > 31)
                    throw new ArgumentOutOfRangeException(String.Format("'value' cannot be 0: {0}", value));

                this.PackingLevels = Convert.ToUInt16(shift);
            }
        }

        /// <summary>Base packing size attribute of this ACM file</summary>
        /// <see cref="http://falloutmods.wikia.com/wiki/ACM_File_Format" />
        public UInt16 PackingRows
        {
            get { return Convert.ToUInt16(this.packingAttributes >> 4); }
            set
            {
                Int32 existing = 0x000F & this.packingAttributes;
                Int32 val = value << 4;
                this.packingAttributes = Convert.ToUInt16(val | existing);
            }
        }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public AcmHeader() { }

        /// <summary>Definition constructor</summary>
        public AcmHeader(UInt32 sig, UInt32 samples, UInt16 channels, UInt16 frequency, UInt16 packing)
        {
            this.signature = sig;
            this.samplesCount = samples;
            this.channelsCount = channels;
            this.sampleRate = frequency;
            this.packingAttributes = packing;
        }

        /// <summary>Extended definition constructor</summary>
        public AcmHeader(UInt32 sig, Byte version, UInt32 samples, UInt16 channels, UInt16 frequency, UInt16 multiple, UInt16 size)
        {
            this.SignatureACM = sig;
            this.VersionACM = version;
            this.samplesCount = samples;
            this.channelsCount = channels;
            this.sampleRate = frequency;
            this.PackingColumns = multiple;
            this.PackingRows = size;
        }
        #endregion

        #region IO
        /// <summary>Read method, reads ACM header from the input stream</summary>
        /// <param name="input">Stream to read the header from</param>
        public void Read(Stream input)
        {
            Byte[] header = ReusableIO.BinaryRead(input, 14);

            this.signature = ReusableIO.ReadUInt32FromArray(header, 0);
            this.samplesCount = ReusableIO.ReadUInt32FromArray(header, 4);
            this.channelsCount = ReusableIO.ReadUInt16FromArray(header, 8);
            this.sampleRate = ReusableIO.ReadUInt16FromArray(header, 10);
            this.packingAttributes = ReusableIO.ReadUInt16FromArray(header, 12);
        }

        /// <summary>Write method, writes the ACM header to the output stream</summary>
        /// <param name="output">Stream to write the header to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteUInt32ToStream(this.signature, output);
            ReusableIO.WriteUInt32ToStream(this.samplesCount, output);
            ReusableIO.WriteUInt16ToStream(this.channelsCount, output);
            ReusableIO.WriteUInt16ToStream(this.sampleRate, output);
            ReusableIO.WriteUInt16ToStream(this.packingAttributes, output);
        }
        #endregion

        #region ToString
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Interplay ACM header:");
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(this.signature);
            builder.Append(StringFormat.ToStringAlignment("Signature (3 byte)"));
            builder.Append(this.SignatureACM);
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(this.VersionACM);
            builder.Append(StringFormat.ToStringAlignment("Sample count"));
            builder.Append(this.samplesCount);
            builder.Append(StringFormat.ToStringAlignment("(Output) Channels"));
            builder.Append(this.channelsCount);
            builder.Append(StringFormat.ToStringAlignment("Sample rate"));
            builder.Append(this.sampleRate);
            builder.Append(StringFormat.ToStringAlignment("Packing attributes (4 byte)"));
            builder.Append(this.packingAttributes);
            builder.Append(StringFormat.ToStringAlignment("Packing Columns (4 bit)"));
            builder.Append(this.PackingColumns);
            builder.Append(StringFormat.ToStringAlignment("Packing Rows (12 bit)"));
            builder.Append(this.PackingRows);

            return builder.ToString();
        }
        #endregion

        /// <summary>Returns a WaveFormatEx instance from this header data</summary>
        /// <returns>A WaveFormatEx instance to submit to API calls</returns>
        public virtual WaveFormatEx GetWaveFormat()
        {
            WaveFormatEx waveEx = new WaveFormatEx();

            waveEx.AverageBytesPerSec = this.SampleRate * 2U /*sizeof(short)*/ * this.ChannelsCount /* sizeof(usort) */;
            waveEx.BitsPerSample = 16; /* sizeof(short) */
            waveEx.BlockAlignment = Convert.ToUInt16(2U * this.ChannelsCount);
            waveEx.FormatTag = 1;   //1 for PCM
            waveEx.NumberChannels = this.ChannelsCount; //designating 1 causes errors
            waveEx.SamplesPerSec = this.SampleRate;
            waveEx.Size = 0;    //no extra data; this is strictly a WaveFormatEx instance 

            return waveEx;
        }
    }
}