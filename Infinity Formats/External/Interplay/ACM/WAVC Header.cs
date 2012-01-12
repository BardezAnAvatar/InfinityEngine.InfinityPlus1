using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.ACM
{
    /// <summary>Represents the header of the WAVC file</summary>
    public class WavCHeader
    {
        #region Fields & Members
        /// <summary>File signature</summary>
        public String Signature { get; set; }

        /// <summary>File version</summary>
        public String Version { get; set; }

        /// <summary>Uncompressed size of the ACM file</summary>
        /// <remarks>Is this raw PCM bytes, or inclusive of header?</remarks>
        public UInt32 SizeUncompressed { get; set; }

        /// <summary>Compresed ACM file dize</summary>
        public UInt32 SizeCompressed { get; set; }

        /// <summary>Offset to theACM file</summary>
        public UInt32 OffsetAcm { get; set; }

        /// <summary>Count of channels</summary>
        /// <remarks>This channel countis actualy trustworthy, as opposed to actual ACM</remarks>
        public UInt16 Channels { get; set; }

        /// <summary>Count of bits per sample</summary>
        /// <value>Usually 16</value>
        public UInt16 BitsPerSample { get; set; }

        /// <summary>Number of samples per second</summary>
        public UInt32 Frequency { get; set; }
        #endregion
        
        #region Construction
        /// <summary>Default constructor</summary>
        public WavCHeader() { }

        /// <summary>Definition constructor</summary>
        /// <param name="signature">Signature string, usually "WAVC"</param>
        /// <param name="version">Version string, usually "V1.0"</param>
        /// <param name="uncompressedSize">Raw PCM sample size in bytes</param>
        /// <param name="compressedSize">Compressed ACM file size</param>
        /// <param name="acmOffset">Offset frm start of file to ACM subfile</param>
        /// <param name="channels">Reliable count of channels</param>
        /// <param name="sampleBitSize">Cont of bits per sample</param>
        /// <param name="frequency">Number of samples per second</param>
        public WavCHeader(String signature, String version, UInt32 uncompressedSize, UInt32 compressedSize, UInt32 acmOffset,  UInt16 channels, UInt16 sampleBitSize, UInt16 frequency)
        {
            this.Signature = signature;
            this.Version = version;
            this.SizeUncompressed = uncompressedSize;
            this.SizeCompressed = compressedSize;
            this.OffsetAcm = acmOffset;
            this.Channels = channels;
            this.BitsPerSample = sampleBitSize;
            this.Frequency = frequency;
        }
        #endregion

        #region IO
        /// <summary>Read method, reads ACM header from the input stream</summary>
        /// <param name="input">Stream to read the header from</param>
        public void Read(Stream input)
        {
            Byte[] header = ReusableIO.BinaryRead(input, 28);

            this.Signature = ReusableIO.ReadStringFromByteArray(header, 0, CultureConstants.CultureCodeEnglish, 4);
            this.Version = ReusableIO.ReadStringFromByteArray(header, 4, CultureConstants.CultureCodeEnglish, 4);
            this.SizeUncompressed = ReusableIO.ReadUInt32FromArray(header, 8);
            this.SizeCompressed = ReusableIO.ReadUInt32FromArray(header, 12);
            this.OffsetAcm = ReusableIO.ReadUInt32FromArray(header, 16);
            this.Channels = ReusableIO.ReadUInt16FromArray(header, 20);
            this.BitsPerSample = ReusableIO.ReadUInt16FromArray(header, 22);
            this.Frequency = ReusableIO.ReadUInt32FromArray(header, 24);
        }

        /// <summary>Write method, writes the ACM header to the output stream</summary>
        /// <param name="output">Stream to write the header to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.Signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.Version, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteUInt32ToStream(this.SizeUncompressed, output);
            ReusableIO.WriteUInt32ToStream(this.SizeCompressed, output);
            ReusableIO.WriteUInt32ToStream(this.OffsetAcm, output);
            ReusableIO.WriteUInt16ToStream(this.Channels, output);
            ReusableIO.WriteUInt16ToStream(this.BitsPerSample, output);
            ReusableIO.WriteUInt32ToStream(this.Frequency, output);
        }
        #endregion

        #region ToString
        /// <summary>Overridden ToString() implementation</summary>
        /// <returns>A String representation of this WAVC Header</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Interplay ACM header:");
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(this.Signature);
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(this.Version);
            builder.Append(StringFormat.ToStringAlignment("Size (Uncompressed)"));
            builder.Append(this.SizeUncompressed);
            builder.Append(StringFormat.ToStringAlignment("Size (Compressed)"));
            builder.Append(this.SizeCompressed);
            builder.Append(StringFormat.ToStringAlignment("ACM Offset"));
            builder.Append(this.OffsetAcm);
            builder.Append(StringFormat.ToStringAlignment("Channels"));
            builder.Append(this.Channels);
            builder.Append(StringFormat.ToStringAlignment("Sample size (bits)"));
            builder.Append(this.BitsPerSample);
            builder.Append(StringFormat.ToStringAlignment("Sample rate"));
            builder.Append(this.Frequency);

            return builder.ToString();
        }
        #endregion
    }
}