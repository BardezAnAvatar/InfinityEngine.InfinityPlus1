﻿using System;
using System.IO;
using System.Text;

using Bardez.Projects.BasicStructures.Win32.Audio;
using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Wave
{
    /// <summary>
    ///     This class expects specifically to be a Wave RIFF file.
    ///     It will raise an error if this is not the case.
    /// </summary>
    public class WaveRiffFile : RiffFile
    {
        #region Construction
        /// <summary>PArtial definition constructor</summary>
        /// <param name="input">Stream of which to take ownership of</param>
        public WaveRiffFile(Stream input)
        {
        }
        #endregion

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
            builder.Append(base.ToString());

            return builder.ToString();
        }

        /// <summary>Gets binary sample data from Riff file</summary>
        /// <param name="riff">Riff file to read data from</param>
        /// <returns>Byte array of sample data</returns>
        public virtual Byte[] GetWaveData()
        {
            WaveSampleDataChunk wave = (this.Header.FindFirstSubChunk(ChunkType.data).Chunk as WaveSampleDataChunk);
            return wave.Data;
        }

        public static Boolean IsWaveFile(Stream dataStream)
        {
            return false;
        }
    }
}