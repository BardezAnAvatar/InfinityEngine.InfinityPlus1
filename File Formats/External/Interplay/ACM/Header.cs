using System;
using System.IO;
using System.Text;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.External.Interplay.ACM
{
    /// <summary>Interplay ACM file header</summary>
    public class Header
    {
        #region Members
        /// <summary>Leading four bytes of the header file</summary>
        /// <remarks>Read as two separate reads, one the first three bytes, the second the fourth</remarks>
        /// <see cref="http://falloutmods.wikia.com/wiki/ACM_File_Format" />
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
            get { return this.signature >> 8; }
            set
            {
                UInt32 val = value << 8;
                UInt32 existing = this.signature & 0x000000FF;
                this.signature = val | existing;
            }
        }

        /// <summary>Leading three bytes of the header file</summary>
        /// <see cref="http://falloutmods.wikia.com/wiki/ACM_File_Format" />
        public UInt32 VersionACM
        {
            get { return this.signature & 0x000000FF; }
            set
            {
                UInt32 val = value & 0xFFFFFF00;
                UInt32 existing = this.signature & 0xFFFFFF00;
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
        /// <remarks>Supposedly bogus; does this mean intended as output (i.e.: mono, outputting stereo)?</remarks>
        public UInt16 ChannelsCount
        {
            get { return this.channelsCount; }
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
        public UInt16 PackingAttributesValue1
        {
            get { return Convert.ToUInt16(this.packingAttributes >> 12); }
            set
            {
                Int32 existing = 0x0FFF & this.packingAttributes;
                Int32 val = value << 12;
                this.packingAttributes = Convert.ToUInt16(val | existing);
            }
        }

        /// <summary>Base packing size attribute of this ACM file</summary>
        /// <see cref="http://falloutmods.wikia.com/wiki/ACM_File_Format" />
        public UInt16 PackingAttributesValue2
        {
            get { return Convert.ToUInt16(this.packingAttributes & 0x0FFF); }
            set
            {
                Int32 existing = 0xF000 & this.packingAttributes;
                Int32 val = value & 0x0FFF;
                this.packingAttributes = Convert.ToUInt16(val | existing);
            }
        }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public Header() { }

        /// <summary>Definition constructor</summary>
        public Header(UInt32 sig, UInt32 samples, UInt16 channels, UInt16 frequency, UInt16 packing)
        {
            this.signature = sig;
            this.samplesCount = samples;
            this.channelsCount = channels;
            this.sampleRate = frequency;
            this.packingAttributes = packing;
        }

        /// <summary>Extended definition constructor</summary>
        public Header(UInt32 sig, Byte version, UInt32 samples, UInt16 channels, UInt16 frequency, UInt16 multiple, UInt16 size)
        {
            this.SignatureACM = sig;
            this.VersionACM = version;
            this.samplesCount = samples;
            this.channelsCount = channels;
            this.sampleRate = frequency;
            this.PackingAttributesValue1 = multiple;
            this.PackingAttributesValue2 = size;
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
            builder.AppendLine("Interplay ACM header:");
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(this.signature);
            builder.Append(StringFormat.ToStringAlignment("Signature (3 byte)"));
            builder.Append(this.SignatureACM);
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(this.VersionACM);
            builder.Append(StringFormat.ToStringAlignment("Sample count"));
            builder.Append(this.samplesCount);
            builder.Append(StringFormat.ToStringAlignment("Channels"));
            builder.Append(this.channelsCount);
            builder.Append(StringFormat.ToStringAlignment("Sample rate"));
            builder.Append(this.sampleRate);
            builder.Append(StringFormat.ToStringAlignment("Packing attributes (4 byte)"));
            builder.Append(this.packingAttributes);
            builder.Append(StringFormat.ToStringAlignment("Packing multiplier (4 bit)"));
            builder.Append(this.PackingAttributesValue1);
            builder.Append(StringFormat.ToStringAlignment("Packing base size (12 bit)"));
            builder.Append(this.PackingAttributesValue2);
            builder.Append("\n\n");

            return builder.ToString();
        }
        #endregion
    }
}