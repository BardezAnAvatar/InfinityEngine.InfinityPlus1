using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Wave;
using Bardez.Projects.BasicStructures.Win32.Audio;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component
{
    public class RiffFile
    {
        /// <summary>Main Riff chunk</summary>
        public RiffThroneChunk Header { get; set; }

        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public void Read(Stream input)
        {
            this.Header = new RiffThroneChunk(input);
            this.Header.ReadChunkHeader();
            this.Header.Read();
        }

        /// <summary>Returns a Win32 API WaveFormatEx object for submission to Windows APIs</summary>
        /// <returns>A Win32 API WaveFormatEx object with data from this RiffFile</returns>
        public WaveFormatEx GetWaveFormat()
        {
            WaveFormatEx waveEx = new WaveFormatEx();
            //WaveFormatExtensible waveExtensible = new WaveFormatExtensible();

            WaveFormatChunk format = (this.Header.FindFirstSubChunk(ChunkType.fmt).Chunk as WaveFormatChunk);
            format.Read();

            waveEx.AverageBytesPerSec = format.ByteRate;
            waveEx.BitsPerSample = format.BitsPerSample;
            waveEx.BlockAlignment = format.BlockAlignment;
            waveEx.FormatTag = (UInt16)format.DataType;
            waveEx.NumberChannels = format.NumChannels;
            waveEx.SamplesPerSec = format.SampleRate;
            waveEx.Size = 0; //Convert.ToUInt16(format.Size);

            return waveEx;
        }

        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(this.GetWaveFormat().ToDescriptionString());
            builder.Append(StringFormat.ToStringAlignment("Sample Data Length"));
            builder.Append(this.Header.Data.Length);

            return builder.ToString();
        }
    }
}