using System;
using System.IO;
using System.Text;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component
{
    /// <summary>Represents a container for a single RIFF (Resource Interchange File Format) file</summary>
    /// <remarks>This should be a general RIFF file, not a specific one</remarks>
    public class RiffFile
    {
        /// <summary>Main Riff chunk</summary>
        public RiffThroneChunk Header { get; set; }

        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public void Read(Stream input)
        {
            RiffChunk header = RiffChunkFactory.CreateChunk(input);

            //Check that the first bits read was "RIFF"
            if ( !(header is RiffThroneChunk))
                throw new ApplicationException(String.Format("Expected to find a RiffThroneChunk type (for a RIFF fourCC) while expecting to read a RIFF file. Found \"{0}\" instead.", header== null ? "null" : header.GetType().Name));

            this.Header = header as RiffThroneChunk;
            this.Header.ReadChunkHeader();
            this.Header.Read();
        }

        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Sample Data Length"));
            builder.Append(this.Header.Data.Length);

            return builder.ToString();
        }
    }
}