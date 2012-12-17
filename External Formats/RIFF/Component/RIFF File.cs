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
        #region Fields
        /// <summary>Main Riff chunk</summary>
        public RiffSuperChunk RootChunk { get; set; }
        #endregion


        #region Properties
        /// <summary>Four character code indicating the type of RIFF content within this super-chunk</summary>
        public ChunkType ContainerType
        {
            get { return this.RootChunk.ContainerType; }
        }

        /// <summary>Four character code indicating the type of RIFF content within this super-chunk</summary>
        public String ContainerTypeName
        {
            get { return RiffChunk.GetFourCCName((UInt32)(this.ContainerType)); ; }
        }
        #endregion


        #region Read method(s)
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public void Read(Stream input)
        {
            IRiffChunk header = RiffChunkFactory.CreateChunk(input);

            //Check that the first bits read was "RIFF"
            if ( !(header is RiffSuperChunk))
                throw new ApplicationException(String.Format("Expected to find a RiffSuperChunk type (for a RIFF fourCC) while expecting to read a RIFF file. Found \"{0}\" instead.", header== null ? "null" : header.GetType().Name));

            this.RootChunk = header as RiffSuperChunk;
            this.RootChunk.ReadSubChunks();
        }
        #endregion


        #region ToString method(s)
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            this.WriteString(builder);

            return builder.ToString();
        }

        /// <summary>This method prints a human-readable representation to the given StringBuilder</summary>
        /// <param name="builder">StringBuilder to write to</param>
        public void WriteString(StringBuilder builder)
        {
            builder.Append("RIFF File:");
            StringFormat.ToStringAlignment("Chunks", builder);
            StringFormat.IndentAllLines(this.RootChunk.ToString(), 1, builder);
        }
        #endregion
    }
}