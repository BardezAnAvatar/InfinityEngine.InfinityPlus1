using System;
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
        /// <summary>Returns a Win32 API WaveFormatEx object for submission to Windows APIs</summary>
        /// <returns>A Win32 API WaveFormatEx object with data from this RiffFile</returns>
        public WaveFormatEx GetWaveFormat()
        {
            WaveFormatChunk format = (this.RootChunk.FindFirstSubChunk(ChunkType.fmt).Chunk as WaveFormatChunk);
            return format.GetWaveFormat();
        }

        /// <summary>Gets binary sample data from Riff file</summary>
        /// <returns>Byte array of sample data</returns>
        public virtual Byte[] GetWaveData()
        {
            IRiffChunk wave = this.RootChunk.FindFirstSubChunk(ChunkType.data).Chunk;
            return wave.Data;
        }

        #region ToString methods
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(this.GetWaveFormat().ToDescriptionString());
            builder.Append(base.ToString());

            return builder.ToString();
        }

        /// <summary>This method prints a human-readable representation to the given StringBuilder</summary>
        /// <param name="builder">StringBuilder to write to</param>
        public void WriteString(StringBuilder builder)
        {
            WaveFormatEx format = this.GetWaveFormat();
            builder
        }
        #endregion
    }
}